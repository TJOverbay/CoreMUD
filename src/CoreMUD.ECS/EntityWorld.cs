using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMUD.ECS.Pooling;

namespace CoreMUD.ECS
{
    public sealed class EntityWorld
    {
        private readonly ConcurrentBag<Entity> _deletedEntities;
        private readonly ConcurrentDictionary<string, IEntityTemplate> _entityTemplates;
        private readonly ConcurrentDictionary<Type, IComponentPool<PoolableComponent>> _componentPools;

    }
}
