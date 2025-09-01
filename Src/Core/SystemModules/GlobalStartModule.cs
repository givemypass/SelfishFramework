
using System.Collections.Generic;
using SelfishFramework.Src.Core.Systems;

namespace SelfishFramework.Src.Core.SystemModules
{
    public interface IGlobalStart : ISystemAction
    {
        void GlobalStart();
    }
    
    public class GlobalStartModule : IModule<IGlobalStart>
    {
        private readonly HashSet<IGlobalStart> _globalStarts = new();
        private bool _isGlobalStarted;

        public int Priority => 200;
        
        public void GlobalStartAll()
        {
            foreach (var globalStart in _globalStarts)
            {
                globalStart.GlobalStart();
            }
            _globalStarts.Clear();
            _isGlobalStarted = true;
        }
        
        public void TryRegister(object consumer)
        {
            if (consumer is not IGlobalStart globalStart)
            {
                return;
            }
            
            if (_isGlobalStarted)
            {
                globalStart.GlobalStart();
                return;
            }
            Register(globalStart);
        }

        public void TryUnregister(object consumer)
        {
            if (consumer is not IGlobalStart globalStart)
            {
                return;
            }

            if (_isGlobalStarted)
            {
                globalStart.GlobalStart();
                return;
            }
            Unregister(globalStart);
        }

        public void Register(IGlobalStart system)
        {
            _globalStarts.Add(system);
        }

        public void Unregister(IGlobalStart system)
        {
            _globalStarts.Remove(system);
        }

        public void Dispose()
        {
            _globalStarts.Clear();
        }
    }
}