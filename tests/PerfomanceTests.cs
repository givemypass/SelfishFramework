using NUnit.Framework;
using SelfishFramework.Core;
using SelfishFramework.tests.TestSystems;
using Tests.TestComponents;
using UnityEngine;

namespace SelfishFramework.tests
{
    public class PerfomanceTests
    {
        private Actor[] array;
        private const int Capacity = 1000000;

        [SetUp]
        public void SetUp()
        {
            ActorsManager.RecreateInstance();
            array = new Actor[Capacity];
            for (int i = 0; i < Capacity; i++)
            {
                var actor = new GameObject().AddComponent<Actor>();
                actor.Init(ActorsManager.Default);
                actor.Add<TestComponentA>();
                actor.AddSystem<TestSystemA>();
                array[i] = actor;
            }
        }

        [Test]
        public void NActors1ComponentLocalSystem()
        {
            ActorsManager.Default.Update();
        }

        [Test]
        public void NActors1ComponentGlobalSystem()
        {
            array[0].TryGetSystem(out TestSystemA system);
            for (int i = 0; i < Capacity; i++)
            {
                system.Run(array[i]);
            }
        }
    }
}