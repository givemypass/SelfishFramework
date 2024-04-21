using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace SelfishFramework.Core
{
    [Serializable]
    public partial class Actor
    {
        public int Id;
        public int Generation;
        public World World { get; private set; }

        //systems
    }

    public partial class Actor : MonoBehaviour
    {
        [Serializable]
        private class InitModule
        {
            public enum InitWhenMode
            {
                Manually,
                OnAwake,
                OnStart
            }

            [SerializeField] private InitWhenMode initWhen;
            [SerializeField] private int worldIndex;

            public InitWhenMode InitWhen => initWhen;
            public int WorldIndex => worldIndex;
        }

        [SerializeField] private InitModule initModule = new InitModule();
        public void Init([NotNull]World world)
        {
            World = world;
            World.RegisterActor(this); 
        }

        private void Awake()
        {
            if (initModule.InitWhen == InitModule.InitWhenMode.OnAwake)
            {
                Init(ActorsManager.Worlds[initModule.WorldIndex]);
            }       
        }

        private void Start()
        {
            if (initModule.InitWhen == InitModule.InitWhenMode.OnStart)
            {
                Init(ActorsManager.Worlds[initModule.WorldIndex]);
            }
        }

        private void OnDestroy()
        {
            World?.UnregisterActor(this);
            World = default;
        }
    }

}