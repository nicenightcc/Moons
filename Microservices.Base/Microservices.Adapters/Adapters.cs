using Microservices.Builder;
using Microservices.Common;
using System;

namespace Microservices.Adapters
{
    public static class Adapters
    {
        public static ServiceBuilder UseAdapter(this ServiceBuilder builder, Type adapterType)
        {
            builder.ConfigureServices((b) =>
            {
                Log.SetName("adapters");
                AdapterFac.Instance.Register(adapterType);
            });
            return builder;
        }
        public static ServiceBuilder UseAdapter<TAdapter>(this ServiceBuilder builder)
        {
            builder.ConfigureServices((b) =>
            {
                Log.SetName("adapters");
                AdapterFac.Instance.Register(typeof(TAdapter));
            });
            return builder;
        }
    }
}
