using System;
using System.Collections.Generic;

namespace SelfishFramework.Core
{
    public interface IComponent{}

    public class World : IDisposable
    {
        private Dictionary<Type, IComponentPool> componentPools = new();
        private Dictionary<Type, ISystemPool> systemPools = new();
        
        private Actor[] actors = new Actor[Constants.StartActorsCount];
        private int actorsCount = 0;
        private Queue<int> recycledIndices = new(Constants.StartActorsCount);
        
        public readonly UpdateDefaultModule UpdateModule = new();

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
            if (actors.Length == actorsCount + Constants.ActorsIndexShift)
            {
                var newSize = (actorsCount + Constants.ActorsIndexShift) << 1;
                Array.Resize(ref actors, newSize);
                foreach (var componentPool in componentPools)
                {
                    componentPool.Value.Resize(newSize);
                }

                foreach (var systemPool in systemPools)
                {
                    systemPool.Value.Resize(newSize);
                }
            }

            actor.Generation++;
            var idx = recycledIndices.Count > 0 ? recycledIndices.Dequeue() : actorsCount++;
            actor.Id = idx + Constants.ActorsIndexShift;
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
            
            var pool = new ComponentPool<T>(this, actors.Length);
            componentPools.Add(type, pool);
            return pool;
        }

        public SystemPool<T> GetSystemPool<T>() where T : BaseSystem, new()
        {
            var type = typeof(T);
            if (systemPools.TryGetValue(type, out var rawPool))
                return (SystemPool<T>)rawPool;
            
            var pool = new SystemPool<T>(this);
            systemPools.Add(type, pool);
            return pool;
        }

        public void Update()
        {
            foreach (var systemPool in systemPools.Values)
            {
                systemPool.Update();
            }
            UpdateModule.Update();
        }

        public void Dispose()
        {
            //todo dispose all
        }
    }

}