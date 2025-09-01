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
            foreach (var system in entity.Systems)
            {
                if (!world.TryGetSystemPool(system, out var pool))
                {
                    throw new Exception("System pool not found for system: " + system);
                }

                if (pool.Has(entity.Id))
                {
                    ((IAfterEntityInit)pool.GetRaw(entity.Id)).AfterEntityInit();
                }
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