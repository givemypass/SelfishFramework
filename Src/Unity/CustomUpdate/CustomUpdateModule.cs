using System.Collections;
using System.Collections.Generic;
using SelfishFramework.Src.Core.DefaultUpdates;
using SelfishFramework.Src.Core.Systems;
using UnityEngine;

namespace SelfishFramework.Src.Unity.CustomUpdate
{
    public interface ICustomUpdatable : ISystemAction
    {
        YieldInstruction Interval { get; }
        void CustomUpdate();
    }
    
    public class CustomUpdateModule : ISystemModule<ICustomUpdatable>
    {
        private readonly Dictionary<ICustomUpdatable, Coroutine> _coroutines = new(8);
        private readonly MonoBehaviour _runner;

        public CustomUpdateModule(MonoBehaviour runner)
        {
            _runner = runner;  
        }
        
        private static IEnumerator Coroutine(ICustomUpdatable updatable)
        {
            while (true)
            {
                yield return updatable.Interval;
                updatable.CustomUpdate();
            }
        }

        public void TryRegister(ISystem system)
        {
            if (system is ICustomUpdatable executor)
            {
                Register(executor); 
            }
        }

        public void TryUnregister(ISystem system)
        {
            if (system is ICustomUpdatable executor)
            {
                Unregister(executor); 
            }
        }

        public void Register(ICustomUpdatable system)
        {
            if (_coroutines.ContainsKey(system))
                return;

            var coroutine = _runner.StartCoroutine(Coroutine(system));
            _coroutines.Add(system, coroutine); 
        }

        public void Unregister(ICustomUpdatable system)
        {
            if (!_coroutines.TryGetValue(system, out var coroutine))
                return;

            _runner.StopCoroutine(coroutine);
            _coroutines.Remove(system);
        }

        public void Dispose()
        {
            foreach (var kvp in _coroutines)
            {
                if (kvp.Value != null)
                    _runner.StopCoroutine(kvp.Value);
            }
        }
    }
}