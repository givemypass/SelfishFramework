using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Components;
using SelfishFramework.Src.Core.Filter;
using SelfishFramework.Src.SLogs;

namespace SelfishFramework.Src
{
    public class Single<T> where T : struct, IComponent
    {
        private readonly Filter _filter;

        public Single(World world)
        {
            _filter = world.Filter.With<T>().Build();
        }
        
        public bool Exists()
        {
            return _filter.Count > 0;
        }
        
        public ref T Get(out bool exists)
        {
            if (_filter.Count > 1)
            {
                SLog.LogError("There are multiple entities with the same single component type: " + typeof(T).Name);
            }
            
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
            if (_filter.Count > 1)
            {
                SLog.LogError("There are multiple entities with the same single component type: " + typeof(T).Name);
            }
            
            foreach (var entity in _filter)
            {
                return ref entity.Get<T>();
            }

            return ref ComponentPool<T>.Default;
        }

        public Entity GetEnt()
        {
            if (_filter.Count > 1)
            {
                SLog.LogError("There are multiple entities with the same single component type: " + typeof(T).Name);
            }
            
            foreach (var entity in _filter)
            {
                return entity; 
            }

            return default;
        }
    }
}