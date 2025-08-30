using System.Collections.Generic;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Features.GameFSM.Commands;
using SelfishFramework.Src.Features.GameFSM.Components;
using SelfishFramework.Src.SLogs;

namespace SelfishFramework.Src.Features.GameFSM.Systems
{
    public abstract class BaseMainGameLogicSystem : BaseSystem,
        IPreUpdatable,
        IReactGlobal<EndGameStateCommand>,
        IReactGlobal<ForceGameStateTransitionGlobalCommand>
    {
        private readonly Queue<EndGameStateCommand> _endGameStateCommands = new(2);
        private readonly Queue<ForceGameStateTransitionGlobalCommand> _forceStateCommands = new(2);

        void IPreUpdatable.PreUpdate()
        {
            if (_endGameStateCommands.TryDequeue(out var command))
                ProcessEndState(command);

            if (_forceStateCommands.TryDequeue(out var forceCommand))
                ProcessForceState(forceCommand);
        }

        void IReactGlobal<EndGameStateCommand>.ReactGlobal(EndGameStateCommand command)
        {
            ref var gameStateComponent = ref Owner.Get<GameStateComponent>();
            if (command.GameState != gameStateComponent.CurrentState)
                return;

            _endGameStateCommands.Enqueue(command);
        }

        void IReactGlobal<ForceGameStateTransitionGlobalCommand>.ReactGlobal(ForceGameStateTransitionGlobalCommand command)
        {
            _forceStateCommands.Enqueue(command);
        }

        public abstract void ProcessEndState(EndGameStateCommand endGameStateCommand);

        protected virtual void ProcessForceState(ForceGameStateTransitionGlobalCommand command)
        {
            ref var gameStateComponent = ref Owner.Get<GameStateComponent>();
            Owner.GetWorld().Command(new StopGameStateGlobalCommand(gameStateComponent.CurrentState));
            ChangeGameState(gameStateComponent.CurrentState, command.GameState);
        }

        protected void ChangeGameState(int from, int to)
        {
            ref var gameStateComponent = ref Owner.Get<GameStateComponent>();
            gameStateComponent.SetState(to);
            Owner.GetWorld().Command(new TransitionGameStateCommand { From = from, To = to });
        }
        
        protected void ChangeGameState(int to)
        {
            SLog.Log("Changing game state to: " + to);
            ref var gameStateComponent = ref Owner.Get<GameStateComponent>();
            var from = gameStateComponent.CurrentState;
            ChangeGameState(from, to);
        }
    }
}