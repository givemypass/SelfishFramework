namespace SelfishFramework.Src.Core.DefaultUpdates
{
    public interface IUpdatable : ISystemAction
    {
        void Update();
    }
    
    public class UpdateDefaultModule : BaseSystemModule<IUpdatable>
    {
        public void UpdateAll()
        {
            foreach (var executor in executors)
            {
                executor.Update(); 
            }
        }
    }
}