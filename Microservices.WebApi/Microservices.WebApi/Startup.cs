using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microservices.Adapters.IWebApi;
using Microservices.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Microservices.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.Builder = new ContainerBuilder();
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }
        private ContainerBuilder Builder { get; set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            Builder.RegisterType<WebApiController>();

            LoadServices();

            Builder.Populate(services);
            this.ApplicationContainer = Builder.Build();

            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
        {
            logger.AddConsole(LogLevel.Error);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                logger.AddDebug();
            }

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Server", "Moos.Microservices.WebApi");
                await next();
            });

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
            HandlerAdapter apiAdapter = new HandlerAdapter();
            ApiCache apiCache = new ApiCache();
            var handlers = IoCFac.Instance.GetClassList<IApiHandler>();
            foreach (var handler in handlers)
            {
                try
                {
                    apiAdapter.Register(handler);
                    var name = handler.FullName.ToLower();
                    if (name.EndsWith("handler"))
                        name = name.Substring(0, name.Length - 7);
                    apiCache.Add(name, handler);
                }
                catch { }
            }
            apiAdapter.Build();
            Builder.RegisterInstance(apiAdapter).As<IHandlerAdapter>().SingleInstance();
            Builder.RegisterInstance(apiCache).As<ApiCache>().SingleInstance();
        }
    }
}
