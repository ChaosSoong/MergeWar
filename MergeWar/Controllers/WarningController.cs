using HCZZ.Common;
using HCZZ.DAL;
using HCZZ.ModeDB;
using MergeWar.Filter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HCZZ.Controllers
{
    [SystemAutherFilter]
    public class WarningController : Controller
    {
        private OPLog log = new OPLog();

        public WarningController()
        {
            ViewBag.lishow = "Audit";
            log.Module = "审计管理";
            ChangeValue.AddOpLog(log);
        }
        public static string PageSize = System.Configuration.ConfigurationManager.AppSettings["PageSize"];
        //
        // GET: /Warning/
        string lishow = "BJGL";
        string LfetShow = "布控报警";

        #region 布控管理
        public ActionResult Index(string TaskName, string selTaskName, string keyWord, string selkeyWord, string HeadName, string selHeadName, string StartCreateTime, string EndCreateTime, string StartEndTime, string EndEndTime, int? PageIndex)
        {
            ViewBag.lishow = lishow;
            ViewBag.ashow = "war";
            ViewBag.LfetShow = LfetShow;
            ViewBag.ashow = "war";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Warning/Index") + "'>布控报警</a>>><a href='" + Url.Content("~/Warning/Index") + "'>布控管理</a>";
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"TaskName", TaskName},
                    {"selTaskName", selTaskName},
                    { "keyWord",keyWord},
                    { "selkeyWord",selkeyWord},
                    {"Mobile", ""},
                    {"selMobile", ""},
                    {"Mac", ""},
                    {"selMac", ""},
                    {"IMEI", ""},
                    {"selIMEI", ""},
                    {"HeadName", HeadName},
                    {"CaseType", "1"},
                    {"selHeadName", selHeadName},
                    {"StartCreateTime", StartCreateTime},
                    {"EndCreateTime", EndCreateTime},
                    {"StartEndTime", StartEndTime},
                    {"EndEndTime", EndEndTime},
                    {
                        "Uid",
                        ((UserInfo) Session["userinfo"]).Type == 1
                            ? "0"
                            : ((UserInfo) Session["userinfo"]).ID.ToString()
                    },
                    {"PageIndex", (PageIndex ?? 1).ToString()},
                    {"PageSize", "10"}
                };
                ViewBag.pl = new AuditDAL().GetAuditList(dic);
                ViewBag.dic = dic;
            }
            catch (SqlException sql)
            {
                Logger.ErrorLog(sql, null);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
            }
            return View();
        }

        public ActionResult RepIndex(string CaseType, string TaskName, string selTaskName, string keyWord, string selkeyWord, string HeadName, string selHeadName, string StartCreateTime, string EndCreateTime, string StartEndTime, string EndEndTime)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                {"CaseType",CaseType},
                {"TaskName", TaskName},
                {"selTaskName", selTaskName},
                { "keyWord",keyWord},
                { "selkeyWord",selkeyWord},
                {"Mac", ""},
                {"selMac", ""},
                {"Mobile", ""},
                {"selMobile", ""},
                {"IMEI", ""},
                {"selIMEI", ""},
                {"IM", ""},
                {"selIM", ""},
                {"HeadName", HeadName},
                {"selHeadName", selHeadName},
                {"StartCreateTime", StartCreateTime},
                {"EndCreateTime", EndCreateTime},
                {"StartEndTime", StartEndTime},
                {"EndEndTime", EndEndTime},
                {"Uid", ((UserInfo) Session["userinfo"]).Type==1?"0":((UserInfo) Session["userinfo"]).ID.ToString()},
            };
            DataTable dt = new AuditDAL().ExportAuditDT(dic);

            Dictionary<string, string> Head_Dic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(TaskName)) Head_Dic.Add("审计任务名称", TaskName);
            if (!string.IsNullOrEmpty(keyWord)) Head_Dic.Add("布控内容", keyWord);
            if (!string.IsNullOrEmpty(HeadName)) Head_Dic.Add("负责人", HeadName);
            if (!string.IsNullOrEmpty(StartCreateTime)) Head_Dic.Add("审计任务添加开始时间", StartCreateTime);
            if (!string.IsNullOrEmpty(EndCreateTime)) Head_Dic.Add("审计任务添加结束时间", EndCreateTime);
            if (!string.IsNullOrEmpty(StartEndTime)) Head_Dic.Add("审计条件-审计有效开始时间", StartEndTime);
            if (!string.IsNullOrEmpty(EndEndTime)) Head_Dic.Add("审计条件-审计有效结束时间", EndEndTime);

            Dictionary<string, string> Title_Dic = new Dictionary<string, string>();
            if (CaseType == "1")
            {
                Title_Dic = new Dictionary<string, string>()
                {
                    {"序号", "ID"},
                    {"布控任务名称", "TaskName"},
                    {"布控类型", "CaseItem"},
                    {"布控内容", "CaseValue"},
                    {"负责人", "HeadName"},
                    {"布控任务添加时间", "CreateTime"},
                    {"布控条件-审计有效时间", "EndTime"},
                    {"状态", "IsEnabled"},
                };
            }
            else
            {
                Title_Dic = new Dictionary<string, string>()
                {
                    {"序号","ID"},
                    {"审计任务名称","TaskName"},
                    {"即时通讯标识","CaseValue"},
                    {"即时通讯工具","ST_NETWORK_APP"},
                    {"负责人", "HeadName"},
                    {"审计任务添加时间", "CreateTime"},
                    {"审计条件-审计有效时间", "EndTime"},
                    {"状态", "IsEnabled"},
                };
            }

            NPOISheetModel sheet = new NPOISheetModel();
            sheet.dt = dt;
            sheet.ExcelTitle = CaseType == "1" ? "布控报警" : "即时通讯审计";
            sheet.TableTitle = Title_Dic;
            sheet.TableSearch = Head_Dic;
            string path = new ComNPOIExcel().Export(sheet, Server.MapPath("~/") + "UserData/EXP/Audit/");
            return Content("{\"result\":\"" + path + "\"}");
        }
        [HttpGet]
        public ActionResult AddIndex()
        {
            ViewBag.lishow = lishow;
            ViewBag.ashow = "war";
            ViewBag.LfetShow = LfetShow;
            ViewBag.ashow = "war";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Warning/Index") + "'>布控管理</a> >>添加布控项";
            ViewBag.DSArea = new AuditDAL().GetAreaList();
            ViewBag.NetbarType = ChangeValue.GetLocaTypeList();
            return View();
        }
        [HttpPost]
        public ActionResult AddIndex(FormCollection form)
        {
            AuditDAL auditdal = new AuditDAL();
            UserInfo user = (UserInfo)Session["userInfo"];
            BKBJ.AuditInfo audit = new BKBJ.AuditInfo();
            try
            {
                if (form["txtCaseItem"] == "3")
                {
                    if (Request.Files.Count >= 1 && Request.Files[0] != null)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        string path = Server.MapPath("~/UserData/Warning/");
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        string ext = file.FileName.Substring(file.FileName.LastIndexOf(".")).Split('.')[1].ToString();
                        if (!(ext.ToLower() == "txt"))
                            return Content("<script>alert('只支持后缀名为txt的文件格式');history.go(-1);</script>");
                        else if (file.ContentLength == 0)
                            return Content("<script>alert('导入文件不能为空');history.go(-1);</script>");
                        else if (file.ContentLength > 20 * 1024 * 1024)
                            return Content("<script>alert('文件不能大于20MB');history.go(-1);</script>");
                        string extendName = file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
                        string wholePath = Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + extendName);
                        file.SaveAs(wholePath);
                        string strval = string.Empty;
                        FileStream fs = new FileStream(wholePath, FileMode.Open);
                        StreamReader reader = new StreamReader(fs, Encoding.Default);
                        int ErrNum = 0;
                        int Count = 0;
                        int skip = 0;
                        while ((strval = reader.ReadLine()) != null)
                        {
                            if (strval.Contains("|"))
                            {
                                string[] TempArray = strval.Split('|');
                                for (int i = 0; i < TempArray.Length; i++)
                                {
                                    audit.TaskName = i == 0 ? "黑名单：" + TempArray[0] : "黑名单：" + TempArray[1];
                                    audit.Uid = user.ID;
                                    audit.ProID = user.ProID;
                                    audit.CityID = user.CityID;
                                    audit.AID = user.AId;
                                    audit.CaseItem = Convert.ToInt32(form["txtCaseItem"]);
                                    audit.CaseType = 1;
                                    audit.CaseValue = TempArray[i];
                                    audit.NETWORK_APP = 0;
                                    audit.HeadName = form["txtHeadName"];
                                    audit.HeadMobile = form["txtHeadMobile"];
                                    audit.DeployArea = string.IsNullOrEmpty(form["ckAll"]) ? form["txtDeployArea"] : "0";
                                    audit.IsEnabled = 2;
                                    audit.IsValid = 0;
                                    audit.MailWarn = Convert.ToInt32(form["txtMailWarn"]);
                                    audit.WMail = audit.MailWarn == 1 ? form["txtWMail"] : "";
                                    audit.StartTime = DateTime.Now;
                                    audit.EndTime = Convert.ToDateTime(form["txtEndTime"]);
                                    audit.CreateTime = DateTime.Now;
                                    audit.Remark = form["txtRemark"];
                                    audit.NetbarType = form["cbtype"];
                                    if (auditdal.GetCaseTaskNameCount(audit.TaskName, 0, user.ID) > 0)
                                    {
                                        skip = skip + 1;
                                        //DirectoryInfo difo = new DirectoryInfo(file.FileName);
                                        //string path = difo.Parent.ToString();
                                        //StreamWriter sw = new StreamWriter(file + "/布控失败" + difo.Name, true, Encoding.Default);
                                        //sw.WriteLine(string.Join("|", TempArray));
                                        //sw.Close();
                                        //sw.Dispose();
                                    }
                                    else
                                    {
                                        if (new AuditDAL().AddAudit(audit) > 0)
                                        {
                                            log.What = "添加人员审计," + "用户ID:" + user.ID + "；审计类型：" + (audit.CaseItem == 1 ? "手机号码" : (audit.CaseItem == 2 ? "MAC地址" : "批量导入")) + "；审计内容：" + audit.CaseValue;
                                        }
                                        else
                                        {
                                            ErrNum = ErrNum + 1;
                                        }
                                    }
                                }
                            }
                            Count = Count + 1;
                        }
                        fs.Close();
                        fs.Dispose();
                        reader.Close();
                        reader.Dispose();
                        return Content("<script>alert('操作完成，成功‘" + ((Count - ErrNum - skip) > 0 ? (Count - ErrNum - skip) : 0) + "’条！，跳过重复添加‘" + (skip) + "’条，失败‘" + ErrNum + "’条，请重新添加。');location.replace('" + Url.Content("~/Warning/Index") + "');</script>");
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(form["txtTaskName"]))
                        return Content("<script>alert('布控任务名称不能为空。');history.go(-1);</script>");
                    if (string.IsNullOrEmpty(form["txtCaseItem"]))
                        return Content("<script>alert('布控类型不能为空。');history.go(-1);</script>");
                    if(string.IsNullOrEmpty(form["keyWord"]))
                        return Content("<script>alert('布控内容不能为空。');history.go(-1);</script>");
                    if (string.IsNullOrEmpty(form["txtEndTime"]))
                        return Content("<script>alert('布控有效时间不能为空。');history.go(-1);</script>");
                    if (string.IsNullOrEmpty(form["txtDeployArea"]))
                        return Content("<script>alert('布控范围不能为空。');history.go(-1);</script>");
                    if (string.IsNullOrEmpty(form["cbtype"]))
                        return Content("<script>alert('布控场所类别不能为空。');history.go(-1);</script>");
                    if (auditdal.GetCaseTaskNameCount(form["txtTaskName"], 0, user.ID) > 0)
                        return Content("<script>alert('布控任务名称已存在，请重新输入。');history.go(-1);</script>");
                    if (auditdal.GetCaseMobileCount(0, Convert.ToInt32(form["txtCaseItem"]), form["txtCaseValue"], -1, user.ID, 1) > 0)
                        return Content("<script>alert('该" + (Convert.ToInt32(form["txtCaseItem"]) == 1 ? "手机号码" : (Convert.ToInt32(form["txtCaseItem"]) == 2 ? "MAC地址" : "IMEI")) + "已存在，请重新输入。');history.go(-1);</script>");
                    audit.TaskName = form["txtTaskName"];
                    audit.Uid = user.ID;
                    audit.ProID = user.ProID;
                    audit.CityID = user.CityID;
                    audit.AID = user.AId;
                    audit.CaseItem = Convert.ToInt32(form["txtCaseItem"]);
                    audit.CaseType = 1;
                    audit.CaseValue = form["txtCaseValue"];
                    audit.NETWORK_APP = 0;
                    audit.HeadName = form["txtHeadName"];
                    audit.HeadMobile = form["txtHeadMobile"];
                    audit.DeployArea = string.IsNullOrEmpty(form["ckAll"]) ? form["txtDeployArea"] : "0";
                    audit.IsEnabled = 2;
                    audit.IsValid = 0;
                    audit.MailWarn = Convert.ToInt32(form["txtMailWarn"]);
                    audit.WMail = audit.MailWarn == 1 ? form["txtWMail"] : "";
                    audit.StartTime = DateTime.Now;
                    audit.EndTime = Convert.ToDateTime(form["txtEndTime"]);
                    audit.CreateTime = DateTime.Now;
                    audit.Remark = form["txtRemark"];
                    audit.NetbarType = form["cbtype"];
                    if (new AuditDAL().AddAudit(audit) > 0)
                    {
                        log.What = "添加人员审计," + "用户ID:" + user.ID + "；审计类型：" + (audit.CaseItem == 1 ? "手机号码" : (audit.CaseItem == 2 ? "MAC地址" : "批量导入")) + "；审计内容：" + audit.CaseValue;
                        return Content("<script>alert('添加成功');location.replace('" + Url.Content("~/Warning/Index") + "');</script>");
                    }
                    else
                    {
                        return Content("<script>alert('添加失败');history.go(-1);</script>");
                    }
                }
            }
            catch (SqlException sqlException)
            {
                Logger.ErrorLog(sqlException, null);
                return Content("<script>alert('添加失败，请稍后重试！');history.go(-1);</script>");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                return Content("<script>alert('添加失败');history.go(-1);</script>");
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditIndex(int Id)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "war";
                ViewBag.LfetShow = LfetShow;
                ViewBag.ashow = "war";
                ViewBag.location = "所在位置：<a href='" + Url.Content("~/Warning/Index") + "'>布控管理</a> >>编辑布控项";
                UserInfo user = (UserInfo)Session["userInfo"];
                BKBJ.AuditInfo audit = new AuditDAL().GetAuditInfoById(Id, user.ID);
                ViewBag.deploy = audit;
                ViewBag.NetbarType = ChangeValue.GetLocaTypeList();
                ViewBag.DSArea = new AuditDAL().GetAreaList();
            }
            catch (SqlException sqlException)
            {
                Logger.ErrorLog(sqlException, null);
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditIndex(FormCollection form)
        {
            try
            {
                AuditDAL auditdal = new AuditDAL();
                UserInfo user = (UserInfo)Session["userInfo"];
                BKBJ.AuditInfo audit = new BKBJ.AuditInfo();
                audit.ID = Convert.ToInt32(form["ID"]);
                if (string.IsNullOrEmpty(form["txtTaskName"]))
                    return Content("<script>alert('审计任务名称不能为空。');history.go(-1);</script>");
                if (string.IsNullOrEmpty(form["txtCaseValue"]))
                    return Content("<script>alert('审计条件不能为空。');history.go(-1);</script>");
                if (string.IsNullOrEmpty(form["txtEndTime"]))
                    return Content("<script>alert('审计有效时间不能为空。');history.go(-1);</script>");
                if (string.IsNullOrEmpty(form["txtDeployArea"]))
                    return Content("<script>alert('布控范围不能为空。');history.go(-1);</script>");
                if (string.IsNullOrEmpty(form["cbtype"]))
                    return Content("<script>alert('布控场所类别不能为空。');history.go(-1);</script>");
                if (auditdal.GetCaseTaskNameCount(form["txtTaskName"], audit.ID, user.ID) > 0)
                    return Content("<script>alert('审计任务名称已存在，请重新输入。');history.go(-1);</script>");
                if (auditdal.GetCaseMobileCount(audit.ID, Convert.ToInt32(form["txtCaseItem"]), form["txtCaseValue"], -1, user.ID, 1) > 0)
                    return Content("<script>alert('该" + (Convert.ToInt32(form["txtCaseItem"]) == 1 ? "手机号码" : (Convert.ToInt32(form["txtCaseItem"]) == 2 ? "MAC地址" : "IMEI")) + "已存在，请重新输入。');history.go(-1);</script>");
                audit.TaskName = form["txtTaskName"];
                audit.Uid = user.ID;
                audit.ProID = user.ProID;
                audit.CityID = user.CityID;
                audit.AID = user.AId;
                audit.CaseItem = Convert.ToInt32(form["txtCaseItem"]);
                audit.CaseType = 1;
                audit.CaseValue = form["txtCaseValue"];
                audit.NETWORK_APP = 0;
                audit.HeadName = form["txtHeadName"];
                audit.HeadMobile = form["txtHeadMobile"];
                audit.DeployArea = string.IsNullOrEmpty(form["ckAll"]) ? form["txtDeployArea"] : "0";
                audit.IsEnabled = 2;
                audit.IsValid = 0;
                audit.MailWarn = Convert.ToInt32(form["txtMailWarn"]);
                audit.WMail = audit.MailWarn == 1 ? form["txtWMail"] : "";
                audit.StartTime = DateTime.Now;
                audit.EndTime = Convert.ToDateTime(form["txtEndTime"]);
                audit.CreateTime = DateTime.Now;
                audit.Remark = form["txtRemark"];
                audit.NetbarType = form["cbtype"];
                if (new AuditDAL().UpdateAudit(audit) > 0)
                {
                    log.What = "修改人员审计," + "用户ID:" + user.ID + "；审计ID：" + audit.ID + "；审计类型：" + (audit.CaseItem == 1 ? "手机号码" : (audit.CaseItem == 2 ? "MAC地址" : "IMEI")) + "；审计内容：" + audit.CaseValue;
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('修改成功');location.replace('" + Url.Content("~/Warning/Index") + "');</script>");
                }
                else
                {
                    return Content("<script>alert('修改失败');history.go(-1);</script>");
                }
            }
            catch (SqlException sqlException)
            {
                Logger.ErrorLog(sqlException, new Dictionary<string, string>()
                {
                    {"Function", "AuditController.AddPersonAudit"}
                });
                return Content("<script>alert('修改失败，请稍后重试！');history.go(-1);</script>");
            }
        }

        public ActionResult IndexModel(int Id)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "war";
                ViewBag.LfetShow = LfetShow;
                ViewBag.ashow = "war";
                ViewBag.location = "所在位置：<a href='" + Url.Content("~/Warning/Index") + "'>布控管理</a> >>布控项详情";
                UserInfo user = (UserInfo)Session["userInfo"];
                BKBJ.AuditInfo audit = new AuditDAL().GetAuditInfoById(Id, user.ID);
                ViewBag.NetbarType = ChangeValue.GetLocaTypeList();
                ViewBag.deploy = audit;
                ViewBag.DSArea = new AuditDAL().GetAreaList();
            }
            catch (SqlException sqlex)
            {
                Logger.ErrorLog(sqlex, null);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
            }
            return View();
        }
        [HttpGet]
        public ActionResult DelIndex(int Id)
        {
            try
            {
                UserInfo user = (UserInfo)Session["userInfo"];
                if (new AuditDAL().DelAudit(Id, user.ID) > 0)
                {
                    return Content("<script>alert('删除成功');location.replace('" + Url.Content("~/Warning/Index") + "');</script>");
                }
                else
                {
                    return Content("<script>alert('删除失败');history.go(-1);</script>");
                }
            }
            catch (SqlException sqlException)
            {
                Logger.ErrorLog(sqlException, new Dictionary<string, string>()
                {
                    {"Function", "public ActionResult DelPersonAudit(int Id)[HttpGet]"}
                });
                return Content("<script>alert('删除失败，数据库错误');history.go(-1);</script>");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function", "public ActionResult DelPersonAudit(int Id)[HttpGet]"}
                });
                return Content("<script>alert('删除失败，未知错误');history.go(-1);</script>");
            }
        }

        /// <summary>
        /// 修改审计的使用状态
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeAuditAbled(int Id)
        {
            try
            {
                UserInfo user = (UserInfo)Session["userInfo"];
                if (new AuditDAL().ChangeAuditAbled(Id, user.ID) > 0)
                    return Content("{\"result\":\"ok\"}");
                else
                    return Content("{\"result\":\"nopower\"}");
            }
            catch (SqlException sqlException)
            {
                Logger.ErrorLog(sqlException, new Dictionary<string, string>()
                {
                    {"Function", "ChangeAuditAbled(int Id)"}
                });
                return Content("{\"err\":\"dataerr\"}");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function", "ChangeAuditAbled(int Id)"}
                });
                return Content("{\"err\":\"err\"}");
            }
        }
        #endregion

        #region 报警管理
        public ActionResult TimelyMsg(int? PageIndex, int? selTaskName, string txtTaskName, int? selName, int? selWaringType, string txtName, string keyWord, string selkeyWord, string txtStartTime, string txtEndTime)
        {
            ViewBag.lishow = lishow;
            ViewBag.LfetShow = LfetShow;
            ViewBag.ashow = "TimelyMsg";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Warning/Index") + "'>布控报警</a>>><a href='" + Url.Content("~/Charater/TimelyMsg") + "'>报警管理</a>";
            try
            {
                UserInfo user = (UserInfo)Session["userInfo"];
                //ViewBag.liShow = "Warning";
                ViewBag.left = "PWList";
                ViewBag.navig = "所在位置：报警管理 >> <a href=\"" + Url.Content("~/Warning/PersonWaringList") + "\">报警管理</a> >> 人员报警";
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"PageIndex", (PageIndex ?? 1).ToString()},
                    {"PageSize", "10"},
                    {"selTaskName", (selTaskName ?? 1).ToString()},
                    {"txtTaskName", txtTaskName},
                    {"selName", (selName ??1).ToString()},
                    {"txtName", txtName},
                    { "keyWord",keyWord},
                    { "selkeyWord",selkeyWord},
                    {"selMobile", ""},
                    {"txtMobile", ""},
                    {"selMac", ""},
                    {"txtMac", ""},
                    {"selIMEI", ""},
                    {"txtIMEI", ""},
                    {"txtStartTime", txtStartTime},
                    {"txtEndTime", txtEndTime},
                    {"CaseType", "1"},
                    {"UserID", user.ID.ToString()},
                    {"ExcelType","1"},
                    {"UserType",user.Type.ToString()},
                    {"selWaringType",(selWaringType??0).ToString()}
                };
                ViewBag.pl = new WarningDAL().GetPersonWaringListDal(dic);
                ViewBag.dic = dic;
                return View(ViewBag.pl);
            }
            catch (SqlException ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>() { { "Function", "Warning.PersonWaringList(int selTaskName, string txtTaskName, int selName, string txtName, int selMobile, string txtMobile, int selMac, string txtMac, int selIMEI, string txtIMEI, string txtStartTime, string txtEndTime)" } });
                return Content("<script>alert('查询超时，请稍后再试');history.go(-1);parent.closeDiv();</script>");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>() { { "Function", "Warning.PersonWaringList(int selTaskName, string txtTaskName, int selName, string txtName, int selMobile, string txtMobile, int selMac, string txtMac, int selIMEI, string txtIMEI, string txtStartTime, string txtEndTime)" } });
                return Content("<script>alert('查询超时，请稍后再试');history.go(-1);parent.closeDiv();</script>");
            }
        }

        public ActionResult RepTimelyMsg(int? selTaskName, string txtTaskName, int? selName, int? selWaringType, string txtName, string keyWord, string selkeyWord, string txtStartTime, string txtEndTime)
        {
            try
            {
                UserInfo user = (UserInfo)Session["userInfo"];
                //ViewBag.liShow = "Warning";
                ViewBag.left = "PWList";
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"selTaskName", (selTaskName??1).ToString()},
                    {"txtTaskName", txtTaskName},
                    {"selName", (selName??1).ToString()},
                    {"txtName", txtName},
                    { "keyWord",keyWord},
                    { "selkeyWord",selkeyWord},
                    {"selMobile",""},
                    {"txtMobile", ""},
                    {"selMac", ""},
                    {"txtMac", ""},
                    {"selIMEI", ""},
                    {"txtIMEI", ""},
                    {"txtStartTime", txtStartTime},
                    {"txtEndTime", txtEndTime},
                    {"CaseType", "1"},
                    {"UserID", user.ID.ToString()},
                    {"ExcelType","2"},
                    { "UserType",user.Type.ToString()},
                      {"selWaringType",(selWaringType??0).ToString()}
                };
                object obj = new WarningDAL().GetPersonWaringListDal(dic);
                System.Data.DataTable dt = new System.Data.DataTable();
                System.Data.DataSet ds = new System.Data.DataSet();
                ds = (System.Data.DataSet)obj;
                dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DST_IP"] == DBNull.Value)
                    {
                        dt.Rows[i]["DST_IP"] = "00000";
                    }
                }
                Dictionary<string, string> Table_dic = new Dictionary<string, string>()
                {
                    {"审计任务名称","TaskName"},
                    {"场所名称","PLACE_NAME"},
                    {"布控类型","CaseItem"},
                    {"布控内容","CaseValue"},
                    {"预警邮箱","WMail"},
                    {"报警时间","Createtime"},
                    {"场所内网端口号","PORT"},
                    {"目的公网IPv4地址","DST_IP"}
                };
                Dictionary<string, string> Head_dic = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(txtTaskName))
                    dic.Add("审计任务名称", txtTaskName);
                if (!string.IsNullOrEmpty(txtName))
                    dic.Add("场所名称", txtName);
                //if (!string.IsNullOrEmpty(txtMobile))
                //    dic.Add("手机号码", txtMobile);
                //if (!string.IsNullOrEmpty(txtMac))
                //    dic.Add("上网终端MAC地址：", txtMac);
                if (!string.IsNullOrEmpty(txtStartTime))
                    dic.Add("报警开始时间", txtStartTime);
                if (!string.IsNullOrEmpty(txtEndTime))
                    dic.Add("报警结束时间", txtEndTime);

                NPOISheetModel sheel = new NPOISheetModel();
                sheel.dt = dt;
                sheel.ExcelTitle = "网警通预警---人员报警";
                sheel.TableTitle = Table_dic;
                sheel.TableSearch = Head_dic;
                string filepath = new ComNPOIExcel().Export(sheel, Server.MapPath("~/") + "UserData/EXP/PWList/");
                //Dispose();
                //return Content("{\"result\":"+filepath+"\"}");
                return Content("{\"result\":\"" + filepath + "\"}");
            }
            catch (SqlException ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>() { { "Function", "Warning.ExcelPwList(int selTaskName, string txtTaskName, int selName, string txtName, int selMobile, string txtMobile, int selMac, string txtMac, string txtStartTime, string txtEndTime)" } });
                return Content("{\"err\":\"导出文件出错，请稍后重试\"}");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>() { { "Function", "Warning.ExcelPwList(int selTaskName, string txtTaskName, int selName, string txtName, int selMobile, string txtMobile, int selMac, string txtMac, string txtStartTime, string txtEndTime)" } });
                return Content("{\"err\":\"导出文件出错，请稍后重试\"}");
            }
        }

        public ActionResult TimelyMsgModel(int ID)
        {
            ViewBag.lishow = lishow;
            ViewBag.LfetShow = LfetShow;
            ViewBag.ashow = "TimelyMsg";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Warning/TimelyMsg") + "'>报警管理</a>>><a href='" + Url.Content("~/Charater/TimelyMsg?ID=" + ID) + "'>报警详细</a>";
            try
            {

                BKBJ.WarningInfo warinfo = new DAL.WarningDAL().PwDetailsDal(ID);
                ViewBag.warinfo = warinfo;
                return View();
            }
            catch (SqlException ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>() { { "Function", "Warning.PWDetails(int ID)" } });
                return Content("<script>alert('操作超时，请稍后再试');history.go(-1);parent.closeDiv();</script>");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>() { { "Function", "Warning.PWDetails(int ID)" } });
                return Content("<script>alert('操作超时，请稍后再试');history.go(-1);parent.closeDiv();</script>");
            }
        }
        #endregion
    }
}
