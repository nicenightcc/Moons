using Microservices.Adapters.IWebApi;
using Microservices.Base;

namespace TestService
{
    public class Test2Request : IApiRequest<Test2Response>
    {
    }
    public class Test2Response : IApiResponse
    {
        public ProcessCode Code { get; set; }
        public string Message { get; set; }
    }
    public class Test2Handler : ApiHandler<Test2Request, Test2Response>
    {
        private IApiAdapter webApiAdapter;
        public Test2Handler(IApiAdapter webApiAdapter)
        {
            this.webApiAdapter = webApiAdapter;
        }
        public override Test2Response Execute(Test2Request request)
        {
            return new Test2Response { Code = ProcessCode.OK, Message = "aaa" };
        }
    }
}
