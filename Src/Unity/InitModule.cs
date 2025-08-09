using System;
using UnityEngine;

namespace SelfishFramework.Src.Unity
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
}