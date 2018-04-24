using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Web;
using HCZZ.Common;

namespace HCZZ
{
    public class HttpHelps
    {
        public string Jsondata;
        private string SynUrl = ConfigurationManager.AppSettings["SynHostUrl"];

        private static string key = "abcdefg,,,";
        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            //为了通过证书验证，总是返回true
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sendType">1：添加，2：编辑，3：删除</param>
        /// <param name="DataType">1：Excel批量导入，2：单条数据</param>
        /// <param name="DeviceList"></param>
        public void PostSend(int sendType, int DataType, string DeviceList)
        {
            HttpContext context = HttpContext.Current;
            Random rand = new Random();
            int Num = rand.Next(1000, 9999);
            string Token = StringFilter.RefKeyMd5(key + Num);
            string tempDeviceList = DeviceList;
            DeviceList = context.Server.UrlEncode(DesModel.RefDesStr(DeviceList, 1));

            System.Net.WebClient WebClientObj = new System.Net.WebClient();
            System.Collections.Specialized.NameValueCollection PostVars = new System.Collections.Specialized.NameValueCollection();
            PostVars.Add("Token", Token);
            PostVars.Add("Num", Num.ToString());
            PostVars.Add("DeviceList", DeviceList);
            PostVars.Add("DataType", DataType.ToString());

            try
            {
                byte[] byRemoteInfo = WebClientObj.UploadValues(SynUrl + DevSendType(sendType), "POST", PostVars);
                //下面都没用啦，就上面一句话就可以了
                string sRemoteInfo = System.Text.Encoding.Default.GetString(byRemoteInfo);
                Logger.writeLog("同步AP数据：" + tempDeviceList + "，处理结果：" + sRemoteInfo);
                //这是获取返回信息
            }
            catch(Exception e)
            {
                Logger.ErrorLog(e, new Dictionary<string, string>() { { "sendMessage", tempDeviceList } });
            }
        }

        public string DevSendType(int sendType)
        {
            switch (sendType)
            {
                case 1: return "/DeviceInfo/BatchAdd";
                case 2: return "/DeviceInfo/BatchEdit";
                case 3: return "/DeviceInfo/BatchDelete";
                default: return "";
            }
        }
    }
}

