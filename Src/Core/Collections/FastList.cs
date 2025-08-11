using System.Collections.Generic;

namespace SelfishFramework.Src.Core.Collections
{
    public sealed class FastList<T> : List<T>
    {
        //todo write a faster implementation  
        public FastList()
        {
        }
        public FastList(int capacity) : base(capacity)
        {
        }
    }
}