using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMUD.ECS;
using System.Reflection;

namespace CoreMUD.ECS.Test.Component
{
    internal class TestBaseComponent : IComponent
    {
        public int Value { get; set; }

        public virtual bool IsDerived()
        {
            return IsDerivedFromComponent(GetType().GetTypeInfo());
        }

        private static bool IsDerivedFromComponent(TypeInfo theType)
        {
            var baseType = theType.BaseType;

            if (baseType == null)
                return false;

            var baseTypeInfo = baseType.GetTypeInfo();
            if (baseTypeInfo.ImplementedInterfaces.Contains(typeof(IComponent)))
                return true;

            return IsDerivedFromComponent(baseTypeInfo);
        }
    }
}
