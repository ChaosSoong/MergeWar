using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HCZZ.ModeDB;
using HCZZ.DAL;
using HCZZ.AppCode;
using Common;
using System.Web.Script.Serialization;
using System.Data;
using HCZZ.Models;
using MergeWar.Filter;

namespace HCZZ.Controllers
{
    [SystemAutherFilter]
    public class LocationController : Controller
    {
        OPLog log = new OPLog();
        private string lishow = "CSGL";
        public ActionResult Index()
        {
            ViewBag.LfetShow = "场所管理";
            return View();
        }

        #region 场所管理
        #region 场所管理数据列表 上网场所名称,APmac,场所详细地址,备案时间范围，省市区，
        [HttpGet]
        public ActionResult LocationList(int? Id, string txtNetbar_Wacode, string txtAP_MAC, string txtStartTime, string txtEndTime, string txtSite_Address, string selProvince, string selCity, string selArea, string selPolice)
        {
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Location/LocationList") + "'>场所管理</a>>><a href='" + Url.Content("~/Location/LocationList") + "'>场所列表</a>";
            ViewBag.LfetShow = "场所管理";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "local";
            ViewBag.Id = Id;
            try
            {
                selProvince = selProvince ?? "0";
                selCity = selCity ?? "0";
                selArea = selArea ?? "0";
                selPolice = selPolice ?? "0";

                int[] userType = ChangeValue.GetUserType();
                //UserInfo user = (UserInfo)Session["userInfo"];
                //string proId = "0";
                //if (userType[0] != 1 && userType[0] != 7)
                //    proId = user.ProID.ToString();
                Dictionary<string, string> dic = new Dictionary<string, string>() 
                { 
                    {"pageIndex",(Id??1).ToString()},
                    {"pageSize","5"},
                    {"txtNetbar_Wacode",txtNetbar_Wacode},
                    {"txtSite_Address",txtSite_Address},
                    {"UserType",userType[0].ToString()},
                    {"AreaId",userType[1].ToString()},
                    {"AP_MAC",txtAP_MAC},
                    {"selProvince",selProvince},
                    {"txtStartTime",txtStartTime},
                    {"txtEndTime",txtEndTime},
                    {"selCity",selCity},
                    {"selArea",selArea},
                    {"selPolice",selPolice},
                };
                ViewBag.dic = dic;
                ViewBag.pl = new LocationDAL().GetList(dic);

                if (!string.IsNullOrEmpty(txtNetbar_Wacode)) log.What += "上网服务场所编码：" + txtNetbar_Wacode;
                if (!string.IsNullOrEmpty(txtSite_Address)) log.What += "场所详细地址：" + txtSite_Address;
                if (!string.IsNullOrEmpty(txtAP_MAC)) log.What += "APMAC：" + txtAP_MAC;
                if (txtStartTime != "0") log.What += "开始搜索时间范围:" + txtStartTime;
                if (txtEndTime != "0") log.What += "终止搜索时间范围:" + txtEndTime;
                if (selProvince != "0") log.What += " 省Id：" + selProvince;
                if (selCity != "0") log.What += " 市Id：" + selCity;
                if (selArea != "0") log.What += " 区Id：" + selArea;
                if (selPolice != "0") log.What += " 派出所Id：" + selPolice;
                if (!string.IsNullOrEmpty(log.What)) { log.What = "场所列表查询，" + log.What; new OPLogDAL().InsertLog(log); }
            }
            catch (System.Data.SqlClient.SqlException sql)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
                Logger.ErrorLog(sql, new Dictionary<string, string>()
                {
                    {"Function","LocationController.List(int? Id, string txtNetbar_Wacode, string txtPlace_Name, string txtSite_Address, string txtLaw_Principal_Name, string txtRelationship_Account, string selNetsite_Type, string selAccess_Type, string selStatus, string selBusiness_Nature, string selOperator_Net, string txtBeginTime, string txtEndTime, string selApfwStatus, string selApsjStatus, string txtAP_MAC, string selVerified, string txtOutIpAddress)[HttpGet]"}
                });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.List(int? Id, string txtNetbar_Wacode, string txtPlace_Name, string txtSite_Address, string txtLaw_Principal_Name, string txtRelationship_Account, string selNetsite_Type, string selAccess_Type, string selStatus, string selBusiness_Nature, string selOperator_Net, string txtBeginTime, string txtEndTime, string selApfwStatus, string selApsjStatus, string txtAP_MAC, string selVerified, string txtOutIpAddress)[HttpGet]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }
        #endregion

        #region  添加场所
        [HttpGet]
        public ActionResult AddLocation()
        {
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Location/LocationList") + "'>场所管理</a>>><a href='" + Url.Content("~/Location/AddLocation") + "'>添加场所</a>";
            ViewBag.LfetShow = "场所管理";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "local";
            ViewBag.dic = new LocationDAL().GetSecurityList();  
            return View();
        }
        [HttpPost]
        public ActionResult AddLocation(FormCollection form)
        {
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Location/LocationList") + "'>场所管理</a>>><a href='" + Url.Content("~/Location/AddLocation") + "'>添加场所</a>";
            ViewBag.lishow = lishow;
            ViewBag.LfetShow = "场所管理";
            try
            {
                Loc_NetBarInfo NetBarInfo = new Loc_NetBarInfo();
                NetBarInfo.Createtime = DateTime.Now;
                NetBarInfo.PLACE_NAME = form["txtPLACE_NAME"];  //上网场所服务名称
                NetBarInfo.BUSINESS_NATURE = form["selBusiness_Nature"]; //场所经营性质
                NetBarInfo.LAW_PRINCIPAL_NAME = form["txtUserName"];//场所经营法人
                NetBarInfo.LAW_PRINCIPAL_CERTIFICATE_TYPE = form["txtCertifiID"];//有效证件类型
                NetBarInfo.START_TIME = form["txtSTART_TIME"];//营业开始时间
                NetBarInfo.ProID = Convert.ToInt32(form["selProvince"]);        //所在区域-省-市-区-警区
                NetBarInfo.CityID = Convert.ToInt32(form["selCity"]);
                NetBarInfo.Aid = Convert.ToInt32(form["SelArea"]);
                NetBarInfo.Sid = Convert.ToInt32(form["selPolice"]);
                NetBarInfo.Verified = 1;//审核状态
                NetBarInfo.Valid = 1;//是否删除
                NetBarInfo.ACCESS_TYPE = Convert.ToInt32(form["SelConnectType"]);//场所网络接入方式
                NetBarInfo.Statis = Convert.ToInt32(form["radStatis"]);//场所状态
                NetBarInfo.NETSITE_TYPE = Convert.ToInt32(form["SelNETSITE_TYPE"]);//场所类型
                NetBarInfo.SITE_ADDRESS = form["txtSITE_ADDRESS"];//详细地址
                NetBarInfo.RELATIONSHIP_ACCOUNT = form["txtMobile"];//联系方式
                NetBarInfo.LAW_PRINCIPAL_CERTIFICATE_ID = form["txtCERTIFICATE_ID"];//有效证件号码
                NetBarInfo.END_TIME = form["txtEND_TIME"];//营业结束时间
                // 数据来源
                NetBarInfo.SecId = Convert.ToInt32(form["SelMakeType"]);//安全厂商ID
                NetBarInfo.OPERATOR_NET = form["SelServiceBusines"];//网络接入服务商
                NetBarInfo.CODE_ALLOCATION_ORGANIZATION = (NetBarInfo.MakeType == 8 ? (WebCommon.RefSecurityList().Where(m => m.SECURITY_SOFTWARE_ORGNAME.Contains("舜游")).First().SECURITY_SOFTWARE_ORGCODE) : "779852855");//安全厂商组织机构代码
                NetBarInfo.Service_code = 0;//流水号
                //NetBarInfo.NETBAR_WACODE = "";//上网服务场所编码
                NetBarInfo.IsUpdate_NETBAR_WACODE = 0;//是否更新场所编码
                if (!string.IsNullOrEmpty(form["txtItude"]))//经纬度
                {
                    NetBarInfo.LONGITUDE = string.Format("{0:0.000000}", form["txtItude"].ToString().Split(',')[0]);
                    NetBarInfo.LATITUDE = string.Format("{0:0.000000}", form["txtItude"].ToString().Split(',')[1]);
                }
                else
                {
                    NetBarInfo.LONGITUDE = "";
                    NetBarInfo.LATITUDE = "";
                }
                int LocaId = new LocationDAL().InsertLocation(NetBarInfo);
                if (LocaId > 0)
                {
                    WebCommon.GetCacheLoca(true);
                    log.What = "添加场所信息，场所名称：" + NetBarInfo.PLACE_NAME + "，ID:" + LocaId;
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>if (confirm('保存成功，是否继续添加AP设备')) {window.location.replace('" + Url.Content("~/Location/AddAPMac/" + LocaId) + "');} else { window.location.replace('" + Url.Content("~/Location/LocationList") + "');}</script>");
                }
                else
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
            }
            catch (System.Data.SqlClient.SqlException sql)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
                Logger.ErrorLog(sql, new Dictionary<string, string>()
                {
                    {"Function","LocationController.AddLocation(FormCollection form)[HttpPost]"}
                });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.AddLocation(FormCollection form)[HttpPost]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }
        #endregion

        #region 场所详情
        [HttpGet]
        public ActionResult DetailLoca(int Id)
        {
            try
            {
                ViewBag.location = "所在位置：<a href='" + Url.Content("~/Location/LocationList") + "'>场所管理</a> >> 场所详细信息";
                ViewBag.lishow = lishow;
                ViewBag.LfetShow = "场所管理";
                ViewBag.ashow = "local";
                LocationDAL ldal = new LocationDAL();
                ViewBag.loca = ldal.GetNetBarModelById(Id);
                ViewBag.dt = ldal.GetVerifiyNetBarById(Id);
            }
            catch (System.Data.SqlClient.SqlException sql)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
                Logger.ErrorLog(sql, new Dictionary<string, string>()
                {
                    {"Function","LocationController.DetailLoca(int Id)[HttpGet]"}
                });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.DetailLoca(int Id)[HttpGet]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }
        [HttpPost]
        public ActionResult DetailLoca(FormCollection form)
        {
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Location/LocationList") + "'>场所管理</a> >> 场所详细信息";
            ViewBag.LfetShow = "场所管理";
            ViewBag.lishow = lishow;
            try
            {
                int id = Convert.ToInt32(form["HLid"]);
                int Verified = Convert.ToInt32(form["radVerified"]);
                LocationDAL locadal = new LocationDAL();
                Loc_NetBarInfo loca = locadal.GetLocaById(id, "");
                int IsUpdate_NETBAR_WACODE = string.IsNullOrEmpty(loca.NETBAR_WACODE) ? 1 : 0;
                int result = locadal.UpdateLocaVerifiedById(Verified, id, IsUpdate_NETBAR_WACODE);
                if (result > 0)
                {
                    if (Verified == 3)
                        locadal.CopyVerifiyNetbarToNetbarById(id);
                    log.What = "审核场所信息，场所名称：" + loca.PLACE_NAME + "，上网服务场所编码：" + loca.NETBAR_WACODE + "，审核状态：" + (Verified == 3 ? "通过" : "不通过");
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('保存成功'); window.location.replace('" + Url.Content("~/Location/LocationList/") + "');</script>");
                }
                else
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
            }
            catch (System.Data.SqlClient.SqlException sql)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
                Logger.ErrorLog(sql, new Dictionary<string, string>()
                {
                    {"Function","LocationController.DetailLoca(FormCollection form)[HttpPost]"}
                });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.DetailLoca(FormCollection form)[HttpPost]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }

        #endregion

        #region 编辑场所
        [HttpGet]
        public ActionResult EditLocation(int Id)
        {
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Location/LocationList") + "'>场所管理</a> >> 编辑场所信息";
            ViewBag.LfetShow = "场所管理";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "local";
            ViewBag.dic = new LocationDAL().GetSecurityList();
            try
            {
                LocationDAL ldal = new LocationDAL();
                Loc_NetBarInfo loca = ldal.GetLocaById(Id, "");
                DataTable dt = ldal.GetVerifiyNetBarById(Id);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());
                Verify_NetBarInfo vnetbar = new DataSetConvert(ds).Get_SingleModel<Verify_NetBarInfo>();
                ViewBag.loca = loca;
                if (vnetbar == null)
                    ViewBag.vnetbar = "";
                else
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    vnetbar.certifiName = ChangeValue.GetCertifiCateList().Where(m => m.Key == vnetbar.LAW_PRINCIPAL_CERTIFICATE_TYPE).First().Value;
                    ViewBag.vnetbar = ser.Serialize(vnetbar);
                }
            }
            catch (System.Data.SqlClient.SqlException sql)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
                Logger.ErrorLog(sql, new Dictionary<string, string>()
                {
                    {"Function","LocationController.EditLocation(int Id)[HttpGet]"}
                });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.EditLocation(int Id)[HttpGet]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditLocation(FormCollection form)
        {
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Location/LocationList") + "'>场所管理</a> >> 编辑场所信息";
            ViewBag.LfetShow = "场所管理";
            ViewBag.lishow = lishow;
            try
            {
                UserInfo user = (UserInfo)Session["userInfo"];
                Loc_NetBarInfo loca = new Loc_NetBarInfo();
                loca.ACCESS_TYPE = string.IsNullOrEmpty(form["SelConnectType"]) ? 0 : Convert.ToInt32(form["SelConnectType"]);
                //loca.ACSSES_IP = form["txtOutIpAddress"].ToString();
                loca.BUSINESS_NATURE = form["selBusiness_Nature"].ToString();
                loca.END_TIME = form["txtEND_TIME"].ToString();
                loca.LAW_PRINCIPAL_CERTIFICATE_ID = form["txtCERTIFICATE_ID"].ToString();
                loca.LAW_PRINCIPAL_CERTIFICATE_TYPE = form["txtCertifiID"].ToString();
                loca.LAW_PRINCIPAL_NAME = form["txtUserName"].ToString();
                loca.NETSITE_TYPE = Convert.ToInt32(form["SelNETSITE_TYPE"]);
                loca.OPERATOR_NET = form["SelServiceBusines"].ToString();
                loca.PLACE_NAME = form["txtPLACE_NAME"].ToString();
                loca.RELATIONSHIP_ACCOUNT = form["txtMobile"].ToString();
                loca.SITE_ADDRESS = form["txtSITE_ADDRESS"].ToString();
                loca.START_TIME = form["txtSTART_TIME"].ToString();
                loca.Verified = Convert.ToInt32(form["radVerified"]);
                loca.ProID = Convert.ToInt32(form["selProvince"]);
                loca.CityID = Convert.ToInt32(form["selCity"]);
                loca.Aid = Convert.ToInt32(form["SelArea"]);
                loca.Pid = Convert.ToInt32(form["selPolice"]);
                loca.Sid = Convert.ToInt32(form["selscenic"]);
                loca.MakeType = Convert.ToInt32(form["SelMakeType"]);
                loca.SecId = Convert.ToInt32(form["SelMakeType"]);
                loca.CODE_ALLOCATION_ORGANIZATION = (loca.MakeType == 8 ? (WebCommon.RefSecurityList().Where(m => m.SECURITY_SOFTWARE_ORGNAME.Contains("舜游")).First().SECURITY_SOFTWARE_ORGCODE) : "779852855");
                loca.Statis = Convert.ToInt32(form["radStatis"]);
                loca.ID = Convert.ToInt32(form["Hid"]);
                loca.TableKey = "";

                LocationDAL locadal = new LocationDAL();

                //if (Convert.ToInt32(form["HLocaType"]) != loca.NETSITE_TYPE || Convert.ToInt32(form["HAid"]) != loca.Aid)
                //{
                //    loca.Service_code = locadal.GetMaxServiceCode(loca.Aid, loca.NETSITE_TYPE, loca.TableKey);
                //    AreaInfo area = new PoliceDAL().GetAreaInfoById(loca.Aid, 2);
                //    loca.NETBAR_WACODE = ChangeValue.RefNetbarWacode(area.City_code, loca.NETSITE_TYPE, loca.Service_code);
                //}
                //else
                //{
                if (string.IsNullOrEmpty(form["HNETBAR_WACODE"]))
                {
                    /*
                     * 此处场所编码生成交给触发器来做，项目中注释掉
                     */

                    //loca.Service_code = locadal.GetMaxServiceCode(loca.Aid, loca.NETSITE_TYPE, loca.TableKey);
                    //AreaInfo area = new PoliceDAL().GetAreaInfoById(loca.Aid, 1);
                    //loca.NETBAR_WACODE = ChangeValue.RefNetbarWacode(area.PName, area.City_code, loca.NETSITE_TYPE, loca.Service_code);
                    loca.Service_code = 0;
                    loca.IsUpdate_NETBAR_WACODE = 1;
                    loca.NETBAR_WACODE = "";
                }
                else
                {
                    loca.Service_code = Convert.ToInt32(form["HService_Code"]);
                    loca.NETBAR_WACODE = form["HNETBAR_WACODE"].ToString();
                    loca.IsUpdate_NETBAR_WACODE = 0;
                }
                //}

                if (!string.IsNullOrEmpty(form["txtItude"]))
                {
                    string[] strSp = form["txtItude"].ToString().Split(',');
                    loca.LONGITUDE = string.Format("{0:0.000000}", Convert.ToDouble(strSp[0]));
                    loca.LATITUDE = string.Format("{0:0.000000}", Convert.ToDouble(strSp[1]));
                }
                else
                {
                    loca.LATITUDE = "";
                    loca.LONGITUDE = "";
                }

                if (locadal.UpdateLoca(loca, (user.Type == 1 || user.Type == 8 ? true : false)))
                {
                    WebCommon.GetCacheLoca(true);
                    log.What = "编辑场所信息，场所名称：" + loca.PLACE_NAME + "，ID:" + loca.ID;
                    new OPLogDAL().InsertLog(log);

                    if (!string.IsNullOrEmpty(form["ReturnUrl"]))
                        return Content("<script>alert('保存成功');window.location.replace('" + form["ReturnUrl"] + "');</script>");
                    else
                        return Content("<script>alert('保存成功');window.location.replace('" + Url.Content("~/Location/LocationList") + "');</script>");
                }
                else
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
            }
            catch (System.Data.SqlClient.SqlException sqlex)
            {
                Logger.ErrorLog(sqlex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.EditLocation(FormCollection form)[HttpPost]"}
                });
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.EditLocation(FormCollection form)[HttpPost]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }
        #endregion

        #region  删除场所
        [HttpGet]
        public ActionResult DelLocation(int id)
        {
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Location/DelLocation") + "'>场所管理</a> >> 删除场所";
            ViewBag.LfetShow = "场所管理";
            try
            {
                Loc_NetBarInfo DevInfo = new Loc_NetBarInfo();
                DevInfo = new LocationDAL().GetNetBarModelById(id);//获取实体
                int result = new LocationDAL().UpdateNetBarById(id);//更新是否删除状态
                if (result > 0)
                {
                    new LocationDAL().UpdateDevById(DevInfo.ID);
                    WebCommon.GetCacheLoca(true);
                    WebCommon.GetCacheMacAllList(true);
                    log.What = "删除场所信息，场所名称：" + DevInfo.PLACE_NAME + "，ID:" + DevInfo.ID;
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('删除成功'); window.location.replace('" + Url.Content("~/Location/LocationList") + "');</script>");
                }
                else
                    return Content("<script>alert('删除失败');history.go(-1);</script>");
            }
            catch (System.Data.SqlClient.SqlException sql)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
                Logger.ErrorLog(sql, new Dictionary<string, string>()
                {
                    {"Function","LocationController.DelLoca(int Id)[HttpGet]"}
                });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.DelLoca(int Id)[HttpGet]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return Redirect("~/Location/LocationList");
        }
        #endregion
        #endregion

        #region 地图标记
        public ActionResult LocaShowMap()
        {
            ViewBag.LfetShow = "场所管理";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Location/LocationList") + "'>场所管理</a>>><a href='" + Url.Content("~/Location/LocaShowMap") + "'>地图标记</a>";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "map";
            return View();
        }

        #endregion

        #region 未知设备
        public ActionResult UnDeviceList(int? pageIndex, string txtAP_MAC, string txtStartTime, string txtEndTime, string AreaName)
        {
            ViewBag.LfetShow = "场所管理";
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Location/LocationList") + "'>场所管理</a>>><a href='" + Url.Content("~/Location/UnDeviceList") + "'>未知设备</a>";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "undev";
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>() 
                { 
                    {"pageIndex",(pageIndex??1).ToString()},
                    {"pageSize","10"},
                    {"AP_MAC",txtAP_MAC},
                    {"StartTime",txtStartTime},
                    {"EndTime",txtEndTime},
                    {"AreaName",AreaName}
                };
                ViewBag.pl = new LocationDAL().GetDeviceList(dic);
                ViewBag.dic = dic;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.UnDeviceList(int? pageIndex, string txtAP_MAC, string txtStartTime, string txtEndTime)[HttpGet]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }

        [HttpGet]
        public ActionResult DeleteUnDev(int Id)
        {
            try
            {
                int result = new LocationDAL().delUnDev(Id);
                if (result > 0)
                {
                    log.What = "删除未知场所，Id：" + Id;
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('删除成功');location.replace('" + Url.Content("~/Location/UnDeviceList/1") + "');</script>");
                }
                else
                    return Content("<script>alert('删除失败');history.go(-1);</script>");
            }
            catch (System.Data.SqlClient.SqlException)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.DeleteUnDev(int Id)[HttpGet]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }
        #endregion

        #region 设备管理
        #region 设备列表
        public ActionResult DevInfoList(int Id, string key, string txtAP_MAC_ID, string txtAP_MAC, string PotType)
        {
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Location/LocationList") + "'>场所管理</a>>><a href='" + Url.Content("~/Location/DevInfoList") + "'>设备列表</a>";
            ViewBag.LfetShow = "场所管理";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "local";
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>() 
                {
                    {"pageIndex",key??"1"},
                    {"pageSize","10"},
                    {"Lid",Id.ToString()},
                    {"txtAP_MAC_ID",txtAP_MAC_ID},
                    {"txtAP_MAC",txtAP_MAC},
                    {"PotType",string.IsNullOrEmpty(PotType)?"0":PotType }
                };
                ViewBag.pl = new LocationDAL().GetAPMACList(dic);
                ViewBag.dic = dic;

                if (!string.IsNullOrEmpty(txtAP_MAC_ID)) log.What += "AP设备编码：" + txtAP_MAC_ID;
                if (!string.IsNullOrEmpty(txtAP_MAC)) log.What += "AP设备MAC地址：" + txtAP_MAC;
                if (!string.IsNullOrEmpty(log.What)) { log.What = "AP设备列表查询，" + log.What; new OPLogDAL().InsertLog(log); }
            }
            catch (System.Data.SqlClient.SqlException sql)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
                Logger.ErrorLog(sql, new Dictionary<string, string>()
                {
                    {"Function","LocationController.DevInfoList(int Id, string key, string txtAP_MAC_ID, string txtAP_MAC)[HttpGet]"}
                });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.DevInfoList(int Id, string key, string txtAP_MAC_ID, string txtAP_MAC)[HttpGet]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }
        #endregion

        #region 添加设备
        [HttpGet]
        public ActionResult AddAPMac(int Id)
        {
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Location/LocationList") + "'>场所管理</a> >> <a href='" + Url.Content("~/Location/DevInfoList/" + Id) + "'>AP设备管理</a> >> 添加AP设备信息";
            ViewBag.LfetShow = "场所管理";
            ViewBag.Lid = Id;
            ViewBag.lishow = lishow;
            ViewBag.ashow = "local";
            return View();
        }

        [HttpPost]
        public ActionResult AddAPMac(FormCollection form)
        {
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Location/LocationList") + "'>场所管理</a> >> <a href='" + Url.Content("~/Location/DevInfoList/") + "'>AP设备管理</a> >> 添加AP设备信息";
            ViewBag.LfetShow = "场所管理";
            ViewBag.lishow = lishow;
            try
            {
                MacInfo mac = new MacInfo();
                LocationDAL locadal = new LocationDAL();
                MacDAL macdal = new MacDAL();
                mac.NETBAR_ID = Convert.ToInt32(form["HLid"]);

                Loc_NetBarInfo loca = locadal.GetLocaById(mac.NETBAR_ID, "");
                mac.SECURITY_SOFTWARE_ORGCODE = loca.CODE_ALLOCATION_ORGANIZATION;
                mac.ID = Convert.ToInt32(form["HMid"] == "" ? "0" : form["HMid"]);
                mac.APType = Convert.ToInt32(form["radAPType"]);
                mac.APName = form["txtAPName"].ToString();

                mac.ModeType = Convert.ToInt32(form["selModeType"]);
                if (mac.ModeType == 2)
                    mac.IsTrial = form["ckIstrial"] == null ? 1 : Convert.ToInt32(form["ckIstrial"]);
                //mac.LogCapture = ChangeValue.ReturnUintValue(form["ckLogCapture"]);
                mac.COLLECTION_EQUIPMENT_ID = form["txtCOLLECTION_EQUIPMENT_ID"];
                mac.LogCapture = 7;
                mac.supplier = Convert.ToInt32(form["selsupplier"]);
                mac.ProjectType = Convert.ToInt32(form["selProjectType"]);
                mac.CasesType = Convert.ToInt32(form["selCasesType"]);
                mac.FenceOffTime = Convert.ToInt32(form["txtFenceOffTime"]);
                mac.ForcedOfflineTime = Convert.ToInt32(form["txtForcedOfflineTime"]);
                mac.Channel = Convert.ToInt32(form["txtChannel"]);
                mac.IsReboot = Convert.ToInt32(form["radIsReboot"]);
                mac.AP_ACSSES_IP = form["txtACSSES_IP"];
                if (mac.ProjectType == 7)
                    mac.V3CID = form["txt3CID"];

                if (mac.APType == 1)
                {
                    mac.FLOOR = form["txtFloor"].ToString();
                    string[] strSp = form["txtItude"].ToString().Split(',');
                    mac.LONGITUDE = string.Format("{0:0.000000}", Convert.ToDouble(strSp[0]));
                    mac.LATITUDE = string.Format("{0:0.000000}", Convert.ToDouble(strSp[1]));
                    mac.CAR_CODE = "";
                    mac.SUBWAY_COMPARTMENT_NUMBER = "";
                    mac.SUBWAY_LINE_INFO = "";
                    mac.SUBWAY_STATION = "";
                    mac.SUBWAY_VEHICLE_INFO = "";
                }
                else
                {
                    mac.FLOOR = "";
                    mac.LONGITUDE = "";
                    mac.LATITUDE = "";
                    mac.CAR_CODE = form["txtCar_Code"].ToString();
                    mac.SUBWAY_COMPARTMENT_NUMBER = form["txtCompartment"].ToString();
                    mac.SUBWAY_LINE_INFO = form["txtLine_Info"].ToString();
                    mac.SUBWAY_STATION = form["txtStation"].ToString();
                    mac.SUBWAY_VEHICLE_INFO = form["txtVehicle_Info"].ToString();
                }
                int result = macdal.UpdateMacById(mac);
                if (result > 0)
                {
                    locadal.UpdateAPNumById(mac.NETBAR_ID, "+", "");
                    //int syn_Result = locadal.UpdateOldLoca(mac.ID, "");

                    //舜游中心禁用AP信息同步
                    //JavaScriptSerializer ser = new JavaScriptSerializer();
                    //new HttpHelps().PostSend(1, 2, ser.Serialize(new Json_DevInfo() { MAC = form["txtDeviceMac"], APName = mac.APName }));

                    WebCommon.GetCacheMac(true);
                    WebCommon.GetCacheMacAllList(true);

                    /*修改缓存中场所信息*/
                    List<Loc_NetBarInfo> Localist = null;
                    if (HttpContext.Cache["ShowCacheLoca"] == null)
                        WebCommon.GetCacheLoca();
                    else
                    {
                        Localist = (List<Loc_NetBarInfo>)HttpContext.Cache["ShowCacheLoca"];
                        Localist.Where(m => m.ID == mac.NETBAR_ID).First().IsVoid = 1;
                        HttpContext.Cache.Insert("ShowCacheLoca", Localist, null, DateTime.Now.AddHours(6), TimeSpan.Zero);
                    }

                    log.What = "添加AP设备信息，AP设备名称：" + mac.APName + "，AP设备MAC地址：" + mac.AP_MAC;
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('保存成功'); window.location.replace('" + Url.Content("~/Location/DevInfoList/" + mac.NETBAR_ID) + "');</script>");
                }
                else
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
            }
            catch (System.Data.SqlClient.SqlException sql)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
                Logger.ErrorLog(sql, new Dictionary<string, string>()
                {
                    {"Function","APMacController.AddAPMac(FormCollection form)[HttpPost]"}
                });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","APMacController.AddAPMac(FormCollection form)[HttpPost]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }

        #endregion

        #region 设备详情
        [HttpGet]
        public ActionResult DetailAPMac(int Id, string PotType)
        {
            try
            {
                PotType = string.IsNullOrEmpty(PotType) ? "0" : PotType;
                ViewBag.PotType = Convert.ToInt32(PotType);
                MacInfo mac = new MacDAL().GetMACInfoById(Id, "");
                ViewBag.location = "所在位置：<a href='" + Url.Content("~/Location/LocationList") + "'>场所管理</a> >> <a href='" + Url.Content("~/Location/DevInfoList/" + mac.NETBAR_ID) + "'>AP设备管理</a> >> AP设备详细信息";
                ViewBag.LfetShow = "场所管理";
                ViewBag.lishow = lishow;
                ViewBag.ashow = "local";
                ViewBag.mac = mac;
            }
            catch (System.Data.SqlClient.SqlException sql)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
                Logger.ErrorLog(sql, new Dictionary<string, string>()
                {
                    {"Function","APMacController.DetailAPMac(int Id)[HttpGet]"}
                });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","APMacController.DetailAPMac(int Id)[HttpGet]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }

        [HttpPost]
        public ActionResult DetailAPMac(FormCollection form)
        {
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Location/LocationList") + "'>场所管理</a> >> <a href='" + Url.Content("~/Location/DevInfoList/") + "'>AP设备管理</a> >> AP设备详细信息";
            ViewBag.LfetShow = "场所管理";
            ViewBag.lishow = lishow;
            try
            {
                int id = Convert.ToInt32(form["HMid"]);
                int Verified = Convert.ToInt32(form["radVerified"]);
                int radDevStatis = Convert.ToInt32(form["radDevStatis"]);
                int PotType = Convert.ToInt32(form["radPotType"]);
                MacDAL mdal = new MacDAL();
                MacInfo mac = mdal.GetMACInfoById(id, "");
                int result = mdal.UpdateMacVerifiedById(Verified, id, radDevStatis, "", PotType);
                if (PotType != Convert.ToInt32(form["HPotType"]))
                    result = mdal.UpdateMacCheckTimeById(id);
                if (result > 0)
                {
                    log.What = "审核AP设备信息，AP设备MAC地址：" + mac.AP_MAC + "，审核状态：" + (Verified == 3 ? "通过" : "不通过");
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('保存成功'); window.location.replace('" + Url.Content("~/Location/DevInfoList/" + mac.NETBAR_ID + "?PotType=" + TempData["PotType"]) + "');</script>");
                }
                else
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
            }
            catch (System.Data.SqlClient.SqlException sql)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
                Logger.ErrorLog(sql, new Dictionary<string, string>()
                {
                    {"Function","APMacController.DetailAPMac(FormCollection form)[HttpPost]"}
                });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","APMacController.DetailAPMac(FormCollection form)[HttpPost]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }
        #endregion

        #region 编辑设备
        [HttpGet]
        public ActionResult EditAPMac(int Id)
        {
            ViewBag.location = "所在位置：<a href='" + Url.Content("~/Location/LocationList") + "'>场所管理</a> >> <a href='" + Url.Content("~/Location/DevInfoList/" + Id) + "'>AP设备管理</a> >> AP设备详细信息";
            ViewBag.LfetShow = "场所管理";
            ViewBag.lishow = lishow;
            ViewBag.ashow = "local";
            try
            {
                MacInfo mac = new MacDAL().GetMACInfoById(Id, "");
                ViewBag.mac = mac;
            }
            catch (System.Data.SqlClient.SqlException sql)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
                Logger.ErrorLog(sql, new Dictionary<string, string>()
                {
                    {"Function","LocationController.DetailAPMac(int Id)[HttpGet]"}
                });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.DetailAPMac(int Id)[HttpGet]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }

            return View();
        }

        [HttpPost]
        public ActionResult EditAPMac(FormCollection form)
        {
            ViewBag.LfetShow = "场所管理";
            ViewBag.lishow = lishow;
            try
            {
                MacInfo mac = new MacInfo();
                LocationDAL locadal = new LocationDAL();
                MacDAL macdal = new MacDAL();
                mac.NETBAR_ID = Convert.ToInt32(form["HLid"]);

                Loc_NetBarInfo loca = locadal.GetLocaById(mac.NETBAR_ID, "");

                loca.SECURITY_SOFTWARE_ORGCODE = loca.SECURITY_SOFTWARE_ORGCODE;
                mac.ID = Convert.ToInt32(form["HMid"]);
                mac.APType = Convert.ToInt32(form["radAPType"]);
                mac.APName = form["txtAPName"].ToString();
                mac.COLLECTION_EQUIPMENT_ID = form["txtCOLLECTION_EQUIPMENT_ID"];
                mac.ModeType = Convert.ToInt32(form["selModeType"]);
                mac.IsTrial = Convert.ToInt32(form["ckIstrial"]);
                //mac.LogCapture = ChangeValue.ReturnUintValue(form["ckLogCapture"]);
                mac.LogCapture = 7;
                mac.supplier = Convert.ToInt32(form["selsupplier"]);
                mac.ProjectType = Convert.ToInt32(form["selProjectType"]);
                mac.CasesType = Convert.ToInt32(form["selCasesType"]);
                mac.FenceOffTime = Convert.ToInt32(form["txtFenceOffTime"]);
                mac.ForcedOfflineTime = Convert.ToInt32(form["txtForcedOfflineTime"]);
                mac.Channel = Convert.ToInt32(form["txtChannel"]);
                mac.IsReboot = Convert.ToInt32(form["radIsReboot"]);
                mac.AP_ACSSES_IP = form["txtACSSES_IP"];
                if (mac.ProjectType == 7)
                    mac.V3CID = form["txt3CID"];

                if (mac.APType == 1)
                {
                    mac.FLOOR = form["txtFloor"].ToString();
                    string[] strSp = form["txtItude"].ToString().Split(',');
                    mac.LONGITUDE = string.Format("{0:0.000000}", Convert.ToDouble(strSp[0]));
                    mac.LATITUDE = string.Format("{0:0.000000}", Convert.ToDouble(strSp[1]));
                    mac.CAR_CODE = "";
                    mac.SUBWAY_COMPARTMENT_NUMBER = "";
                    mac.SUBWAY_LINE_INFO = "";
                    mac.SUBWAY_STATION = "";
                    mac.SUBWAY_VEHICLE_INFO = "";
                }
                else
                {
                    mac.FLOOR = "";
                    mac.LONGITUDE = "";
                    mac.LATITUDE = "";
                    mac.CAR_CODE = form["txtCar_Code"].ToString();
                    mac.SUBWAY_COMPARTMENT_NUMBER = form["txtCompartment"].ToString();
                    mac.SUBWAY_LINE_INFO = form["txtLine_Info"].ToString();
                    mac.SUBWAY_STATION = form["txtStation"].ToString();
                    mac.SUBWAY_VEHICLE_INFO = form["txtVehicle_Info"].ToString();
                }

                if (Convert.ToInt32(form["HOldMid"]) != mac.ID)
                    new LocationDAL().UpdateMacNULLById(Convert.ToInt32(form["HOldMid"]));
                int result = new MacDAL().UpdateMacById(mac);
                if (result > 0)
                {
                    //int syn_Result = new LocationDAL().UpdateOldLoca(mac.ID, "");
                    //if (form["txtDeviceMac"] != form["hidDeviceMac"] || form["txtAPName"] != form["hidApName"])
                    //{
                    //    //如果修改了AP的mac地址，则需要删除以前的注册设备
                    //    JavaScriptSerializer ser = new JavaScriptSerializer();
                    //    new HttpHelps().PostSend(2, 2, ser.Serialize(new Json_DevInfo() { MAC = form["txtDeviceMac"], APName = mac.APName }));
                    //    if(form["txtDeviceMac"] != form["hidDeviceMac"])
                    //        new HttpHelps().PostSend(3, 2, ser.Serialize(new Json_DevInfo() { MAC = form["hidDeviceMac"], APName = "" }));
                    //}

                    WebCommon.GetCacheMac(true);
                    WebCommon.GetCacheMacAllList(true);

                    /*修改缓存中场所信息*/
                    List<Loc_NetBarInfo> Localist = null;
                    if (HttpContext.Cache["ShowCacheLoca"] == null)
                        WebCommon.GetCacheLoca();
                    else
                    {
                        Localist = (List<Loc_NetBarInfo>)HttpContext.Cache["ShowCacheLoca"];
                        Localist.Where(m => m.ID == mac.NETBAR_ID).First().IsVoid = 1;
                        HttpContext.Cache.Insert("ShowCacheLoca", Localist, null, DateTime.Now.AddHours(6), TimeSpan.Zero);
                    }

                    log.What = "编辑AP设备信息，AP设备名称：" + mac.APName + "，AP设备MAC地址：" + mac.AP_MAC;
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('保存成功'); window.location.replace('" + Url.Content("~/Location/DevInfoList/" + mac.NETBAR_ID) + "');</script>");
                }
                else
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
            }
            catch (System.Data.SqlClient.SqlException sql)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
                Logger.ErrorLog(sql, new Dictionary<string, string>()
                {
                    {"Function","LocationController.EditAPMac(FormCollection form)[HttpPost]"}
                });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.EditAPMac(FormCollection form)[HttpPost]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }
        #endregion

        #region 删除设备
        [HttpGet]
        public ActionResult DeleteAPMac(int Id)
        {
            ViewBag.LfetShow = "场所管理";
            try
            {
                MacDAL mdal = new MacDAL();
                LocationDAL ldal = new LocationDAL();

                MacInfo mac = mdal.GetMACInfoById(Id, "");
                int result = ldal.UpdateMacNULLById(Id);
                if (result > 0)
                {
                    ldal.UpdateAPNumById(mac.NETBAR_ID, "-", "");
                    //ldal.DeleteLoca(mac.Lguid);

                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    new HttpHelps().PostSend(3, 2, ser.Serialize(new Json_DevInfo() { MAC = mac.AP_MAC, APName = mac.APName }));

                    WebCommon.GetCacheMac(true);
                    WebCommon.GetCacheMacAllList(true);

                    log.What = "删除AP设备信息，AP设备名称：" + mac.APName + "，AP设备MAC地址：" + mac.AP_MAC;
                    new OPLogDAL().InsertLog(log);
                    return Content("<script>alert('保存成功'); window.location.replace('" + Url.Content("~/Location/DevInfoList/" + mac.NETBAR_ID) + "');</script>");
                }
                else
                    return Content("<script>alert('保存失败');history.go(-1);</script>");
            }
            catch (System.Data.SqlClient.SqlException sql)
            {
                ViewBag.errscript = "alert('检索数据遇到错误，请联系管理员')";
                Logger.ErrorLog(sql, new Dictionary<string, string>()
                {
                    {"Function","LocationController.DeleteAPMac(int Id)[HttpGet]"}
                });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.DeleteAPMac(int Id)[HttpGet]"}
                });
                ViewBag.errscript = "alert('未知错误，请联系管理员')";
            }
            return View();
        }
        #endregion

        #region 用户重新认证，修改身份认证
        [HttpGet]
        public ActionResult UpdateState(int id, string strId)
        {
            try
            {
                MacDAL mdal = new MacDAL();
                LocationDAL ldal = new LocationDAL();
                int num = mdal.UpdateIsOpenById(strId, id);
                if (num > 0)
                {
                    //string[] sp = strId.Split(',');
                    //for (int i = 0; i < sp.Length; i++)
                    //{
                    //    int intId = Convert.ToInt32(sp[i]);
                    //    if (intId > 0)
                    //        ldal.UpdateOldLoca(Convert.ToInt32(sp[i]), "");
                    //}

                    return Content("{\"result\":\"1\"}");
                }
                else
                    return Content("{\"result\":\"0\"}");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationController.UpdateState(string Id, int type)[HttpGet]"}
                });
                return Content("{\"err\":\"获取数据出错，请稍后重试\"}");
            }
        }

        [HttpGet]
        public ActionResult RecCheck(int id, string txtAP_MAC_ID, string txtAP_MAC, string StrLid, string PastTime)
        {
            try
            {
                txtAP_MAC = txtAP_MAC == "A1-B2-C3-D4-E5-F6" ? "" : txtAP_MAC;
                PastTime = PastTime ?? "0";

                int[] userType = ChangeValue.GetUserType();
                UserInfo user = (UserInfo)Session["userInfo"];

                Dictionary<string, string> dic = new Dictionary<string, string>() 
                { 
                    {"RecVal",id.ToString()},
                    {"StrLid",StrLid},
                    {"txtAP_MAC_ID",txtAP_MAC_ID},
                    {"txtAP_MAC",txtAP_MAC},
                    {"PastTime",PastTime},
                    {"UserType",userType[0].ToString()},
                    {"AreaId",userType[1].ToString()}
                };

                int result = new MacDAL().RecCheck(dic);
                if (result >= 0)
                {
                    return Content("{\"err\":\"\"}");
                }
                else
                {
                    return Content("{\"err\":\"重新验证操作失败，请稍后再试\"}");
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","APMacController.RecCheck(int ckType, string StrLid, string txtName, string selName, string selMAC, string txtMAC, string selSN, string txtSN, string selPolice, string selStatus, string selArea, string selType, string txtBeginTime, string txtEndTime, string selUserName, string txtUserName, string selProvince, string selCity, string radLocaType, string selValid)[HttpGet]"}
                });
                return Content("{\"err\":\"重新验证操作失败，请稍后再试\"}");
            }
        }
        #endregion

        #endregion
        
    }
}