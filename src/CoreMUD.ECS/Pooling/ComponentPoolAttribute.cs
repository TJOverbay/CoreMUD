using System;

namespace CoreMUD.ECS.Pooling
{
    /// <summary>
    /// Attribute applied to a component to specify how the component 
    /// should be pooled
    /// </summary>

    [AttributeUsage(AttributeTargets.Class,
                    Inherited = false,
                    AllowMultiple =false)]
    public sealed class ComponentPoolAttribute : Attribute
    {
        /// <summary>
        /// Constructs a new <see cref="ComponentPoolAttribute"/> with 
        /// default values./>
        /// </summary>

        public ComponentPoolAttribute()
        {
            InitialSize = 10;
            ResizeIncrement = 10;
            IsResizable = true;
            IsThreadSafe = false;
        }

        /// <summary>
        /// Gets or sets the initial size of the pool. Default is 10.
        /// </summary>

        public int InitialSize { get; set; }

        /// <summary>
        /// Gets or sets whether the pool is resizable. Default is true.
        /// </summary>

        public bool IsResizable { get; set; }

        /// <summary>
        /// Gets or sets whether the pool supports multi-threading. Default is false.
        /// </summary>

        public bool IsThreadSafe { get; set; }

        /// <summary>
        /// Gets or sets the increment at which the pool will be resized. Default is 10.
        /// </summary>

        public int ResizeIncrement { get; set; }
    }
}
