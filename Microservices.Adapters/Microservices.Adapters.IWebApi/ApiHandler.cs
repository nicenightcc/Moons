using Microservices.Base;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Microservices.Adapters.IWebApi
{
    public abstract class ApiHandler<TRequest, TResponse> : IApiHandler where TRequest : IApiRequest<TResponse> where TResponse : IApiResponse, new()
    {
        public HttpContext HttpContext { get; set; }

        public abstract TResponse Execute(TRequest request);

        public virtual async Task<IResponse> ExecuteAsync(IRequest request)
        {
            return await Task.Run(() => { return Execute((TRequest)request); });
        }

        public IResponse Execute(IRequest request)
        {
            return Execute((TRequest)request);
        }
    }
}
