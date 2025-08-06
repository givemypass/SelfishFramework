using System.Collections.Generic;
using SelfishFramework.Src.Core.Systems;

namespace SelfishFramework.Src.Core.Update
{
    public abstract class BaseSystemModule<T> : ISystemModule<T> where T : ISystemAction
    {
        protected readonly HashSet<T> executors = new();

        public void Register(T executor)
        {
            executors.Add(executor); 
        }

        public void Unregister(T executor)
        {
            executors.Remove(executor); 
        }

        public void TryRegister(ISystem system)
        {
            if (system is T executor)
            {
                executors.Add(executor);
            }
        }

        public void TryUnregister(ISystem system)
        {
            if(system is T executor)
            {
                executors.Remove(executor);
            }
        }

        public abstract void UpdateAll();
        public virtual void Dispose()
        {
            executors.Clear();
        }
    }
}