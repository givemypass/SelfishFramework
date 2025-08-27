namespace SelfishFramework.Src.Core.SystemModules
{
    public interface IPreUpdatable : ISystemAction
    {
        void PreUpdate();
    }
    
    public class PreUpdateModule : BaseSystemModule<IPreUpdatable>
    {
        public override int Priority => 400;

        public void UpdateAll()
        {
            foreach (var executor in executors)
            {
                executor.PreUpdate(); 
            }
        }
    }
}