using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Collections;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Features.GameFSM.Commands;
using SelfishFramework.Src.Features.GameFSM.Components;

namespace Systems
{
    public abstract class BaseGameStateSystem : BaseSystem, IReactGlobal<TransitionGameStateCommand>,
        IReactGlobal<StopGameStateGlobalCommand>
    {
        //we can have various force transitions from this state
        private readonly FastList<IForceTransition> _forceTransitions = new(2);

        protected abstract int State { get; }

        void IReactGlobal<StopGameStateGlobalCommand>.ReactGlobal(StopGameStateGlobalCommand command)
        {
            if (command.GameState == State)
            {
                OnExitState();
                StopState();
            }
        }

        void IReactGlobal<TransitionGameStateCommand>.ReactGlobal(TransitionGameStateCommand command)
        {
            if (command.To != State)
                return;

            ProcessState(command.From, command.To);
        }

        protected void AddForceTransitions(IForceTransition forceTransition)
        {
            _forceTransitions.Add(forceTransition);
        }

        protected abstract void ProcessState(int from, int to);

        /// <summary>
        ///     shortcut for end a state
        /// </summary>
        protected void EndState()
        {
            OnExitState();

            foreach (var transition in _forceTransitions)
                if (transition.ForceTransitionComplete(Owner, State))
                    return;

            World.Command(new EndGameStateCommand(State));
        }

        protected virtual void OnExitState()
        {
        }

        /// <summary>
        ///     helper method for cheking needed states
        /// </summary>
        /// <param name="from">we provide here information from transition command</param>
        /// <param name="fromCheck">needed from state (GameStateIdentifierMap)</param>
        /// <param name="to">we provide here information from transition command (to)</param>
        /// <param name="toCheck">needed (to) from state (GameStateIdentifierMap)</param>
        /// <returns></returns>
        protected bool IsNeededStates(int from, int fromCheck, int to, int toCheck)
        {
            return from == fromCheck && to == toCheck;
        }

        protected bool IsNeededStates(int from, int to)
        {
            ref var gameStateComponent = ref Owner.Get<GameStateComponent>();
            return from == gameStateComponent.PreviousState && to == gameStateComponent.CurrentState;
        }
        
        protected bool IsNeededStates()
        {
            ref var gameStateComponent = ref Owner.Get<GameStateComponent>();
            return State == gameStateComponent.CurrentState;
        }

        /// <summary>
        ///     u should override at child this method, for implementation of stoping state
        /// </summary>
        protected virtual void StopState()
        {
        }
    }

    public interface IForceTransition
    {
        bool ForceTransitionComplete(Entity owner, int CurrentState);
    }
}