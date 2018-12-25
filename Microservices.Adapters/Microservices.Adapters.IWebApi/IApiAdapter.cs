using Microservices.Base;
using System.Threading.Tasks;

namespace Microservices.Adapters.IWebApi
{
    public interface IApiAdapter : IAdapter
    {
        TResponse Call<TRequest, TResponse>(TRequest request)
            where TRequest : IApiRequest<TResponse> where TResponse : IApiResponse;
        TResponse Call<TResponse>(IApiRequest<TResponse> request) where TResponse : IApiResponse;
        //IApiResponse Call(IApiRequest request);
        Task<TResponse> CallAsync<TRequest, TResponse>(TRequest request)
           where TRequest : IApiRequest<TResponse> where TResponse : IApiResponse;
        Task<TResponse> CallAsync<TResponse>(IApiRequest<TResponse> request) where TResponse : IApiResponse;
        //Task<IApiResponse> CallAsync(IApiRequest request);
    }
}
