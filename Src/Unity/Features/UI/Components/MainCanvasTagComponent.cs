using System;
using SelfishFramework.Src.Core.Components;
using UnityEngine;

namespace SelfishFramework.Src.Unity.UI.Components
{
    [Serializable]
    public struct MainCanvasTagComponent : IComponent
    {
        public Canvas Value;
    }
}