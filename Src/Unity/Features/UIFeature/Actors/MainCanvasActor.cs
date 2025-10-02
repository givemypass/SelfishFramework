using SelfishFramework.Src.Unity.UI.Components;
using UnityEngine;

namespace SelfishFramework.Src.Unity.Features.UI.Actors
{
    public partial class MainCanvasActor : Actor
    {
        [HideInInspector]
        public MainCanvasTagComponent MainCanvasTagComponent;

        protected override void BeforeInitialize()
        {
            base.BeforeInitialize();
            MainCanvasTagComponent.Value = GetComponent<Canvas>();
        }
    }
}