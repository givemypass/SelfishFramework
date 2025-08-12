namespace SelfishFramework.Src.Core.Systems
{
    public interface ISystemPool
    {
        void Resize(int newSize);
        bool Has(int entityId);
        ISystem GetRaw(int entityId);
    }
}