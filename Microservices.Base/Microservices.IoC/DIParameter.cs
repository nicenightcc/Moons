using System;
using System.Reflection;

namespace Microservices.IoC
{
    public class DIParameter
    {
        public DIParameter(Func<ParameterInfo, bool> predicate, object value)
        {
            this.Predicate = predicate ?? throw new Exception("predicate cannot be null");
            this.Value = value;
        }
        public Func<ParameterInfo, bool> Predicate { get; }
        public object Value { get; }
    }
}
