using System;
using SelfishFramework.Src.Core.Components;
using UnityEngine;

namespace SelfishFramework.Src.Unity.CommonComponents
{
    [Serializable]
    public struct CameraProviderComponent : IComponent
    {
        public Camera Camera;
    }
}