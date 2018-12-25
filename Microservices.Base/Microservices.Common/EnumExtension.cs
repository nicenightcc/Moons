using System;

namespace Microservices.Common
{
    public static class EnumExtension
    {
        public static int ToInt(this Enum e)
        {
            return (int)(e as ValueType);
        }
    }
}
