using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.DefaultUpdates;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Tests.EditMode.TestComponents;

namespace SelfishFramework.Tests.EditMode.TestSystems
{
    public class TestSystemA : BaseSystem, IUpdatable
    {
        public int TestValue { get; set; }
        public void Update()
        {
            var actor = Owner;
            Run(actor);
        }

        public void Run(Actor actor)
        {
            ref var component = ref actor.Get<TestComponentA>();
            component.TestInt++;
            TestValue++;
        }
    }
}