namespace SelfishFramework.Src.Core
{
    public static class ActorExtensions
    {
#region Components
        /// <summary>
        /// Add a component of type T to the specified actor.
        /// </summary>
        /// <typeparam name="T">The type of component to add.</typeparam>
        /// <param name="actor">The actor to add the component to.</param>
        /// <returns>A reference to the added component.</returns>
        public static ref T Add<T>(this Actor actor) where T : struct, IComponent
        {
            var pool = actor.World.GetComponentPool<T>();
            return ref pool.Add(actor.Id);
        } 
        /// <summary>
        /// Get a component of type T to the specified actor.
        /// </summary>
        /// <typeparam name="T">The type of component.</typeparam>
        /// <param name="actor">The actor owner of the component to.</param>
        /// <returns>A reference to the component.</returns>
        public static ref T Get<T>(this Actor actor) where T : struct, IComponent
        {
            var pool = actor.World.GetComponentPool<T>();
            return ref pool.Get(actor.Id);
        } 
        /// <summary>
        /// Check if the specified actor has a component of type T.
        /// </summary>
        /// <typeparam name="T">The type of component.</typeparam>
        /// <param name="actor">The actor of the component to check. </param>
        /// <returns>True if the actor has the component.</returns>
        public static bool Contains<T>(this Actor actor) where T : struct, IComponent
        {
            var pool = actor.World.GetComponentPool<T>();
            return pool.Has(actor.Id);
        } 
        /// <summary>
        /// Remove a component of type T from the specified actor.
        /// </summary>
        /// <typeparam name="T">The type of component to remove.</typeparam>
        /// <param name="actor">The actor to remove the component from. </param>
        public static void Remove<T>(this Actor actor) where T : struct, IComponent
        {
            var pool = actor.World.GetComponentPool<T>();
            pool.Remove(actor.Id);
        } 
#endregion

#region Systems
        /// <summary>
        /// Adds a system of type T to the specified actor.
        /// </summary>
        /// <typeparam name="T">The type of system to add.</typeparam>
        /// <param name="actor">The actor to add the system to.</param>
        /// <returns>A reference to the added system.</returns>
        public static void AddSystem<T>(this Actor actor) where T : BaseSystem, new()
        {
            var system = actor.World.GetSystemPool<T>().Add(actor.Id);
            system.Owner = actor;
        }

        /// <summary>
        /// Try get a system of type T to the specified actor.
        /// </summary>
        /// <typeparam name="T">The type of system.</typeparam>
        /// <param name="actor">The actor owner of the system.</param>
        /// <param name="system">The reference to the system.</param>
        /// <returns>True if the actor has the system.</returns>
        public static bool TryGetSystem<T>(this Actor actor, out T system) where T : BaseSystem, new()
        {
            var systemPool = actor.World.GetSystemPool<T>();
            if (!systemPool.Has(actor.Id))
            {
                system = default; 
                return false;
            }
            system = systemPool.Get(actor.Id);
            return true;
        }

        /// <summary>
        /// Remove a system of type T from the specified actor.
        /// </summary>
        /// <typeparam name="T">The type of system.</typeparam>
        /// <param name="actor">The actor to remove the system from.</param>
        public static void RemoveSystem<T>(this Actor actor) where T : BaseSystem, new()
        {
            var systemPool = actor.World.GetSystemPool<T>();
            systemPool.Remove(actor.Id); 
        }
#endregion
    }
}