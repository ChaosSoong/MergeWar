using System.Collections.Generic;
using System.Web.Mvc;
using HCZZ.ModeDB;
using Common;
using MergeWar.Filter;

namespace HCZZ.Controllers
{
    [SystemAutherFilter]
    public class SearchController : Controller
    {
        OPLog log = new OPLog();
        public SearchController()
        {
            ViewBag.lishow = "Search";
            log.Module = "一键查询";
            ChangeValue.AddOpLog(log);
        }
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Search/Index") + "'>一键查询</a>";
            return View();
        }

        /// <summary>
        /// 数据查询总界面
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="KeyWord"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Result(int Id, string KeyWord)
        {
            ViewBag.dic = new Dictionary<string, string> { { "TypeVal", Id.ToString() }, { "KeyWord", KeyWord } };
            return View();
        }
    }
}
