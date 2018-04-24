using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;
using HCZZ.ModeDB;
using System.Data.SqlClient;
using System.Data;
using HCZZ.Common;

namespace HCZZ.DAL
{
    public class HBHtmlDAL
    {
        #region 数据输出
        /// <summary>
        /// 查询数据输出列表管理信息
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public PagedList<OutPutInfo> GetList(Dictionary<string, string> dic)
        {
            string sql = "";
            try
            {
                int pageIndex = Convert.ToInt32(dic["pageIndex"]);
                int pageSize = Convert.ToInt32(dic["pageSize"]);

                string txtUserId = dic["UserId"];
                string txtContact = dic["Contact"];
                string txtQQ = dic["QQ"];
                string txtDestination = dic["Destination"];
                string selPattern = dic["Pattern"];

                List<SqlParameter> listParam = new List<SqlParameter>();
                string whereSql = " WHERE 1=1 ";

                if (!string.IsNullOrEmpty(txtUserId))
                {
                    whereSql += " AND  UserId LIKE '%" + txtUserId + "%'";
                }
                if (!string.IsNullOrEmpty(txtContact))
                {
                    whereSql += " AND  Contact LIKE '%" + txtContact + "%'";
                }
                if (!string.IsNullOrEmpty(txtQQ))
                {
                    whereSql += " AND  QQ LIKE '%" + txtQQ + "%'";
                }
                if (!string.IsNullOrEmpty(txtDestination))
                {
                    whereSql += " AND  Destination LIKE '%" + txtDestination + "%'";
                }
                if (selPattern != "0")
                {
                    whereSql += " AND Pattern = @Pattern ";
                    listParam.Add(new SqlParameter("@Pattern", selPattern));
                }

                SqlParameter[] param = listParam.ToArray();

                sql = "SELECT * FROM ( SELECT ROW_NUMBER () OVER (ORDER BY EstablishTime DESC) PageNum ,* FROM OutPutInfo " + whereSql + " ) temp WHERE temp.PageNum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param);
                sql = " SELECT COUNT(0) from OutPutInfo " + whereSql;

                int result = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, param));

                DataSetConvert convert = new DataSetConvert(ds);
                List<OutPutInfo> list = convert.Get_ListModel<OutPutInfo>();
                return new PagedList<OutPutInfo>(list, pageIndex, pageSize, result);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","HBHtmlDAL.GetList(Dictionary<string,string> dic)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        /// <summary>
        /// 添加数据输出管理信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int AddOutPutInfo(OutPutInfo info)
        {
            string sql = "";
            try
            {
                sql = "INSERT INTO OutPutInfo(Address,UserId,UserPwd,Contact,Email,Phone,QQ,Pattern,Destination,EstablishTime)VALUES(@Address,@UserId,@UserPwd,@Contact,@Email,@Phone,@QQ,@Pattern,@Destination,GETDATE())";
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@Address",info.Address),
                    new SqlParameter("@UserId",info.UserId),
                    new SqlParameter("@UserPwd",info.UserPwd),
                    new SqlParameter("@Contact",info.Contact),
                    new SqlParameter("@Email",info.Email),
                    new SqlParameter("@Phone",info.Phone),
                    new SqlParameter("@QQ",info.QQ),
                    new SqlParameter("@Pattern",info.Pattern),
                    new SqlParameter("@Destination",info.Destination)
                };
                return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","HBHtmlDAL.AddOutPutInfo(OutPutInfo info)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
        /// <summary>
        /// 获取数据输出管理详细信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public OutPutInfo GetOutPutId(int Id)
        {
            string sql = "";
            try
            {
                sql = "SELECT * FROM OutPutInfo WHERE @ID = Id ";
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Id", Id));
                return new DataSetConvert(ds).Get_SingleModel<OutPutInfo>();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","HBHtmlDAL.GetOutPutId(int Id)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int GetOutPutCountId(string Destination, int Id)
        {
            string sql = "SELECT count(0) FROM OutPutInfo WHERE Destination = @Destination ";
            if (Id > 0)
                sql += " AND Id != @Id ";
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter[] { new SqlParameter("@Destination", Destination), new SqlParameter("@Id", Id) }));
        }



        /// <summary>
        /// 查询安全厂商数量
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int GetAQCountId(string Orgcode, int Id)
        {
            string sql = "SELECT count(0) FROM SecurityOrg WHERE SECURITY_SOFTWARE_ORGCODE = @Orgcode ";
            if (Id > 0)
                sql += " AND Id != @Id ";
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter[] { new SqlParameter("@Orgcode", Orgcode), new SqlParameter("@Id", Id) }));
        }

        /// <summary>
        /// 修改数据输出管理信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int UpadteOutPut(OutPutInfo info)
        {
            string sql = "";
            try
            {
                sql = "UPDATE OutPutInfo SET Address = @Address,Contact = @Contact,UserId = @UserId,UserPwd = @UserPwd,Email = @Email,Phone = @Phone ,QQ = @QQ,Pattern = @Pattern,Destination = @Destination WHERE ID = @Id ";
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@Address",info.Address),
                    new SqlParameter("@Contact",info.Contact),
                    new SqlParameter("@UserId",info.UserId),
                    new SqlParameter("@UserPwd",info.UserPwd),
                    new SqlParameter("@Email",info.Email),
                    new SqlParameter("@Phone",info.Phone),
                    new SqlParameter("@QQ",info.QQ),
                    new SqlParameter("@Pattern",info.Pattern),
                    new SqlParameter("@Destination",info.Destination),
                    new SqlParameter("@Id",info.ID)
                };
                return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","HBHtmlDAL.UpadteOutPut(OutPutInfo info)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }



        /// <summary>
        /// 删除数据输出管理信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int DeleteOutPut(int Id)
        {
            string sql = "";
            try
            {
                sql = "DELETE FROM OutPutInfo WHERE ID=@Id";
                return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Id", Id));
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","HBHtmlDAL.DeleteOutPut(int Id)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
        #endregion


        /// <summary>
        /// 查询安全厂商列表管理信息
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public PagedList<SecurityOrg> GetAQList(Dictionary<string, string> dic)
        {
            string sql = "";
            try
            {
                int pageIndex = Convert.ToInt32(dic["pageIndex"]);
                int pageSize = Convert.ToInt32(dic["pageSize"]);

                string ORGNAME = dic["ORGNAME"];
                string ORGCODE = dic["ORGCODE"];

                List<SqlParameter> listParam = new List<SqlParameter>();
                string whereSql = " WHERE 1=1 ";

                if (!string.IsNullOrEmpty(ORGNAME))
                    whereSql += " AND  SECURITY_SOFTWARE_ORGNAME LIKE '%" + ORGNAME + "%'";
                if (!string.IsNullOrEmpty(ORGCODE))
                    whereSql += " AND  SECURITY_SOFTWARE_ORGCODE LIKE '%" + ORGCODE + "%'";

                SqlParameter[] param = listParam.ToArray();

                sql = "SELECT * FROM ( SELECT ROW_NUMBER () OVER (ORDER BY ID DESC) PageNum ,* FROM SecurityOrg " + whereSql + " ) temp WHERE temp.PageNum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param);
                sql = " SELECT COUNT(0) from SecurityOrg " + whereSql;

                int result = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, param));

                DataSetConvert convert = new DataSetConvert(ds);
                List<SecurityOrg> list = convert.Get_ListModel<SecurityOrg>();
                return new PagedList<SecurityOrg>(list, pageIndex, pageSize, result);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","HBHtmlDAL.GetAQList(Dictionary<string,string> dic)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        /// <summary>
        /// 获取安全厂商列表
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public List<SecurityOrg> GetAQList()
        {
            string sql = "";
            try
            {
                sql = "SELECT * FROM SecurityOrg ";

                //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
                //return new DataSetConvert(ds).Get_ListModel<SecurityOrg>();
                List<SecurityOrg> list = SqlHelper.ExecuteListModel<SecurityOrg>(SqlHelper.DBConnStr, sql);
                return list;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","HBHtmlDAL.GetAQList()"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        /// <summary>
        /// 添加安全厂商管理信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int InsertOrg(SecurityOrg sec)
        {
            string sql = "";
            try
            {
                sql = "INSERT INTO SecurityOrg(SECURITY_SOFTWARE_ORGNAME,SECURITY_SOFTWARE_ORGCODE,SECURITY_SOFTWARE_ORG_ADDRESS,CONTACTOR,CONTACTOR_TEL,CONTACTOR_MAIL,CreateTime)VALUES(@SECURITY_SOFTWARE_ORGNAME,@SECURITY_SOFTWARE_ORGCODE,@SECURITY_SOFTWARE_ORG_ADDRESS,@CONTACTOR,@CONTACTOR_TEL,@CONTACTOR_MAIL,getdate())";
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@SECURITY_SOFTWARE_ORGNAME",sec.SECURITY_SOFTWARE_ORGNAME),
                    new SqlParameter("@SECURITY_SOFTWARE_ORGCODE",sec.SECURITY_SOFTWARE_ORGCODE),
                    new SqlParameter("@SECURITY_SOFTWARE_ORG_ADDRESS",sec.SECURITY_SOFTWARE_ORG_ADDRESS),
                    new SqlParameter("@CONTACTOR",sec.CONTACTOR),
                    new SqlParameter("@CONTACTOR_TEL",sec.CONTACTOR_TEL),
                    new SqlParameter("@CONTACTOR_MAIL",sec.CONTACTOR_MAIL)
                };
                return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","HBHtmlDAL.InsertOrg(SecurityOrg sec)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
        /// <summary>
        /// 获取安全厂商管理详细信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public SecurityOrg GetSecId(int Id)
        {
            string sql = "";
            try
            {
                sql = "SELECT *,(SELECT count(0) FROM TermianlNetworkLog tnl WHERE tnl.SecId = so.id AND tnl.NETWORK_APP = '24' GROUP BY tnl.SecId) IMEINum,(SELECT count(0) FROM TermianlNetworkLog tnl WHERE tnl.SecId = so.id AND tnl.NETWORK_APP = '25' GROUP BY tnl.SecId) IMSINum,(SELECT COUNT(0) FROM TermianlNetworkLog tnl WHERE tnl.SecId = so.ID AND tnl.NETWORK_APP NOT IN(24, 25)) virtualNum,(SELECT count(0) FROM TermianlNetworkLog tnl WHERE tnl.SecId = so.id GROUP BY tnl.SecId) MACNum FROM SecurityOrg so WHERE SO.ID = @Id";
                //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Id", Id));
                SecurityOrg model = SqlHelper.ExecuteModel<SecurityOrg>(sql, new SqlParameter("@Id", Id));
                //return new DataSetConvert(ds).Get_SingleModel<SecurityOrg>();
                return model;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","HBHtmlDAL.GetSecId(int Id)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }



        /// <summary>
        /// 修改安全厂商管理信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int UpadteSec(SecurityOrg sec)
        {
            string sql = "";
            try
            {
                sql = "UPDATE SecurityOrg SET SECURITY_SOFTWARE_ORGNAME = @SECURITY_SOFTWARE_ORGNAME,SECURITY_SOFTWARE_ORGCODE = @SECURITY_SOFTWARE_ORGCODE,SECURITY_SOFTWARE_ORG_ADDRESS = @SECURITY_SOFTWARE_ORG_ADDRESS,CONTACTOR = @CONTACTOR,CONTACTOR_TEL = @CONTACTOR_TEL,CONTACTOR_MAIL = @CONTACTOR_MAIL  WHERE ID=@Id ";
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@SECURITY_SOFTWARE_ORGNAME",sec.SECURITY_SOFTWARE_ORGNAME),
                    new SqlParameter("@SECURITY_SOFTWARE_ORGCODE",sec.SECURITY_SOFTWARE_ORGCODE),
                    new SqlParameter("@SECURITY_SOFTWARE_ORG_ADDRESS",sec.SECURITY_SOFTWARE_ORG_ADDRESS),
                    new SqlParameter("@CONTACTOR",sec.CONTACTOR),
                    new SqlParameter("@CONTACTOR_TEL",sec.CONTACTOR_TEL),
                    new SqlParameter("@CONTACTOR_MAIL",sec.CONTACTOR_MAIL),
                    new SqlParameter("@Id",sec.ID)
                };
                return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","HBHtmlDAL.UpadteSec(SecurityOrg sec)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
        /////<summary>
        /////根据安全厂商管理ID统计场所信息 
        ///// </summary>
        ///// <param name="ID"></param>
        ///// <returns></returns>
        //public int GetOrgCountByID(int ID)
        //{
        //    string sql = "SELECT COUNT(0) FROM NetBarInfo WHERE SecId=@ID AND Valid=1";
        //    return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@ID", ID)));
        //}
        /// <summary>
        /// 删除安全厂商管理信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int DeleteSec(int Id)
        {
            string sql = "";
            try
            {
                sql = "DELETE FROM SecurityOrg WHERE ID=@Id";
                return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Id", Id));
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","HBHtmlDAL.DeleteSec(int Id)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
    }
}