using System;

namespace SelfishFramework.Src.Features.Features.Serialization
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class JsonPolyTypeAttribute : Attribute
    {
        public Type BaseType { get; }
        public string TypeName { get; }

        public JsonPolyTypeAttribute(Type baseType, string typeName)
        {
            BaseType = baseType;
            TypeName = typeName;
        } 
    }
}