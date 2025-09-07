using System;
using SelfishFramework.Src.Core.Systems;

namespace SelfishFramework.Src.Core.SystemModules
{
    public interface IAfterEntityInit : ISystemAction
    {
        void AfterEntityInit();
    }

    public class AfterEntityInitModule : IModule<IAfterEntityInit>
    {
        public int Priority => 100;

        public void TryRegister(object consumer)
        {
            if (consumer is not (ISystem system and IAfterEntityInit afterEntityInit))
            {
                return;
            }

            if (system.Owner.IsInitialized())
            {
                afterEntityInit.AfterEntityInit();
            }
        }

        public void TryUnregister(object consumer)
        {
        }

        public void Run(Entity entity)
        {
            var world = entity.GetWorld();
            foreach (var systemId in entity.Systems)
            {
                if (!world.TryGetSystemPool(systemId, out var pool))
                {
                    throw new Exception("System pool not found for system: " + systemId);
                }

                var system = pool.GetRaw(entity.Id);
                if(system is not IAfterEntityInit afterEntityInit)
                {
                    continue;
                }
                afterEntityInit.AfterEntityInit();
            }
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