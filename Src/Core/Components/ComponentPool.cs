using System;

namespace SelfishFramework.Src.Core.Components
{
    public class ComponentPool<T> : IComponentPool where T : struct, IComponent
    {
        public static readonly ComponentInfo Info = ComponentInfo.Create<T>();
        public static T Default = default;
        
        private readonly int _id;
        
        private T[] _denseItems;
        private int _denseCount;
        private int[] _sparseItems;
        private int _sparseCount;
        private int[] _recycledItems;
        private int _recycledCount;

        public ComponentPool(int id, int length)
        {
            _id = id;
            _denseCount = 1;
            _denseItems = new T[length];
            _sparseItems = new int[length];
            _recycledItems = new int[length];
        }
        public int GetId()
        {
            return _id;
        }

        public void Set(int entityId, in T component)
        {
            var idx = _sparseItems[entityId];

            if(idx == 0)
            {
                if (_recycledCount > 0)
                {
                    idx = _recycledItems[--_recycledCount];
                }
                else
                {
                    if (_denseCount == _denseItems.Length)
                    {
                        Array.Resize(ref _denseItems, _denseCount << 1);
                    }

                    idx = _denseCount++;
                }

                _sparseItems[entityId] = idx;
            }
            
            _denseItems[idx] = component;
        }

        public ref T Get(int entityId)
        {
            return ref _denseItems[_sparseItems[entityId]];
        }

        public void Remove(int id)
        {
            if (!Has(id))
                return;
            if (_recycledItems.Length == _recycledCount)
            {
                Array.Resize(ref _recycledItems, _recycledCount << 1);
            }

            var idx = _sparseItems[id];
            _sparseItems[id] = 0;
            _recycledItems[_recycledCount++] = idx;
            _denseItems[_sparseItems[id]] = default;
        }

        public bool Has(int id)
        {
            return _sparseItems[id] > 0;
        }

        public void Resize(int capacity)
        {
            Array.Resize(ref _sparseItems, capacity); 
        }
    }
}