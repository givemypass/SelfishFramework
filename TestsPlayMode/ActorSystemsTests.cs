using System.Collections;
using NUnit.Framework;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Core.DefaultUpdates;
using SelfishFramework.Src.Unity.CustomUpdate;
using SelfishFramework.Tests.TestSystems;
using UnityEngine;
using UnityEngine.TestTools;

namespace SelfishFramework.Tests
{
    public class ActorSystemsTests
    {
        private MonoBehaviour coroutineRunner;
        private Actor actor;

        [SetUp]
        public void SetUp()
        {
            ActorsManager.RecreateInstance();
            ActorsManager.Default.SystemModuleRegistry.RegisterModule(new UpdateDefaultModule());
            var gameObject = new GameObject();
            coroutineRunner = gameObject.AddComponent<Actor>();
            var customUpdateModule = new CustomUpdateModule(coroutineRunner);
            ActorsManager.Default.SystemModuleRegistry.RegisterModule(customUpdateModule);
            actor = new GameObject().AddComponent<Actor>();
            actor.InitMode.InitWhen = Actor.InitModule.InitWhenMode.Manually;
            actor.Init(ActorsManager.Default);
        }

        [TearDown]
        public void TearDown()
        {
            ActorsManager.Default.Dispose();
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