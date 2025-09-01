using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SelfishFramework.Src.Core.Systems;

namespace SelfishFramework.Src.Core.SystemModules
{
    public interface ISystemAction { }

    public interface IModule : IDisposable
    {
        int Priority { get; }
        void TryRegister(object consumer);
        void TryUnregister(object consumer);
    }
    
    public interface IModule<in T> : IModule where T : ISystemAction
    {
        void Register(T system);
        void Unregister(T system);
    }
    
    public class ModuleRegistry : IDisposable
    {
        private readonly List<int> _modulesRegistrationOrder = new();
        private readonly Dictionary<int, IModule> _modules = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RegisterModule<T>(T module) where T : IModule
        {
            var moduleIndex = ModuleIndex<T>.Index;
            if(!_modules.TryAdd(moduleIndex, module))
            {
                throw new InvalidOperationException($"Module for type {typeof(T)} is already registered.");
            }

            //todo create priority order custom type and move it into one + optimize(use a tree?)
            var indexToInsert = _modulesRegistrationOrder.FindIndex(a => a > module.Priority);
            if (indexToInsert < 0)
            {
                _modulesRegistrationOrder.Add(moduleIndex);
            }
            else
            {
                _modulesRegistrationOrder.Insert(indexToInsert, moduleIndex);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Register(object consumer)
        {
            foreach (var moduleIndex in _modulesRegistrationOrder)
            {
                if (_modules.TryGetValue(moduleIndex, out var module))
                {
                    module.TryRegister(consumer);
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unregister(object consumer)
        {
            foreach (var moduleIndex in _modulesRegistrationOrder)
            {
                if (_modules.TryGetValue(moduleIndex, out var module))
                {
                    module.TryRegister(consumer);
                }
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetModule<T>() where T : IModule
        {
            var index = ModuleIndex<T>.Index;
            return (T)GetModule(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IModule GetModule(int index)
        {
            if (_modules.TryGetValue(index, out var module))
                return module;
            
            throw new InvalidOperationException($"No module registered for type {index}.");
        }
        
        public void Dispose()
        {
            foreach (var module in _modules.Values)
            {
                module.Dispose(); 
            }
            _modules.Clear();     
        }
    }
}