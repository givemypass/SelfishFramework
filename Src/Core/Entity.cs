using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SelfishFramework.Src.Core
{
    public readonly struct Entity : IEquatable<Entity>
    {
        internal readonly long _value;
        
        //todo move to custom storage with allocator
        public readonly HashSet<int> Systems;
        
        public Entity(int id, ushort generation, ushort worldId)
        {
            _value = ((id & 0xFFFFFFFFL) << 32) | (generation & 0xFFFFL) << 16 | (worldId & 0xFFFFL);
            Systems = new();
        }

        public int Id
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int)((_value >> 32) & 0xFFFFFFFFL);
        }

        public ushort Generation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (ushort)(_value >> 16 & 0xFFFFL);
        }
        
        public ushort WorldId
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (ushort)(_value & 0xFFFFL);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Entity other)
        {
            return _value == other._value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is Entity other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Entity left, Entity right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
}