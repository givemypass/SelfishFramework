using System.Collections;
using NUnit.Framework;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;
using SelfishFramework.Src.Unity.CustomUpdate;
using UnityEngine;
using UnityEngine.TestTools;

namespace SelfishFramework.Tests.PlayMode
{
    public class EntitySystemsTests
    {
        private SManager _sManager;
        private MonoBehaviour _coroutineRunner;
        private Actor _actor;

        [SetUp]
        public void SetUp()
        {
            _sManager?.Dispose();
            _sManager = new SManager();
            var gameObject = new GameObject();
            _coroutineRunner = gameObject.AddComponent<Actor>();
            var customUpdateModule = new CoroutineUpdateModule(_coroutineRunner);
            SManager.World.ModuleRegistry.RegisterModule(customUpdateModule);
            _actor = new GameObject().AddComponent<Actor>();
            _actor.InitMode.InitWhen = InitModule.InitWhenMode.Manually;
            _actor.Init(SManager.World);
            _actor.InitSystems();
        }

        [TearDown]
        public void TearDown()
        {
            SManager.World.Dispose();
            if (_actor != null) Object.DestroyImmediate(_actor.gameObject);
            if (_coroutineRunner != null) Object.DestroyImmediate(_coroutineRunner.gameObject);
        }

        [UnityTest]
        [Order(3)]
        public IEnumerator CustomUpdateSystem()
        {
            _actor.Entity.AddSystem<TestCustomUpdateSystem>();
            _actor.Entity.TryGetSystem(out TestCustomUpdateSystem system);
            
            yield return new WaitForSeconds(2);
            Assert.True(system != null);
            Assert.True(system.TestValue > 0);
        }
    }
}