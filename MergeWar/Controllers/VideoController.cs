using HCZZ.Common;
using HCZZ.ModeDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HCZZ.DAL;
using System.Data;

namespace MergeWar.Controllers
{
    public class VideoController : Controller
    {
        OPLog log = new OPLog();
        public VideoController()
        {
            ViewBag.liShow = "video";
            log.Module = "视频检索";
            ChangeValue.AddOpLog(log);
        }
        string lishow = "SPJS";
        string LfetShow = "视频检索";
        // GET: Video
        [HttpGet]
        public ActionResult VideoList(int? Id, string txtAPName, string txtMac, string txtNETBAR_WACODE, string txtLName, string txtBeginTime, string txtEndTime)
        {
            try
            {
                ViewBag.location = "所在位置： <a href=\"" + Url.Content("~/Video/VideoList/1") + "\">视频检索</a> >> 视频数据列表";
                ViewBag.lishow = lishow;
                ViewBag.ashow = "video";
                ViewBag.LfetShow = LfetShow;
                ViewBag.ashow = "video";

                string display = "";

                if (txtMac == null && txtAPName == null && txtNETBAR_WACODE == null && txtLName == null && txtBeginTime == null && txtEndTime == null)
                    display = "display:none";

                int[] userType = ChangeValue.GetUserType();
                Dictionary<string, string> dic = new Dictionary<string, string>() { { "pageIndex", (Id ?? 1).ToString() }, { "pageSize", "10" }, { "txtMac", txtMac }, { "txtAPName", txtAPName }, { "txtNETBAR_WACODE", txtNETBAR_WACODE }, { "txtLName", txtLName==","?"":txtLName }, { "txtBeginTime", txtBeginTime }, { "txtEndTime", txtEndTime }, { "UserType", userType[0].ToString() }, { "AreaId", userType[1].ToString() } };
                if (string.IsNullOrEmpty(display))
                    ViewBag.pl = new Video3CDAL().GetVideo3CList(dic);
                else
                    ViewBag.pl = null;
                ViewBag.dic = dic;
                ViewBag.display = display;
                if (!string.IsNullOrEmpty(txtLName)) log.What = "场所名称：" + txtLName;
                if (!string.IsNullOrEmpty(txtAPName)) log.What += "AP名称：" + txtAPName;
                if (!string.IsNullOrEmpty(txtMac)) log.What += "审计终端MAC地址：" + txtMac;
                if (!string.IsNullOrEmpty(txtBeginTime)) log.What += "开始时间：" + txtBeginTime;
                if (!string.IsNullOrEmpty(txtEndTime)) log.What += "结束时间：" + txtEndTime;
                if (!string.IsNullOrEmpty(log.What)) { log.What = "视频数据列表导出Excel" + log.What; new OPLogDAL().InsertLog(log); }
            }
            catch (System.Data.SqlClient.SqlException sql)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
                Logger.ErrorLog(sql, new Dictionary<string, string>() { { "Function", this.GetType().Name + ".VideoList[HttpGet]" } });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>() { { "Function", this.GetType().Name + "VideoList[HttpGet]" } });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }

        [HttpGet]
        public ActionResult VideoExprt(string txtMac, string txtAPName, string txtLName, string txtBeginTime, string txtEndTime, string txtNETBAR_WACODE)
        {
            try
            {
                UserInfo user = (UserInfo)Session["userInfo"];

                if (Convert.ToDateTime(txtBeginTime) > Convert.ToDateTime(txtEndTime))
                    return Content("<script>alert('开始时间不能大于结束时间');</script>");

                int[] userType = ChangeValue.GetUserType();
                Dictionary<string, string> dic = new Dictionary<string, string>() { { "txtMac", txtMac }, { "txtNETBAR_WACODE", txtNETBAR_WACODE }, { "txtLName", txtLName }, { "txtAPName", txtAPName }, { "txtBeginTime", txtBeginTime }, { "txtEndTime", txtEndTime }, { "UserType", userType[0].ToString() }, { "AreaId", userType[1].ToString() } };

                if (!string.IsNullOrEmpty(txtLName)) log.What = "场所名称：" + txtLName;
                if (!string.IsNullOrEmpty(txtAPName)) log.What += "AP名称：" + txtAPName;
                if (!string.IsNullOrEmpty(txtMac)) log.What += "AP设备MAC地址：" + txtMac;
                if (!string.IsNullOrEmpty(txtNETBAR_WACODE)) log.What += "上网服务场所编码：" + txtNETBAR_WACODE;
                if (!string.IsNullOrEmpty(txtBeginTime)) log.What += "开始时间：" + txtBeginTime;
                if (!string.IsNullOrEmpty(txtEndTime)) log.What += "结束时间：" + txtEndTime;
                if (!string.IsNullOrEmpty(log.What)) { log.What = "视频数据列表导出Excel" + log.What; new OPLogDAL().InsertLog(log); }
                DataTable dt = new Video3CDAL().GetVideoToTable(dic);

                int rowCount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExprotCout"]);
                if (dt == null || dt.Rows.Count == 0)
                    return Content("{\"err\":\"对不起，没有要导出的数据\"}");
                else if (dt.Rows.Count > rowCount)
                    return Content("{\"err\":\"为了保证导出数据的速度，数据源不能大于“" + rowCount + "”行\"}");
                else
                {
                    Dictionary<string, string> Head_Dic = new Dictionary<string, string>();
                    if (!string.IsNullOrEmpty(txtLName)) Head_Dic.Add("场所名称", txtLName);
                    if (!string.IsNullOrEmpty(txtAPName)) Head_Dic.Add("AP名称", txtAPName);
                    if (!string.IsNullOrEmpty(txtMac)) Head_Dic.Add("AP设备MAC地址", txtMac);
                    if (!string.IsNullOrEmpty(txtNETBAR_WACODE)) Head_Dic.Add("上网服务场所编码", txtNETBAR_WACODE);
                    if (!string.IsNullOrEmpty(txtBeginTime)) Head_Dic.Add("开始时间", txtBeginTime);
                    if (!string.IsNullOrEmpty(txtEndTime)) Head_Dic.Add("结束时间", txtEndTime);

                    Dictionary<string, string> Title_Dic = new Dictionary<string, string>()
                    {
                        {"序号","PageNum"},
                        {"场所名称","LName"},
                        {"AP名称","APName"},
                        {"文件名","FileName"},
                        {"文件大小","FileSize"},
                        {"开始时间","BeginTime"},
                        {"结束时间","EndTime"},
                        {"创建时间","CreateTime"}
                    };

                    NPOISheetModel sheet = new NPOISheetModel();
                    sheet.dt = dt;
                    sheet.ExcelTitle = "视频数据查询";
                    sheet.TableTitle = Title_Dic;
                    sheet.TableSearch = Head_Dic;

                    string path = new ComNPOIExcel().Export(sheet, Server.MapPath("~/") + "UserData/EXP/Video/");
                    return Content("{\"result\":\"" + path + "\"}");
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                return Content("{\"err\":\"检索数据遇到错误，请联系管理员\"}");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function",this.GetType().Name+".VideoExprt()[HttpGet]"}
                });
                return Content("{\"err\":\"导出文件时出错，请联系管理员\"}");
            }
        }

        [HttpGet]
        public ActionResult DownloadVideo(int Id)
        {
            try
            {
                Video3CInfo vi = new Video3CDAL().GetVideo3CInfoById(Id);
                string VideoFilePath = System.Configuration.ConfigurationManager.AppSettings["VideoFilePath"];
                string videoPath = VideoFilePath + "/" + vi.V3CID + "/" + vi.DayFile + "/" + vi.FileName;

                System.IO.FileInfo file = new FileInfo(videoPath);
                Response.Clear();
                Response.ClearHeaders();
                Response.Buffer = false;
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("" + vi.FileName, System.Text.Encoding.UTF8).Replace("+", "%20"));
                Response.AppendHeader("Content-Length", file.Length.ToString());
                Response.WriteFile(file.FullName);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function",this.GetType().Name+".DownloadVideo()[HttpGet]"}
                });
                return Content("<script>alert('下载过程出现问题，请稍后重试')</script>");
            }
            return View();
        }
    }
}