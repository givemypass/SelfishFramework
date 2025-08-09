using System.Runtime.CompilerServices;
using System.Threading;

namespace SelfishFramework.Src.Core.Components
{
    public static class ComponentIncrementor
    {
        private static int _value = 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetNextIndex()
        {
            Interlocked.Increment(ref _value);
            return _value;
        }
    }
}