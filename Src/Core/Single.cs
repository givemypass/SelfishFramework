using System;
using SelfishFramework.Src.Core.Components;

namespace SelfishFramework.Src.Core
{
    public readonly struct Single<T> where T : struct, IComponent
    {
        private readonly Filter.Filter _filter;

        public Single(World world)
        {
            _filter = world.Filter.With<T>().Build();
        }
        
        public bool Exists()
        {
            return _filter.IsNotEmpty();
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
        
        public ref T Get()
        {
            foreach (var entity in _filter)
            {
                return ref entity.Get<T>();
            }

            throw new Exception("Component of type " + typeof(T).Name + " not found in the world.");
        }

        public Entity GetEnt()
        {
            foreach (var entity in _filter)
            {
                return entity; 
            }

            return default;
        }

        public void ForceUpdate()
        {
            _filter.ForceUpdate();
        }
    }
}