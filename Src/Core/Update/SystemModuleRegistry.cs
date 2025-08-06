using System;
using System.Collections.Generic;
using SelfishFramework.Src.Core.Systems;

namespace SelfishFramework.Src.Core.Update
{
    public interface ISystemAction { }

    public interface ISystemModule : IDisposable
    {
        void TryRegister(ISystem system);
        void TryUnregister(ISystem system);
    }
    
    public interface ISystemModule<in T> : ISystemModule where T : ISystemAction
    {
        void Register(T system);
        void Unregister(T system);
    }
    
    public class SystemModuleRegistry : IDisposable
    {
        private readonly Dictionary<int, ISystemModule> _systemModules = new();

        public void RegisterModule<T>(T module) where T : ISystemModule
        {
            var index = SystemModuleIndex<T>.Index;
            if(!_systemModules.TryAdd(index, module))
            {
                throw new InvalidOperationException($"Module for type {typeof(T)} is already registered.");
            }
        }

        public void Register(ISystem system)
        {
            foreach (var module in _systemModules.Values)
            {
                module.TryRegister(system);
            }
        }
        
        public void Unregister(ISystem system)
        {
            foreach (var module in _systemModules.Values)
            {
                module.TryUnregister(system);
            }
        }

        public T GetModule<T>() where T : ISystemModule
        {
            var index = SystemModuleIndex<T>.Index;
            if (_systemModules.TryGetValue(index, out var rawModule))
                return (T)rawModule;
            
            throw new InvalidOperationException($"No module registered for type {typeof(T)}.");
        }
        
        public void Dispose()
        {
            _systemModules.Clear();     
        }
    }
}