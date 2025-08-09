using System.Collections.Generic;
using SelfishFramework.Src.Core.Systems;
using UnityEngine.UIElements;

namespace SelfishFramework.Src.Core.SystemModules
{
    public interface IAfterEntityInit : ISystemAction
    {
        void AfterEntityInit();
    }

    public class AfterEntityInitModule : ISystemModule<IAfterEntityInit>
    {
        private readonly HashSet<Entity> _entities = new();
        public int Priority { get; } = 100;

        public void TryRegister(ISystem system)
        {
            if (system is not IAfterEntityInit afterEntityInit)
            {
                return;
            }

            // if (system.Owner.IsInitted)
            // {
            //     afterEntityInit.AfterEntityInit();
            //     return;
            // }

            // Register(system.Owner.Id);
        }

        public void TryUnregister(ISystem system)
        {
            if (system is not IAfterEntityInit afterEntityInit)
            {
                return;
            }
            
            Unregister(afterEntityInit);
        }

        public void Register(IAfterEntityInit system)
        {
        }

        public void Unregister(IAfterEntityInit system)
        {
        }

        public void Dispose()
        {
        }
    }
}