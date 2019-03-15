using Microservices.Base;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Microservices.Adapters.IWebApi
{
    public interface IApiAdapter : IAdapter
    {
        TResponse Call<TRequest, TResponse>(TRequest request)
            where TRequest : IApiRequest<TResponse> where TResponse : class, IApiResponse, new();
        TResponse Call<TResponse>(IApiRequest<TResponse> request)
            where TResponse : class, IApiResponse, new();
        Task<TResponse> CallAsync<TRequest, TResponse>(TRequest request)
            where TRequest : IApiRequest<TResponse> where TResponse : class, IApiResponse, new();
        Task<TResponse> CallAsync<TResponse>(IApiRequest<TResponse> request)
            where TResponse : class, IApiResponse, new();

        TResponse Execute<TRequest, TResponse>(TRequest request, HttpContext context = null)
            where TRequest : class, IApiRequest<TResponse>, new() where TResponse : class, IApiResponse, new();
        TResponse Execute<TResponse>(IApiRequest<TResponse> request, HttpContext context = null)
            where TResponse : class, IApiResponse, new();
        IResponse Execute(IRequest request, HttpContext context = null);
    }
}
