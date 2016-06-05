using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMUD.ECS.Pooling
{
    /// <summary>
    /// Base class for poolable Components
    /// </summary>

    public abstract class PoolableComponent : IComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PoolableComponent"/>.
        /// </summary>

        protected PoolableComponent()
        {
            PoolId = 0;
        }

        internal int PoolId { get; set; }

        public virtual void CleanUp() { }

        public virtual void Initialize() { }
    }
}
