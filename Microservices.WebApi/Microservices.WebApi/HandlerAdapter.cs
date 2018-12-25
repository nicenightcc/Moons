using Microservices.Adapters;
using Microservices.Adapters.IWebApi;
using Microservices.Base;
using Microservices.Common;
using Microservices.IoC;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservices.WebApi
{
    public class HandlerAdapter : IoCAdapter, IHandlerAdapter
    {
        public HandlerAdapter() : base() { }

        public IResponse Execute(Type handlerType, IRequest request, HttpContext context = null)
        {
            IApiHandler handler = null;
            try
            {
                handler = this.Resolve<IApiHandler>(handlerType.BaseType) ?? throw new Exception("Handler Resolve Failed");
            }
            catch (Autofac.Core.Registration.ComponentNotRegisteredException e)
            {
                throw new Exception("未注册: ", e);
            }
            catch (Exception e)
            {
                throw new Exception("Resolve Error: ", e);
            }
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

        //public TResponse Execute<TRequest, TResponse>(Type handlerType, TRequest request, HttpContext context = null)
        //    where TRequest : class, IApiRequest
        //    where TResponse : class, IApiResponse
        //{
        //    ApiHandler<TRequest, TResponse> handler = null;
        //    try
        //    {
        //        handler = this.Resolve<ApiHandler<TRequest, TResponse>>() ?? throw new Exception();
        //    }
        //    catch (Autofac.Core.Registration.ComponentNotRegisteredException e)
        //    {
        //        throw new Exception("未注册: ", e);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Resolve Error: ", e);
        //    }
        //    if (context != null)
        //        handler.HttpContext = context;
        //    try
        //    {
        //        return handler.Execute(request);
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public TResponse Execute<TResponse>(Type handlerType, IApiRequest request, HttpContext context = null)
        //    where TResponse : class, IApiResponse
        //{
        //    IApiHandler handler = null;
        //    try
        //    {
        //        var requestType = request.GetType();
        //        var responseType = typeof(TResponse);
        //        var type = typeof(ApiHandler<,>).MakeGenericType(requestType, responseType);
        //        handler = this.Resolve<IApiHandler>(type) ?? throw new Exception();
        //    }
        //    catch (Autofac.Core.Registration.ComponentNotRegisteredException e)
        //    {
        //        throw new Exception("未注册: ", e);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Resolve Error: ", e);
        //    }
        //    if (context != null)
        //        handler.HttpContext = context;
        //    try
        //    {
        //        return handler.Execute(request) as TResponse;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public IHandlerAdapter Register(Type handlerType)
        {
            var paramTypes = handlerType.GetConstructors().FirstOrDefault().GetParameters().Select(p => p.ParameterType);
            var diParams = new List<DIParameter>();

            foreach (var pt in paramTypes)
            {
                var param = AdapterFac.Instance.GetAdapter(pt);
                if (param == null)
                {
                    Log.Logger.Error("Register Handler Error: Adapter Not Found: " + pt.Name);
                    //throw new Exception("Register Handler Error: Adapter Not Found: " + pt.Name);
                }
                diParams.Add(new DIParameter((p) => p.ParameterType == pt, param));
            }

            this.Register(handlerType, handlerType.BaseType, diParams);
            Log.Logger.Info($"Registed Handler: [{handlerType.FullName}]");
            return this;
        }

        public new IHandlerAdapter Build()
        {
            base.Build();
            return this;
        }
    }
}
