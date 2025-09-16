using System;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Components;
using SelfishFramework.Src.Core.Dependency;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.SystemModules.CommandBusModule;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.SLogs;

namespace SelfishFramework.Src.Core
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Check if the specified entity is disposed.
        /// </summary>
        public static bool IsDisposed(this World world, Entity entity)
        {
            return entity.Id <= 0 ||
                   entity.Id >= world.entitiesCapacity ||
                   world.entitiesGenerations[entity.Id] != entity.Generation;
        }

        public static World GetWorld(this Entity entity)
        {
            var world = SManager.GetWorld(entity.WorldId);
            return world;
        }
        
        public static bool IsInitialized(this Entity entity)
        {
            var world = entity.GetWorld();
            if (world.IsDisposed(entity) || !world.entitiesInitStatus[entity.Id])
            {
                return false;
            }
            return true;
        }
        
        public static void Init(this Entity entity)
        {
            var world = entity.GetWorld();
            if (world.IsDisposed(entity))
            {
                SLog.LogError($"Entity {entity.Id} is disposed and cannot be initialized.");
                return;
            }
            entity.InitSystems();
            world.ModuleRegistry.GetModule<AfterEntityInitModule>().Run(entity);
            world.entitiesInitStatus[entity.Id] = true;
        }
        
        public static void Command<T>(this Entity entity, in T command) where T : struct, ICommand
        {
            var world = entity.GetWorld();
            if (world.IsDisposed(entity))
            {
                SLog.LogError($"Entity {entity.Id} is disposed and cannot execute commands.");
                return;
            }
            world.ModuleRegistry.GetModule<LocalCommandModule>().Invoke(entity, command);
        }
        
#region Components

        /// <summary>
        /// Add a component of type T to the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of component to set.</typeparam>
        /// <param name="entity">The entity to set the component to.</param>
        /// <param name="component">component to set</param>
        /// <returns>A reference to the set component.</returns>
        public static void Set<T>(this Entity entity, in T component) where T : struct, IComponent
        {
            var world = entity.GetWorld();
            var pool = world.GetComponentPool<T>();
            pool.Set(entity.Id, component);
            world.dirtyEntities.Add(entity);
            ref var entityData = ref world.entitiesData[entity.Id];
            if (entityData.componentCount >= entityData.components.Length)
            {
                Array.Resize(ref entityData.components, entityData.components.Length << 1);
            }
            entityData.components[entityData.componentCount++] = pool.GetId();
        } 
        
        /// <summary>
        /// Get a component of type T to the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of component.</typeparam>
        /// <param name="entity">The entity owner of the component to.</param>
        /// <returns>A reference to the component.</returns>
        public static ref T Get<T>(this Entity entity) where T : struct, IComponent
        {
            var pool = entity.GetWorld().GetComponentPool<T>();
            return ref pool.Get(entity.Id);
        } 
        
        /// <summary>
        /// Check if the specified entity has a component of type T.
        /// </summary>
        /// <typeparam name="T">The type of component.</typeparam>
        /// <param name="entity">The entity of the component to check. </param>
        /// <returns>True if the entity has the component.</returns>
        public static bool Has<T>(this Entity entity) where T : struct, IComponent
        {
            var pool = entity.GetWorld().GetComponentPool<T>();
            return pool.Has(entity.Id);
        } 
        
        /// <summary>
        /// Remove a component of type T from the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of component to remove.</typeparam>
        /// <param name="entity">The entity to remove the component from. </param>
        public static void Remove<T>(this Entity entity) where T : struct, IComponent
        {
            var world = entity.GetWorld();
            var pool = world.GetComponentPool<T>();
            pool.Remove(entity.Id);
            world.dirtyEntities.Add(entity);
            var typeId = ComponentPool<T>.Info.TypeId;
            RemoveComponentFromEntityData(entity, world, typeId);
        }

        /// <summary>
        /// Remove a component of typeId from the specified entity.
        /// </summary>
        /// <param name="entity">The entity to remove the component from. </param>
        /// <param name="poolIndex">index of component pool</param>
        public static void Remove(this Entity entity, int poolIndex)
        {
            var world = entity.GetWorld();
            if (!world.TryGetComponentPool(poolIndex, out var pool))
            {
                SLog.LogError($"Component pool not found for ID: {poolIndex}");
                return;
            }
            pool.Remove(entity.Id);
            world.dirtyEntities.Add(entity);
            RemoveComponentFromEntityData(entity, world, poolIndex);
        }

        private static void RemoveComponentFromEntityData(Entity entity, World world, int poolIndex)
        {
            ref var entityData = ref world.entitiesData[entity.Id];
            for (int i = entityData.componentCount - 1; i >= 0; i--)
            {
                if (entityData.components[i] == poolIndex)
                {
                    entityData.components[i] = entityData.components[--entityData.componentCount];
                    break;
                }
            }
        }

        #endregion

#region Systems
        /// <summary>
        /// Adds a system of type T to the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of system to add.</typeparam>
        /// <param name="entity">The entity to add the system to.</param>
        /// <returns>A reference to the added system.</returns>
        public static void AddSystem<T>(this Entity entity) where T : BaseSystem, new()
        {
            var world = entity.GetWorld();
            var system = world.GetSystemPool<T>().Add(entity.Id);
            system.Owner = entity;
            system.World = world;
            ref var entityData = ref world.entitiesData[entity.Id];
            if (entityData.systemCount >= entityData.systems.Length)
            {
                Array.Resize(ref entityData.systems, entityData.systems.Length << 1);
            }
            entityData.systems[entityData.systemCount++] = SystemPool<T>.TypeId;
            if (system is IInjectable injectable)
            {
                injectable.ResolveDependencies(world.DependencyContainer);
            }
        }

        private static void InitSystems(this Entity entity)
        {
            var world = entity.GetWorld();
            ref var entityData = ref world.entitiesData[entity.Id];
            for (var i = 0; i < entityData.systemCount; i++)
            {
                var systemId = entityData.systems[i];
                if (!world.TryGetSystemPool(systemId, out var pool))
                {
                    SLog.LogError("System pool not found for ID: " + systemId);
                    continue;
                }

                var system = pool.GetRaw(entity.Id);
                system.InitSystem();
                world.ModuleRegistry.Register(system);
                system.RegisterCommands();
            }
        }
        /// <summary>
        /// Try get a system of type T to the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of system.</typeparam>
        /// <param name="entity">The entity owner of the system.</param>
        /// <param name="system">The reference to the system.</param>
        /// <returns>True if the entity has the system.</returns>
        public static bool TryGetSystem<T>(this Entity entity, out T system) where T : BaseSystem, new()
        {
            var systemPool = entity.GetWorld().GetSystemPool<T>();
            if (!systemPool.Has(entity.Id))
            {
                system = default; 
                return false;
            }
            system = systemPool.Get(entity.Id);
            return true;
        }

        /// <summary>
        /// Remove a system of type T from the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of system.</typeparam>
        /// <param name="entity">The entity to remove the system from.</param>
        public static void RemoveSystem<T>(this Entity entity) where T : BaseSystem, new()
        {
            var world = entity.GetWorld();
            var systemPool = world.GetSystemPool<T>();
            var system = systemPool.Get(entity.Id);
            systemPool.Remove(entity.Id); 
            world.ModuleRegistry.Unregister(system);
            var typeId = SystemPool<T>.TypeId;
            system.Dispose();
            system.UnregisterCommands();
            RemoveSystemFromEntityData(entity, world, typeId);
        }

        /// <summary>
        /// Remove a system of type T from the specified entity.
        /// </summary>
        /// <param name="entity">The entity to remove the system from.</param>
        /// <param name="systemId">typeId of system</param>
        public static void RemoveSystem(this Entity entity, int systemId)
        {
            var world = entity.GetWorld();
            if (!world.TryGetSystemPool(systemId, out var systemPool))
            {
                SLog.LogError($"System with ID {systemId} not found in world {world.Index} for entity {entity.Id}.");
                return;
            }
            
            var system = systemPool.GetRaw(entity.Id);
            systemPool.Remove(entity.Id);
            world.ModuleRegistry.Unregister(system);
            system.Dispose();
            system.UnregisterCommands(); 
            RemoveSystemFromEntityData(entity, world, systemId);
        }

        private static void RemoveSystemFromEntityData(Entity entity, World world, int typeId)
        {
            ref var entityData = ref world.entitiesData[entity.Id];
            for (int i = entityData.systemCount - 1; i >= 0; i--)
            {
                if (entityData.systems[i] == typeId)
                {
                    entityData.systems[i] = entityData.systems[--entityData.systemCount];
                    break;
                }
            }
        }

        #endregion
    }
}