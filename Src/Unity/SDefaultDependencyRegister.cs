using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity.AssetsManagement;
using SelfishFramework.Src.Unity.Features.AssetsManagement;
using SelfishFramework.Src.Unity.Features.UI.Systems;

namespace SelfishFramework.Src.Unity
{
    public class SDefaultDependencyRegister : SDependencyRegister
    {
        protected override World World => SManager.World;

        public override void Register()
        {
            Container.Register<IAssetsService>(new AssetsService());
            Container.Register<IActorPoolingService>(new ActorPoolingService());
            Container.Register<IUIService>(new UIService(World));
        }       
    }
}