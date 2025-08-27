using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.Components;
using SelfishFramework.Src.Unity.Systems;

namespace SelfishFramework.Src.Core.Actors
{
    public partial class InputManagerActor : Actor
    {
        public InputActionsComponent InputActionsComponent = new();
        protected override void SetSystems()
        {
            base.SetSystems();
            Entity.AddSystem<InputListenSystem>();
        }
    }
}