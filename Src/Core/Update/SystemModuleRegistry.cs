using System;
using System.Collections.Generic;

namespace SelfishFramework.Src.Core.Update
{
    public interface ISystemAction { }

    public interface ISystemModule
    {
        
    }
    
    public interface ISystemModule<T> : ISystemModule where T : ISystemAction
    {
        void ExecuteAll();
        void Register(T system);
        void Unregister(T system);
    }
    
    public class SystemModuleRegistry : IDisposable
    {
        private readonly Dictionary<Type, ISystemModule> _systemModules = new();

        public void RegisterModule<T>(ISystemModule module) where T : ISystemAction
        {
            if(!_systemModules.TryAdd(typeof(T), module))
            {
                throw new InvalidOperationException($"Module for type {typeof(T)} is already registered.");
            }
        }

        public void Register<T>(T system) where T : ISystemAction
        {
            GetModule<T>().Register(system);
        }
        
        public void Unregister<T>(T system) where T : ISystemAction
        {
            GetModule<T>().Unregister(system);
        }

        private ISystemModule<T> GetModule<T>() where T : ISystemAction
        {
            var type = typeof(T);
            if (_systemModules.TryGetValue(type, out var rawModule))
                return (ISystemModule<T>)rawModule;
            
            throw new InvalidOperationException($"No module registered for type {typeof(T)}.");
        }
        
        public void Dispose()
        {
            _systemModules.Clear();     
        }
    }
}