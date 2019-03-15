using Microservices.Base;
using System;

namespace Microservices.Common
{
    public static class IDefinitionExtension
    {
        public static T ThrowIfNull<T>(this T value, string message = null) where T : IDefinition
        {
            return value != null ? value : throw new Exception(message == null ? message : (typeof(T).Name + " NotFound"));
        }
    }
}
