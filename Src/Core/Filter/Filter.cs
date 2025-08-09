using SelfishFramework.Src.Core.Collections;

namespace SelfishFramework.Src.Core.Filter
{
    public class Filter
    {
        private readonly World _world;
        private readonly FastList<int> _includes = new(Constants.MAX_INCLUDES);
        private readonly FastList<int> _excludes = new(Constants.MAX_INCLUDES);
        private readonly FastList<Entity> _entities = new(32);
        
        public Filter(World world)
        {
            _world = world;
        }
    }
}