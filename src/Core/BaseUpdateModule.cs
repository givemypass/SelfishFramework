using SelfishFramework.Core.Collections;

namespace SelfishFramework.Core
{
    public abstract class BaseUpdateModule<T>
    {
        protected FastList<T> updatables = new();
        public void Register(T updatable)
        {
            updatables.Add(updatable); 
        }

        public void Unregister(T updatable)
        {
            updatables.Remove(updatable); 
        }
    }

    public class UpdateDefaultModule : BaseUpdateModule<IUpdatable>
    {
        public void Update()
        {
            foreach (var updatable in updatables)
            {
                updatable.Update();
            } 
        }
    }
}