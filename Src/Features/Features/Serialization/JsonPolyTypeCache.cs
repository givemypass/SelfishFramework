using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SelfishFramework.Src.Features.Features.Serialization
{
    public static class JsonPolyTypeCache
    {
        private static readonly Dictionary<(Type baseType, string typeName), Type> _cache;
        private static readonly Dictionary<(Type targetType, Type baseType), string> _targetBaseToName;
    
        static JsonPolyTypeCache()
        {
            _cache = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Select(type => (target: type, attrs: type.GetCustomAttributes<JsonPolyTypeAttribute>()))
                .Where(pair => pair.attrs != null && pair.attrs.Any())
                .SelectMany(item => item.attrs, (item, attr) => (item.target, attr))
                .Select(pair => (targetType: pair.target, typeName: pair.attr.TypeName, baseType: pair.attr.BaseType))
                .ToDictionary(tuple => (tuple.baseType, tuple.typeName), tuple => tuple.targetType);
        
            _targetBaseToName = _cache.ToDictionary(pair => (pair.Value, pair.Key.baseType), pair => pair.Key.typeName);
        }

        public static Type FindTargetType(Type baseType, string typeName)
        {
            return _cache.GetValueOrDefault((baseType, typeName));
        }
    
        public static string GetTypeName(Type targetType, Type baseType)
        {
            return _targetBaseToName.GetValueOrDefault((targetType, baseType));
        }
    }
}