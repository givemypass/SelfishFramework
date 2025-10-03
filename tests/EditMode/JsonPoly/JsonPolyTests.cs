using Newtonsoft.Json;
using NUnit.Framework;

namespace SelfishFramework.Tests.EditMode.JsonPoly
{
    public class JsonPolyTests
    {
        [SetUp]
        public void SetUp()
        {
        }
        
        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void SerializeTestA()
        {
            var testA = new TestJsonPolyTypeA
            {
                ValueA = 42,
            };
            var serializable = new TestJsonPolySerializable
            {
                Value1 = testA,
                Value2 = testA,
            };
            var json = JsonConvert.SerializeObject(serializable);
            var deserialized = JsonConvert.DeserializeObject<TestJsonPolySerializable>(json);
            Assert.NotNull(deserialized);
            Assert.NotNull(deserialized.Value1);
            Assert.NotNull(deserialized.Value2);
            Assert.IsInstanceOf<TestJsonPolyTypeA>(deserialized.Value1);
            Assert.IsInstanceOf<TestJsonPolyTypeA>(deserialized.Value2);
            var deserialized1 = (TestJsonPolyTypeA)deserialized.Value1;
            var deserialized2 = (TestJsonPolyTypeA)deserialized.Value2;
            Assert.AreEqual(42, deserialized1.ValueA);
            Assert.AreEqual(42, deserialized2.ValueA);
        }
        
        [Test]
        public void SerializeTestB()
        {
            var testB = new TestJsonPolyTypeB
            {
                ValueB = "test",
            };
            var serializable = new TestJsonPolySerializable
            {
                Value1 = testB,
                Value2 = testB,
            };
            var json = JsonConvert.SerializeObject(serializable);
            var deserialized = JsonConvert.DeserializeObject<TestJsonPolySerializable>(json);
            Assert.NotNull(deserialized);
            Assert.NotNull(deserialized.Value1);
            Assert.NotNull(deserialized.Value2);
            Assert.IsInstanceOf<TestJsonPolyTypeB>(deserialized.Value1);
            Assert.IsInstanceOf<TestJsonPolyTypeB>(deserialized.Value2);
            var deserializedA = (TestJsonPolyTypeB)deserialized.Value1;
            var deserializedB = (TestJsonPolyTypeB)deserialized.Value2;
            Assert.AreEqual("test", deserializedA.ValueB);
            Assert.AreEqual("test", deserializedB.ValueB);
        }
        
        [Test]
        public void SerializeTestNull()
        {
            var serializable = new TestJsonPolySerializable
            {
                Value1 = null,
                Value2 = null,
            };
            var json = JsonConvert.SerializeObject(serializable);
            var deserialized = JsonConvert.DeserializeObject<TestJsonPolySerializable>(json);
            Assert.NotNull(deserialized);
            Assert.Null(deserialized.Value1);
            Assert.Null(deserialized.Value2);
        }
        
        [Test]
        public void SerializeTestMixed()
        {
            var testA = new TestJsonPolyTypeA
            {
                ValueA = 42,
            };
            var testB = new TestJsonPolyTypeB
            {
                ValueB = "test",
            };
            var serializable = new TestJsonPolySerializable
            {
                Value1 = testA,
                Value2 = testB,
            };
            var json = JsonConvert.SerializeObject(serializable);
            var deserialized = JsonConvert.DeserializeObject<TestJsonPolySerializable>(json);
            Assert.NotNull(deserialized);
            Assert.NotNull(deserialized.Value1);
            Assert.NotNull(deserialized.Value2);
            Assert.IsInstanceOf<TestJsonPolyTypeA>(deserialized.Value1);
            Assert.IsInstanceOf<TestJsonPolyTypeB>(deserialized.Value2);
            var deserializedA = (TestJsonPolyTypeA)deserialized.Value1;
            var deserializedB = (TestJsonPolyTypeB)deserialized.Value2;
            Assert.AreEqual(42, deserializedA.ValueA);
            Assert.AreEqual("test", deserializedB.ValueB);
        }
    }
}