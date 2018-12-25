using Microservices.Base;
using Microservices.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservices.Adapters
{
    public class AdapterFac
    {
        public static readonly AdapterFac Instance = new AdapterFac();
        protected List<IAdapter> adapters = new List<IAdapter>();
        protected AdapterFac() { }

        [Obsolete("使用ServiceBuilder.UserAdapter")]
        public AdapterFac Register(Type adapterType)
        {
            if (typeof(IAdapter).IsAssignableFrom(adapterType))
            {
                try
                {
                    if (adapterType.GetConstructors().Any(en => en.GetParameters().Count() == 0))
                        adapters.Add(CreateAdapter(adapterType));
                    else
                        Log.Logger.Error($"Register Adapter Error: [{adapterType.FullName}] Constructor Error");
                }
                catch (Exception e)
                {
                    Log.Logger.Error($"Register Adapter Error: [{adapterType.FullName}]", e);
                }
            }
            else
            {
                Log.Logger.Error($"Register Adapter Error: [{adapterType.FullName}] is not a adpater");
            }
            return this;
        }

        protected IAdapter CreateAdapter(Type type)
        {
            return Activator.CreateInstance(type) as IAdapter;
        }

        public IAdapter NewAdapter(string name)
        {
            var type = GetAdapter(name)?.GetType() ?? throw new Exception("Adapter is NotFound");
            return CreateAdapter(type);
        }

        public IAdapter NewAdapter(Type targetType)
        {
            var type = GetAdapter(targetType)?.GetType() ?? throw new Exception("Adapter is NotFound");
            return CreateAdapter(type);
        }

        public TAdapter NewAdapter<TAdapter>()
        {
            var type = GetAdapter<TAdapter>()?.GetType() ?? throw new Exception("Adapter is NotFound");
            return (TAdapter)CreateAdapter(type);
        }

        public IAdapter GetAdapter(string name)
        {
            return adapters.FirstOrDefault(en => en.Name == name);
        }

        public IAdapter GetAdapter(Type targetType)
        {
            return adapters.FirstOrDefault(en => en.TargetType == targetType);
        }

        public TAdapter GetAdapter<TAdapter>()
        {
            return (TAdapter)adapters.FirstOrDefault(en => en.TargetType == typeof(TAdapter));
        }

        public IList<IAdapter> GetAdapters(Type targetType)
        {
            return adapters.Where(en => en.TargetType == targetType)?.ToList();
        }

        public IList<TAdapter> GetAdapters<TAdapter>()
        {
            return adapters.Where(en => en.TargetType == typeof(TAdapter))?.Cast<TAdapter>()?.ToList();
        }

        public IList<IAdapter> GetAll()
        {
            return adapters;
        }
    }
}
