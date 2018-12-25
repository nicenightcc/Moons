using Microservices.Adapters.IWebApi;
using Microservices.Base;

namespace TestService
{
    public class TestRequest : IApiRequest<TestResponse>
    {
    }
    public class TestHandler : ApiHandler<TestRequest, TestResponse>
    {
        private IApiAdapter webApiAdapter;
        public TestHandler(IApiAdapter webApiAdapter)
        {
            this.webApiAdapter = webApiAdapter;
        }
        public override TestResponse Execute(TestRequest request)
        {
            var a = webApiAdapter.Call(new Test2Request { Message = "Hello World!" });
            return new TestResponse
            {
                Code = ProcessCode.OK,
                Message = a.Message
            };
        }
    }
    public class TestResponse : IApiResponse
    {
        public ProcessCode Code { get; set; }
        public string Message { get; set; }
    }
}
