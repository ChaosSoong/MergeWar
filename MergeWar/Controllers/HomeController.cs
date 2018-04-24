using System.Web.Mvc;
using MergeWar.Filter;

namespace HCZZ.Controllers
{
    [SystemAutherFilter]
    public class HomeController : Controller
    {        /// <summary>
             /// 每页显示数量
             /// </summary>
        public static string PageSize = System.Configuration.ConfigurationManager.AppSettings["PageSize"];

        public ActionResult Index()
        {
            string lishow = "home";
            ViewBag.lishow = lishow;
            //ViewBag.location = "所在位置：<a href='" + Url.Content("~/Home/Index") + "'>首页</a>";
            ViewBag.lishow = "SY";
            return View();
        }
    }
}
