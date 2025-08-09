using System;
using System.Collections.Generic;
using SelfishFramework.Src.Core.Components;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;

namespace SelfishFramework.Src.Core
{
    public class World : IDisposable
    {
        private readonly Dictionary<int, IComponentPool> _componentPools = new();
        private readonly Dictionary<int, ISystemPool> _systemPools = new();
        
        private int[] _entitiesGenerations = new int[Constants.START_ENTITY_COUNT];
        private int _entitiesCount;
        private int _entitiesCapacity = Constants.START_ENTITY_COUNT;
        private readonly Queue<int> _recycledIndices = new(Constants.START_ENTITY_COUNT);
        
        public readonly SystemModuleRegistry SystemModuleRegistry = new();

        public FilterBuilder Filter => FilterBuilder.Create(this);
        internal readonly Dictionary<long, Dictionary<long, Filter.Filter>> filters = new();
        
        public bool IsEntityAlive(Entity entity)
        {
            return entity.Id > 0 && entity.Id < _entitiesCount && _entitiesGenerations[entity.Id] == entity.Generation;
        }

        /// <summary>
        /// Registers a new entity in the system.
        /// </summary>
        public Entity NewEntity()
        {
            if (_entitiesCapacity <= _entitiesCount)
            {
                var newSize = _entitiesCount << 1;
                Array.Resize(ref _entitiesGenerations, newSize);
                foreach (var componentPool in _componentPools)
                {
                    componentPool.Value.Resize(newSize);
                }

                foreach (var systemPool in _systemPools)
                {
                    systemPool.Value.Resize(newSize);
                }
            }

            int index;
            if (_recycledIndices.Count > 0)
            {
                index = _recycledIndices.Dequeue();
            }
            else
            {
                index = _entitiesCount;
                _entitiesCount++;
            }

            var entity = new Entity(index, (ushort)(_entitiesGenerations[index] + 1));
            return entity;
        }
        
        /// <summary>
        /// Unregisters an entity in the system.
        /// </summary>
        /// <param name="entity">The entity to unregister.</param>
        public void UnregisterEntity(Entity entity)
        {
            _recycledIndices.Enqueue(entity.Id);
            _entitiesGenerations[entity.Id]++;
            _entitiesCount--;
        }

        public ComponentPool<T> GetComponentPool<T>() where T : struct, IComponent
        {
            var index = ComponentPool<T>.Info.Index;
            if (_componentPools.TryGetValue(index, out var rawPool))
                return (ComponentPool<T>)rawPool;
            
            var pool = new ComponentPool<T>(_entitiesCapacity);
            _componentPools.Add(index, pool);
            return pool;
        }

        public SystemPool<T> GetSystemPool<T>() where T : BaseSystem, new()
        {
            var index = SystemPool<T>.Index;
            if (_systemPools.TryGetValue(index, out var rawPool))
                return (SystemPool<T>)rawPool;
            
            var pool = new SystemPool<T>();
            _systemPools.Add(index, pool);
            return pool;
        }

        public void Dispose()
        {
            SystemModuleRegistry.Dispose();
        }
    }
}