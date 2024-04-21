using System;
using System.Collections.Generic;

namespace SelfishFramework.Core
{
    public interface IComponent{}

    public class World : IDisposable
    {
        private Dictionary<Type, IComponentPool> componentPools = new();
        
        private Actor[] actors = new Actor[Constants.StartEntitiesCount];
        private int actorsCount = 0;
        private Queue<int> recycledIndices = new(Constants.StartEntitiesCount); 

        public bool IsActorAlive(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers a new actor in the system.
        /// </summary>
        /// <param name="actor">The actor to register.</param>
        public void RegisterActor(Actor actor)
        {
            if(actors.Length == actorsCount)
                Array.Resize(ref actors, actorsCount << 1);
            
            actor.Generation++;
            var idx = recycledIndices.Count > 0 ? recycledIndices.Dequeue() : actorsCount++;
            actor.Id = idx;
            actors[idx] = actor;
        }
        /// <summary>
        /// Unregisters an actor in the system.
        /// </summary>
        /// <param name="actor">The actor to unregister.</param>
        public void UnregisterActor(Actor actor)
        {
            recycledIndices.Enqueue(actor.Id);
            actors[actor.Id] = default;
            actorsCount--;
        }

        public ComponentPool<T> GetComponentPool<T>() where T : struct, IComponent
        {
            var type = typeof(T);
            if (componentPools.TryGetValue(type, out var rawPool))
                return (ComponentPool<T>)rawPool;
            
            var pool = new ComponentPool<T>(this);
            componentPools.Add(type, pool);
            return pool;
        }

        public void Dispose()
        {
            //todo dispose all
        }
    }

    public interface IComponentPool{}
    public class ComponentPool<T> : IComponentPool where T : struct, IComponent
    {
        private readonly World world;

        private T[] denseItems;
        private int denseCount;
        private int[] sparseItems;
        private int sparseCount;
        private int[] recycledItems;
        private int recycledCount;

        public ComponentPool(World world)
        {
            this.world = world;
            denseCount = 1;
            denseItems = new T[Constants.StartEntitiesCount];
            sparseItems = new int[Constants.StartEntitiesCount];
            recycledItems = new int[Constants.StartEntitiesCount];
        }

        public ref T Add(int actorId)
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
            sparseItems[actorId] = idx;
            return ref denseItems[idx];
        }
        
        public ref T Get(int actorId)
        {
            return ref denseItems[sparseItems[actorId]];
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