using SelfishFramework.Src.Core.Components;
using SelfishFramework.Src.Core.Systems;

namespace SelfishFramework.Src.Core
{
    public static class EntityExtensions
    {
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
            var pool = entity.World.GetComponentPool<T>();
            pool.Set(entity.Id, component);
        } 
        /// <summary>
        /// Get a component of type T to the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of component.</typeparam>
        /// <param name="entity">The entity owner of the component to.</param>
        /// <returns>A reference to the component.</returns>
        public static ref T Get<T>(this Entity entity) where T : struct, IComponent
        {
            var pool = entity.World.GetComponentPool<T>();
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
            var pool = entity.World.GetComponentPool<T>();
            return pool.Has(entity.Id);
        } 
        /// <summary>
        /// Remove a component of type T from the specified entity.
        /// </summary>
        /// <typeparam name="T">The type of component to remove.</typeparam>
        /// <param name="entity">The entity to remove the component from. </param>
        public static void Remove<T>(this Entity entity) where T : struct, IComponent
        {
            var pool = entity.World.GetComponentPool<T>();
            pool.Remove(entity.Id);
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
            var system = entity.World.GetSystemPool<T>().Add(entity.Id);
            system.Owner = entity;
            entity.World.SystemModuleRegistry.Register(system);
            entity.Systems.Add(SystemPool<T>.Index);
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
            var systemPool = entity.World.GetSystemPool<T>();
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
            var systemPool = entity.World.GetSystemPool<T>();
            var system = systemPool.Get(entity.Id);
            systemPool.Remove(entity.Id); 
            entity.World.SystemModuleRegistry.Unregister(system);
            entity.Systems.Remove(SystemPool<T>.Index);
        }
#endregion
    }
}