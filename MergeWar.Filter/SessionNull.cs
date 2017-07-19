using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HCZZ.ModeDB;

namespace HCZZt.Filter
{
    public class SessionNull : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            Controller controller = ((Controller)filterContext.Controller);
            UserInfo user = session["userInfo"] as UserInfo;
            if (user == null)
            {
                filterContext.Result = new ContentResult() { Content = "<script>alert('登录超时，请重新登录');parent.location.href='" + controller.Url.Content("Login/Index") + "';</script>" };
            }
        }
    }
}
