using System.Collections.Generic;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.CommandBus;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity.Features.InputFeature.Commands;
using SelfishFramework.Src.Unity.Features.InputFeature.Components;
using UnityEngine.InputSystem;

namespace SelfishFramework.Src.Unity.Features.InputFeature.Systems
{
    public sealed partial class InputListenSystem : BaseSystem, IPreUpdatable
    {
        private readonly List<UpdatableAction> _actions = new();

        public override void InitSystem()
        {
            LinkActions();
        }

        public void PreUpdate()
        {
            foreach (var action in _actions)
            {
                action.UpdateAction();
            }
        }

        private void LinkActions()
        {
            ref var actionsComponent = ref Owner.Get<InputActionsComponent>();
            var defaultActionMap = actionsComponent.Actions.actionMaps[0];
            defaultActionMap.Enable();

            foreach (var action in defaultActionMap.actions)
            {
                var neededIndex = actionsComponent.GetInputActionIndex(action.name);
                action.Enable();
                var updateableAction = new UpdatableAction(neededIndex, action);
                updateableAction.OnStart += OnActionStart;
                updateableAction.OnEnd += OnActionEnd;
                updateableAction.OnUpdate += OnActionUpdate;
                updateableAction.OnPerformed += OnPerformed;
                _actions.Add(updateableAction);
            }
        }

        private void OnPerformed(int index, InputAction.CallbackContext context)
        {
            var command = new InputPerformedCommand { Index = index, Context = context };
            SendCommandToAllListeners(command);
        }

        private void OnActionStart(int index, InputAction.CallbackContext context)
        {
            var command = new InputStartedCommand { Index = index, Context = context };
            SendCommandToAllListeners(command);
        }

        private void OnActionUpdate(int index, InputAction.CallbackContext context)
        {
            var command = new InputCommand { Index = index, Context = context };
            SendCommandToAllListeners(command);
        }

        private void OnActionEnd(int index, InputAction.CallbackContext context)
        {
            var command = new InputEndedCommand { Index = index,  Context = context };
            SendCommandToAllListeners(command);
        }

        private void SendCommandToAllListeners<T>(T command) where T : struct, IGlobalCommand
        {
            foreach (var w in SManager.GetWorlds())
            {
                if (w == null)
                    continue;

                var collection = w.Filter.With<InputListenerTagComponent>().Build();

                foreach (var listener in collection)
                {
                    listener.Command(command);
                }
            }
        }

        public override void Dispose()
        {
            foreach (var action in _actions)
                action.Dispose();
        }
    }
}