using System;

namespace SelfishFramework.Src.Core.Components
{
    //todo generate map for all components in codegen
    public struct ComponentInfo
    {
        public readonly int Index;
        public readonly LongHash Hash;

        private ComponentInfo(int index, LongHash hash)
        {
            Index = index;
            Hash = hash;
        }
        
        public static ComponentInfo Create()
        {
            //todo generate index by codegen
            var index = ComponentIncrementor.GetNextIndex();
            
            var hash = Math.Abs(7_777_777_777_777_777_773L * index);
            return new ComponentInfo(index, new LongHash(hash));
        }
    }
}