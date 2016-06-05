using CoreMUD.ECS.Test.Component;
using CoreMUD.ECS.Pooling;
using NUnit.Framework;
using Shouldly;
using System;

namespace CoreMUD.ECS.Test.Pooling
{
    [TestFixture]
    public class ComponentPool_Tests
    {
        [Test]
        public void ComponentPool_Ctor_InitialSize_Sets_InvalidCount()
        {
            Type innerType = typeof(PoolableComponent);
            Func<Type, TestPowerPoolableComponent> factoryFunc = (t) =>
                new TestPowerPoolableComponent();
            int initialSize = 10;

            var target = new ComponentPool<TestPowerPoolableComponent>(
                innerType,
                factoryFunc,
                initialSize);

            target.InvalidCount.ShouldBe(10);
        }

        [Test]
        public void ComponentPool_Ctor_Throws_on_NULL_InnerType()
        {

            Type innerType = null;
            Func<Type, TestPowerPoolableComponent> factoryFunc = (t) =>
                new TestPowerPoolableComponent();
            int initialSize = 10;

            try
            {
                var target = new ComponentPool<TestPowerPoolableComponent>(
                    innerType,
                    factoryFunc,
                    initialSize);
            }
            catch (ArgumentNullException ex)
            {
                ex.Message.ShouldContain(nameof(innerType));
                return;
            }

            Assert.Fail($"An {nameof(ArgumentNullException)} should have been thrown, but it was not");
        }

        [Test]
        public void ComponentPool_Ctor_Throws_on_NULL_FactoryFunc()
        {
            Type innerType = typeof(PoolableComponent);
            Func<Type, TestPowerPoolableComponent> factoryFunc = null;
            int initialSize = 10;

            try
            {
                var target = new ComponentPool<TestPowerPoolableComponent>(
                    innerType,
                    factoryFunc,
                    initialSize);
            }
            catch (ArgumentNullException ex)
            {
                ex.Message.ShouldContain(nameof(factoryFunc));
                return;
            }

            Assert.Fail($"An {nameof(ArgumentNullException)} should have been thrown, but it was not");
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-456)]
        public void ComponentPool_Ctor_Throws_on_InitialSize_Less_than_One(int initialSize)
        {
            Type innerType = typeof(PoolableComponent);
            Func<Type, TestPowerPoolableComponent> factoryFunc = (t) =>
                new TestPowerPoolableComponent();

            try
            {
                var target = new ComponentPool<TestPowerPoolableComponent>(
                    innerType,
                    factoryFunc,
                    initialSize);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ex.Message.ShouldContain(nameof(initialSize));
                return;
            }

            Assert.Fail($"An {nameof(ArgumentOutOfRangeException)} should have been thrown, but it was not");
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-456)]
        public void ComponentPool_Ctor_Throws_on_CanResize_True_and_ResizeIncrement_Less_than_One(int resizeIncrement)
        {
            Type innerType = typeof(PoolableComponent);
            Func<Type, TestPowerPoolableComponent> factoryFunc = (t) =>
                new TestPowerPoolableComponent();
            int initialSize = 10;
            bool canResize = true;

            try
            {
                var target = new ComponentPool<TestPowerPoolableComponent>(
                    innerType,
                    factoryFunc,
                    initialSize,
                    canResize,
                    resizeIncrement);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ex.Message.ShouldContain(nameof(resizeIncrement));
                return;
            }

            Assert.Fail($"An {nameof(ArgumentOutOfRangeException)} should have been thrown, but it was not");
        }

        [TestCase(10)]
        [TestCase(0)]
        [TestCase(-10)]
        public void ComponentPool_Ctor_sets_ResizeIncrement_to_Zero_when_CanResize_False(int resizeIncrement)
        {
            Type innerType = typeof(PoolableComponent);
            Func<Type, TestPowerPoolableComponent> factoryFunc = (t) =>
                new TestPowerPoolableComponent();
            int initialSize = 10;
            bool canResize = false;

            var target = new ComponentPool<TestPowerPoolableComponent>(
                innerType,
                factoryFunc,
                initialSize,
                canResize,
                resizeIncrement);

            target.ResizeIncrement.ShouldBe(0);
        }

        [Test]
        public void ComponentPool_TryCreate_Returns_True_When_Available()
        {
            Type innerType = typeof(PoolableComponent);
            Func<Type, TestPowerPoolableComponent> factoryFunc = (t) =>
                new TestPowerPoolableComponent()
                {
                    Power = 250
                };

            int initialSize = 10;

            TestPowerPoolableComponent result;

            var target = new ComponentPool<TestPowerPoolableComponent>(
                innerType,
                factoryFunc,
                initialSize);

            target.TryCreate(out result).ShouldBeTrue();
            target.ValidCount.ShouldBe(1);
            target.InvalidCount.ShouldBe(9);

            result.ShouldNotBeNull();
            result.Power.ShouldBe(250);
        }

        [Test]
        public void ComponentPool_TryCreate_Returns_False_When_Not_Available_and_CanResize_False()
        {
            Type innerType = typeof(PoolableComponent);
            Func<Type, TestPowerPoolableComponent> factoryFunc = (t) =>
                new TestPowerPoolableComponent()
                {
                    Power = 250
                };

            int initialSize = 1; // Only allow 1 component in the pool

            TestPowerPoolableComponent first, result;

            var target = new ComponentPool<TestPowerPoolableComponent>(
                innerType,
                factoryFunc,
                initialSize);

            // First try should empty the pool
            target.TryCreate(out first).ShouldBeTrue();
            target.ValidCount.ShouldBe(1);
            target.InvalidCount.ShouldBe(0);

            // Next try should fail
            target.TryCreate(out result).ShouldBeFalse();
            target.ValidCount.ShouldBe(1);
            target.InvalidCount.ShouldBe(0);

            result.ShouldBeNull();
        }

        [Test]
        public void ComponentPool_TryCreate_Returns_True_and_Increases_Pool_Size_When_Not_Available_and_CanResize_True()
        {
            Type innerType = typeof(PoolableComponent);
            Func<Type, TestPowerPoolableComponent> factoryFunc = (t) =>
                new TestPowerPoolableComponent()
                {
                    Power = 250
                };

            int initialSize = 1; // Only allow 1 component in the pool

            TestPowerPoolableComponent first, result;

            var target = new ComponentPool<TestPowerPoolableComponent>(
                innerType,
                factoryFunc,
                initialSize,
                canResize: true,
                resizeIncrement: 10);

            // First try should empty the pool
            target.TryCreate(out first).ShouldBeTrue();
            target.ValidCount.ShouldBe(1);
            target.InvalidCount.ShouldBe(0);

            // Next try should increase the pool size
            target.TryCreate(out result).ShouldBeTrue();
            target.ValidCount.ShouldBe(2);
            target.InvalidCount.ShouldBe(9); // 1 - 1 + 10 - 1 = 9

            result.ShouldNotBeNull();
        }
    }
}
