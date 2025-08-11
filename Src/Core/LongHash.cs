using System;
using System.Runtime.CompilerServices;

namespace SelfishFramework.Src.Core
{
    public readonly struct LongHash : IEquatable<LongHash>
    {
        private readonly long _value;

        public long Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _value;
        }

        public LongHash(long hash)
        {
            _value = hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(LongHash other)
        {
            return _value == other._value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is LongHash other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LongHash Combine(LongHash hash1, LongHash hash2)
        {
            return new LongHash(hash1._value ^ hash2._value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(LongHash hash1, LongHash hash2)
        {
            return hash1._value == hash2._value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(LongHash hash1, LongHash hash2)
        {
            return !(hash1 == hash2);
        }
    }
}