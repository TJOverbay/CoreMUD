using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace CoreMUD.Core.Test
{
    [TestFixture]
    public class ActionInput_Tests
    {
        [Test]
        public void ActionInput_ParseText_with_Empty_String()
        {
            var command = string.Empty;
            var actionInput = new ActionInput(command, null);
            actionInput.Noun.ShouldBeNull();
            actionInput.Tail.ShouldBeNull();
            actionInput.Params.ShouldBeNull();
        }

        [Test]
        public void ActionInput_ParseText_with_One_Word_Command()
        {
            var command = "look";
            var actionInput = new ActionInput(command, null);
            actionInput.Noun.ShouldBe("look");
            actionInput.Tail.ShouldBeNull();
            actionInput.Params.ShouldBeNull();
        }

        [Test]
        public void ActionInput_ParseText_with_Two_Word_Command()
        {
            var command = "look foo";
            var actionInput = new ActionInput(command, null);
            actionInput.Noun.ShouldBe("look");
            actionInput.Tail.ShouldBe("foo");
            actionInput.Params.ShouldNotBeNull();
            actionInput.Params.Length.ShouldBe(1);
        }

        [Test]
        public void ActionInput_ParseText_with_Three_Word_Command()
        {
            var command = "create consumable metal";
            var actionInput = new ActionInput(command, null);
            actionInput.Noun.ShouldBe("create");
            actionInput.Tail.ShouldBe("consumable metal");
            actionInput.Params.ShouldNotBeNull();
            actionInput.Params.Length.ShouldBe(2);
        }

        [Test]
        public void ActionInput_ParseText_with_Leading_and_Trailing_Spaces()
        {
            var command = " look foo ";
            var actionInput = new ActionInput(command, null);
            actionInput.Noun.ShouldBe("look");
            actionInput.Tail.ShouldBe("foo");
            actionInput.Params.ShouldNotBeNull();
            actionInput.Params.Length.ShouldBe(1);
        }

        [Test]
        public void ActionInput_ParseText_with_Multiple_Whitespace()
        {
            var command = "   look \r\n foo  \t\t  ";
            var actionInput = new ActionInput(command, null);
            actionInput.Noun.ShouldBe("look");
            actionInput.Tail.ShouldBe("foo");
            actionInput.Params.ShouldNotBeNull();
            actionInput.Params.Length.ShouldBe(1);
        }
    }
}
