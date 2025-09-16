using System;

namespace SelfishFramework.Src.Core.Systems
{
    public interface ISystem : IDisposable
    {
        public World World { get; set; }
        public Entity Owner { get; set; }
        public void InitSystem();
        void UnregisterCommands();
        void RegisterCommands();
    }

    public abstract class BaseSystem : ISystem
    {
        public World World { get; set; }
        public Entity Owner { get; set; }
        public abstract void InitSystem();
        public virtual void Dispose() { }
        public abstract void RegisterCommands();
        public abstract void UnregisterCommands();
    }
}