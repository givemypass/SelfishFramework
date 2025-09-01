using System;
using System.Collections.Generic;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;

namespace SelfishFramework.Src.Core.SystemModules.CommandBusModule
{
    public class LocalCommandModule : IModule
    {
        private readonly Dictionary<Type, LocalCommandBus> _buses = new();
            
        public int Priority => 0;

        public void TryRegister(object consumer)
        {
        }

        public void TryUnregister(object consumer)
        {
        }
        
        public void Register<T>(Entity entity, ISystem system) where T : ICommand
        {
            var type = typeof(T);
            if (!_buses.ContainsKey(type))
                _buses.Add(type, new LocalCommandBus());
            _buses[type].Register(entity.Id, system);
        }
        
        public void Unregister<T>(Entity entity, ISystem system) where T : ICommand
        {
            var type = typeof(T);
            _buses[type].UnRegister(entity.Id, system);
        }

        public void Invoke<T>(Entity entity, T command) where T : ICommand
        {
            var type = typeof(T);
            if (_buses.TryGetValue(type, out var bus))
            {
                bus.Invoke(entity.Id, command);
            }
        }
        
        public void Dispose()
        {
            foreach (var bus in _buses.Values)
            {
                bus.Dispose();
            }
            _buses.Clear();     
        }
    }
}