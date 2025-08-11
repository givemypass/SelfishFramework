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
        private readonly Queue<int> _recycledIndices = new(Constants.START_ENTITY_COUNT);
        private readonly Dictionary<int, ISystemPool> _systemPools = new();

        internal readonly HashSet<Entity> dirtyEntities = new(Constants.START_ENTITY_COUNT);

        internal readonly Dictionary<long, Dictionary<long, Filter.Filter>> filters = new();

        public readonly SystemModuleRegistry SystemModuleRegistry = new();
        
        private IComponentPool[] _componentPools = new IComponentPool[32];
        private int _componentPoolsCount;
        private readonly Dictionary<LongHash, int> _componentPoolsMap = new(32);

        internal int entitiesCapacity = Constants.START_ENTITY_COUNT;
        internal int entitiesCount;
        internal int[] entitiesGenerations = new int[Constants.START_ENTITY_COUNT];

        public World()
        {
            SystemModuleRegistry.RegisterModule(new UpdateDefaultModule());
            SystemModuleRegistry.RegisterModule(new FixedUpdateModule());
            SystemModuleRegistry.RegisterModule(new GlobalStartModule());
        }

        public FilterBuilder Filter => FilterBuilder.Create(this);

        public void Dispose()
        {
            SystemModuleRegistry.Dispose();
            //todo dispose all stuff
        }

        public void Commit()
        {
            foreach (var dict in filters.Values)
            {
                foreach (var filter in dict.Values)
                {
                    filter.UpdateFilter(dirtyEntities);
                }
            }

            dirtyEntities.Clear();
        }

        /// <summary>
        ///     Registers a new entity in the system.
        /// </summary>
        public Entity NewEntity()
        {
            int index;
            if (_recycledIndices.Count > 0)
                index = _recycledIndices.Dequeue();
            else
                //we need to skip 0 index, because it is reserved for default entity
                index = ++entitiesCount;

            if (entitiesCapacity <= entitiesCount)
            {
                entitiesCapacity = entitiesCount << 1;
                Array.Resize(ref entitiesGenerations, entitiesCapacity);
                foreach (var componentPool in _componentPools)
                {
                    componentPool.Resize(entitiesCapacity);
                }

                foreach (var systemPool in _systemPools)
                {
                    systemPool.Value.Resize(entitiesCapacity);
                }
            }

            var entity = new Entity(index, (ushort)entitiesGenerations[index]);
            return entity;
        }

        /// <summary>
        ///     Unregisters an entity in the system.
        /// </summary>
        /// <param name="entity">The entity to unregister.</param>
        public void UnregisterEntity(Entity entity)
        {
            _recycledIndices.Enqueue(entity.Id);
            dirtyEntities.Add(entity);
            entitiesGenerations[entity.Id]++;
            entitiesCount--;
        }

        public ComponentPool<T> GetComponentPool<T>() where T : struct, IComponent
        {
            var hash = ComponentPool<T>.Info.Hash;
            if(_componentPoolsMap.TryGetValue(hash, out var index))
            {
                return _componentPools[index] as ComponentPool<T>;
            }
            
            if(_componentPoolsCount >= _componentPools.Length)
            {
                Array.Resize(ref _componentPools, _componentPools.Length << 1);
            }
            
            index = _componentPoolsCount++;
            var pool = new ComponentPool<T>(index, entitiesCapacity);
            _componentPools[index] = pool;
            _componentPoolsMap.Add(hash, index);
            return pool;
        }

        public bool TryGetComponentPool(int index, out IComponentPool pool)
        {
            if(index < 0 || index >= _componentPools.Length)
            {
                pool = null;
                return false;
            }
            pool = _componentPools[index];
            return pool != null;
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
    }
}