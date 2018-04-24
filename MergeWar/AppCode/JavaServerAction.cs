using HCZZ.AppCode;
using HCZZ.ModeDB;
using MergeWar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MergeWar.AppCode
{
    public class JavaServerAction
    {
        MergeWar.JavaServer.BigDataServiceClient java = new MergeWar.JavaServer.BigDataServiceClient();
        SolrShowModel solrmodel = new SolrShowModel();
        /// <summary>
        ///MAC分析调用Java
        /// </summary>
        /// <param name="ids">设备id</param>
        /// <param name="txtStartTimes">开始时间</param>
        /// <param name="txtEndTimes">结束时间</param>
        /// <param name="keys">碰撞次数/Mac地址</param>
        /// <param name="type">类别：1碰撞、2伴随、3出现设备、4消失设备</param>
        /// <returns></returns>
        public List<Collision> GetJavaData(string ids, string txtStartTimes, string txtEndTimes, string keys, string type)
        {
            string CacheType = string.Empty;
            UserInfo dbUser =(UserInfo) HttpContext.Current.Session["userinfo"];
            java.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
            ids = ids.Substring(ids.Length - 1, 1) == "_" ? ids.Substring(0, ids.Length - 1) : ids;
            txtStartTimes =txtStartTimes== "NaN_" ? "0":  txtStartTimes.Substring(txtStartTimes.Length - 1, 1) == "_" ? txtStartTimes.Substring(0, txtStartTimes.Length - 1) : txtStartTimes;
            txtEndTimes = txtEndTimes== "NaN_" ? "4099776775" : txtEndTimes.Substring(txtEndTimes.Length - 1, 1) == "_" ? txtEndTimes.Substring(0, txtEndTimes.Length - 1) : txtEndTimes;
            List<Collision> clList = new List<Collision>();
            var argjson = "";
            switch (type)
            {
                case "1":
                    var arg = new
                    {
                        size = keys,
                        devIds = ids,
                        startTime = txtStartTimes,
                        endTime = txtEndTimes,
                    };
                    argjson = Newtonsoft.Json.JsonConvert.SerializeObject(arg);
                    argjson = java.collisionAnalysisMAC(argjson);
                    CacheType = "pz";
                    break;
                case "2":
                    var arg2 = new
                    {
                        mac = keys,
                        devIds = ids,
                        startTime=txtStartTimes,
                        endTime=txtEndTimes
                    };
                    argjson = Newtonsoft.Json.JsonConvert.SerializeObject(arg2);
                    argjson = java.accompanyAnalysisMAC(argjson);
                    CacheType = "bs";
                    break;
                case "3":
                    var arg3 = new
                    {
                        devIds = ids,
                        startTime = txtStartTimes,
                        endTime = txtEndTimes,
                    };
                    argjson = Newtonsoft.Json.JsonConvert.SerializeObject(arg3);
                    argjson = java.appearDevMAC(argjson);
                    CacheType = "cx";
                    break;
                case "4":
                    var arg4 = new
                    {
                        devIds = ids,
                        startTime = txtStartTimes,
                        endTime = txtEndTimes,
                    };
                    argjson = Newtonsoft.Json.JsonConvert.SerializeObject(arg4);
                    argjson = java.disappearDevMAC(argjson);
                    CacheType = "xs";
                    break;
                default: break;
            }
            HttpContext.Current.Cache.Insert(CacheType+"ids" + dbUser.ID, ids, null, DateTime.Now.AddHours(5), TimeSpan.Zero);
            HttpContext.Current.Cache.Insert(CacheType+"txtStartTimes" + dbUser.ID, txtStartTimes, null, DateTime.Now.AddHours(5), TimeSpan.Zero);
            HttpContext.Current.Cache.Insert(CacheType + "txtEndTimes" + dbUser.ID, txtEndTimes, null, DateTime.Now.AddHours(5), TimeSpan.Zero);
            HttpContext.Current.Cache.Insert(CacheType + "keys" + dbUser.ID, keys, null, DateTime.Now.AddHours(5), TimeSpan.Zero);
            var Json = Newtonsoft.Json.Linq.JObject.Parse(argjson);
            if (Json["state"].ToString() == "1")
            {
                return clList;
            }
            else
            {
                switch (type)
                {
                    case "1":
                        clList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Collision>>(Json["collisionAnalysisMACResult"].ToString());
                        break;
                    case "2":
                        clList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Collision>>(Json["accompanyAnalysisMACResult"].ToString());
                        break;
                    default:
                        string relult = type == "3" ? "appearDevMACResult" : "disappearDevMACResult";
                        clList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Collision>>(Json[""+ relult + ""].ToString());
                        if ((List<Loc_NetBarInfo>)System.Web.HttpContext.Current.Cache["ShowCacheLoca"] == null)
                            WebCommon.GetCacheLoca();
                        List<Loc_NetBarInfo> netlist = (List<Loc_NetBarInfo>)System.Web.HttpContext.Current.Cache["ShowCacheLoca"];
                        //获取设备缓存
                        if ((List<Loc_DevInfo>)System.Web.HttpContext.Current.Cache["CacheMacAll"] == null)
                            WebCommon.GetCacheMacAllList();
                        List<Loc_DevInfo> delist = (List<Loc_DevInfo>)System.Web.HttpContext.Current.Cache["CacheMacAll"];
                        for (int i = 0; i < clList.Count; i++)
                        {
                            Loc_DevInfo dev = delist.Find(a => a.ID.Equals(clList[i].DevAP_ID));
                            if (dev != null)
                            {
                                Loc_NetBarInfo loc = netlist.Find(a => a.ID == (dev.NETBAR_ID));
                                if (loc != null)
                                {
                                    clList[i].PLACE_NAME = loc.PLACE_NAME;
                                    clList[i].APName = dev.APName;
                                }
                            }
                           // clList[i].CAPTURE_TIME = Szcert.Audit.CommonBase.ChangeValueBase.RefAuditTime(Convert.ToInt64(clList[i].CAPTURE_TIME));
                            clList[i].CAPTURE_TIME = HCZZ.Common.ChangeValue.RefAuditTime(Convert.ToInt64(clList[i].CAPTURE_TIME));
                        }
                        break;
                }
                //HttpContext.Current.Cache.Insert("MACAnalyzeDate", clList, null, DateTime.Now.AddHours(5), TimeSpan.Zero);
                return clList;
            }
        }
    }
}