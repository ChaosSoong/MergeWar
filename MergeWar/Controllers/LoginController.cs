using Common;
using HCZZ.DAL;
using HCZZ.ModeDB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace HCZZ.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        OPLog log=new OPLog();
        public ActionResult Index(FormCollection form)
        {
            try
            {
                if (Session["ValidateStr"] != null)
                {
                    return View();
                }
                else
                {
                    //if (form["txtValidator"].ToString().ToLower() != Session["ValidateStr"].ToString().ToLower())
                    //{
                        string userName  = "001";
                        // form["txtName"].ToString().Trim();userName
                        //string pwd = StringFilter.getSHA1Code(form["txtPwd"].ToString().Trim());
                        string pwd = StringFilter.getSHA1Code("123456").Trim();
                        
                        UserInfoDAL udal = new UserInfoDAL();
                        UserInfo user = udal.GetUserInfoDSByLogin(userName, pwd);
                        if (user != null)
                        {
                            Session["userInfo"] = user;
                            log.UID = user.ID;
                            log.Module = "登录管理";
                            log.What = "登录成功，用户名：" + user.UserName + "；用户Id：" + user.ID;
                            udal.UpdateListtime(user.ID);
                            new OPLogDAL().InsertLog(log);
                            user.PowerPathList = udal.GetUserPowerFSListToJid(user.JId);
                            IEnumerable<Sys_UserPowerInfo> IElist=user.PowerPathList.OrderBy(a => a.Indexs);
                            user.PowerPathList = IElist.ToList();
                            user._UserShowPage = udal.GetUserShowPageByJid(user.JId);
                            if (user.PowerPathList.Count > 0)
                            {
                                 return Redirect(Url.Content("/"+user.PowerPathList[0].FilePath));
                            }
                        }
                        else
                        {
                            Session["ValidateStr"] = null;
                            ViewBag.errscript = "alert('登录失败')";
                            log.Module = "登录管理";
                            log.What = "登录失败，用户名：" + user.UserName + "；用户Id：" + user.ID;
                        }
                    //}
                    //else
                    //{
                    //    Session["ValidateStr"] = null;
                    //    ViewBag.errscript = "alert('验证码输入错误，请重新输入')";
                    //}
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



    }
}
