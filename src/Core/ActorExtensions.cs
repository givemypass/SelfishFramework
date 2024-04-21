namespace SelfishFramework.Core
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
        public static ref T AddComponent<T>(this Actor actor) where T : struct, IComponent
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
        public static ref T GetComponent<T>(this Actor actor) where T : struct, IComponent
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
        public static bool ContainMask<T>(this Actor actor) where T : struct, IComponent
        {
            var pool = actor.World.GetComponentPool<T>();
            return pool.Has(actor.Id);
        } 
        /// <summary>
        /// Remove a component of type T to the specified actor.
        /// </summary>
        /// <typeparam name="T">The type of component to remove.</typeparam>
        /// <param name="actor">The actor to remove the component from. </param>
        public static void RemoveComponent<T>(this Actor actor) where T : struct, IComponent
        {
            var pool = actor.World.GetComponentPool<T>();
            pool.Remove(actor.Id);
        } 
#endregion
    }
}