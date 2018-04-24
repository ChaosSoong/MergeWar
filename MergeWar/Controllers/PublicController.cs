using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using HCZZ.Common;
using HCZZ.AppCode;
using HCZZ.Models;
using Webdiyer.WebControls.Mvc;
using HCZZ.ModeDB;
using HCZZ.DAL;
using MergeWar.Models;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace HCZZ.Controllers
{

    public class PublicController : Controller
    {
        OPLog log = new OPLog();
        HomeCountDAL home = new HomeCountDAL();
        public static string PageSize = System.Configuration.ConfigurationManager.AppSettings["PageSize"];
        SolrShowModel solrmodel = new SolrShowModel();
        MergeWar.JavaServer.BigDataServiceClient java = new MergeWar.JavaServer.BigDataServiceClient();

        Index_Json json = new Index_Json();
        //创建人：邓佳训
        //时  间：2017-6-12
        //公用控制器，用来创建不在权限控制范围内的页面
        // GET: /Public/

        #region 导出未知设备
        [HttpGet]
        public ActionResult ExcelUnDevice(string txtAP_MAC, string txtStartTime, string txtEndTime, string AreaName)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                {"AP_MAC",txtAP_MAC},
                {"StartTime",txtStartTime},
                {"EndTime",txtEndTime},
                {"AreaName",AreaName}
            };
            DataTable dt = new LocationDAL().GetDeviceExcel(dic);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    StringBuilder New_APMAC = new StringBuilder();
                    char[] AP_MAC = dt.Rows[i]["AP_MAC"].ToString().ToCharArray();
                    for (int J = 0; J < AP_MAC.Length; J++)
                    {
                        New_APMAC.Append(AP_MAC[J]);
                        if (J % 2 != 0)
                        {
                            New_APMAC.Append("-");
                        }
                    }
                    dt.Rows[i]["AP_MAC"] = New_APMAC.ToString().Substring(0, New_APMAC.Length - 1);
                }
            }
            if (dt == null || dt.Rows.Count == 0)
            {
                return Content("{\"err\":\"对不起，没有要导出的数据\"}");
            }
            Dictionary<string, string> Table_dic = new Dictionary<string, string>()
            {
                {"审计终端MAC","AP_MAC"},
                {"最近更新时间","UpdateTime"},
                {"公网IP地址","OutIpAddress"},
                {"区域名称","AreaName"},
                {"创建时间","CreateTime"}
            };
            Dictionary<string, string> Head_Dic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(txtAP_MAC)) Head_Dic.Add("审计终端MAC", txtAP_MAC);
            if (!string.IsNullOrEmpty(txtStartTime)) Head_Dic.Add("最近更新时间范围--开始时间", txtStartTime);
            if (!string.IsNullOrEmpty(txtEndTime)) Head_Dic.Add("最近更新时间范围--结束时间", txtEndTime);
            NPOISheetModel sheel = new NPOISheetModel();
            sheel.dt = dt;
            sheel.ExcelTitle = "未知设备管理";
            sheel.TableTitle = Table_dic;
            sheel.TableSearch = Head_Dic;
            string path = new ComNPOIExcel().Export(sheel, Server.MapPath("~/") + "UserData/EXP/Loca/");
            return Content("{\"result\":\"" + path + "\"}");
        }
        #endregion

        #region  导出数据
        [HttpGet]
        public ActionResult LocaExport(string txtNetbar_Wacode, string txtPlace_Name, string txtSite_Address, string txtLaw_Principal_Name, string txtRelationship_Account, string selNetsite_Type, string selAccess_Type, string selStatus, string selBusiness_Nature, string selOperator_Net, string txtBeginTime, string txtEndTime, string selApfwStatus, string selApsjStatus, string txtAP_MAC, string selVerified, string txtOutIpAddress, string txtOtherDay, string selProvince, string selCity, string selArea, string selPolice, string SelPotType, string txtProjectAccessNum, string strExprotId)
        {
            try
            {
                selNetsite_Type = selNetsite_Type ?? "0";
                selStatus = selStatus ?? "0";
                selBusiness_Nature = selBusiness_Nature ?? "-1";
                selAccess_Type = selAccess_Type ?? "0";
                selOperator_Net = selOperator_Net ?? "0";
                selApfwStatus = selApfwStatus ?? "0";
                selApsjStatus = selApsjStatus ?? "0";
                selVerified = selVerified ?? "0";
                txtOtherDay = txtOtherDay ?? "0";
                selProvince = selProvince ?? "0";
                selCity = selCity ?? "0";
                selArea = selArea ?? "0";
                selPolice = selPolice ?? "0";
                SelPotType = SelPotType ?? "0";
                int[] userType = ChangeValue.GetUserType();
                UserInfo user = (UserInfo)Session["userInfo"];
                string proId = "0";
                if (userType[0] != 1 && userType[0] != 7)
                    proId = user.ProID.ToString();

                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"txtNetbar_Wacode",txtNetbar_Wacode},
                    {"txtPlace_Name",txtPlace_Name},
                    {"txtSite_Address",txtSite_Address},
                    {"txtLaw_Principal_Name",txtLaw_Principal_Name},
                    {"txtRelationship_Account",txtRelationship_Account},
                    {"selNetsite_Type",selNetsite_Type},
                    {"selStatus",selStatus},
                    {"selBusiness_Nature",selBusiness_Nature},
                    {"selAccess_Type",selAccess_Type},
                    {"selOperator_Net",selOperator_Net},
                    {"UserType",userType[0].ToString()},
                    {"AreaId",userType[1].ToString()},
                    {"txtBeginTime",txtBeginTime},
                    {"txtEndTime",txtEndTime},
                    {"AP_MAC",txtAP_MAC},
                    {"selApfwStatus",selApfwStatus},
                    {"selApsjStatus",selApsjStatus},
                    {"selVerified",selVerified},
                    {"txtOutIpAddress",txtOutIpAddress},
                    {"txtOtherDay",txtOtherDay},
                    {"selProvince",selProvince},
                    {"selCity",selCity},
                    {"selArea",selArea},
                    {"selPolice",selPolice},
                    {"strExprotId",strExprotId},
                    {"SelPotType",SelPotType},
                    {"ProjectAccessNum",txtProjectAccessNum}
                };
                DataTable dt = new LocationDAL().GetLocaDT(dic);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return Content("{\"err\":\"对不起，没有要导出的数据\"}");
                }
                else
                {
                    Dictionary<string, string> Head_Dic = new Dictionary<string, string>();
                    if (!string.IsNullOrEmpty(txtNetbar_Wacode)) Head_Dic.Add("上网服务场所编码", txtNetbar_Wacode);
                    if (!string.IsNullOrEmpty(txtPlace_Name)) Head_Dic.Add("上网服务场所名称", txtPlace_Name);
                    if (!string.IsNullOrEmpty(txtSite_Address)) Head_Dic.Add("场所详细地址", txtSite_Address);
                    if (!string.IsNullOrEmpty(txtLaw_Principal_Name)) Head_Dic.Add("场所经营法人", txtLaw_Principal_Name);
                    if (!string.IsNullOrEmpty(txtAP_MAC)) Head_Dic.Add("AP_MAC", txtAP_MAC);
                    if (!string.IsNullOrEmpty(txtRelationship_Account)) Head_Dic.Add("联系方式", txtRelationship_Account);
                    if (selNetsite_Type != "0") Head_Dic.Add("场所服务类型", ChangeValue.GetLocaTypeValue(Convert.ToInt32(selNetsite_Type)));
                    if (selApfwStatus != "0" && selApsjStatus != "0") Head_Dic.Add("设备状态", ((RefAPStatus(1, selApfwStatus, txtOtherDay)) + (RefAPStatus(2, selApsjStatus, txtOtherDay))));
                    else if (selApfwStatus != "0") Head_Dic.Add("设备状态", (RefAPStatus(1, selApfwStatus, txtOtherDay)));
                    else if (selApsjStatus != "0") Head_Dic.Add("设备状态", (RefAPStatus(2, selApsjStatus, txtOtherDay)));
                    if (selBusiness_Nature != "-1") Head_Dic.Add("场所经营性质", (selBusiness_Nature == "0" ? "经营" : (selBusiness_Nature == "1" ? "非经营" : "其他")));
                    if (selStatus != "0") Head_Dic.Add("场所状态", (selStatus == "1" ? "场所营业" : "场所停业"));
                    if (selAccess_Type != "0") Head_Dic.Add("场所网络接入方式", ChangeValue.GetConnectTypeValue(Convert.ToInt32(selAccess_Type)));
                    if (selOperator_Net != "0") Head_Dic.Add("场所网络接入服务商：", ChangeValue.GetServiceBusinesValue(selOperator_Net));
                    //if (selVerified != "0") Head_Dic.Add("场所审核状态：", ChangeValue.RefVerifiedStrExcel(Convert.ToInt32(selVerified)));
                    if (selProvince != "0") Head_Dic.Add("省Id：", selProvince);
                    if (selCity != "0") Head_Dic.Add("市Id：", selCity);
                    if (selArea != "0") Head_Dic.Add("区Id：", selArea);
                    if (selPolice != "0") Head_Dic.Add("派出所Id：", selPolice);
                    if (!string.IsNullOrEmpty(txtBeginTime)) Head_Dic.Add("备案开始时间", txtBeginTime);
                    if (!string.IsNullOrEmpty(txtEndTime)) Head_Dic.Add("备案结束时间", txtEndTime);
                    if (!string.IsNullOrEmpty(txtOutIpAddress)) Head_Dic.Add("网络接入账号或固定IP地址", txtOutIpAddress);
                    if (!string.IsNullOrEmpty(txtOutIpAddress)) Head_Dic.Add("拆换机审核状态：", SelPotType == "1" ? "待审核" : (SelPotType == "2" ? "审核通过" : "审核失败"));
                    if (!string.IsNullOrEmpty(txtOutIpAddress)) Head_Dic.Add("产品接入号：", txtProjectAccessNum);
                    Dictionary<string, string> Title_Dic = new Dictionary<string, string>() { };
                    string[] spExprotId = strExprotId.Split(',');
                    for (int i = 1; i < spExprotId.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(spExprotId[i]) && Title_Dic_Statc.ContainsKey(spExprotId[i]))
                            Title_Dic.Add(Title_Dic_Statc[spExprotId[i]].Key, Title_Dic_Statc[spExprotId[i]].Value);
                    }
                    NPOISheetModel sheet = new NPOISheetModel();
                    sheet.dt = dt;
                    sheet.ExcelTitle = "场所列表";
                    sheet.TableTitle = Title_Dic;
                    sheet.TableSearch = Head_Dic;

                    string path = new ComNPOIExcel().Export(sheet, Server.MapPath("~/") + "UserData/EXP/Loca/");
                    return Content("{\"result\":\"" + path + "\"}");
                }
            }
            catch (System.Data.SqlClient.SqlException sql)
            {
                Logger.ErrorLog(sql, new Dictionary<string, string>()
                {
                    {"Function","LocationController.LocaExport(string txtNetbar_Wacode, string txtPlace_Name, string txtSite_Address, string txtLaw_Principal_Name, string txtRelationship_Account, string selNetsite_Type, string selAccess_Type, string selStatus, string selBusiness_Nature, string selOperator_Net)[HttpGet]"}
                });
                return Content("{\"err\":\"导出文件出错，请稍后重试\"}");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.LocaExport(string txtNetbar_Wacode, string txtPlace_Name, string txtSite_Address, string txtLaw_Principal_Name, string txtRelationship_Account, string selNetsite_Type, string selAccess_Type, string selStatus, string selBusiness_Nature, string selOperator_Net)[HttpGet]"}
                });
                return Content("{\"err\":\"导出文件出错，请稍后重试\"}");
            }
            return View();
        }
        public string RefAPStatus(int type, string status, string val)
        {
            if (type == 1)
            {
                switch (status)
                {
                    case "1": return "服务在线";
                    case "2": return "服务离线";
                    case "3": return "其他服务在线 " + val + " 天";
                    case "4": return "其他服务离线 " + val + " 天";
                    default: return "";
                }
            }
            else
            {
                switch (status)
                {
                    case "1": return "数据在线";
                    case "2": return "数据离线";
                    case "3": return "其他数据在线 " + val + " 天";
                    case "4": return "其他数据离线 " + val + " 天";
                    default: return "";
                }
            }
        }
        //要导出的列
        static Dictionary<string, KeyValuePair<string, string>> Title_Dic_Statc = new Dictionary<string, KeyValuePair<string, string>>()
        {
            {"1", new KeyValuePair<string,string>("上网服务场所编码","NETBAR_WACODE")},
            {"2", new KeyValuePair<string,string>("上网服务场所名称","PLACE_NAME")},
            {"3", new KeyValuePair<string,string>("场所详细地址","SITE_ADDRESS")},
            {"4", new KeyValuePair<string,string>("场所服务类型","NETSITE_TYPE")},
            {"5", new KeyValuePair<string,string>("场所经营法人","LAW_PRINCIPAL_NAME")},
            {"6", new KeyValuePair<string,string>("联系方式","RELATIONSHIP_ACCOUNT")},
            {"7", new KeyValuePair<string,string>("场所状态","Statis")},
            {"8", new KeyValuePair<string,string>("安全厂商组织机构代码","CODE_ALLOCATION_ORGANIZATION")},
            {"9", new KeyValuePair<string,string>("经度","LONGITUDE")},
            {"10", new KeyValuePair<string,string>("纬度","LATITUDE")},
            {"11", new KeyValuePair<string,string>("审核状态","Verified")},
            {"12", new KeyValuePair<string,string>("网络接入账号或固定IP地址","ACSSES_IP")},
            {"13", new KeyValuePair<string,string>("经营法人有效证件类型","LAW_PRINCIPAL_CERTIFICATE_TYPE")},
            {"14", new KeyValuePair<string,string>("经营法人有效证件号码","LAW_PRINCIPAL_CERTIFICATE_ID")},
            {"15", new KeyValuePair<string,string>("场所网络接入方式","ACCESS_TYPE")},
            {"16", new KeyValuePair<string,string>("场所网络接入服务商","OPERATOR_NET")},
            {"17", new KeyValuePair<string,string>("营业开始时间","LocaSTARTTIME")},
            {"18", new KeyValuePair<string,string>("营业结束时间","LocaENDTIME")},
            {"19", new KeyValuePair<string,string>("所属省","ProName")},
            {"20", new KeyValuePair<string,string>("所属市","CName")},
            {"21", new KeyValuePair<string,string>("所属区","Area")},
            {"22", new KeyValuePair<string,string>("派出所","PName")},
            {"23", new KeyValuePair<string,string>("警区","SName")},
            {"24", new KeyValuePair<string,string>("备案时间","createTime")},
            {"25", new KeyValuePair<string,string>("服务状态","fuwu")},
            {"26", new KeyValuePair<string,string>("数据状态","shuju")},
            {"27", new KeyValuePair<string,string>("场所标识ID","ID")},
            {"28", new KeyValuePair<string,string>("AP标识ID","APID")},
            {"29", new KeyValuePair<string,string>("AP名称","APName")},
            {"30", new KeyValuePair<string,string>("APMac地址","AP_MAC")},
            {"31", new KeyValuePair<string,string>("产品接入号","ProjectAccessNum")},
            {"32", new KeyValuePair<string,string>("拆换机类型","PullOrTrade")},
            {"33", new KeyValuePair<string,string>("拆换机状态","PotType")},
            {"34", new KeyValuePair<string,string>("拆换机审核时间","CheckTime")}
        };
        [HttpGet]
        public ActionResult Download(string name, string path)
        {
            try
            {
                name = (name + DateTime.Now.ToString("yyyyMMddHHmmss")) + path.Substring(path.LastIndexOf("."));

                DownFile(name, path);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LoginController.Download(string name, string path)[HttpGet]"}
                });
                return Content("<script>alert('下载过程出现问题，请稍后重试')</script>");
            }
            return View();
        }
        #endregion

        /// <summary>
        ///Mac分析四类导出
        /// </summary>
        /// <returns></returns>
        public ActionResult AnalyzeExport(string dataType)
        {
            try
            {
                string CacheType = string.Empty;
                UserInfo dbUser = (UserInfo)Session["userinfo"];
                List<Collision> clList = null;
                Dictionary<string, string> Head_Dic = new Dictionary<string, string>();
                switch (dataType)
                {
                    case "1":
                        clList = (List<Collision>)System.Web.HttpContext.Current.Cache["collisionAnalysisMACResult" + dbUser.ID];
                        CacheType = "pz";
                        Head_Dic.Add("碰撞次数", System.Web.HttpContext.Current.Cache[CacheType + "keys" + dbUser.ID].ToString() + "次");
                        break;
                    case "2":
                        clList = (List<Collision>)System.Web.HttpContext.Current.Cache["accompanyAnalysisMACResult" + dbUser.ID];
                        CacheType = "bs";
                        Head_Dic.Add("MAC地址", System.Web.HttpContext.Current.Cache[CacheType + "keys" + dbUser.ID].ToString());
                        break;
                    case "3":
                        clList = (List<Collision>)System.Web.HttpContext.Current.Cache["appearDevMACResult" + dbUser.ID];
                        CacheType = "cx";
                        break;
                    case "4":
                        clList = (List<Collision>)System.Web.HttpContext.Current.Cache["disappearDevMACResult" + dbUser.ID];
                        CacheType = "xs";
                        break;
                    default:
                        break;
                }
                DataTable dt = Szcert.Audit.CommonBase.DataSetConvert.ToDataTable(clList);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return Content("{\"err\":\"对不起，没有要导出的数据\"}");
                }
                Dictionary<string, string> Table = new Dictionary<string, string>() { };
                if (dataType == "1" || dataType == "2")
                {
                    Table = new Dictionary<string, string> {
                     {"MAC地址","MAC"},
                     {"出现次数","c"}
                    };
                }
                else if (dataType == "3" || dataType == "4")
                {
                    Table = new Dictionary<string, string> {
                        {"场所名称","PLACE_NAME"},
                        {"设备名称","APName"},
                        {"最小采集时间","CAPTURE_TIME"},
                        {"Mac地址","MAC"}
                    };
                }
                NPOISheetModel sheel = new NPOISheetModel();
                sheel.dt = dt;
                switch (dataType)
                {
                    case "1":
                        sheel.ExcelTitle = "碰撞分析结果";
                        break;
                    case "2":
                        sheel.ExcelTitle = "伴随分析结果";
                        break;
                    case "3":
                        sheel.ExcelTitle = "出现设备结果";
                        break;
                    case "4":
                        sheel.ExcelTitle = "消失设备结果";
                        break;
                    default:
                        break;
                }

                Head_Dic.Add("设备ID", System.Web.HttpContext.Current.Cache[CacheType + "ids" + dbUser.ID].ToString());
                Head_Dic.Add("开始时间段", System.Web.HttpContext.Current.Cache[CacheType + "txtStartTimes" + dbUser.ID].ToString());
                Head_Dic.Add("结束时间段", System.Web.HttpContext.Current.Cache[CacheType + "txtEndTimes" + dbUser.ID].ToString());
                sheel.TableSearch = Head_Dic;
                sheel.TableTitle = Table;
                string path = new ComNPOIExcel().Export(sheel, Server.MapPath("~/") + "UserData/EXP/Analyze/");
                return Content("{\"result\":\"" + path + "\"}");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","PublicController.AnalyzeExport(string dataType)[HttpGet]"}
                });
                return Content("{\"err\":\"导出文件出错，请稍后重试\"}");
            }
        }

        #region 导入数据
        public ActionResult BatchAddLocation(int importType)
        {
            /*
             * importType 说明：  1：添加信息   2：编辑信息
             */
            int isSuccess = 0;
            string errIDs = "\r\n";
            try
            {
                if (Request.Files.Count >= 1 && Request.Files[0] != null)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string fileName = file.FileName;
                    string ext = fileName.Substring(fileName.LastIndexOf(".")).Split('.')[1].ToString();
                    if (!(ext.ToLower() == "xlsx" || ext.ToLower() == "xls"))
                        return Json(new { result = isSuccess, message = "只支持后缀名为xlsx或xls的文件格式" }, JsonRequestBehavior.AllowGet);
                    else if (file.ContentLength == 0)
                        return Json(new { result = isSuccess, message = "导入文件不能为空" }, JsonRequestBehavior.AllowGet);
                    else if (file.ContentLength > 20 * 1024 * 1024)
                        return Json(new { result = isSuccess, message = "文件不能大于20MB" }, JsonRequestBehavior.AllowGet);
                    UserInfo user = (UserInfo)Session["userInfo"];
                    string path = Server.MapPath("~/UserData/Location/");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    string extendName = fileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
                    string wholePath = Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + extendName);
                    file.SaveAs(wholePath);

                    /*
                     * 1、添加和编辑使用同一个页面，但使用不同的模版
                     * 2、在此处添加编辑的代码，尝试将下面的代码进行提取封装
                     */
                    DataTable dt = new DataTable();
                    try
                    {
                        dt = new ComNPOIExcel().ComXLSXOrXLSToTable(wholePath);
                    }
                    catch
                    {
                        return Json(new { result = 0, message = "请先下载模板" }, JsonRequestBehavior.AllowGet);
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        LocationDAL LocaDal = new LocationDAL();
                        PoliceDAL pdal = new PoliceDAL();
                        DataTable dtAreaAll = null;
                        Dictionary<int, DataTable> dicArea = new Dictionary<int, DataTable>();
                        int LocaType = ChangeValue.GetLocaTypeList().OrderBy(m => m.Key).Last().Key;
                        int count = 0;
                        int APCount = 0;
                        if (importType == 2)
                        {
                            if (!dt.Columns.Contains("ID"))
                            {
                                log.What = "批量修改场所失败，数据源列中缺少场所ID";
                                new OPLogDAL().InsertLog(log);
                                return Json(new { result = 0, message = "数据源列中缺少场所ID" }, JsonRequestBehavior.AllowGet);
                            }
                            if (!dt.Columns.Contains("LocaName"))
                            {
                                log.What = "批量修改场所失败，数据源列中缺少场所LocaName";
                                new OPLogDAL().InsertLog(log);
                                return Json(new { result = 0, message = "数据源列中缺少场所LocaName" }, JsonRequestBehavior.AllowGet);
                            }
                            if (!dt.Columns.Contains("Address"))
                            {
                                log.What = "批量修改场所失败，数据源列中缺少场所Address";
                                new OPLogDAL().InsertLog(log);
                                return Json(new { result = 0, message = "数据源列中缺少场所Address" }, JsonRequestBehavior.AllowGet);
                            }
                            if (!dt.Columns.Contains("UserName"))
                            {
                                log.What = "批量修改场所失败，数据源列中缺少场所UserName";
                                new OPLogDAL().InsertLog(log);
                                return Json(new { result = 0, message = "数据源列中缺少场所UserName" }, JsonRequestBehavior.AllowGet);
                            }
                            if (!dt.Columns.Contains("Mobile"))
                            {
                                log.What = "批量修改场所失败，数据源列中缺少场所Mobile";
                                new OPLogDAL().InsertLog(log);
                                return Json(new { result = 0, message = "数据源列中缺少场所Mobile" }, JsonRequestBehavior.AllowGet);
                            }
                            if ((dt.Columns.Contains("APID") && !dt.Columns.Contains("APName")) || (!dt.Columns.Contains("APID") && dt.Columns.Contains("APName")))
                            {
                                log.What = "批量修改场所失败，数据源列中有AP信息，但APID和AP名称必须同时存在";
                                new OPLogDAL().InsertLog(log);
                                return Json(new { result = 0, message = "数据源列中有AP信息，但APID和AP名称必须同时存在" }, JsonRequestBehavior.AllowGet);
                            }

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow dr = dt.Rows[i];
                                ImportLocation il = new ImportLocation();

                                if (string.IsNullOrEmpty(dr["ID"].ToString()))
                                {
                                    errIDs += (i + 2).ToString() + "--字段ID不能为空\r\n"; continue;
                                }
                                else if (!Regex.IsMatch(dr["ID"].ToString(), @"^[0-9]*$"))
                                {
                                    errIDs += (i + 2).ToString() + "--字段ID应该为正整数\r\n"; continue;
                                }
                                if (string.IsNullOrEmpty(dr["LocaName"].ToString()))
                                {
                                    errIDs += (i + 2).ToString() + "--字段LocaName不能为空\r\n"; continue;
                                }
                                if (string.IsNullOrEmpty(dr["Address"].ToString()))
                                {
                                    errIDs += (i + 2).ToString() + "--字段Address不能为空\r\n"; continue;
                                }
                                if (string.IsNullOrEmpty(dr["UserName"].ToString()))
                                {
                                    errIDs += (i + 2).ToString() + "--字段UserName不能为空\r\n"; continue;
                                }
                                if (string.IsNullOrEmpty(dr["Mobile"].ToString()))
                                {
                                    errIDs += (i + 2).ToString() + "--字段Mobile不能为空\r\n"; continue;
                                }

                                if (dt.Columns.Contains("Sid") && !string.IsNullOrEmpty(dr["Sid"].ToString()) && dr["Sid"].ToString() != "0")
                                {
                                    if (!Regex.IsMatch(dr["Sid"].ToString(), @"^[0-9]*$"))
                                    {
                                        errIDs += (i + 2).ToString() + "--字段Sid应该为正整数\r\n"; continue;
                                    }
                                    else
                                    {
                                        il.Sid = Convert.ToInt32(dr["Sid"]);
                                        if (dicArea.Keys.Contains(il.Sid)) dtAreaAll = dicArea.Where(m => m.Key == il.Sid).First().Value;
                                        else { dtAreaAll = new PoliceDAL().GetAreaAllIdBySid(il.Sid); dicArea.Add(il.Sid, dtAreaAll); }
                                        if (dtAreaAll == null || dtAreaAll.Rows.Count == 0)
                                        {
                                            errIDs += (i + 2).ToString() + "--该警区ID无效\r\n"; continue;
                                        }
                                        else
                                        {
                                            il.ProId = Convert.ToInt32(dtAreaAll.Rows[0]["ProID"]);
                                            il.CityId = Convert.ToInt32(dtAreaAll.Rows[0]["CityID"]);
                                            il.Aid = Convert.ToInt32(dtAreaAll.Rows[0]["AId"]);
                                            il.Pid = Convert.ToInt32(dtAreaAll.Rows[0]["PId"]);
                                        }
                                    }
                                }
                                else
                                    il.Sid = 0;
                                if (dt.Columns.Contains("LocaType") && !string.IsNullOrEmpty(dr["LocaType"].ToString()))
                                {
                                    if (!Regex.IsMatch(dr["LocaType"].ToString(), @"^[0-9]*$"))
                                    {
                                        errIDs += (i + 2).ToString() + "--字段LocaType应该为1-" + LocaType + "的数字\r\n"; continue;
                                    }
                                    else if (Convert.ToInt32(dr["LocaType"]) > LocaType)
                                    {
                                        errIDs += (i + 2).ToString() + "--字段LocaType应该为1-" + LocaType + "的数字\r\n"; continue;
                                    }
                                    else
                                        il.LocaType = Convert.ToInt32(dr["LocaType"]);
                                }
                                else
                                    il.LocaType = 0;
                                if (dt.Columns.Contains("Longitude") && !string.IsNullOrEmpty(dr["Longitude"].ToString()))
                                    il.LONGITUDE = dr["Longitude"].ToString();
                                if (dt.Columns.Contains("Latitude") && !string.IsNullOrEmpty(dr["Latitude"].ToString()))
                                    il.LATITUDE = dr["Latitude"].ToString();

                                il.verify = 0;
                                if (dt.Columns.Contains("IsVerify") && !string.IsNullOrEmpty(dr["IsVerify"].ToString()))
                                {
                                    string verify = dr["IsVerify"].ToString();
                                    if (verify == "通过")
                                        il.verify = 3;
                                    else if (verify == "不通过")
                                        il.verify = 4;
                                }
                                if (dt.Columns.Contains("APName") && !string.IsNullOrEmpty(dr["APName"].ToString()))
                                    il.ApName = dr["APName"].ToString();
                                if (dt.Columns.Contains("APID") && !string.IsNullOrEmpty(dr["APID"].ToString()))
                                {
                                    if (!Regex.IsMatch(dr["APID"].ToString(), @"^[0-9]*$"))
                                    {
                                        errIDs += (i + 2).ToString() + "--字段APID应该为正整数\r\n"; continue;
                                    }
                                    else if (dt.Columns.Contains("APID"))
                                        il.DveId = Convert.ToInt32(dr["APID"]);
                                }
                                else
                                    il.DveId = 0;
                                il.LocaName = dr["LocaName"].ToString();
                                il.Address = dr["Address"].ToString();
                                il.UserName = dr["UserName"].ToString();
                                il.Mobile = dr["Mobile"].ToString();
                                il.NetBarId = Convert.ToInt32(dr["ID"]);

                                int identityNet = LocaDal.UpdateLocaById(il);
                                if (identityNet > 0) count += 1;
                                if (il.DveId > 0 && !string.IsNullOrEmpty(il.ApName))
                                {
                                    int identityDev = LocaDal.UpdateDevById(il);
                                    if (identityDev > 0) APCount += 1;
                                }
                            }
                            if (errIDs.Length > 5)
                                errIDs = errIDs.Substring(1, errIDs.Length - 3);
                            isSuccess = 1;
                            log.What = "批量修改，场所成功" + count + "条，AP成功" + APCount + "条，失败信息：" + errIDs;
                            new OPLogDAL().InsertLog(log);
                            return Json(new { result = isSuccess, errIndexs = "批量修改，场所成功" + count + "条，AP成功" + APCount + "条。" + errIDs }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var linq_MacCount = (from c in dt.AsEnumerable() select new { DeviceMac = c["Mac"] }).Distinct().Count();

                            if (dt.Rows.Count != linq_MacCount)
                            {
                                log.What = "批量导入场所失败，设备MAC有数据重复";
                                new OPLogDAL().InsertLog(log);
                                return Json(new { result = 0, message = "数据源中“设备MAC”有数据重复" }, JsonRequestBehavior.AllowGet);
                            }
                            List<Json_DevInfo> importList = new List<Json_DevInfo>();
                            Dictionary<string, int> dicLoca = new Dictionary<string, int>();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow dr = dt.Rows[i];
                                if (string.IsNullOrEmpty(dr["LocaName"].ToString()))
                                {
                                    errIDs += (i + 2).ToString() + "--场所名称不能为空\r\n"; continue;
                                }
                                if (!Regex.IsMatch(dr["LocaType"].ToString(), @"^[0-9]*$"))
                                {
                                    errIDs += (i + 2).ToString() + "--字段LocaType应该为1-" + LocaType + "的数字\r\n"; continue;
                                }
                                else if (Convert.ToInt32(dr["LocaType"]) > LocaType)
                                {
                                    errIDs += (i + 2).ToString() + "--字段LocaType应该为1-" + LocaType + "的数字\r\n"; continue;
                                }
                                if (dt.Columns.Contains("Mobile") && !Regex.IsMatch(dr["Mobile"].ToString(), @"^\d{11}$"))
                                {
                                    errIDs += (i + 2).ToString() + "--手机号出错\r\n"; continue;
                                }
                                if (!Regex.IsMatch(dr["Sid"].ToString(), @"^[0-9]*$"))
                                {
                                    errIDs += (i + 2).ToString() + "--字段Sid应该为正整数\r\n"; continue;
                                }
                                string deviceMac = dr["Mac"].ToString().Replace(":", "-").Trim();
                                if (deviceMac.Length != 17)
                                { errIDs += (i + 2).ToString() + "--字段Mac应该为17位\r\n"; continue; }
                                else if (!Regex.IsMatch(deviceMac, @"^(?in)([\da-f]{2}(-|$)){6}$"))
                                { errIDs += (i + 2).ToString() + "--字段Mac格式有误\r\n"; continue; }
                                int mid = new MacDAL().GetDevIdByMac(deviceMac);
                                if (mid == -2) { errIDs += (i + 2).ToString() + "--该Mac已使用\r\n"; continue; }

                                if (!Regex.IsMatch(dr["CasesType"].ToString(), @"^([1-3]|99)$"))
                                {
                                    errIDs += (i + 2).ToString() + "--字段CasesType应该为1-3或者99的数字\r\n"; continue;
                                }
                                if (!Regex.IsMatch(dr["ProjectType"].ToString(), @"^([1-6]|99)$"))
                                {
                                    errIDs += (i + 2).ToString() + "--字段ProjectType应该为1-6或者99的数字\r\n"; continue;
                                }
                                if (dt.Columns.Contains("ApType") && !Regex.IsMatch(dr["ApType"].ToString(), @"^[1-2]$"))
                                {
                                    errIDs += (i + 2).ToString() + "--字段ApType应该为1-2数字\r\n"; continue;
                                }
                                ImportLocation il = new ImportLocation();
                                il.Sid = Convert.ToInt32(dr["Sid"]);
                                il.LocaName = dr["LocaName"].ToString();

                                if (dicArea.Keys.Contains(il.Sid)) dtAreaAll = dicArea.Where(m => m.Key == il.Sid).First().Value;
                                else { dtAreaAll = new PoliceDAL().GetAreaAllIdBySid(il.Sid); dicArea.Add(il.Sid, dtAreaAll); }
                                if (dtAreaAll == null || dtAreaAll.Rows.Count == 0)
                                {
                                    errIDs += (i + 2).ToString() + "--该警区ID无效\r\n"; continue;
                                }
                                if (dicLoca.Keys.Contains(il.LocaName)) il.NetBarId = dicLoca.Where(m => m.Key == il.LocaName).First().Value;

                                il.ProId = Convert.ToInt32(dtAreaAll.Rows[0]["ProID"]);
                                il.CityId = Convert.ToInt32(dtAreaAll.Rows[0]["CityID"]);
                                il.Aid = Convert.ToInt32(dtAreaAll.Rows[0]["AId"]);
                                il.Pid = Convert.ToInt32(dtAreaAll.Rows[0]["PId"]);
                                il.LocaType = Convert.ToInt32(dr["LocaType"]);
                                il.ProjectType = Convert.ToInt32(dr["ProjectType"]);
                                il.CasesType = Convert.ToInt32(dr["CasesType"]);
                                il.LATITUDE = dt.Columns.Contains("Latitude") ? dr["Latitude"].ToString() : "22.602074";
                                il.LONGITUDE = dt.Columns.Contains("Longitude") ? dr["Longitude"].ToString() : "114.011904";
                                il.Mac = deviceMac;
                                string QecurityName = dt.Columns.Contains("Security") ? dr["Security"].ToString() : "携网科技";
                                il.SECURITY_SOFTWARE_ORGCODE = (WebCommon.RefSecurityList().Where(m => m.SECURITY_SOFTWARE_ORGNAME == QecurityName).First().SECURITY_SOFTWARE_ORGCODE);
                                //il.SECURITY_SOFTWARE_ORGCODE = "746295626";
                                if (dt.Columns.Contains("Address")) il.Address = dr["Address"].ToString(); else il.Address = "预派单地址";
                                if (dt.Columns.Contains("UserName")) il.UserName = dr["UserName"].ToString(); else il.UserName = "李四";
                                if (dt.Columns.Contains("Mobile")) il.Mobile = dr["Mobile"].ToString(); else il.Mobile = "13888888888";
                                if (dt.Columns.Contains("ApName")) il.ApName = dr["ApName"].ToString(); else il.ApName = "预派单_" + il.Mac;
                                if (dt.Columns.Contains("ApType")) il.ApType = Convert.ToInt32(dr["ApType"]); else il.ApType = 1;
                                if (mid > 0) il.DveId = mid;
                                //il.Service_code = LocaDal.GetMaxServiceCode(il.Aid, il.LocaType, "");
                                //il.NETBAR_WACODE = ChangeValue.RefNetbarWacode(dtAreaAll.Rows[0]["PName"].ToString(), dtAreaAll.Rows[0]["City_code"].ToString(), il.LocaType, il.Service_code);
                                il.Service_code = 0;
                                il.NETBAR_WACODE = "";
                                il.QecurityName = QecurityName;
                                if (il.NetBarId == 0)
                                {
                                    il.NetBarId = LocaDal.ImportLoca(il);
                                    if (il.NetBarId > 0)
                                    {
                                        int devId = 0;
                                        if (il.DveId > 0) devId = LocaDal.updateImportDev(il); else devId = LocaDal.ImportDev(il);
                                        if (devId > 0)
                                            count += 1;
                                    }
                                }
                                else
                                {
                                    int devId = 0;
                                    if (il.DveId > 0) devId = LocaDal.updateImportDev(il); else devId = LocaDal.ImportDev(il);
                                    if (devId > 0)
                                        count += 1;
                                }
                                importList.Add(new Json_DevInfo { APName = il.ApName, MAC = il.Mac });
                                //将添加成功的场所ID加入到字典中
                                if (!dicLoca.Keys.Contains(il.LocaName)) dicLoca.Add(il.LocaName, il.NetBarId);
                            }
                            if (importList != null && importList.Count > 0)
                            {
                                List<int> listid = dicLoca.Values.ToList();
                                //更新导入场所的ap数量
                                LocaDal.UpdateLocaApNumbyLid(string.Join(",", listid));
                                JavaScriptSerializer ser = new JavaScriptSerializer();
                                new HttpHelps().PostSend(1, 1, ser.Serialize(importList));
                            }
                            if (errIDs.Length > 5)
                                errIDs = errIDs.Substring(1, errIDs.Length - 3);
                            isSuccess = 1;
                            log.What = "批量导入场所，成功" + count + "条，失败信息：" + errIDs;
                            new OPLogDAL().InsertLog(log);
                            return Json(new { result = isSuccess, errIndexs = "批量导入场所，成功" + count + "条。" + errIDs }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                        return Json(new { result = isSuccess, message = "文件中不存在导入的数据" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { result = isSuccess, message = "请选择要导入的文件" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.BatchAddLocation()[HttpPost]"}
                });
                return Json(new { result = isSuccess, message = "系统错误，请稍后重试" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region  下载模版
        public ActionResult DownloadTemplate()
        {
            try
            {
                DownFile("场所导入模版.xlsx", "Template/MergeWar_ImprotLoca.xlsx");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LoginController.Download2()[HttpGet]"}
                });
                return Content("<script>alert('下载过程出现问题，请稍后重试')</script>");
            }
            return View();
        }
        void DownFile(string name, string path)
        {
            FileInfo file = new FileInfo(Server.MapPath("~/") + path);
            Response.Clear();
            Response.ClearHeaders();
            Response.Buffer = false;
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("" + name, System.Text.Encoding.UTF8).Replace("+", "%20"));
            Response.AppendHeader("Content-Length", file.Length.ToString());
            Response.WriteFile(file.FullName);
            Response.Flush();
            Response.End();
        }
        #endregion

        #region 首页方法
        /// <summary>
        ///创建人：邓佳训
        /// 创建时间：2017-06-15
        /// 首页近期采集数量趋势图统计
        /// </summary>
        /// <param name="TimeType">1按月统计 2按周统计</param>
        /// <returns></returns>
        public ActionResult DataCountTrend(string TimeType)
        {
            try
            {
                DataTable dt = GetCollectCountByType(TimeType, "1");
                JsonResult jsondata = new JsonResult();
                jsondata.Data = RrsJson(dt);
                jsondata.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsondata;
            }
            catch (SqlException sqlex)
            {
                Logger.ErrorLog(sqlex, null);
                throw;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                throw;
            }
        }
        /// <summary>
        ///创建人：邓佳训
        /// 创建时间：2017-06-16
        /// 首页近期设备数量趋势图统计
        /// </summary>
        /// <param name="TimeType">1按月统计 2按周统计</param>
        /// <returns></returns>
        public ActionResult DevCountTrend(string TimeType)
        {
            try
            {
                DataTable dt = GetCollectCountByType(TimeType, "2");
                JsonResult jsondata = new JsonResult();
                jsondata.Data = RrsJson(dt);
                jsondata.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsondata;
            }
            catch (SqlException sqlex)
            {
                Logger.ErrorLog(sqlex, null);
                throw;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                throw;
            }
        }

        public DataTable GetCollectCountByType(string TimeType, string SumType)
        {
            DateTime timenow = DateTime.Now;
            TimeType = string.IsNullOrEmpty(TimeType) ? "2" : TimeType;
            TimeType = TimeType == "1" ? timenow.AddDays(-30).ToString("yyyyMMdd") : timenow.AddDays(-7).ToString("yyyyMMdd");
            DataTable dt = home.DataCountTrendDal(TimeType, SumType);
            return dt;
        }
        /// <summary>
        /// 创建人：张团勃
        /// 创建时间：2017-06-16
        /// 设备数量，在线数量，在线率
        /// </summary>
        /// <returns></returns>
        public ActionResult DevNum()
        {
            JsonResult jsondata = new JsonResult();
            try
            {
                DataTable dt = home.DataOnline();
                double Online1 = string.IsNullOrEmpty(dt.Rows[0]["DataNum"].ToString()) ? 0.00 : Convert.ToDouble(dt.Rows[0]["DataNum"].ToString());
                double Online2 = string.IsNullOrEmpty(dt.Rows[0]["ZoomNum"].ToString()) ? 0.00 : Convert.ToDouble(dt.Rows[0]["ZoomNum"].ToString());
                string Online = (((Online1 / Online2) * 100).ToString("f2"));

                var num = new
                {
                    dn = dt.Rows[0]["DataNum"],
                    zn = dt.Rows[0]["ZoomNum"],
                    ol = Online
                };
                jsondata.Data = num;
                jsondata.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsondata;
            }
            catch (Exception e)
            {
                Logger.ErrorLog(e, null);
                jsondata.Data = "err";
                return jsondata;
            }
        }
        /// <summary>
        /// 创建人：张团勃
        /// 创建时间：2017-06-16
        /// 地图下方数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MapInferior()
        {
            JsonResult jsondata = new JsonResult();
            try
            {
                DataTable dt = home.DataMapInferior();
                List<Index_Json> jsonlist = new List<Index_Json>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Index_Json json = new Index_Json();
                    json.Mobile = dt.Rows[i]["Mobile"].ToString();
                    json.NETWORK_APP = ChangeValue.NetworkServe(dt.Rows[i]["NETWORK_APP"].ToString());
                    json.IdValue = dt.Rows[i]["IdValue"].ToString();
                    json.PLACE_NAME = dt.Rows[i]["PLACE_NAME"].ToString();
                    jsonlist.Add(json);
                }
                jsondata.Data = jsonlist;
                jsondata.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsondata;
            }
            catch (Exception e)
            {
                Logger.ErrorLog(e, null);
                jsondata.Data = "err";
                return jsondata;
            }
        }

        /// <summary>
        /// 创建人：韩希鹏
        /// 创建时间：2017-06-15
        /// 场所类型在线数量
        /// </summary>
        /// <returns></returns>
        public ActionResult DataOnlineCount()
        {
            JsonResult jsondata = new JsonResult();
            //Index_Json json = new Index_Json();
            try
            {
                DataTable dt = home.DataOnlineCount();

                string[] name = new string[dt.Rows.Count];
                int[] datanum = new int[dt.Rows.Count];
                if (dt.Rows.Count != 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        name[i] = ChangeValue.ChangeLevelWord(dt.Rows[i][0].ToString());
                        datanum[i] = Convert.ToInt32(dt.Rows[i][1].ToString());
                    }
                }
                else
                {
                    for (int i = 0; i < name.Length; i++)
                    {
                        name[i] = "暂无数据";
                        datanum[i] = 0;
                    }
                }
                json.namenum = name;
                json.datanum = datanum;
                jsondata.Data = json;
                jsondata.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsondata;

            }
            catch (Exception e)
            {
                Logger.ErrorLog(e, null);
                jsondata.Data = "err";
                return jsondata;
            }
        }
        /// <summary>
        /// 创建人：韩希鹏
        /// 创建时间：2017-06-15
        /// 场所类型在线率
        /// </summary>
        /// <returns></returns>
        public ActionResult DataOnlineRate()
        {
            JsonResult jsondata = new JsonResult();
            //Index_Json json = new Index_Json();
            try
            {
                DataTable dt = new DataTable();
                dt = home.DataOnlineRate();//获取数据
                dt.Columns.Add("Rate", typeof(string));//在线率
                string[] name = new string[dt.Rows.Count];//场所名称
                string[] datanum = new string[dt.Rows.Count];//场所在线率
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //data = (Convert.ToDouble(dt.Rows[i][2]) / Convert.ToDouble(dt.Rows[i][1])) * 100;
                    dt.Rows[i][3] = (Convert.ToDouble(dt.Rows[i][2]) / Convert.ToDouble(dt.Rows[i][1])).ToString("f2");
                    name[i] = ChangeValue.ChangeLevelWord(dt.Rows[i][0].ToString());
                    datanum[i] = dt.Rows[i][3].ToString();
                }
                dt.DefaultView.Sort = "Rate DESC";//从大到小排序
                dt = dt.DefaultView.ToTable();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    name[i] = ChangeValue.ChangeLevelWord(dt.Rows[i][0].ToString());
                    datanum[i] = dt.Rows[i][3].ToString();
                }
                json.namenum = name;//场所名称 
                json.ratenum = datanum;//场所在线率
                jsondata.Data = json;
                jsondata.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsondata;
            }
            catch (Exception e)
            {
                Logger.ErrorLog(e, null);
                jsondata.Data = "err";
                return jsondata;
            }
        }
        /// <summary>
        ///创建人：邓佳训 
        ///创建时间：2017-06-16
        /// 组装返回数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public Index_Json RrsJson(DataTable dt)
        {
            Index_Json Calssjson = new Index_Json(dt.Rows.Count);
            for (int a = 0; a < dt.Rows.Count; a++)
            {
                Calssjson.times[a] += dt.Rows[a][0].ToString();
                Calssjson.phone[a] += Convert.ToInt64(dt.Rows[a][1]);
                Calssjson.mac[a] += Convert.ToInt64(dt.Rows[a][2]);
                Calssjson.virtuals[a] += Convert.ToInt64(dt.Rows[a][3]);
                Calssjson.imei[a] += Convert.ToInt64(dt.Rows[a][4]);
                Calssjson.carNum[a] += Convert.ToInt64(dt.Rows[a][5]);
            }
            return Calssjson;
        }
        /// <summary>
        /// 创建人：邓佳训
        /// 创建时间：2017-06-14
        /// 首页数据统计 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DataConunt()
        {
            JsonResult jsonres = new JsonResult();
            try
            {
                DataTable dt = home.DataCount();
                if (dt.Select("name='30'").Length == 0)
                    dt.Rows.Add("30", "0");
                if (dt.Select("name='31'").Length == 0)
                    dt.Rows.Add("31", "0");
                if (dt.Select("name='32'").Length == 0)
                    dt.Rows.Add("32", "0");
                if (dt.Select("name='33'").Length == 0)
                    dt.Rows.Add("33", "0");
                if (dt.Select("name='34'").Length == 0)
                    dt.Rows.Add("34", "0");
                List<Index_Json> jsonlist = new List<Index_Json>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Index_Json json = new Index_Json();
                    json.name = ChangeValue.ChangeLevelWord(dt.Rows[i]["name"].ToString());
                    json.data = dt.Rows[i]["data"].ToString();
                    jsonlist.Add(json);
                }
                jsonres.Data = jsonlist;
                jsonres.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonres;
            }
            catch (SqlException sqlex)
            {
                Logger.ErrorLog(sqlex, null);
                jsonres.Data = "err";
                return jsonres;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                jsonres.Data = "err";
                return jsonres;
            }
        }
        #endregion

        #region 一键查询方法

        /// <summary>
        /// 数据查询结果
        /// </summary>
        /// <param name="RadVal">主类别：1实名、2硬件、3虚拟</param>
        /// <param name="Id">关键字</param>
        /// <param name="key">子类别：1实名信息、2上下线信息、3虚拟身份、4硬件特征</param>
        /// <param name="pageIndex">当前页</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ResultList(string RadVal, string Id, string key, int? pageIndex = 1)
        {
            try
            {
                int[] userType = ChangeValue.GetUserType();
                if (Id != "undefined")
                {
                    switch (RadVal)
                    {
                        case "1":
                            ViewBag.Val = "实名";
                            switch (key)
                            {
                                case "1":

                                    break;
                                case "2":

                                    break;
                                case "3":

                                    break;
                                case "4":

                                    break;
                            }
                            break;
                        case "2":
                            ViewBag.Val = "硬件";
                            switch (key)
                            {
                                case "1":

                                    break;
                                case "2":

                                    break;
                                case "3":

                                    break;
                                case "4":

                                    break;
                            }
                            break;
                        case "3":
                            ViewBag.Val = "虚拟";
                            switch (key)
                            {
                                case "1":

                                    break;
                                case "2":

                                    break;
                                case "3":

                                    break;
                                case "4":

                                    break;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
            }
            return View();
        }
        #endregion

        #region 场所管理中所用
        [HttpGet]
        public ActionResult LoadItude()
        {
            return View();
        }
        /// <summary>
        /// 查询mac地址匹配的集合
        /// </summary>
        /// <param name="val"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RefMacList(string Id)
        {
            try
            {
                List<MacInfo> list = WebCommon.GetMacList(Id);
                List<string> sl = new List<string>();
                string message = "[";
                if (list != null && list.Count > 0)
                {
                    foreach (MacInfo item in list)
                    {
                        sl.Add("'" + item.COLLECTION_EQUIPMENT_ID + "'");
                    }
                    message += string.Join(",", sl);
                }
                message += "]";
                return Content(message);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.RefMacList(string Id)[HttpGet]"}
                });
                return Content("{\"err\":\"系统错误，请稍后重试\"}");
            }
        }

        [HttpGet]
        public ActionResult CKMac(string Id)
        {
            try
            {

                List<MacInfo> list = WebCommon.GetMacList(Id);
                if (list != null && list.Count > 0)
                {
                    IEnumerable<MacInfo> flist = list;
                    flist = flist.Where(c => c.COLLECTION_EQUIPMENT_ID == Id);
                    list = flist.ToList();
                    return Content("{\"result\":\"" + list.First().AP_MAC + "\",\"Id\":" + list.First().ID + "}");
                }
                else
                    return Content("{\"result\":\"\"}");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.CKMac(string Id)[HttpGet]"}
                });
                return Content("{\"err\":\"系统错误，请稍后重试\"}");
            }
        }
        [HttpGet]
        public ActionResult AreaValue(int Id, string key)
        {
            try
            {
                MacDAL macDal = new MacDAL();
                UserInfo user = (UserInfo)Session["userInfo"];
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                string strJson = "";
                if (key == " Province")
                {
                    List<AreaInfo> list = new List<AreaInfo>();
                    //只能在自己省下面创建用户
                    if ((user.Type == 8 || user.Type == 6 || user.Type == 4 || user.Type == 2 || user.Type == 3) && user.ProID != 0)
                    {
                        AreaInfo area = new PoliceDAL().GetAreaInfoById(user.ProID, 2);
                        area.Name = area.Area;
                        list.Add(area);
                    }
                    else
                        list = macDal.GetAreaInfoList(0, 2);
                    strJson = ser.Serialize(list);
                }
                else if (key == " City")
                {
                    List<AreaInfo> list = new List<AreaInfo>();
                    if ((user.Type == 8 || user.Type == 6 || user.Type == 4 || user.Type == 2 || user.Type == 3) && user.CityID != 0)
                    {
                        AreaInfo area = new PoliceDAL().GetAreaInfoById(user.CityID, 1);
                        area.Name = area.Area;
                        list.Add(area);
                    }
                    else
                        list = macDal.GetAreaInfoList(Id, 1);
                    strJson = ser.Serialize(list);
                }
                else if (key == " Area")
                {
                    List<AreaInfo> list = new List<AreaInfo>();
                    if ((user.Type == 8 || user.Type == 6 || user.Type == 4 || user.Type == 2 || user.Type == 3) && user.AId != 0)
                    {
                        AreaInfo area = new PoliceDAL().GetAreaInfoById(user.AId, 0);
                        area.Name = area.Area;
                        list.Add(area);
                    }
                    else
                        list = macDal.GetAreaInfoList(Id, 0);
                    strJson = ser.Serialize(list);
                }
                else if (key == " police")
                {
                    List<PoliceInfo> list = new List<PoliceInfo>();
                    if ((user.Type == 6 || user.Type == 4 || user.Type == 2 || user.Type == 3) && user.PId != 0)
                    {
                        PoliceInfo pinfo = new PoliceDAL().GetPoliceInfo("", user.PId);
                        pinfo.Name = pinfo.Name;
                        list.Add(pinfo);
                    }
                    else
                        list = macDal.GetPoliceList(Id);
                    strJson = ser.Serialize(list);
                }
                else if (key == " Scenic")
                {
                    List<ScenicInfo> list = macDal.GetScenicList(Id);
                    strJson = ser.Serialize(list);
                }
                else if (key == " Location")
                {
                    //根据派出所ID获取场所集合信息
                    List<SelMacInfo> list = new LocationDAL().GetLocaListByPid(Id);
                    strJson = ser.Serialize(list);
                }
                else if (key == " DevAp")
                {
                    List<SelMacInfo> list = macDal.GetListYYMac(Id);

                    strJson = ser.Serialize(list);
                }
                return Content(strJson);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.AreaValue(string key, int Id)[HttpGet]"}
                });
                return Content("{\"err\":\"获取数据出错，请稍后重试\"}");
            }
        }
        /// <summary>
        /// 证件类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShowCertifi()
        {
            return View();
        }
        [HttpGet]
        public ActionResult RefLocaMapVal(string ProId, string CityId, string Aid)
        {
            string mapJs = "[]";
            string jsAlter = "";
            string centerGnote = "114.031716,22.545583";
            UserInfo user = (UserInfo)Session["userinfo"];
            if (user != null && (user.Type == 4 || user.Type == 2 || user.Type == 3 || user.Type == 5 || user.Type == 6 || user.Type == 8))
            {
                ProId = user.ProID.ToString();
                CityId = user.CityID.ToString();
                //Aid = user.Type == 2 ? user.AId.ToString() : Aid;
                Aid = user.AId.ToString();
            }
            else
            {
                ProId = ProId == "0" || ProId == "undefined" ? "0" : ProId;
                CityId = CityId == "0" || CityId == "undefined" ? "0" : CityId;
                Aid = Aid == "0" || Aid == "undefined" ? "0" : Aid;
            }
            try
            {
                ProId = "5";
                CityId = "6";
                Aid = "32";
                //为了测试方便。先加载深圳市南山的场所
                DataTable dt = new LocationDAL().GetLocaMapDt(ProId, CityId, Aid, "1");
                List<Json_LocaMap> listMap = new List<Json_LocaMap>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    centerGnote = dt.Rows[0]["Gnote"].ToString();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Json_LocaMap jlm = new Json_LocaMap();
                        jlm.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                        jlm.statis = Convert.ToInt32(dt.Rows[i]["LocaStatis"]);
                        jlm.loadMessage = "场所名称：" + dt.Rows[i]["PLACE_NAME"].ToString() + "<br>场所状态：" + RefLocaStatisHtml(jlm.statis) + "<br>采集设备数量：" + dt.Rows[i]["APNum"];
                        string tempGnote = dt.Rows[i]["Gnote"].ToString();
                        jlm.lng = tempGnote.Split(',')[0];
                        jlm.lat = tempGnote.Split(',')[1];
                        listMap.Add(jlm);
                    }
                    if (listMap != null && listMap.Count > 0)
                    {
                        mapJs = Newtonsoft.Json.JsonConvert.SerializeObject(listMap);
                    }
                }
                else
                    jsAlter = "null";
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.LocaShowMap(string ProId, string CityId, string Aid)"}
                });
                jsAlter = "err";
            }
            return Content("{\"jsAlter\":\"" + jsAlter + "\",\"mapJs\":" + mapJs + ",\"Lng\":" + centerGnote.Split(',')[0] + ",\"Lat\":" + centerGnote.Split(',')[1] + "}");
        }

        [HttpGet]
        public ActionResult IdMapVal(int nbId)
        {
            string mapJs = "";
            try
            {
                //string message = "{\"nbId\":\"" + nbId + "\"}";
                //JavaModel jm = new JavaModel();
                //try
                //{
                //    jm = new SolrModel().RefDealJavaModel(13, message);
                //}
                //catch (Exception x)
                //{
                //    Logger.ErrorLog(x, new Dictionary<string, string>()
                //    {
                //        {"Function","LocationController.SolrModel().RefDealJavaModel(1, message)"+"_____"+message}
                //    });
                //}
                //if (jm.LMlist.Count > 0)
                //{
                //    mapJs = "<br>已认证数量：" + jm.LMlist.First().AUTH_ACCOUNTNum + "<br>未认证数量：" + jm.LMlist.First().AUTH_ACCOUNTNotNum + "<br>上下线数量：" + jm.LMlist.First().AuditNum + "<br>上网日志数量：" + jm.LMlist.First().HttpNum + "<br>终端MAC数量：" + jm.LMlist.First().MacNum + "<br>被采集热点数量：" + jm.LMlist.First().APMacNum;
                //}
                //else { mapJs = "<br>已认证数量：" + 0 + "<br>未认证数量：" + 0 + "<br>上下线数量：" + 0 + "<br>上网日志数量：" + 0 + "<br>终端MAC数量：" + 0 + "<br>被采集热点数量：" + 0; }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.IdMapVal(int bbId)"}
                });
            }
            return Content("{\"mapJs\":\"" + mapJs + "\"}");
        }

        public string RefLocaStatisHtml(object statis)
        {
            if (statis == DBNull.Value || Convert.ToInt32(statis) == 0)
                return "<font color='red'>离线</font>";
            else
                return "<font color='green'>在线</font>";
        }
        #endregion

        #region 系统数据查询方法
        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="id">厂商Id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="dataType">数据类型</param>
        /// <param name="authType">上网日志数据类别 1、虚拟身份，2、HTTP协议</param>
        /// <returns></returns>
        //public ActionResult GetDataNum(string id, string startTime, string endTime, string dataType, string authType, int pageIndex = 1)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(id))
        //            Session["factoryId"] = id;
        //        id = string.IsNullOrEmpty(id) ? Session["factoryId"].ToString():id;
        //        SolrShowModel solrmodel = new SolrShowModel();
        //        var javaResJson = string.Empty;
        //        dataType = "3";
        //        if (!string.IsNullOrEmpty(dataType))
        //        {
        //            dataType = ChangeValue.RefauthTypeStr(dataType);
        //            MergeWar.JavaServer.SCPInquireServiceClient java = new MergeWar.JavaServer.SCPInquireServiceClient();
        //            var parameter = new
        //            {
        //                curPage = pageIndex - 1,
        //                factoryId = 1,
        //                startTime = startTime,
        //                endTime = endTime,
        //                dataType = dataType,
        //                networkApp = authType
        //            };
        //            try
        //            {
        //                javaResJson = Newtonsoft.Json.JsonConvert.SerializeObject(parameter);
        //                javaResJson = java.queryTerminalManageList(javaResJson);
        //                var Json = Newtonsoft.Json.Linq.JObject.Parse(javaResJson);
        //                if (Convert.ToInt32(Json["state"]) == 0)
        //                {
        //                    Logger.writeLog(Json.ToString());
        //                    return Content("<script>alert('暂无数据可查!');</script>");
        //                }
        //                solrmodel.numFound = Convert.ToInt32(Json["numFound"].ToString());
        //                switch (dataType)
        //                {
        //                    case "AuditLog":
        //                        solrmodel._taLog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TermianlAccessLog>>(Json["terminalList"].ToString());
        //                        ViewBag.pl = new PagedList<TermianlAccessLog>(solrmodel._taLog, pageIndex, 10, solrmodel.numFound);
        //                        break;
        //                    case "NetLog":
        //                        solrmodel._tnkLog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TermianlNetworkLog>>(Json["terminalList"].ToString());
        //                        ViewBag.pl = new PagedList<TermianlNetworkLog>(solrmodel._tnkLog, pageIndex, 10, solrmodel.numFound);
        //                        break;
        //                    case "Termianl":
        //                        solrmodel._ftInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Fench_TerminalInfo>>(Json["terminalList"].ToString());
        //                        ViewBag.pl = new PagedList<Fench_TerminalInfo>(solrmodel._ftInfo, pageIndex, 10, solrmodel.numFound);
        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //            catch (Exception solr)
        //            {
        //                Logger.ErrorLog(solr, null);
        //                return Content("<script>alert('服务器开小差啦!');</script>");
        //            }
        //        }
        //        ViewBag.dataType = dataType;
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.ErrorLog(ex, null);
        //        throw;
        //    }
        //}
        public ActionResult GetFirmName()
        {
            try
            {
                List<SecurityOrg> list = new SystemDAL().GetFirmNameDal();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        /// <summary>
        /// 加载地图
        /// </summary>
        /// <returns></returns>
        public ActionResult Generate()
        {
            try
            {
                var entity = new { ID = 0, LONGITDE = "", LATITUDE = "", APName = "", NETBAR_ID = "" };
                DataTable dt = new DataTable();
                string ProId = "0", CityId = "0", Aid = "0";
                UserInfo user = (UserInfo)Session["userInfo"];
                if (user != null && (user.Type == 4 || user.Type == 2 || user.Type == 3 || user.Type == 5 || user.Type == 6 || user.Type == 8))
                {
                    ProId = user.ProID.ToString();
                    CityId = user.CityID.ToString();
                    //Aid = user.Type == 2 ? user.AId.ToString() : Aid;
                    Aid = user.AId.ToString();
                }
                JsonResult result = new JsonResult();
                //result.Data = Enumerable.Range(1, 100000);
                dt = new LocationDAL().GetLocaMapDt(ProId, CityId, Aid, "2");
                DataRow[] drArr = dt.Select(" J NOT IN(NULL) and W NOT IN (NULL) AND J NOT IN('') and W NOT IN ('') AND J NOT IN(0.00) and W NOT IN (0.00) AND J NOT IN(0.000000) and W NOT IN (0.000000) ");
                DataTable dtNew = dt.Clone();
                for (int i = 0; i < drArr.Length; i++)
                {
                    dtNew.Rows.Add(drArr[i].ItemArray);  // 将DataRow添加到DataTable中
                }
                TempData["dtlength"] = dtNew.Rows.Count;
                var list = Newtonsoft.Json.JsonConvert.SerializeObject(dtNew);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception EX)
            {

                throw;
            }

        }
        /// <summary>
        /// 设置最大maxJsonLength值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="contentEncoding"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
        /// <summary>
        /// 碰撞分析出现次数显示
        /// </summary>
        /// <returns></returns>
        public ActionResult AnalysisDet(string analyType, string mac)
        {
            UserInfo dbUser = (UserInfo)Session["userinfo"];
            try
            {
                string Seltype = analyType == "1" ? "pz" : "bs";
                string relList = string.Empty;
                var argjson = "";
                try
                {
                    java.InnerChannel.OperationTimeout = new TimeSpan(0, 10, 0);
                    if (analyType == "1")
                    {
                        var arg = new
                        {
                            queryMac = mac,
                            devIds = HttpContext.Cache[Seltype + "ids" + dbUser.ID],
                            startTime = HttpContext.Cache[Seltype + "txtStartTimes" + dbUser.ID],
                            endTime = HttpContext.Cache[Seltype + "txtEndTimes" + dbUser.ID],
                        };
                        argjson = JsonConvert.SerializeObject(arg);
                        argjson = java.collisionAnalysisDetailMAC(argjson);
                        relList = "collisionAnalysisDetailMACResult";
                    }
                    else
                    {
                        var arg = new
                        {
                            queryMac = mac,
                            devIds = HttpContext.Cache[Seltype + "ids" + dbUser.ID],
                            startTime = HttpContext.Cache[Seltype + "txtStartTimes" + dbUser.ID],
                            endTime = HttpContext.Cache[Seltype + "txtEndTimes" + dbUser.ID],
                            mac = HttpContext.Cache[Seltype + "keys" + dbUser.ID],
                        };
                        argjson = JsonConvert.SerializeObject(arg);
                        argjson = java.accompanyAnalysisDetailMAC(argjson);
                        relList = "accompanyAnalysisDetailMACResult";
                    }

                    var Json = Newtonsoft.Json.Linq.JObject.Parse(argjson);
                    if (Json["state"].ToString() == "1")
                    {
                        return Content("<script>alert('暂无数据!');history.go(-1);</script>");
                    }
                    else
                    {
                        solrmodel._cl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Collision>>(Json[relList].ToString());
                        ////获取场所缓存
                        if ((List<Loc_NetBarInfo>)System.Web.HttpContext.Current.Cache["ShowCacheLoca"] == null)
                            WebCommon.GetCacheLoca();
                        List<Loc_NetBarInfo> netlist = (List<Loc_NetBarInfo>)System.Web.HttpContext.Current.Cache["ShowCacheLoca"];
                        Logger.writeLog("缓存场所的个数：" + netlist.Count());
                        //获取设备缓存
                        if ((List<Loc_DevInfo>)System.Web.HttpContext.Current.Cache["CacheMacAll"] == null)
                            WebCommon.GetCacheMacAllList();
                        List<Loc_DevInfo> delist = (List<Loc_DevInfo>)System.Web.HttpContext.Current.Cache["CacheMacAll"];
                        Logger.writeLog("缓存设备的个数：" + delist.Count());
                        for (int i = 0; i < solrmodel._cl.Count; i++)
                        {
                            Loc_DevInfo dev = delist.Find(a => a.ID.Equals(solrmodel._cl[i].DevAP_ID));
                            if (dev != null)
                            {
                                Loc_NetBarInfo loc = netlist.Find(a => a.ID == (dev.NETBAR_ID));
                                if (loc != null)
                                {
                                    solrmodel._cl[i].PLACE_NAME = loc.PLACE_NAME;
                                    solrmodel._cl[i].APName = dev.APName;
                                }
                            }
                        }
                        ViewBag.pl = new PagedList<Collision>(solrmodel._cl, 1, Convert.ToInt32(PageSize), 10);
                    }
                }
                catch (Exception solr)
                {
                    Logger.ErrorLog(solr, null);
                    return Content("<script>alert('网络错误!');history.go(-1);</script>");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return View();
        }


        public ActionResult Demo()
        {
            JsonResult result = new JsonResult();
            List<Collision> clList = new MergeWar.AppCode.JavaServerAction().GetJavaData("", "", "", "", "1");
            result.Data = clList;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        #region 一键查询数据处理
        #region 实名/上下线
        /// <summary>
        /// 实名/上下线
        /// </summary>
        /// <param name="keyWord">手机号/MAC</param>
        /// tabType:1实名、2虚拟身份、3硬件、4上下线
        /// dataType 区分参数是mac还是手机号以及分页
        /// <returns></returns>
        public ActionResult TableShow(string dataType, string tabType, string keyWord, int pageIndex = 1)
        {
            try
            {
                ViewBag.tabType = tabType;
                if (!keyWord.Equals("数据正在加载……"))
                {
                    List<DatasInfo> list = null;
                    string result = string.Empty;
                    int[] userType = ChangeValue.GetUserType();
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    if (tabType == "1" || tabType == "4")
                        dic.Add("pageIndex", pageIndex.ToString());
                    dic.Add("authAccountOrMac", keyWord);
                    try
                    {
                        switch (tabType)
                        {
                            case "1"://实名

                                result = java.queryRealname(Newtonsoft.Json.JsonConvert.SerializeObject(dic));
                                var real = JObject.Parse(result);
                                if (real["state"].ToString() == "0")
                                {
                                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DatasInfo>>(JObject.Parse(result)["realnameResult"].ToString());
                                    list = new PagedList<DatasInfo>(list, pageIndex, Convert.ToInt32(PageSize), Convert.ToInt32(real["totalNumber"].ToString()));
                                }
                                ViewBag.pl = list;
                                break;
                            case "2"://虚拟身份
                                dic.Add("proID", string.IsNullOrEmpty(RefFilterUserAreaValue0(6)) ? "0" : RefFilterUserAreaValue0(6));//省
                                dic.Add("cityID", string.IsNullOrEmpty(RefFilterUserAreaValue0(4)) ? "0" : RefFilterUserAreaValue0(4));//市
                                dic.Add("aid", string.IsNullOrEmpty(RefFilterUserAreaValue0(2)) ? "0" : RefFilterUserAreaValue0(2));//区
                                result = java.queryVirtualIdentDetail(Newtonsoft.Json.JsonConvert.SerializeObject(dic));
                                var virtualval = JObject.Parse(result);
                                if (virtualval["state"].ToString() == "0")
                                {
                                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DatasInfo>>(JObject.Parse(result)["virtualIdentDetailResult"].ToString());
                                    ViewBag.VidDic = new ChangeValue().RefVidDetailDic(list);
                                }
                                ViewBag.pl = list;
                                break;
                            case "3"://硬件特征
                                result = java.queryHardware(Newtonsoft.Json.JsonConvert.SerializeObject(dic));
                                var mac = JObject.Parse(result);
                                if (mac["state"].ToString() == "0")
                                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DatasInfo>>(JObject.Parse(result)["hardwareResult"].ToString());
                                ViewBag.pl = list;
                                break;
                            default:
                                List<TermianlAccessLog> talist = null;
                                dic.Add("UserType", userType[0].ToString());
                                dic.Add("AreaId", userType[1].ToString());
                                result = java.queryTermianlAccessLogList(Newtonsoft.Json.JsonConvert.SerializeObject(dic));
                                var logval = JObject.Parse(result);
                                if (logval["state"].ToString() == "0")
                                {
                                    talist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TermianlAccessLog>>(JObject.Parse(result)["termianlAccessLogListResult"].ToString());
                                    var jsonval = Newtonsoft.Json.Linq.JObject.Parse(result);
                                    talist = new PagedList<TermianlAccessLog>(talist, pageIndex, Convert.ToInt32(PageSize), Convert.ToInt32(jsonval["totalNumber"].ToString()));
                                }
                                ViewBag.pl = talist;
                                break;
                        }
                        Logger.writeLog("-----------------一键查询返回结果正常：" + result + "------------请求参数类型：" + tabType);
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, null);
                        Logger.writeLog("-----------------一键查询返回结果错误：" + result + "------------请求参数类型：" + tabType);
                        ViewBag.errscript = "alert('请求超时，请稍后再试')";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }
        #endregion
        #region 上下线信息
        [HttpGet]
        public ActionResult OninfoDetail(string ID)
        {
            try
            {
                string jsonval = "{ \"ID\":" + ID + "}";
                string relust = java.queryTermianlAccessLogDetail(jsonval);
                var logjson = JObject.Parse(relust);
                if (logjson["state"].ToString() == "0")
                {
                    solrmodel._taLog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TermianlAccessLog>>(logjson["termianlAccessLogDetailResult"].ToString());
                    ViewBag.talog = solrmodel._taLog.First();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                return Content("<script> alert('服务器异常'); history.go(-1);</ script > ");
                throw;
            }
            return View();
        }
        #endregion
        //#region 硬件特征/虚拟身份
        //public ActionResult HardwareDetail(string dataType, string tabType, string keyWord)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.ErrorLog(ex, null);
        //        ViewBag.errscript = "alert('未知错误，请联系管理员')";
        //    }
        //    return View();
        //}

        //#endregion
        //#region 虚拟身份
        //public ActionResult TipVidDetail()
        //{
        //    return View();
        //}
        //#endregion
        #endregion
        public string RefFilterUserAreaValue0(int Type)
        {
            int[] userType = ChangeValue.GetUserType();
            return ((userType[0] == Type || userType[0] == 8) && userType[1] != 0) ? userType[1].ToString() : null;
        }

        public ActionResult RefAddressToVal(string Address)
        {
            string returnVal = "";
            if (!string.IsNullOrEmpty(Address))
            {
                WebRequest myrequest = WebRequest.Create("http://www.baidu.com");
                WebResponse myres = null;
                bool isWeb = false;
                try
                {
                    myres = myrequest.GetResponse();
                    myres.Close();
                    isWeb = true;
                }
                catch (Exception)
                {
                    returnVal = "{\"status\":1,\"message\":\"无法连接网络，请手动确定经纬度。\"}";
                }
                if (string.IsNullOrEmpty(returnVal))
                {
                    string result = "";
                    if (isWeb)
                    {
                        result = ChangeValue.SendRequest("http://api.map.baidu.com/geocoder/v2/?address=" + HttpUtility.UrlEncode(Address) + "&output=json&ak=" + ConfigurationManager.AppSettings["baiduAk"]);
                    }
                    if (result == "-1")
                        returnVal = "{\"status\":1,\"message\":\"获取经纬度失败，请稍后重试。\"}";
                    else
                    {
                        JObject str = (JObject)JsonConvert.DeserializeObject(result);
                        string status = str["status"].ToString();
                        if (status == "0")
                        {
                            returnVal = "{\"status\":0,\"lng\":\"" + string.Format("{0:0.000000}", Convert.ToDouble(str["result"]["location"]["lng"].ToString())) + "\",\"lat\":\"" + string.Format("{0:0.000000}", Convert.ToDouble(str["result"]["location"]["lat"].ToString())) + "\"}";
                        }
                        else
                            returnVal = "{\"status\":1,\"message\":\"无法确定该地址经纬度，请检查地址是否正确。\"}";
                    }
                }
            }
            else
                returnVal = "{\"status\":1,\"message\":\"请输入地址。\"}";
            return Content(returnVal);
        }

        public ActionResult EditUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditUser(FormCollection form)
        {
            try
            {
                UserInfo user = (UserInfo)Session["userInfo"];
                UserInfoDAL userDal = new UserInfoDAL();
                string oldPwd = form["txtOldPwd"].ToString();
                string newPwd = form["txtNewPwd"].ToString();
                if (oldPwd != "" && StringFilter.getSHA1Code(oldPwd) != user.Password)
                {
                    return Content("<script>alert('原密码错误，请重新输入');history.go(-1);</script>");
                }
                user.TrueName = form["txtTrueName"].ToString();
                user.Mobile = form["txtMobile"].ToString();
                user.idType = Convert.ToInt32(form["txtCertifiID"]);
                user.idNumber = form["txtIdnumber"].ToString();
                user.Email = form["txtEmail"].ToString();
                if (newPwd != "")
                    user.Password = StringFilter.getSHA1Code(newPwd);
                user.ID = Convert.ToInt32(form["HId"]);
                int num = userDal.UPdateUser(user);
                if (num > 0)
                {
                    log.Module = "修改用户信息";
                    ChangeValue.AddOpLog(log);
                    log.What = "修改用户信息成功";
                    new OPLogDAL().InsertLog(log);
                    Session["userInfo"] = user;
                    if (oldPwd != "")
                    {
                        Session.Abandon();
                        Session.RemoveAll();
                        return Content("<script>alert('修改成功，请重新登录');parent.window.location.replace('" + Url.Content("~/login/index") + "');</script>");
                    }
                    else
                        return Content("<script>alert('修改成功'); history.go(0)</script>");
                }
                else
                {
                    return Content("<script>alert('修改失败');history.go(-1);</script>");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Exit()
        {
            log.Module = "登录管理";
            ChangeValue.AddOpLog(log);
            log.What = "退出登录";
            new OPLogDAL().InsertLog(log);
            UserInfo user = (UserInfo)Session["userInfo"];

            Session.Abandon();
            Session.RemoveAll();

            return Redirect(Url.Content("~/Login/Index"));
        }
        public ActionResult GetRoleList(string Id)
        {
            try
            {
                List<Sys_UserPowerInfo> list = new UserInfoDAL().GetRoleList(Convert.ToInt32(Id));
                if (list != null && list.Count > 0)
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    return Content(ser.Serialize(list));
                }
                else
                    return Content("[]");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","CommonMapController.GetRoleList(string Id)[HttpGet]"}
                });
                return Content("{\"err\":\"获取角色列表失败，请稍后重试\"}");
            }
        }
    }
}
