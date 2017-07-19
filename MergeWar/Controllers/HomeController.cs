using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using Common;
using HCZZ.DAL;
using HCZZ.ModeDB;
using MergeWar.Filter;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HCZZ.Controllers
{
    [SystemAutherFilter]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            string lishow = "home";
            ViewBag.lishow = lishow;
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Home/Index") + "'>首页</a>";
            ViewBag.lishow = "SY";
            return View();
        }
    }
}
