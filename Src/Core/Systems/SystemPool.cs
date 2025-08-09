using System;

namespace SelfishFramework.Src.Core.Systems
{
    public class SystemPool<T> : ISystemPool where T : BaseSystem, new()
    {
        private T[] denseItems = new T[Constants.StartEntityCount];

        public static readonly int Index = IndexGenerator.GetIndexForType(typeof(T));

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

        public void Resize(int newSize)
        {
            Array.Resize(ref denseItems, newSize);
        }
    }
}