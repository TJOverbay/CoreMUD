using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMUD.Core
{
    public interface ISystem
    {
        // Called by the server every frame
        void Update(GameTime gameTime);
    }

    public interface ISystem<TComponent> : ISystem where TComponent : IComponent
    {
        void RegisterComponent(int entityId, TComponent component);
        IEnumerable<TComponent> GetComponents();
        TComponent this[int entityId] { get; }
    }
}
