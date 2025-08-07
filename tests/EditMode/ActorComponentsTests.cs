using NUnit.Framework;
using SelfishFramework.Src.Core;
using SelfishFramework.Tests.EditMode.TestComponents;
using UnityEngine;

namespace SelfishFramework.Tests.EditMode
{
    public class ActorComponentsTests
    {
        private ActorsManager _actorsManager;
        private Actor actor;

        [SetUp]
        public void SetUp()
        {
            _actorsManager?.Dispose();
            _actorsManager = new ActorsManager();
            actor = new GameObject().AddComponent<Actor>();
            actor.Init(ActorsManager.Default);
        }

        [TearDown]
        public void TearDown()
        {
            _actorsManager?.Dispose();
            Object.DestroyImmediate(actor.gameObject);
        }
        
        [Test]
        [Order(0)]
        public void InitActor()
        {
            Assert.True(actor.Id > 0 && actor.Generation > 0 && actor.World != null);  
        }
        [Test]
        [Order(1)]
        public void AddGetComponent()
        {
            actor.Set(new TestComponentA
            {
                TestInt = 1,
            });
            ref var component2 = ref actor.Get<TestComponentA>();
            Assert.True(component2.TestInt == 1);           
        }
        [Test]
        [Order(2)]
        public void HasComponent()
        {
            actor.Set(new TestComponentA());
            Assert.True(actor.Contains<TestComponentA>());
        }
        [Test]
        [Order(3)]
        public void RemoveComponent()
        {
            actor.Set(new TestComponentA());
            actor.Remove<TestComponentA>();
            Assert.False(actor.Contains<TestComponentA>());
        }
    }
}