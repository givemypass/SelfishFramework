using SelfishFramework.Src.Core.CommandBus;

namespace SelfishFramework.Src.Unity.Commands
{
    public struct InputStartedCommand : ICommand
    {
        public int Index;
    }
}