using System;
using System.Collections.Generic;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Collections;
using SelfishFramework.Src.Core.DefaultUpdates;

namespace SelfishFramework.Src.StateMachine
{
    public partial class StateMachine : IUpdatable, IDisposable
    {
        private readonly Dictionary<int, BaseFSMState> states = new Dictionary<int, BaseFSMState>(8);
        private readonly Dictionary<int, FastList<ITransition>> transitions = new(2);
        private readonly Queue<int> changeState = new Queue<int>(1);
        private readonly Actor owner;

        private int currentState;
        private int previousState;
        private bool isPaused;

        public StateMachine(Actor owner)
        {
            this.owner = owner;
            this.owner.World.SystemModuleRegistry.GetModule<UpdateDefaultModule>().Register(this);
        }

        public int CurrentState { get => currentState; }
        public int PreviousState { get => previousState; }
        public bool IsPaused { get => isPaused; }

        public void Update()
        {
            if (isPaused)
                return;
            
            if (changeState.TryDequeue(out var state))
            {
                ProcessChangeState(state);
            }

            if (CurrentState != 0)
            {
                states[CurrentState].Update(owner);
            }
        }

        public void ChangeState(int toState)
        {
            if (isPaused)
                return;
            
            changeState.Enqueue(toState);
        }

        public void Pause(bool enable)
        {
            if (isPaused == enable)
                return;
            
            isPaused = enable;

            if(currentState == 0)
                return;
            
            if (enable)
            {
                states[currentState].Disable(owner);
            }
            else
            {
                states[currentState].Enable(owner);
            }
        }

        private void ProcessChangeState(int toState)
        {
            if (currentState != 0)
            {
                states[currentState].Disable(owner);
                states[currentState].Exit(owner);
            }

            previousState = currentState;
            currentState = toState;
            if (TryGetState(toState, out var state))
            {
                state.Enter(owner);
                state.Enable(owner);
            }
        }

        public void NextState()
        {
            if (currentState == 0)
                return;

            if (this.transitions.TryGetValue(currentState, out var transitionsOfState))
            {
                foreach (var t in transitionsOfState)
                {
                    if (t.IsReady(owner))
                    {
                        ChangeState(t.ToState);
                        return;
                    }
                }
            }
         
            ChangeState(states[currentState].NextStateID);
        }

        public void AddState(BaseFSMState state)
        {
            if (!states.TryAdd(state.StateID, state))
                states[state.StateID] = state;
        }

        public bool RemoveState(BaseFSMState state)
        {
            return states.Remove(state.StateID);
        }

        public bool TryGetState(int id, out BaseFSMState state)
        {
            return states.TryGetValue(id, out state);
        }

        public void AddStateTransition(int stateId, ITransition transition)
        {
            if (transitions.TryGetValue(stateId, out var stateTransitions))
            {
                stateTransitions.Add(transition);
            }
            else
            {
                transitions.Add(stateId,new FastList<ITransition>());
                transitions[stateId].Add(transition);
            }
        }

        public void Dispose()
        {
            owner.World.SystemModuleRegistry.GetModule<UpdateDefaultModule>().Unregister(this);
            foreach (var state in states.Values)
            {
                state.Dispose();
            }
        }
    }

    public abstract class BaseFSMState : IDisposable
    {
        public abstract int StateID { get; }
        public abstract int NextStateID { get; }

        protected StateMachine stateMachine;

        public abstract void Enter(Actor entity);
        public abstract void Exit(Actor entity);
        public abstract void Update(Actor entity);

        public virtual void Enable(Actor entity)
        {
            
        }
        
        public virtual void Disable(Actor entity)
        {
            
        }

        protected void EndState()
        {
            stateMachine.NextState();
        }

        protected bool IsCurrentState()
        {
            return stateMachine.CurrentState == StateID;
        }

        public virtual void Dispose()
        {
        }

        public BaseFSMState(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
    }

    public interface ITransition
    {
        int ToState { get; }
        bool IsReady(Actor entity);
    }
}
