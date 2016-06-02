namespace CoreMUD.Core.AI
{
    public class TurnBasedAiComponentDecorator
        : TurnBasedObject, IAiComponent
    {
        private readonly IAiComponent _decorated;

        public TurnBasedAiComponentDecorator(IAiComponent decoratedComponent)
            : base(string.IsNullOrWhiteSpace(decoratedComponent?.Name) ? "AI component" : decoratedComponent.Name)
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
