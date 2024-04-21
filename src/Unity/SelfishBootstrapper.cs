using System;
using SelfishFramework.Core;
using UnityEngine;

namespace SelfishFramework.src.Unity
{
    public class SelfishBootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            ActorsManager.RecreateInstance();
        }

        private void Update()
        {
            for (var i = 0; i < ActorsManager.Worlds.Length; i++)
            {
                ActorsManager.Worlds[i]?.Update();
            }
        }
    }
}