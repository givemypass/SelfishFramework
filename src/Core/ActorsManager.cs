using System;
using System.Collections.Generic;

namespace SelfishFramework.Core
{
    public class ActorsManager : IDisposable
    {
        private static ActorsManager instance;

        private readonly World[] worlds;
        
        public static void Init()
        {
            if(instance == null)
                instance = new ActorsManager();
        }

        public static World Default => instance.worlds[0];
        
        //todo return read only collection
        public static World[] Worlds => instance.worlds;

        public ActorsManager()
        {
            worlds = new World[1];
            worlds[0] = new World();
        }

        public static void RecreateInstance()
        {
            instance?.Dispose();
            instance = new ActorsManager();
        }

        public void Dispose()
        {
            foreach (var world in worlds)
            {
                world?.Dispose();
            }
        }
    }
}