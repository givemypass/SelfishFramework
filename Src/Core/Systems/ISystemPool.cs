using System;

namespace SelfishFramework.Src.Core.Systems
{
    public interface ISystemPool : IDisposable
    {
        void Resize(int newSize);
        bool Has(int entityId);
        void Remove(int entityId);
        ISystem GetRaw(int entityId);
    }
}