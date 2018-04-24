using HCZZ.Common;
using HCZZ.AppCode;
using HCZZ.DAL;
using HCZZ.ModeDB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace HCZZ.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        OPLog log = new OPLog();
        [HttpGet]
        public ActionResult Index()
        {
            ////禁止页面被缓存
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.Today.AddYears(-2));
            ////调用Mac数据缓存
            //WebCommon.GetCacheMac(false);
            ////获取生产商信息缓存
            ////WebCommon.GetCacheBusName();

            ////获取场所和全部Mac缓存信息
            //WebCommon.GetCacheLoca();
            //WebCommon.GetCacheMacAllList();

            ////获取安全厂商缓存
            //WebCommon.GetSecurityList(false);
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            try
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Today.AddYears(-2));
                //调用Mac数据缓存
                WebCommon.GetCacheMac(false);
                //获取生产商信息缓存
                //WebCommon.GetCacheBusName();
                WebCommon.GetCacheLoca();
                WebCommon.GetCacheMacAllList();
                string userName = form["txtName"].ToString();
                string pwd = StringFilter.getSHA1Code(form["txtPwd"].ToString());
                if (Session["ValidateStr"] == null)
                {
                    ViewBag.errscript = "alert('验证码过期！')";
                }
                else
                {
                    if (form["txtValidator"].ToString().ToLower() == Session["ValidateStr"].ToString().ToLower())
                    {
                        //string userName  = "001";
                        //string userName = form["txtName"].ToString().Trim();
                        //string pwd = StringFilter.getSHA1Code(form["txtPwd"].ToString().Trim());
                        //string pwd = StringFilter.getSHA1Code("123456").Trim();

                        UserInfoDAL udal = new UserInfoDAL();
                        UserInfo user = udal.GetUserInfoDSByLogin(userName, pwd);
                        if (user != null)
                        {
                            Session["userInfo"] = user;
                            log.UID = user.ID;
                            log.Module = "登录管理";
                            log.What = "登录成功，用户名：" + user.UserName + "；用户Id：" + user.ID;
                            udal.UpdateListtime(user.ID);
                            ChangeValue.AddOpLog(log);
                            new OPLogDAL().InsertLog(log);
                            user.PowerPathList = udal.GetUserPowerFSListToJid(user.JId);
                            IEnumerable<Sys_UserPowerInfo> IElist = user.PowerPathList.OrderBy(a => a.Indexs);
                            user.PowerPathList = IElist.ToList();
                            user.PowerList = udal.GetUserShowPageByJid(user.JId);
                            List<Sys_UserPowerInfo> sys_list  =user.PowerList.Where(a => a.FilePath.Equals("Home/Index")).ToList();
                            if (sys_list != null && sys_list.Count() > 0)
                            {
                                return Redirect(Url.Content("~/Home/Index" + ""));
                            }
                            else
                            {
                                int pid = user.PowerList.Where(A => A.Pid == 0).OrderBy(A => A.Indexs).First().SpId; ;
                                string url = user.PowerList.Where(m => m.Pid == pid).OrderBy(m => m.Indexs).First().FilePath;
                                if (!string.IsNullOrEmpty(url))
                                {
                                    return Redirect(Url.Content("~/" + url + ""));
                                }
                                else
                                {
                                    return Content("<script>alert('抱歉，没有任何权限');window.location.href='" + Url.Content("~/Login/Index") + "';</script>");
                                }
                            }
                        }
                        else
                        {
                            Session["ValidateStr"] = null;
                            //return Content("alert('登录失败')");
                            log.Module = "登录管理";
                            log.What = "登录失败，用户名：" + userName;
                            ViewBag.errscript = "alert('登录失败')";
                        }
                    }
                    else
                    {
                        Session["ValidateStr"] = null;
                        ViewBag.errscript = "alert('验证码输入错误，请重新输入')";
                    }
                }
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
        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult Validator()
        {
            try
            {
                Common.Validator vali = new Common.Validator();
                string code = vali.GenerateCheckCode();
                Session["ValidateStr"] = code;
                byte[] imgstream = vali.CreateCheckCodeImage(code);
                return File(imgstream, "image/jpg");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LoginController.Validator()[HttpGet]"}
                });
                return Content("<script>alert('下载过程出现错误，请联系管理员');history.go(-1);</script>");
            }
        }

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
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        private void DownFile(string name, string path)
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
    }
}
