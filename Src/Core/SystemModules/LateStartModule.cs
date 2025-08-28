using System.Collections.Generic;
using SelfishFramework.Src.Core.Systems;

namespace SelfishFramework.Src.Core.SystemModules
{
    public interface ILateStart : ISystemAction
    {
        public void LateStart();
    }
    
    public class LateStartModule : ISystemModule<ILateStart>
    {
        private readonly HashSet<ILateStart> _systems = new();
        private bool _isLateStarted;
 
        public int Priority => 300;
         
        public void LateStartAll()
        {
            foreach (var globalStart in _systems)
            {
                globalStart.LateStart();
            }
            _systems.Clear();
            _isLateStarted = true;
        }
         
        public void TryRegister(ISystem system)
        {
            if (system is not ILateStart lateStart)
            {
                return;
            }
             
            if (_isLateStarted)
            {
                lateStart.LateStart();
                return;
            }
            Register(lateStart);
        }
 
        public void TryUnregister(ISystem system)
        {
            if (system is not ILateStart lateStart)
            {
                return;
            }
 
            if (_isLateStarted)
            {
                lateStart.LateStart();
                return;
            }
            Unregister(lateStart);
        }
 
        public void Register(ILateStart system)
        {
            _systems.Add(system);
        }
 
        public void Unregister(ILateStart system)
        {
            _systems.Remove(system);
        }
 
        public void Dispose()
        {
            _systems.Clear();
        }   
    }
}