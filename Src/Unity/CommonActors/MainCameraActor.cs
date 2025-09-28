
using SelfishFramework.Src.Unity.CommonComponents;
using UnityEngine;

namespace SelfishFramework.Src.Unity.CommonActors
{
    public partial class MainCameraActor : Actor
    {
        private CameraProviderComponent _cameraProviderComponent = new();

        protected override void BeforeInitialize()
        {
            base.BeforeInitialize();
            _cameraProviderComponent.Camera = GetComponent<Camera>();
        }
    }
}