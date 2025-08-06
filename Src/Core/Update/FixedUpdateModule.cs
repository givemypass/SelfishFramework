namespace SelfishFramework.Src.Core.Update
{
    public interface IFixedUpdatable : ISystemAction
    {
        void FixedUpdate();
    }
    
    public class FixedUpdateModule : BaseSystemModule<IFixedUpdatable>
    {
        public override void UpdateAll()
        {
            foreach (var executor in executors)
            {
                executor.FixedUpdate(); 
            }
        }
    }
}