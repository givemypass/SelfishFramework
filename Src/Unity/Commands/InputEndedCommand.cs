using SelfishFramework.Src.Core.CommandBus;

namespace SelfishFramework.Src.Unity.Commands
{
    public struct InputEndedCommand : ICommand
    {
        public int Index;
    }
}