using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using HCZZ.ModeDB;

namespace MergeWar.Filter
{
    public class SystemAutherFilter : System.Web.Mvc.ActionFilterAttribute
    {
        /// <summary>
        /// 权限判断
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            Controller controller = ((Controller)filterContext.Controller);
            string controll = filterContext.RouteData.Values["Controller"].ToString();
            string action = filterContext.RouteData.Values["Action"].ToString();
            UserInfo user = session["userInfo"] as UserInfo;
            if (user == null)
            {
                filterContext.Result = new ContentResult() { Content = "<script>alert('身份已经过期，请重新登录');parent.location.href='" + controller.Url.Content("~/Login/Index") + "';</script>" };
            }
            else
            {
                if (user.PowerPathList != null && user.PowerPathList.Count > 0)
                {
                    IEnumerable<Sys_UserPowerInfo> list = user.PowerPathList.Where(m => m.FilePath.ToUpper().Contains((controll + "/" + action).ToUpper()));
                    if (list == null || list.ToList().Count == 0)
                        filterContext.Result = new ContentResult() { Content = "<script>alert('权限不足');history.go(-1);</script>" };
                    else
                    {
                        string fValue = "";
                        string sValue = "";
                        int pageType = list.ToList().First().PageType;
                        if (pageType == 1)
                        {
                            fValue = list.ToList().First().IdValues;
                            sValue = list.ToList().First().SelValues;
                        }
                        else
                        {
                            fValue = list.ToList().First().SelValues;
                            sValue = list.ToList().First().IdValues;
                        }
                        string[] sp = null;
                        if (sValue.IndexOf(',') > -1)
                            sp = sValue.Split(',');
                        else
                            sp = new string[] { sValue };
                        for (int i = 0; i < sp.Length; i++)
                        {
                            if (!fValue.Contains(sp[i]))
                            {
                                filterContext.Result = new ContentResult() { Content = "<script>alert('权限不足');history.go(-1);</script>" };
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
