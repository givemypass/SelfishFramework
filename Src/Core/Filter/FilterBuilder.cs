using System;
using System.Buffers;
using SelfishFramework.Src.Core.Components;

namespace SelfishFramework.Src.Core.Filter
{
    public struct FilterBuilder
    {
        internal World _world;
        internal LongHash _includes;
        internal LongHash _excludes;
        internal int[] _includedComponents;
        internal int[] _excludedComponents;

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
            var id = ComponentPool<T>.Info.Index;
            Check(id);

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
            var id = ComponentPool<T>.Info.Index;
            Check(id);
            _excludes = LongHash.Combine(_excludes, ComponentPool<T>.Info.Hash);
            return this;
        }

        public Filter Build()
        {
            ArrayPool<int>.Shared.Return(_includedComponents);
            ArrayPool<int>.Shared.Return(_excludedComponents);
        }

        private void Check(int componentId)
        {
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