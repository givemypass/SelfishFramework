using System;

namespace SelfishFramework.Src.Core.Components
{
    //todo generate map for all components in codegen
    public struct ComponentInfo
    {
        public readonly int Index;
        public readonly int TypeId;
        public readonly LongHash Hash;

        private ComponentInfo(int index, int typeId, LongHash hash)
        {
            Index = index;
            TypeId = typeId;
            Hash = hash;
        }
        
        public static ComponentInfo Create<T>()
        {
            var typeId = IndexGenerator.GetIndexForType<T>();
            var index = ComponentIncrementor.GetNextIndex();
            var hash = Math.Abs(7_777_777_777_777_777_773L * index);
            return new ComponentInfo(index, typeId, new LongHash(hash));
        }
    }
}