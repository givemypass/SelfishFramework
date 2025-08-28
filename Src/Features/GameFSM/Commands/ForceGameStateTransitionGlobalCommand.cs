
using SelfishFramework.Src.Core.CommandBus;

namespace SelfishFramework.Src.Features.GameFSM.Commands
{
	public struct ForceGameStateTransitionGlobalCommand : IGlobalCommand
	{
		public int GameState;
	}
}