using System;

namespace SelfishFramework.Src.Core.Systems
{
    public interface ISystem : IDisposable
    {
        public Entity Owner { get; set; }
        public void InitSystem();
    }

    //todo codegen this part
    public abstract class BaseSystem : ISystem
    {
        public Entity Owner { get; set; }
        public abstract void InitSystem();
        public virtual void Dispose() { }
    }
}