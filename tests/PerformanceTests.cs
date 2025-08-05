using System;
using System.Collections.Generic;
using NUnit.Framework;
using SelfishFramework.Src.Core;
using SelfishFramework.tests.TestSystems;
using Tests.TestComponents;
using UnityEngine;

namespace SelfishFramework.tests
{
    public class PerformanceTests
    {
        private Actor[] array;
        private const int Capacity = 1000000 / 2;
        private List<IUpdatable>[] localSystems;

        [SetUp]
        public void SetUp()
        {
            ActorsManager.RecreateInstance();
            localSystems = new List<IUpdatable>[Capacity];
            array = new Actor[Capacity];
            for (int i = 0; i < Capacity; i++)
            {
                var actor = new GameObject().AddComponent<Actor>();
                actor.Init(ActorsManager.Default);
                ref var componentA = ref actor.Add<TestComponentA>();
                componentA.TestIntArray = new int[1000];
                ref var componentB = ref actor.Add<TestComponentB>();
                componentB.TestIntArray = new int[1000];
                actor.AddSystem<TestSystemA>();
                actor.AddSystem<TestSystemB>();
                array[i] = actor;
                localSystems[i] = new List<IUpdatable>()
                {
                    new TestSystemA() {Owner = actor},
                    new TestSystemB() {Owner = actor}
                };
            }
        }

        [Test]
        public void NActors1ComponentLocalSystem()
        {
            Debug.Log($"Start performance test -- {nameof(NActors1ComponentLocalSystem)}");
            var time = DateTime.Now;
            ActorsManager.Default.Update();
            var timeInMillis = (DateTime.Now - time).TotalMilliseconds;
            Debug.Log($"End performance test -- {nameof(NActors1ComponentLocalSystem)}");
            Debug.Log($"Time {timeInMillis}");
        }

        [Test]
        public void NActors1ComponentGlobalSystem()
        {
            Debug.Log($"Start performance test -- {nameof(NActors1ComponentGlobalSystem)}");
            var time = DateTime.Now;
            
            array[0].TryGetSystem(out TestSystemA systemA);
            array[0].TryGetSystem(out TestSystemB systemB);
            for (int i = 0; i < Capacity; i++)
            {
                systemA.Run(array[i]);
            }
            for (int i = 0; i < Capacity; i++)
            {
                systemB.Run(array[i]);
            }
            
            var timeInMillis = (DateTime.Now - time).TotalMilliseconds;
            Debug.Log($"End performance test -- {nameof(NActors1ComponentGlobalSystem)}");
            Debug.Log($"Time {timeInMillis}");
        }
        
        [Test]
        public void NActors1ComponentLocalDefaultImplSystem()
        {
            Debug.Log($"Start performance test -- {nameof(NActors1ComponentGlobalSystem)}");
            var time = DateTime.Now;
            
            for (int i = 0; i < Capacity; i++)
            {
                for (var j = 0; j < localSystems[i].Count; j++)
                {
                    var localSystem = localSystems[i][j];
                    localSystem.Update();
                }
            }
            
            var timeInMillis = (DateTime.Now - time).TotalMilliseconds;
            Debug.Log($"End performance test -- {nameof(NActors1ComponentGlobalSystem)}");
            Debug.Log($"Time {timeInMillis}");
        }
    }
}