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
            Run(Owner);
        }

        public void Run(Entity entity)
        {
            ref var component = ref entity.Get<TestComponentB>();
            component.TestInt++;
            TestValue++;
        }

        public override void InitSystem()
        {
        }
    }
}