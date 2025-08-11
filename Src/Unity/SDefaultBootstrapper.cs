using System;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Unity.CustomUpdate;
using UnityEngine;

namespace SelfishFramework.Src.Unity
{
    public class SDefaultBootstrapper : MonoBehaviour
    {
        private SManager _sManager;

        private event Action OnUpdate;
        private event Action OnFixedUpdate;
        private event Action OnLateUpdate;
        private event Action OnGlobalStart;
        
        private void Awake()
        {
            _sManager = new SManager();

            var world = _sManager.World;
            
            OnUpdate += world.SystemModuleRegistry.GetModule<UpdateDefaultModule>().UpdateAll;
            OnFixedUpdate += world.SystemModuleRegistry.GetModule<FixedUpdateModule>().UpdateAll;
            OnGlobalStart += world.SystemModuleRegistry.GetModule<GlobalStartModule>().GlobalStartAll;
            
            var coroutineUpdateModule = new CoroutineUpdateModule(this);
            world.SystemModuleRegistry.RegisterModule(coroutineUpdateModule);
        }

        private void Start()
        {
            OnGlobalStart?.Invoke();
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
            _sManager.World.Commit();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        private void OnDestroy()
        {
            _sManager.Dispose();
        }
    }
}