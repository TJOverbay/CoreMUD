using System;
using System.Collections.Concurrent;
using System.Threading;
using App.Common;

namespace CoreMUD.ECS.Pooling
{
    /// <summary>
    /// A Collection that maintains a set of Components that 
    /// can be recycled when no longer need to minimize the 
    /// effects of garbase collection.
    /// </summary>
    /// <typeparam name="T">
    /// The type of Component to store in the pool. Pools can 
    /// only hold <see cref="PoolableComponent"/> or subtypes.
    /// </typeparam>

    public class ComponentPool<T> : IComponentPool<T>
        where T : PoolableComponent
    {
        private readonly Func<Type, T> _allocate;
        private readonly Type _innerType;
        private ConcurrentBag<T> _invalidComponents;
        private T[] _components;
        private readonly object _lock = new object();
        private T _dummy;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentPool{T}"/> class.
        /// </summary>
        /// <param name="innerType">The innerType of the poolable component.</param>
        /// <param name="factoryFunc">A function used to allocate an instance for the pool.</param>
        /// <param name="initialSize">The initial size of the pool.</param>
        /// <param name="canResize">Whether or not the pool is allowed to increase its size as needed.</param>
        /// <param name="resizeIncrement">The resize pool increment.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="initialSize"/> is less than 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="resizeIncrement"/> is less than 1 and 
        /// <paramref name="canResize"/> is true.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="factoryFunc"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="innerType"/> is null.</exception>

        public ComponentPool(Type innerType,
                             Func<Type, T> factoryFunc,
                             int initialSize,
                             bool canResize = false,
                             int resizeIncrement = 0)
        {
            innerType.ThrowIfNull(nameof(innerType));
            factoryFunc.ThrowIfNull(nameof(factoryFunc));
            initialSize.ThrowIfLessThan(1, nameof(initialSize));
            if (canResize)
            {
                resizeIncrement.ThrowIfLessThan(1, nameof(resizeIncrement));
                ResizeIncrement = resizeIncrement;
            }
            else
            {
                ResizeIncrement = 0;
            }

            _invalidComponents = new ConcurrentBag<T>();
            _innerType = innerType;

            // Create our component array
            _components = new T[initialSize];
            InvalidCount = _components.Length;

            _allocate = factoryFunc;
        }

        /// <summary>Gets the number of invalid components in the pool.</summary>

        public int InvalidCount { get; private set; }

        /// <summary>
        /// Gets the increment size when resizing the pool.
        /// </summary>

        public int ResizeIncrement { get; private set; }

        /// <summary>Gets the number of valid components in the pool.</summary>

        public int ValidCount
        {
            get
            {
                lock (_lock)
                {
                    return _components.Length - InvalidCount;
                }
            }
        }

        /// <summary>Returns a valid component at the given index.</summary>
        /// <param name="index">The index of the component to return.</param>
        /// <returns>A valid component at the specified index.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Thrown if the index is outside the range [0, <see cref="ValidCount"/>].
        /// </exception>

        public T this[int index]
        {
            get
            {
                index += InvalidCount;
                index.ThrowIfNotInRangeInclusive(InvalidCount, _components.Length - 1, nameof(index),
                    $"The index must be at least 0 and less than ValidCount ({ValidCount})");

                lock (_lock)
                {
                    return _components[index];
                }
            }
        }

        /// <summary>
        /// Cleans up the pool by checking each valid component 
        /// to ensure it is still actually valid.
        /// <para>
        /// Must be called regularly to free returned Components.
        /// </para>
        /// </summary>

        public void CleanUp()
        {
            lock(_lock)
            {
                foreach (T component in _invalidComponents)
                {
                    if (component.PoolId != InvalidCount)
                    {
                        _components[component.PoolId] = _components[InvalidCount];
                        _components[InvalidCount].PoolId = component.PoolId;
                        _components[InvalidCount] = component;
                        component.PoolId = -1;
                    }

                    // clean up the component, if desired
                    component.CleanUp();
                    ++InvalidCount;
                }

                while (!_invalidComponents.IsEmpty)
                {
                    _invalidComponents.TryTake(out _dummy);
                }
            }
        }

        /// <summary>Returns a component back to the pool.</summary>
        /// <param name="component">The component to return.</param>

        public void ReturnObject(T component)
        {
            _invalidComponents.Add(component);
        }

        /// <summary>
        /// Attempts to retrieve a component from the pool.
        /// </summary>
        /// <param name="newComponent">The component that was retrieved or created.</param>
        /// <returns>
        /// True, if a component was successfully retrieved from the pool; Otherwise, false 
        /// if there were no available components in the pool and the pool was set to not 
        /// be resized.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the specified factory function returned a null instance.
        /// </exception>

        public bool TryNew(out T newComponent)
        {
            lock (_lock)
            {
                // If we're out of invalid instances...
                if (InvalidCount == 0)
                {
                    // Can't continue if we can't resize the pool
                    if (ResizeIncrement == 0)
                    {
                        newComponent = null;
                        return false;
                    }

                    ResizeComponentArray(ResizeIncrement);

                    // Move the invalid count based on our resize increment
                    InvalidCount += ResizeIncrement;
                }

                // Decrement the counter
                --InvalidCount;

                // Get the next component in the pool
                newComponent = _components[InvalidCount];

                // If the component is null, we need to allocate a new instance
                if (newComponent == null)
                {
                    newComponent = _allocate(_innerType);

                    if (newComponent == null)
                    {
                        throw new InvalidOperationException(
                            "The pool's factoy method returned a null object reference");
                    }

                    _components[InvalidCount] = newComponent;
                }

                newComponent.PoolId = InvalidCount;
            }

            // Initialize the object, if needed
            newComponent.Initialize();

            return true;
        }

        private void ResizeComponentArray(int resizeAmount)
        {
            T[] newComponents;

            lock (_lock)
            {
                // Create a new array with an incrmented size and copy over the existing components
                newComponents = new T[_components.Length + resizeAmount];

                for (int i = _components.Length - 1; i >= 0; --i)
                {
                    if (i >= InvalidCount)
                    {
                        _components[i].PoolId = i + resizeAmount;
                    }

                    newComponents[i + resizeAmount] = _components[i];
                }
            }

            Interlocked.Exchange(ref _components, newComponents);
        }
    }
}
