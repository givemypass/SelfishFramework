using NUnit.Framework;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.DefaultUpdates;
using SelfishFramework.Src.Unity.CustomUpdate;
using SelfishFramework.Tests.TestSystems;
using UnityEngine;

namespace SelfishFramework.Tests
{
    public class ActorSystemsTests
    {
        private ActorsManager _actorsManager;
        private Actor actor;

        [SetUp]
        public void SetUp()
        {
            _actorsManager?.Dispose();
            _actorsManager = new();
            ActorsManager.Default.SystemModuleRegistry.RegisterModule(new UpdateDefaultModule());
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
            ActorsManager.Default.SystemModuleRegistry.GetModule<UpdateDefaultModule>().UpdateAll();
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