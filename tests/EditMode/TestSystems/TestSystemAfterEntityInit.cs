using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;

namespace SelfishFramework.Tests.EditMode.TestSystems
{
    public partial class TestSystemAfterEntityInit : BaseSystem, IAfterEntityInit
    {
        public int TestValue { get; set; }
        public void Update()
        {
            
        }

        public override void InitSystem()
        {
        }

        public void AfterEntityInit()
        {
            TestValue = 1;
        }
    }
}