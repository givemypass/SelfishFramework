using NUnit.Framework;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;
using SelfishFramework.Tests.EditMode.TestComponents;
using UnityEngine;

namespace SelfishFramework.Tests.EditMode
{
    public class ComponentsTests
    {
        private SManager _sManager;
        private Actor _actor;

        [SetUp]
        public void SetUp()
        {
            _sManager?.Dispose();
            _sManager = new SManager();
            _actor = new GameObject().AddComponent<Actor>();
            _actor.Init(SManager.World);
            _actor.InitSystems();
        }

        [TearDown]
        public void TearDown()
        {
            _sManager?.Dispose();
            Object.DestroyImmediate(_actor.gameObject);
        }
        
        [Test]
        [Order(0)]
        public void InitEntity()
        {
            Assert.True(!_sManager.Worlds[0].IsDisposed(_actor.Entity));  
        }
        
        [Test]
        [Order(1)]
        public void AddGetComponent()
        {
            _actor.Entity.Set(new TestComponentA
            {
                TestInt = 1,
            });
            ref var component2 = ref _actor.Entity.Get<TestComponentA>();
            Assert.True(component2.TestInt == 1);           
        }
        [Test]
        [Order(2)]
        public void HasComponent()
        {
            _actor.Entity.Set(new TestComponentA());
            Assert.True(_actor.Entity.Has<TestComponentA>());
        }
        [Test]
        [Order(3)]
        public void RemoveComponent()
        {
            _actor.Entity.Set(new TestComponentA());
            _actor.Entity.Remove<TestComponentA>();
            Assert.False(_actor.Entity.Has<TestComponentA>());
        }

        [Test]
        [Order(4)]
        public void AddDuplicateComponent()
        {
            _actor.Entity.Set(new TestComponentA
            {
                TestInt = 1,
            });
            _actor.Entity.Set(new TestComponentA
            {
                TestInt = 2,
            });
            ref var component = ref _actor.Entity.Get<TestComponentA>();
            Assert.True(component.TestInt == 2);
        }
    }
}