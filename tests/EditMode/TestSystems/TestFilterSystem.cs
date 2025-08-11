using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Tests.EditMode.TestComponents;

namespace SelfishFramework.Tests.EditMode.TestSystems
{
    public class TestFilterSystem : BaseSystem, IUpdatable
    {
        private Filter _filter;

        public override void InitSystem()
        {
            _filter = SManager.Default.Filter
                .With<TestComponentA>()
                .Build();
        }

        public void Update()
        {
            foreach (var entity in _filter)
            {
                entity.Get<TestComponentA>().TestInt++;
            }
        }
    }
}