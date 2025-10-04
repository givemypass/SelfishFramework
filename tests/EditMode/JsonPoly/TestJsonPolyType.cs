using System;
using Newtonsoft.Json;
using SelfishFramework.Src.Features.Features.Serialization;

namespace SelfishFramework.Tests.EditMode.JsonPoly
{
    public interface ITestJsonPolyInterface
    {
    }
    public class TestJsonPolyBaseType : ITestJsonPolyInterface
    {
    }
    
    [JsonPolyType(typeof(ITestJsonPolyInterface), nameof(TestJsonPolyTypeA))]
    [JsonPolyType(typeof(TestJsonPolyBaseType), nameof(TestJsonPolyTypeA))]
    public class TestJsonPolyTypeA : TestJsonPolyBaseType
    {
        public int ValueA;
    }
    
    [JsonPolyType(typeof(ITestJsonPolyInterface), nameof(TestJsonPolyTypeB))]
    [JsonPolyType(typeof(TestJsonPolyBaseType), nameof(TestJsonPolyTypeB))]
    public class TestJsonPolyTypeB : TestJsonPolyBaseType
    {
        public string ValueB;
    }
    
    [JsonObject]
    [Serializable]
    public class TestJsonPolySerializable
    {
        [JsonProperty]
        [JsonConverter(typeof(EmbeddedTypePolyConverter), typeof(TestJsonPolyBaseType))]
        public TestJsonPolyBaseType Value1;
        [JsonProperty]
        [JsonConverter(typeof(EmbeddedTypePolyConverter), typeof(ITestJsonPolyInterface))]
        public ITestJsonPolyInterface Value2;
    }
}