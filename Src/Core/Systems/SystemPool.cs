using System;

namespace SelfishFramework.Src.Core.Systems
{
    public class SystemPool<T> : ISystemPool where T : BaseSystem, new()
    {
        private T[] denseItems = new T[Constants.START_ENTITY_COUNT];

        public static readonly int TypeId = IndexGenerator.GetIndexForType<T>();

        public T Add(int entityId)
        {
            if(Has(entityId)) throw new Exception("Already added");

            var system = new T();
            denseItems[entityId] = system;
            
            return system;
        }
        
        public T Get(int entityId)
        {
            return denseItems[entityId];
        }

        public void Remove(int entityId)
        {
            denseItems[entityId] = null;
        }

        public bool Has(int id)
        {
            return denseItems[id] != null;
        }

        public ISystem GetRaw(int entityId)
        {
            return denseItems[entityId];
        }

        public void Resize(int newSize)
        {
            Array.Resize(ref denseItems, newSize);
        }
    }
}