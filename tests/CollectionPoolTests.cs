using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using SelfishFramework.Core;
using Tests.TestComponents;

namespace SelfishFramework.tests
{
    public class CollectionPoolTests
    {
        [SetUp]
        public void SetUp()
        {
            ActorsManager.RecreateInstance();
        }

        // A Test behaves as an ordinary method
        [Test]
        public void AddGetHasRemove()
        {
            var pool = new ComponentPool<TestComponentA>(ActorsManager.Default);
            var actorIdx = 1;
            ref var component = ref pool.Add(actorIdx);
            component.TestInt = 1;
            Assert.True(pool.Has(actorIdx));
            ref var component2 = ref pool.Get(actorIdx);
            Assert.True(component2.TestInt == 1);
            pool.Remove(actorIdx);
            Assert.False(pool.Has(actorIdx));
        }
    }
}