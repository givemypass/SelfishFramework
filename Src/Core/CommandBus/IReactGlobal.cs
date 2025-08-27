using SelfishFramework.Src.Core.SystemModules;

namespace SelfishFramework.Src.Core.CommandBus
{
    public interface IReactGlobal : ISystemAction{}
    public interface IReactGlobal<in T> : IReactGlobal where T : IGlobalCommand
    {
        void ReactGlobal(T command);
    }
}