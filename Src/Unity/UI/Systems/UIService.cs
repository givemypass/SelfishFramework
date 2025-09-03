using System;
using System.Collections.Generic;
using AssetsManagement;
using Cysharp.Threading.Tasks;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Unity.UI.Actors;
using SelfishFramework.Src.Unity.UI.Components;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SelfishFramework.Src.Unity.UI.Systems
{
    [Injectable]
    public sealed partial class UIService
    {
        private const string UI_BLUE_PRINT = "UIBluePrint";
        
        [Inject] private AssetsService _assetsService;
        
        private readonly World _world;
        
        private readonly Single<MainCanvasTagComponent> _mainCanvas;
        private readonly Filter _additionalCanvasFilter;

        public UIService(World world)
        {
            // _world = world;
            // _mainCanvas = new Single<MainCanvasTagComponent>(world);
            // _additionalCanvasFilter = world.Filter.With<MainCanvasTagComponent>().Build();
            // Addressables.LoadAssetsAsync<UIBluePrint>(UI_BLUE_PRINT, null).Completed += LoadReact;
        }

        public async UniTask<UIActor> CreateUIAsync(int uiType, bool init = true, bool initSystems = true, int additionalCanvas = 0)
        {
            throw new NotImplementedException();
        }

        private void LoadReact(AsyncOperationHandle<IList<UIBluePrint>> obj)
        {
            
        }
    }
}