namespace SelfishFramework.Src.Core.Components
{
    public interface IComponentPool
    {
        void Resize(int capacity);
        bool Has(int entityId);
        void Remove(int entityId);
    }
}