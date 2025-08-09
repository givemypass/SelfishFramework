using NUnit.Framework;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Unity;
using SelfishFramework.Tests.EditMode.TestSystems;
using UnityEngine;

namespace SelfishFramework.Tests.EditMode
{
    public class SystemsTests
    {
        private SManager _sManager;
        private Actor _actor;

        [SetUp]
        public void SetUp()
        {
            _sManager?.Dispose();
            _sManager = new();
            SManager.Default.SystemModuleRegistry.RegisterModule(new UpdateDefaultModule());
            _actor = new GameObject().AddComponent<Actor>();
            _actor.Init(SManager.Default);
        }

        [TearDown]
        public void TearDown()
        {
            _sManager?.Dispose();
            Object.DestroyImmediate(_actor.gameObject);
        }

        [Test]
        [Order(0)]
        public void AddGetSystem()
        {
            _actor.Entity.AddSystem<TestSystemA>();
            Assert.True(_actor.Entity.TryGetSystem<TestSystemA>(out var system));
            Assert.True(system != null);
            Assert.True(system.Owner == _actor.Entity);
        }

        [Test]
        [Order(1)]
        public void UpdateSystem()
        {
            _actor.Entity.AddSystem<TestSystemA>();
            SManager.Default.SystemModuleRegistry.GetModule<UpdateDefaultModule>().UpdateAll();
            _actor.Entity.TryGetSystem(out TestSystemA system);
            Assert.True(system.TestValue > 0);
        }
        
        [Test]
        [Order(2)]
        public void RemoveSystem()
        {
            _actor.Entity.AddSystem<TestSystemA>();
            _actor.Entity.RemoveSystem<TestSystemA>();
            Assert.False(_actor.Entity.TryGetSystem<TestSystemA>(out _));
        }
    }
}