using Microservices.Adapters.IWebApi;
using Microservices.Base;
using Microservices.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Microservices.Adapters.WebApi
{
    public class WebApiAdapter : IApiAdapter
    {
        private Dictionary<string, string> hosts;
        private IApiAdapter apiAdapter;
        public string Name { get => "WebApiAdpater"; }
        public Type TargetType { get => typeof(IApiAdapter); }
        public WebApiAdapter()
        {
            this.hosts = Config.Root["WebApi"].GetConfig()?.GetChildren()?.ToDictionary(
                k => k.Key,
                v => v.Value + '/' + v.Key + '/'
            ) ?? new Dictionary<string, string>();
            var adptype = Type.GetType("Microservices.WebApi.WebApiAdapter, Microservices.WebApi");
            this.apiAdapter = adptype == null ? null : Activator.CreateInstance(adptype) as IApiAdapter;
        }

        private string GetUrl(Type type)
        {
            if (!hosts.ContainsKey(type.Namespace))
                throw new Exception($"[WebApi] Server for [{ type.Namespace }] is not found");
            return hosts[type.Namespace] + type.Name;
        }

        public TResponse Call<TRequest, TResponse>(TRequest request)
            where TRequest : IApiRequest<TResponse> where TResponse : class, IApiResponse, new()
        {
            var url = GetUrl(typeof(TRequest));
            var response = Post(url, SerializeRequest(request));
            return DeserializeResponse<TResponse>(response);
        }

        public TResponse Call<TResponse>(IApiRequest<TResponse> request)
            where TResponse : class, IApiResponse, new()
        {
            //先检查工程中是否存在这个api，有则直接调用
            if (this.apiAdapter != null && IoC.IoCFac.Instance.GetClass(request.GetType().FullName) != null)
                return apiAdapter.Execute(request);

            var url = GetUrl(request.GetType());
            var response = Post(url, SerializeRequest(request));
            return DeserializeResponse<TResponse>(response);
        }

        public async Task<TResponse> CallAsync<TRequest, TResponse>(TRequest request)
            where TRequest : IApiRequest<TResponse> where TResponse : class, IApiResponse, new()
        {
            return await Task.Run(() =>
            {
                if (this.apiAdapter != null && IoC.IoCFac.Instance.GetClass(request.GetType().FullName) != null)
                    return apiAdapter.Execute(request);

                var url = GetUrl(typeof(TRequest));
                var response = Post(url, SerializeRequest(request));
                return DeserializeResponse<TResponse>(response);
            });
        }

        public async Task<TResponse> CallAsync<TResponse>(IApiRequest<TResponse> request)
            where TResponse : class, IApiResponse, new()
        {
            return await Task.Run(() =>
            {
                if (this.apiAdapter != null && IoC.IoCFac.Instance.GetClass(request.GetType().FullName) != null)
                    return apiAdapter.Execute(request);

                var url = GetUrl(request.GetType());
                var response = Post(url, SerializeRequest(request));
                return DeserializeResponse<TResponse>(response);
            });
        }

        private string SerializeRequest<TResponse>(IApiRequest<TResponse> request) where TResponse : class, IApiResponse, new()
        {
            return JsonConvert.SerializeObject(request);
        }

        private TResponse DeserializeResponse<TResponse>(string response) where TResponse : class, IApiResponse, new()
        {
            var table = JsonConvert.DeserializeObject<Hashtable>(response);
            TResponse result;
            if (table.ContainsKey("content"))
                result = JsonConvert.DeserializeObject<TResponse>(table["content"].ToString());
            else
                result = new TResponse();
            result.Code = (ProcessCode)int.Parse(table["code"].ToString());
            result.Message = table["message"]?.ToString();
            return result;
        }

        private string Post(string url, string body)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                using (var writer = new StreamWriter(request.GetRequestStream()))
                    writer.Write(body);
                using (var reader = new StreamReader(request.GetResponse().GetResponseStream()))
                    return reader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        private async Task<string> PostAsync(string url, string body)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                using (var writer = new StreamWriter(request.GetRequestStream()))
                    writer.Write(body);
                using (var reader = new StreamReader((await request.GetResponseAsync()).GetResponseStream()))
                    return reader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        [Obsolete("方法无效", true)]
        TResponse IApiAdapter.Execute<TRequest, TResponse>(TRequest request, HttpContext context)
        {
            throw new NotImplementedException();
        }

        [Obsolete("方法无效", true)]
        TResponse IApiAdapter.Execute<TResponse>(IApiRequest<TResponse> request, HttpContext context)
        {
            throw new NotImplementedException();
        }

        [Obsolete("方法无效", true)]
        public IResponse Execute(IRequest request, HttpContext context = null)
        {
            throw new NotImplementedException();
        }
    }
}
