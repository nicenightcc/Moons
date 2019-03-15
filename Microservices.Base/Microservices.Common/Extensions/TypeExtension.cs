using System;

namespace Microservices.Common
{
    public static class TypeExtension
    {
        public static bool IsAssignableTo<IType>(this Type type)
        {
            return typeof(IType).IsAssignableFrom(type);
        }
        public static bool IsAssignableTo(this Type type, Type iType)
        {
            return iType.IsAssignableFrom(type);
        }
    }
}
