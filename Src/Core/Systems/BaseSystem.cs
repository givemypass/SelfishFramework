namespace SelfishFramework.Src.Core.Systems
{
    public interface ISystem
    {
        public Entity Owner { get; set; }
        public void InitSystem();
    }

    //todo codegen this part
    public abstract class BaseSystem : ISystem
    {
        public Entity Owner { get; set; }
        public abstract void InitSystem();
    }
}