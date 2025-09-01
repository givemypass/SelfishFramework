using System;
using System.Collections.Generic;
using SelfishFramework.Src.SLogs;

namespace SelfishFramework.Src.Core.Dependency
{
    /// <summary>
    /// super simple dependency container, used for storing dependencies
    /// </summary>
    public class DependencyContainer
    {
        private readonly Dictionary<Type, object> _registry = new();

        public T Get<T>() where T : class
        {
            var type = typeof(T);
            if (_registry.TryGetValue(type, out var instance))
            {
                return (T)instance;
            }

            throw new InvalidOperationException($"No known dependency of type = {type}");             
        }

        public void Register<T>(T instance) where T : class
        {
            var type = typeof(T);
            if (!_registry.TryAdd(type, instance))
            {
                SLog.LogError($"[DI] Dependency of type = {type} already exist");
            }
        }
    }
}