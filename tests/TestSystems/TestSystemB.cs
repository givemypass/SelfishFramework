using SelfishFramework.Src.Core;
using Tests.TestComponents;

namespace SelfishFramework.tests.TestSystems
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