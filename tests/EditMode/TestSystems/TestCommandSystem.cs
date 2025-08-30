using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Tests.EditMode.Commands;

namespace SelfishFramework.Tests.EditMode.TestSystems
{
    public partial class TestCommandSystem : BaseSystem, IReactGlobal<TestGlobalCommand>, IReactLocal<TestLocalCommand>
    {
        public bool IsGlobalReacted;
        public bool IsLocalReacted;
        
        public override void InitSystem()
        {
        }

        public void ReactGlobal(TestGlobalCommand command)
        {
            IsGlobalReacted = true;
        }

        public void ReactLocal(TestLocalCommand command)
        {
            IsLocalReacted = true;
        }
    }
}