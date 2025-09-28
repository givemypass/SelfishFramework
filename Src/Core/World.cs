using System;
using System.Collections.Generic;
using System.Linq;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Components;
using SelfishFramework.Src.Core.Dependency;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.SystemModules.CommandBusModule;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.SLogs;

namespace SelfishFramework.Src.Core
{
    public class World : IDisposable
    {
        private readonly Dictionary<LongHash, int> _componentPoolsMap = new(32);
        private readonly ushort _index;

        //todo polish fields when understand how to use them properly
        private readonly Queue<int> _recycledIndices = new(Constants.START_ENTITY_COUNT);
        private readonly Dictionary<int, ISystemPool> _systemPools = new();

        public readonly DependencyContainer DependencyContainer;

        internal readonly HashSet<Entity> dirtyEntities = new(Constants.START_ENTITY_COUNT);
        internal readonly HashSet<Entity> entities = new(Constants.START_ENTITY_COUNT);
        internal EntityData[] entitiesData = new EntityData[Constants.START_ENTITY_COUNT]; 

        internal readonly Dictionary<long, Dictionary<long, Filter.Filter>> filters = new();
        public readonly ModuleRegistry ModuleRegistry = new();

        private IComponentPool[] _componentPools = new IComponentPool[32];
        private int _componentPoolsCount;

        internal int entitiesCapacity = Constants.START_ENTITY_COUNT;
        internal int entitiesCount;
        internal int[] entitiesGenerations = new int[Constants.START_ENTITY_COUNT];
        internal bool[] entitiesInitStatus = new bool[Constants.START_ENTITY_COUNT];

        public World(ushort index)
        {
            _index = index;
            for (var i = 0; i < entitiesData.Length; i++)
            {
                entitiesData[i].Initialize();
            }

            DependencyContainer = new DependencyContainer(ModuleRegistry);
            ModuleRegistry.RegisterModule(new PreUpdateModule());
            ModuleRegistry.RegisterModule(new UpdateDefaultModule());
            ModuleRegistry.RegisterModule(new FixedUpdateModule());
            ModuleRegistry.RegisterModule(new GlobalStartModule());
            ModuleRegistry.RegisterModule(new LateStartModule());
            ModuleRegistry.RegisterModule(new AfterEntityInitModule());
            ModuleRegistry.RegisterModule(new LocalCommandModule());
            ModuleRegistry.RegisterModule(new GlobalCommandModule());
        }

        public int Index => _index;

        public FilterBuilder Filter => FilterBuilder.Create(this);

        public void Commit()
        {
            foreach (var dict in filters.Values)
            foreach (var filter in dict.Values)
                filter.UpdateFilter(dirtyEntities);

            dirtyEntities.Clear();
        }

        /// <summary>
        ///     Registers a new entity in the system.
        /// </summary>
        public Entity NewEntity()
        {
            //we need to skip 0 index, because it is reserved for default entity
            int index = ++entitiesCount;
            if (_recycledIndices.Count > 0)
            {
                index = _recycledIndices.Dequeue();
            }

            if (entitiesCapacity <= entitiesCount)
            {
                var oldCapacity = entitiesCapacity;
                entitiesCapacity = entitiesCount << 1;
                Array.Resize(ref entitiesData, entitiesCapacity);
                for (int i = oldCapacity; i < entitiesCapacity; i++)
                {
                    entitiesData[i].Initialize(); 
                }
                Array.Resize(ref entitiesGenerations, entitiesCapacity);
                Array.Resize(ref entitiesInitStatus, entitiesCapacity);
                foreach (var componentPool in _componentPools) componentPool.Resize(entitiesCapacity);

                foreach (var systemPool in _systemPools) systemPool.Value.Resize(entitiesCapacity);
            }

            var entity = new Entity(index, (ushort)entitiesGenerations[index], _index);
            entities.Add(entity);
            return entity;
        }

        /// <summary>
        ///     Unregisters an entity in the system.
        /// </summary>
        /// <param name="entity">The entity to unregister.</param>
        public void DelEntity(Entity entity)
        {
            ref var entityData = ref entitiesData[entity.Id];
            while (entityData.systemCount > 0)
            {
                entity.RemoveSystem(entityData.systems[0]); 
            }
            while (entityData.componentCount > 0)
            {
                entity.Remove(entityData.components[0]); 
            }
            _recycledIndices.Enqueue(entity.Id);
            dirtyEntities.Add(entity);
            entitiesGenerations[entity.Id]++;
            entitiesInitStatus[entity.Id] = false;
            entitiesCount--;
            entities.Remove(entity);
        }

        public ComponentPool<T> GetComponentPool<T>() where T : struct, IComponent
        {
            var hash = ComponentPool<T>.Info.Hash;
            if (_componentPoolsMap.TryGetValue(hash, out var index)) return _componentPools[index] as ComponentPool<T>;

            if (++_componentPoolsCount >= _componentPools.Length)
                Array.Resize(ref _componentPools, _componentPools.Length << 1);

            index = _componentPoolsCount;
            var pool = new ComponentPool<T>(index, entitiesCapacity);
            _componentPools[index] = pool;
            _componentPoolsMap.Add(hash, index);
            return pool;
        }

        public bool TryGetComponentPool(int index, out IComponentPool pool)
        {
            if (index < 0 || index >= _componentPools.Length)
            {
                pool = null;
                return false;
            }

            pool = _componentPools[index];
            return pool != null;
        }

        public SystemPool<T> GetSystemPool<T>() where T : BaseSystem, new()
        {
            var typeId = SystemPool<T>.TypeId;
            if (_systemPools.TryGetValue(typeId, out var rawPool))
                return (SystemPool<T>)rawPool;

            var pool = new SystemPool<T>();
            _systemPools.Add(typeId, pool);
            return pool;
        }

        public bool TryGetSystemPool(int typeId, out ISystemPool pool)
        {
            if (_systemPools.TryGetValue(typeId, out pool)) return true;

            return false;
        }

        public void Command<T>(T command) where T : IGlobalCommand
        {
            ModuleRegistry.GetModule<GlobalCommandModule>().Invoke(command);
        }

        public void Dispose()
        {
            ModuleRegistry.Dispose();
            foreach (var systemPool in _systemPools.Values)
            {
                systemPool.Dispose();
            }
        }
    }
}