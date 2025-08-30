using System;
using System.Buffers;
using System.Collections.Generic;
using SelfishFramework.Src.Core.Components;

namespace SelfishFramework.Src.Core.Filter
{
    public struct FilterBuilder
    {
        private World _world;
        private LongHash _includesHash;
        private LongHash _excludesHash;
        private int _includesCount;
        private int _excludesCount;
        private int[] _includedComponents;
        private int[] _excludedComponents;

        public static FilterBuilder Create(World world)
        {
            return new FilterBuilder
            {
                _world = world,
                _includesHash = default,
                _excludesHash = default,
                _includesCount = 0,
                _excludesCount = 0,
                _includedComponents = ArrayPool<int>.Shared.Rent(Constants.MAX_INCLUDES),
                _excludedComponents = ArrayPool<int>.Shared.Rent(Constants.MAX_EXCLUDES),
            };
        }

        public FilterBuilder With<T>() where T : struct, IComponent
        {
            if (_includesCount >= Constants.MAX_INCLUDES)
            {
                throw new InvalidOperationException($"Cannot include more than {Constants.MAX_INCLUDES} components in a filter.");
            }
            
            var componentId = _world.GetComponentPool<T>().GetId();
            CheckDuplicates(componentId);
            _includedComponents[_includesCount++] = componentId;
            return new FilterBuilder
            {
                _world = _world,
                _includesHash = LongHash.Combine(_includesHash, ComponentPool<T>.Info.Hash),
                _excludesHash = _excludesHash,
                _includesCount = _includesCount,
                _excludesCount = _excludesCount,
                _includedComponents = _includedComponents,
                _excludedComponents = _excludedComponents,
            };
        }

        public FilterBuilder Without<T>() where T : struct, IComponent
        {
            if (_excludesCount >= Constants.MAX_EXCLUDES)
            {
                throw new InvalidOperationException($"Cannot exclude more than {Constants.MAX_EXCLUDES} components from a filter.");
            }
            
            var componentId = _world.GetComponentPool<T>().GetId();
            CheckDuplicates(componentId);
            _excludedComponents[_excludesCount++] = componentId;
            return new FilterBuilder
            {
                _world = _world,
                _includesHash = _includesHash,
                _excludesHash = LongHash.Combine(_excludesHash, ComponentPool<T>.Info.Hash),
                _includesCount = _includesCount,
                _excludesCount = _excludesCount,
                _includedComponents = _includedComponents,
                _excludedComponents = _excludedComponents,
            };
        }

        public Filter Build()
        {
            if (!_world.filters.TryGetValue(_includesHash.Value, out var excludesFilters))
            {
                excludesFilters = new Dictionary<long, Filter>();
                _world.filters.Add(_includesHash.Value, excludesFilters);
            }
            if(!excludesFilters.TryGetValue(_excludesHash.Value, out var filter))
            {
                filter = new Filter(_world, _includedComponents, _excludedComponents, _includesCount, _excludesCount);
                //todo fill by entities
                excludesFilters.Add(_excludesHash.Value, filter);
                // Span<int> toCache = stackalloc int[_world.entities.Count];
                // int i = 0;
                // foreach (var entity in _world.entities)
                // {
                //     toCache[i++] = entity.Id;
                // }
                filter.UpdateFilter(_world.entities);
            }
            
            ArrayPool<int>.Shared.Return(_includedComponents);
            ArrayPool<int>.Shared.Return(_excludedComponents);
            return filter;
        }

        private void CheckDuplicates(int componentId)
        {
            for (int i = 0; i < _includesCount; i++)
            {
                if (_includedComponents[i] == componentId)
                {
                    throw new InvalidOperationException($"Component {componentId} is already included in the filter.");
                }
            }

            for (int i = 0; i < _excludesCount; i++)
            {
                if (_excludedComponents[i] == componentId)
                {
                    throw new InvalidOperationException($"Component {componentId} is already excluded from the filter.");
                }
            }
        }
    }
}