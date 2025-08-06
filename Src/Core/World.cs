using System;
using System.Collections.Generic;
using SelfishFramework.Src.Core.Components;
using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Core.Update;

namespace SelfishFramework.Src.Core
{
    public class World : IDisposable
    {
        private readonly Dictionary<Type, IComponentPool> _componentPools = new();
        private readonly Dictionary<Type, ISystemPool> _systemPools = new();
        
        private Actor[] _actors = new Actor[Constants.StartActorsCount];
        private int _actorsCount = 0;
        private readonly Queue<int> _recycledIndices = new(Constants.StartActorsCount);
        
        public readonly SystemModuleRegistry SystemModuleRegistry = new();

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
            if (_actors.Length == _actorsCount + Constants.ActorsIndexShift)
            {
                var newSize = (_actorsCount + Constants.ActorsIndexShift) << 1;
                Array.Resize(ref _actors, newSize);
                foreach (var componentPool in _componentPools)
                {
                    componentPool.Value.Resize(newSize);
                }

                foreach (var systemPool in _systemPools)
                {
                    systemPool.Value.Resize(newSize);
                }
            }

            actor.Generation++;
            var idx = _recycledIndices.Count > 0 ? _recycledIndices.Dequeue() : _actorsCount++;
            actor.Id = idx + Constants.ActorsIndexShift;
            _actors[idx] = actor;
        }
        /// <summary>
        /// Unregisters an actor in the system.
        /// </summary>
        /// <param name="actor">The actor to unregister.</param>
        public void UnregisterActor(Actor actor)
        {
            _recycledIndices.Enqueue(actor.Id);
            _actors[actor.Id] = default;
            _actorsCount--;
        }

        public ComponentPool<T> GetComponentPool<T>() where T : struct, IComponent
        {
            var type = typeof(T);
            if (_componentPools.TryGetValue(type, out var rawPool))
                return (ComponentPool<T>)rawPool;
            
            var pool = new ComponentPool<T>(this, _actors.Length);
            _componentPools.Add(type, pool);
            return pool;
        }

        public SystemPool<T> GetSystemPool<T>() where T : BaseSystem, new()
        {
            var type = typeof(T);
            if (_systemPools.TryGetValue(type, out var rawPool))
                return (SystemPool<T>)rawPool;
            
            var pool = new SystemPool<T>(this);
            _systemPools.Add(type, pool);
            return pool;
        }

        public void Dispose()
        {
            //todo dispose all
        }
    }
}