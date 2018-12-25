using Microservices.Adapters;
using Microservices.Adapters.WebApi;
using Microservices.Common;
using Microservices.WebApi;
using System;

namespace SampleWebApi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Config.Root.Load("config.json");
            new Microservices.Builder.ServiceBuilder().Load("TestService").UseWebApi().UseAdapter<WebApiAdpater>().Run();
        }
    }
}
