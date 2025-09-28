namespace SelfishFramework.Src.Core.Components
{
    public interface IComponentPool
    {
        void Resize(int capacity);
        bool Has(int entityId);
        void Remove(int entityId);

        /// <summary>
        /// USE WITH CAUTION!
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        IComponent GetRaw(int entityId);
    }
}