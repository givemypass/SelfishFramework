namespace SelfishFramework.Src.Core.Update
{
    public static class SystemModuleIndex<T> where T : ISystemModule
    {
        public static readonly int Index = IndexGenerator.GetIndexForType(typeof(T));
    }
}