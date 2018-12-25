using Microservices.Builder;
using Microservices.Common;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Microservices.WebApi
{
    public static class WebApi
    {
        public static ServiceBuilder UseWebApi(this ServiceBuilder builder)
        {
            builder.ConfigureServices((b) =>
            {
                Log.SetName("webapi");
                var host = WebHost.CreateDefaultBuilder()
                     .UseKestrel()
                     .UseStartup<Startup>();
                string urls = Config.Root["WebApi"]["urls"];
                if (!string.IsNullOrEmpty(urls))
                    host.UseUrls(urls);
                host.Build().Run();
            });
            return builder;
        }
    }
}
