using System;
using System.Collections.Generic;
using SelfishFramework.Src.Core.Collections;
using UnityEngine;

namespace SelfishFramework.Src.Core.Filter
{
    public sealed class Filter
    {
        private readonly World _world;
        private readonly FastList<int> _includes;
        private readonly FastList<int> _excludes;
        private readonly HashSet<Entity> _check = new(32);
        private readonly FastList<Entity> _entities = new(32);
        
        public Filter(World world, int[] includes, int[] excludes)
        {
            _world = world;
            _includes = new FastList<int>(includes.Length);
            foreach (var include in includes)
            {
                _includes.Add(include);
            }
            _excludes = new FastList<int>(excludes.Length);
            foreach (var exclude in excludes)
            {
                _excludes.Add(exclude);
            }
        }

        public void ForceUpdate()
        {
            UpdateFilter(_world.dirtyEntities);
        }
        
        public void UpdateFilter(HashSet<Entity> dirtyEntities)
        {
            foreach (var dirtyEntity in dirtyEntities)
            {
                if (_world.IsDisposed(dirtyEntity) || !MatchFilter(dirtyEntity))
                {
                    _check.Remove(dirtyEntity);
                }
                else
                {
                    _check.Add(dirtyEntity);
                }
            } 
            
            //todo clear fast
            _entities.Clear();
            foreach (var entity in _check)
            {
                _entities.Add(entity);
            }
        }

        private bool MatchFilter(Entity entity)
        {
            foreach (var include in _includes)
            {
                if (!_world.TryGetComponentPool(include, out var pool))
                {
                    throw new Exception("Component pool not found: " + include);
                }
                
                if (!pool.Has(entity.Id))
                {
                    return false;
                }
            }

            foreach (var exclude in _excludes)
            {
                if (!_world.TryGetComponentPool(exclude, out var pool))
                {
                    throw new Exception("Component pool not found: " + exclude);
                }
                
                if (pool.Has(entity.Id))
                {
                    return false;
                }
            }

            return true; 
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_world, _entities);
        }

        public struct Enumerator
        {
            private readonly World _world;
            private readonly FastList<Entity> _entities;
            private int _currentIndex;

            public Enumerator(World world, FastList<Entity> entities)
            {
                _world = world;
                _entities = entities;
                _currentIndex = 0;
            }

            public Entity Current => _entities[_currentIndex];

            public bool MoveNext()
            {
                while (++_currentIndex < _entities.Count)
                {
                    if (_world.IsDisposed(_entities[_currentIndex]))
                    {
                        continue;
                    }

                    return true;
                }
                return false; 
            }
        }
    }
}