
using SelfishFramework.Src.Unity.CommonComponents;
using UnityEngine;

namespace SelfishFramework.Src.Unity.CommonActors
{
    public partial class MainCameraActor : Actor
    {
        private MainCameraTagComponent _mainCameraTagComponent = new();

        protected override void BeforeInitialize()
        {
            base.BeforeInitialize();
            _mainCameraTagComponent.Camera = GetComponent<Camera>();
        }
    }
}