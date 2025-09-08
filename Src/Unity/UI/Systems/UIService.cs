using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.Unity.AssetsManagement;
using SelfishFramework.Src.Unity.UI.Actors;
using SelfishFramework.Src.Unity.UI.Components;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SelfishFramework.Src.Unity.UI.Systems
{
    [Injectable]
    public sealed partial class UIService
    {
        public const string UI_BLUE_PRINT = "UIBluePrint";
        
        [Inject] private ActorPoolingService _actorPoolingService;
        
        private readonly World _world;
        private readonly Single<MainCanvasTagComponent> _mainCanvas;
        private readonly Filter _additionalCanvasFilter;
        private readonly Filter _currentUIFilter;
        
        private readonly List<UIBluePrint> _uIBluePrints = new();
        
        private bool _isReady;

        public UIService(World world)
        {
            _world = world;
            _mainCanvas = new Single<MainCanvasTagComponent>(world);
            _currentUIFilter = world.Filter.With<UITagComponent>().Build();
            _additionalCanvasFilter = world.Filter.With<AdditionalCanvasTagComponent>().Build();
            Addressables.LoadAssetsAsync<UIBluePrint>(UI_BLUE_PRINT, null).Completed += LoadReact;
        }

        public async UniTask<UIActor> ShowUIAsync(int uiId, bool initSystems = true, int additionalCanvas = 0)
        {
            if (!_isReady)
            {
                await UniTask.WaitUntil(this, IsReadyPredicate);
            }

            var uIBluePrint = _uIBluePrints.Find(x => x.UIType == uiId);
            if (uIBluePrint == null)
            {
                SLog.LogError($"UI BluePrint with type {uiId} not found.");
                return null;
            }

            Transform canvas = null;
            if (additionalCanvas == 0)
            {
                _mainCanvas.ForceUpdate();
                canvas = _mainCanvas.Get().Value.transform;
            }
            else
            {
                _additionalCanvasFilter.ForceUpdate();
                foreach (var entity in _additionalCanvasFilter)
                {
                    var additionalCanvasComponent = entity.Get<AdditionalCanvasTagComponent>();
                    if (additionalCanvasComponent.Identifier == additionalCanvas)
                    {
                        canvas = additionalCanvasComponent.Value.transform;
                        break;
                    }
                }
            }
            
            if(canvas == null)
            {
                SLog.LogError($"Canvas for {uiId} not found.");
                return null;
            }

            var actor = await _actorPoolingService.GetActorAsync<UIActor>(uIBluePrint.UIActor, initSystems, canvas, _world.Index);
            actor.Entity.Set(new UITagComponent
            {
                ID = uiId,
            });
            return actor;
        }
        
        public void CloseUI(UIActor actor)
        {
            if (actor == null)
            {
                SLog.LogError("Cannot close null UI actor.");
                return;
            }
            
            _actorPoolingService.ReleaseActor(actor);
        }

        public void CloseUI(int uiId)
        {
            _currentUIFilter.ForceUpdate();
            foreach (var entity in _currentUIFilter)
            {
                if (entity.Get<UITagComponent>().ID != uiId) 
                    continue;
                
                var actor = (UIActor)entity.AsActor();
                CloseUI(actor);
            }
        }

        private void LoadReact(AsyncOperationHandle<IList<UIBluePrint>> obj)
        {
            foreach (var bp in obj.Result)
            {
                _uIBluePrints.Add(bp); 
            }

            _isReady = true;
        }

        private static bool IsReadyPredicate(UIService state)
        {
            return state._isReady;
        }
    }
}