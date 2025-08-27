using System;
using System.Collections.Generic;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;

namespace SelfishFramework.Src.Core.SystemModules.CommandBusModule
{
    public class LocalCommandBus : IDisposable
    {
        private readonly Dictionary<int, HashSet<ISystem>> _map = new();

        public void Register(int entityId, ISystem system)
        {
            if (!_map.ContainsKey(entityId))
            {
                _map.Add(entityId, new ());
            } 
            _map[entityId].Add(system);
        }

        public void UnRegister(int entityId, ISystem system)
        {
            _map[entityId].Remove(system);
        }

        public void Invoke<T>(int entityId, T command) where T : ICommand
        {
            if (_map.TryGetValue(entityId, out var systems))
            {
                foreach (var system in systems)
                {
                    ((IReactLocal<T>)system).ReactLocal(command);
                }
            }  
        }

        public void Dispose()
        {
            _map.Clear();
        }
    }
}