using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Components;
using SelfishFramework.Src.Core.Filter;

namespace SelfishFramework.Src
{
    public readonly struct SingleComponent<T> where T : struct, IComponent
    {
        private readonly Filter _filter;

        public SingleComponent(World world)
        {
            _filter = world.Filter.With<T>().Build();
        }
        
        public ref T Get(out bool exists)
        {
            foreach (var entity in _filter)
            {
                exists = true;
                return ref entity.Get<T>();
            }

            exists = false;
            return ref ComponentPool<T>.Default;
        }
    }
}