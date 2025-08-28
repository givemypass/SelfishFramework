
using SelfishFramework.Src.Core.CommandBus;

namespace SelfishFramework.Src.Features.GameFSM.Commands
{
    public struct EndGameStateCommand : IGlobalCommand
	{
		public int GameState;

		public EndGameStateCommand(int gameState)
		{
			GameState = gameState;
		}
	}
}