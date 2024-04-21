using NUnit.Framework;
using SelfishFramework.Core;
using Tests.TestComponents;
using UnityEngine;

namespace SelfishFramework.tests
{
    public class ActorComponentsTests
    {
        private Actor actor;

        [SetUp]
        public void SetUp()
        {
            ActorsManager.RecreateInstance();
            actor = new GameObject().AddComponent<Actor>();
            actor.Init(ActorsManager.Default);
        }

        [TearDown]
        public void TearDown()
        {
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
            ref var component = ref actor.AddComponent<TestComponentA>();
            component.TestInt = 1;
            ref var component2 = ref ActorExtensions.GetComponent<TestComponentA>(actor);
            Assert.True(component2.TestInt == 1);           
        }
        [Test]
        [Order(2)]
        public void HasComponent()
        {
            actor.AddComponent<TestComponentA>();
            Assert.True(actor.ContainMask<TestComponentA>());
        }
        [Test]
        [Order(3)]
        public void RemoveComponent()
        {
            actor.AddComponent<TestComponentA>();
            actor.RemoveComponent<TestComponentA>();
            Assert.False(actor.ContainMask<TestComponentA>());
        }
    }
}