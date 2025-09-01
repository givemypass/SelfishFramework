using System;
using Cysharp.Threading.Tasks;
using SelfishFramework.Src.Unity.UI.Actors;

namespace SelfishFramework.Src.Unity.UI.Systems
{
    public sealed class UIService
    {
        public UIService()
        {
            
        }
        
        public async UniTask<UIActor> CreateUIAsync(int uiType, bool init = true, bool initSystems = true)
        {
            throw new NotImplementedException();
        }
    }
}