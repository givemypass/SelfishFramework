using System;

namespace SelfishFramework.Src.Core.Components
{
    public class ComponentPool<T> : IComponentPool where T : struct, IComponent
    {
        public static readonly ComponentInfo Info = ComponentInfo.Create();
        
        private T[] denseItems;
        private int denseCount;
        private int[] sparseItems;
        private int sparseCount;
        private int[] recycledItems;
        private int recycledCount;

        public ComponentPool(int length)
        {
            denseCount = 1;
            denseItems = new T[length];
            sparseItems = new int[length];
            recycledItems = new int[length];
        }

        public void Set(int entityId, in T component)
        {
            var idx = sparseItems[entityId];

            if(idx == 0)
            {
                if (recycledCount > 0)
                {
                    idx = recycledItems[--recycledCount];
                }
                else
                {
                    if (denseCount == denseItems.Length)
                    {
                        Array.Resize(ref denseItems, denseCount << 1);
                    }

                    idx = denseCount++;
                }

                sparseItems[entityId] = idx;
            }
            
            denseItems[idx] = component;
        }
        
        public ref T Get(int entityId)
        {
            return ref denseItems[sparseItems[entityId]];
        }

        public void Remove(int id)
        {
            if (!Has(id))
                return;
            if (recycledItems.Length == recycledCount)
            {
                Array.Resize(ref recycledItems, recycledCount << 1);
            }

            var idx = sparseItems[id];
            sparseItems[id] = 0;
            recycledItems[recycledCount++] = idx;
            denseItems[sparseItems[id]] = default;
        }

        public bool Has(int id)
        {
            return sparseItems[id] > 0;
        }

        public void Resize(int capacity)
        {
            Array.Resize(ref sparseItems, capacity); 
        }
    }
}