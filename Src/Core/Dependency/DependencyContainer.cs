using System;
using System.Collections.Generic;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.SLogs;

namespace SelfishFramework.Src.Core.Dependency
{
    /// <summary>
    /// super simple dependency container, used for storing dependencies
    /// </summary>
    public class DependencyContainer
    {
        private readonly Dictionary<Type, object> _registry = new();
        private readonly ModuleRegistry _moduleRegistry;

        public DependencyContainer(ModuleRegistry moduleRegistry)
        {
            _moduleRegistry = moduleRegistry;
        }

        public T Get<T>() where T : class
        {
            var type = typeof(T);
            if (_registry.TryGetValue(type, out var instance))
            {
                return (T)instance;
            }

            throw new InvalidOperationException($"No known dependency of type = {type}");             
        }

        public void Resolve<T>(T instance) where T : class
        {
            if(instance is IInjectable injectable)
            {
                injectable.ResolveDependencies(this);
            }
        }

        public void Register<T1, T>(T instance) where T : class, T1
        {
            Resolve(instance);
            
            var type = typeof(T1);
            if (!_registry.TryAdd(type, instance))
            {
                SLog.LogError($"[DI] Dependency of type = {type} already exist");
            }
            _moduleRegistry.Register(instance);
        }

        public void Register<T>(T instance) where T : class
        {
            Register<T, T>(instance);
        }
    }
}