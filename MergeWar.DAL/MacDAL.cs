using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;
using HCZZ.ModeDB;
using Common;
using System.Data;
using System.Data.SqlClient;
using HCZZ.DAL;

namespace HCZZ.DAL
{
    /// <summary>
    /// 设备MAC 地址数据访问类
    /// </summary>
    public class MacDAL
    {
        #region Mac地址管理
        public PagedList<MacInfo> GetList(Dictionary<string, string> dic)
        {
            string sql = "";
            try
            {
                int pageIndex = Convert.ToInt32(dic["pageIndex"]);
                int pageSize = Convert.ToInt32(dic["pageSize"]);

                string txtMac = dic["AP_MAC"];
                string txtCOLLECTION_EQUIPMENT_ID = dic["COLLECTION_EQUIPMENT_ID"];
                string BeginTime = dic["BeginTime"];
                string EndTime = dic["EndTime"];

                List<SqlParameter> listParam = new List<SqlParameter>();

                string whereSql = " WHERE 1=1 ";

                if (!string.IsNullOrEmpty(txtMac))
                {
                    whereSql += " AND AP_MAC LIKE '%" + txtMac + "%'";
                }

                if (!string.IsNullOrEmpty(txtCOLLECTION_EQUIPMENT_ID))
                {
                    whereSql += " AND COLLECTION_EQUIPMENT_ID LIKE '%" + txtCOLLECTION_EQUIPMENT_ID + "%'";
                }

                if (!string.IsNullOrEmpty(BeginTime))
                    whereSql += " AND CreateTime >= '" + BeginTime + "'";
                if (!string.IsNullOrEmpty(EndTime))
                    whereSql += " AND CreateTime <= '" + EndTime + "'";

                SqlParameter[] param = listParam.ToArray();

                sql = "SELECT * FROM ( SELECT ROW_NUMBER () OVER (ORDER BY CreateTime DESC) PageNum, *  FROM DevInfo  " + whereSql + " ) temp WHERE temp.PageNum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;

                sql += " SELECT COUNT(0) FROM DevInfo " + whereSql;

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param);
                DataSetConvert convert = new DataSetConvert(ds);
                List<MacInfo> list = convert.Get_ListModel<MacInfo>();
                return new PagedList<MacInfo>(list, pageIndex, pageSize, Convert.ToInt32(ds.Tables[1].Rows[0][0]));
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","MacDAL.GetList(Dictionary<string,string> dic)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        public MacInfo GetMACInfoById(int Id, string TableKey)
        {
            string sql = "";
            try
            {
                sql = "SELECT * FROM " + TableKey + "DevInfo Where ID=@Id";
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Id", Id));
                return new DataSetConvert(ds).Get_SingleModel<MacInfo>();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","MacDAL.GetMACInfoById(int Id,string TableKey)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        public int InsertMac(MacInfo mac)
        {
            string sql = "";
            try
            {
                sql = " INSERT INTO " + mac.TableKey + "DevInfo(COLLECTION_EQUIPMENT_ID,AP_MAC,CreateTime)VALUES(@COLLECTION_EQUIPMENT_ID,@AP_MAC,GETDATE());SELECT @@IDENTITY;";
                SqlParameter[] param = new SqlParameter[] 
               { 
                    new SqlParameter("@AP_MAC",mac.AP_MAC),
                    new SqlParameter("@COLLECTION_EQUIPMENT_ID",mac.COLLECTION_EQUIPMENT_ID)
               };
                return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, param));
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","MacDAL.InsertMac(MacInfo mac)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        public int UpdateMac(MacInfo mac)
        {
            string sql = "";
            try
            {
                sql = "UPDATE " + mac.TableKey + "DevInfo SET	COLLECTION_EQUIPMENT_ID = @COLLECTION_EQUIPMENT_ID,	AP_MAC = @AP_MAC WHERE ID=@ID";
                SqlParameter[] param = new SqlParameter[] 
               { 
                    new SqlParameter("@COLLECTION_EQUIPMENT_ID",mac.COLLECTION_EQUIPMENT_ID),
                    new SqlParameter("@AP_MAC",mac.AP_MAC),
                    new SqlParameter("@ID",mac.ID)
               };
                return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","MacDAL.UpdateMac(MACInfo mac)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        /// <summary>
        /// 查询设备MAC信息表中设备编号/设备MAC是否存在
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="SN"></param>
        /// <param name="mac"></param>
        /// <returns></returns>
        public int GetMacSNOrMacCount(int Id, string COLLECTION_EQUIPMENT_ID, string AP_MAC, string TableKey)
        {
            string sql = "";
            try
            {
                sql = "SELECT COUNT(0) FROM " + TableKey + "DevInfo m WHERE 1=1 ";
                List<SqlParameter> list = new List<SqlParameter>();
                if (COLLECTION_EQUIPMENT_ID != "")
                {
                    sql += "  AND m.COLLECTION_EQUIPMENT_ID=@COLLECTION_EQUIPMENT_ID";
                    list.Add(new SqlParameter("@COLLECTION_EQUIPMENT_ID", COLLECTION_EQUIPMENT_ID));
                }
                else
                {
                    sql += "  AND m.AP_MAC=@AP_MAC";
                    list.Add(new SqlParameter("@AP_MAC", AP_MAC));
                }

                if (Id != 0)
                {
                    sql += "  AND m.ID!=@Id";
                    list.Add(new SqlParameter("@Id", Id));
                }
                SqlParameter[] param = list.ToArray();
                return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, param));
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","MacDAL.GetMacSNOrMacCount(int Id, string COLLECTION_EQUIPMENT_ID, string mac,string TableKey)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        /// <summary>
        /// 获取没有使用和已删除设备编号
        /// </summary>
        /// <returns></returns>
        public List<MacInfo> GetNoMacList()
        {
            string sql = "";
            try
            {
                //为了提高效率，直接找NETBAR_ID为null的数据
                //sql = "SELECT m.AP_MAC,m.COLLECTION_EQUIPMENT_ID,m.NETBAR_ID,m.ID FROM DevInfo m WHERE (m.NETBAR_ID NOT IN (SELECT li.ID FROM NetBarInfo li WHERE li.Valid=1) OR m.NETBAR_ID IS NULL) ORDER BY m.ID ASC ";
                sql = "SELECT m.AP_MAC,m.COLLECTION_EQUIPMENT_ID,m.NETBAR_ID,m.ID FROM DevInfo m WHERE  m.NETBAR_ID IS NULL ORDER BY m.ID ASC ";
                //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
                //return new DataSetConvert(ds).Get_ListModel<MacInfo>();
                List<MacInfo> list = SqlHelper.ExecuteListModel<MacInfo>(SqlHelper.DBConnStr, sql);
                return list;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","MacDAL.GetNoMacList()"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        public int UpdateMacById(MacInfo mac)
        {
            //COLLECTION_EQUIPMENT_ID=@SECURITY_SOFTWARE_ORGCODE+REPLACE(AP_MAC,'-','')
            string sql =
                "UPDATE DevInfo SET NETBAR_WACODE = (SELECT nbi.NETBAR_WACODE FROM NetBarInfo nbi WHERE nbi.ID=@NETBAR_ID),NETBAR_ID = @NETBAR_ID,Verified = 1,APType = @APType,ModeType = @ModeType,LogCapture = @LogCapture,supplier = @supplier,ProjectType = @ProjectType,CasesType = @CasesType,AP_ACSSES_IP=@AP_ACSSES_IP,IsTrial=@IsTrial,APName=@APName,LONGITUDE = @LONGITUDE,LATITUDE = @LATITUDE,[FLOOR] = @FLOOR,SUBWAY_STATION = @SUBWAY_STATION,SUBWAY_LINE_INFO = @SUBWAY_LINE_INFO,SUBWAY_VEHICLE_INFO = @SUBWAY_VEHICLE_INFO,SUBWAY_COMPARTMENT_NUMBER = @SUBWAY_COMPARTMENT_NUMBER,CAR_CODE = @CAR_CODE,Channel = @Channel,FenceOffTime = @FenceOffTime,ForcedOfflineTime = @ForcedOfflineTime,IsReboot = @IsReboot,V3CID=@V3CID WHERE ID=@ID";
            SqlParameter[] param = new SqlParameter[]{
                    new SqlParameter("@LONGITUDE", mac.LONGITUDE),
                    new SqlParameter("@LATITUDE", mac.LATITUDE),
                    new SqlParameter("@FLOOR", mac.FLOOR),
                    new SqlParameter("@SUBWAY_STATION", mac.SUBWAY_STATION),
                    new SqlParameter("@SUBWAY_LINE_INFO", mac.SUBWAY_LINE_INFO),
                    new SqlParameter("@SUBWAY_VEHICLE_INFO", mac.SUBWAY_VEHICLE_INFO),
                    new SqlParameter("@SUBWAY_COMPARTMENT_NUMBER", mac.SUBWAY_COMPARTMENT_NUMBER),
                    new SqlParameter("@CAR_CODE", mac.CAR_CODE),
                    new SqlParameter("@ID", mac.ID),
                    new SqlParameter("@NETBAR_ID", mac.NETBAR_ID),
                    new SqlParameter("@APType", mac.APType),
                    new SqlParameter("@ModeType", mac.ModeType),
                    new SqlParameter("@LogCapture", mac.LogCapture),
                    new SqlParameter("@supplier", mac.supplier),
                    new SqlParameter("@ProjectType", mac.ProjectType),
                    new SqlParameter("@CasesType", mac.CasesType),
                    new SqlParameter("@IsTrial", mac.IsTrial),
                    new SqlParameter("@APName", mac.APName),
                    new SqlParameter("@Channel", mac.Channel),
                    new SqlParameter("@FenceOffTime", mac.FenceOffTime),
                    new SqlParameter("@ForcedOfflineTime", mac.ForcedOfflineTime),
                    new SqlParameter("@V3CID", mac.V3CID),
                    new SqlParameter("@SECURITY_SOFTWARE_ORGCODE", mac.SECURITY_SOFTWARE_ORGCODE),
                    new SqlParameter("@IsReboot", mac.IsReboot),
                    new SqlParameter("@AP_ACSSES_IP",mac.AP_ACSSES_IP)
                    //new SqlParameter("@COLLECTION_EQUIPMENT_ID",mac.COLLECTION_EQUIPMENT_ID)
                };
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
        }

        /// <summary>
        /// 修改采集设备信息
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        public int UpdateMacFById(MacInfo mac)
        {
            string sql = "UPDATE Fench_DevInfo SET NETBAR_WACODE = (SELECT nbi.NETBAR_WACODE FROM Fench_NetBarInfo nbi WHERE nbi.ID=@NETBAR_ID),COLLECTION_EQUIPMENT_NAME = @COLLECTION_EQUIPMENT_NAME,COLLECTION_EQUIPMENT_ADDRESS = @COLLECTION_EQUIPMENT_ADDRESS,COLLECTION_EQUIPMENT_TYPE = @COLLECTION_EQUIPMENT_TYPE,SECURITY_SOFTWARE_ORGCODE = @SECURITY_SOFTWARE_ORGCODE,COLLECTION_EQUIPMENT_LONGITUDE = @COLLECTION_EQUIPMENT_LONGITUDE,COLLECTION_EQUIPMENT_LATITUDE = @COLLECTION_EQUIPMENT_LATITUDE,UPLOAD_TIME_INTERVAL = @UPLOAD_TIME_INTERVAL,COLLECTION_RADIUS = @COLLECTION_RADIUS,VEHICLE_CODE = @VEHICLE_CODE,SUBWAY_LINE_INFO = @SUBWAY_LINE_INFO,SUBWAY_VEHICLE_INFO = @SUBWAY_VEHICLE_INFO,SUBWAY_COMPARTMENT_NUMBER = @SUBWAY_COMPARTMENT_NUMBER,NETBAR_ID = @NETBAR_ID,Verified = 1,IsTrial = @IsTrial,supplier = @supplier,ProjectType = @ProjectType,CasesType = @CasesType,ModeType = @ModeType,LogCapture = @LogCapture WHERE ID=@ID";
            SqlParameter[] param = new SqlParameter[]{
                    new SqlParameter("@NETBAR_ID", mac.NETBAR_ID),
                    new SqlParameter("@COLLECTION_EQUIPMENT_NAME", mac.COLLECTION_EQUIPMENT_NAME),
                    new SqlParameter("@COLLECTION_EQUIPMENT_ADDRESS", mac.COLLECTION_EQUIPMENT_ADDRESS),
                    new SqlParameter("@COLLECTION_EQUIPMENT_TYPE", mac.COLLECTION_EQUIPMENT_TYPE),
                    new SqlParameter("@SECURITY_SOFTWARE_ORGCODE", mac.SECURITY_SOFTWARE_ORGCODE),
                    new SqlParameter("@COLLECTION_EQUIPMENT_LONGITUDE", mac.COLLECTION_EQUIPMENT_LONGITUDE),
                    new SqlParameter("@COLLECTION_EQUIPMENT_LATITUDE", mac.COLLECTION_EQUIPMENT_LATITUDE),
                    new SqlParameter("@UPLOAD_TIME_INTERVAL", mac.UPLOAD_TIME_INTERVAL),
                    new SqlParameter("@COLLECTION_RADIUS", mac.COLLECTION_RADIUS),
                    new SqlParameter("@VEHICLE_CODE", mac.VEHICLE_CODE),
                    new SqlParameter("@SUBWAY_LINE_INFO", mac.SUBWAY_LINE_INFO),
                    new SqlParameter("@SUBWAY_VEHICLE_INFO", mac.SUBWAY_VEHICLE_INFO),
                    new SqlParameter("@SUBWAY_COMPARTMENT_NUMBER", mac.SUBWAY_COMPARTMENT_NUMBER),
                    new SqlParameter("@IsTrial", mac.IsTrial),
                    new SqlParameter("@supplier", mac.supplier),
                    new SqlParameter("@ProjectType", mac.ProjectType),
                    new SqlParameter("@CasesType", mac.CasesType),
                    new SqlParameter("@ModeType", mac.ModeType),
                    new SqlParameter("@LogCapture", mac.LogCapture),
                    new SqlParameter("@ID", mac.ID)
                };

            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
        }

        public int UpdateMacVerifiedById(int Verified, int Id, int radDevStatis, string TableKey, int PotType)
        {
            string sql = "UPDATE " + TableKey + "DevInfo SET Verified = @Verified,PotType=@PotType";
            if (radDevStatis > 0 && radDevStatis < 4)
            {
                sql += ",DevStatis=@DevStatis";
            }
            sql += " WHERE ID=@ID";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID", Id),
                new SqlParameter("@Verified", Verified),
                new SqlParameter("@DevStatis", radDevStatis),
                new SqlParameter("@PotType",PotType)
            };

            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
        }

        public int UpdateMacCheckTimeById(int Id)
        {
            string sql = " UPDATE DevInfo SET CheckTime=GETDATE() WHERE ID=@ID";
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@ID", Id));
        }

        public int ResetDevInfoByLid(int Lid)
        {
            string sql = "UPDATE DevInfo SET NETBAR_WACODE = NULL,LONGITUDE = NULL,LATITUDE = NULL,[FLOOR] = NULL,SUBWAY_STATION = NULL,SUBWAY_LINE_INFO = NULL,SUBWAY_VEHICLE_INFO = NULL,SUBWAY_COMPARTMENT_NUMBER = NULL,CAR_CODE = NULL,NETBAR_ID = NULL,APType = NULL,DevStatis = 3,UpdateTime = NULL,AuditTime = NULL,ModeType = NULL,LogCapture = NULL,supplier = NULL,ProjectType = NULL,CasesType = NULL,IsTrial = NULL,APName = NULL WHERE NETBAR_ID=@Lid";
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Lid", Lid));
        }

        /// <summary>
        /// 根据场所ID获取MAC列表集合
        /// </summary>
        /// <param name="Lid"></param>
        /// <returns></returns>
        public List<SelMacInfo> GetListYYMac(int NETBAR_ID)
        {
            string sql = "SELECT di.id,di.APName Name FROM DevInfo di WHERE di.NETBAR_ID=@NETBAR_ID";
            //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@NETBAR_ID", NETBAR_ID));
            //return new DataSetConvert(ds).Get_ListModel<SelMacInfo>();
            List<SelMacInfo> list = SqlHelper.ExecuteListModel<SelMacInfo>(SqlHelper.DBConnStr, sql, new SqlParameter("@NETBAR_ID", NETBAR_ID));
            return list;
        }

        #endregion

        #region 获取区域信息集合

        public List<PoliceInfo> GetPoliceList(int aId, int key)
        {
            string sql = "";
            try
            {
                sql = "SELECT ID,NAME FROM PoliceInfo pi1 WHERE pi1.Aid=@aId AND pi1.Valid=1";
                //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@aId", aId));
                //return new DataSetConvert(ds).Get_ListModel<PoliceInfo>();
                List<PoliceInfo> list = SqlHelper.ExecuteListModel<PoliceInfo>(SqlHelper.DBConnStr, sql, new SqlParameter("@aId", aId));
                return list;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","MacDAL.GetPoliceList(int aId)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }


        public List<PoliceInfo> GetPoliceList(int aId)
        {
            return GetPoliceList(aId, 0);
        }

        /// <summary>
        /// 根据派出所Id获取所有景区
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public List<ScenicInfo> GetScenicList(int pId, int key)
        {
            string sql = "";
            try
            {
                sql = " SELECT ID,SName FROM ScenicInfo si WHERE si.PId=@pId AND si.Valid=1";
                //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@pId", pId));
                //return GetScenicList(pId, "pId");
                List<ScenicInfo> list = SqlHelper.ExecuteListModel<ScenicInfo>(SqlHelper.DBConnStr, sql, new SqlParameter("@pId", pId));
                return list;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","MacDAL.GetScenicList(int pId)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }


        public List<ScenicInfo> GetScenicList(int pId)
        {
            return GetScenicList(pId, 0);
        }

        /// <summary>
        /// 根据景区Id获取某派出所下的所有警区
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<ScenicInfo> GetScenicList(int Id, string key)
        {
            string sql = "";
            try
            {
                if (key == "pId")
                    sql = " SELECT ID,SName,si.Pid FROM ScenicInfo si WHERE si.PId=@Id AND si.Valid=1";
                else
                    sql = "SELECT ID,SName,si.Pid FROM ScenicInfo si WHERE si.Valid=1 AND si.PId=(SELECT si2.PId FROM ScenicInfo si2 WHERE si2.ID=@Id)";
                //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Id", Id));
                //return new DataSetConvert(ds).Get_ListModel<ScenicInfo>();
                List<ScenicInfo> list = SqlHelper.ExecuteListModel<ScenicInfo>(SqlHelper.DBConnStr, sql, new SqlParameter("@Id", Id));
                return list;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","MacDAL.GetScenicList(int Id, string key)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        /// <summary>
        /// 获取省、市、区信息集合
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CityType"></param>
        /// <returns></returns>
        public List<AreaInfo> GetAreaInfoList(int Id, int CityType, int key)
        {
            string sql = "";
            try
            {
                sql = "SELECT ID,Area Name FROM AreaInfo ai WHERE ai.CityType=" + CityType;
                if (CityType == 1 && Id > 0)
                    sql += " AND ai.ProID=" + Id;
                else if (CityType == 0 && Id > 0)
                    sql += " AND ai.Pid=" + Id;

                //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
                //return new DataSetConvert(ds).Get_ListModel<AreaInfo>();
                List<AreaInfo> list = SqlHelper.ExecuteListModel<AreaInfo>(SqlHelper.DBConnStr, sql);
                return list;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","MacDAL.GetAreaInfoList(int Id, int CityType)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        public List<AreaInfo> GetAreaInfoList(int Id, int CityType)
        {
            return GetAreaInfoList(Id, CityType, 0);
        }

        #endregion

        public List<MacInfo> GetWaringList()
        {
            string sql = "";
            try
            {
                sql = "SELECT top 10 di.UpdateTime,di.AuditTime,di.NETBAR_ID,di.NETBAR_WACODE,di.AP_MAC,di.APName,(SELECT nbi.PLACE_NAME FROM NetBarInfo nbi WHERE nbi.ID=di.NETBAR_ID AND nbi.Valid=1) LocaName FROM DevInfo di WHERE NETBAR_ID is not null and NETBAR_ID!='' and  DevStatis >1 order by UpdateTime  desc";
                //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
                //return new DataSetConvert(ds).Get_ListModel<MacInfo>();
                List<MacInfo> list = SqlHelper.ExecuteListModel<MacInfo>(SqlHelper.DBConnStr, sql);
                return list;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","MacDAL.GetWaringList()"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }


        public LocaGPSInfo GetLocaGPSInfo(int Id)
        {
            string sql = "";
            try
            {
                sql = "select top 1 * from LocaGPSInfo gps where   gps.WMacAddress=@Id ORDER BY QTime DESC";
                //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Id", Id));
                //return new DataSetConvert(ds).Get_SingleModel<LocaGPSInfo>();
                LocaGPSInfo LocaGPSInfo = SqlHelper.ExecuteModel<LocaGPSInfo>(SqlHelper.DBConnStr, sql, new SqlParameter("@Id", Id));
                return LocaGPSInfo;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","MacDAL.GetLocaGPSInfo(int Id)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        public string GetGPSInfo(int Id)
        {
            string sql = "";
            try
            {
                sql = "SELECT COLLECTION_EQUIPMENT_LONGITUDE,COLLECTION_EQUIPMENT_LATITUDE FROM Fench_TerminalPosOrbit ftpo WHERE ftpo.DevAP_ID =@Id ORDER BY TIME DESC";
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Id", Id)).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                    return dt.Rows[0][0] + "," + dt.Rows[0][0];
                else
                    return "";
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","MacDAL.GetGPSInfo(int Id)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }


        /// <summary>
        /// 获取Mac地址生产商信息
        /// </summary>
        /// <returns></returns>
        public List<MacInfo> GetBusNameList()
        {
            string sql = "";
            try
            {
                sql = "SELECT SN,BusName FROM BussmenInfo ";
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
                return new DataSetConvert(ds).Get_ListModel<MacInfo>();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","MacDAL.GetBusNameList()"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        /// <summary>
        /// 修改场所审计状态
        /// </summary>
        /// <param name="strId">场所Id</param>
        /// <param name="status">状态，1表示开启 0表示关闭</param>
        /// <returns></returns>
        public int UpdateIsOpenById(string strId, int status)
        {
            string sql = "";
            try
            {
                sql = "UPDATE DevInfo SET	isopen =@status WHERE ID IN (" + strId + ")";
                return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter[] { new SqlParameter("@status", status) });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","MacDAL.UpdateIsOpenById(string strId)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        /// <summary>
        /// 指定场所重新验证
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public int RecCheck(Dictionary<string, string> dic)
        {
            string sql = "";
            string txtAP_MAC = dic["txtAP_MAC"];
            string txtAP_MAC_ID = dic["txtAP_MAC_ID"];
            int ckType = Convert.ToInt32(dic["RecVal"]);
            string StrLid = dic["StrLid"];
            string PastTime = dic["PastTime"];
            string UserType = dic["UserType"].ToString();//用户类型
            string AreaId = dic["AreaId"].ToString();//用户区域
            int result = -1;

            if (ckType == 1)
                sql = "DELETE FROM TidMac_Ref WHERE MacAddress IN (SELECT DISTINCT tal.MAC FROM  TermianlAccessLog tal LEFT JOIN DevInfo di ON di.AP_MAC=tal.AP_MAC WHERE di.ID IN (" + StrLid + "))";
            else
                sql = "UPDATE DevInfo SET PastTime=" + PastTime + " WHERE ID IN (" + StrLid + ");";
            result = SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql);
            return result;
        }

        /// <summary>
        /// 根据设备Mac地址获取设备和场所信息
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        public DataTable GetNetBarMacInfoByMac(string mac)
        {
            string sql = "SELECT  nbi.SITE_ADDRESS,nbi.Verified,di.AP_MAC,ISNULL(di.UpdateTime,'') UpdateTime FROM NetBarInfo nbi LEFT JOIN DevInfo di ON nbi.ID=di.NETBAR_ID WHERE di.AP_MAC=@mac";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@mac", mac));
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
        /// <summary>
        /// 根据设备Mac地址查询未知设备表
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        public DataTable GetunKnowMacInfoByMac(string mac)
        {
            string sql = "SELECT AP_MAC,ISNULL(UpdateTime,'') UpdateTime,CreateTime,OutIpAddress,AreaName from UnknownDevice where AP_MAC=@mac";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@mac", mac.Replace("-", "")));
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// 根据AP_Mac判断该设备是否已经使用并返回相应的标识，如果没有使用并存在返回ID，如果存在已使用返回-2，如果不存在返回-1
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        public int GetDevIdByMac(string mac)
        {
            string sql = "declare @id int,@netbar_id int select @id=Id,@netbar_id=netbar_id from devinfo where ap_mac=@mac if(@netbar_id>0) select -2; else if(@netbar_id is null and @id >0) select @id; else if(@id is null) select -1";
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@mac", mac)));
        }
    }
}
