namespace SelfishFramework.Src.Core.SystemModules
{
    public interface IFixedUpdatable : ISystemAction
    {
        void FixedUpdate();
    }
    
    public class FixedUpdateModule : BaseModule<IFixedUpdatable>
    {
        public override int Priority => 600;

        public void UpdateAll()
        {
            foreach (var executor in executors)
            {
                executor.FixedUpdate(); 
            }
        }
    }
}