using System;
using SelfishFramework.Src.Core.Components;

namespace SelfishFramework.Src.Unity.CommonComponents
{
    [Serializable]
    public struct ActorProviderComponent : IComponent
    {
        public Actor Actor;
    }
}