using Microservices.Adapters.IWebApi;
using Microservices.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Microservices.Adapters.WebApi
{
    public class WebApiAdpater : IApiAdapter
    {
        private Dictionary<string, string> hosts;
        public string Name { get => "WebApiAdpater"; }
        public Type TargetType { get => typeof(IApiAdapter); }
        public WebApiAdpater()
        {
            this.hosts = Config.Root["WebApi"].GetConfig()?.GetChildren()?.ToDictionary(
                k => k.Key,
                v => v.Value + '/' + v.Key + '/'
            );
            if (hosts == null)
                throw new Exception("Config Not Found: [WebApi]");
        }

        private string GetUrl(Type type)
        {
            if (!(hosts?.ContainsKey(type.Namespace) ?? false))
                throw new Exception($"[WebApi] Server for [{ type.Namespace }] is not found");
            return hosts[type.Namespace] + type.Name;
        }

        public TResponse Call<TRequest, TResponse>(TRequest request) where TRequest : IApiRequest<TResponse> where TResponse : IApiResponse
        {
            var url = GetUrl(typeof(TRequest));
            var response = Post(url, JsonConvert.SerializeObject(request));
            return JsonConvert.DeserializeObject<TResponse>(response);
        }

        public TResponse Call<TResponse>(IApiRequest<TResponse> request) where TResponse : IApiResponse
        {
            var url = GetUrl(request.GetType());
            var response = Post(url, JsonConvert.SerializeObject(request));
            return JsonConvert.DeserializeObject<TResponse>(response);
        }

        public async Task<TResponse> CallAsync<TRequest, TResponse>(TRequest request)
            where TRequest : IApiRequest<TResponse> where TResponse : IApiResponse
        {
            return await Task.Run(() =>
            {
                var url = GetUrl(typeof(TRequest));
                var response = Post(url, JsonConvert.SerializeObject(request));
                return JsonConvert.DeserializeObject<TResponse>(response);
            });
        }

        public async Task<TResponse> CallAsync<TResponse>(IApiRequest<TResponse> request) where TResponse : IApiResponse
        {
            return await Task.Run(() =>
            {
                var url = GetUrl(request.GetType());
                var response = Post(url, JsonConvert.SerializeObject(request));
                return JsonConvert.DeserializeObject<TResponse>(response);
            });
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
                throw e;
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
                throw e;
            }
        }
    }
}
