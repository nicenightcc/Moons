using System;

namespace Microservices.Base
{
    public interface IAdapter : IDefinition
    {
        string Name { get; }
        Type TargetType { get; }
    }
}
