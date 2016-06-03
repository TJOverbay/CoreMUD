using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMUD.ECS.Test.Component
{
    internal class TestDerivedComponent : TestBaseComponent
    {
        public int DerivedValue { get; set; }
    }
}
