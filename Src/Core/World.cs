using System;
using System.Collections.Generic;
using SelfishFramework.Src.Core.Collections;
using SelfishFramework.Src.Core.Components;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;

namespace SelfishFramework.Src.Core
{
    public class World : IDisposable
    {
        private readonly Dictionary<int, IComponentPool> _componentPools = new();
        private readonly Queue<int> _recycledIndices = new(Constants.START_ENTITY_COUNT);
        private readonly Dictionary<int, ISystemPool> _systemPools = new();

        internal readonly FastList<int> dirtyEntities = new(Constants.START_ENTITY_COUNT);

        internal readonly Dictionary<long, Dictionary<long, Filter.Filter>> filters = new();

        public readonly SystemModuleRegistry SystemModuleRegistry = new();

        internal int entitiesCapacity = Constants.START_ENTITY_COUNT;
        internal int entitiesCount;
        internal int[] entitiesGenerations = new int[Constants.START_ENTITY_COUNT];

        public FilterBuilder Filter => FilterBuilder.Create(this);

        public void Dispose()
        {
            SystemModuleRegistry.Dispose();
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
                foreach (var componentPool in _componentPools) componentPool.Value.Resize(entitiesCapacity);

                foreach (var systemPool in _systemPools) systemPool.Value.Resize(entitiesCapacity);
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
            entitiesGenerations[entity.Id]++;
            entitiesCount--;
        }

        public ComponentPool<T> GetComponentPool<T>() where T : struct, IComponent
        {
            var index = ComponentPool<T>.Info.Index;
            if (_componentPools.TryGetValue(index, out var rawPool))
                return (ComponentPool<T>)rawPool;

            var pool = new ComponentPool<T>(entitiesCapacity);
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
    }
}