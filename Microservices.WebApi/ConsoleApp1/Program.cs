using Microservices.Adapters;
using Microservices.Adapters.WebApi;
using Microservices.Common;
using Microservices.WebApi;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.Root.Load("config.json");
            new Microservices.Builder.ServiceBuilder().Load("TestService").UseWebApi().UseAdapter<WebApiAdpater>().Run();
        }
    }
}
