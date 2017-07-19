using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using HCZZ.ModeDB;
using Common;
using Webdiyer.WebControls.Mvc;


namespace HCZZ.DAL
{
    public class LocationDAL
    {
        #region 场所管理
        #region 场所列表信息
        public PagedList<Loc_NetBarInfo> GetList(Dictionary<string, string> dic)
        {
            string sql = string.Empty;
            string WhereSql = " AND 1=1";
            int pageSize = Convert.ToInt32(dic["pageSize"]);
            int pageIndex = Convert.ToInt32(dic["pageIndex"]);
            string UserType = dic["UserType"].ToString();
            string AreaId = dic["AreaId"].ToString();
            List<SqlParameter> listParam = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(dic["txtNetbar_Wacode"]))
            {
                WhereSql = "  AND nbi.NETBAR_WACODE  LIKE  @NETBAR_WACODE";
                listParam.Add(new SqlParameter("@NETBAR_WACODE", "%" + dic["txtNetbar_Wacode"] + "%"));
            }
            if (!string.IsNullOrEmpty(dic["txtSite_Address"]))
            {
                WhereSql += "  AND  nbi.SITE_ADDRESS LIKE  @SITE_ADDRESS";
                listParam.Add(new SqlParameter("@SITE_ADDRESS", "%" + dic["txtSite_Address"] + "%"));
            }
            if (!string.IsNullOrEmpty(dic["AP_MAC"]))//需注意连表查
            {
                WhereSql += "  AND  di.AP_MAC  LIKE  @AP_MAC";
                listParam.Add(new SqlParameter("@AP_MAC", "%" + dic["AP_MAC"] + "%"));
            }
            if (!string.IsNullOrEmpty(dic["txtStartTime"]))
            {
                WhereSql += " AND nbi.createTime >=@txtStartTime";
                listParam.Add(new SqlParameter("@txtStartTime", Convert.ToDateTime(dic["txtStartTime"])));
            }
            if (!string.IsNullOrEmpty(dic["txtEndTime"]))
            {
                WhereSql += " AND nbi.createTime <=@txtEndTime";
                listParam.Add(new SqlParameter("@txtEndTime", Convert.ToDateTime(dic["txtEndTime"])));
            }
            if (dic["selProvince"] != "0")
            {
                WhereSql += " AND nbi.ProID=@ProID";
                listParam.Add(new SqlParameter("@ProID", dic["selProvince"]));
            }
            if (dic["selCity"] != "0")
            {
                WhereSql += " AND nbi.CityID=@CityID";
                listParam.Add(new SqlParameter("@CityID", dic["selCity"]));
            }
            if (dic["selArea"] != "0")
            {
                WhereSql += " AND nbi.Aid=@Aid";
                listParam.Add(new SqlParameter("@Aid", dic["selArea"]));
            }
            sql = "SELECT * FROM (SELECT ROW_NUMBER()OVER(ORDER BY nbi.ID)PageNum,nbi.*,so.SECURITY_SOFTWARE_ORGNAME,di.AP_MAC FROM NetBarInfo AS nbi LEFT JOIN SecurityOrg AS so ON nbi.SecId=so.ID LEFT JOIN DevInfo AS di ON nbi.ID=di.NETBAR_ID  WHERE nbi.Valid=1  " + WhereSql + ")temp WHERE temp.PageNum BETWEEN  " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;
            //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, listParam.ToArray());
            List<Loc_NetBarInfo> list = SqlHelper.ExecuteListModel<Loc_NetBarInfo>(SqlHelper.DBConnStr, sql, listParam.ToArray());
            //DataSetConvert convert = new DataSetConvert(ds);
            //List<Loc_NetBarInfo> list = convert.Get_ListModel<Loc_NetBarInfo>();
            sql = "SELECT COUNT(*) FROM NetBarInfo AS nbi LEFT JOIN SecurityOrg AS so ON nbi.SecId=so.ID LEFT JOIN DevInfo AS di ON nbi.ID=di.NETBAR_ID WHERE nbi.Valid=1 " + WhereSql;
            int result = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, listParam.ToArray()));
            return new PagedList<Loc_NetBarInfo>(list, pageIndex, pageSize, result);
        }
        #endregion

        #region 安全厂商列表
        public Dictionary<int,string> GetSecurityList() {
            string sql = String.Empty;
            List<SecurityOrg> list = new List<SecurityOrg>();
            try
            {
                sql = "select [ID] ,[SECURITY_SOFTWARE_ORGNAME] from [SecurityOrg]";
                list = SqlHelper.ExecuteListModel<SecurityOrg>(SqlHelper.DBConnStr, sql);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","LocationDAL.GetSecurityList()"},
                    {"SQL",sql}
                });
                throw ex;
            } 
            Dictionary<int, string> dic = new Dictionary<int, string>();
            foreach (var item in list) {
                dic.Add(item.ID,item.SECURITY_SOFTWARE_ORGNAME);
            }
            return dic;
        }
        #endregion
        #region  根据id获得场所实体
        public Loc_NetBarInfo GetNetBarModelById(int id)
        {
            string sql = "SELECT nbi.*,so.SECURITY_SOFTWARE_ORGNAME,so.SECURITY_SOFTWARE_ORGCODE FROM NetBarInfo AS nbi LEFT JOIN SecurityOrg AS so ON nbi.SecId=so.ID WHERE nbi.ID=@id";
            Loc_NetBarInfo ds = SqlHelper.ExecuteModel<Loc_NetBarInfo>(SqlHelper.DBConnStr, sql, new SqlParameter("@id", id));
            //DataSetConvert convert = new DataSetConvert(ds);
            return ds;
        }
        #endregion

        #region 添加场所信息
        public int InsertLocation(Loc_NetBarInfo loca)
        {
            string sql = "INSERT INTO NetBarInfo(NETBAR_WACODE,PLACE_NAME,SITE_ADDRESS,LONGITUDE,LATITUDE,NETSITE_TYPE,BUSINESS_NATURE,LAW_PRINCIPAL_NAME,LAW_PRINCIPAL_CERTIFICATE_TYPE,LAW_PRINCIPAL_CERTIFICATE_ID,RELATIONSHIP_ACCOUNT,START_TIME,END_TIME,ACCESS_TYPE,OPERATOR_NET,Statis,Valid,Createtime,Verified,Service_code,ProID,CityID,Aid,Pid,[Sid],SecId,CODE_ALLOCATION_ORGANIZATION)VALUES(@NETBAR_WACODE,@PLACE_NAME,@SITE_ADDRESS,@LONGITUDE,@LATITUDE,@NETSITE_TYPE,@BUSINESS_NATURE,@LAW_PRINCIPAL_NAME,@LAW_PRINCIPAL_CERTIFICATE_TYPE,@LAW_PRINCIPAL_CERTIFICATE_ID,@RELATIONSHIP_ACCOUNT,@START_TIME,@END_TIME,@ACCESS_TYPE,@OPERATOR_NET,@Statis,@Valid,@Createtime,@Verified,@Service_code,@ProID,@CityID,@Aid,@Pid,@Sid,@SecId,@CODE_ALLOCATION_ORGANIZATION); SELECT SCOPE_IDENTITY();";
            SqlParameter[] para = new SqlParameter[] 
            {
                new SqlParameter("@NETBAR_WACODE",loca.NETBAR_WACODE),
                new SqlParameter("@PLACE_NAME",loca.PLACE_NAME),
                new SqlParameter("@SITE_ADDRESS",loca.SITE_ADDRESS),
                new SqlParameter("@LONGITUDE",loca.LONGITUDE),
                new SqlParameter("@LATITUDE",loca.LATITUDE),
                new SqlParameter("@NETSITE_TYPE",loca.NETSITE_TYPE),
                new SqlParameter("@BUSINESS_NATURE",loca.BUSINESS_NATURE),
                new SqlParameter("@LAW_PRINCIPAL_NAME",loca.LAW_PRINCIPAL_NAME),
                new SqlParameter("@LAW_PRINCIPAL_CERTIFICATE_TYPE",loca.LAW_PRINCIPAL_CERTIFICATE_TYPE),
                new SqlParameter("@LAW_PRINCIPAL_CERTIFICATE_ID",loca.LAW_PRINCIPAL_CERTIFICATE_ID),
                new SqlParameter("@RELATIONSHIP_ACCOUNT",loca.RELATIONSHIP_ACCOUNT),
                new SqlParameter("@START_TIME",loca.START_TIME),
                new SqlParameter("@END_TIME",loca.END_TIME),
                new SqlParameter("@ACCESS_TYPE",loca.ACCESS_TYPE),
                new SqlParameter("@OPERATOR_NET",loca.OPERATOR_NET),
                //new SqlParameter("@ACSSES_IP",loca.ACSSES_IP),
                new SqlParameter("@Statis",loca.Statis),
                new SqlParameter("@Valid",loca.Valid),
                new SqlParameter("@Createtime",loca.Createtime),
                new SqlParameter("@Verified",loca.Verified),
                new SqlParameter("@Service_code",loca.Service_code),
                new SqlParameter("@ProID",loca.ProID),
                new SqlParameter("@CityID",loca.CityID),
                new SqlParameter("@Aid",loca.Aid),
                new SqlParameter("@Pid",loca.Pid),
                new SqlParameter("@Sid",loca.Sid),
                new SqlParameter("@SecId",loca.SecId),
                new SqlParameter("@CODE_ALLOCATION_ORGANIZATION",loca.CODE_ALLOCATION_ORGANIZATION)
            };
            int LocalId = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, para));
            return LocalId;
        }
        #endregion

        #region 更新场所信息
        /// <summary>
        /// 修改场所信息
        /// </summary>
        /// <param name="il"></param>
        /// <returns></returns>
        public int UpdateLocaById(ImportLocation il)
        {
            string sql = "UPDATE NetBarInfo SET PLACE_NAME = @LocaName,SITE_ADDRESS = @Address,LAW_PRINCIPAL_NAME = @UserName,RELATIONSHIP_ACCOUNT = @Mobile";
            sql += (!string.IsNullOrEmpty(il.LONGITUDE) ? ",LONGITUDE = @Longitude" : "");
            sql += (!string.IsNullOrEmpty(il.LATITUDE) ? ",LATITUDE = @Latitude" : "");
            sql += (il.verify != 0 ? ",Verified = @IsVerify" : "");
            sql += ",IsUpdate_NETBAR_WACODE=(SELECT CASE WHEN nbi.NETBAR_WACODE ='' OR nbi.NETBAR_WACODE IS NULL THEN 1 ELSE 0 END FROM NetBarInfo nbi WHERE nbi.ID=@Id)";
            sql += (il.LocaType != 0 ? ",NETSITE_TYPE = @LocaType" : "");
            sql += (il.Sid != 0 ? ",ProID = @ProID,CityID = @CityID,Aid = @Aid,Pid = @Pid,[Sid] = @Sid" : "");
            sql += " WHERE ID=@Id";

            SqlParameter[] param = new SqlParameter[] 
            { 
                new SqlParameter("@LocaName",il.LocaName),
                new SqlParameter("@Address",il.Address),
                new SqlParameter("@UserName",il.UserName),
                new SqlParameter("@Mobile",il.Mobile),
                new SqlParameter("@Longitude",il.LONGITUDE),
                new SqlParameter("@Latitude",il.LATITUDE),
                new SqlParameter("@LocaType",il.LocaType),
                new SqlParameter("@IsVerify",il.verify),
                new SqlParameter("@ProID",il.ProId),
                new SqlParameter("@CityID",il.CityId),
                new SqlParameter("@Aid",il.Aid),
                new SqlParameter("@Pid",il.Pid),
                new SqlParameter("@Sid",il.Sid),
                new SqlParameter("@Id",il.NetBarId)
            };
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
        }
        public bool UpdateLoca(Loc_NetBarInfo loca, bool isTrue)
        {
            //,ACSSES_IP = @ACSSES_IP
            string sql = "if EXISTS(select * from NetBarInfo where APNum=1 and id=@ID)BEGIN update DevInfo set LONGITUDE=@LONGITUDE,LATITUDE=@LATITUDE WHERE NETBAR_ID=@ID AND APTYPE=1 END;      UPDATE NetBarInfo SET NETBAR_WACODE = @NETBAR_WACODE,PLACE_NAME = @PLACE_NAME,SITE_ADDRESS = @SITE_ADDRESS,LONGITUDE = @LONGITUDE,LATITUDE = @LATITUDE,NETSITE_TYPE = @NETSITE_TYPE,BUSINESS_NATURE = @BUSINESS_NATURE,LAW_PRINCIPAL_NAME = @LAW_PRINCIPAL_NAME,LAW_PRINCIPAL_CERTIFICATE_TYPE = @LAW_PRINCIPAL_CERTIFICATE_TYPE,LAW_PRINCIPAL_CERTIFICATE_ID = @LAW_PRINCIPAL_CERTIFICATE_ID,RELATIONSHIP_ACCOUNT = @RELATIONSHIP_ACCOUNT,START_TIME = @START_TIME,END_TIME = @END_TIME,ACCESS_TYPE = @ACCESS_TYPE,OPERATOR_NET = @OPERATOR_NET,CODE_ALLOCATION_ORGANIZATION = @CODE_ALLOCATION_ORGANIZATION,";
            if (isTrue)
                sql += "Statis = @Statis,";
            sql += "Service_code = @Service_code,ProID = @ProID,CityID = @CityID,Aid = @Aid,Pid = @Pid,[Sid] = @Sid,SecId = @SecId,Verified = @Verified,IsUpdate_NETBAR_WACODE=@IsUpdate_NETBAR_WACODE WHERE ID=@ID; update DevInfo set NETBAR_WACODE=@NETBAR_WACODE where NETBAR_ID=@ID";
            SqlParameter[] param = new SqlParameter[] 
            {
                new SqlParameter("@NETBAR_WACODE",loca.NETBAR_WACODE),
                new SqlParameter("@PLACE_NAME",loca.PLACE_NAME),
                new SqlParameter("@SITE_ADDRESS",loca.SITE_ADDRESS),
                new SqlParameter("@LONGITUDE",loca.LONGITUDE),
                new SqlParameter("@LATITUDE",loca.LATITUDE),
                new SqlParameter("@NETSITE_TYPE",loca.NETSITE_TYPE),
                new SqlParameter("@BUSINESS_NATURE",loca.BUSINESS_NATURE),
                new SqlParameter("@LAW_PRINCIPAL_NAME",loca.LAW_PRINCIPAL_NAME),
                new SqlParameter("@LAW_PRINCIPAL_CERTIFICATE_TYPE",loca.LAW_PRINCIPAL_CERTIFICATE_TYPE),
                new SqlParameter("@LAW_PRINCIPAL_CERTIFICATE_ID",loca.LAW_PRINCIPAL_CERTIFICATE_ID),
                new SqlParameter("@RELATIONSHIP_ACCOUNT",loca.RELATIONSHIP_ACCOUNT),
                new SqlParameter("@START_TIME",loca.START_TIME),
                new SqlParameter("@END_TIME",loca.END_TIME),
                new SqlParameter("@ACCESS_TYPE",loca.ACCESS_TYPE),
                new SqlParameter("@OPERATOR_NET",loca.OPERATOR_NET),
                //new SqlParameter("@ACSSES_IP",loca.ACSSES_IP),
                new SqlParameter("@Service_code",loca.Service_code),
                new SqlParameter("@ProID",loca.ProID),
                new SqlParameter("@CityID",loca.CityID),
                new SqlParameter("@Aid",loca.Aid),
                new SqlParameter("@Pid",loca.Pid),
                new SqlParameter("@Sid",loca.Sid),
                new SqlParameter("@SecId",loca.SecId),
                new SqlParameter("@Statis",loca.Statis),
                new SqlParameter("@Verified",loca.Verified),
                new SqlParameter("@ID",loca.ID),
                new SqlParameter("@IsUpdate_NETBAR_WACODE",loca.IsUpdate_NETBAR_WACODE),
                new SqlParameter("@CODE_ALLOCATION_ORGANIZATION",loca.CODE_ALLOCATION_ORGANIZATION)
            };
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param) > 0;
        }
        #endregion

        #region 删除
        #region  更新场所信息是否删除状态
        public int UpdateNetBarById(int id)
        {
            string sql = "UPDATE NetBarInfo  SET Valid = 0 WHERE ID=@id";
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@id", id));
        }
        #endregion

        #region 删除设备信息
        public int UpdateDevById(int id)
        {
            string sql = "UPDATE  DevInfo  SET  COLLECTION_EQUIPMENT_ID =NULL,AP_MAC = NULL,NETBAR_WACODE = NULL,LONGITUDE = NULL,LATITUDE = NULL,[FLOOR] = NULL,SUBWAY_STATION = NULL,SUBWAY_LINE_INFO = NULL,SUBWAY_VEHICLE_INFO = NULL,SUBWAY_COMPARTMENT_NUMBER =NULL,CAR_CODE =NULL,NETBAR_ID =NULL,APType = NULL,Verified = NULL,CreateTime = NULL,DevStatis = NULL,UpdateTime = NULL,AuditTime = NULL,Lguid = NULL,SN = NULL,IsTrial = NULL,Supplier = NULL,ProjectType = NULL,CasesType = NULL,ModeType = NULL,LogCapture = NULL,APName = NULL,UPLOAD_TIME_INTERVAL = NULL,COLLECTION_RADIUS =NULL,Channel = NULL,IsReboot = NULL,ForcedOfflineTime = NULL,FenceOffTime = NULL,Isopen = NULL,PastTime = NULL,WIFIPwd = NULL,SSIDHidden = NULL,OriginalMac = NULL,KernelVersion = NULL,[SysName] = NULL,MemInfo = NULL,SysType = NULL,SSID = NULL,[Version] = NULL,V3CID = NULL,DeviceName = NULL,ProjectAccessNum = NULL,MatchTime = NULL,AP_ACSSES_IP = NULL,Remark = NULL   WHERE  NETBAR_ID =@id";
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@id", id));

        }
        #endregion
        #endregion

        #region 修改场所中的设备数量字段
        /// <summary>
        /// 修改场所中的设备数量字段   加或减数量 传入值（+/-）
        /// </summary>
        /// <param name="Id">场所ID</param>
        /// <param name="FUHAO">加或减数量 传入值（+/-）</param>
        public void UpdateAPNumById(int Id, string FUHAO, string TableKey)
        {
            string sql = " UPDATE " + TableKey + "NetBarInfo SET APNum=APNum" + FUHAO + "1 WHERE ID=@Id";
            SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Id", Id));
        }
        #endregion

        #region 根据场所ID获取审核场所数据信息
        /// <summary>
        /// 根据场所ID获取审核场所数据信息
        /// </summary>
        /// <param name="Lid"></param>
        /// <returns></returns>
        public DataTable GetVerifiyNetBarById(int Lid)
        {
            string sql = "select vnbi.NETBAR_WACODE,PLACE_NAME,NETSITE_TYPE,LAW_PRINCIPAL_NAME,RELATIONSHIP_ACCOUNT,LAW_PRINCIPAL_CERTIFICATE_TYPE,LAW_PRINCIPAL_CERTIFICATE_ID,ACSSES_IP,SITE_ADDRESS,vnbi.ProID,vnbi.CityID,vnbi.Aid,vnbi.Pid,vnbi.Sid,si.SName,pi1.Name PName from Verify_NetBarInfo vnbi LEFT JOIN ScenicInfo si ON vnbi.sid=si.ID LEFT JOIN PoliceInfo pi1 ON si.PId=pi1.ID where NETBAR_ID=@NETBAR_ID";
            return SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@NETBAR_ID", Lid)).Tables[0];
        }
        #endregion
        #endregion

        #region 地图标记
        public DataTable GetLocaMapDt(string proId, string cityId, string aid)
        {
            int fw = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["fw"]) * 3;
            int sj = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["sj"]);
            fw = fw == 0 ? 30 : fw;
            sj = sj == 0 ? 15 : sj;
            string whereSql = "";
            SqlParameter param = null;
            if (aid != "0")
            { whereSql += "  AND nbi.Aid= @Aid "; param = new SqlParameter("@Aid", aid); }
            else if (cityId != "0")
            { whereSql += "  AND nbi.CityID=@CityID"; param = new SqlParameter("@CityID", cityId); }
            else if (proId != "0")
            { whereSql += "  AND nbi.ProID=@ProID"; param = new SqlParameter("@ProID", proId); }

            //string sql = "SELECT nbi.Id,nbi.PLACE_NAME,nbi.LONGITUDE +','+nbi.LATITUDE Gnote,(SELECT TOP 1 CASE when (di.UpdateTime>=DATEADD(mi,-" + fw + ",GETDATE()) OR  di.AuditTime>=DATEADD(mi,-" + sj + ",GETDATE())) THEN 1 ELSE 0 END LocaStatis FROM DevInfo di WHERE di.NETBAR_ID=nbi.ID ORDER BY di.UpdateTime desc ) LocaStatis FROM NetBarInfo nbi WHERE nbi.Valid=1 AND (nbi.LONGITUDE!='' AND nbi.LONGITUDE IS NOT NULL) " + whereSql;
            string sql = "SELECT nbi.Id,nbi.PLACE_NAME,nbi.APNum,nbi.LONGITUDE +','+nbi.LATITUDE Gnote,ISNULL((SELECT TOP 1 CASE when (di.UpdateTime>=DATEADD(mi,-" + fw + ",GETDATE()) OR  di.AuditTime>=DATEADD(mi,-" + sj + ",GETDATE())) THEN 1 ELSE 0 END  LocaStatis FROM DevInfo di WHERE di.NETBAR_ID=nbi.ID ORDER BY di.UpdateTime desc),0) LocaStatis FROM NetBarInfo nbi WHERE nbi.Valid=1 AND (nbi.LONGITUDE!='' AND nbi.LONGITUDE IS NOT NULL) and APNum >0 " + whereSql;
            return SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param).Tables[0];
        }
        #endregion

        #region 未知场所

        public PagedList<UnknownDevice> GetDeviceList(Dictionary<string, string> dic)
        {
            int pageIndex = Convert.ToInt32(dic["pageIndex"]);
            int pageSize = Convert.ToInt32(dic["pageSize"]);
            string whereSql = " where 1=1 ";
            string sql;

            List<SqlParameter> listParam = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(dic["AP_MAC"]))
            {
                whereSql += " AND ud.AP_MAC LIKE @AP_MAC ";
                listParam.Add(new SqlParameter("@AP_MAC", "%" + dic["AP_MAC"].Replace("-", "") + "%"));
            }
            if (!string.IsNullOrEmpty(dic["StartTime"]))
            {
                whereSql += " AND ud.UpdateTime >=@StartTime ";
                listParam.Add(new SqlParameter("@StartTime", dic["StartTime"]));
            }
            if (!string.IsNullOrEmpty(dic["EndTime"]))
            {
                whereSql += " AND ud.UpdateTime <=@EndTime ";
                listParam.Add(new SqlParameter("@EndTime", dic["EndTime"]));
            }
            if (!string.IsNullOrEmpty(dic["AreaName"]))
            {
                whereSql += " AND ud.AreaName LIKE @AreaName";
                listParam.Add(new SqlParameter("@AreaName", "%" + dic["AreaName"] + "%"));
            }
            sql = "SELECT * FROM ( SELECT ROW_NUMBER() OVER (ORDER BY ud.Id DESC) PageNum,ud.ID,ud.AP_MAC,ud.UpdateTime,ud.CreateTime,ud.OutIpAddress,ud.AreaName FROM UnknownDevice ud " + whereSql + ") temp WHERE temp.PageNum BETWEEN " + ((pageIndex - 1) * pageSize) + " AND " + pageIndex * pageSize;

            //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, listParam.ToArray());
            List<UnknownDevice> list = SqlHelper.ExecuteListModel<UnknownDevice>(SqlHelper.DBConnStr, sql, listParam.ToArray());
            sql = "SELECT count(0) FROM UnknownDevice ud " + whereSql;
            int result = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, listParam.ToArray()));
            //List<UnknownDevice> list = new DataSetConvert(ds).Get_ListModel<UnknownDevice>();
            return new PagedList<UnknownDevice>(list, pageIndex, pageSize, result);
        }
        public DataTable GetDeviceExcel(Dictionary<string, string> dic)
        {
            string sql;
            DataTable dt = new DataTable();
            string whereSql = " where 1=1 ";
            List<SqlParameter> listParam = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(dic["AP_MAC"]))
            {
                whereSql += " AND ud.AP_MAC LIKE @AP_MAC ";
                listParam.Add(new SqlParameter("@AP_MAC", "%" + dic["AP_MAC"].Replace("-", "") + "%"));
            }
            if (!string.IsNullOrEmpty(dic["StartTime"]))
            {
                whereSql += " AND ud.UpdateTime >=@StartTime ";
                listParam.Add(new SqlParameter("@StartTime", dic["StartTime"]));
            }
            if (!string.IsNullOrEmpty(dic["EndTime"]))
            {
                whereSql += " AND ud.UpdateTime <=@EndTime ";
                listParam.Add(new SqlParameter("@EndTime", dic["EndTime"]));
            }
            if (!string.IsNullOrEmpty(dic["AreaName"]))
            {
                whereSql += " AND ud.AreaName LIKE @AreaName";
                listParam.Add(new SqlParameter("@AreaName", "%" + dic["AreaName"] + "%"));
            }
            sql = " SELECT * FROM UnknownDevice ud " + whereSql;
            sql += " order by ud.Id desc";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, listParam.ToArray());
            return ds.Tables[0];
        }

        /// <summary>
        /// 删除未知设备
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int delUnDev(int Id)
        {
            string sql = "DELETE FROM UnknownDevice WHERE ID=@Id";
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Id", Id));
        }
        #endregion

        #region 设备管理
        #region 获取设备列表
        /// <summary>
        /// AP设备列表
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public PagedList<MacInfo> GetAPMACList(Dictionary<string, string> dic)
        {
            string sql = "";
            int pageIndex = Convert.ToInt32(dic["pageIndex"]);
            int pageSize = Convert.ToInt32(dic["pageSize"]);
            int Lid = Convert.ToInt32(dic["Lid"]);
            string whereSql = " WHERE NETBAR_ID=@Lid ";

            if (!string.IsNullOrEmpty(dic["txtAP_MAC_ID"]))
                whereSql += " AND di.COLLECTION_EQUIPMENT_ID LIKE '%" + dic["txtAP_MAC_ID"] + "%'";
            if (!string.IsNullOrEmpty(dic["txtAP_MAC"]))
                whereSql += " AND di.AP_MAC LIKE '%" + dic["txtAP_MAC"] + "%'";
            if (dic["PotType"] != "0")
                whereSql += " AND di.PotType=" + dic["PotType"];
            sql = "SELECT * FROM ( SELECT ROW_NUMBER () OVER (ORDER BY di.ID DESC) PageNum ,di.* FROM DevInfo di " + whereSql + " ) temp WHERE temp.PageNum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;

            sql += " SELECT COUNT(0) FROM  DevInfo di " + whereSql;

            //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Lid", Lid));
            List<MacInfo> list = SqlHelper.ExecuteListModel<MacInfo>(SqlHelper.DBConnStr,sql, new SqlParameter("@Lid", Lid));
            //DataSetConvert convert = new DataSetConvert(ds);
            //List<MacInfo> list = convert.Get_ListModel<MacInfo>();
            sql = "SELECT COUNT(*) FROM DevInfo di" + whereSql;
            int result = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Lid", Lid)));
            return new PagedList<MacInfo>(list, pageIndex, pageSize, result);
        }
        public PagedList<Loc_DevInfo> GetDevList(Dictionary<string, string> dic)
        {
            string sql = string.Empty;
            string NetId = dic["NetId"];
            string WhereSql = " 1=1";
            int PageIndex = Convert.ToInt32(dic["PageIndex"]);
            int PageSize = Convert.ToInt32(dic["PageSize"]);
            List<SqlParameter> Listpara = new List<SqlParameter>();
            sql = "SELECT * FROM(SELECT ROW_NUMBER() OVER (ORDER BY di.ID)PageNum,* FROM DevInfo AS di  WHERE di.NETBAR_ID='" + NetId + "'  AND " + WhereSql + ")temp WHERE temp.PageNum  BETWEEN  " + (PageIndex - 1) * PageSize + 1 + " AND " + PageSize * PageIndex;
            List<Loc_DevInfo> list = SqlHelper.ExecuteListModel<Loc_DevInfo>(SqlHelper.DBConnStr, sql, Listpara.ToArray());
            //DataSetConvert convert = new DataSetConvert(ds);
            //List<Loc_DevInfo> list = convert.Get_ListModel<Loc_DevInfo>();
            sql = "SELECT COUNT(*) FROM DevInfo AS di WHERE  di.NETBAR_ID=" + NetId + "  AND  " + WhereSql;
            int result = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql));
            return new PagedList<Loc_DevInfo>(list, PageIndex, PageSize, result);
        }
        #endregion


        public int UpdateMacNULLById(int Mid)
        {
            string sql = "UPDATE DevInfo SET NETBAR_WACODE = NULL,NETBAR_ID = NULL,Verified = NULL,APType = NULL,LONGITUDE = NULL,LATITUDE = NULL,[FLOOR] = NULL ,SUBWAY_STATION = NULL,SUBWAY_LINE_INFO = NULL,SUBWAY_VEHICLE_INFO = NULL,SUBWAY_COMPARTMENT_NUMBER = NULL,CAR_CODE = NULL,V3CID = NULL  WHERE ID=@ID";
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@ID", Mid));
        }

        public int UpdateLocaVerifiedById(int Verified, int Id)
        {
            return UpdateLocaVerifiedById(Verified, Id, 0);
        }

        public int UpdateLocaVerifiedById(int Verified, int Id, int isUpdateNetBarWACODE)
        {
            string sql = "UPDATE NetBarInfo SET Verified = @Verified,IsUpdate_NETBAR_WACODE=@isUpdateNetBarWACODE WHERE ID=@ID";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID", Id),
                new SqlParameter("@Verified", Verified),
                new SqlParameter("@isUpdateNetBarWACODE", isUpdateNetBarWACODE)
            };

            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
        }

        /// <summary>
        /// 修改AP信息
        /// </summary>
        /// <param name="il"></param>
        /// <returns></returns>
        public int UpdateDevById(ImportLocation il)
        {
            string sql = "UPDATE devinfo SET APName = @APName WHERE ID=@Id";

            SqlParameter[] param = new SqlParameter[] 
            { 
                new SqlParameter("@APName",il.ApName),
                new SqlParameter("@Id",il.DveId)
            };
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
        }
        /// <summary>
        /// 将审核通过的场所信息拷贝到正式表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public void CopyVerifiyNetbarToNetbarById(int Id)
        {
            string sql = "IF EXISTS(SELECT * FROM Verify_NetBarInfo nbi WHERE nbi.NETBAR_ID=@NetBar_Id) BEGIN DECLARE @PLACE_NAME VARCHAR(256),@SITE_ADDRESS VARCHAR(256),@LAW_PRINCIPAL_NAME VARCHAR(64),@LAW_PRINCIPAL_CERTIFICATE_ID VARCHAR(128),@RELATIONSHIP_ACCOUNT VARCHAR(128),@ACSSES_IP VARCHAR(64),@ProID INT,@CityID INT,@Aid INT,@Pid INT,@Sid int SELECT @PLACE_NAME =nbi.PLACE_NAME,@SITE_ADDRESS =SITE_ADDRESS,@LAW_PRINCIPAL_NAME =LAW_PRINCIPAL_NAME,@LAW_PRINCIPAL_CERTIFICATE_ID =LAW_PRINCIPAL_CERTIFICATE_ID,@RELATIONSHIP_ACCOUNT =RELATIONSHIP_ACCOUNT,@ACSSES_IP =ACSSES_IP,@ProID=nbi.ProID,@CityID=nbi.CityID,@Aid=nbi.Aid,@Pid=nbi.Pid,@Sid=nbi.[Sid] FROM Verify_NetBarInfo nbi WHERE nbi.NETBAR_ID=@NetBar_Id; UPDATE NetBarInfo SET PLACE_NAME = @PLACE_NAME,SITE_ADDRESS = @SITE_ADDRESS,NETSITE_TYPE = 9,LAW_PRINCIPAL_NAME = @LAW_PRINCIPAL_NAME,LAW_PRINCIPAL_CERTIFICATE_TYPE = 111,LAW_PRINCIPAL_CERTIFICATE_ID = @LAW_PRINCIPAL_CERTIFICATE_ID,RELATIONSHIP_ACCOUNT = @RELATIONSHIP_ACCOUNT,ProID = @ProID ,CityID = @CityID ,Aid = @Aid ,Pid = @Pid ,Sid = @Sid,IsUpdate_NETBAR_WACODE=1 WHERE ID=@NetBar_Id; END";
            SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@NetBar_Id", Id));
        }


        #endregion

        #region     获取缓存数据
        /// <summary>
        /// 获取场所缓存的数据集合
        /// </summary>
        /// <returns></returns>
        public List<Loc_NetBarInfo> GetLocationList()
        {
            //string sql = "select * FROM NetBarInfo nbi";
            string sql = "select  nbi.ID,nbi.PLACE_NAME,(CASE WHEN EXISTS(SELECT * FROM DevInfo di WHERE di.NETBAR_ID=nbi.ID AND di.ProjectType=7) THEN 1 ELSE 0 END ) IsVoid,nbi.NETSITE_TYPE FROM NetBarInfo nbi";
            //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
            List<Loc_NetBarInfo> list = SqlHelper.ExecuteListModel<Loc_NetBarInfo>(SqlHelper.DBConnStr, sql);
            //return new DataSetConvert(ds).Get_ListModel<Loc_NetBarInfo>();
            return list;
        }
        /// <summary>
        /// 获取设备缓存的数据集合
        /// </summary>
        /// <returns></returns>
        public List<Loc_DevInfo> GetDevList()
        {
            string sql = "SELECT * FROM DevInfo";
            //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
            //return new DataSetConvert(ds).Get_ListModel<Loc_DevInfo>();
            List<Loc_DevInfo> list = SqlHelper.ExecuteListModel<Loc_DevInfo>(SqlHelper.DBConnStr, sql);
            return list;
        }
        #endregion      

        #region 其他
        /// <summary>
        /// 根据ID或者场所名称获取场所详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="TableKey"></param>
        /// <returns></returns>
        public Loc_NetBarInfo GetLocaByIdOrName(int id, string locaName)
        {
            string sql = "SELECT top 1 nbi.* FROM NetBarInfo nbi where ";
            sql += id > 0 ? "nbi.ID=@key" : "nbi.PLACE_NAME=@key";
            //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@key", (id > 0 ? id.ToString() : locaName)));
            //return new DataSetConvert(ds).Get_SingleModel<Loc_NetBarInfo>();
            Loc_NetBarInfo nbi = SqlHelper.ExecuteModel<Loc_NetBarInfo>(SqlHelper.DBConnStr, sql, new SqlParameter("@key", (id > 0 ? id.ToString() : locaName)));
            return nbi;
        }

        
        /// <summary>
        /// 根据AP设备ID或者Mac地址获取AP信息
        /// </summary>
        /// <param name="Mid"></param>
        /// <param name="mac"></param>
        /// <returns></returns>
        public Loc_DevInfo GetMacInfoByIdOrMac(int Mid, string mac)
        {
            string sql = "select top 1 * from DevInfo WHERE ";
            sql += Mid > 0 ? "ID=@key" : "AP_MAC=@key";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@key", (Mid > 0 ? Mid.ToString() : mac)));
            return new DataSetConvert(ds).Get_SingleModel<Loc_DevInfo>();
        }
        /// <summary>
        /// 根据派出所ID获取场所列表
        /// </summary>
        /// <param name="Pid"></param>
        /// <returns></returns>
        public List<SelMacInfo> GetLocaListByPid(int Pid)
        {
            string sql = "select nbi.ID,nbi.PLACE_NAME Name FROM NetBarInfo nbi WHERE nbi.Pid=@Pid AND nbi.Valid=1";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Pid", Pid));
            return new DataSetConvert(ds).Get_ListModel<SelMacInfo>();
        }

        public DataTable GetLocaDT(Dictionary<string, string> dic)
        {
            string sql = "";
            string UserType = dic["UserType"].ToString();
            string AreaId = dic["AreaId"].ToString();
            int fw = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["fw"]) * 3;
            int sj = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["sj"]);
            string strExprotId = dic["strExprotId"];
            string whereSql = " WHERE Valid=1 ";
            List<SqlParameter> listParam = new List<SqlParameter>();
            string PotType = " LEFT JOIN DevInfo di ON di.NETBAR_ID=nbi.ID WHERE 1=1 ";
            if (dic["SelPotType"] != "0")
            {
                PotType += " AND di.PotType=@SelPotType";
                listParam.Add(new SqlParameter("@SelPotType", dic["SelPotType"]));
            }
            if (!string.IsNullOrEmpty(dic["ProjectAccessNum"]))
            {
                PotType += " AND di.ProjectAccessNum LIKE @ProjectAccessNum";
                listParam.Add(new SqlParameter("ProjectAccessNum", "%" + dic["ProjectAccessNum"] + "%"));
            }
            if (UserType == "6")
                whereSql += " AND nbi.ProID=" + AreaId;
            else if (UserType == "4")
                whereSql += " AND nbi.CityID=" + AreaId;
            else if (UserType == "2")
                whereSql += " AND nbi.Aid=" + AreaId;
            else if (UserType == "3")
                whereSql += " AND nbi.PId=" + AreaId;

            if (dic["selProvince"] != "0")
            {
                whereSql += " AND nbi.ProID=@ProID";
                listParam.Add(new SqlParameter("@ProID", dic["selProvince"]));
            }
            if (dic["selCity"] != "0")
            {
                whereSql += " AND nbi.CityID=@CityID";
                listParam.Add(new SqlParameter("@CityID", dic["selCity"]));
            }
            if (dic["selArea"] != "0")
            {
                whereSql += " AND nbi.Aid=@Aid";
                listParam.Add(new SqlParameter("@Aid", dic["selArea"]));
            }
            if (dic["selPolice"] != "0")
            {
                whereSql += " AND nbi.PId=@PId";
                listParam.Add(new SqlParameter("@PId", dic["selPolice"]));
            }

            if (!string.IsNullOrEmpty(dic["txtNetbar_Wacode"]))
                whereSql += " AND nbi.NETBAR_WACODE LIKE '%" + dic["txtNetbar_Wacode"] + "%'";
            if (!string.IsNullOrEmpty(dic["txtPlace_Name"]))
                whereSql += " AND nbi.PLACE_NAME LIKE '%" + dic["txtPlace_Name"] + "%'";
            if (!string.IsNullOrEmpty(dic["txtSite_Address"]))
                whereSql += " AND nbi.SITE_ADDRESS LIKE '%" + dic["txtSite_Address"] + "%'";
            if (!string.IsNullOrEmpty(dic["txtLaw_Principal_Name"]))
                whereSql += " AND nbi.LAW_PRINCIPAL_NAME LIKE '%" + dic["txtLaw_Principal_Name"] + "%'";
            if (!string.IsNullOrEmpty(dic["txtRelationship_Account"]))
                whereSql += " AND nbi.RELATIONSHIP_ACCOUNT LIKE '%" + dic["txtRelationship_Account"] + "%'";

            if (dic["selNetsite_Type"] != "0")
                whereSql += " AND nbi.NETSITE_TYPE  = '" + dic["selNetsite_Type"] + "'";
            if (dic["selStatus"] != "0")
                whereSql += " AND nbi.Statis  = " + dic["selStatus"];
            if (dic["selBusiness_Nature"] != "-1")
                whereSql += " AND nbi.BUSINESS_NATURE  = '" + dic["selBusiness_Nature"] + "'";
            if (dic["selAccess_Type"] != "0")
                whereSql += " AND nbi.ACCESS_TYPE  = " + dic["selAccess_Type"];
            if (dic["selOperator_Net"] != "0")
                whereSql += " AND nbi.OPERATOR_NET  = '" + dic["selOperator_Net"] + "'";
            if (dic["selVerified"] != "0")
                whereSql += " AND nbi.Verified  = " + dic["selVerified"];
            if (!string.IsNullOrEmpty(dic["txtBeginTime"]))
            { whereSql += " AND nbi.createTime >=@beginTime"; listParam.Add(new SqlParameter("@beginTime", dic["txtBeginTime"])); }
            if (!string.IsNullOrEmpty(dic["txtEndTime"]))
            { whereSql += " AND nbi.createTime <=@EndTime"; listParam.Add(new SqlParameter("@EndTime", dic["txtEndTime"])); }
            if (!string.IsNullOrEmpty(dic["AP_MAC"]))
            { whereSql += " AND id IN (SELECT di.NETBAR_ID FROM DevInfo di WHERE di.AP_MAC LIKE @AP_MAC)"; listParam.Add(new SqlParameter("@AP_MAC", "%" + dic["AP_MAC"] + "%")); }
            //if (!string.IsNullOrEmpty(dic["txtOutIpAddress"]))
            //{
            //    PotType += " AND di.AP_ACSSES_IP LIKE @ACSSES_IP "; listParam.Add(new SqlParameter("@ACSSES_IP", "%" + dic["txtOutIpAddress"] + "%"));
            //}
            DateTime tempFW = DateTime.Now.AddMinutes(-fw);
            DateTime tempSJ = DateTime.Now.AddMinutes(-sj);
            int fwStatus = Convert.ToInt32(dic["selApfwStatus"]);
            int sjStatus = Convert.ToInt32(dic["selApsjStatus"]);
            string fwOper = ">="; string sjOper = ">=";
            if (fwStatus != 0)
            { whereSql += " AND nbi.fuwu =@fw"; listParam.Add(new SqlParameter("@fw", (fwStatus == 1 || fwStatus == 3 ? "服务在线" : "服务离线"))); }
            if (fwStatus == 3 || fwStatus == 4) tempFW = DateTime.Now.AddDays(-Convert.ToInt32(dic["txtOtherDay"]));
            if (fwStatus == 2 || fwStatus == 4) fwOper = "<";

            if (sjStatus != 0)
            { whereSql += " AND nbi.shuju =@sj"; listParam.Add(new SqlParameter("@sj", (sjStatus == 1 || sjStatus == 3 ? "数据在线" : "数据离线"))); }
            if (sjStatus == 3 || sjStatus == 4) tempSJ = DateTime.Now.AddDays(-Convert.ToInt32(dic["txtOtherDay"]));
            if (sjStatus == 2 || sjStatus == 4) sjOper = "<";
            sql = "SELECT * from(  SELECT distinct nbi.id,nbi.NETBAR_WACODE,nbi.PLACE_NAME,nbi.SITE_ADDRESS,nbi.NETSITE_TYPE,nbi.LAW_PRINCIPAL_NAME,nbi.RELATIONSHIP_ACCOUNT,nbi.Statis,nbi.CODE_ALLOCATION_ORGANIZATION,nbi.LONGITUDE,nbi.LATITUDE,nbi.Verified,nbi.LAW_PRINCIPAL_CERTIFICATE_TYPE,nbi.LAW_PRINCIPAL_CERTIFICATE_ID,nbi.ACCESS_TYPE,nbi.OPERATOR_NET,nbi.START_TIME LocaSTARTTIME,nbi.END_TIME LocaENDTIME,nbi.Valid,nbi.ProID,nbi.CityID,nbi.Aid,nbi.PId,nbi.createTime,nbi.BUSINESS_NATURE,(SELECT CASE WHEN COUNT(0)>0 THEN '服务在线' ELSE '服务离线' end FROM DevInfo di2 WHERE di2.NETBAR_ID=nbi.id AND di2.UpdateTime" + fwOper + "'" + tempFW + "')fuwu,(SELECT CASE WHEN COUNT(0)>0 THEN '数据在线' ELSE '数据离线' end FROM DevInfo di2 WHERE di2.NETBAR_ID=nbi.id AND di2.AuditTime" + sjOper + "'" + tempSJ + "') shuju";
            if (strExprotId.Contains("19"))
                sql += ",ai11.Area ProName";
            if (strExprotId.Contains("20"))
                sql += ",ai12.Area CName";
            if (strExprotId.Contains("21"))
                sql += ",ai.Area Area";
            if (strExprotId.Contains("22"))
                sql += ",pi1.[Name] PName";
            if (strExprotId.Contains("23"))
                sql += ",si.SName";
            if (strExprotId.Contains("28") || strExprotId.Contains("29") || strExprotId.Contains("30"))
                sql += ",diAll.APName,diAll.AP_MAC,diAll.ID APID ";
            if (strExprotId.Contains("31"))
                sql += ",diAll.ProjectAccessNum";
            //if (strExprotId.Contains("32"))
            //    sql += ",diAll.PullOrTrade";
            //if (strExprotId.Contains("33"))
            //    sql += ",diAll.PotType";
            //if (strExprotId.Contains("34"))
            //    sql += ",diAll.CheckTime";
            sql += " FROM NetBarInfo nbi ";
            if (strExprotId.Contains("19"))
                sql += " LEFT JOIN areainfo ai11 on ai11.ID=nbi.ProID ";
            if (strExprotId.Contains("20"))
                sql += " LEFT JOIN areainfo ai12 on ai12.ID=nbi.CityID ";
            if (strExprotId.Contains("21"))
                sql += " LEFT JOIN areainfo ai on ai.ID=nbi.Aid ";
            if (strExprotId.Contains("22"))
                sql += " LEFT JOIN PoliceInfo pi1 ON pi1.ID = nbi.Pid ";
            if (strExprotId.Contains("23"))
                sql += " LEFT JOIN ScenicInfo si ON si.ID = nbi.[Sid]";
            if (strExprotId.Contains("28") || strExprotId.Contains("29") || strExprotId.Contains("30") || strExprotId.Contains("31") || !string.IsNullOrEmpty(dic["ProjectAccessNum"]) || dic["SelPotType"] != "0")
                sql += " LEFT JOIN DevInfo diAll on diAll.NETBAR_ID=nbi.id ";
            sql = sql + PotType;
            sql += ") nbi " + whereSql;
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, listParam.ToArray());
            return ds.Tables[0];
        }

        public Loc_NetBarInfo GetLocaById(int id, string TableKey)
        {
            string sql = "SELECT nbi.*,pi1.Name PName,si.SName FROM NetBarInfo nbi LEFT JOIN PoliceInfo pi1 ON nbi.Pid=pi1.ID LEFT JOIN ScenicInfo si ON nbi.[Sid]=si.ID where nbi.ID=@id";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@id", id));
            return new DataSetConvert(ds).Get_SingleModel<Loc_NetBarInfo>();
        }
        #endregion

        #region  批量导入场所和AP

        public int ImportLoca(ImportLocation il)
        {
            string sql = "INSERT INTO NetBarInfo (NETBAR_WACODE,PLACE_NAME,SITE_ADDRESS,LONGITUDE,LATITUDE,NETSITE_TYPE,BUSINESS_NATURE,LAW_PRINCIPAL_NAME,RELATIONSHIP_ACCOUNT,CODE_ALLOCATION_ORGANIZATION,Statis,Valid,Createtime,Verified,Service_code,ProID,CityID,Aid,Pid,[Sid],APNum,SecId)VALUES(@NETBAR_WACODE,@PLACE_NAME,@SITE_ADDRESS,@LONGITUDE,@LATITUDE,@NETSITE_TYPE,1,@LAW_PRINCIPAL_NAME,@RELATIONSHIP_ACCOUNT,'" + il.SECURITY_SOFTWARE_ORGCODE + "',1,1,GETDATE(),1,@Service_code,@ProID,@CityID,@Aid,@Pid,@Sid,@APNum,@SecId); SELECT SCOPE_IDENTITY();";
            SqlParameter[] param = new SqlParameter[] 
            { 
                new SqlParameter("@NETBAR_WACODE",il.NETBAR_WACODE),
                new SqlParameter("@PLACE_NAME",il.LocaName),
                new SqlParameter("@SITE_ADDRESS",il.Address),
                new SqlParameter("@NETSITE_TYPE",il.LocaType),
                new SqlParameter("@LONGITUDE",(il.ApType == 1 ? il.LONGITUDE : null)),
                new SqlParameter("@LATITUDE",(il.ApType == 1 ? il.LATITUDE : null)),
                new SqlParameter("@LAW_PRINCIPAL_NAME",il.UserName),
                new SqlParameter("@RELATIONSHIP_ACCOUNT",il.Mobile),
                new SqlParameter("@Service_code",il.Service_code),
                new SqlParameter("@ProID",il.ProId),
                new SqlParameter("@CityID",il.CityId),
                new SqlParameter("@Aid",il.Aid),
                new SqlParameter("@Pid",il.Pid),
                new SqlParameter("@Sid",il.Sid),
                new SqlParameter("@APNum",0),
                new SqlParameter("@SecId",(il.QecurityName.Contains("舜游")?8:1))
            };
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, param));
        }

        public int ImportDev(ImportLocation il)
        {
            string sql = "INSERT INTO DevInfo (COLLECTION_EQUIPMENT_ID,AP_MAC,NETBAR_WACODE,LONGITUDE,LATITUDE,NETBAR_ID,APType,Verified,CreateTime,DevStatis,Lguid,IsTrial,ProjectType,CasesType,ModeType,LogCapture,APName,Channel,IsReboot,ForcedOfflineTime,FenceOffTime,isopen,PastTime) VALUES(@COLLECTION_EQUIPMENT_ID,@AP_MAC,@NETBAR_WACODE,@LONGITUDE,@LATITUDE,@NETBAR_ID,@APType,1,GETDATE(),2,NEWID(),0,@ProjectType,@CasesType,1,7,@APName,11,0,30,0,1,999) ";
            SqlParameter[] param = new SqlParameter[] 
            { 
                new SqlParameter("@COLLECTION_EQUIPMENT_ID",il.SECURITY_SOFTWARE_ORGCODE+il.Mac.Replace("-","")),
                new SqlParameter("@AP_MAC",il.Mac),
                new SqlParameter("@NETBAR_WACODE",il.NETBAR_WACODE),
                new SqlParameter("@LONGITUDE",(il.ApType == 1 ? il.LONGITUDE : null)),
                new SqlParameter("@LATITUDE",(il.ApType == 1 ? il.LATITUDE : null)),
                new SqlParameter("@NETBAR_ID",il.NetBarId),
                new SqlParameter("@APType",il.ApType),
                new SqlParameter("@ProjectType",il.ProjectType),
                new SqlParameter("@CasesType",il.CasesType),
                new SqlParameter("@APName",il.ApName)
            };
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
        }

        public int updateImportDev(ImportLocation il)
        {
            string sql = "UPDATE DevInfo SET NETBAR_WACODE = @NETBAR_WACODE,NETBAR_ID = @NETBAR_ID,APType = @APType,ProjectType = @ProjectType,CasesType = @CasesType,APName = @APName,LONGITUDE=@LONGITUDE,LATITUDE=@LATITUDE,Verified=1 WHERE ID=@devId";
            SqlParameter[] param = new SqlParameter[] 
            { 
                new SqlParameter("@NETBAR_WACODE",il.NETBAR_WACODE),
                new SqlParameter("@LONGITUDE",(il.ApType == 1 ? il.LONGITUDE : null)),
                new SqlParameter("@LATITUDE",(il.ApType == 1 ? il.LATITUDE : null)),
                new SqlParameter("@devId",il.DveId),
                new SqlParameter("@NETBAR_ID",il.NetBarId),
                new SqlParameter("@APType",il.ApType),
                new SqlParameter("@ProjectType",il.ProjectType),
                new SqlParameter("@CasesType",il.CasesType),
                new SqlParameter("@APName",il.ApName)
            };
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
        }

        public int UpdateLocaApNumbyLid(string lid)
        {
            string sql = "UPDATE NetBarInfo SET APNum = (SELECT COUNT(0) FROM DevInfo di WHERE di.NETBAR_ID=NetBarInfo.id) WHERE ID IN (" + lid + ")";
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql);
        }

        #endregion
    }
}
