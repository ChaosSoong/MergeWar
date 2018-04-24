using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HCZZ.Common;
using HCZZ.ModeDB;
using Webdiyer.WebControls.Mvc;

namespace HCZZ.DAL
{
  public  class OPLogDAL
    {

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public int InsertLog(OPLog log)
        {
            string sql = "";
           
                sql = "INSERT INTO OPLog(UID,Module,What,IpAddress,CreateTime) VALUES(@UID,@Module,@What,@IpAddress,GETDATE())";
                SqlParameter[] param = new SqlParameter[] 
                { 
                    new SqlParameter("@UID",log.UID),
                    new SqlParameter("@Module",log.Module),
                    new SqlParameter("@What",ChangeValue.Base64Str(1,log.What)),
                    new SqlParameter("@IpAddress",log.IpAddress)
                };
                return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
        }

        /// <summary>
        /// 日志列表
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public PagedList<OPLog> GetList(Dictionary<string, string> dic)
        {
            string sql = "";
            try
            {
                int pageIndex = Convert.ToInt32(dic["pageIndex"]);
                int pageSize = Convert.ToInt32(dic["pageSize"]);

                string txtName = dic["txtName"];
                string selName = dic["selName"].ToString();

                string txtIP = dic["txtAdderss"];
                string selIP = dic["selAdderss"].ToString();

                string beginTime = dic["CreateTime"];
                string endTime = dic["EndTime"];

                string what = dic["txtWhat"];

                List<SqlParameter> listParam = new List<SqlParameter>();

                string whereSql = " WHERE 1=1 ";

                if (selName == "1" && !string.IsNullOrEmpty(txtName))
                    whereSql += " AND u.UserName LIKE '%" + txtName + "%'";
                else if (selName == "2" && !string.IsNullOrEmpty(txtName))
                {
                    whereSql += " AND u.UserName =@UserName";
                    listParam.Add(new SqlParameter("@UserName", txtName));
                }

                if (selIP == "1" && !string.IsNullOrEmpty(txtIP))
                    whereSql += " AND l.IpAddress LIKE '%" + txtIP + "%'";
                else if (selIP == "2" && !string.IsNullOrEmpty(txtIP))
                {
                    whereSql += " AND l.IpAddress =@IpAddress";
                    listParam.Add(new SqlParameter("@IpAddress", txtIP));
                }

                if (!string.IsNullOrEmpty(beginTime) && !string.IsNullOrEmpty(endTime))
                {
                    whereSql += " AND l.CreateTime BETWEEN '" + beginTime + "' AND '" + endTime + "' ";
                }
                else
                {
                    if (!string.IsNullOrEmpty(beginTime))
                        whereSql += " AND l.CreateTime > '" + beginTime + "'";
                    if (!string.IsNullOrEmpty(endTime))
                        whereSql += " AND l.CreateTime < '" + endTime + "'";
                }


                if (!string.IsNullOrEmpty(what))
                {
                    whereSql += " AND l.What LIKE '%" + ChangeValue.Base64Str(1, what) + "%'";
                    listParam.Add(new SqlParameter("@What", ChangeValue.Base64Str(1, what)));
                }

                SqlParameter[] param = listParam.ToArray();

                sql = "SELECT * FROM ( SELECT ROW_NUMBER () OVER (ORDER BY l.ID DESC) PageNum ,l.*,u.UserName,l.What ShowWhat FROM OPLog l with(nolock) LEFT JOIN UserInfo u ON u.ID = l.UID " + whereSql + " ) temp WHERE temp.PageNum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;

                sql += " SELECT COUNT(0) FROM  OPLog l LEFT JOIN UserInfo u ON u.ID = l.UID " + whereSql;

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param);
                DataSetConvert convert = new DataSetConvert(ds);
                List<OPLog> list = convert.Get_ListModel<OPLog>();
                return new PagedList<OPLog>(list, pageIndex, pageSize, Convert.ToInt32(ds.Tables[1].Rows[0][0]));
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","OPLogDAL.GetList(Dictionary<string,string> dic)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        /// <summary>
        /// 获取操作日志详细信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public OPLog GetOPLogInfoById(int Id)
        {
            string sql = "";
            try
            {
                sql = "SELECT *,l. Module,What FROM OPLog l WHERE l.ID=@Id";
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Id", Id));
                return new DataSetConvert(ds).Get_SingleModel<OPLog>();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","OPLogDAL.GetOPLogInfoById(int Id)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
    }
}
