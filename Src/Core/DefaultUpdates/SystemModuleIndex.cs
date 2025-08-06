namespace SelfishFramework.Src.Core.DefaultUpdates
{
    public static class SystemModuleIndex<T> where T : ISystemModule
    {
        public static readonly int Index = IndexGenerator.GetIndexForType(typeof(T));
    }
}