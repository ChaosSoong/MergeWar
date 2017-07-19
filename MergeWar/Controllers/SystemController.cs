using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Common;
using HCZZ.AppCode;
using HCZZ.DAL;
using HCZZ.ModeDB;
using HCZZt.Filter;
using MergeWar.Filter;
using Webdiyer.WebControls.Mvc;

namespace HCZZ.Controllers
{
    [SessionNull]
    [SystemAutherFilter]
    public class SystemController : Controller
    {
        //
        // GET: /System/
        string lishow = "XTGL";
        VersionDAL VerDal = new VersionDAL();
        OPLogDAL logDal = new OPLogDAL();
        UserInfoDAL userDal = new UserInfoDAL();
        OPLog log = new OPLog();
        UserInfo DBuser = new UserInfo();
        public ActionResult Index()
        {
            return View();
        }
        #region 用户管理
        /// <summary>
        /// 用户管理列表
        /// 创建人:邓佳训
        /// 创建时间：2017-06-20
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult UserList(int? PageIndex, string selName, string txtName, string selTrueName, string txtTrueName, string selMobile, string txtMobile, string selIdnumber, string txtIdnumber, string UserType)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "user";
                ViewBag.LfetShow = "系统管理";
                ViewBag.location = "所在位置：<a href='" + Url.Content("~/System/UserList") + "'>系统管理</a>>><a href='" + Url.Content("~/System/UserList") + "'>用户管理</a>";
                ViewBag.lishow = lishow;
                selName = selName == null ? "1" : selName;
                selIdnumber = selIdnumber == null ? "1" : selIdnumber;
                selMobile = selMobile == null ? "1" : selMobile;
                selTrueName = selTrueName == null ? "1" : selTrueName;
                //form["Valid"] = string.IsNullOrEmpty(form["Valid"]) ? "0" : form["Valid"];
                //form["UserType"] = form["UserType"] ?? "0";
                int[] userType = ChangeValue.GetUserType();
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"pageIndex",((PageIndex??1).ToString())},
                    {"pageSize","2"},
                    {"txtName",txtName},
                    {"selName",selName},
                    {"txtMobile",txtMobile},
                    {"selMobile",selMobile},
                    //{"txtIdnumber",txtIdnumber},
                    //{"selIdnumber",selIdnumber},
                    {"txtTrueName",txtTrueName},
                    {"selTrueName",selTrueName},
                    //{"CreateId",Valid},
                    //{"UserType",UserType},
                    {"AreaId",userType[1].ToString()}
                };
                ViewBag.ul = new UserInfoDAL().GetList(dic);
                log.Module = "系统管理-用户管理";
                if (!string.IsNullOrEmpty(txtName)) log.What += " 用户名：" + txtName;
                if (!string.IsNullOrEmpty(txtMobile)) log.What += " 手机号码：" + txtMobile;
                if (!string.IsNullOrEmpty(txtIdnumber)) log.What += " 证件号码：" + txtIdnumber;
                if (!string.IsNullOrEmpty(txtTrueName)) log.What += " 真实姓名：" + txtTrueName;
                //if (selUserType != "0") log.What = " 用户类型：" + ChangeValue.RefUseTypeValue(Convert.ToInt32(selUserType));
                if (!string.IsNullOrEmpty(log.What)) { log.What += "用户管理列表查询,查询条件：" + log.What; new OPLogDAL().InsertLog(log); }

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
        /// <summary>
        /// 添加新用户
        /// 创建人:邓佳训
        /// 创建时间：2017-06-20
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddUser()
        {
            ViewBag.lishow = lishow;
            ViewBag.ashow = "user";
            ViewBag.LfetShow = "系统管理";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/System/UserList") + "'>系统管理</a>>><a href='" + Url.Content("~/System/UserList") + "'>用户管理</a>>>添加新用户";

            return View();
        }
        [HttpPost]
        public ActionResult AddUser(FormCollection form)
        {
            try
            {
                UserInfoDAL userDal = new UserInfoDAL();
                DBuser = ChangeValue.GteUserVal(form);
                DBuser.CreateId = (Session["userInfo"] as UserInfo).ID;
                if (userDal.GetUserInfoByName(DBuser.UserName, 0) != null)
                    return Content("<script>alert('用户名已经存在,请重新输入!');history.go(-1);</script>");
                int result = userDal.InsertUser(DBuser);
                if (result > 0)
                {
                    log.Module = "系统管理-用户管理-添加用户";
                    log.What = "添加用户，用户名：" + DBuser.UserName + "，真实姓名：" + DBuser.TrueName;
                    new OPLogDAL().InsertLog(log);
                    userDal.AddCreateUserNum(DBuser.CreateId);
                    return Content("<script>alert('保存成功');location.replace('" + Url.Content("~/System/UserList") + "');</script>");
                }
                else
                {
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
                }
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
        /// <summary>
        /// 删除用户信息
        /// 创建人:邓佳训
        /// 创建时间：2017-06-20
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DelUser(int Id)
        {
            try
            {
                if (Id == ((Session["userInfo"] as UserInfo).ID))
                {
                    return Content("<script>alert('不能删除自己');history.go(-1);</script>");
                }
                DBuser = userDal.GetUserInfoById(Id);
                if (userDal.DeleteUser(Id) > 0)
                {
                    log.Module = "系统管理-用户管理-删除用户信息";
                    log.What = "删除用户，Id：" + DBuser.ID + "，用户名：" + DBuser.UserName + "，真实姓名：" + DBuser.TrueName;
                    new OPLogDAL().InsertLog(log);
                    return
                        Content("<script>alert('删除成功');location.replace('" + Url.Content("~/System/UserList") +
                                "');</script>");
                }
                else
                    return Content("<script>alert('删除失败');history.go(-1);</script>");
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
        /// <summary>
        /// 编辑用户信息
        /// 创建人:邓佳训
        /// 创建时间：2017-06-20
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditUser(int Id)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "user";
                ViewBag.LfetShow = "系统管理";
                ViewBag.location = "所在位置：<a href='" + Url.Content("~/System/UserList") + "'>系统管理</a>>><a href='" + Url.Content("~/System/UserList") + "'>用户管理</a>>>编辑用户信息";
                ViewBag.user = new UserInfoDAL().GetUserInfoById(Id);
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
        [HttpPost]
        public ActionResult EditUser(FormCollection form)
        {
            try
            {
                UserInfoDAL userDal = new UserInfoDAL();
                DBuser = ChangeValue.GteUserVal(form);
                DBuser.ID = Convert.ToInt32(form["HId"]);
                if (userDal.GetUserInfoByName(DBuser.UserName, DBuser.ID) != null)
                    return Content("<script>alert('用户名已经存在,请重新输入!');history.go(-1);</script>");
                int result = userDal.UPdateUser(DBuser);
                if (result > 0)
                {
                    log.Module = "系统管理-用户管理-编辑用户信息";
                    log.What = "编辑用户信息，Id：" + DBuser.ID + "，用户名：" + DBuser.UserName + "，真实姓名：" + DBuser.TrueName;
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('保存成功');location.replace('" + Url.Content("~/System/UserList") + "');</script>");
                }
                else
                {
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
                }
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
        /// <summary>
        /// 用户信息详细
        /// 创建人:邓佳训
        /// 创建时间：2017-06-20
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DetUser(int Id)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "user";
                ViewBag.LfetShow = "系统管理";
                ViewBag.user = new UserInfoDAL().GetUserInfoById(Id);
                log.UserName = (Session["userInfo"] as UserInfo).UserName;
                log.Module = "系统管理-用户管理-查看用户信息";
                log.What = "查看用户详细信息，用户Id：" + Id;
                new OPLogDAL().InsertLog(log);
            }
            catch (SqlException sql)
            {
                Logger.ErrorLog(sql, null);
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
            }
            return View();
        }

        #endregion

        #region 角色管理
        /// <summary>
        /// 角色管理列表
        /// 创建人:邓佳训
        /// 创建时间：2017-06-20
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RoleList(int? PageIndex, string CreateTime, string EndTime, string keyword)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "role";
                ViewBag.LfetShow = "系统管理";
                ViewBag.location = "所在位置：<a href='" + Url.Content("~/System/RoleList") + "'>系统管理>><a href='" + Url.Content("~/System/RoleList") + "'>角色管理</a></a>";
                keyword = (string.IsNullOrEmpty(keyword) || keyword == "请输入查询条件") ? "" : keyword;
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"PageIndex",((PageIndex??1).ToString())},
                    {"PageSize","5"},
                    {"CreateTime",CreateTime},
                    {"EndTime", EndTime },
                    {"txtkeyword",keyword}
                };
                PagedList<Sys_UserPowerInfo> pl = VerDal.GetRoleList(dic);
                ViewBag.dic = dic;
                return View(pl);
            }
            catch (SqlException sql)
            {
                Logger.ErrorLog(sql, null);
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
            }
            return View();
        }
        /// <summary>
        /// 添加新角色
        /// 创建人:邓佳训
        /// 创建时间：2017-06-20
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddRole()
        {
            ViewBag.lishow = lishow;
            ViewBag.ashow = "role";
            ViewBag.LfetShow = "系统管理";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/System/UserList") + "'>系统管理</a>>><a href='" + Url.Content("~/System/RoleList") + "'>角色管理</a>>>添加新角色";
            return View();
        }
        /// <summary>
        /// 添加新角色
        /// 创建人:邓佳训
        /// 创建时间：2017-06-20
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRole(string name)
        {
            if (string.IsNullOrEmpty(name))
                return Content("{\"err\":\"请输入提交的数据\"}");
            try
            {
                VersionDAL VerDal = new VersionDAL();
                if (VerDal.GetRoleNameCountByNameOrId(name, 0))
                    return Content("{\"err\":\"角色名称已存在\"}");
                int result = VerDal.InsertRole(name);
                if (result > 0)
                {
                    log.What = "添加角色：" + name;
                    new OPLogDAL().InsertLog(log);
                    return Content("{\"result\":\"OK\"}");
                }
                else
                    return Content("{\"result\":\"NO\"}");
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                Logger.ErrorLog(sqlEx, null);
                return Content("{\"err\":\"添加角色失败\"}");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                return Content("{\"err\":\"添加角色失败\"}");
            }
        }
        /// <summary>
        /// 删除角色
        /// 创建人:邓佳训
        /// 创建时间：2017-06-20
        /// </summary>
        /// <returns></returns>
        public ActionResult DelRole(int Jid)
        {
            try
            {
                int result = VerDal.DeleteRoleById(Jid);
                if (result > 0)
                {
                    log.Module = "系统管理-角色管理-删除角色信息";
                    log.What = "删除角色信息，Id：" + Jid;
                    logDal.InsertLog(log);
                    return Content("<script>alert('删除成功');location.replace('" + Url.Content("~/System/RoleList") + "');</script>");
                }
                else
                    return Content("<script>alert('删除失败');history.go(-1);</script>");
            }
            catch (SqlException sql)
            {
                Logger.ErrorLog(sql, null);
                return Content("<script>alert('删除失败');history.go(-1);</script>");

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                return Content("<script>alert('删除失败');history.go(-1);</script>");
            }
        }
        /// <summary>
        /// 编辑角色信息
        /// 创建人:邓佳训
        /// 创建时间：2017-06-20
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditRole(int Jid)
        {
            ViewBag.lishow = lishow;
            ViewBag.ashow = "role";
            ViewBag.LfetShow = "系统管理";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/System/UserList") + "'>系统管理</a>>><a href='" + Url.Content("~/System/RoleList") + "'>角色管理</a>>>编辑角色信息";
            VersionDAL VerDal = new VersionDAL();
            Sys_UserPowerInfo suPower = VerDal.GetUserRoleName(Jid);
            suPower.Jid = Jid;
            return View(suPower);
        }
        [HttpPost]
        public ActionResult EditRole(int Jid, string Name)
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                    return Content("{\"err\":\"请输入提交的数据\"}");
                else
                {
                    VersionDAL VerDal = new VersionDAL();
                    if (VerDal.GetRoleNameCountByNameOrId(Name, Jid))
                        return Content("{\"err\":\"角色名称已存在\"}");
                    if (VerDal.ChangeRoleNameById(Jid, Name) > 0)
                    {
                        log.Module = "系统管理-角色管理-编辑角色信息";
                        log.What = "编辑角色名称";
                        logDal.InsertLog(log);
                        return Content("{\"result\":\"OK\"}");
                    }
                    else
                        return Content("{\"err\":\"保存失败，请稍后重试\"}");
                }
            }
            catch (SqlException sql)
            {
                Logger.ErrorLog(sql, null);
                return Content("{\"err\":\"修改角色失败\"}");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                return Content("{\"err\":\"修改角色失败\"}");
            }
        }

        #endregion

        #region 角色关系管理
        /// <summary>
        /// 添加角色权限-从库里取出所有权限
        /// 创建人：邓佳训
        /// 创建时间：2017-06-20
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddPower(int id)
        {
            ViewBag.lishow = lishow;
            ViewBag.ashow = "role";
            ViewBag.LfetShow = "系统管理";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/System/RoleList") + "'>系统管理>></a><a href='" + Url.Content("~/System/RoleList") + "'>角色管理></a>>添加权限";
            List<Sys_UserPowerInfo> list = null;
            try
            {
                list = new VersionDAL().GetPowerList();
                ViewBag.JId = id;
                return View(list);
            }
            catch (SqlException sql)
            {
                Logger.ErrorLog(sql, null);
                return Content("<script>alert('数据处理遇到错误，请联系管理员！');history.go(-1);</script>");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                return Content("<script>alert('系统错误，请联系管理员！');history.go(-1);</script>");
            }
        }
        /// <summary>
        /// 编辑角色权限
        /// 创建人：邓佳训
        /// 创建时间：2017-06-20
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditPower(int id)
        {
            ViewBag.LfetShow = "系统管理";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "role";
            List<Sys_UserPowerInfo> list = null;
            try
            {
                ViewBag.Left = "Power";
                ViewBag.location = "所在位置：<a href='" + Url.Content("~/System/RoleList") + "'>系统管理>></a><a href='" + Url.Content("~/System/RoleList") + "'>角色管理></a>>编辑权限";
                VersionDAL VerDal = new VersionDAL();
                list = VerDal.GetPowerList();
                List<Sys_UserPowerInfo> QXlist = VerDal.GetPowerListByJid(id);
                ViewBag.JId = id;
                ViewBag.QXlist = QXlist;
                log.Module = "系统管理-角色管理-编辑角色权限";
                log.What = "编辑角色";
                return View(list);
            }
            catch (System.Data.SqlClient.SqlException sql)
            {
                Logger.ErrorLog(sql, null);
                return Content("<script>alert('数据处理遇到错误，请联系管理员！');history.go(-1);</script>");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                return Content("<script>alert('系统错误，请联系管理员！');history.go(-1);</script>");
            }
        }

        /// <summary>
        /// 添加、编辑权限关系值
        /// 创建人：邓佳训
        /// 创建时间：2017-06-20
        /// </summary>
        /// <param name="Jid"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddPower(int Jid, string strVal)
        {
            try
            {
                string[] Fsp = strVal.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, string> dic = new Dictionary<string, string>();
                for (int i = 0; i < Fsp.Length; i++)
                {
                    string[] sVal = Fsp[i].Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                    dic.Add(sVal[1].ToString(), sVal[2].ToString().Substring(0, sVal[2].Length - 1).Replace("-", ","));
                    if (!dic.Keys.Contains(sVal[0].ToString()))
                    {
                        List<string> list = new List<string>();
                        if (sVal[2].Contains("1")) list.Add("1");
                        if (sVal[2].Contains("2")) list.Add("2");
                        if (sVal[2].Contains("3")) list.Add("3");
                        if (sVal[2].Contains("4")) list.Add("4");
                        dic.Add(sVal[0].ToString(), string.Join(",", list));
                    }
                }

                VersionDAL VerDal = new VersionDAL();
                VerDal.DelRelaByJid(Jid);

                if (VerDal.InsertRelaGX(Jid, dic) > 0)
                {
                    UserInfo user = (UserInfo)Session["userinfo"];
                    if (HttpContext.Cache["userNavigMenu_" + user.ID] == null)
                    {
                        HttpContext.Cache.Remove("userNavigMenu_" + user.ID);
                    }
                    log.Module = "系统管理-角色管理";
                    log.What = "添加、编辑权限关系值";
                    logDal.InsertLog(log);
                    return Content("{\"result\":\"OK\"}");
                }
                else
                    return Content("{\"err\":\"false\"}");
            }
            catch (SqlException sql)
            {
                Logger.ErrorLog(sql, null);
                return Content("{\"err\":\"false\"}");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                return Content("{\"err\":\"false\"}");
            }
        }
        #endregion

        #region 系统日志

        public ActionResult LogList(int? PageIndex, string selName, string txtName, string selAdderss, string txtAdderss, string txtWhat, string CreateTime, string EndTime)
        {
            ViewBag.lishow = lishow;
            ViewBag.ashow = "log";
            ViewBag.LfetShow = "系统管理";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/System/UserList") + "'>系统管理>><a href='" + Url.Content("~/System/LogList") + "'>日志管理</a></a>";
            try
            {
                selName = string.IsNullOrEmpty(selName) ? "1" : selName;
                selAdderss = string.IsNullOrEmpty(selAdderss) ? "1" : selAdderss;
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"pageIndex", (PageIndex ?? 1).ToString()},
                    {"pageSize", "10"},
                    {"txtName", txtName},
                    {"selName", selName},
                    {"selAdderss", selAdderss},
                    {"txtAdderss", txtAdderss},
                    {"txtWhat", txtWhat},
                    {"CreateTime", CreateTime},
                    {"EndTime", EndTime}
                };
                ViewBag.dic = dic;
                ViewBag.log = logDal.GetList(dic);
                log.Module = "系统管理-日志管理-查看日志列表";
                if (!string.IsNullOrEmpty(txtName)) log.What += "用户名：" + txtName;
                if (!string.IsNullOrEmpty(txtAdderss)) log.What += "IP地址：" + txtAdderss;
                if (!string.IsNullOrEmpty(txtWhat)) log.What += "操作描述：" + txtWhat;
                if (!string.IsNullOrEmpty(CreateTime)) log.What += "记录开始时间范围：" + CreateTime;
                if (!string.IsNullOrEmpty(EndTime)) log.What += "记录结束时间范围：" + EndTime;
                if (!string.IsNullOrEmpty(log.What)) { log.What = "日志管理列表，" + log.What; new OPLogDAL().InsertLog(log); }
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
        [HttpGet]
        public ActionResult DetailLog(int Id)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "log";
                ViewBag.LfetShow = "系统管理";
                ViewBag.location = "所在位置：<a href='" + Url.Content("~/System/UserList") + "'>系统管理</a>>><a href='" + Url.Content("~/System/LogList") + "'>日志管理</a>>>查看日志详细";
                ViewBag.log = logDal.GetOPLogInfoById(Id);
                log.Module = "系统管理-系统日志";
                log.What = "查看日志详细信息，Id：" + Id;
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
        #endregion 

        #region 区域管理--省管理列表
        /// <summary>
        /// 区域管理列表
        /// 创建人:张团勃
        /// 创建时间：2017-06-27
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AreaManageInfoList(int? Id, string selName, string txtName)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "Area";
                ViewBag.LfetShow = "系统管理";
                ViewBag.location = "所在位置：<a href='" + Url.Content("~/System/UserList") + "'>系统管理>><a href='" + Url.Content("~/System/AreaManageInfoList") + "'>区域管理</a></a>";
                selName = selName == null ? "1" : selName;
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"pageIndex",(Id??1).ToString()},
                    {"pageSize","10"},
                    {"txtName",txtName},
                    {"selName",selName},
                    {"CityType","2"}
                };
                ViewBag.Pro = new SystemDAL().GetAreaManage(dic);
                if (!string.IsNullOrEmpty(txtName)) { log.What = "省管理列表查询,查询条件：省名称：" + txtName; new OPLogDAL().InsertLog(log); }
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
        /// <summary>
        /// 编辑省信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditProArea(int Id)
        {
            try
            {
                ViewBag.LfetShow = "系统管理";
                ViewBag.ProArea = new SystemDAL().GetAreaInfoById(Id, 2);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditProArea(FormCollection form)
        {
            AreaInfo ProArea = new AreaInfo();
            SystemDAL SystemDAL = new SystemDAL();
            ProArea.Area = form["txtArea"].ToString();
            ProArea.Pid = 0;
            ProArea.CityType = 2;
            ProArea.ID = Convert.ToInt32(form["Hid"]);
            ProArea.City_code = form["txtCity_code"].ToString();
            if (SystemDAL.JudgeProExist(ProArea.ID, ProArea.City_code, 2))
            {
                return Content("<script>alert('省代码已存在,请重新输入!');history.go(-1);</script>");
            }
            if (SystemDAL.JudgeProvince(ProArea.Area, 2, ProArea.ID, ProArea.Pid))
            {
                return Content("<script>alert('省已经存在,请重新输入!');history.go(-1);</script>");
            }
            if (SystemDAL.UpdateProArea(ProArea))
            {
                log.What = "编辑省信息，省ID：" + ProArea.ID + "，省名称名称：" + ProArea.Area;
                new OPLogDAL().InsertLog(log);
                return Content("<script>alert('保存成功');parent.location.replace('" + Url.Content("~/System/AreaManageInfoList") + "');</script>");
            }
            else
            {
                return Content("<script>alert('保存失败');history.go(-1);</script>");
            }
        }

        /// <summary>
        /// 添加省信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddProArea()
        {
            ViewBag.LfetShow = "系统管理";
            return View();
        }
        [HttpPost]
        public ActionResult AddProArea(FormCollection form)
        {
            AreaInfo area = new AreaInfo();
            SystemDAL SystemDAL = new SystemDAL();
            area.Area = form["txtArea"].ToString();
            area.Pid = 0;
            area.CityType = 2;
            area.City_code = form["txtCity_code"].ToString();
            area.ProID = 0;
            area.CreateTime = DateTime.Now;
            if (SystemDAL.JudgeProExist(0, area.City_code, 2))
            {
                return Content("<script>alert('省代码已存在,请重新输入!');history.go(-1);</script>");
            }
            if (SystemDAL.JudgeProvince(area.Area, 2, 0, area.Pid))
            {
                return Content("<script>alert('省已经存在,请重新输入!');history.go(-1);</script>");
            }
            int result = SystemDAL.InsertProArea(area);
            if (result > 0)
            {
                log.What = "添加省信息，省ID：" + area.ID + "，省名称名称：" + area.Area;
                new OPLogDAL().InsertLog(log);
                return Content("<script>alert('保存成功');parent.location.replace('" + Url.Content("~/System/AreaManageInfoList") + "');</script>");
            }
            else
            {
                return Content("<script>alert('保存失败');history.go(-1);</script>");
            }
        }




        #endregion

        #region 市管理列表
        /// <summary>
        /// 创建人:张团勃
        /// 创建时间：2017-06-27
        ///</summary>
        /// <param name="Id"></param>
        /// <param name="key"></param>
        /// <param name="selName"></param>
        /// <param name="txtName"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AreaManageCityInfoList(int Id, int? key, string selName, string txtName)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "Area";
                ViewBag.LfetShow = "系统管理";
                ViewBag.location = "所在位置：<a href=\"" + Url.Content("~/User/UserList") + "\">系统管理</a> >> <a href=\"" + Url.Content("~/System/AreaManageInfoList") + "\">区域管理</a> >> <a href=\"" + Url.Content("~/System/AreaManageInfoList") + "\">省管理</a> >> 市管理";
                selName = selName == null ? "1" : selName;
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"pageIndex",(key??1).ToString()},
                    {"pageSize","10"},
                    {"txtName",txtName},
                    {"selName",selName},
                    {"CityType","1"},
                    {"ProId",Id.ToString()}
                };
                ViewBag.Pro = new SystemDAL().GetAreaManage(dic);
                if (!string.IsNullOrEmpty(txtName))
                {
                    log.What = "省管理列表查询,查询条件：省名称：" + txtName;
                    new OPLogDAL().InsertLog(log);
                }
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

        /// <summary>
        /// 添加市信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddCity(int ID)
        {
            ViewBag.LfetShow = "系统管理";
            ViewBag.CityArea = new SystemDAL().GetAreaInfoById(ID, 2);
            return View();
        }
        [HttpPost]
        public ActionResult AddCity(FormCollection form)
        {
            AreaInfo CityArea = new AreaInfo();
            SystemDAL SystemDAL = new SystemDAL();
            CityArea.Area = form["txtArea"].ToString();
            CityArea.Pid = Convert.ToInt32(form["Hid"]);
            CityArea.CityType = 1;
            CityArea.City_code = form["txtCity_code"].ToString();
            CityArea.ProID = CityArea.Pid;
            CityArea.CreateTime = DateTime.Now;
            if (SystemDAL.JudgeProExist(0, CityArea.City_code, 1))
            {
                return Content("<script>alert('市代码已存在,请重新输入!');history.go(-1);</script>");
            }
            if (SystemDAL.JudgeProvince(CityArea.Area, 1, 0, CityArea.Pid))
            {
                return Content("<script>alert('市已经存在,请重新输入!');history.go(-1);</script>");
            }
            int result = SystemDAL.InsertProArea(CityArea);
            if (result > 0)
            {
                log.What = "添加市信息，市ID：" + CityArea.ID + "市名称名称：" + CityArea.Area;
                new OPLogDAL().InsertLog(log);
                return Content("<script>alert('保存成功');parent.location.replace('" + Url.Content("~/System/AreaManageCityInfoList/" + CityArea.Pid) + "');</script>");
            }
            else
            {
                return Content("<script>alert('保存失败');history.go(-1);</script>");
            }
        }
        /// <summary>
        /// 编辑市信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditCity(int ID)
        {
            ViewBag.LfetShow = "系统管理";
            ViewBag.CityArea = new SystemDAL().GetAreaInfoById(ID, 1);
            return View();
        }
        [HttpPost]
        public ActionResult EditCity(FormCollection form)
        {
            AreaInfo CityArea = new AreaInfo();
            SystemDAL SystemDAL = new SystemDAL();
            CityArea.Area = form["txtArea"].ToString();
            CityArea.ID = Convert.ToInt32(form["Hid"]);
            CityArea.Pid = Convert.ToInt32(form["Hpid"]);
            CityArea.CityType = 1;
            CityArea.City_code = form["txtCity_code"].ToString();

            if (SystemDAL.JudgeProExist(CityArea.ID, CityArea.City_code, 1))
            {
                return Content("<script>alert('市代码已存在,请重新输入!');history.go(-1);</script>");
            }

            if (SystemDAL.JudgeProvince(CityArea.Area, 1, CityArea.ID, CityArea.Pid))
            {
                return Content("<script>alert('市已经存在,请重新输入!');history.go(-1);</script>");
            }

            if (SystemDAL.UpdateProArea(CityArea))
            {
                log.What = "编辑市信息，市ID：" + CityArea.ID + "市名称名称：" + CityArea.Area;
                new OPLogDAL().InsertLog(log);
                return Content("<script>alert('保存成功');parent.location.replace('" + Url.Content("~/System/AreaManageCityInfoList/" + CityArea.Pid) + "');</script>");
            }
            else
            {
                return Content("<script>alert('保存失败');history.go(-1);</script>");
            }
        }
        #endregion

        #region 区管理列表
        /// <summary>
        /// 创建人：张团勃
        /// 创建时间：2017-06-29
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="key"></param>
        /// <param name="selName"></param>
        /// <param name="txtName"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AreaInfoList(int Id, int? key, string selName, string txtName)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "Area";
                AreaInfo area = new SystemDAL().GetAreaInfoById(Id, 1);
                ViewBag.LfetShow = "系统管理";
                ViewBag.location = "所在位置：<a href=\"" + Url.Content("~/User/UserList") + "\">系统管理</a> >> <a href=\"" + Url.Content("~/System/AreaManageInfoList") + "\">区域管理</a> >> <a href=\"" + Url.Content("~/System/AreaManageInfoList") + "\">省管理</a> >> <a href=\"" + Url.Content("~/System/AreaManageCityInfoList/" + area.Pid) + "\">市管理</a> >> 区管理";

                selName = selName == null ? "1" : selName;
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"pageIndex",(key??1).ToString()},
                    {"pageSize","10"},
                    {"txtName",txtName},
                    {"selName",selName},
                    {"CityType","0"},
                    {"Cid",Id.ToString()}
                };
                ViewBag.Pro = new SystemDAL().GetAreaManage(dic);
                if (!string.IsNullOrEmpty(txtName))
                {
                    log.What = "区管理列表查询,查询条件：区名称：" + txtName;
                    new OPLogDAL().InsertLog(log);
                }
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
        /// <summary>
        /// 添加区信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddArea(int ID)
        {
            ViewBag.LfetShow = "系统管理";
            ViewBag.area = new SystemDAL().GetAreaInfoById(ID, 1);
            return View();
        }
        public ActionResult AddArea(FormCollection form)
        {
            AreaInfo Area = new AreaInfo();
            SystemDAL SystemDAL = new SystemDAL();
            Area.Area = form["txtArea"].ToString();
            Area.Pid = Convert.ToInt32(form["Hpid"]);
            Area.CityType = 0;
            Area.City_code = form["txtCity_code"].ToString();
            Area.ProID = Convert.ToInt32(form["Hproid"]);
            Area.CreateTime = DateTime.Now;
            if (SystemDAL.JudgeProExist(0, Area.City_code, 0))
            {
                return Content("<script>alert('区代码已存在,请重新输入!');history.go(-1);</script>");
            }
            if (SystemDAL.JudgeProvince(Area.Area, 0, 0, Area.Pid))
            {
                return Content("<script>alert('区已经存在,请重新输入!');history.go(-1);</script>");
            }
            int result = SystemDAL.InsertProArea(Area);
            if (result > 0)
            {
                log.What = "添加区信息，区ID：" + Area.ID + "区名称名称：" + Area.Area;
                new OPLogDAL().InsertLog(log);
                return Content("<script>alert('保存成功');parent.location.replace('" + Url.Content("~/System/AreaInfoList/" + Area.Pid) + "');</script>");
            }
            else
            {
                return Content("<script>alert('保存失败');history.go(-1);</script>");
            }
        }

        /// <summary>
        /// 编辑区信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditArea(int ID)
        {
            ViewBag.LfetShow = "系统管理";
            ViewBag.area = new SystemDAL().GetAreaInfoById(ID, 0);
            return View();
        }
        public ActionResult EditArea(FormCollection form)
        {
            AreaInfo Area = new AreaInfo();
            SystemDAL SystemDAL = new SystemDAL();
            Area.Area = form["txtArea"].ToString();
            Area.Pid = Convert.ToInt32(form["Hpid"]);
            Area.CityType = 0;
            Area.City_code = form["txtCity_code"].ToString();
            Area.ID = Convert.ToInt32(form["Hid"]);
            if (SystemDAL.JudgeProExist(Area.ID, Area.City_code, 0))
            {
                return Content("<script>alert('区代码已存在,请重新输入!');history.go(-1);</script>");
            }
            if (SystemDAL.JudgeProvince(Area.Area, 0, Area.ID, Area.Pid))
            {
                return Content("<script>alert('区已经存在,请重新输入!');history.go(-1);</script>");
            }

            if (SystemDAL.UpdateProArea(Area))
            {
                log.What = "编辑区信息，市ID：" + Area.ID + "区名称名称：" + Area.Area;
                new OPLogDAL().InsertLog(log);
                return Content("<script>alert('保存成功');parent.location.replace('" + Url.Content("~/System/AreaInfoList/" + Area.Pid) + "');</script>");
            }
            else
            {
                return Content("<script>alert('保存失败');history.go(-1);</script>");
            }
        }
        #endregion

        #region 派出所列表
        /// <summary>
        /// 创建人：张团勃
        /// 创建时间：2017-06-29
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="key"></param>
        /// <param name="selName"></param>
        /// <param name="txtName"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PoliceInfoList(int Id, int? key, string selName, string txtName, string selMobile, string txtMobile, string selAddress, string txtAddress, string selContact, string txtContact)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "Area";
                AreaInfo area = new SystemDAL().GetAreaInfoById(Id, 0);
                ViewBag.LfetShow = "系统管理";
                ViewBag.location = "所在位置：<a href=\"" + Url.Content("~/User/UserList") + "\">系统管理</a> >> <a href=\"" + Url.Content("~/System/AreaManageInfoList") + "\">区域管理</a> >> <a href=\"" + Url.Content("~/System/AreaManageInfoList") + "\">省管理</a> >> <a href=\"" + Url.Content("~/System/AreaManageCityInfoList/" + area.ProID) + "\">市管理</a> >> <a href=\"" + Url.Content("~/System/AreaInfoList/" + area.Pid) + "\">区管理</a> >> 派出所管理";

                selName = selName == null ? "1" : selName;
                selMobile = selMobile == null ? "1" : selMobile;
                selAddress = selAddress == null ? "1" : selAddress;
                selContact = selContact == null ? "1" : selContact;
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"pageIndex",(key??1).ToString()},
                    {"pageSize","10"},
                    {"txtName",txtName},
                    {"selName",selName},
                    {"txtMobile",txtMobile},
                    {"selMobile",selMobile},
                    {"txtAddress",txtAddress},
                    {"selAddress",selAddress},
                    {"txtContact",txtContact},
                    {"selContact",selContact},
                    {"Aid",Id.ToString()}
                };
                ViewBag.Police = new SystemDAL().GetPoliceInfoList(dic);
                if (!string.IsNullOrEmpty(txtName) || !string.IsNullOrEmpty(txtMobile) || !string.IsNullOrEmpty(txtAddress) || !string.IsNullOrEmpty(txtContact))
                {
                    log.What = "派出所管理列表查询,查询条件：派出所名字：" + txtName + "；派出所地址：" + txtAddress + "；联系人：" + txtContact + "；联系人电话：" + txtMobile;
                    new OPLogDAL().InsertLog(log);
                }
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
        /// <summary>
        /// 添加派出所信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddPoliceInfo(int ID)
        {
            ViewBag.LfetShow = "系统管理";
            ViewBag.area = new SystemDAL().GetAreaInfoById(ID, 0);
            return View();
        }
        [HttpPost]
        public ActionResult AddPoliceInfo(FormCollection form)
        {
            try
            {
                PoliceInfo police = new PoliceInfo();
                SystemDAL SystemDAL = new SystemDAL();
                police.Aid = Convert.ToInt32(form["HAid"]);
                police.Name = form["txtName"].ToString();
                if (SystemDAL.GetPoliceInfo(police.Name, 0, police.Aid))
                {
                    return Content("<script>alert('派出所已经存在,请重新输入!');history.go(-1);</script>");
                }
                police.Mobile = form["txtMobile"].ToString();
                police.Address = form["txtAddress"].ToString();
                police.Contact = form["txtContact"].ToString();
                police.PoliceInfo_code = form["txtPoliceInfo_code"].ToString();
                police.CreateTime = DateTime.Now;
                int result = SystemDAL.InsertPoliceInfo(police);
                if (result > 0)
                {
                    log.What = "添加派出所信息，市ID：" + police.ID + "，派出所名称名称：" + police.Name;
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('保存成功');parent.location.replace('" + Url.Content("~/System/PoliceInfoList/" + police.Aid) + "');</script>");
                }
                else
                {
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
                }
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
        /// <summary>
        ///编辑派出所
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditPoliceInfo(int ID)
        {
            ViewBag.LfetShow = "系统管理";
            ViewBag.police = new SystemDAL().GetPoliceInfoById(ID);
            return View();
        }
        public ActionResult EditPoliceInfo(FormCollection form)
        {
            try
            {
                PoliceInfo police = new PoliceInfo();
                SystemDAL SystemDAL = new SystemDAL();
                police.Aid = Convert.ToInt32(form["HAid"]);
                police.ID = Convert.ToInt32(form["Hid"]);
                police.Name = form["txtName"].ToString();
                if (SystemDAL.GetPoliceInfo(police.Name, police.ID, police.Aid))
                {
                    return Content("<script>alert('派出所已经存在,请重新输入!');history.go(-1);</script>");
                }
                police.Mobile = form["txtMobile"].ToString();
                police.Address = form["txtAddress"].ToString();
                police.Contact = form["txtContact"].ToString();
                police.PoliceInfo_code = form["txtPoliceInfo_code"].ToString();
                int result = SystemDAL.UPdatePoliceInfo(police);
                if (result > 0)
                {
                    log.What = "修改派出所信息，市ID：" + police.ID + "，派出所名称名称：" + police.Name;
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('保存成功');parent.location.replace('" + Url.Content("~/System/PoliceInfoList/" + police.Aid) + "');</script>");
                }
                else
                {
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
                }
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
        #endregion

        #region 警区列表
        public ActionResult ScenicInfoList(int Id, int? key, string selName, string txtName, string selMobile, string txtMobile, string selAddress, string txtAddress, string selContact, string txtContact)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "Area";
                AreaInfo area = new SystemDAL().GetAreaInfoById(Id, 0);
                PoliceInfo police = new SystemDAL().GetPoliceInfoById(Id);
                ViewBag.LfetShow = "系统管理";
                ViewBag.location = "所在位置：<a href=\"" + Url.Content("~/User/UserList") + "\">系统管理</a> >> <a href=\"" + Url.Content("~/System/AreaManageInfoList") + "\">区域管理</a> >> <a href=\"" + Url.Content("~/System/AreaManageInfoList") + "\">省管理</a> >> <a href=\"" + Url.Content("~/System/AreaManageCityInfoList/" + area.ProID) + "\">市管理</a> >> <a href=\"" + Url.Content("~/System/AreaInfoList/" + area.Pid) + "\">区管理</a> >> <a href=\"" + Url.Content("~/System/PoliceInfoList/" + police.Aid) + "\">派出所管理</a> >> 警区管理";
                selName = selName == null ? "1" : selName;
                selMobile = selMobile == null ? "1" : selMobile;
                selAddress = selAddress == null ? "1" : selAddress;
                selContact = selContact == null ? "1" : selContact;
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"pageIndex",(key??1).ToString()},
                    {"pageSize","10"},
                    {"txtName",txtName},
                    {"selName",selName},
                    {"txtMobile",txtMobile},
                    {"selMobile",selMobile},
                    {"txtAddress",txtAddress},
                    {"selAddress",selAddress},
                    {"txtContact",txtContact},
                    {"selContact",selContact},
                    {"PId",Id.ToString()}
                };
                ViewBag.scenic = new SystemDAL().GetScenicInfoList(dic);
                ViewBag.police = new SystemDAL().GetPoliceInfoById(Id);
                if (!string.IsNullOrEmpty(txtName) || !string.IsNullOrEmpty(txtMobile) || !string.IsNullOrEmpty(txtAddress) || !string.IsNullOrEmpty(txtContact))
                {
                    log.What = "警区管理列表查询,查询条件：警区名字：" + txtName + "；派出所地址：" + txtAddress + "；联系人：" + txtContact + "；联系人电话：" + txtMobile;
                    new OPLogDAL().InsertLog(log);
                }
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
        /// <summary>
        /// 添加警区
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddScenicInfo(int ID)
        {
            ViewBag.LfetShow = "系统管理";
            ViewBag.police = new SystemDAL().GetPoliceInfoById(ID);
            return View();
        }
        [HttpPost]
        public ActionResult AddScenicInfo(FormCollection form)
        {
            try
            {
                ScenicInfo scenic = new ScenicInfo();
                SystemDAL SystemDAL = new SystemDAL();
                scenic.SName = form["txtName"].ToString();
                scenic.Pid = Convert.ToInt32(form["HidPid"].ToString());
                if (SystemDAL.GetScenicInfo(scenic.SName, 0, scenic.Pid))
                {
                    return Content("<script>alert('警区已经存在,请重新输入!');history.go(-1);</script>");
                }
                scenic.Mobile = form["txtMobile"].ToString();
                scenic.Address = form["txtAddress"].ToString();
                scenic.Contact = form["txtContact"].ToString();
                scenic.CreateTime = DateTime.Now;
                int result = SystemDAL.InsertScenicInfo(scenic);
                if (result > 0)
                {
                    log.What = "添加警区信息，市ID：" + scenic.ID + "，警区名称名称：" + scenic.SName;
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('保存成功');parent.location.replace('" + Url.Content("~/System/ScenicInfoList/" + scenic.Pid.ToString()) + "');</script>");
                }
                else
                {
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
                }
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
        /// <summary>
        /// 编辑警区
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditScenicInfo(int ID)
        {
            ViewBag.LfetShow = "系统管理";
            ViewBag.scenic = new SystemDAL().GetScenicInfoById(ID);
            return View();
        }
        [HttpPost]
        public ActionResult EditScenicInfo(FormCollection form)
        {
            try
            {
                ScenicInfo scenic = new ScenicInfo();
                SystemDAL SystemDAL = new SystemDAL();
                scenic.ID = Convert.ToInt32(form["Hid"]);
                scenic.Pid = Convert.ToInt32(form["HidPid"]);
                scenic.SName = form["txtName"].ToString();
                if (SystemDAL.GetScenicInfo(scenic.SName, scenic.ID, scenic.Pid))
                {
                    return Content("<script>alert('警区已经存在,请重新输入!');history.go(-1);</script>");
                }
                scenic.Mobile = form["txtMobile"].ToString();
                scenic.Address = form["txtAddress"].ToString();
                scenic.Contact = form["txtContact"].ToString();
                int result = SystemDAL.UPdateScenicInfo(scenic);
                if (result > 0)
                {

                    log.What = "修改警区信息，市ID：" + scenic.ID + "，警区名称名称：" + scenic.SName;
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('保存成功');parent.location.replace('" + Url.Content("~/System/ScenicInfoList/" + scenic.Pid) + "');</script>");
                }
                else
                {
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
                }
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
        #endregion

        #region  删除区域
        [HttpGet]
        public ActionResult DelProArea(int Id, string key, string key1)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "Area";
                int result = 0;
                SystemDAL pdal = new SystemDAL();
                int Num = pdal.GetAreaCountByID(Id, key);
                if (Num > 0)
                {
                    if (key == "Police")
                        return Content("<script>alert('该派出所下还存在 " + Num + " 个警区，请先删除警区');history.go(-1);</script>");
                    if (key == "Area")
                        return Content("<script>alert('该区下还存在 " + Num + " 个派出所，请先删除派出所信息');history.go(-1);</script>");
                    if (key == "AreaManageCity")
                        return Content("<script>alert('该市下还存在 " + Num + " 个区，请先删除区信息');history.go(-1);</script>");
                    if (key == "AreaManage")
                        return Content("<script>alert('该省下还存在 " + Num + " 个市，请先删除市信息');history.go(-1);</script>");
                    if (key == "Scenic")
                        return Content("<script>alert('该警区下还存在 " + Num + " 个场所，请先删除场所信息');history.go(-1);</script>");
                }
                else
                {
                    result = new SystemDAL().DeleteProArea(Id, key);
                    if (result > 0)
                    {

                        log.What = "删除数据成功，标示Id：" + Id + "，关键字：" + key;
                        new OPLogDAL().InsertLog(log);
                        return Content("<script>alert('删除成功');window.location.href='" + Url.Content("~/System/" + key + "InfoList" + (key == "AreaManage" ? "" : "/" + key1)) + "';</script>");
                    }
                    else
                        return Content("<script>alert('删除失败');history.go(-1);</script>");
                }

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

        #endregion

        #region 系统参数配置
        public ActionResult ConfigInfoList()
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "Config";
                ViewBag.LfetShow = "系统管理";
                ViewBag.location = "所在位置：<a href='" + Url.Content("~/System/UserList") + "'>系统管理>><a href='" + Url.Content("~/System/ConfigInfoList") + "'>系统参数配置管理</a></a>";
                ViewBag.pl = new SystemDAL().GetSysConfigList();
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
        [HttpGet]
        public ActionResult EditConfigInfo()
        {
            try
            {
                DataTable dt = new SystemDAL().GetSysConfigTable();
                List<int> list = ChangeValue.GetReturnInt(Convert.ToInt32(dt.Rows[5]["ConfigValue"]));
                ViewBag.list = list;
                ViewBag.dt = dt;
            }
            catch (SqlException sql)
            {
                Logger.ErrorLog(sql, null);
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
        public ActionResult EditConfigInfo(FormCollection form)
        {
            try
            {
                Dictionary<int, string> dic = new Dictionary<int, string>()
                {
                    {1 , form["txtConfigValue1"].ToString()},
                    {2 , form["txtConfigValue2"].ToString()},
                    {3 , form["txtConfigValue3"].ToString()},
                    {4 , form["txtConfigValue4"].ToString()},
                    {5 , form["txtConfigValue5"].ToString()},
                    {6 , ChangeValue.ReturnUintValue(form["ckConfigValue6"].ToString()).ToString()},
                    {7 , form["txtConfigValue7"].ToString()},
                    {8, form["txtConfigValue8"].ToString()},
                    {9, form["txtConfigValue9"].ToString()},
                    {10, form["txtConfigValue10"].ToString()},
                    {11, form["txtConfigValue11"].ToString()},
                    {12, form["txtConfigValue12"].ToString()},
                    {13, form["txtConfigValue13"].ToString()}
                };
                int result = new SystemDAL().UpdateSysConfig(dic);
                if (result > 0)
                {
                    log.What = "编辑系统参数配置";
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('保存成功！');parent.location.href=\"" + Url.Content("~/System/ConfigInfoList") + "\"</script>");
                }
                else
                    return Content("<script>alert('保存失败！');history.go(-1);</script>");
            }
            catch (SqlException sql)
            {
                Logger.ErrorLog(sql, null);
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }
        #endregion

        #region  终端版本管理
        ///终端版本管理信息详细
        ///创建人:张团勃
        ///创建时间：2017-07-03
        [HttpGet]
        public ActionResult VersionInfoList(int? ID)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "Version";
                ViewBag.LfetShow = "系统管理";
                ViewBag.location = "所在位置：系统管理 >> <a href=\"" + Url.Content("~/System/VersionInfoList") + "\">终端版本管理</a> ";
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"pageIndex",(ID??1).ToString()},
                    {"pageSize","10"}
                };
                ViewBag.pl = new SystemDAL().GetVersionList(dic);
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
        /// <summary>
        /// 添加终端版本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddVersionInfo()
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "Version";
                ViewBag.LfetShow = "系统管理";
                ViewBag.location = "所在位置：系统管理 >> <a href=\"" + Url.Content("~/System/VersionInfoList") + "\">终端版本管理</a> >> 添加终端版本信息";

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


        [HttpPost]
        public ActionResult AddVersionInfo(FormCollection form)
        {
            try
            {
                VersionInfo version = new VersionInfo();
                SystemDAL SystemDAL = new SystemDAL();
                version.Type = Convert.ToInt32(form["selType"]);
                version.OutVersion = form["txtOutVersion"].ToString();
                if (version.Type == 1 || version.Type == 2)
                {
                    version.CasesType = Convert.ToInt32(form["selCasesType"]);
                    version.UpdateType = Convert.ToInt32(form["selUpdateType"]);
                    version.otherType = Convert.ToInt32(form["selotherType"]);
                    if (version.UpdateType == 2)
                        version.Aid = Convert.ToInt32(form["selProvince"]);
                    else if (version.UpdateType == 3)
                        version.Aid = Convert.ToInt32(form["selCity"]);
                    else if (version.UpdateType == 4)
                        version.Aid = Convert.ToInt32(form["SelArea"]);
                    else
                        version.Aid = 0;
                    if (version.Type == 1)
                    {
                        version.VerifyType = form["ckVerify"].ToString();
                    }
                }
                if (SystemDAL.GetVersionInfo(version.Type, version.OutVersion, version.CasesType, 0) != null)
                {
                    return Content("<script>alert('版本已经存在,请重新输入!');history.go(-1);</script>");
                }
                version.FocusUpdate = Convert.ToInt32(form["selFocusUpdate"]);
                version.ChangeLog = form["txtChangeLog"].ToString();
                version.Version = form["txtVersion"].ToString();
                version.DownloadUrl = form["txtDownloadUrl"].ToString();
                int result = SystemDAL.InsertVersionInfo(version);
                if (result > 0)
                {
                    if (version.Type == 5)
                    {
                        version = new SystemDAL().GetMaxVersionInfoByType(5);
                        string strVer = null;
                        if (version == null)
                            strVer = "1.0";
                        else
                            strVer = version.OutVersion;
                        HttpContext.Cache.Insert("Version", strVer, null, DateTime.Now.AddHours(5), TimeSpan.Zero);
                    }
                    log.What = "添加终端版本信息，外部版本：" + version.OutVersion;
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('保存成功');window.location.replace('" + Url.Content("~/System/VersionInfoList") + "');</script>");
                }
                else
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
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
        /// <summary>
        /// 编辑终端版本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditVersionInfo(int Id)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "Version";
                ViewBag.LfetShow = "系统管理";
                ViewBag.location = "所在位置：系统管理 >> <a href=\"" + Url.Content("~/System/VersionInfoList") + "\">终端版本管理</a> >> 编辑终端版本信息";
                VersionInfo ver = new SystemDAL().GetVersionInfoById(Id);
                int proId = 0;
                int cid = 0;
                int aid = 0;
                if (ver.UpdateType == 2)
                    proId = ver.Aid;
                else if (ver.UpdateType == 3)
                {
                    proId = new PoliceDAL().GetAreaInfoById(ver.Aid, 2, 1).Pid;
                    cid = ver.Aid;
                }
                else if (ver.UpdateType == 4)
                {
                    AreaInfo area = new PoliceDAL().GetAreaInfoById(ver.Aid, 1, 1);
                    proId = area.ProID;
                    cid = area.Pid;
                    aid = ver.Aid;
                }
                ViewBag.proId = proId;
                ViewBag.cid = cid;
                ViewBag.aid = aid;
                ViewBag.detail = ver;
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

        [HttpPost]
        public ActionResult EditVersionInfo(FormCollection form)
        {
            try
            {
                VersionInfo version = new VersionInfo();
                SystemDAL SystemDAL = new SystemDAL();
                version.Type = Convert.ToInt32(form["selType"]);
                version.OutVersion = form["txtOutVersion"].ToString();
                version.ID = Convert.ToInt32(form["Hid"]);
                if (version.Type == 1 || version.Type == 2)
                {
                    version.CasesType = Convert.ToInt32(form["selCasesType"]);
                    version.UpdateType = Convert.ToInt32(form["selUpdateType"]);
                    version.otherType = Convert.ToInt32(form["selotherType"]);
                    if (version.UpdateType == 2)
                        version.Aid = Convert.ToInt32(form["selProvince"]);
                    else if (version.UpdateType == 3)
                        version.Aid = Convert.ToInt32(form["selCity"]);
                    else if (version.UpdateType == 4)
                        version.Aid = Convert.ToInt32(form["SelArea"]);
                    else
                        version.Aid = 0;
                    if (version.Type == 1)
                    {
                        version.VerifyType = form["ckVerify"].ToString();
                    }
                }
                if (SystemDAL.GetVersionInfo(version.Type, version.OutVersion, version.CasesType, version.ID) != null)
                    return Content("<script>alert('版本已经存在,请重新输入!');history.go(-1);</script>");
                version.FocusUpdate = Convert.ToInt32(form["selFocusUpdate"]);
                version.ChangeLog = form["txtChangeLog"].ToString();
                version.Version = form["txtVersion"].ToString();
                version.DownloadUrl = form["txtDownloadUrl"].ToString();
                int result = SystemDAL.UpdateVersionInfo(version);
                if (result > 0)
                {
                    if (version.Type == 5)
                    {
                        version = new SystemDAL().GetMaxVersionInfoByType(5);
                        string strVer = null;
                        if (version == null)
                            strVer = "1.0";
                        else
                            strVer = version.OutVersion;
                        HttpContext.Cache.Insert("Version", strVer, null, DateTime.Now.AddHours(5), TimeSpan.Zero);
                    }
                    log.What = "编辑终端版本信息，Id：" + version.ID;
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('保存成功');window.location.replace('" + Url.Content("~/System/VersionInfoList") + "');</script>");
                }
                else
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
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
        /// <summary>
        /// 删除终端版本
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DelVersionInfo(int ID)
        {
            try
            {
                int result = new SystemDAL().DeleteVersionInfo(ID);
                if (result > 0)
                {
                    return Content("<script>alert('保存成功');window.location.replace('" + Url.Content("~/System/VersionInfoList") + "');</script>");
                }
                else
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
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
        public ActionResult DetailedVersion(int ID)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "Version";
                ViewBag.LfetShow = "系统管理";
                ViewBag.location = "所在位置：系统管理 >> <a href=\"" + Url.Content("~/System/VersionInfoList") + "\">终端版本管理</a> >> 编辑终端版本信息";
                ViewBag.detail = new SystemDAL().GetVersionInfoById(ID);
                log.What = "查询终端版本详细信息ID：" + ID;
                new OPLogDAL().InsertLog(log);
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
        #endregion

        #region 安全厂商管理
        /// <summary>
        /// 安全厂商管理列表
        /// 创建人：邓佳训
        /// 创建时间：2017-07-03
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SecurityOrgList(int? pageIndex, string orgName, string orgCode)
        {

            ViewBag.lishow = lishow;
            ViewBag.ashow = "Org";
            ViewBag.LfetShow = "系统管理";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/System/UserList") + "'>系统管理>><a href='" + Url.Content("~/System/SecurityOrgList") + "'>安全厂商管理</a></a>";
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    {"pageIndex",(pageIndex??1).ToString()},
                    {"pageSize","10"},
                    {"orgName",orgName},
                    {"orgCode",orgCode}
                };
                ViewBag.pl = new SystemDAL().GetAQList(dic);
                ViewBag.dic = dic;
                if (!string.IsNullOrEmpty(orgName)) log.What += "厂商名称：" + orgName;
                if (!string.IsNullOrEmpty(orgCode)) log.What += "场所组织机构代码：" + orgCode;
                if (!string.IsNullOrEmpty(log.What)) { log.What = "安全厂商管理列表，" + log.What; new OPLogDAL().InsertLog(log); }
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

       
        /// <summary>
        /// 新增安全厂商
        /// 创建人：邓佳训
        /// 创建时间：2017-07-03
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddOrg()
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "Org";
                ViewBag.LfetShow = "系统管理";
                ViewBag.location = "所在位置：系统管理 >> <a href=\"" + Url.Content("~/System/SecurityOrgList") + "\">安全厂商管理</a> >> 添加安全厂商信息";
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
        /// <summary>
        /// /// <summary>
        /// 新增安全厂商
        /// 创建人：邓佳训
        /// 创建时间：2017-07-03
        /// </summary>
        /// <returns></returns>
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOrg(FormCollection form)
        {
            try
            {
                SecurityOrg sec = new SecurityOrg();
                HBHtmlDAL HBDal = new HBHtmlDAL();
                sec.SECURITY_SOFTWARE_ORGCODE = form["txtOrgcode"];
                sec.SECURITY_SOFTWARE_ORG_ADDRESS = form["txtAddress"];
                sec.SECURITY_SOFTWARE_ORGNAME = form["txtName"];
                sec.CONTACTOR = form["txtContact"];
                sec.CONTACTOR_TEL = form["txtPhone"];
                sec.CONTACTOR_MAIL = form["txtEmail"];
                if (HBDal.GetAQCountId(sec.SECURITY_SOFTWARE_ORGCODE, 0) > 0)
                {
                    return Content("<script>alert('该厂商组织机构代码已经存在,请重新输入!');history.go(-1);</script>");
                }
                int result = HBDal.InsertOrg(sec);
                if (result > 0)
                {
                    //重新获取安全厂商缓存
                    WebCommon.GetSecurityList(true);
                    log.What = "添加安全厂商信息，厂商名称：" + sec.SECURITY_SOFTWARE_ORGNAME + "，厂商组织机构代码：" + sec.SECURITY_SOFTWARE_ORGCODE;
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('保存成功');location.replace('" + Url.Content("~/System/SecurityOrgList") + "');</script>");
                }
                else
                {
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","SystemController.AddOrg(FormCollection form)[HttpPost]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();

        }
        /// <summary>
        /// /// <summary>
        /// 编辑安全厂商
        /// 创建人：邓佳训
        /// 创建时间：2017-07-03
        /// </summary>
        /// <returns></returns>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditOrg(int id)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "Org";
                ViewBag.LfetShow = "系统管理";
                ViewBag.location = "所在位置：系统管理 >> <a href=\"" + Url.Content("~/System/SecurityOrgList") + "\">安全厂商管理</a> >> 编辑安全厂商信息";
                ViewBag.sec = new HBHtmlDAL().GetSecId(id);
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
        /// <summary>
        /// 编辑安全厂商
        /// 创建人：邓佳训
        /// 创建时间：2017-07-03
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditOrg(FormCollection form)
        {
            try
            {
                SecurityOrg sec = new SecurityOrg();
                HBHtmlDAL putDal = new HBHtmlDAL();
                sec.SECURITY_SOFTWARE_ORGCODE = form["txtOrgcode"];
                sec.ID = Convert.ToInt32(form["Hid"]);
                sec.SECURITY_SOFTWARE_ORG_ADDRESS = form["txtAddress"];
                sec.SECURITY_SOFTWARE_ORGNAME = form["txtName"];
                sec.CONTACTOR = form["txtContact"];
                sec.CONTACTOR_TEL = form["txtPhone"];
                sec.CONTACTOR_MAIL = form["txtEmail"];
                if (putDal.GetAQCountId(sec.SECURITY_SOFTWARE_ORGCODE, sec.ID) > 0)
                {
                    return Content("<script>alert('该厂商组织机构代码已经存在,请重新输入!');history.go(-1);</script>");
                }
                int result = putDal.UpadteSec(sec);
                if (result > 0)
                {
                    //重新获取安全厂商缓存
                    WebCommon.GetSecurityList(true);
                    log.What = "编辑安全厂商信息，厂商名称：" + sec.SECURITY_SOFTWARE_ORGNAME + "，厂商组织机构代码：" + sec.SECURITY_SOFTWARE_ORGCODE;
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('保存成功');location.replace('" + Url.Content("~/System/SecurityOrgList") + "');</script>");
                }
                else
                {
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","SystemController.EditOrg(FormCollection form)[HttpPost]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }
        /// <summary>
        /// 删除安全厂商
        /// 创建人：邓佳训
        /// 创建时间：2017-07-03
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DelOrg(int Id)
        {
            try
            {
                HBHtmlDAL putDal = new HBHtmlDAL();
                int num = putDal.GetOrgCountByID(Id);//获取场所信息
                if (num > 0)
                {
                    return Content("<script>alert('该安全厂商下还存在 " + num + " 个场所，请先删除场所');history.go(-1);</script>");
                }
                int result = putDal.DeleteSec(Id);//删除没有场所的安全厂商
                if (result > 0)
                {
                    //重新获取安全厂商缓存
                    WebCommon.GetSecurityList(true);
                    log.What = "删除安全厂商信息成功，ID：" + Id;
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('保存成功');location.replace('" + Url.Content("~/System/SecurityOrgList") + "');</script>");
                }
                else
                {
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","SystemController.DelOrg(int Id)[HttpGet]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }

        /// <summary>
        /// /// <summary>
        /// 安全厂商详细
        /// 创建人：张团勃
        /// 创建时间：2017-07-018
        /// </summary>
        /// <returns></returns>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DetOrg(int id)
        {
            try
            {
                ViewBag.lishow = lishow;
                ViewBag.ashow = "Org";
                ViewBag.LfetShow = "系统管理";
                ViewBag.location = "所在位置：系统管理 >> <a href=\"" + Url.Content("~/System/SecurityOrgList") + "\">安全厂商管理</a> >> 安全厂商详细信息";
                ViewBag.sec = new HBHtmlDAL().GetSecId(id);
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
        [HttpGet]
        public ActionResult NumSelList(int? pageIndex, int id, string startTime, string endTime, string dataType, string authType)
        {
            try
            {
                var javaResJson=string.Empty;
                dataType = "1";
                if (!string.IsNullOrEmpty(dataType))
                {
                    pageIndex = pageIndex == null ? 0 : pageIndex;
                    dataType = ChangeValue.RefauthTypeStr(dataType);
                    MergeWar.JavaServer.SCPInquireServiceClient java = new MergeWar.JavaServer.SCPInquireServiceClient();
                    var parameter = new
                    {
                        curPage = pageIndex,
                        factoryId = 1,
                        startTime = startTime,
                        endTime = endTime,
                        dataType = dataType,
                        authType = authType
                    };
                    try
                    {
                        javaResJson = Newtonsoft.Json.JsonConvert.SerializeObject(parameter);
                        javaResJson = java.queryTerminalManageList(javaResJson);
                        var Json = Newtonsoft.Json.Linq.JObject.Parse(javaResJson);
                        if (Convert.ToInt32(Json["state"]) == 1)
                        {
                            Logger.writeLog(Json.ToString());
                            return Content("<script>alert('暂无数据可查!');</script>");
                        }
                        ViewBag.count = Convert.ToInt32(Json["numFound"].ToString());
                        ViewBag.pl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TermianlAccessLog>>(Json["terminalList"].ToString());
                    }
                    catch (Exception solr)
                    {
                        Logger.ErrorLog(solr, null);
                        return Content("<script>alert('服务器开小差啦!');</script>");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, null);
                throw;
            }
        }
        #endregion 
    }
}
