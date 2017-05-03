using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationLibrary
{
    /// <summary>
    /// 用于调用webapi的类
    /// </summary>
    public class WebApiHelper
    {
        /// <summary>
        /// 数据加密方法代理
        /// </summary>
        private static Func<string,string, string> DESEncode;
        private static string desKey;
        static HttpClientHelper client = new HttpClientHelper();
        /// <summary>
        /// webapi 地址，不包含controller/action
        /// </summary>
        public static string ApiAddress
        {
            get
            {
                //获取配置的api地址
                return "http://localhost:8030/api/";
            }
        }

        static WebApiHelper()
        {
            DESEncode = (msg, key) => { return msg; };
        }

        private static void SetHeaderValues()
        {
            List<KeyValuePair<string, string>> paras = new List<KeyValuePair<string, string>>();
            paras.Add(new KeyValuePair<string, string>("para1", DESEncode("userid01293878435",desKey)));//UserId
            paras.Add(new KeyValuePair<string, string>("para2", DESEncode( "usernameiysoqweiroe", desKey)));//UserName
            client.SetHeaderKeyValues(paras);
        }
        /// <summary>
        /// 调用webapi指定接口的方法
        /// </summary>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="actionName">接口名称（controller/action）</param>
        /// <returns></returns>
        public static TResult Post<TResult>(string actionName)
        {
            return Post<TResult>(actionName, null, null);
        }
        /// <summary>
        /// 调用webapi指定接口的方法
        /// </summary>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="actionName">接口名称（controller/action）</param>
        /// <param name="simpleParas">简单类型参数（键值对）</param>
        /// <returns></returns>
        public static TResult Post<TResult>(string actionName, Dictionary<string, string> simpleParas)
        {
            return Post<TResult>(actionName, simpleParas, null);
        }
        /// <summary>
        /// 调用webapi指定接口的方法
        /// </summary>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="actionName">接口名称（controller/action）</param>
        /// <param name="entData">要传输的实体数据</param>
        /// <returns></returns>
        public static TResult Post<TResult>(string actionName, object entData)
        {
            return Post<TResult>(actionName, null, entData);
        }
        /// <summary>
        /// 调用webapi指定接口的方法
        /// </summary>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="actionName">接口名称（controller/action）</param>
        /// <param name="simpleParas">简单类型参数（键值对）</param>
        /// <param name="entData">要传输的实体数据</param>
        /// <returns></returns>
        public static TResult Post<TResult>(string actionName, Dictionary<string, string> simpleParas, object entData)
        {
            try
            {
                TResult tResult = default(TResult);
                string url = ApiAddress +"/"+ actionName;
                SetHeaderValues();
                string jsonData = "";
                if (entData!=null)
                {
                    jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(entData);
                    if (!string.IsNullOrEmpty(desKey)/* 秘钥为空时不加密*/)
                    {
                        jsonData = DESEncode(jsonData, desKey);
                    } 
                }
                var ctnData = new { ClientId = "客户端唯一标示", Data = jsonData };
                MsgResult msgResult = client.Post<MsgResult>(url, GetUrlParas(simpleParas), ctnData);

                if(msgResult.State == "成功！")
                {
                    if (msgResult.Data != null)
                    {
                        if(msgResult.Data is Newtonsoft.Json.Linq.JObject)
                        {
                            tResult = ((Newtonsoft.Json.Linq.JObject)msgResult.Data).ToObject<TResult>();
                        }
                        else if (msgResult.Data is Newtonsoft.Json.Linq.JArray)
                        {
                            tResult = ((Newtonsoft.Json.Linq.JArray)msgResult.Data).ToObject<TResult>();
                        }
                        ////Json格式化时将int32转为了int64，接口返回int类型时需要用int64接收返回值
                        //else if(msgResult.Data is Int64)
                        //{
                        //    object obj = Convert.ToInt32(msgResult.Data);
                        //    tResult = (TResult)obj;
                        //}
                        else
                        {
                            tResult = (TResult)msgResult.Data;
                        }
                    }
                }
                return tResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<KeyValuePair<string,string>> GetUrlParas(Dictionary<string, string> urlParas)
        {
            if (urlParas!=null)
            {
                List<KeyValuePair<string, string>> paras = new List<KeyValuePair<string, string>>();
                string strUrlParas = "";
                foreach (string key in urlParas.Keys)
                {
                    strUrlParas += string.Format("&{0}={1}", key, urlParas[key]);
                }
                if(strUrlParas!=null&&strUrlParas.Length>0)
                {
                    strUrlParas = strUrlParas.Remove(0, 1);
                }
                paras.Add(new KeyValuePair<string, string>("p", DESEncode(strUrlParas,desKey)));
                return paras; 
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 用于数据传输的类
        /// </summary>
        public class MsgResult
        {
            /// <summary>
            /// 状态
            /// </summary>
            public string State { get; set; }
            /// <summary>
            /// 数据包
            /// </summary>
            public object Data { get; set; }
        }
    }
}
