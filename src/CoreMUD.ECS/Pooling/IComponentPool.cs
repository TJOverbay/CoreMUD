namespace CoreMUD.ECS.Pooling
{
    /// <summary>
    /// Interface for component pools.
    /// </summary>
    /// <typeparam name="T">The type of component to pool.</typeparam>

    public interface IComponentPool<T> where T : PoolableComponent
    {
        /// <summary>
        /// Cleans up the pool by checking each pooled Component 
        /// to ensure it is still actually valid.
        /// <para>
        /// Must be called regularly to free returned Components.
        /// </para>
        /// </summary>

        void CleanUp();

        /// <summary>
        ///// Tries to gets a new Component from the Pool.
        /// </summary>
        /// <param name="newComponent">
        /// When this method returns, contains the next object in the pool, 
        /// if available; otherwise, null if all pooled instances are valid. 
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// True if a new Component could be created from the pool; otherwise, false.
        /// </returns>

        bool TryCreate(out T newComponent);
        
        /// <summary>
        /// Returns the Component to the pool so that it can be reused.
        /// </summary>
        /// <param name="component">The Component to return.</param>
        void ReturnObject(T component);
    }
}