using System;
using System.Collections.Generic;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Collections;
using SelfishFramework.Src.Core.SystemModules;

namespace SelfishFramework.Src.StateMachine
{
    public class StateMachine : IUpdatable, IDisposable
    {
        private readonly Dictionary<int, BaseFSMState> _states = new(8);
        private readonly Dictionary<int, FastList<ITransition>> _transitions = new(2);
        private readonly Queue<int> _changeState = new(1);
        private readonly Entity _owner;

        private int _currentState;
        private int _previousState;
        private bool _isPaused;

        public StateMachine(Entity owner)
        {
            _owner = owner;
            _owner.GetWorld().SystemModuleRegistry.GetModule<UpdateDefaultModule>().Register(this);
        }

        public int CurrentState => _currentState;
        public int PreviousState => _previousState;
        public bool IsPaused => _isPaused;

        public void Update()
        {
            if (_isPaused)
                return;
            
            if (_changeState.TryDequeue(out var state))
            {
                ProcessChangeState(state);
            }

            if (CurrentState != 0)
            {
                _states[CurrentState].Update(_owner);
            }
        }

        public void ChangeState(int toState)
        {
            if (_isPaused)
                return;
            
            _changeState.Enqueue(toState);
        }

        public void Pause(bool enable)
        {
            if (_isPaused == enable)
                return;
            
            _isPaused = enable;

            if(_currentState == 0)
                return;
            
            if (enable)
            {
                _states[_currentState].Disable(_owner);
            }
            else
            {
                _states[_currentState].Enable(_owner);
            }
        }

        private void ProcessChangeState(int toState)
        {
            if (_currentState != 0)
            {
                _states[_currentState].Disable(_owner);
                _states[_currentState].Exit(_owner);
            }

            _previousState = _currentState;
            _currentState = toState;
            if (TryGetState(toState, out var state))
            {
                state.Enter(_owner);
                state.Enable(_owner);
            }
        }

        public void NextState()
        {
            if (_currentState == 0)
                return;

            if (this._transitions.TryGetValue(_currentState, out var transitionsOfState))
            {
                foreach (var t in transitionsOfState)
                {
                    if (t.IsReady(_owner))
                    {
                        ChangeState(t.ToState);
                        return;
                    }
                }
            }
         
            ChangeState(_states[_currentState].NextStateID);
        }

        public void AddState(BaseFSMState state)
        {
            if (!_states.TryAdd(state.StateID, state))
                _states[state.StateID] = state;
        }

        public bool RemoveState(BaseFSMState state)
        {
            return _states.Remove(state.StateID);
        }

        public bool TryGetState(int id, out BaseFSMState state)
        {
            return _states.TryGetValue(id, out state);
        }

        public void AddStateTransition(int stateId, ITransition transition)
        {
            if (_transitions.TryGetValue(stateId, out var stateTransitions))
            {
                stateTransitions.Add(transition);
            }
            else
            {
                _transitions.Add(stateId,new FastList<ITransition>());
                _transitions[stateId].Add(transition);
            }
        }

        public void Dispose()
        {
            _owner.GetWorld().SystemModuleRegistry.GetModule<UpdateDefaultModule>().Unregister(this);
            foreach (var state in _states.Values)
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

        public abstract void Enter(Entity entity);
        public abstract void Exit(Entity entity);
        public abstract void Update(Entity entity);

        public virtual void Enable(Entity entity)
        {
            
        }
        
        public virtual void Disable(Entity entity)
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
        bool IsReady(Entity entity);
    }
}
