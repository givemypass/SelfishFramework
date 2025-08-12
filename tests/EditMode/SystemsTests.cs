using NUnit.Framework;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Unity;
using SelfishFramework.Tests.EditMode.TestComponents;
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
            _actor = new GameObject().AddComponent<Actor>();
            _actor.Init(SManager.World);
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
            _sManager.Worlds[0].SystemModuleRegistry.GetModule<UpdateDefaultModule>().UpdateAll();
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
        
        [Test]
        [Order(3)]
        public void FilterCache()
        {
            var filter1 = _sManager.Worlds[0].Filter.With<TestComponentA>().Build();
            var filter2 = _sManager.Worlds[0].Filter.With<TestComponentA>().Build();
            Assert.True(filter1 == filter2);
        }

        [Test]
        [Order(4)]
        public void IterateFilter()
        {
            _actor.Entity.Set(new TestComponentA { TestInt = 1 });
            _actor.Entity.AddSystem<TestFilterSystem>();
            _sManager.Worlds[0].SystemModuleRegistry.GetModule<UpdateDefaultModule>().UpdateAll();
            var val = _actor.Entity.Get<TestComponentA>().TestInt;
            Assert.True(val == 1);
        }

        [Test]
        [Order(5)]
        public void BuildFilter1()
        {
            var filter = _sManager.Worlds[0].Filter
                .With<TestComponentA>()
                .Without<TestComponentB>()
                .Build();
            
            _actor.Entity.Set(new TestComponentA { TestInt = 1 });
            filter.ForceUpdate();
            Assert.True(filter.Count == 1);
        }
        
        [Test]
        [Order(6)]
        public void BuildFilter2()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.Set(new TestComponentA { TestInt = 1 });
            
            var filter = _sManager.Worlds[0].Filter
                .With<TestComponentA>()
                .Without<TestComponentB>()
                .Build();
            Assert.True(filter.Count == 1);
            _sManager.Worlds[0].DelEntity(entity);
        }

        [Test]
        [Order(6)]
        public void FilterTestWithout()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.Set(new TestComponentA { TestInt = 1 });
            entity.Set(new TestComponentB { TestInt = 1 });

            var filter = _sManager.Worlds[0].Filter
                .With<TestComponentA>()
                .Without<TestComponentB>()
                .Build();
            
            Assert.True(filter.Count == 0);
            _sManager.Worlds[0].DelEntity(entity);
        }
        
        [Test]
        [Order(7)]
        public void AfterEntityInit()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.AddSystem<TestSystemAfterEntityInit>();
            Assert.True(!entity.IsInitialized() && entity.TryGetSystem(out TestSystemAfterEntityInit system) && system.TestValue == 0);
            entity.Init();
            Assert.True(entity.IsInitialized() && entity.TryGetSystem(out system) && system.TestValue == 1);
            _sManager.Worlds[0].DelEntity(entity);
        }
    }
}