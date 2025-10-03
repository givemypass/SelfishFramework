using NUnit.Framework;
using SelfishFramework.Src.Core;
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

        [Test]
        public void AddGetHasRemove()
        {
            var pool = _sManager.Worlds[0].GetComponentPool<TestComponentA>();
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