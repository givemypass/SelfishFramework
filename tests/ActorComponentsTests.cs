using NUnit.Framework;
using SelfishFramework.Core;
using Tests.TestComponents;
using UnityEngine;

namespace SelfishFramework.tests
{
    public class ActorComponentsTests
    {
        private Actor actor;

        [SetUp]
        public void SetUp()
        {
            ActorsManager.RecreateInstance();
            actor = new GameObject().AddComponent<Actor>();
            actor.Init(ActorsManager.Default);
        }
        [Test]
        public void AddGetHasRemove()
        {
            ref var component = ref actor.Add<TestComponentA>();
            component.TestInt = 1;
            Assert.True(actor.Has<TestComponentA>());
            ref var component2 = ref actor.Get<TestComponentA>();
            Assert.True(component2.TestInt == 1);
            actor.Remove<TestComponentA>();
            Assert.False(actor.Has<TestComponentA>());
        }
    }
}