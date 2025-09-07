using NUnit.Framework;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Tests.EditMode.Commands;
using SelfishFramework.Tests.EditMode.TestComponents;
using SelfishFramework.Tests.EditMode.TestSystems;

namespace SelfishFramework.Tests.EditMode
{
    public class SystemsTests
    {
        private SManager _sManager;

        [SetUp]
        public void SetUp()
        {
            _sManager?.Dispose();
            _sManager = new();
        }

        [TearDown]
        public void TearDown()
        {
            _sManager?.Dispose();
        }

        [Test]
        [Order(0)]
        public void AddGetSystem()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.AddSystem<TestSystemA>();
            entity.Init();
            Assert.True(entity.TryGetSystem<TestSystemA>(out var system));
            Assert.True(system != null);
            Assert.True(system.Owner == entity);
        }

        [Test]
        [Order(1)]
        public void UpdateSystem()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.AddSystem<TestSystemA>();
            entity.Init();
            _sManager.Worlds[0].ModuleRegistry.GetModule<UpdateDefaultModule>().UpdateAll();
            entity.TryGetSystem(out TestSystemA system);
            Assert.True(system.TestValue > 0);
        }
        
        [Test]
        [Order(2)]
        public void RemoveSystem()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.AddSystem<TestSystemA>();
            entity.RemoveSystem<TestSystemA>();
            entity.Init();
            Assert.False(entity.TryGetSystem<TestSystemA>(out _));
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
            var entity = _sManager.Worlds[0].NewEntity();
            entity.Set(new TestComponentA { TestInt = 0 });
            entity.AddSystem<TestFilterSystem>();
            entity.Init();
            _sManager.Worlds[0].ModuleRegistry.GetModule<UpdateDefaultModule>().UpdateAll();
            var val = entity.Get<TestComponentA>().TestInt;
            Assert.True(val == 1);
        }

        [Test]
        [Order(5)]
        public void BuildFilter1()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            var filter = _sManager.Worlds[0].Filter
                .With<TestComponentA>()
                .Without<TestComponentB>()
                .Build();
            
            entity.Set(new TestComponentA { TestInt = 1 });
            entity.Init();
            filter.ForceUpdate();
            Assert.True(filter.SlowCount() == 1);
        }
        
        [Test]
        [Order(6)]
        public void BuildFilter2()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.Set(new TestComponentA { TestInt = 1 });
            entity.Init();
            
            var filter = _sManager.Worlds[0].Filter
                .With<TestComponentA>()
                .Without<TestComponentB>()
                .Build();
            Assert.True(filter.SlowCount() == 1);
            _sManager.Worlds[0].DelEntity(entity);
        }

        [Test]
        [Order(6)]
        public void FilterTestWithout()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.Init();
            entity.Set(new TestComponentA { TestInt = 1 });
            entity.Set(new TestComponentB { TestInt = 1 });

            var filter = _sManager.Worlds[0].Filter
                .With<TestComponentA>()
                .Without<TestComponentB>()
                .Build();
            
            Assert.True(filter.SlowCount() == 0);
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

        [Test]
        [Order(8)]
        public void GlobalCommandTest()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.AddSystem<TestCommandSystem>(); 
            entity.Init();
            _sManager.Worlds[0].Command(new TestGlobalCommand());
            Assert.True(entity.TryGetSystem(out TestCommandSystem system) && system.IsGlobalReacted);
        }
        
        [Test]
        [Order(9)]
        public void LocalCommandTest()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.AddSystem<TestCommandSystem>(); 
            entity.Init();
            entity.Command(new TestLocalCommand());
            Assert.True(entity.TryGetSystem(out TestCommandSystem system) && system.IsLocalReacted);
        }
        
        [Test]
        [Order(10)]
        public void DisposeSystemOnRemove()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.AddSystem<TestSystemA>();
            entity.Init();
            Assert.True(entity.TryGetSystem<TestSystemA>(out var system));
            Assert.True(system != null);
            entity.RemoveSystem<TestSystemA>();
            Assert.True(system.Disposed);
        }

        [Test]
        [Order(11)]
        public void DisposeSystemOnWorldDispose()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.AddSystem<TestSystemA>();
            entity.Init();
            Assert.True(entity.TryGetSystem<TestSystemA>(out var system));
            Assert.True(system != null);
            _sManager.Dispose();
            _sManager = null;
            Assert.True(system.Disposed);
        }

        [Test]
        [Order(12)]
        public void DisposeSystemOnEntityDelete()
        {
            var entity = _sManager.Worlds[0].NewEntity();
            entity.AddSystem<TestSystemA>();
            entity.Init();
            Assert.True(entity.TryGetSystem<TestSystemA>(out var system));
            Assert.True(system != null);
            _sManager.Worlds[0].DelEntity(entity);
            Assert.True(system.Disposed);
        }
    }
}