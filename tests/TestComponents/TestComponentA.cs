using IComponent = SelfishFramework.Core.IComponent;

namespace Tests.TestComponents
{
    public struct TestComponentA : IComponent
    {
        public int TestInt;
        public int[] TestIntArray;
    }
}