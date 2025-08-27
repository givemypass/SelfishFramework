namespace SelfishFramework.Src.Core.CommandBus
{
    public interface IReactLocal<T> where T : ICommand
    {
        void ReactLocal(T command);
    }
}