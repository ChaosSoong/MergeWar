using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.IO;

namespace HCZZ.Common
{
    public class DesModel
    {
        public static string RefDesStr(string data, int desType)
        {
            if (desType == 1)
                return DesModel.DesEncode(data, "@#ER7+-!", "1A|?.,o9");
            else
                return DesModel.DesDecode(data, "@#ER7+-!", "1A|?.,o9");
            
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DesEncode(string data, string key, string strIv)
        {
            System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
            byte[] byteKey = utf8.GetBytes(key);

            byte[] iv = utf8.GetBytes(strIv); //当模式为ECB时，IV无用
            return Encrypt(data, byteKey, iv);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="des3Type"></param>
        /// <returns></returns>
        public static string DesDecode(string data, string key, string strIv)
        {
            System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
            byte[] byteKey = utf8.GetBytes(key);
            byte[] iv = utf8.GetBytes(strIv); //当模式为ECB时，IV无用
            return Decrypt(data, byteKey, iv);
        }

        /// <summary>
        /// 实现加密的方法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encrypt(string value, byte[] key, byte[] iv)
        {
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            cryptoProvider.Mode = CipherMode.CBC;
            cryptoProvider.Padding = PaddingMode.PKCS7;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateEncryptor(key, iv), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cs);
            sw.Write(value);
            sw.Flush();
            cs.FlushFinalBlock();
            ms.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }

        /// <summary>
        /// 实现解密的方法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Decrypt(string value, byte[] key, byte[] iv)
        {
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            cryptoProvider.Mode = CipherMode.CBC;
            cryptoProvider.Padding = PaddingMode.PKCS7;
            byte[] buffer = Convert.FromBase64String(value);
            MemoryStream ms = new MemoryStream(buffer);
            CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateDecryptor(key, iv), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}