using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMUD.ECS.Attributes;
using CoreMUD.ECS.Pooling;

namespace CoreMUD.ECS.Test.Component
{
    /// <summary> The pooled power component </summary>

    [ComponentPool(InitialSize = 10, IsResizable = false)]
    public class TestPowerPoolableComponent : PoolableComponent
    {
        /// <summary> Gets or sets the entity's power </summary>

        public double Power { get; set; }

        /// <summary>
        /// Creates new <see cref="TestPowerPoolableComponent"/> components.
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The new <see cref="TestPowerPoolableComponent"/></returns>

        [ComponentConstructor]
        public static TestPowerPoolableComponent CreateInstance(Type type)
        {
            return new TestPowerPoolableComponent();
        }
    }
}
