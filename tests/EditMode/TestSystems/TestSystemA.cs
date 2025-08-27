using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Tests.EditMode.TestComponents;

namespace SelfishFramework.Tests.EditMode.TestSystems
{
    public partial class TestSystemA : BaseSystem, IUpdatable
    {
        public int TestValue { get; set; }
        public void Update()
        {
            var entity = Owner;
            Run(entity);
        }

        public void Run(Entity entity)
        {
            ref var component = ref entity.Get<TestComponentA>();
            component.TestInt++;
            TestValue++;
        }

        public override void InitSystem()
        {
        }
    }
}