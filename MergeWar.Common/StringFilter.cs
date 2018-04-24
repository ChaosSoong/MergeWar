using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HCZZ.Common
{
    public class StringFilter
    {
        /// <summary>
        /// SHA1加密方法
        /// </summary>
        /// <param name="str">传入的明文</param>
        /// <returns>加密后的密文</returns>
        public static string getSHA1Code(string str)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] str1 = Encoding.UTF8.GetBytes(str);
            byte[] str2 = sha1.ComputeHash(str1);
            sha1.Clear();
            (sha1 as IDisposable).Dispose();
            return BitConverter.ToString(str2).Replace("-", "");
        }

        /// <summary>
        /// sql注入过滤功能
        /// </summary>
        /// <param name="str">传入的sql字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string SqlFilter(string str)
        {
            str = str == null ? "" : str.Trim();
            str = str.Replace("'", "''");
            return str;
        }

        /// <summary>
        /// 多条件模糊查询字符串替换（空格替换为%）
        /// </summary>
        /// <param name="str">传入的字符串</param>
        /// <returns>替换后的字符串</returns>
        public static string LikeQueryFilter(string str)
        {
            str = str == null ? "" : str.Trim();
            str = str.Replace(" ", "%");
            return str;
        }

        /// <summary>
        /// 从JAVA代码中 Date date = new Date(long time)转换为C#的时间
        /// </summary>
        /// <param name="javaMS">传入的long值</param>
        /// <returns>真实日期</returns>
        public static string ConvertJavaMiliSecondToDateTime(string javaMS)
        {
            long time = Convert.ToInt64(javaMS);
            if (javaMS.Trim() == "" || javaMS.Trim() == "0")
            {
                return "";
            }
            else
            {
                TimeSpan timespan = TimeSpan.FromMilliseconds(time);
                DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
                DateTime temp = Jan1st1970.Add(timespan);
                DateTime final = temp.ToUniversalTime(); // Change to local-time
                return final.AddDays(1).ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 对字符串进行Base64编码/解码  1：编码 其他：解码
        /// </summary>
        /// <param name="strType">1：编码 其他：解码</param>
        /// <param name="content">编码/解码 内容</param>
        /// <returns></returns>
        public static string Base64Str(int strType, string content)
        {
            try
            {
                if (strType == 1)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(content);
                    return Convert.ToBase64String(bytes);
                }
                else
                {
                    byte[] outpub = Convert.FromBase64String(content);
                    return Encoding.UTF8.GetString(outpub);
                }
            }
            catch (Exception)
            {

                return content;
            }

        }
        /// <summary>
        /// 返回MD5-32加密
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RefKeyMd5(string key)
        {
            MD5 md5 = MD5.Create();
            StringBuilder sb = new StringBuilder();

            byte[] b = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            for (int i = 0; i < b.Length; i++)
            {
                sb.Append(b[i].ToString("X").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
    }
}
