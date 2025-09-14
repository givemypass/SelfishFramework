using System.Linq;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity.CommonComponents;

namespace SelfishFramework.Src.Unity
{
    public static class ActorExtensions
    {
        public static Actor AsActor(this Entity entity)
        {
            return entity.Get<ActorProviderComponent>().Actor;
        }
        
        public static bool TryGetComponent<T>(this Actor actor, out T component, bool lookInChildsToo = false)
        {
            if (lookInChildsToo)
            {
                component = actor.GetComponentsInChildren<T>(true).FirstOrDefault();
                return component != null;
            }

            component = actor.GetComponent<T>();
            return component != null && component.ToString() != "null";
        }
    }
}