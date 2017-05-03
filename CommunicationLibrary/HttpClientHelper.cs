using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationLibrary
{
    /// <summary>
    /// 用于发送http请求的类
    /// </summary>
    public class HttpClientHelper
    {
        private HttpClient instance = null;
        /// <summary>
        /// 当前httpclient实例
        /// </summary>
        public HttpClient ClientInstance
        {
            get
            {
                if(instance==null)
                {
                    instance = new HttpClient();
                }
                return instance;
            }
        }
        /// <summary>
        /// 向指定url发送请求
        /// </summary>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <param name="url">url地址</param>
        /// <param name="paras">url参数（键值对）</param>
        /// <returns></returns>
        public TResult Post<TResult>(string url, IEnumerable<KeyValuePair<string, string>> paras)
        {
            return Post<TResult>(url, paras, null);
        }
        /// <summary>
        /// 向指定url发送请求
        /// </summary>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <param name="url">url地址</param>
        /// <param name="data">包含在content中的实体类型数据</param>
        /// <returns></returns>
        public TResult Post<TResult>(string url, object data)
        {
            return Post<TResult>(url, null, data);
        }
        /// <summary>
        /// 向指定url发送请求
        /// </summary>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <param name="url">url地址</param>
        /// <param name="paras">url参数（键值对）</param>
        /// <param name="data">包含在content中的实体类型数据</param>
        /// <returns></returns>
        public TResult Post<TResult>(string url, IEnumerable<KeyValuePair<string, string>> paras, object data)
        {
            try
            {
                url = CreateUrl(url, paras);
                MediaTypeFormatter formatter = new JsonMediaTypeFormatter();
                HttpContent content = new ObjectContent(data.GetType(), data, formatter);
                HttpResponseMessage response = ClientInstance.PostAsync(url, content).Result; 
                if (response.IsSuccessStatusCode)
                {
                    TResult result = response.Content.ReadAsAsync<TResult>().Result;
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 拼接url及参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        private string CreateUrl(string url, IEnumerable<KeyValuePair<string, string>> paras)
        {
            if (paras != null)
            {
                string urlParas = "";
                foreach (KeyValuePair<string, string> keyValue in paras)
                {
                    if (!string.IsNullOrEmpty(keyValue.Key))
                    {
                        urlParas += "&"+ keyValue.Key + "=" + UrlEncode(keyValue.Value);
                    }
                }
                if (!string.IsNullOrEmpty(urlParas) && urlParas.Length > 1)
                {
                    urlParas = urlParas.Remove(0,1);
                    url = url + "?" + urlParas;
                }
            }
            return url;
        }

        private string UrlEncode(string str)
        {
            return System.Web.HttpUtility.UrlEncode(str);
        }
        /// <summary>
        /// 要向请求的head中添加的数据（键值对）
        /// </summary>
        /// <param name="paras"></param>
        public void SetHeaderKeyValues(IEnumerable<KeyValuePair<string, string>> paras)
        {
            foreach (KeyValuePair<string,string> keyValue in paras)
            {
                if(!string.IsNullOrEmpty(keyValue.Key))
                {
                    if(ClientInstance.DefaultRequestHeaders.Contains(keyValue.Key))
                    {
                        ClientInstance.DefaultRequestHeaders.Remove(keyValue.Key);
                    }
                    ClientInstance.DefaultRequestHeaders.Add(keyValue.Key, UrlEncode(keyValue.Value));
                }
            }
        }
    }
}
