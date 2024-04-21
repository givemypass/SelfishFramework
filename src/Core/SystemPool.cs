using System;

namespace SelfishFramework.Core
{
    public interface ISystemPool{}

    public class SystemPool<T> : ISystemPool where T : BaseSystem, new()
    {
        private readonly World world;
        
        private T[] denseItems;
        private int denseCount;
        //todo make resize
        private int[] sparseItems;
        private int sparseCount;
        private int[] recycledItems;
        private int recycledCount;
        
        public SystemPool(World world)
        {
            this.world = world;
            denseCount = 1;
            denseItems = new T[Constants.StartActorsCount];
            sparseItems = new int[Constants.StartActorsCount];
            recycledItems = new int[Constants.StartActorsCount];
        }

        public T Add(int actorId)
        {
            if(Has(actorId)) throw new Exception("Already added");
            
            int idx;
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
    }
}