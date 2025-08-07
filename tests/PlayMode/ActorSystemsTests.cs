using System.Collections;
using NUnit.Framework;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Unity.CustomUpdate;
using UnityEngine;
using UnityEngine.TestTools;

namespace SelfishFramework.Tests.PlayMode
{
    public class EntitySystemsTests
    {
        private SManager _sManager;
        private MonoBehaviour coroutineRunner;
        private Entity _entity;

        [SetUp]
        public void SetUp()
        {
            _sManager?.Dispose();
            _sManager = new SManager();
            _sManager.World.SystemModuleRegistry.RegisterModule(new UpdateDefaultModule());
            var gameObject = new GameObject();
            coroutineRunner = gameObject.AddComponent<Entity>();
            var customUpdateModule = new CustomUpdateModule(coroutineRunner);
            SManager.Default.SystemModuleRegistry.RegisterModule(customUpdateModule);
            _entity = new GameObject().AddComponent<Entity>();
            _entity.InitMode.InitWhen = Entity.InitModule.InitWhenMode.Manually;
            _entity.Init(SManager.Default);
        }

        [TearDown]
        public void TearDown()
        {
            SManager.Default.Dispose();
            if (_entity != null) Object.DestroyImmediate(_entity.gameObject);
            if (coroutineRunner != null) Object.DestroyImmediate(coroutineRunner.gameObject);
        }

        [UnityTest]
        [Order(3)]
        public IEnumerator CustomUpdateSystem()
        {
            _entity.AddSystem<TestCustomUpdateSystem>();
            _entity.TryGetSystem(out TestCustomUpdateSystem system);
            
            yield return new WaitForSeconds(2);
            Assert.True(system != null);
            Assert.True(system.TestValue > 0);
        }
    }
}