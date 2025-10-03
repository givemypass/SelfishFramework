using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SelfishFramework.Src.Features.Features.Serialization
{
    public class EmbeddedTypePolyConverter : JsonConverter
    {
        private readonly string _typeFieldName;
        private readonly Type _fieldType;

        public EmbeddedTypePolyConverter(Type fieldType)
        {
            _fieldType = fieldType;
            _typeFieldName = "type";
        }       
        
        public EmbeddedTypePolyConverter(Type fieldType, string typeFieldName)
        {
            _fieldType = fieldType;
            _typeFieldName = typeFieldName;
        }
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var obj = JObject.FromObject(value);
            obj.AddFirst(new JProperty(_typeFieldName, JsonPolyTypeCache.GetTypeName(value.GetType(), _fieldType)));
            obj.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(reader.TokenType == JsonToken.Null)
                return null;
            var jsonObject = JObject.Load(reader);
            var typeName = jsonObject.Value<string>(_typeFieldName);
            var targetType = JsonPolyTypeCache.FindTargetType(objectType, typeName);
            var result = jsonObject.ToObject(targetType, serializer);
            return result;    
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}