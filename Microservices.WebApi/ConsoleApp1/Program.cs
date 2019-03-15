using Microservices.Builder;
using Microservices.Common;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.Root.Load("config.json");
            new Microservices.Builder.ServiceBuilder()
            .LoadAssembly("TestService")
            .UseAdapter<Microservices.Adapters.WebApi.WebApiAdapter>()
            .UseWebApi()
            .Run();
        }
    }
}
