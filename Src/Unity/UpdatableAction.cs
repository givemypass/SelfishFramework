using System;
using UnityEngine.InputSystem;

namespace SelfishFramework.Src.Unity
{
    public class UpdatableAction : IDisposable
    {
        private readonly InputAction _action;

        private InputAction.CallbackContext _cachedForUpdate;
        private bool _isPressed;
        private readonly int _index;

        public event Action<int, InputAction.CallbackContext> OnStart;
        public event Action<int, InputAction.CallbackContext> OnEnd;
        public event Action<int, InputAction.CallbackContext> OnUpdate;
        public event Action<int, InputAction.CallbackContext> OnPerformed;
        
        public void UpdateAction()
        {
            if (!_isPressed) return;
            OnUpdate?.Invoke(_index, _cachedForUpdate);
        }

        public UpdatableAction(int index, InputAction action)
        {
            _action = action;
            action.started += Started;
            action.performed += Updated;
            action.performed += x => OnPerformed?.Invoke(index, x);
            action.canceled += Ended;
            this._index = index;
        }

        private void Ended(InputAction.CallbackContext obj)
        {
            OnEnd?.Invoke(_index,obj);
        }

        private void Updated(InputAction.CallbackContext obj)
        {
            _isPressed = true;
            _cachedForUpdate = obj;
        }

        private void Started(InputAction.CallbackContext obj)
        {
            OnStart?.Invoke(_index, obj);
        }


        public void Dispose()
        {
            _action.started -= Started;
            _action.performed -= Updated;
            _action.canceled -= Ended;
            OnStart = null;
            OnEnd = null;
            OnUpdate = null;
        }
    }
}