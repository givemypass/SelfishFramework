using SelfishFramework.Src.Core;
using SelfishFramework.Tests.TestComponents;

namespace SelfishFramework.Tests.TestSystems
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