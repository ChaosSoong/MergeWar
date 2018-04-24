using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MergeWar.AppCode
{
    public class WeChatPay
    {
        public string AppID { get; set; }
        public string AppSecret { get; set; }
        public string MchID { get; set; }
        public string MchKey { get; set; }
        public string orderAPI = "https://api.mch.weixin.qq.com/pay/unifiedorder";
        public WeChatPay()
        {
            this.AppID = "wx2421b1c4370ec43b";//AppID
            this.AppSecret = "123456789";//App密码
            this.MchID = "10000100";//商户ID
            this.MchKey = "192006250b4c09247ec02edce69f6a2d";//商户密码
        }
        public string Pay(Dictionary<string, string> dic)
        {
            dic.Add("appid", this.AppID);
            //dic.Add("",this.AppSecret);
            dic.Add("mch_id", this.MchID);
            dic.Add("nonce_str", GetRndom());//随机字符串
            dic.Add("out_trade_no", "1234567890qwertyuioplkjhgfdsazxc");//订单号32位
            dic.Add("spbill_create_ip", "192.168.0.179");//终端IP
            dic.Add("notify_url", "192.168.0.179:8080/public/notify");//通知地址
            dic.Add("trade_type", "NATIVE");//交易类型
            dic.Add("product_id", "12235413214070356458058");//native时必填
            string sign = String.Empty;
            string strA = "appid=" + this.AppID + "&body=" + dic["body"] + "&nonce_str=" + GetRndom() + "&notify_url=" + dic["notify_url"] + "&mch_id=" + this.MchID + "&out_trade_no=" + dic["out_trade_no"] + "&product_id=" +dic["product_id"] + "&sign=" + sign + "&spbill_create_ip=" + dic["spbill_create_ip"] + "&total_fee=" + dic["total_fee"] + "&trade_type=" + dic["trade_type"];
            string stringSignTemp = strA + "&key=" + this.MchKey; //注：key为商户平台设置的密钥key
            sign = GetMd5Hash(stringSignTemp);
            string _req_data = "<xml>";
            _req_data += "<appid>" + dic["appid"] + "</appid>";
            _req_data += "<body><![CDATA[" + dic["body"] + "]]></body> ";
            _req_data += "<nonce_str><![CDATA[" + dic["nonce_str"] + "]]></nonce_str> ";
            _req_data += "<notify_url><![CDATA[" + dic["notify_url"] + "]]></notify_url> ";
            _req_data += "<mch_id><![CDATA[" + dic["mch_id"] + "]]></mch_id> ";
            _req_data += "<out_trade_no><![CDATA[" + dic["out_trade_no"] + "]]></out_trade_no> ";
            _req_data += "<product_id><![CDATA[" + dic["product_id"] + "]]></product_id> ";
            _req_data += "<sign><![CDATA[" + sign + "]]></sign> ";
            _req_data += "<spbill_create_ip><![CDATA[" + dic["spbill_create_ip"] + "]]></spbill_create_ip> ";
            _req_data += "<total_fee><![CDATA[" + dic["total_fee"] + "]]></total_fee> ";
            _req_data += "<trade_type><![CDATA[" + dic["trade_type"] + "]]></trade_type> ";
            _req_data += "</xml>";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.orderAPI);
            request.Method = "POST";
            request.ContentType = "text/xml;charset=UTF-8";
            byte[] payload = Encoding.UTF8.GetBytes(_req_data);
            request.ContentLength = payload.Length;

            //发送post的请求
            Stream writer = request.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();

            //接受返回来的数据
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string value = reader.ReadToEnd();

            reader.Close();
            stream.Close();
            response.Close();
            return value;
        }
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <param name="useNum"></param>
        /// <param name="useLow"></param>
        /// <param name="useUp"></param>
        /// <param name="useSpe"></param>
        /// <param name="custom"></param>
        /// <returns></returns>
        public string GetRndom(int length = 16, bool useNum = true, bool useLow = true, bool useUp = true, bool useSpe = false, string custom = "")
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
        /// <summary>
        /// 字符串MD5运算
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5Hash(String input)
        {
            if (input == null)
            {
                return null;
            }

            MD5 md5Hash = MD5.Create();

            // 将输入字符串转换为字节数组并计算哈希数据  
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // 创建一个 Stringbuilder 来收集字节并创建字符串  
            StringBuilder sBuilder = new StringBuilder();

            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串  
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // 返回十六进制字符串  
            return sBuilder.ToString().ToUpper();
        }
    }
}