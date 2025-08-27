using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity.Components;

namespace SelfishFramework.Src.Unity
{
    public static class ActorExtensions
    {
        public static Actor AsActor(this Entity entity)
        {
            return entity.Get<ActorProviderComponent>().Actor;
        }
    }
}