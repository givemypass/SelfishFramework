namespace SelfishFramework.Src.Core.Dependency
{
    public interface IInjectable
    {
        void ResolveDependencies(DependencyContainer dependencyContainer);
    }
}