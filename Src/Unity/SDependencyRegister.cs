using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Dependency;
using UnityEngine;

namespace SelfishFramework.Src.Unity
{
    public abstract class SDependencyRegister : MonoBehaviour
    {
        protected abstract World World { get; }
        protected DependencyContainer Container => World.DependencyContainer;

        public abstract void Register();
    }
}