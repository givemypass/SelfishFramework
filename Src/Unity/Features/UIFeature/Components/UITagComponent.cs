using System;
using SelfishFramework.Src.Core.Components;

namespace SelfishFramework.Src.Unity.UI.Components
{
    [Serializable]
    public struct UITagComponent : IComponent
    {
        public int ID;
        public int GroupID;
    }
}