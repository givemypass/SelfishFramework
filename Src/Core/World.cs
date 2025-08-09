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
        
        private Entity[] _entities = new Entity[Constants.StartEntityCount];
        private int _entitiesCount;
        private readonly Queue<int> _recycledIndices = new(Constants.StartEntityCount);
        
        public readonly SystemModuleRegistry SystemModuleRegistry = new();

        public FilterBuilder Filter => new(this);
        
        public bool IsEntityAlive(Entity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers a new entity in the system.
        /// </summary>
        /// <param name="entity">The entity to register.</param>
        public void RegisterEntity(Entity entity)
        {
            if (_entities.Length == _entitiesCount + Constants.EntityIndexShift)
            {
                var newSize = (_entitiesCount + Constants.EntityIndexShift) << 1;
                Array.Resize(ref _entities, newSize);
                foreach (var componentPool in _componentPools)
                {
                    componentPool.Value.Resize(newSize);
                }

                foreach (var systemPool in _systemPools)
                {
                    systemPool.Value.Resize(newSize);
                }
            }

            entity.Generation++;
            var idx = _recycledIndices.Count > 0 ? _recycledIndices.Dequeue() : _entitiesCount++;
            entity.Id = idx + Constants.EntityIndexShift;
            _entities[idx] = entity;
        }
        
        /// <summary>
        /// Unregisters an entity in the system.
        /// </summary>
        /// <param name="entity">The entity to unregister.</param>
        public void UnregisterEntity(Entity entity)
        {
            _recycledIndices.Enqueue(entity.Id);
            _entities[entity.Id] = default;
            _entitiesCount--;
        }

        public ComponentPool<T> GetComponentPool<T>() where T : struct, IComponent
        {
            var index = ComponentPool<T>.Index;
            if (_componentPools.TryGetValue(index, out var rawPool))
                return (ComponentPool<T>)rawPool;
            
            var pool = new ComponentPool<T>(_entities.Length);
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