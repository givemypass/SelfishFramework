using System;
using System.Collections.Generic;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;

namespace SelfishFramework.Src.Core.SystemModules.CommandBusModule
{
    public class GlobalCommandModule : IModule
    {
        private readonly Dictionary<Type, HashSet<object>> _buses = new();
        
        public int Priority => 0;

        public void TryRegister(object consumer)
        {
        }

        public void TryUnregister(object consumer)
        {
        }
        
        public void Register<T>(IReactGlobal<T> system) where T : IGlobalCommand
        {
            var type = typeof(T);
            if (!_buses.ContainsKey(type))
                _buses.Add(type, new ());
            _buses[type].Add(system);
        }
        
        public void Unregister<T>(IReactGlobal<T> system) where T : IGlobalCommand
        {
            var type = typeof(T);
            _buses[type].Remove(system);
        }

        public void Invoke<T>(T command) where T : IGlobalCommand
        {
            var type = typeof(T);
            if (_buses.TryGetValue(type, out var bus))
            {
                foreach (var system in bus)
                {
                    ((IReactGlobal<T>)system).ReactGlobal(command); 
                }
            }
        }

        public void Dispose()
        {
            
        }
    }
}