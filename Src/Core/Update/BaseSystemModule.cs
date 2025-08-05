using SelfishFramework.Src.Core.Collections;

namespace SelfishFramework.Src.Core.Update
{
    public abstract class BaseSystemModule<T>
    {
        protected readonly FastList<T> systems = new();
        public void Register(T updatable)
        {
            systems.Add(updatable); 
        }

        public void Unregister(T updatable)
        {
            systems.Remove(updatable); 
        }
    }
}