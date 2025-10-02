using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SelfishFramework.Src.Unity.UI
{
    [CreateAssetMenu(fileName = "UIBluePrint", menuName = "BluePrints/UIBluePrint")]
    public class UIBluePrint : ScriptableObject
    {
        public AssetReference UIActor;
        public UIIdentifier UIType;
    }
}