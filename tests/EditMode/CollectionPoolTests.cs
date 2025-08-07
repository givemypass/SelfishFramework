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
            var entityId = 1;
            pool.Set(entityId, new TestComponentA
            {
                TestInt = 1,
            });
            Assert.True(pool.Has(entityId));
            ref var component = ref pool.Get(entityId);
            Assert.True(component.TestInt == 1);
            pool.Remove(entityId);
            Assert.False(pool.Has(entityId));
        }
    }
}