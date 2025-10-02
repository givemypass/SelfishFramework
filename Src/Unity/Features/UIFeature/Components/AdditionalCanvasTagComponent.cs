using System;
using SelfishFramework.Src.Core.Components;
using SelfishFramework.Src.Unity.Identifiers;
using UnityEngine;

namespace SelfishFramework.Src.Unity.UI.Components
{
    [Serializable]
    public struct AdditionalCanvasTagComponent : IComponent
    {
        public AdditionalCanvasIdentifier Identifier;
        public Canvas Value;
    }
}