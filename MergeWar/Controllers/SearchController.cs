using System.Collections.Generic;
using System.Web.Mvc;
using HCZZ.ModeDB;
using HCZZ.Common;
using MergeWar.Filter;
using Newtonsoft.Json.Linq;
using System;
using Webdiyer.WebControls.Mvc;

namespace HCZZ.Controllers
{
    [SystemAutherFilter]
    public class SearchController : Controller
    {
        public static string PageSize = System.Configuration.ConfigurationManager.AppSettings["PageSize"];
        MergeWar.JavaServer.BigDataServiceClient java = new MergeWar.JavaServer.BigDataServiceClient();
        
        OPLog log = new OPLog();
        string lishow = "YJCX";
        public SearchController()
        {
            log.Module = "一键查询";
            ChangeValue.AddOpLog(log);
        }
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.lishow = lishow;
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Search/Index") + "'>一键查询</a>";
            return View();
        }

        /// <summary>
        /// 数据查询总界面
        /// </summary>
        /// <param name="Id">1：实名、3：硬件特征、2：虚拟身份</param>
        /// <param name="KeyWord">关键字</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Result(string KeyWord, int pageIndex = 1, int Id = 1)
        {
            if(KeyWord.Length<3)
                return Content("<script>alert('查询条件最少三位数!');history.go(-1);</script>");
            java.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
            string relList = string.Empty;
            ViewBag.lishow = lishow;
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Search/Index") + "'>一键查询</a>>查询结果";
            ViewBag.dic = new Dictionary<string, string> { { "TypeVal", Id.ToString() }, { "KeyWord", KeyWord } };
            try
            {
                List<DatasInfo> dlist = new List<DatasInfo>();
                string result = string.Empty;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("proID", string.IsNullOrEmpty(RefFilterUserAreaValue0(6))?"0": RefFilterUserAreaValue0(6));
                dic.Add("cityID",string.IsNullOrEmpty(RefFilterUserAreaValue0(4))?"0": RefFilterUserAreaValue0(4));
                dic.Add("aid",string.IsNullOrEmpty(RefFilterUserAreaValue0(2))?"0": RefFilterUserAreaValue0(2));
                dic.Add("pageIndex", pageIndex.ToString());
                try
                {
                    switch (Id)
                    {
                        case 1:
                            dic.Add("AUTH_ACCOUNT",KeyWord);
                            result = java.queryByRealnameInfo(Newtonsoft.Json.JsonConvert.SerializeObject(dic));
                            relList = "realnameInfoResult";
                            break;
                        case 2:
                            dic.Add("NETWORK_APP_VALUES", KeyWord);
                            result = java.queryByVirtualIdentity(Newtonsoft.Json.JsonConvert.SerializeObject(dic));
                            relList = "virtualIdentityResult";
                            break;
                        case 3:
                            dic.Add("MAC",KeyWord.ToUpper());
                            result = java.queryByHardwareFeature(Newtonsoft.Json.JsonConvert.SerializeObject(dic));
                            relList = "hardwareFeatureResult";
                            break;
                        default:
                            break;
                    }
                    int count = 0;
                    if (JObject.Parse(result)["state"].ToString() == "0")
                    {
                      count = Convert.ToInt32(JObject.Parse(result)["totalNumber"] ?? 0);
                      dlist =Newtonsoft.Json.JsonConvert.DeserializeObject<List<DatasInfo>>(JObject.Parse(result)[relList].ToString());
                    }
                    ViewBag.pl = new PagedList<DatasInfo>(dlist,pageIndex,Convert.ToInt32(PageSize), count);
                }
                catch (System.Exception solr)
                {
                    Logger.ErrorLog(solr, null);
                    ViewBag.errscript = "alert('请求超时，请稍后重试')";
                }
            }
            catch (System.Exception ex)
            {
                Logger.ErrorLog(ex, null);
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }
        /// <summary>
        /// 根据用户类型返回区域值并过滤区域值为0的情况
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public string RefFilterUserAreaValue0(int Type)
        {
            int[] userType = ChangeValue.GetUserType();
            return ((userType[0] == Type || userType[0] == 8) && userType[1] != 0) ? userType[1].ToString() : null;
        }
        /// <summary>
        /// 一键查询详细页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ResultDet()
        {
            return View();
        }
    }
}
