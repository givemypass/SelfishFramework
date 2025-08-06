using System;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Update;
using UnityEngine;

namespace SelfishFramework.Src.Unity
{
    public class SBootstrapper : MonoBehaviour
    {
        private event Action OnUpdate;
        private event Action OnFixedUpdate;
        
        private void Awake()
        {
            ActorsManager.RecreateInstance();
            
            foreach (var world in ActorsManager.Worlds)
            {
                var updateDefaultModule = new UpdateDefaultModule();
                OnUpdate += updateDefaultModule.UpdateAll;
                world?.SystemModuleRegistry.RegisterModule(updateDefaultModule);
                
                var fixedUpdateModule = new FixedUpdateModule();
                OnFixedUpdate += fixedUpdateModule.UpdateAll;
                world?.SystemModuleRegistry.RegisterModule(fixedUpdateModule);
            }
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }
    }
}