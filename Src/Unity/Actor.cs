using System;
using System.Diagnostics.CodeAnalysis;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.SLogs;
using UnityEngine;

namespace SelfishFramework.Src.Unity
{
    public class Actor : MonoBehaviour, IDisposable
    {
        private Entity _entity;
        public readonly InitModule InitMode = new();

        public Entity Entity => _entity;

        public void Init([NotNull]World world)
        {
            _entity = world.NewEntity();
        }

        public void Dispose()
        {
            if (!SManager.Default.IsDisposed(_entity))
            {
                SManager.Default.DelEntity(_entity);
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
            var world = SManager.Default;
            if (world.IsDisposed(_entity))
            {
                Init(world);
                return;
            }

            SLog.LogError("Entity already initialized");
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}