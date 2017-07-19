using MergeWar.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HCZZ.Controllers
{
    [SystemAutherFilter]
    public class MacController : Controller
    {
        private string lishow = "MACFX";
        public ActionResult Index()
        {
            ViewBag.lfetShow = "Mac分析";
            ViewBag.lishow = lishow;
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Mac/Index") + "'>Mac分析</a>>><a href='" + Url.Content("~/Mac/Index") + "'>碰撞分析</a>";
            return View();
        }

        #region 碰撞分析
        public ActionResult Collision() {
            ViewBag.lfetShow = "Mac分析";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "collision";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Mac/Collision") + "'>Mac分析</a>>><a href='" + Url.Content("~/Mac/Collision") + "'>碰撞分析</a>";
            return View();
        }
        [HttpGet]
        public ActionResult Analysis(string ids,string txtStartTimes, string txtEndTimes) {
            ViewBag.lfetShow = "Mac分析";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "collision";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Mac/Collision") + "'>Mac分析</a>>><a href='" + Url.Content("~/Mac/Collision") + "'>碰撞分析结果</a>";
            ViewBag.ids = ids;
            return View();
        }
        #endregion

        #region 伴随分析
        public ActionResult Follow()
        {
            ViewBag.lfetShow = "Mac分析";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "follow";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Mac/Collision") + "'>Mac分析</a>>><a href='" + Url.Content("~/Mac/Follow") + "'>伴随分析</a>";
            return View();
        }
        #endregion

        #region 出现设备
        public ActionResult AppearDevice()
        {
            ViewBag.lfetShow = "Mac分析";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "appear";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Mac/Collision") + "'>Mac分析</a>>><a href='" + Url.Content("~/Mac/AppearDevice") + "'>出现设备</a>";
            return View();
        }
        #endregion

        #region 消失设备
        public ActionResult DisappearDevice()
        {
            ViewBag.lfetShow = "Mac分析";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "disappear";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Mac/Collision") + "'>Mac分析</a>>><a href='" + Url.Content("~/Mac/DisappearDevice") + "'>消失设备</a>";
            return View();
        }
        #endregion

        public ActionResult Generate() {
            JsonResult result = new JsonResult();
            result.Data = Enumerable.Range(1, 100000);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
    }
}
