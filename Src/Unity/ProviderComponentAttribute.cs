using System;

namespace SelfishFramework.Src.Unity
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class ProviderComponentAttribute : Attribute
    {
        public Type ComponentType { get; }

        public ProviderComponentAttribute(Type componentType)
        {
            ComponentType = componentType;
        }
    }
}