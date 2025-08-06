using System;

namespace SelfishFramework.Src.Core.Systems
{
    public class SystemPool<T> : ISystemPool where T : BaseSystem, new()
    {
        private readonly World world;
        
        private T[] denseItems;
        private int denseCount;
        private int[] sparseItems;
        private int sparseCount;

        public SystemPool(World world)
        {
            this.world = world;
            denseCount = 1;
            denseItems = new T[Constants.StartActorsCount];
            sparseItems = new int[Constants.StartActorsCount];
        }

        public T Add(int actorId)
        {
            if(Has(actorId)) throw new Exception("Already added");

            if (denseCount == denseItems.Length)
            {
                Array.Resize(ref denseItems, denseCount << 1);
            }
            var idx = denseCount++;

            var system = new T();
            denseItems[idx] = system;
            sparseItems[actorId] = idx;
            
            return system;
        }
        
        public T Get(int actorId)
        {
            return denseItems[sparseItems[actorId]];
        }

        public void Remove(int id)
        {
            sparseItems[id] = 0;
            var idx = sparseItems[id];
            denseItems[idx] = default;
        }

        public bool Has(int id)
        {
            return sparseItems[id] > 0;
        }

        public void Resize(int newSize)
        {
            Array.Resize(ref sparseItems, newSize);
        }
    }
}