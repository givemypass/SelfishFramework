using System;
using System.Diagnostics.CodeAnalysis;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.SLogs;
using UnityEngine;

namespace SelfishFramework.Src.Unity
{
    public class Actor : MonoBehaviour, IDisposable
    {
        internal Entity _entity;
        public readonly InitModule InitMode = new();

        public Entity Entity => _entity;

        public void Init([NotNull]World world)
        {
            // World = world;
            // _entity = World.RegisterEntity(this);
            // IsInitted = true;
        }

        public void Dispose()
        {
            // if (!IsInitted)
            //     return;
            // World.UnregisterEntity(this);
            // IsInitted = false; 
        }

        private void Awake()
        {
            if (InitMode.InitWhen == InitModule.InitWhenMode.OnAwake)
            {
                if (_entity.IsInitted)
                {
                    SLog.LogError("Entity already initted");
                    return;
                }
                Init(SManager.Default);
            }       
        }

        private void Start()
        {
            if (InitMode.InitWhen == InitModule.InitWhenMode.OnStart)
            {
                if (_entity.IsInitted)
                {
                    SLog.LogError("Entity already initted");
                    return;
                }
                Init(SManager.Default);
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}