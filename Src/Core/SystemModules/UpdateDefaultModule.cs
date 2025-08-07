namespace SelfishFramework.Src.Core.SystemModules
{
    public interface IUpdatable : ISystemAction
    {
        void Update();
    }
    
    public class UpdateDefaultModule : BaseSystemModule<IUpdatable>
    {
        public override int Priority => 500;

        public void UpdateAll()
        {
            foreach (var executor in executors)
            {
                executor.Update(); 
            }
        }
    }

}