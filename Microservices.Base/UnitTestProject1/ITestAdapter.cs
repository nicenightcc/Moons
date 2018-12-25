using Microservices.Base;

namespace TestService
{
    public interface ITestAdapter : IAdapter
    {
        string Test();
    }
}
