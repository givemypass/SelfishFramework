using System;
using System.Runtime.CompilerServices;

namespace SelfishFramework.Src.Core
{
    public readonly struct Entity : IEquatable<Entity>
    {
        internal readonly long value;
        
        public Entity(int id, ushort generation, ushort worldId)
        {
            value = ((id & 0xFFFFFFFFL) << 32) | (generation & 0xFFFFL) << 16 | (worldId & 0xFFFFL);
        }

        public int Id
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int)((value >> 32) & 0xFFFFFFFFL);
        }

        public ushort Generation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (ushort)(value >> 16 & 0xFFFFL);
        }
        
        public ushort WorldId
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (ushort)(value & 0xFFFFL);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Entity other)
        {
            return value == other.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is Entity other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return value.GetHashCode();
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