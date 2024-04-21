using SelfishFramework.Core;

namespace SelfishFramework.tests.TestSystems
{
    public class TestSystemA : BaseSystem, IUpdatable
    {
        public int TestValue { get; set; }
        public void Update()
        {
            TestValue++;
        }
    }
}