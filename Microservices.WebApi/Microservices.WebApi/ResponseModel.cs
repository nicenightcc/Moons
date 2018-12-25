using Microservices.Adapters.IWebApi;
using Microservices.Base;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace Microservices.WebApi
{
    public class ResponseModel
    {
        public ResponseModel() { }
        public ResponseModel(ProcessCode code, string message = null, object content = null)
        {
            this.Code = code;
            this.Message = message;
            this.Content = content;
        }

        public ResponseModel(IApiResponse response)
        {
            var result = GenerateFunc(null, response);
            this.Code = result.Code;
            this.Message = result.Message;
            this.Content = result.Content;
        }

        public ResponseModel(object request, IApiResponse response)
        {
            var result = GenerateFunc(request, response);
            this.Code = result.Code;
            this.Message = result.Message;
            this.Content = result.Content;
        }

        public delegate ResponseModel ResponseDelegate(object request, IApiResponse response);
        public static ResponseDelegate GenerateFunc = (request, response) =>
        {
            var props = response.GetType().GetProperties().Where(p => p.Name != "Code" && p.Name != "Message");
            return new ResponseModel(response.Code, response.Message, props.ToDictionary(p => p.Name, p => p.GetValue(response)));
        };

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("code")]
        public ProcessCode Code { get; set; }

        [JsonProperty("content")]
        public object Content { get; set; }

        public static implicit operator ActionResult(ResponseModel response)
        {
            ContentResult result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(response),
                ContentType = "application/json;charset=UTF-8",
                StatusCode = (int)response.Code
            };
            return result;
        }

        public static implicit operator string(ResponseModel response)
        {
            return string.Format("[Code: {0}] - {1}", response.Code, response.Message);
        }
    }
}
