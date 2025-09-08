namespace SelfishFramework.Src.Core
{
    //todo use custom allocator
    public struct EntityData
    {
        internal int[] components;
        internal int componentCount;
        
        internal int[] systems;
        internal int systemCount;

        internal void Initialize()
        {
            components = new int[8];
            systems = new int[2];
            componentCount = 0;
            systemCount = 0;
        }
    }
}