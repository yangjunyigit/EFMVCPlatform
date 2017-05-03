using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApiServer.Controllers
{
    public class ClientInfoCache
    {
        /// <summary>
        /// 数据解密的方法代理
        /// </summary>
        public static Func<string, string, string> DESDecode { get; set; }
        private static string desKey = "4925A069-9CF0-40FB-A4B2-E6970031F296";
        private static Dictionary<string, ClientInfo> clientList = null;
        /// <summary>
        /// 保存客户端信息的数据结构
        /// </summary>
        public struct ClientInfo
        {
            /// <summary>
            /// 客户端Id
            /// </summary>
            public string ClientId { get; set; }
            /// <summary>
            /// 客户端公钥
            /// </summary>
            public string PublicKey { get; set; }
            /// <summary>
            /// 客户端秘钥
            /// </summary>
            public string DesKey { get; set; }
            /// <summary>
            /// 最后一侧访问时间
            /// </summary>
            public DateTime LastTime { get; set; }
        }
        /// <summary>
        /// 获取此终端的des秘钥
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public static string GetClientDesKey(string clientId)
        {
            if (clientList != null&& clientList.ContainsKey(clientId))
            {
                ClientInfo ci = clientList[clientId];
                ci.LastTime = DateTime.Now;
                //每次读取数据就更新访问时间
                UpdateClientInfo(ci);
                return ci.DesKey;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 获取此终端的公钥
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public static string GetClientPublicKey(string clientId)
        {
            if (clientList != null&& clientList.ContainsKey(clientId))
            {
                ClientInfo ci = clientList[clientId];
                ci.LastTime = DateTime.Now;
                //每次读取数据就更新访问时间
                UpdateClientInfo(ci);
                return ci.PublicKey;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 更新终端数据
        /// </summary>
        /// <param name="cltInfo"></param>
        public static void UpdateClientInfo(ClientInfo cltInfo)
        {
            try
            {
                if (clientList == null)
                {
                    clientList = new Dictionary<string, ClientInfo>();
                }
                if (clientList.ContainsKey(cltInfo.ClientId))
                {
                    clientList[cltInfo.ClientId] = cltInfo;
                }
                else
                {
                    clientList.Add(cltInfo.ClientId, cltInfo);
                }
                SaveClientInfo();
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 更新终端数据
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="pubKey"></param>
        /// <param name="desKey"></param>
        public static void UpdateClientInfo(string clientId, string pubKey = null,string desKey=null)
        {
            try
            {
                ClientInfo ci = new ClientInfo();
                ci.ClientId = clientId;
                ci.LastTime = DateTime.Now;
                if (pubKey != null)
                {
                    ci.PublicKey = pubKey;
                }
                if (desKey != null)
                {
                    ci.DesKey = desKey;
                }
                UpdateClientInfo(ci);
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 将连接的终端数据读入缓存
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string,ClientInfo> LoadClientInfo()
        {
            try
            {
                //文件路径
                string filePath = HttpContext.Current.Server.MapPath("App_Data/");
                string fileName = "ClientInfoCache.txt";
                if (File.Exists(filePath + fileName))
                {
                    //从文件中读取数据
                    FileStream fs = new FileStream(filePath + fileName, FileMode.Open);
                    StreamReader sr = new StreamReader(fs);
                    string enStrInfo = sr.ReadToEnd();
                    //清空缓冲区、关闭流
                    fs.Flush();
                    fs.Close();

                    //解密数据，json数据转为对象进行缓存
                    string jsonStrInfo = DESDecode(enStrInfo, desKey);
                    clientList = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, ClientInfo>>(jsonStrInfo);
                }
                return clientList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 保存终端访问数据（保存在文件中）
        /// </summary>
        private static void SaveClientInfo()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(m =>
            {
                try
                {
                    if (clientList != null && clientList.Count > 0)
                    {
                        string filePath = HttpContext.Current.Server.MapPath("/App_Data/");
                        if (Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        string fileName = "ClientInfoCache.txt";
                        //将数据转为json格式，并加密。
                        string strClientInfo = Newtonsoft.Json.JsonConvert.SerializeObject(clientList);
                        string enStrInfo = DESDecode(strClientInfo, desKey);

                        //向文件中写入数据
                        FileStream fs = new FileStream(filePath + fileName, FileMode.Create);
                        //获得字节数组
                        byte[] data = System.Text.Encoding.Default.GetBytes(enStrInfo);
                        //开始写入
                        fs.Write(data, 0, data.Length);
                        //清空缓冲区、关闭流
                        fs.Flush();
                        fs.Close();
                    }
                }
                catch (Exception ex)
                {
                }
            });
        }
    }
}