using System;

namespace Microservices.Common
{
    public static class TypeExtension
    {
        public static bool IsAssignableTo<ToType>(this Type type)
        {
            return typeof(ToType).IsAssignableFrom(type);
        }
        public static bool IsAssignableTo(this Type type, Type toType)
        {
            return toType.IsAssignableFrom(type);
        }
    }
}
