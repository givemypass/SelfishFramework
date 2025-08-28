using SelfishFramework.Src.Core.CommandBus;

namespace SelfishFramework.Src.Features.GameFSM.Commands
{
    public struct StopGameStateGlobalCommand : IGlobalCommand
    {
        public int GameState;

        public StopGameStateGlobalCommand(int gameState)
        {
            GameState = gameState;
        }
    }
}