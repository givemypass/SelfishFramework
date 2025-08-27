using System;
using SelfishFramework.Src.Core.Components;

namespace SelfishFramework.Src.Unity.Components
{
    [Serializable]
    public struct ActorProviderComponent : IComponent
    {
        public Actor Actor;
    }
}