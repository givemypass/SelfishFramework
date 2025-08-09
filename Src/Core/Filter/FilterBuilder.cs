using System;
using System.Buffers;
using System.Collections.Generic;
using SelfishFramework.Src.Core.Components;

namespace SelfishFramework.Src.Core.Filter
{
    public struct FilterBuilder
    {
        private World _world;
        private LongHash _includes;
        private LongHash _excludes;
        private int[] _includedComponents;
        private int[] _excludedComponents;

        public static FilterBuilder Create(World world)
        {
            return new FilterBuilder
            {
                _world = world,
                _includes = default,
                _excludes = default,
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
                _includes = LongHash.Combine(_includes, ComponentPool<T>.Info.Hash),
                _excludes = _excludes,
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
                _includes = _includes,
                _excludes = LongHash.Combine(_excludes, ComponentPool<T>.Info.Hash),
                _includedComponents = _includedComponents,
                _excludedComponents = _excludedComponents,
            };
        }

        public Filter Build()
        {
            if (!_world.filters.TryGetValue(_includes.Value, out var excludesFilters))
            {
                excludesFilters = new Dictionary<long, Filter>();
                _world.filters.Add(_includes.Value, excludesFilters);
            }
            if(!excludesFilters.TryGetValue(_excludes.Value, out var filter))
            {
                filter = new Filter(_world);
                //todo init filter
                excludesFilters.Add(_excludes.Value, filter);
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