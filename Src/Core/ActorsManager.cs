using System;

namespace SelfishFramework.Src.Core
{
    public class ActorsManager : IDisposable
    {
        private static ActorsManager instance;

        public static World Default => instance.World;

        public readonly World World;

        public ActorsManager()
        {
            if (instance != null)
            {
                throw new Exception($"{nameof(ActorsManager)} instance already created");
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