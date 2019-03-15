using System;

namespace Microservices.Common
{
    [Flags]
    public enum DataStatus : ushort
    {
        Invalid = 0,
        Normal = 1,
        Expired = 2,
        Locked = 4,
        Disabled = 8
    }
}
