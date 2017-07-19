using MergeWar.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HCZZ.Controllers
{
    [SystemAutherFilter]
    public class FenceController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.LfetShow = "围栏分析";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Plate/Index") + "'>围栏分析</a>>><a href='" + Url.Content("~/Plate/Index") + "'>围栏分析</a>";
            return View();
        }

    }
}
