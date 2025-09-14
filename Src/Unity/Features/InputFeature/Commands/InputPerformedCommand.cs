using SelfishFramework.Src.Core.CommandBus;
using UnityEngine.InputSystem;

namespace SelfishFramework.Src.Unity.Features.InputFeature.Commands
{
    public struct InputPerformedCommand : IGlobalCommand
    {
        public int Index;
        public InputAction.CallbackContext Context;
    }
}