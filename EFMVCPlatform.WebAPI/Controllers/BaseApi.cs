using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApiServer.Controllers
{
    [Filter.TransferFilter]
    public class BaseApi: ApiController
    {
        /// <summary>
        /// 数据解密的方法代理
        /// </summary>
        public Func<string,string, string> DESDecode { get; set; }
        /// <summary>
        /// 请求头里包含的用户信息
        /// </summary>
        public object CurrentUser { get; set; }
        public BaseApi()
        {
            DESDecode = (msg,key)=> { return msg; };
        }
        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            try
            {
                ContentData cData = controllerContext.Request.Content.ReadAsAsync<ContentData>().Result;
                IEnumerable<KeyValuePair<string, string>> keyValue = controllerContext.Request.GetQueryNameValuePairs();
                KeyValuePair<string,string> encodeStrParas = keyValue.Where(m => m.Key == "p").FirstOrDefault();
                string urlParas = "";
                string desKey = ClientInfoCache.GetClientDesKey(cData.ClientId);
                if (encodeStrParas.Key =="p"&&!string.IsNullOrEmpty(encodeStrParas.Value))
                {
                    urlParas = DESDecode(encodeStrParas.Value, desKey);
                }
                if(!string.IsNullOrEmpty(urlParas))
                {
                    urlParas = "?" + urlParas;// System.Web.HttpUtility.HtmlEncode();
                }
                controllerContext.Request.RequestUri = new Uri(controllerContext.Request.RequestUri.AbsoluteUri.Split('?')[0] + urlParas);
                if (cData!=null)
                {
                    Type type = typeof(object);
                    string strDecodeData = cData.Data;
                    if (!string.IsNullOrEmpty(desKey))
                    {
                        strDecodeData = DESDecode(strDecodeData, desKey);
                    }
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject(strDecodeData, type);

                    MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

                    HttpContent strCtn = new ObjectContent(type, data, formatter);

                    controllerContext.Request.Content = strCtn; 
                }
                Task<HttpResponseMessage> task = base.ExecuteAsync(controllerContext, cancellationToken);
                return task;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        private class ContentData
        {
            public string ClientId { get; set; }
            public string Data { get; set; }
            public string DataType { get; set; }            
        }
    }
    
}