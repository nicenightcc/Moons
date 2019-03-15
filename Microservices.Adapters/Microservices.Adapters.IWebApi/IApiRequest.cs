using Microservices.Base;

namespace Microservices.Adapters.IWebApi
{
    public interface IApiRequest<TResponse> : IRequest where TResponse : IApiResponse, new()
    {
    }
}
