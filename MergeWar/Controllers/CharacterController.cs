using MergeWar.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HCZZ.Controllers
{
    [SystemAutherFilter]
    public class CharacterController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.LfetShow = "特征碰撞";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Charater/Index") + "'>特征碰撞</a>>><a href='" + Url.Content("~/Charater/Index") + "'>特征碰撞</a>";
            return View();
        }

    }
}
