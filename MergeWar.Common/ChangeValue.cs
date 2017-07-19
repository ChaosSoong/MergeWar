using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HCZZ.ModeDB;

namespace Common
{
    public class ChangeValue
    {
        /// <summary>
        /// 转换场所审计状态
        /// </summary>
        /// <param name="Verified"></param>
        /// <returns></returns>
        public static string RefVerifiedStr(int Verified)
        {
            switch (Verified)
            {
                case 1: return "<font color='blue'>待审核</font>";
                case 3: return "<font color='green'>审核成功</font>";
                case 4: return "<font color='red'>审核失败</font>";
                default: return "";
            }
        }
        public static string RefPotTypeStr(int PotType)
        {
            switch (PotType)
            {
                case 1: return "<font color='blue'>待审核</font>";
                case 2: return "<font color='green'>审核成功</font>";
                case 3: return "<font color='red'>审核失败</font>";
                default: return "";
            }
        }
        /// <summary>
        /// 厂商数据查询类别区分
        /// </summary>
        /// <param name="authType"></param>
        /// <returns></returns>
        public static string RefauthTypeStr(string authType)
        {
            switch (authType)
            {
                case "1": return "AuditLog";
                case "2": return "NetLog";
                case "3": return "Termianl";
                case "4": return "";
                default: return "";
            }
        }
        /// <summary>
        /// 数据类型数字转换为前端识别的字符串
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static string ChangeLevelWord(string level)
        {
            switch (level)
            {
                case "1": return "旅店宾馆类";
                case "2": return "图书馆阅览室";
                case "3": return "电脑培训中心";
                case "4": return "娱乐休闲场所";
                case "5": return "交通枢纽";
                case "6": return "公共交通工具";
                case "7": return "餐饮服务场所";
                case "8": return "金融服务场所";
                case "9": return "其他";
                case "10": return "购物场所";
                case "11": return "公共服务场所";
                case "12": return "文化服务场所";
                case "13": return "公共休闲场所";
                case "30":
                    return "手机号";
                case "31":
                    return "mac";
                case "32":
                    return "虚拟身份";
                case "33":
                    return "imei";
                case "34":
                    return "imsi";
                case "35":
                    return "车牌号";
                default:
                    return level;
            }
        }
        public static Dictionary<int, string> GetLocaTypeList()
        {
            Dictionary<int, string> dic = new Dictionary<int, string> 
            {
                {1,"旅店宾馆类"},
                {2,"图书馆阅览室"},
                {3,"电脑培训中心"},
                {4,"娱乐休闲场所"},
                {5,"交通枢纽"},
                {6,"公共交通工具"},
                {7,"餐饮服务场所"},
                {8,"金融服务场所"},
                {10,"购物场所"},
                {11,"公共服务场所"},
                {12,"文化服务场所"},
                {13,"公共休闲场所"},
                {9,"其他"}
            };
            return dic;
        }
        /// <summary>
        /// 获取厂商数据集合
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> DicMakeType()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>() { { 1, "携网" }, { 2, "任子行" }, { 3, "企智通" }, { 4, "网博" }, { 5, "中科信业" }, { 6, "迈普" }, { 7, "平安" }, { 8, "舜游" }, { 99, "其他" } };
            return dic;
        }
        /// <summary>
        /// 根据厂商类型值获取厂商名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string RefMakeTypeValue(int type)
        {
            switch (type)
            {
                case 1: return "携网";
                case 2: return "任子行";
                case 3: return "企智通";
                case 4: return "网博";
                case 5: return "中科信业";
                case 6: return "迈普";
                case 7: return "平安";
                case 8: return "舜游";
                case 99: return "其他";
                default: return "其他";
            }
        }
        /// <summary>
        /// 将场所验证值转换为场所验证名称
        /// </summary>
        /// <returns></returns>
        public static string GetLocaCheckValue(int value)
        {
            switch (value)
            {
                case 0: return "关闭";
                case 1: return "网站验证";
                case 2: return "第三方验证";
                case 3: return "微信验证";
                case 4: return "WIFIDOG验证";
                case 5: return "下行短信";
                case 99: return "其他";
                default: return "";
            }
        }


        public static Dictionary<int, string> GetConnectTypeList()
        {
            Dictionary<int, string> dic = new Dictionary<int, string> 
            {
                {1,"专网"},
                {2,"专线"},
                {3,"ADSL拨号"},
                {4,"ISDN"},
                {5,"普通拨号"},
                {6,"Cable modem拨号"},
                {7,"电力线"},
                {8,"无线上网"},
                {99,"其他"}
            };
            return dic;
        }

        public static Dictionary<string, string> GetServiceBusinesList()
        {
            Dictionary<string, string> dic = new Dictionary<string, string> 
            {
                {"01","中国电信"},
                {"02","中国网通"},
                {"03","中国联通"},
                {"04","中国长城宽带"},
                {"05","中国铁通"},
                {"06","中国移动"},
                {"08","教育部门"},
                {"09","中科院"},
                {"11","广电部门"},
                {"99","其他"}
            };
            return dic;
        }
        /// <summary>
        /// 网络应用服务类型转换
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static string NetworkServe(string NETWORK)
        {
            switch (NETWORK)
            {
                case "QQ":
                    return "20";
                case "淘宝":
                    return "21";
                case "微信":
                    return "22";
                case "新浪微博":
                    return "23";
                case "IMEI":
                    return "24";
                case "IMSI":
                    return "25";
                case "京东":
                    return "26";
                case "苏宁易购":
                    return "27";
                case "天涯":
                    return "29";
                case "猫扑":
                    return "30";
                case "滴滴":
                    return "32";
                case "QQ邮箱":
                    return "33";
                case "网易邮箱":
                    return "34";
                case "携程":
                    return "35";
                case "去哪儿":
                    return "36";
                case "阿里旺旺":
                    return "37";
                case "飞信":
                    return "38";
                case "米聊":
                    return "39";
                case "陌陌":
                    return "40";
                case "新浪邮箱":
                    return "41";
                case "139邮箱":
                    return "42";
                case "天猫":
                    return "43";
                case "唯品会":
                    return "44";
                case "美团":
                    return "45";
                case "大众点评":
                    return "46";
                case "12306":
                    return "47";
                case "赶集网":
                    return "48";
                case "58同城":
                    return "49";
                case "QQ微博":
                    return "50";
                case "20":
                    return "QQ";
                case "21":
                    return "淘宝";
                case "22":
                    return "微信";
                case "23":
                    return "新浪微博";
                case "24":
                    return "IMEI";
                case "25":
                    return "IMSI";
                case "26":
                    return "京东";
                case "27":
                    return "苏宁易购";
                case "29":
                    return "天涯";
                case "30":
                    return "猫扑";
                case "32":
                    return "滴滴";
                case "33":
                    return "QQ邮箱";
                case "34":
                    return "网易邮箱";
                case "35":
                    return "携程";
                case "36":
                    return "去哪儿";
                case "37":
                    return "阿里旺旺";
                case "38":
                    return "飞信";
                case "39":
                    return "米聊";
                case "40":
                    return "陌陌";
                case "41":
                    return "新浪邮箱";
                case "42":
                    return "139邮箱";
                case "43":
                    return "天猫";
                case "44":
                    return "唯品会";
                case "45":
                    return "美团";
                case "46":
                    return "大众点评";
                case "47":
                    return "12306";
                case "48":
                    return "赶集网";
                case "49":
                    return "58同城";
                case "50":
                    return "QQ微博";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 操作日志数据填充
        /// </summary>
        /// <param name="log"></param>
        public static void AddOpLog(OPLog log)
        {
            HttpContext corrent = HttpContext.Current;

            log.IpAddress = corrent.Request.UserHostAddress;
            log.CreateTime = DateTime.Now;
            UserInfo user = (UserInfo)corrent.Session["userInfo"];
            if (user != null)
                log.UID = user.ID;
        }
        /// <summary>
        /// 根据用户类型获取对应的用户类型值
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static string RefUseTypeValue(int userType)
        {
            switch (userType)
            {
                case 8: return "运营用户";
                case 7: return "全国用户";
                case 6: return "省用户";
                case 4: return "市局用户";
                case 2: return "分局用户";
                case 3: return "派出所用户";
                case 1: return "管理员";
                default: return "";
            }
        }

        /// <summary>
        /// 获取区域  userType[0]: 用户类型，userType[1]: 所属区域
        /// </summary>
        /// <returns></returns>
        public static int[] GetUserType()
        {
            UserInfo user = (UserInfo)HttpContext.Current.Session["userInfo"];
            int[] userType = new int[2];
            if (user != null)
            {
                userType[0] = user.Type;
                if (user.Type == 8)
                {
                    if (user.AId > 0)
                    {
                        userType[0] = 2;
                        userType[1] = user.AId;
                    }
                    else if (user.CityID > 0)
                    {
                        userType[0] = 4;
                        userType[1] = user.CityID;
                    }
                    else if (user.ProID > 0)
                    {
                        userType[0] = 6;
                        userType[1] = user.ProID;
                    }
                    else
                        userType[1] = 0;
                }

                if (user.Type == 7) userType[1] = user.ProID;
                if (user.Type == 6) userType[1] = user.ProID;
                if (user.Type == 4) userType[1] = user.CityID;
                if (user.Type == 3) userType[1] = user.PId;
                if (user.Type == 2) userType[1] = user.AId;
                if (user.Type == 1) userType[1] = user.ProID;
            }
            else
            {
                userType[0] = 0; userType[1] = 0;
            }
            return userType;
        }

        public static UserInfo GteUserVal(FormCollection form)
        {
           UserInfo DBuser=new UserInfo();
           DBuser.UserName = form["txtName"];
           DBuser.TrueName = form["txtTrueName"];
           DBuser.idNumber = form["txtidNumber"];
           DBuser.idType = Convert.ToInt32(form["selIdType"]);
           DBuser.Mobile = form["txtMobile"];
           DBuser.Email = form["txtEmail"];
           DBuser.Type = Convert.ToInt32(form["selUserType"]);
           switch (DBuser.Type)
           {
               case 8:
                   DBuser.ProID = (string.IsNullOrEmpty(form["selProvince"]) ? 0 : Convert.ToInt32(form["selProvince"]));
                   DBuser.CityID = (string.IsNullOrEmpty(form["selCity"]) ? 0 : Convert.ToInt32(form["selCity"]));
                   DBuser.AId = (string.IsNullOrEmpty(form["SelArea"]) ? 0 : Convert.ToInt32(form["SelArea"]));
                   DBuser.PId = 0;
                   break;
               case 7:
                   DBuser.ProID = 0;
                   DBuser.CityID = 0;
                   DBuser.AId = 0;
                   DBuser.PId = 0;
                   break;
               case 6:
                   DBuser.ProID = Convert.ToInt32(form["selProvince"]);
                   DBuser.CityID = 0;
                   DBuser.AId = 0;
                   DBuser.PId = 0;
                   break;
               case 4:
                   DBuser.ProID = Convert.ToInt32(form["selProvince"]);
                   DBuser.CityID = Convert.ToInt32(form["selCity"]);
                   DBuser.AId = 0;
                   DBuser.PId = 0;
                   break;
               case 2:
                   DBuser.ProID = Convert.ToInt32(form["selProvince"]);
                   DBuser.CityID = Convert.ToInt32(form["selCity"]);
                   DBuser.AId = Convert.ToInt32(form["SelArea"]);
                   DBuser.PId = 0;
                   break;
               default:
                   DBuser.ProID = Convert.ToInt32(form["selProvince"]);
                   DBuser.CityID = Convert.ToInt32(form["selCity"]);
                   DBuser.AId = Convert.ToInt32(form["SelArea"]);
                   DBuser.PId = Convert.ToInt32(form["selPolice"]);
                   break;
           }
           DBuser.CreateTime = DateTime.Now;
           DBuser.JId = Convert.ToInt32(form["Txtpart"]);
           if (!string.IsNullOrEmpty(form["txtPassword"]))
           DBuser.Password = StringFilter.getSHA1Code(form["txtPassword"]);
            return DBuser;
        }
        /// <summary>
        /// 将场所类型值转换为场所类型名称
        /// </summary>
        /// <returns></returns>
        public static string GetLocaTypeValue(int value)
        {
            switch (value)
            {
                case 1: return "旅店宾馆类";
                case 2: return "图书馆阅览室";
                case 3: return "电脑培训中心";
                case 4: return "娱乐休闲场所";
                case 5: return "交通枢纽";
                case 6: return "公共交通工具";
                case 7: return "餐饮服务场所";
                case 8: return "金融服务场所";
                case 10: return "购物场所";
                case 11: return "公共服务场所";
                case 12: return "文化服务场所";
                case 13: return "公共休闲场所";
                case 9: return "其他";
                default: return "";
            }
        }

        /// <summary>
        /// 将接入方式值转换为接入方式名称
        /// </summary>
        /// <returns></returns>
        public static string GetConnectTypeValue(int value)
        {
            switch (value)
            {
                case 1: return "专网";
                case 2: return "专线";
                case 3: return "ADSL拨号";
                case 4: return "ISDN";
                case 5: return "普通拨号";
                case 6: return "Cable modem拨号";
                case 7: return "电力线";
                case 8: return "无线上网";
                case 99: return "其他";
                default: return "";
            }
        }
        public static string GetServiceBusinesValue(string value)
        {
            switch (value)
            {
                case "01": return "中国电信";
                case "02": return "中国网通";
                case "03": return "中国联通";
                case "04": return "中国长城宽带";
                case "05": return "中国铁通";
                case "06": return "中国移动";
                case "08": return "教育部门";
                case "09": return "中科院";
                case "11": return "广电部门";
                case "99": return "其他";
                default: return "";
            }
        }
        /// <summary>
        /// 转换场所审计状态
        /// </summary>
        /// <param name="Verified"></param>
        /// <returns></returns>
        public static string RefVerifiedStrExcel(int Verified)
        {
            switch (Verified)
            {
                case 1: return "待审核";
                case 3: return "审核成功";
                case 4: return "审核失败";
                default: return "";
            }
        }
        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="type">格式化结果，type：day，具体到天；否则具体到秒</param>
        /// <returns></returns>
        public static string RefFormatTimeToString(DateTime time, string type)
        {
            return RefFormatTimeToString(time, type, "");
        }
        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="type">格式化结果，type：day，具体到天；否则具体到秒</param>
        /// <param name="outStr">为空的情况下，自定义输出内容</param>
        /// <returns></returns>
        public static string RefFormatTimeToString(DateTime time, string type, string outStr)
        {
            if (time.ToString("yyyy-MM-dd") == "0001-01-01" || time.ToString("yyyy-MM-dd") == "1970-01-01") return outStr;
            if (type == "day")
                return time.ToString("yyyy-MM-dd");
            else
                return time.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 根据数据查询类型返回查询值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string RefRadVal(int val)
        {
            switch (val)
            {
                case 1:
                    return "实  名";
                case 2:
                    return "硬件特征";
                case 3:
                    return "虚拟身份";
                default:
                    return "实  名";
            }
        }
        /// <summary>
        /// 设备状态
        /// </summary>
        /// <param name="DevStatisID">状态id</param>
        /// <returns></returns>
        public static string DevStatis(int DevStatisID)
        {
            switch (DevStatisID)
            {
                case 1: return "设备工作";
                case 2: return "设备维护";
                case 3: return "设备异常";
                default: return "";
            }
        }
        /// <summary>
        /// 当前用户在对应路径下的权限
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string RefPageRelaByPath(string path)
        {
            string message = "";
            HttpContext current = HttpContext.Current;
            UserInfo user = (UserInfo)current.Session["userinfo"];
            if (user != null)
            {
                IEnumerable<Sys_UserPowerInfo> list = user._UserShowPage.Where(m => m.Pid > 0 && m.FilePath.ToUpper().Equals(path.ToUpper()));
                if (list != null && list.ToList().Count > 0)
                    return list.First().SelValues;
            }
            return message;
        }


        public static string ByteSubString(string strInput, int length)
        {
            if (strInput == null)
                return "";
            strInput = strInput.Trim();
            int byteLen = Encoding.Default.GetByteCount(strInput);
            if (byteLen > length)
            {
                string resultStr = String.Empty;
                for (int i = 0; i < strInput.Length; i++)
                {
                    if (Encoding.Default.GetByteCount(resultStr) < length)
                    {
                        resultStr += strInput.Substring(i, 1);
                    }
                    else
                    {
                        break;
                    }
                }
                return resultStr + "...";
            }
            else
            {
                return strInput;
            }
        }
        /// <summary>
        /// 文件大小单位转换,以KB做为依据
        /// </summary>
        /// <param name="fileSize">文件长度</param>
        /// <returns>文件大小字符串</returns>
        public static String FormatFileSize(Int64 fileSize)
        {
            if (fileSize < 0)
            {
                throw new ArgumentOutOfRangeException("fileSize");
            }
            else if (fileSize >= 1024 * 1024 * 1024)
            {
                return string.Format("{0:########0.00} GB", ((Double)fileSize) / (1024 * 1024 * 1024));
            }
            else if (fileSize >= 1024 * 1024)
            {
                return string.Format("{0:####0.00} MB", ((Double)fileSize) / (1024 * 1024));
            }
            else if (fileSize >= 1024)
            {
                return string.Format("{0:####0.00} KB", ((Double)fileSize) / 1024);
            }
            else
            {
                return string.Format("{0} B", fileSize);
            }
        }
        /// <summary>
        /// 对邮件密码进行加密
        /// </summary>
        /// <param name="SPassWord"></param>
        /// <returns></returns>
        public static string RefSendPwd(string SPassWord)
        {
            if (!string.IsNullOrEmpty(SPassWord))
            {
                if (SPassWord.Length > 4)
                    return SPassWord.Substring(0, 2) + "***" + SPassWord.Substring(SPassWord.Length - 2);
                else if (SPassWord.Length > 2)
                    return SPassWord.Substring(0, 2) + "***";
                else
                    return SPassWord + "***";
            }
            else
                return "";

        }
        /// <summary>
        /// 布控项类型转换
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string pwItemvalue(int type)
        {
            switch (type)
            {
                case 1: return "手机号码";
                case 2: return "设备MAC地址";
                case 3: return "IMEI";
                default: return "";
            }
        }
        /// <summary>
        /// 虚拟身份类型转换
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static string VidContentType(string Type)
        {
            switch (Type)
            {
                case "QQ": return "20";
                case "淘宝": return "21";
                case "微信": return "22";
                case "新浪微博": return "23";
                case "IMEI": return "24";
                case "IMSI": return "25";
                case "京东": return "26";
                case "苏宁易购": return "27";
                case "天涯": return "29";
                case "猫扑": return "30";
                case "滴滴": return "32";
                case "QQ邮箱": return "33";
                case "网易邮箱": return "34";
                case "携程": return "35";
                case "去哪儿": return "36";
                case "阿里旺旺": return "37";
                case "飞信": return "38";
                case "米聊": return "39";
                case "陌陌": return "40";
                case "新浪邮箱": return "41";
                case "139邮箱": return "42";
                case "天猫": return "43";
                case "唯品会": return "44";
                case "美团": return "45";
                case "大众点评": return "46";
                case "12306": return "47";
                case "赶集网": return "48";
                case "58同城": return "49";
                case "QQ微博": return "50";
                case "20": return "QQ";
                case "21": return "淘宝";
                case "22": return "微信";
                case "23": return "新浪微博";
                case "24": return "IMEI";
                case "25": return "IMSI";
                case "26": return "京东";
                case "27": return "苏宁易购";
                case "29": return "天涯";
                case "30": return "猫扑";
                case "32": return "滴滴";
                case "33": return "QQ邮箱";
                case "34": return "网易邮箱";
                case "35": return "携程";
                case "36": return "去哪儿";
                case "37": return "阿里旺旺";
                case "38": return "飞信";
                case "39": return "米聊";
                case "40": return "陌陌";
                case "41": return "新浪邮箱";
                case "42": return "139邮箱";
                case "43": return "天猫";
                case "44": return "唯品会";
                case "45": return "美团";
                case "46": return "大众点评";
                case "47": return "12306";
                case "48": return "赶集网";
                case "49": return "58同城";
                case "50": return "QQ微博";
                default: return "";
            }
        }
        /// <summary>
        /// 将数值型IP转换为String型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RefIPvlong(long ipLong)
        {
            long[] mask = { 0x000000FF, 0x0000FF00, 0x00FF0000, 0xFF000000 };
            long num = 0;
            StringBuilder ipInfo = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                num = (ipLong & mask[i]) >> (i * 8);
                if (i > 0) ipInfo.Insert(0, ".");

                ipInfo.Insert(0, Convert.ToString(num, 10));
            }
            return ipInfo.ToString();
        }
                /// <summary>
        /// 上网日志协议信息
        /// </summary>
        /// <returns></returns>
        public static string GetDetailHttpLog(string value)
        {
            value = string.Format("{0:00}", value);
            var ilist = GetNetworkAppList().Where(m => m.Key == value);
            if (ilist == null || ilist.Count() == 0)
                return "";
            return ilist.First().Value;
        }
        /// <summary>
        /// 虚拟身份列表
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetNetworkAppList()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>() 
            {
                {"-1","所有虚拟身份"},{"01","HTTP协议"},{"02","WAP协议"},{"03","SMTP协议"},{"04","POP3协议"},
                {"05","IMAP协议"},{"06","NNTP协议"},{"07","FTP协议"},{"08","SFTP协议"},{"09","TELNET协议"},{"10","HTTPS协议"},
                {"11","RSTP协议"},{"12","MMS协议"},{"13","WEP协议"},{"14","WPA协议"},{"15","PPTP协议"},{"16","L2TP协议"},
                {"17","SOCKS代理协议"},{"18","Compo协议"},{"19","Cmsmtp协议"},{"20","QQ"},{"21","淘宝"},{"22","微信"},
                {"23","新浪微博"},{"24","IMEI"},{"25","IMSI"},{"26","京东"},{"27","苏宁易购"},{"29","天涯"},
                {"30","猫扑"},{"32","滴滴"},{"33","QQ邮箱"},{"34","网易邮箱"},{"35","携程"},{"36","去哪儿"},
                {"37","阿里旺旺"},{"38","飞信"},{"39","米聊"},{"40","陌陌"},{"41","新浪邮箱"},{"42","139邮箱"},
                {"43","天猫"},{"44","唯品会"},{"45","美团"},{"46","大众点评"},{"47","12306"},{"48","赶集网"},
                {"49","58同城"},{"50","QQ微博"},{"51","QQ游戏"},{"52","百度搜索"},{"53","虎牙直播"},{"54","必应搜索"},
                {"55","360搜索"},{"91","私有协议"},{"99","其他协议"}
            };
            return dic;
        }
        public static string RefAuditTime(long time)
        {
            if (time > 0)
            {
                time = time + (8 * 3600);
                DateTime dt = DateTime.Parse("1970-01-01");
                dt = dt.AddSeconds(time);
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
                return "N/A";
        }
        /// <summary>
        /// 硬件产品类型
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> ProjectType()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>() 
            { 
                {1,"户外（全向）"},
                {2,"户外（定向）"},
                {3,"车载"},
                {4,"室内"},
                {5,"旁路"},
                {6,"吸顶"},
                {7,"视频二合一"},
                {99,"其他"}
            };
            return dic;
        }
        /// <summary>
        /// 硬件方案类型
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> CasesType()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>() 
            { 
                {1,"Atheros"},
                {2,"7620N"},
                {3,"X86"},
                {99,"其他"}
            };
            return dic;
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetCertifiCateList()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>() 
            { 
                {"111","居民身份证"},
                {"112","临时居民身份证"},
                {"113","户口薄"},
                {"114","中国人民解放军军官证"},
                {"115","中国人民武装警察部队普官证"},
                {"116","暂住证"},
                {"117","出生医学证明"},
                {"121","法官证"},
                {"123","警官证"},
                {"125","检察官证"},
                {"127","律师证"},
                {"129","记者证"},
                {"131","工作证"},
                {"133","学生证"},
                {"151","出人证"},
                {"153","临时出人证"},
                {"155","住宿证"},
                {"157","医疗证"},
                {"159","劳保证"},
                {"161","献血证"},
                {"163","保险单"},
                {"191","会员证"},
                {"211","离休证"},
                {"213","退休证"},
                {"215","老年证"},
                {"217","残疾证"},
                {"219","结婚证"},
                {"221","离婚证"},
                {"223","独生子女证"},
                {"225","毕业证书"},
                {"227","肄业证"},
                {"229","结业证"},
                {"231","学位证"},
                {"233","军人通行证"},
                {"291","证明信"},
                {"311","持枪证"},
                {"313","枪证"},
                {"315","枪支(弹药)挽运许可证"},
                {"317","砍伐证"},
                {"319","准运证"},
                {"321","准购证"},
                {"323","粮油证"},
                {"325","购煤证"},
                {"327","购煤气证"},
                {"329","房屋产权证"},
                {"331","土地使用证"},
                {"333","车辆通行证"},
                {"335","机动车驾驶证"},
                {"337","机动车行驶证"},
                {"339","机动车登记证书"},
                {"341","机动车年检合格证"},
                {"343","春运临时检验合格证"},
                {"345","飞机驾驶证"},
                {"347","船舶驾驶证"},
                {"349","船舶行驶证"},
                {"351","自行车行驶证"},
                {"353","汽车号牌"},
                {"355","拖拉机牌"},
                {"357","摩托车牌"},
                {"359","船舶牌"},
                {"361","三轮车牌"},
                {"363","自行车牌"},
                {"391","残疾人机动轮椅车"},
                {"411","外交护照"},
                {"412","公务护照"},
                {"413","因公普通护照"},
                {"414","普通护照"},
                {"415","旅行证"},
                {"416","人出境通行证"},
                {"417","外国人出人境证"},
                {"418","外国人旅行证"},
                {"419","海员证"},
                {"420","香港特别行政区护照"},
                {"421","澳门特别行政区护照"},
                {"423","澳门特别行政区旅行证"},
                {"511","台湾居民来往大陆通行证"},
                {"512","台湾居民来往大陆通行证(一次有效)"},
                {"513","往来港澳通行证"},
                {"515","前往港澳通行证"},
                {"516","港澳同胞回乡证(通行卡)"},
                {"517","大陆居民往来台湾通行证"},
                {"518","因公往来香港澳门特别行政区通行证"},
                {"551","华侨回国定居证"},
                {"552","台湾居民定居证"},
                {"553","外国人永久居留证"},
                {"554","外国人居留证"},
                {"555","外国人临时居留证"},
                {"556","人籍证书"},
                {"557","出籍证书"},
                {"558","复籍证书"},
                {"611","外籍船员住宿证"},
                {"612","随船工作证"},
                {"620","海上值勤证(红色)"},
                {"621","海上值勤证(蓝色)"},
                {"631","出海船民证"},
                {"633","出海船舶户口薄"},
                {"634","出海船舶边防登记簿"},
                {"635","搭靠台轮许可证"},
                {"636","台湾居民登陆证"},
                {"637","台湾船员登陆证"},
                {"638","外国船员登陆证"},
                {"639","对台劳务人员登轮作业证"},
                {"640","合资船船员登陆证"},
                {"641","合资船船员登轮作业证"},
                {"642","粤港澳流动渔民证"},
                {"643","粤港澳临时流动渔民证"},
                {"644","粤港澳流动渔船户口簿"},
                {"645","航行港澳船舶证明书"},
                {"646","往来港澳小型船舶查验薄"},
                {"650","劳务人员登轮作业证"},
                {"711","边境管理区通行证"},
                {"721","中朝鸭绿江、图们江水文作业证"},
                {"722","朝中鸭绿江、图们江水文作业证"},
                {"723","中朝流筏固定代表证"},
                {"724","朝中流筏固定代表证"},
                {"725","中朝鸭绿江、图们江船员证"},
                {"726","朝中鸭绿江、图们江船员证"},
                {"727","中朝边境地区公安总代表证"},
                {"728","朝中边境地区公安总代表证"},
                {"729","中朝边境地区公安副总代表证"},
                {"730","朝中边境地区公安副总代表证"},
                {"731","中朝边境地区公安代表证"},
                {"732","朝中边境地区公安代表证"},
                {"733","中朝边境地区出人境通行证(甲、乙种本)"},
                {"734","朝中边境公务通行证"},
                {"735","朝中边境住民国境通行证"},
                {"736","中蒙边境地区出人境通行证(甲、乙种本)"},
                {"737","蒙中边境地区出入境通行证"},
                {"738","中缅边境地区出人境通行证"},
                {"739","缅甸中国边境通行证"},
                {"740","云南省边境地区境外边民人出境证"},
                {"741","中尼边境地区出人境通行证"},
                {"742","尼中边境地区出人境通行证"},
                {"743","中越边境地区出入境通行证"},
                {"744","越中边境地区出人境通行证"},
                {"745","中老边境地区出人境通行证"},
                {"746","老中边境地区出人境通行证"},
                {"747","中印边境地区出人境通行证"},
                {"748","印中边境地区出人境通行证"},
                {"761","深圳市过境耕作证"},
                {"765","贸易证"},
                {"771","铁路员工证"},
                {"781","机组人员证"},
                {"990","其他"}
            };
            return dic;
        }
        /// <summary>
        /// 系统参数配置类型集合
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> ConfigType()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>() 
            { 
                {1,"心跳包间隔时间"},
                {2,"配置信息查询间隔时间"},
                {3,"电子围栏信息发送时间"},
                {4,"电子围栏离线时间"},
                {5,"上网设备离线时间"},
                {6,"信息采集粒度"},
                {7,"电子围栏采集范围"},
                {8,"采集速度"},
                {9,"GPS位置上报时间"},
                {10,"系统日志存储时间"},
                {11,"时间窗口内采集数据去重"},
                {12,"密钥"},
                {13,"初始向量"}
            };
            return dic;
        }


        /// <summary>
        /// 将系统参数配置类型转换为字符
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string RefConfigType(int type)
        {
            switch (type)
            {
                case 1: return "心跳包间隔时间";
                case 2: return "配置信息查询间隔时间";
                case 3: return "电子围栏信息发送时间";
                case 4: return "电子围栏离线时间";
                case 5: return "上网设备离线时间";
                case 6: return "信息采集粒度";
                case 7: return "电子围栏采集范围";
                case 8: return "采集速度";
                case 9: return "GPS位置上报时间";
                case 10: return "系统日志存储时间";
                case 11: return "时间窗口内采集数据去重";
                case 12: return "密钥";
                case 13: return "初始向量";
                default: return "";
            }
        }

        /// <summary>
        /// 将配置值转换为文本值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string RefSysConfigVal(int type, string val)
        {
            if (type == 6)
            {
                List<int> list = GetReturnInt(Convert.ToInt32(val));
                string[] splist = new string[list.Count];

                for (int i = 0; i < list.Count; i++)
                {
                    int k = list[i];
                    if (k == 1)
                        splist[i] = "HTTP日志";
                    else if (k == 2)
                        splist[i] = "电子围栏";
                    else if (k == 4)
                        splist[i] = "虚拟账户";
                    else if (k == 8)
                        splist[i] = "数据解密";
                }

                return string.Join("、", splist);
            }
            else if (type == 7)
                return val + "米";
            else if (type == 8)
                return val + "个";
            else if (type == 9 || type == 3 || type == 4 || type == 11)
                return val + "秒";
            else if (type == 10)
                return val + "小时";
            else if (type == 12 || type == 13)
                return val;
            else
                return val + "分钟";
        }

        /// <summary>
        /// 或运算将数值进行分组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<int> GetReturnInt(int value)
        {
            List<int> list = new List<int>();
            if (value == -1)
                return list;
            if (value == 0)
            {
                list.Add(0);
                return list;
            }
            uint ptv = Convert.ToUInt32(value);
            uint[] urr = new uint[] { 1, 2, 4, 8, 16, 32 };
            for (int i = 0; i < urr.Length; i++)
            {
                if ((ptv & urr[i]) == urr[i])
                    list.Add(Convert.ToInt32(urr[i]));
            }
            return list;
        }
        /// <summary>
        /// 或运算将字符转换为数值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ReturnUintValue(string value)
        {
            uint ptv = 0;
            if (value.Contains('0'))
                return Convert.ToInt32(ptv);
            if (value.IndexOf(',') > 0)
            {
                string[] split = value.Split(',');
                for (int i = 0; i < split.Length; i++)
                {
                    ptv = (ptv | Convert.ToUInt32(split[i]));
                }
            }
            else
                ptv = Convert.ToUInt32(value);
            return Convert.ToInt32(ptv);
        }

        /// <summary>
        /// 是否可查看系统日志
        /// </summary>
        /// <param name="IsLog"></param>
        /// <returns></returns>
        public static string RefIsLogType(int IsLog)
        {
            switch (IsLog)
            {
                case 1:
                    return "是";
                case 2:
                    return "否";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 证件类型转换
        /// </summary>
        /// <param name="IsLog"></param>
        /// <returns></returns>
        public static string RefIdType(int idType)
        {
            switch (idType)
            {
                case 1:
                    return "身份证";
                case 2:
                    return "军官证";
                case 3:
                    return "港澳通行证";
                case 4:
                    return "护照";
                case 5:
                    return "驾驶证";
                default:
                    return "";
            }
        }
        /// <summary>
        /// 根据用户类型获取对应的
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> UserListDIC()
        {
            Dictionary<int, string> dic = null;
            int userType = GetUserType()[0];
            if (userType == 1)
                dic = new Dictionary<int, string> { { 7, "全国用户" }, { 6, "省用户" }, { 4, "市局用户" }, { 2, "分局用户" }, { 3, "派出所用户" }, { 8, "运营用户" } };
            else if (userType == 2)
                dic = new Dictionary<int, string> { { 3, "派出所用户" } };
            else if (userType == 4)
                dic = new Dictionary<int, string> { { 2, "分局用户" }, { 3, "派出所用户" } };
            else if (userType == 6)
                dic = new Dictionary<int, string> { { 4, "市局用户" }, { 2, "分局用户" }, { 3, "派出所用户" } };
            else if (userType == 7)
                dic = new Dictionary<int, string> { { 6, "省用户" }, { 4, "市局用户" }, { 2, "分局用户" }, { 3, "派出所用户" } };
            return dic;
        }
    }
}
