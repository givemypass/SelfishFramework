using NUnit.Framework;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Tests.EditMode.TestSystems;
using UnityEngine;

namespace SelfishFramework.Tests.EditMode
{
    public class SystemsTests
    {
        private SManager _sManager;
        private Entity _entity;

        [SetUp]
        public void SetUp()
        {
            _sManager?.Dispose();
            _sManager = new();
            SManager.Default.SystemModuleRegistry.RegisterModule(new UpdateDefaultModule());
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
        public void AddGetSystem()
        {
            _entity.AddSystem<TestSystemA>();
            Assert.True(_entity.TryGetSystem<TestSystemA>(out var system));
            Assert.True(system != null);
            Assert.True(system.Owner == _entity);
        }

        [Test]
        [Order(1)]
        public void UpdateSystem()
        {
            _entity.AddSystem<TestSystemA>();
            SManager.Default.SystemModuleRegistry.GetModule<UpdateDefaultModule>().UpdateAll();
            _entity.TryGetSystem(out TestSystemA system);
            Assert.True(system.TestValue > 0);
        }
        
        [Test]
        [Order(2)]
        public void RemoveSystem()
        {
            _entity.AddSystem<TestSystemA>();
            _entity.RemoveSystem<TestSystemA>();
            Assert.False(_entity.TryGetSystem<TestSystemA>(out _));
        }
    }
}