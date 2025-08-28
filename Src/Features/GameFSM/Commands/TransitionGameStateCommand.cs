using SelfishFramework.Src.Core.CommandBus;

namespace SelfishFramework.Src.Features.GameFSM.Commands
{
    public struct TransitionGameStateCommand : IGlobalCommand
    {
        //game state identifier
        public int From;
        public int To;
    }
}