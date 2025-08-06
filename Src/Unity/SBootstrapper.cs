using System;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.DefaultUpdates;
using SelfishFramework.Src.Unity.CustomUpdate;
using UnityEngine;

namespace SelfishFramework.Src.Unity
{
    public class SBootstrapper : MonoBehaviour
    {
        private ActorsManager _actorsManager;
        
        private event Action OnUpdate;
        
        private event Action OnFixedUpdate;
        
        private void Awake()
        {
            _actorsManager = new ActorsManager();

            var world = _actorsManager.World;
            var updateDefaultModule = new UpdateDefaultModule();
            OnUpdate += updateDefaultModule.UpdateAll;
            world.SystemModuleRegistry.RegisterModule(updateDefaultModule);
            
            var fixedUpdateModule = new FixedUpdateModule();
            OnFixedUpdate += fixedUpdateModule.UpdateAll;
            world.SystemModuleRegistry.RegisterModule(fixedUpdateModule);
            
            var customUpdateModule = new CustomUpdateModule(this);
            world.SystemModuleRegistry.RegisterModule(customUpdateModule);
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
            _actorsManager.Dispose();
        }
    }
}