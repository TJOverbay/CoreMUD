namespace CoreMUD.Core.AI
{
    public class TurnBasedAiComponentDecorator
        : TurnBasedObject, IAiComponent
    {
        private readonly IAiComponent _decorated;

        public TurnBasedAiComponentDecorator(IAiComponent decoratedComponent)
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
