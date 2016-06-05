using CoreMUD.ECS.Test.Component;
using NUnit.Framework;
using Shouldly;

namespace CoreMUD.ECS.Test
{
    [TestFixture]
    public class Aspect_Tests
    {
        [Test]
        public void TestBaseComponent_IsDerived_Should_Be_False()
        {
            var baseComponent = new TestBaseComponent();
            baseComponent.IsDerived().ShouldBeFalse();
        }

        [Test]
        public void TestDerivedComponent_IsDerived_Should_Be_True()
        {
            var derived = new TestDerivedComponent();
            derived.IsDerived().ShouldBeTrue();
        }
    }
}
