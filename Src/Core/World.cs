using System;
using System.Collections.Generic;
using SelfishFramework.Src.Core.Components;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;

namespace SelfishFramework.Src.Core
{
    public class World : IDisposable
    {
        private readonly Dictionary<Type, IComponentPool> _componentPools = new();
        private readonly Dictionary<Type, ISystemPool> _systemPools = new();
        
        private Entity[] _entities = new Entity[Constants.StartEntityCount];
        private int _entitiesCount = 0;
        private readonly Queue<int> _recycledIndices = new(Constants.StartEntityCount);
        
        public readonly SystemModuleRegistry SystemModuleRegistry = new();

        public bool IsEntityAlive(int id)
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
            var type = typeof(T);
            if (_componentPools.TryGetValue(type, out var rawPool))
                return (ComponentPool<T>)rawPool;
            
            var pool = new ComponentPool<T>(this, _entities.Length);
            _componentPools.Add(type, pool);
            return pool;
        }

        public SystemPool<T> GetSystemPool<T>() where T : BaseSystem, new()
        {
            var type = typeof(T);
            if (_systemPools.TryGetValue(type, out var rawPool))
                return (SystemPool<T>)rawPool;
            
            var pool = new SystemPool<T>(this);
            _systemPools.Add(type, pool);
            return pool;
        }

        public void Dispose()
        {
            SystemModuleRegistry.Dispose();
        }
    }
}