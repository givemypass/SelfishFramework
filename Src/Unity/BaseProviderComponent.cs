using System;
using SelfishFramework.Src.Core.Components;

namespace SelfishFramework.Src.Unity
{
    [Serializable]
    public struct BaseProviderComponent<T> : IComponent where T : UnityEngine.Component
    {
        public T Component;
    }
}