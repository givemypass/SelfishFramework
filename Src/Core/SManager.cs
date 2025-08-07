using System;

namespace SelfishFramework.Src.Core
{
    public class SManager : IDisposable
    {
        private static SManager instance;

        public static World Default => instance.World;

        public readonly World World;

        public SManager()
        {
            if (instance != null)
            {
                throw new Exception($"{nameof(SManager)} instance already created");
            }
            
            instance = this;
            World = new World();
        }

        public void Dispose()
        {
            instance = null;
            World.Dispose();
        }
    }
}