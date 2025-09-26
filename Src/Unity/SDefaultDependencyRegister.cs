using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity.AssetsManagement;
using SelfishFramework.Src.Unity.UI.Systems;

namespace SelfishFramework.Src.Unity
{
    public class SDefaultDependencyRegister : SDependencyRegister
    {
        protected override World World => SManager.World;

        public override void Register()
        {
            Container.Register(new AssetsService());
            Container.Register(new ActorPoolingService());
            Container.Register(new UIService(World));
        }       
    }
}