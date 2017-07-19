using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace HCZZ.ModeDB
{
    /// <summary>
    /// 操作日志表
    /// 创建人：邓佳训
    /// 创建时间：2017-06-19
    /// </summary>
   public class OPLog
    {
       public int ID { get; set; }
       public int UID { get; set; }
       public string Module { get; set; }
       public  string What { get; set; }
       public string ShowWhat
       {
           get;
           set;
       }
       public string IpAddress { get; set; }
       public DateTime CreateTime { get; set; }
       public  long PageNum { get; set; }
       public  string UserName { get; set; }

        public OPLog()
        {
            IpAddress =HttpContext.Current.Request.UserHostAddress;
            IpAddress=IpAddress == "::1" ? "127.0.0.1" : IpAddress;
            CreateTime = DateTime.Now;
            if (((UserInfo)HttpContext.Current.Session["userInfo"])!=null)
            {
                 UserName = ((UserInfo)HttpContext.Current.Session["userInfo"]).UserName;
                 UID = ((UserInfo) HttpContext.Current.Session["userInfo"]).ID;
            }
        }
    }
}
