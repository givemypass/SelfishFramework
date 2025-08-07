using NUnit.Framework;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.Components;
using SelfishFramework.Tests.EditMode.TestComponents;

namespace SelfishFramework.Tests.EditMode
{
    public class CollectionPoolTests
    {
        private SManager _sManager;
        
        [SetUp]
        public void SetUp()
        {
            _sManager?.Dispose();
            _sManager = new();
        }
        
        [TearDown]
        public void TearDown()
        {
            _sManager?.Dispose();
        }

        // A Test behaves as an ordinary method
        [Test]
        public void AddGetHasRemove()
        {
            var pool = new ComponentPool<TestComponentA>(SManager.Default, 32);
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