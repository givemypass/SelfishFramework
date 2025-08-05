using NUnit.Framework;
using SelfishFramework.Src.Core;
using SelfishFramework.tests.TestSystems;
using UnityEngine;

namespace SelfishFramework.tests
{
    public class ActorSystemsTests
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
        public void AddGetSystem()
        {
            actor.AddSystem<TestSystemA>();
            Assert.True(actor.TryGetSystem<TestSystemA>(out var system));
            Assert.True(system != null);
            Assert.True(system.Owner == actor);
        }

        [Test]
        [Order(1)]
        public void UpdateSystem()
        {
            actor.AddSystem<TestSystemA>();
            ActorsManager.Default.Update();
            actor.TryGetSystem(out TestSystemA system);
            Assert.True(system.TestValue > 0);
        }
        [Test]
        [Order(2)]
        public void RemoveSystem()
        {
            actor.AddSystem<TestSystemA>();
            actor.RemoveSystem<TestSystemA>();
            Assert.False(actor.TryGetSystem<TestSystemA>(out _));
        }
    }
}