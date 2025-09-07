using System;
using System.Collections.Generic;
using AssetsManagement;
using Cysharp.Threading.Tasks;
using SelfishFramework.Src.SLogs;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace SelfishFramework.Src.Unity.AssetsManagement
{
    public sealed class AssetsService
    {
        private const int MAX_RETRY_DELAY = 60;

        private readonly Dictionary<string, UniTask> _assetsLoadsMap = new();
        private readonly Dictionary<string, object> _assetsContainersCache = new();
        private readonly Dictionary<string, int> _containersRefsCount = new();

        private int _exceptionsCount;

        public async UniTask<AssetRefContainer<TRef, TObject>> GetContainer<TRef, TObject>(TRef reference)
            where TRef : AssetReference
            where TObject : Object
        {
            if (_assetsLoadsMap.TryGetValue(reference.AssetGUID, out var value))
            {
                await value;
            }
            else if (!_assetsContainersCache.ContainsKey(reference.AssetGUID))
            {
                await PreloadContainer<TRef, TObject>(reference);
            }

            _containersRefsCount[reference.AssetGUID]++;
            return _assetsContainersCache[reference.AssetGUID] as AssetRefContainer<TRef, TObject>;
        }

        public bool TryGetContainerFast<TRef, TObject>(TRef reference, out AssetRefContainer<TRef, TObject> container)
            where TRef : AssetReference
            where TObject : Object
        {
            if (_assetsContainersCache.TryGetValue(reference.AssetGUID, out var obj))
            {
                container = obj as AssetRefContainer<TRef, TObject>;
                return true;
            }

            container = default;
            return false;
        }

        public void ReleaseContainer<TRef, TObject>(AssetRefContainer<TRef, TObject> refContainer)
            where TRef : AssetReference
            where TObject : Object
        {
            TRef reference = refContainer.Reference;
            if (!_assetsContainersCache.ContainsKey(reference.AssetGUID))
            {
                SLog.LogError($"Cannot find container with provided ref {reference}");
                return;
            }

            _containersRefsCount[reference.AssetGUID]--;

            int assetsInstancesRefsCount = refContainer.RefsCount;
            int assetContainerRefsCount = _containersRefsCount[reference.AssetGUID];

            if (assetsInstancesRefsCount == 0 && assetContainerRefsCount == 0)
            {
                _containersRefsCount.Remove(reference.AssetGUID);
                _assetsContainersCache.Remove(reference.AssetGUID);
                reference.ReleaseAsset();
            }
        }

        private async UniTask PreloadContainer<TRef, TObject>(
            TRef reference,
            UniTaskCompletionSource loadingTCS = null
        )
            where TRef : AssetReference
            where TObject : Object
        {
            if (loadingTCS == null)
            {
                loadingTCS = new UniTaskCompletionSource();
                _assetsLoadsMap[reference.AssetGUID] = loadingTCS.Task.Preserve();
            }

            try
            {
                if(!reference.IsDone || !reference.IsValid())
                    await reference.LoadAssetAsync<TObject>().ToUniTask();

                _exceptionsCount = 0;
                var refContainer = new AssetRefContainer<TRef, TObject>(reference);
                _assetsContainersCache[reference.AssetGUID] = refContainer;
                _containersRefsCount[reference.AssetGUID] = 0;

                _assetsLoadsMap.Remove(reference.AssetGUID);
                loadingTCS.TrySetResult();
            }
            catch (Exception e)
            {
                SLog.LogException(e);
                _exceptionsCount++;
                var delayTime = Mathf.Clamp((int)Mathf.Pow(2, _exceptionsCount), 0, MAX_RETRY_DELAY) * 1000;
                await UniTask.Delay(delayTime);
                await PreloadContainer<TRef, TObject>(reference, loadingTCS);
            }
        }
    }
}