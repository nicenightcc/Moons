using Microservices.Adapters.IWebApi;
using Microservices.Base;
using Microservices.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace Microservices.WebApi
{
    public class WebApiController : Controller
    {
        IHandlerAdapter adapter = null;
        ApiCache cache = null;

        public WebApiController(IHandlerAdapter adapter, ApiCache cache)
        {
            Log.SetName("controller");
            this.adapter = adapter;
            this.cache = cache;
        }

        //[Route("/{path}/{api}/{ver?}")]
        public ActionResult Index(string path, string api, string ver = null)
        {
            #region 检查接口
            Type handlerType;
            Type requestType;
            Type responseType;

            try
            {
                var name = (path + "." + api).ToLower();
                if (name.EndsWith("request") || name.EndsWith("handler"))
                    name = name.Substring(0, name.Length - 7);

                if (cache.ContainsKey(name))
                    handlerType = cache[name];
                //if (handlerType == null || !typeof(IApiHandler).IsAssignableFrom(handlerType))
                else throw new Exception("接口不存在");

                var typeArgs = handlerType.BaseType.GenericTypeArguments;
                requestType = typeArgs.FirstOrDefault(en => typeof(IRequest).IsAssignableFrom(en));
                responseType = typeArgs.FirstOrDefault(en => typeof(IResponse).IsAssignableFrom(en));
            }
            catch (Exception e)
            {
                var result = new ResponseModel(ProcessCode.BadRequest, e.Message);
                Log.Logger.Error(result);
                return result;
            }
            //if (!string.IsNullOrEmpty(ver))
            //{
            //    var fieldCount = ver.Count(c => c == '.') + 1;
            //    if (fieldCount > 4)
            //    {
            //        var result = new ResponseModel { Code = ProcessCode.NotFound, Message = "版本格式错误" };
            //        Log.Logger.Error(result);
            //        return result;
            //    }
            //    var handlerVer = handlerType.Assembly.GetName().Version.ToString(fieldCount);
            //    if (ver != handlerVer)
            //    {
            //        var result = new ResponseModel { Code = ProcessCode.NotFound, Message = "版本不存在" };
            //        Log.Logger.Error(result);
            //        return result;
            //    }
            //}


            #endregion

            #region 生成Request
            IRequest request;
            var body = string.Empty;
            try
            {
                using (var reader = new StreamReader(HttpContext.Request.Body))
                    body = reader.ReadToEnd();
#if DEBUG
                if (string.IsNullOrWhiteSpace(body))
                    body = "{}";
#endif
                if (string.IsNullOrWhiteSpace(body)) throw new Exception("requset错误");

                try
                {
                    request = Newtonsoft.Json.JsonConvert.DeserializeObject(body ?? "", requestType) as IRequest;
                }
                catch (Exception e)
                {
                    throw new Exception("requset错误: " + e.Message);
                }

                if (request == null) throw new Exception("requset错误");
            }
            catch (Exception e)
            {
                var result = new ResponseModel(ProcessCode.BadRequest, e.Message);
                Log.Logger.Error(result);
                return result;
            }

            #endregion

            #region 执行

            //Log.Logger.InfoFormat("[{0}:{1}] 请求: [{2}, {3}, ver={4}]",
            //    HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
            //    HttpContext.Connection.RemotePort,
            //    path, api, ver ?? "<default>");
            try
            {
                var response = adapter.Execute(handlerType, request, HttpContext);

                ResponseModel result = new ResponseModel(request, response as IApiResponse);
                //Log.Logger.Info(result);
                return result;
            }
            catch (Exception e)
            {
                var result = new ResponseModel(ProcessCode.InternalServerError, e.InnerException?.Message ?? e.Message);
                Log.Logger.Error(result);
                return result;
            }
            #endregion
        }
    }
}
