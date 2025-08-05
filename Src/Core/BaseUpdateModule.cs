using SelfishFramework.Src.Core.Collections;

namespace SelfishFramework.Src.Core
{
    public abstract class BaseUpdateModule<T>
    {
        protected readonly FastList<T> updatables = new();
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
            for (var i = 0; i < updatables.Count; i++)
            {
                updatables[i].Update();
            }
        }
    }
}