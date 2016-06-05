namespace CoreMUD.ECS
{
    /// <summary>
    /// Interface for building Entities from a template
    /// </summary>

    public interface IEntityTemplate
    {
        /// <summary>
        /// Builds an Entity from this template.
        /// </summary>
        /// <param name="entity">The Entity.</param>
        /// <param name="entityWorld">The entities world.</param>
        /// <param name="args">Optional arguments.</param>
        /// <returns>
        /// The newly built Entity
        /// </returns>

        Entity BuildEntity(Entity entity, EntityWorld world, params object[] args);
    }
}