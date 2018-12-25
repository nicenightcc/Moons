using Microservices.Base;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Microservices.Adapters.IWebApi
{
    public interface IApiHandler : IHandler
    {
        HttpContext HttpContext { get; set; }
        Task<IResponse> ExecuteAsync(IRequest request);
    }
}
