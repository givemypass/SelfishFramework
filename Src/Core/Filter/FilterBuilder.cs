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
        private int[] _includedComponents;
        private int[] _excludedComponents;

        public static FilterBuilder Create(World world)
        {
            return new FilterBuilder
            {
                _world = world,
                _includesHash = default,
                _excludesHash = default,
                _includedComponents = ArrayPool<int>.Shared.Rent(Constants.MAX_INCLUDES),
                _excludedComponents = ArrayPool<int>.Shared.Rent(Constants.MAX_EXCLUDES),
            };
        }

        public FilterBuilder With<T>() where T : struct, IComponent
        {
            Validate<T>();

            return new FilterBuilder
            {
                _world = _world,
                _includesHash = LongHash.Combine(_includesHash, ComponentPool<T>.Info.Hash),
                _excludesHash = _excludesHash,
                _includedComponents = _includedComponents,
                _excludedComponents = _excludedComponents
            };
        }

        public FilterBuilder Without<T>() where T : struct, IComponent
        {
            Validate<T>();
            
            return new FilterBuilder
            {
                _world = _world,
                _includesHash = _includesHash,
                _excludesHash = LongHash.Combine(_excludesHash, ComponentPool<T>.Info.Hash),
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
                filter = new Filter(_world, _includedComponents, _excludedComponents);
                excludesFilters.Add(_excludesHash.Value, filter);
            }
            
            ArrayPool<int>.Shared.Return(_includedComponents);
            ArrayPool<int>.Shared.Return(_excludedComponents);
            return filter;
        }

        private void Validate<T>() where T : struct, IComponent
        {
            var componentId = ComponentPool<T>.Info.Index;
            for (int i = 0; i < Constants.MAX_INCLUDES; i++)
            {
                if (_includedComponents[i] == componentId)
                {
                    throw new InvalidOperationException($"Component {componentId} is already included in the filter.");
                }
            }

            for (int i = 0; i < Constants.MAX_EXCLUDES; i++)
            {
                if (_excludedComponents[i] == componentId)
                {
                    throw new InvalidOperationException($"Component {componentId} is already excluded from the filter.");
                }
            }
        }
    }
}