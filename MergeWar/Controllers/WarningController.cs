using MergeWar.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HCZZ.Controllers
{
    [SystemAutherFilter]
    public class WarningController : Controller
    {
        //
        // GET: /Warning/
        string lishow = "Warning";
        public ActionResult Index()
        {
            ViewBag.lishow = lishow;
            return View();
        }

    }
}
