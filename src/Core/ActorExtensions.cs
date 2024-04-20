namespace SelfishFramework.Core
{
    public static class ActorExtensions
    {
        public static ref T Add<T>(this Actor actor) where T : struct, IComponent
        {
            var pool = actor.World.GetComponentPool<T>();
            return ref pool.Add(actor.Id);
        } 
        
        public static ref T Get<T>(this Actor actor) where T : struct, IComponent
        {
            var pool = actor.World.GetComponentPool<T>();
            return ref pool.Get(actor.Id);
        } 
        public static bool Has<T>(this Actor actor) where T : struct, IComponent
        {
            var pool = actor.World.GetComponentPool<T>();
            return pool.Has(actor.Id);
        } 
        public static void Remove<T>(this Actor actor) where T : struct, IComponent
        {
            var pool = actor.World.GetComponentPool<T>();
            pool.Remove(actor.Id);
        } 
    }
}