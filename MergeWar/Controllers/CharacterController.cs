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
        string lishow = "TZPZ";
        /// <summary>
        /// 每页显示数量
        /// </summary>
        public static string PageSize = System.Configuration.ConfigurationManager.AppSettings["PageSize"];
        #region MAC-IMEI
        public ActionResult MacImei()
        {
            ViewBag.LfetShow = "特征碰撞";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "MacImei";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Charater/MacImei") + "'>特征碰撞</a>>><a href='" + Url.Content("~/Charater/MacImei") + "'>MAC-IMEI</a>";
            return View();
        }
        #endregion
        #region MAC-车牌
        public ActionResult MacPlate()
        {
            ViewBag.LfetShow = "特征碰撞";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "MacPlate";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Charater/MacImei") + "'>特征碰撞</a>>><a href='" + Url.Content("~/Charater/MacPlate") + "'>MAC-车牌</a>";
            return View();
        }
        #endregion
        #region MAC-IMSI
        public ActionResult MacImsi()
        {
            ViewBag.LfetShow = "特征碰撞";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "MacImsi";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Charater/MacImei") + "'>特征碰撞</a>>><a href='" + Url.Content("~/Charater/MacImsi") + "'>MAC-IMSI</a>";
            return View();
        }
        #endregion
        #region IMEI-IMSI
        public ActionResult ImeiImsi()
        {
            ViewBag.LfetShow = "特征碰撞";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "ImeiImsi";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Charater/MacImei") + "'>特征碰撞</a>>><a href='" + Url.Content("~/Charater/ImeiImsi") + "'>IMEI-IMSI</a>";
            return View();
        }
        #endregion
        #region IMEI-车牌
        public ActionResult ImeiPlate()
        {
            ViewBag.LfetShow = "特征碰撞";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "ImeiPlate";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Charater/MacImei") + "'>特征碰撞</a>>><a href='" + Url.Content("~/Charater/ImeiPlate") + "'>IMEI-车牌</a>";
            return View();
        }
        #endregion
        #region 车牌-IMSI
        public ActionResult PlateImsi()
        {
            ViewBag.LfetShow = "特征碰撞";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "PlateImsi";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Charater/MacImei") + "'>特征碰撞</a>>><a href='" + Url.Content("~/Charater/PlateImsi") + "'>车牌-IMSI</a>";
            return View();
        }
        #endregion

    }
}
