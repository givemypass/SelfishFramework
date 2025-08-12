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
        
        public int Count => _entities.Count;

        public Filter(World world, int[] includes, int[] excludes, int includesCount, int excludesCount)
        {
            _world = world;
            _includes = new FastList<int>(includes.Length);
            for (var i = 0; i < includesCount; i++)
            {
                _includes.Add(includes[i]);
            }
            _excludes = new FastList<int>(excludes.Length);
            for (var i = 0; i < excludesCount; i++)
            {
                _excludes.Add(excludes[i]);
            }
        }

        public void ForceUpdate()
        {
            UpdateFilter(_world.dirtyEntities);
        }
        
        public void UpdateFilter(HashSet<Entity> entities)
        {
            foreach (var entity in entities)
            {
                if (_world.IsDisposed(entity) || !MatchFilter(entity))
                {
                    _check.Remove(entity);
                }
                else
                {
                    _check.Add(entity);
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