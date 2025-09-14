using System;
using SelfishFramework.Src.Core.Components;
using UnityEngine.AddressableAssets;

namespace SelfishFramework.Src.Unity.AssetsManagement.Components
{
    [Serializable]
    public struct AssetReferenceComponent : IComponent
    {
        public AssetReference Reference;
    }
}