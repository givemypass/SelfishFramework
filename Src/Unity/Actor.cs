using System;
using System.Diagnostics.CodeAnalysis;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.SLogs;
using UnityEngine;

namespace SelfishFramework.Src.Unity
{
    public abstract class Actor : MonoBehaviour, IDisposable
    {
        private Entity _entity;
        public readonly InitModule InitMode = new();

        public Entity Entity => _entity;

        public void Init([NotNull]World world)
        {
            _entity = world.NewEntity();
            SetComponents();
            SetSystems();
        }

        public void Dispose()
        {
            if (!_entity.GetWorld().IsDisposed(_entity))
            {
                SManager.World.DelEntity(_entity);
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
                Init(world);
                return;
            }

            SLog.LogError("Entity already initialized");
        }

        protected abstract void SetComponents();
        protected abstract void SetSystems();

        private void OnDestroy()
        {
            Dispose();
        }
    }
}