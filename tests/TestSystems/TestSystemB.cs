using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Core.Update;
using SelfishFramework.Tests.TestComponents;

namespace SelfishFramework.Tests.TestSystems
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