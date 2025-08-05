using NUnit.Framework;
using SelfishFramework.Src.Core;
using SelfishFramework.Tests.TestComponents;

namespace SelfishFramework.Tests
{
    public class CollectionPoolTests
    {
        [SetUp]
        public void SetUp()
        {
            ActorsManager.RecreateInstance();
        }

        // A Test behaves as an ordinary method
        [Test]
        public void AddGetHasRemove()
        {
            var pool = new ComponentPool<TestComponentA>(ActorsManager.Default, 32);
            var actorIdx = 1;
            pool.Set(actorIdx, new TestComponentA
            {
                TestInt = 1,
            });
            Assert.True(pool.Has(actorIdx));
            ref var component = ref pool.Get(actorIdx);
            Assert.True(component.TestInt == 1);
            pool.Remove(actorIdx);
            Assert.False(pool.Has(actorIdx));
        }
    }
}