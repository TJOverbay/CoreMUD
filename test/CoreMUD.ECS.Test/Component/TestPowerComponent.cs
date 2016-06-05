using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMUD.ECS.Test.Component
{
    /// <summary>The power component</summary>

    public class TestPowerComponent : IComponent
    {
        /// <summary>
        /// Gets or sets the power.
        /// </summary>

        public double Power { get; set; }
    }
}
