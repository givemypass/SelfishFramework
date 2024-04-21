namespace SelfishFramework.Core
{
    public interface ISystem { }

    public interface IUpdatable
    {
        void Update();
    }

    public interface IFixedUpdatable
    {
        void FixedUpdate();
    }
    
    public class BaseSystem : ISystem
    {
        public Actor Owner { get; set; }
    }
}