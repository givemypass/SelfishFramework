using System;

namespace SelfishFramework.Src.Core
{
    public static class IndexGenerator
    {
        public static int GenerateIndex(this string data)
        {
            return GetIndexForTypeName(data);
        }

        public static int GetIndexForType<T>()
        {
            var type = typeof(T);
            var typeName = type.Name;
            return GetIndexForTypeName(typeName);
        }

        public static int GetIndexForTypeName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException("Type name cannot be null or empty.", nameof(typeName));
            }

            unchecked
            {
                const int p = 16777619;
                var hash = (int)2166136261;

                for (int i = 0; i < typeName.Length; i++)
                {
                    hash = (hash ^ typeName[i]) * p;
                }

                hash += hash << 13;
                hash ^= hash >> 7;
                hash += hash << 3;
                hash ^= hash >> 17;
                hash += hash << 5;

                return hash;
            }
        }
    }

}