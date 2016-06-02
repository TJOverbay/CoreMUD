using System;
using System.ComponentModel;
using CoreMUD.Interfaces;
using NUnit.Framework;
using Shouldly;

namespace CoreMUD.Core.Test
{
    [TestFixture]
    public class IHavIdentity_Tests
    {
        private class TestIdentity<T> : IHaveIdentity<T>
        {
            private static readonly TypeConverter IdConverter =
                TypeDescriptor.GetConverter(typeof(T));

            private readonly string _id;

            public TestIdentity(T id)
            {
                if (id is string)
                {
                    _id = id as string;
                }
                else
                {
                    _id = IdConverter.ConvertToInvariantString(id);
                }
            }

            public T Id
            {
                get
                {
                    return (T)IdConverter.ConvertFromInvariantString(_id);
                }
            }

            string IHaveIdentity.Id
            {
                get
                {
                    return _id;
                }
            }
        }

        [Test]
        public void IHaveIdentity_Id_Property()
        {
            string expected = "OBJ123";

            IHaveIdentity target = new TestIdentity<string>(expected);

            target.Id.ShouldBeOfType<string>();
            target.Id.ShouldBe(expected);
        }

        [Test]
        public void IHaveIdentity_of_Int_Id_Property()
        {
            int expected = 9876;
            IHaveIdentity<int> target = new TestIdentity<int>(expected);

            target.Id.ShouldBeOfType<int>();
            target.Id.ShouldBe(expected);
        }

        [Test]
        public void IHaveIdentity_of_Guid_Id_Property()
        {
            Guid expected = Guid.NewGuid();
            IHaveIdentity<Guid> target = new TestIdentity<Guid>(expected);

            target.Id.ShouldBeOfType<Guid>();
            target.Id.ShouldBe(expected);
        }

        [Test]
        public void IHaveIdentity_of_Int_as_IHaveIdentity_Id_Property()
        {
            int expected = 334;
            IHaveIdentity target = new TestIdentity<int>(expected);

            target.Id.ShouldBeOfType<string>();
            target.Id.ShouldBe(expected.ToString());
        }

        [Test]
        public void IHaveIdentity_of_Guid_as_IHaveIdentity_Id_Property()
        {
            Guid expected = Guid.NewGuid();
            IHaveIdentity target = new TestIdentity<Guid>(expected);

            target.Id.ShouldBeOfType<string>();
            target.Id.ShouldBe(expected.ToString());
        }

    }
}
