using System;
using System.Diagnostics.CodeAnalysis;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.Unity.Components;
using UnityEngine;

namespace SelfishFramework.Src.Unity
{
    public abstract class Actor : MonoBehaviour, IDisposable
    {
        private Entity _entity;
        public InitModule InitMode = new();

        public Entity Entity => _entity;

        public void SetEntity([NotNull]World world)
        {
            _entity = world.NewEntity();
            SetComponents();
            SetSystems();
        }

        public void InitEntity()
        {
            _entity.Init();
        }

        public void Dispose()
        {
            if (SManager.IsAlive)
            {
                _entity.GetWorld().DelEntity(_entity);
                _entity = default;
            }
        }

        private void Awake()
        {
            if (InitMode.InitWhen != InitModule.InitWhenMode.OnAwake)
                return;
            TryInitialize();
        }

        private void Start()
        {
            if (InitMode.InitWhen != InitModule.InitWhenMode.OnStart)
                return;
            TryInitialize();
        }

        private void TryInitialize()
        {
            var world = SManager.World;
            if (world.IsDisposed(_entity))
            {
                SetEntity(world);
                InitEntity();
                return;
            }

            SLog.LogError("Entity already initialized");
        }

        protected virtual void SetComponents()
        {
            _entity.Set(new ActorProviderComponent
            {
                Actor = this,
            }); 
        }

        protected virtual void SetSystems()
        {
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}