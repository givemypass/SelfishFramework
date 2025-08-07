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
        private event Action OnGlobalStart;
        
        private void Awake()
        {
            _sManager = new SManager();

            var world = _sManager.World;
            var updateDefaultModule = new UpdateDefaultModule();
            OnUpdate += updateDefaultModule.UpdateAll;
            world.SystemModuleRegistry.RegisterModule(updateDefaultModule);
            
            var fixedUpdateModule = new FixedUpdateModule();
            OnFixedUpdate += fixedUpdateModule.UpdateAll;
            world.SystemModuleRegistry.RegisterModule(fixedUpdateModule);
            
            var customUpdateModule = new CustomUpdateModule(this);
            world.SystemModuleRegistry.RegisterModule(customUpdateModule);
            
            var globalStartModule = new GlobalStartModule();
            OnGlobalStart += globalStartModule.GlobalStartAll;
            world.SystemModuleRegistry.RegisterModule(globalStartModule);
        }

        private void Start()
        {
            OnGlobalStart?.Invoke();
        }

        private void Update()
        {
            OnUpdate?.Invoke();
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