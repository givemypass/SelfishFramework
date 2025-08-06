namespace SelfishFramework.Src.Core.DefaultUpdates
{
    public interface IFixedUpdatable : ISystemAction
    {
        void FixedUpdate();
    }
    
    public class FixedUpdateModule : BaseSystemModule<IFixedUpdatable>
    {
        public void UpdateAll()
        {
            foreach (var executor in executors)
            {
                executor.FixedUpdate(); 
            }
        }
    }
}