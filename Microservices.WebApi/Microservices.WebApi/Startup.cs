using Microservices.Adapters.IWebApi;
using Microservices.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            LoadServices();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Server", "Moos.Microservices.WebApi");
                await next();
            });

            //app.UseHsts();
            //app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "webapi",
                    template: "{path}/{api}/{ver?}",
                    defaults: new { controller = "WebApi", action = "Index" }
                );
            });
        }

        private void LoadServices()
        {
            WebApiAdapter apiAdapter = WebApiAdapter.Instance as WebApiAdapter;
            var handlers = IoCFac.Instance.GetClassList<IApiHandler>();
            foreach (var handler in handlers)
            {
                try
                {
                    apiAdapter.Register(handler);
                    var name = handler.FullName.ToLower();
                    if (name.EndsWith("handler"))
                        name = name.Substring(0, name.Length - 7);
                    ApiCache.Instance.Add(name, handler);
                }
                catch { }
            }
            apiAdapter.Build();
        }
    }
}
