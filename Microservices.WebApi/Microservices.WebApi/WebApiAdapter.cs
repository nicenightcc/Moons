using Microservices.Adapters;
using Microservices.Adapters.IWebApi;
using Microservices.Base;
using Microservices.Common;
using Microservices.IoC;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.WebApi
{
    public class WebApiAdapter : IoCAdapter, IApiAdapter
    {
        public static readonly IApiAdapter Instance = new WebApiAdapter();

        [Obsolete("方法无效", true)]
        public string Name => throw new NotImplementedException();

        [Obsolete("方法无效", true)]
        public Type TargetType => throw new NotImplementedException();

        public WebApiAdapter() { }

        public IResponse Execute(IRequest request, HttpContext context = null)
        {
            if (this != Instance)
                return Instance.Execute(request, context);
            IApiHandler handler = null;
            try
            {
                var name = request.GetType().FullName.ToLower();
                if (name.EndsWith("request"))
                    name = name.Substring(0, name.Length - 7);
                var type = ApiCache.Instance[name].BaseType;
                handler = this.Resolve<IApiHandler>(type);
            }
            catch (Exception e)
            {
                throw new ResolveException("Resolve Error: " + e.Message);
            }
            if (handler == null)
                throw new ResolveException("Handler Resolve Failed");
            if (context != null)
                handler.HttpContext = context;
            try
            {
                return handler.Execute(request);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TResponse Execute<TRequest, TResponse>(TRequest request, HttpContext context = null)
            where TRequest : class, IApiRequest<TResponse>, new()
            where TResponse : class, IApiResponse, new()
        {
            if (this != Instance)
                return Instance.Execute(request, context);
            ApiHandler<TRequest, TResponse> handler = null;
            try
            {
                handler = this.Resolve<ApiHandler<TRequest, TResponse>>() ?? throw new Exception();
            }
            catch (Exception e)
            {
                throw new ResolveException("Resolve Error: " + e.Message);
            }
            if (handler == null)
                throw new ResolveException("Handler Resolve Failed");
            if (context != null)
                handler.HttpContext = context;
            try
            {
                return handler.Execute(request);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TResponse Execute<TResponse>(IApiRequest<TResponse> request, HttpContext context = null)
            where TResponse : class, IApiResponse, new()
        {
            if (this != Instance)
                return Instance.Execute(request, context);
            IApiHandler handler = null;
            try
            {
                var type = typeof(ApiHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
                handler = this.Resolve<IApiHandler>(type) ?? throw new Exception();
            }
            catch (Exception e)
            {
                throw new ResolveException("Resolve Error: " + e.Message);
            }
            if (handler == null)
                throw new ResolveException("Handler Resolve Failed");
            if (context != null)
                handler.HttpContext = context;
            try
            {
                return handler.Execute(request) as TResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Obsolete("外部调用无效")]
        public IApiAdapter Register(Type handlerType)
        {
            var paramTypes = handlerType.GetConstructors().FirstOrDefault().GetParameters().Select(p => p.ParameterType);
            var diParams = new List<DIParameter>();

            foreach (var pt in paramTypes)
            {
                var param = AdapterFac.Instance.GetAdapter(pt);
                if (param == null)
                {
                    Log.Logger.Error("Register Handler Error: Adapter Not Found: " + pt.Name);
                }
                diParams.Add(new DIParameter((p) => p.ParameterType == pt, param));
            }
            this.Register(handlerType, handlerType.BaseType, diParams);
            Log.Logger.Info($"Registed Handler: [{handlerType.FullName}]");
            return this;
        }

        [Obsolete("外部调用无效")]
        public new IApiAdapter Build()
        {
            base.Build();
            return this;
        }

        [Obsolete("方法无效", true)]
        TResponse IApiAdapter.Call<TRequest, TResponse>(TRequest request)
        {
            throw new NotImplementedException();
        }

        [Obsolete("方法无效", true)]
        TResponse IApiAdapter.Call<TResponse>(IApiRequest<TResponse> request)
        {
            throw new NotImplementedException();
        }

        [Obsolete("方法无效", true)]
        Task<TResponse> IApiAdapter.CallAsync<TRequest, TResponse>(TRequest request)
        {
            throw new NotImplementedException();
        }

        [Obsolete("方法无效", true)]
        Task<TResponse> IApiAdapter.CallAsync<TResponse>(IApiRequest<TResponse> request)
        {
            throw new NotImplementedException();
        }
    }
}
