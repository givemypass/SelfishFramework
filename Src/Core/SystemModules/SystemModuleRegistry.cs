using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SelfishFramework.Src.Core.Systems;

namespace SelfishFramework.Src.Core.SystemModules
{
    public interface ISystemAction { }

    public interface ISystemModule : IDisposable
    {
        int Priority { get; }
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
        private readonly List<int> _modulesRegistrationOrder = new();
        private readonly Dictionary<int, ISystemModule> _systemModules = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RegisterModule<T>(T module) where T : ISystemModule
        {
            var moduleIndex = SystemModuleIndex<T>.Index;
            if(!_systemModules.TryAdd(moduleIndex, module))
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
        public void Register(ISystem system)
        {
            foreach (var moduleIndex in _modulesRegistrationOrder)
            {
                if (_systemModules.TryGetValue(moduleIndex, out var module))
                {
                    module.TryRegister(system);
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unregister(ISystem system)
        {
            foreach (var moduleIndex in _modulesRegistrationOrder)
            {
                if (_systemModules.TryGetValue(moduleIndex, out var module))
                {
                    module.TryRegister(system);
                }
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetModule<T>() where T : ISystemModule
        {
            var index = SystemModuleIndex<T>.Index;
            return (T)GetModule(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ISystemModule GetModule(int index)
        {
            if (_systemModules.TryGetValue(index, out var module))
                return module;
            
            throw new InvalidOperationException($"No module registered for type {index}.");
        }
        
        public void Dispose()
        {
            foreach (var module in _systemModules.Values)
            {
                module.Dispose(); 
            }
            _systemModules.Clear();     
        }
    }
}