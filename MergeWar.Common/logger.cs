using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Common
{
        public class Logger
        {
            static string LogPath = System.Configuration.ConfigurationManager.AppSettings["IsSynPath"];
            /// <summary>
            /// 日志记录
            /// </summary>
            /// <param name="userSessionName">用户名</param>
            /// <param name="message">日志内容</param>
            public static void writeLog(string message)
            {
                HttpContext current = HttpContext.Current;
                DirectoryInfo dinfo = new DirectoryInfo(current.Server.MapPath("~/Log/Info"));
                if (!dinfo.Exists)
                {
                    dinfo.Create();
                }
                string filePath = "/Log/Info/Info_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
                filePath = current.Server.MapPath("~/") + filePath;
                //string user = userName;
                StreamWriter sw = new StreamWriter(filePath, true, Encoding.Default);
                sw.WriteLine("=============================================================");
                //sw.WriteLine("用户名：" + user);
                sw.WriteLine("时间：" + DateTime.Now.ToString());
                //sw.WriteLine("IP：" + current.Request.UserHostAddress);
                sw.WriteLine("日志内容：");
                sw.WriteLine(message);
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }

            /// <summary>
            /// 日志异常记录
            /// </summary>
            /// <param name="exception">捕获的异常对象</param>
            /// <param name="information">产生异常的其他相关信息（比如参数，或SQL语句）</param>
            public static void ErrorLog(Exception ex, Dictionary<string, string> information)
            {
                HttpContext current = HttpContext.Current;
                DirectoryInfo dinfo = new DirectoryInfo(current.Server.MapPath("~/Log/Error"));
                if (!dinfo.Exists)
                {
                    dinfo.Create();
                }
                string filePath = "/Log/Error/Error_" + DateTime.Today.ToString("yyyy-MM-dd") + ".log";
                filePath = current.Server.MapPath("~/") + filePath;
                StreamWriter sw = new StreamWriter(filePath, true, Encoding.Default);
                sw.WriteLine("=============================================================");
                sw.WriteLine("====时间：" + DateTime.Now.ToString());
                sw.WriteLine("====IP：" + current.Request.UserHostAddress);
                sw.WriteLine("====异常内容：");
                sw.WriteLine(ex.ToString());
                if (information != null && information.Keys.Count > 0)
                {
                    sw.WriteLine("====相关信息：");
                    foreach (var temp in information.Keys)
                    {
                        sw.WriteLine("——" + temp + "：");
                        sw.WriteLine(information[temp]);
                    }
                }
                sw.WriteLine();
                sw.WriteLine();
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }

            /// <summary>
            /// 日志记录
            /// </summary>
            /// <param name="message">日志内容</param>
            public static void InSendDataLog(string message)
            {
                DirectoryInfo dinfo = new DirectoryInfo(LogPath + "/Log/SendSynData");
                if (!dinfo.Exists)
                {
                    dinfo.Create();
                }
                string filePath = LogPath + "/Log/SendSynData/" + DateTime.Today.ToString("yyyy-MM-dd") + ".log";
                StreamWriter sw = new StreamWriter(filePath, true, Encoding.Default);
                sw.WriteLine("");
                sw.WriteLine("同步数据：" + message + "，时间：" + DateTime.Now);
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }

            /// <summary>
            /// 同步数据写文件
            /// </summary>
            /// <param name="message">日志内容</param>
            /// <param name="path">文件夹名称</param>
            public static void writeFile(string message, string path)
            {
                DateTime time = DateTime.Now;
                path = "/" + path + "/" + time.ToString("yyyy-MM-dd") + "/" + time.ToString("HH");
                DirectoryInfo dinfo = new DirectoryInfo(LogPath + path);
                if (!dinfo.Exists)
                {
                    dinfo.Create();
                }
                string filePath = path + "/" + Guid.NewGuid();
                filePath = LogPath + filePath;
                using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.Default))
                {
                    sw.WriteLine(message);
                }
                //修改文件后缀名
                File.Move(filePath, (filePath + ".txt"));
                //File.Delete(filePath);
            }

            /// <summary>
            /// 同步数据写文件
            /// </summary>
            /// <param name="message">日志内容</param>
            /// <param name="path">文件夹名称</param>
            public static void writeFile(byte[] message, string path)
            {
                DateTime time = DateTime.Now;
                path = "/" + path + "/" + time.ToString("yyyy-MM-dd") + "/" + time.ToString("HH");
                DirectoryInfo dinfo = new DirectoryInfo(LogPath + path);
                if (!dinfo.Exists)
                {
                    dinfo.Create();
                }
                string filePath = path + "/" + Guid.NewGuid();
                filePath = LogPath + filePath;
                using (FileStream fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Write(message, 0, message.Length);
                }
                //修改文件后缀名
                File.Move(filePath, (filePath + ".txt"));
                //File.Delete(filePath);
            }

            /// <summary>
            /// 读取日志记录
            /// </summary>
            public static List<string> SecuttyReadLog()
            {
                HttpContext current = HttpContext.Current;
                string path = current.Server.MapPath("~/Log/Security/Security.log");
                List<string> list = new List<string>();
                FileInfo fi = new FileInfo(path);
                if (!fi.Exists)
                {
                    list.Add("");
                    list.Add("");
                }
                else
                {
                    using (FileStream file = File.Open(path, FileMode.Open))
                    {
                        using (StreamReader strem = new StreamReader(file))
                        {
                            while (!strem.EndOfStream)
                            {
                                list.Add(strem.ReadLine());
                            }
                        }
                    }
                    if (list.Count == 0)
                    {
                        list.Add("");
                        list.Add("");
                    }
                }
                return list;
            }
        }
}
