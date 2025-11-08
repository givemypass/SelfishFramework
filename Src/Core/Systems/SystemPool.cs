using System;

namespace SelfishFramework.Src.Core.Systems
{
    public class SystemPool<T> : ISystemPool where T : BaseSystem, new()
    {
        private T[] _denseItems;

        public static readonly int TypeId = IndexGenerator.GetIndexForType<T>();

        public SystemPool(int capacity)
        {
            _denseItems = new T[capacity];
        }

        public T Add(int entityId)
        {
            if(Has(entityId)) throw new Exception("Already added");

            var system = new T();
            _denseItems[entityId] = system;
            
            return system;
        }
        
        public T Get(int entityId)
        {
            return _denseItems[entityId];
        }

        public void Remove(int entityId)
        {
            _denseItems[entityId] = null;
        }

        public bool Has(int id)
        {
            return _denseItems[id] != null;
        }

        public ISystem GetRaw(int entityId)
        {
            return _denseItems[entityId];
        }

        public void Resize(int newSize)
        {
            Array.Resize(ref _denseItems, newSize);
        }

        public void Dispose()
        {
            foreach (var system in _denseItems)
            {
                system?.Dispose();
            }
        }
    }
}