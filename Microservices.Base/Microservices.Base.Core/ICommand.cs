using System;

namespace Microservices.Base.Core
{
    public interface ICommand : IDefinition
    {
        string Name { get; }
        Type Parent { get; }
        bool Execute(Action<string> output, params string[] args);
    }
}
