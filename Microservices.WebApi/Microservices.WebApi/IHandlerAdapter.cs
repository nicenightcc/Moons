using Microservices.Base;
using Microsoft.AspNetCore.Http;
using System;

namespace Microservices.WebApi
{
    public interface IHandlerAdapter
    {
        //TResponse Execute<TRequest, TResponse>(Type handlerType, TRequest request, HttpContext context = null)
        //    where TRequest : class, IApiRequest where TResponse : class, IApiResponse;
        //TResponse Execute<TResponse>(Type handlerType, IApiRequest request, HttpContext context = null)
        //    where TResponse : class, IApiResponse;
        IResponse Execute(Type handlerType, IRequest request, HttpContext context = null);
        IHandlerAdapter Register(Type handlerType);
        IHandlerAdapter Build();
    }
}
