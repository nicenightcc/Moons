using System;

namespace TestService
{
    public class TestAdapter : ITestAdapter
    {
        public string Name => "TestAdapter";

        public Type TargetType => typeof(ITestAdapter);

        public string Test()
        {
            return this.Name;
        }
    }
}
