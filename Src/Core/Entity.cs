using System;
using System.Collections.Generic;

namespace SelfishFramework.Src.Core
{
    [Serializable]
    public class Entity
    {
        public int Id;
        
        public int Generation;
        
        public bool IsInitted;

        public World World { get; private set; }

        //todo move to custom type with allocator
        public readonly HashSet<int> Systems = new();
    }
}