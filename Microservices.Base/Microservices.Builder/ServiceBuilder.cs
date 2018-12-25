using Microservices.Common;
using Microservices.IoC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservices.Builder
{
    public class ServiceBuilder
    {
        public ServiceBuilder()
        {
            Log.SetName("main");
        }

        private List<Action<ServiceBuilder>> configure = new List<Action<ServiceBuilder>>();
        public ServiceBuilder ConfigureServices(Action<ServiceBuilder> configureServices)
        {
            configure.Add(configureServices);
            return this;
        }

        public ServiceBuilder Load(string path)
        {
            IoCFac.Instance.Load(path);
            return this;
        }

        public void Run()
        {
            var tasks = new List<Task>();
            foreach (var action in configure)
            {
                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        action.Invoke(this);
                    }
                    catch (Exception e)
                    {
                        Log.Logger.Error("Server Error", e);
                    }
                }));
            }
            Log.Logger.Info("Server Start");
            Task.WaitAll(tasks.ToArray());
        }
    }
}