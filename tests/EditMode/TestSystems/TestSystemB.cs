using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Tests.EditMode.TestComponents;

namespace SelfishFramework.Tests.EditMode.TestSystems
{
    public class TestSystemB : BaseSystem, IUpdatable
    {
        public int TestValue { get; set; }
        public void Update()
        {
            var actor = Owner;
            Run(actor);
        }

        public void Run(Actor actor)
        {
            ref var component = ref actor.Get<TestComponentB>();
            component.TestInt++;
            TestValue++;
        }
    }
}