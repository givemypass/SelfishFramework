using SelfishFramework.Src.Core.Systems;
using SelfishFramework.Src.Unity.CustomUpdate;
using UnityEngine;

namespace SelfishFramework.Tests.TestSystems
{
    public class TestCustomUpdateSystem : BaseSystem, ICustomUpdatable
    {
        public int TestValue { get; set; }

        public YieldInstruction Interval { get; } = new WaitForSeconds(1);
                
        public void CustomUpdate()
        {
            TestValue++;
        }
    }
}