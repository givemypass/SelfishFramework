namespace SelfishFramework.Src.Core.Systems
{
    public interface ISystem { }

    public class BaseSystem : ISystem
    {
        public Actor Owner { get; set; }
    }
}