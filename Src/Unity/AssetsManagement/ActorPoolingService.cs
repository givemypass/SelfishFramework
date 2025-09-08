using Cysharp.Threading.Tasks;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Attributes;
using SelfishFramework.Src.SLogs;
using SelfishFramework.Src.Unity.AssetsManagement.Components;
using SelfishFramework.Src.Unity.UI.Actors;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SelfishFramework.Src.Unity.AssetsManagement
{
    [Injectable]
    public sealed partial class ActorPoolingService
    {
        [Inject] private AssetsService _assetsService;
        
        public async UniTask<TActor> GetActorAsync<TActor>(AssetReference reference, bool initSystems = true, Transform parent = null, int worldId = 0)
            where TActor : Actor
        {
            var container = await _assetsService.GetContainer<AssetReference, GameObject>(reference);
            var actor = await container.CreateInstanceForComponent<TActor>(parent);
            _assetsService.ReleaseContainer(container);
            actor.SetEntity(SManager.GetWorld(worldId));
            actor.Entity.Set(new AssetReferenceComponent
            {
                Reference = reference,
            });
            if (initSystems)
            {
                actor.InitEntity();
            }
            return actor;
        }
        
        public void ReleaseActor<TActor>(TActor actor)
            where TActor : Actor
        {
            if (actor == null)
            {
                SLog.LogError("Cannot release null actor.");
                return;
            }
            
            if (!actor.Entity.Has<AssetReferenceComponent>())
            {
                SLog.LogError($"Actor {actor} has no AssetReferenceComponent.");
                return;
            }
            
            ref var referenceComponent = ref actor.Entity.Get<AssetReferenceComponent>();
            if (referenceComponent.Reference == null)
            {
                SLog.LogError($"Actor {actor} has no reference in AssetReferenceComponent.");
                return;
            }
            
            var container = _assetsService.TryGetContainerFast<AssetReference, GameObject>(referenceComponent.Reference, out var assetContainer);
            if (!container)
            {
                SLog.LogError($"Cannot find container for actor {actor} with reference {referenceComponent.Reference}");
                return;
            }
            
            assetContainer.ReleaseInstance(actor.gameObject);
        }
    }
}