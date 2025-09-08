using NUnit.Framework;
using SelfishFramework.Src.Core;
using SelfishFramework.Tests.EditMode.TestComponents;

namespace SelfishFramework.Tests.EditMode
{
    public class ComponentsTests
    {
        private SManager _sManager;

        [SetUp]
        public void SetUp()
        {
            _sManager?.Dispose();
            _sManager = new SManager();
        }

        [TearDown]
        public void TearDown()
        {
            _sManager?.Dispose();
        }
        
        [Test]
        [Order(0)]
        public void InitEntity()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            Assert.True(!_sManager.Worlds[0].IsDisposed(entity));  
        }
        
        [Test]
        [Order(1)]
        public void AddGetComponent()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.Set(new TestComponentA
            {
                TestInt = 1,
            });
            ref var component2 = ref entity.Get<TestComponentA>();
            Assert.True(component2.TestInt == 1);           
        }
        [Test]
        [Order(2)]
        public void HasComponent()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.Set(new TestComponentA());
            Assert.True(entity.Has<TestComponentA>());
        }
        [Test]
        [Order(3)]
        public void RemoveComponent()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.Set(new TestComponentA());
            entity.Remove<TestComponentA>();
            Assert.False(entity.Has<TestComponentA>());
        }

        [Test]
        [Order(4)]
        public void AddDuplicateComponent()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.Set(new TestComponentA
            {
                TestInt = 1,
            });
            entity.Set(new TestComponentA
            {
                TestInt = 2,
            });
            ref var component = ref entity.Get<TestComponentA>();
            Assert.True(component.TestInt == 2);
        }

        [Test]
        [Order(5)]
        public void DeleteEntity()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.Set(new TestComponentA());
            entity.Init();
            var id = entity.Id;
            Assert.True(!_sManager.Worlds[0].IsDisposed(entity));
            _sManager.Worlds[0].DelEntity(entity);
            Assert.True(_sManager.Worlds[0].IsDisposed(entity));
            Assert.False(entity.Has<TestComponentA>());
            Assert.False(_sManager.Worlds[0].GetComponentPool<TestComponentA>().Has(id));
        }
    }
}