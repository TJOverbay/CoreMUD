using System;
using System.Collections.Generic;

namespace CoreMUD.Core.AI
{
    public class TurnBasedAiServiceDecorator : TurnBasedObject, IAiSystem
    {
        private readonly IDictionary<int, IAiComponent> _components;

        public TurnBasedAiServiceDecorator()
        {
            _components = new Dictionary<int, IAiComponent>();
        }

        public IAiComponent this[int entityId]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<IAiComponent> GetComponents()
        {
            throw new NotImplementedException();
        }

        public void AddComponent(int entityId, IAiComponent component)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
