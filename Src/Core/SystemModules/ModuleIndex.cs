namespace SelfishFramework.Src.Core.SystemModules
{
    public static class ModuleIndex<T> where T : IModule
    {
        public static readonly int Index = IndexGenerator.GetIndexForType<T>();
    }
}