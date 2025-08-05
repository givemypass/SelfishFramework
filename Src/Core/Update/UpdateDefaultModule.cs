namespace SelfishFramework.Src.Core.Update
{
    public interface IUpdatable : ISystemAction
    {
        void Update();
    }
    
    public class UpdateDefaultModule : BaseSystemModule<IUpdatable>
    {
        public void Update()
        {
            for (var i = 0; i < systems.Count; i++)
            {
                systems[i].Update();
            }
        }
    }
}