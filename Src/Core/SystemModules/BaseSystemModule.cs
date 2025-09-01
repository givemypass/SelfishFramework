using System.Collections.Generic;
using SelfishFramework.Src.Core.Systems;

namespace SelfishFramework.Src.Core.SystemModules
{
    public abstract class BaseModule<T> : IModule<T> where T : ISystemAction
    {
        protected readonly HashSet<T> executors = new();

        public abstract int Priority { get; }
        
        public void Register(T executor)
        {
            executors.Add(executor); 
        }

        public void Unregister(T executor)
        {
            executors.Remove(executor); 
        }

        public void TryRegister(object consumer)
        {
            if (consumer is T executor)
            {
                executors.Add(executor);
            }
        }

        public void TryUnregister(object consumer)
        {
            if(consumer is T executor)
            {
                executors.Remove(executor);
            }
        }

        public virtual void Dispose()
        {
            executors.Clear();
        }
    }
}