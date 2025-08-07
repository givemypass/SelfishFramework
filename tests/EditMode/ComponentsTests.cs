using NUnit.Framework;
using SelfishFramework.Src.Core;
using SelfishFramework.Tests.EditMode.TestComponents;
using UnityEngine;

namespace SelfishFramework.Tests.EditMode
{
    public class ComponentsTests
    {
        private SManager _sManager;
        private Entity _entity;

        [SetUp]
        public void SetUp()
        {
            _sManager?.Dispose();
            _sManager = new SManager();
            _entity = new GameObject().AddComponent<Entity>();
            _entity.Init(SManager.Default);
        }

        [TearDown]
        public void TearDown()
        {
            _sManager?.Dispose();
            Object.DestroyImmediate(_entity.gameObject);
        }
        
        [Test]
        [Order(0)]
        public void InitEntity()
        {
            Assert.True(_entity.Id > 0 && _entity.Generation > 0 && _entity.World != null);  
        }
        [Test]
        [Order(1)]
        public void AddGetComponent()
        {
            _entity.Set(new TestComponentA
            {
                TestInt = 1,
            });
            ref var component2 = ref _entity.Get<TestComponentA>();
            Assert.True(component2.TestInt == 1);           
        }
        [Test]
        [Order(2)]
        public void HasComponent()
        {
            _entity.Set(new TestComponentA());
            Assert.True(_entity.Contains<TestComponentA>());
        }
        [Test]
        [Order(3)]
        public void RemoveComponent()
        {
            _entity.Set(new TestComponentA());
            _entity.Remove<TestComponentA>();
            Assert.False(_entity.Contains<TestComponentA>());
        }
    }
}