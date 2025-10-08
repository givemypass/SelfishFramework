using SelfishFramework.Src.Core;
using SelfishFramework.Src.Features.GameFSM.Components;
using SelfishFramework.Src.Unity.CommonCommands;
using UnityEngine;

namespace SelfishFramework.Src.Unity.CommonMono
{
    public class SaveWhenExitMonoComponent : MonoBehaviour
    {
#if !UNITY_EDITOR
        private void OnApplicationPause(bool onPause)
        {
            if (!onPause)
                return;
            if (!SManager.IsAlive)
            {
                return;
            }
            var filter = SManager.World.Filter.With<GameStateComponent>().Build();
            foreach (var entity in filter)
            {
                ref var gameStateComponent = ref entity.Get<GameStateComponent>();
                if (gameStateComponent.CurrentState == 0) continue;
                SManager.World.Command(new BeforeSaveCommand());
                SManager.World.Command(new SaveCommand());
            }
        }
#endif
#if UNITY_EDITOR
        private void OnApplicationQuit()
        {
            SManager.World.Command(new BeforeSaveCommand());
            SManager.World.Command(new SaveCommand());
        }
#endif
    }
}