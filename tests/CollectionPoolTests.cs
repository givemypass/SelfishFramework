using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using SelfishFramework.Core;
using Tests.TestComponents;

public class CollectionPoolTests
{
    [SetUp]
    public void SetUp()
    {
         
    }
    // A Test behaves as an ordinary method
    [Test]
    public void AddGetHasRemove()
    {
        var world = new World();
        var pool = new ComponentPool<TestComponentA>(world);
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
