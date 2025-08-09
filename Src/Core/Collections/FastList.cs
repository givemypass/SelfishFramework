using System.Collections.Generic;

namespace SelfishFramework.Src.Core.Collections
{
    public sealed class FastList<T> : List<T>
    {
        public FastList()
        {
        }
        public FastList(int capacity) : base(capacity)
        {
        }
        //todo write a better implementation  
    }
}