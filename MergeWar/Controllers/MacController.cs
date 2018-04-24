using HCZZ.Common;
using MergeWar.Filter;
using MergeWar.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace HCZZ.Controllers
{
    [SystemAutherFilter]
    public class MacController : Controller
    {
        public static string PageSize = System.Configuration.ConfigurationManager.AppSettings["PageSize"];
        private string lishow = "MACFX";
        MergeWar.JavaServer.BigDataServiceClient java = new MergeWar.JavaServer.BigDataServiceClient();
        SolrShowModel solrmodel = new SolrShowModel();
        public ActionResult Index()
        {
            ViewBag.lfetShow = "Mac分析";
            ViewBag.lishow = lishow;
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Mac/Index") + "'>Mac分析</a>>><a href='" + Url.Content("~/Mac/Index") + "'>碰撞分析</a>";
            return View();
        }

        #region 碰撞分析
        public ActionResult Collision()
        {
            ViewBag.lfetShow = "Mac分析";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "collision";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Mac/Collision") + "'>Mac分析</a>>><a href='" + Url.Content("~/Mac/Collision") + "'>碰撞分析</a>";
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids">设备id集合</param>
        /// <param name="txtStartTimes">开始时间集合</param>
        /// <param name="txtEndTimes">结束时间集合</param>
        /// <param name="size">设计碰撞次数</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Analysis(string ids, string txtStartTimes, string txtEndTimes, string size)
        {
            try
            {
                ModeDB.UserInfo dbUser = (ModeDB.UserInfo)Session["userinfo"];
                List<Collision> clList = null;
                try
                {
                    clList = new MergeWar.AppCode.JavaServerAction().GetJavaData(ids, txtStartTimes, txtEndTimes, size, "1");
                    HttpContext.Cache.Insert("collisionAnalysisMACResult" + dbUser.ID, clList, null, DateTime.Now.AddHours(5), TimeSpan.Zero);
                    return Redirect(Url.Content("~/Mac/Analysis?md5Key=collisionAnalysisMACResult" + dbUser.ID));

                }
                catch (Exception solr)
                {
                    Logger.ErrorLog(solr, null);
                    return Content("<script>alert('网络错误!');history.go(-1);</script>");
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                return Content("<script>alert('未知错误!');history.go(-1);</script>");
            }
        }
        [HttpGet]
        public ActionResult Analysis(string md5Key, int pageIndex = 1)
        {
            ViewBag.lfetShow = "Mac分析";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "collision";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Mac/Collision") + "'>Mac分析</a>>><a href='" + Url.Content("~/Mac/Collision") + "'>碰撞分析结果</a>";
            List<Collision> clList = (List<Collision>)HttpContext.Cache[md5Key];
            var list = clList.ToPagedList(Convert.ToInt16(pageIndex), Convert.ToInt32(PageSize));
            ViewBag.pl = list;
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

        /// <summary>
        /// 伴随分析数据处理
        /// </summary>
        /// <param name="mac">mac地址</param>
        /// <param name="ids">设备id集合</param>
        /// <param name="txtStartTimes">开始时间</param>
        /// <param name="txtEndTimes">截止时间</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GowithSwot(string ids, string txtStartTimes, string txtEndTimes, string mac)
        {
            try
            {
                ModeDB.UserInfo dbUser = (ModeDB.UserInfo)Session["userinfo"];
                List<Collision> clList = null;
                try
                {
                    clList = new MergeWar.AppCode.JavaServerAction().GetJavaData(ids, txtStartTimes, txtEndTimes, mac, "2");
                    HttpContext.Cache.Insert("accompanyAnalysisMACResult" + dbUser.ID, clList, null, DateTime.Now.AddHours(5), TimeSpan.Zero);
                    return Redirect(Url.Content("~/Mac/GowithSwot?md5Key=accompanyAnalysisMACResult" + dbUser.ID));
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, null);
                    return Content("<script>alert('网络错误!');history.go(-1);</script>");
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                return Content("<script>alert('未知错误!');history.go(-1);</script>");
            }
        }
        [HttpGet]
        public ActionResult GowithSwot(string md5Key, int pageIndex = 1)
        {
            ViewBag.lfetShow = "Mac分析";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "follow";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Mac/Collision") + "'>Mac分析</a>>><a href='" + Url.Content("~/Mac/Follow") + "'>伴随分析</a>>伴随分析结果";
            List<Collision> clList = (List<Collision>)HttpContext.Cache[md5Key];
            var list = clList.ToPagedList(Convert.ToInt16(pageIndex), Convert.ToInt32(PageSize));
            ViewBag.pl = list;
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
        [HttpPost]
        public ActionResult AppearDevRes(string ids, string txtStartTimes, string txtEndTimes)
        {
            try
            {
                ModeDB.UserInfo dbUser = (ModeDB.UserInfo)Session["userinfo"];
                List<Collision> clList = null;
                try
                {
                    clList = new MergeWar.AppCode.JavaServerAction().GetJavaData(ids, txtStartTimes, txtEndTimes, "", "3");
                    HttpContext.Cache.Insert("appearDevMACResult" + dbUser.ID, clList, null, DateTime.Now.AddHours(5), TimeSpan.Zero);
                    return Redirect(Url.Content("~/Mac/AppearDevRes?md5Key=appearDevMACResult" + dbUser.ID));
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, null);
                    return Content("<script>alert('网络错误!');history.go(-1);</script>");
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                return Content("<script>alert('未知错误!');history.go(-1);</script>");
            }
        }
        [HttpGet]
        public ActionResult AppearDevRes(string md5Key, int pageIndex = 1)
        {
            ViewBag.lfetShow = "Mac分析";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "appear";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Mac/Collision") + "'>Mac分析</a>>><a href='" + Url.Content("~/Mac/AppearDevice") + "'>出现设备</a>>分析结果";
            List<Collision> clList = (List<Collision>)HttpContext.Cache[md5Key];
            var list = clList.ToPagedList(Convert.ToInt16(pageIndex), Convert.ToInt32(PageSize));
            ViewBag.pl = list;
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
        [HttpPost]
        public ActionResult DisappearDevRes(int? pageIndex, string ids, string txtStartTimes, string txtEndTimes, string mac)
        {
            try
            {
                ModeDB.UserInfo dbUser = (ModeDB.UserInfo)Session["userinfo"];
                List<Collision> clList = null;
                try
                {
                        clList = new MergeWar.AppCode.JavaServerAction().GetJavaData(ids, txtStartTimes, txtEndTimes, "", "4");
                        HttpContext.Cache.Insert("disappearDevMACResult" + dbUser.ID, clList, null, DateTime.Now.AddHours(5), TimeSpan.Zero);
                    return Redirect(Url.Content("~/Mac/DisappearDevRes?md5Key=disappearDevMACResult" + dbUser.ID));

                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, null);
                    return Content("<script>alert('网络错误!');history.go(-1);</script>");
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                return Content("<script>alert('未知错误!');history.go(-1);</script>");
            }
        }
        public ActionResult DisappearDevRes(string md5Key, int pageIndex = 1)
        {
            ViewBag.lfetShow = "Mac分析";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "disappear";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Mac/Collision") + "'>Mac分析</a>>><a href='" + Url.Content("~/Mac/DisappearDevice") + "'>消失设备</a>>分析结果";
            List<Collision> clList = (List<Collision>)HttpContext.Cache[md5Key];
            var list = clList.ToPagedList(Convert.ToInt16(pageIndex), Convert.ToInt32(PageSize));
            ViewBag.pl = list;
            return View();
        }
        #endregion
    }
}
