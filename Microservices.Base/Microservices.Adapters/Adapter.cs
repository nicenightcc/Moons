using Microservices.Adapters;
using System;

namespace Microservices.Builder
{
    public static class Adapter
    {
        public static ServiceBuilder UseAdapter(this ServiceBuilder builder, Type adapterType)
        {
            AdapterFac.Instance.Register(adapterType);
            return builder;
        }
        public static ServiceBuilder UseAdapter<TAdapter>(this ServiceBuilder builder)
        {
            AdapterFac.Instance.Register(typeof(TAdapter));
            return builder;
        }
    }
}
