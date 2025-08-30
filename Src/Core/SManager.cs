using System;
using System.Collections.Generic;

namespace SelfishFramework.Src.Core
{
    public class SManager : IDisposable
    {
        private const int START_WORLDS = 1;

        private static SManager instance;

        public static World World => instance.Worlds[0];

        public readonly List<World> Worlds = new();

        public SManager()
        {
            if (instance != null)
            {
                throw new Exception($"{nameof(SManager)} instance already created");
            }
            
            instance = this;
            for (int i = 0; i < START_WORLDS; i++)
            {
                var world = new World((ushort)i);
                Worlds.Add(world); 
            }
        }

        public static World GetWorld(int index)
        {
            return instance.Worlds[index];
        }
        
        public static IEnumerable<World> GetWorlds()
        {
            return instance.Worlds;
        }

        public void Dispose()
        {
            instance = null;
            foreach (var world in Worlds)
            {
                world.Dispose();
            }
        }
    }
}