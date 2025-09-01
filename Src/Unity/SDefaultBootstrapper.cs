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

        private event Action OnPreUpdate;
        private event Action OnUpdate;
        private event Action OnFixedUpdate;
        private event Action OnLateUpdate;
        private event Action OnGlobalStart;
        private event Action OnLateStart;

        [SerializeField] private SDependencyRegister[] _dependencyRegisters;
        [SerializeField] private Actor[] _actorPrefabs;
        
        private void Awake()
        {
            _sManager = new SManager();

            foreach (var world in _sManager.Worlds)
            {
                OnPreUpdate += world.SystemModuleRegistry.GetModule<PreUpdateModule>().UpdateAll;
                OnUpdate += world.SystemModuleRegistry.GetModule<UpdateDefaultModule>().UpdateAll;
                OnFixedUpdate += world.SystemModuleRegistry.GetModule<FixedUpdateModule>().UpdateAll;
                OnGlobalStart += world.SystemModuleRegistry.GetModule<GlobalStartModule>().GlobalStartAll;
                OnLateStart += world.SystemModuleRegistry.GetModule<LateStartModule>().LateStartAll;

                var coroutineUpdateModule = new CoroutineUpdateModule(this);
                world.SystemModuleRegistry.RegisterModule(coroutineUpdateModule);
            }

            foreach (var register in _dependencyRegisters)
            {
                register.Register(); 
            }
            foreach (var prefab in _actorPrefabs)
            {
                var actor = Instantiate(prefab);
                actor.Init(SManager.World);
                actor.InitSystems();
            }
        }

        private void Start()
        {
            OnGlobalStart?.Invoke();
            OnLateStart?.Invoke();
        }

        private void Update()
        {
            OnPreUpdate?.Invoke();
            OnUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
            foreach (var world in _sManager.Worlds)
            {
                world.Commit();
            }
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