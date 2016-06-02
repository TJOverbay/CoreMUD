using System;

namespace CoreMUD.Core.Input
{
    public class TurnBasedInputComponentDecorator
        : TurnBasedObject, IInputComponent
    {
        private readonly IInputComponent _decorated;

        public TurnBasedInputComponentDecorator(IInputComponent decoratedComponent)
        {
            _decorated = decoratedComponent;
        }

        public string Name
        {
            get { return _decorated.Name; }
            set { _decorated.Name = value; }
        }
    }
}
