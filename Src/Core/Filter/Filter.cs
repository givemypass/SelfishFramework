using SelfishFramework.Src.Core.Collections;

namespace SelfishFramework.Src.Core.Filter
{
    public class Filter
    {
        private readonly World _world;
        private readonly FastList<int> _includes;
        private readonly FastList<int> _excludes;
        private readonly FastList<Entity> _entities = new(32);
        
        public Filter(World world, int[] includes, int[] excludes)
        {
            _world = world;
            _includes = new FastList<int>(includes.Length);
            foreach (var include in includes)
            {
                _includes.Add(include);
            }
            _excludes = new FastList<int>(excludes.Length);
            foreach (var exclude in excludes)
            {
                _excludes.Add(exclude);
            }
        }
    }
}