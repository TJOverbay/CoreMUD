using System;

namespace CoreMUD.Core.Input
{
    public class TurnBasedInputComponentDecorator
        : TurnBasedObject, IInputComponent
    {
        private readonly IInputComponent _decorated;

        public TurnBasedInputComponentDecorator(IInputComponent decoratedComponent)
            : base(string.IsNullOrWhiteSpace(decoratedComponent?.Name) ? "input component" : decoratedComponent.Name)
        {
            _decorated = decoratedComponent;
        }

        protected override string Name
        {
            get { return _decorated.Name; }
            set { _decorated.Name = value; }
        }
    }
}
