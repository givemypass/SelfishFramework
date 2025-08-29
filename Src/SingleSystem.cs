using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Components;
using SelfishFramework.Src.Core.Systems;

namespace SelfishFramework.Src
{
    public struct SingleSystem<T, TSystem> where T : struct, IComponent
        where TSystem : BaseSystem, new()
    {
        private Entity _entity;
        private readonly Single<T> _single;

        public SingleSystem(World world)
        {
            _single = new Single<T>(world);
            _entity = default;
        }

        public TSystem Get()
        {
            //todo add checks
            
            var entity = _single.GetEnt();
            if (!entity.Equals(_entity))
            {
                _entity = entity;
            }

            entity.TryGetSystem(out TSystem system);
            return system;
        }
    }
}