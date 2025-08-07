using System;
using System.Diagnostics.CodeAnalysis;
using SelfishFramework.Src.Core.Collections;
using SelfishFramework.Src.SLogs;
using UnityEngine;

namespace SelfishFramework.Src.Core
{
    //todo separate Actor and Entity to be able use ecs potential in bottlenecks
    [Serializable]
    public partial class Actor
    {
        [NonSerialized]
        public int Id;
        
        [NonSerialized]
        public int Generation;
        
        [NonSerialized]
        public bool IsInitted;

        public World World { get; private set; }
    }

    public partial class Actor : MonoBehaviour, IDisposable
    {
        [Serializable]
        public class InitModule
        {
            public enum InitWhenMode
            {
                Manually,
                OnAwake,
                OnStart
            }

            [SerializeField] private InitWhenMode initWhen;

            public InitWhenMode InitWhen
            {
                get => initWhen;
                set => initWhen = value;
            }
        }

        public InitModule InitMode = new();
        public void Init([NotNull]World world)
        {
            World = world;
            World.RegisterActor(this);
            IsInitted = true;
        }

        public void Dispose()
        {
            if (!IsInitted)
                return;
            World.UnregisterActor(this);
            IsInitted = false; 
        }

        private void Awake()
        {
            if (InitMode.InitWhen == InitModule.InitWhenMode.OnAwake)
            {
                if (IsInitted)
                {
                    SLog.LogError("Actor already initted");
                    return;
                }
                Init(SManager.Default);
            }       
        }

        private void Start()
        {
            if (InitMode.InitWhen == InitModule.InitWhenMode.OnStart)
            {
                if (IsInitted)
                {
                    SLog.LogError("Actor already initted");
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