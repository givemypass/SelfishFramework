using SelfishFramework.Src.Core.CommandBus;
using UnityEngine.InputSystem;

namespace SelfishFramework.Src.Unity.Commands
{
    public struct InputEndedCommand : ICommand, IGlobalCommand
    {
        public int Index;
        public InputAction.CallbackContext Context;
    }
}