using System.Collections;
using NUnit.Framework;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.SystemModules;
using SelfishFramework.Src.Unity.CustomUpdate;
using UnityEngine;
using UnityEngine.TestTools;

namespace SelfishFramework.Tests.PlayMode
{
    public class ActorSystemsTests
    {
        private SManager _sManager;
        private MonoBehaviour coroutineRunner;
        private Actor actor;

        [SetUp]
        public void SetUp()
        {
            _sManager?.Dispose();
            _sManager = new SManager();
            _sManager.World.SystemModuleRegistry.RegisterModule(new UpdateDefaultModule());
            var gameObject = new GameObject();
            coroutineRunner = gameObject.AddComponent<Actor>();
            var customUpdateModule = new CustomUpdateModule(coroutineRunner);
            SManager.Default.SystemModuleRegistry.RegisterModule(customUpdateModule);
            actor = new GameObject().AddComponent<Actor>();
            actor.InitMode.InitWhen = Actor.InitModule.InitWhenMode.Manually;
            actor.Init(SManager.Default);
        }

        [TearDown]
        public void TearDown()
        {
            SManager.Default.Dispose();
            if (actor != null) Object.DestroyImmediate(actor.gameObject);
            if (coroutineRunner != null) Object.DestroyImmediate(coroutineRunner.gameObject);
        }

        [UnityTest]
        [Order(3)]
        public IEnumerator CustomUpdateSystem()
        {
            actor.AddSystem<TestCustomUpdateSystem>();
            actor.TryGetSystem(out TestCustomUpdateSystem system);
            
            yield return new WaitForSeconds(2);
            Assert.True(system != null);
            Assert.True(system.TestValue > 0);
        }
    }
}