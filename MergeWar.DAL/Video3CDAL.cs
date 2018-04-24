using HCZZ.Common;
using HCZZ.ModeDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace HCZZ.DAL
{
    /* ==============================================================================
   * 创 建 者：邓佳训
   * 创建日期：2018/4/10 11:10:30
   * 功能描述：
   *
   * 修改者：         
   * 修改时间：       
   * 修改说明:
   * ==============================================================================*/
    public class Video3CDAL
    {
        public PagedList<Video3CInfo> GetVideo3CList(Dictionary<string, string> dic)
        {
            int pageIndex = Convert.ToInt32(dic["pageIndex"]);
            int pageSize = Convert.ToInt32(dic["pageSize"]);
            string UserType = dic["UserType"].ToString();//用户类型
            string AreaId = dic["AreaId"].ToString();//用户区域
            string whereSql = " where 1=1 ";
            List<SqlParameter> listParam = new List<SqlParameter>();

            if (UserType == "6")
                whereSql += " AND nbi.ProID=" + AreaId;
            else if (UserType == "4")
                whereSql += " AND nbi.CityID=" + AreaId;
            else if (UserType == "2")
                whereSql += " AND nbi.Aid=" + AreaId;

            if (!string.IsNullOrEmpty(dic["txtMac"]))
            {
                whereSql += " and di.AP_MAC like @DeviceMac";
                listParam.Add(new SqlParameter("@DeviceMac", "%" + dic["txtMac"] + "%"));
            }

            if (!string.IsNullOrEmpty(dic["txtAPName"]))
            {
                whereSql += " and di.APName like @APName";
                listParam.Add(new SqlParameter("@APName", "%" + dic["txtAPName"] + "%"));
            }

            if (!string.IsNullOrEmpty(dic["txtLName"]))
            {
                whereSql += " and nbi.PLACE_NAME like @Name";
                listParam.Add(new SqlParameter("@Name", "%" + dic["txtLName"] + "%"));
            }

            if (!string.IsNullOrEmpty(dic["txtNETBAR_WACODE"]))
            {
                whereSql += " and nbi.NETBAR_WACODE like @NETBAR_WACODE";
                listParam.Add(new SqlParameter("@NETBAR_WACODE", "%" + dic["txtNETBAR_WACODE"] + "%"));
            }
            if (!string.IsNullOrEmpty(dic["txtBeginTime"]))
            { whereSql += " and vc.BeginTime >= @BeginTime"; listParam.Add(new SqlParameter("@BeginTime", dic["txtBeginTime"])); }
            if (!string.IsNullOrEmpty(dic["txtEndTime"]))
            { whereSql += " and vc.BeginTime <= @EndTime"; listParam.Add(new SqlParameter("@EndTime", dic["txtEndTime"])); }


            string sql = "select * from (SELECT ROW_NUMBER() OVER ( ORDER BY vc.ID DESC) PageNum, vc.*,nbi.PLACE_NAME LName,di.APName FROM Video3CInfo vc LEFT JOIN NetBarInfo nbi ON vc.NETBAR_ID=nbi.ID LEFT JOIN DevInfo di ON di.ID=vc.DevAP_ID " + whereSql;
            sql += ") temp where temp.PageNum between " + ((pageIndex - 1) * pageSize + 1) + " and " + pageIndex * pageSize;
            sql += " select count(0) FROM Video3CInfo vc LEFT JOIN NetBarInfo nbi ON vc.NETBAR_ID=nbi.ID LEFT JOIN DevInfo di ON di.ID=vc.DevAP_ID " + whereSql;
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, listParam.ToArray());
            List<Video3CInfo> list = new DataSetConvert(ds).Get_ListModel<Video3CInfo>();
            return new PagedList<Video3CInfo>(list, pageIndex, pageSize, Convert.ToInt32(ds.Tables[1].Rows[0][0]));
        }

        /// <summary>
        /// 视频数据导出
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public DataTable GetVideoToTable(Dictionary<string, string> dic)
        {
            string whereSql = " where 1=1 ";
            string UserType = dic["UserType"].ToString();//用户类型
            string AreaId = dic["AreaId"].ToString();//用户区域
            List<SqlParameter> listParam = new List<SqlParameter>();

            if (UserType == "6")
                whereSql += " AND nbi.ProID=" + AreaId;
            else if (UserType == "4")
                whereSql += " AND nbi.CityID=" + AreaId;
            else if (UserType == "2")
                whereSql += " AND nbi.Aid=" + AreaId;

            if (!string.IsNullOrEmpty(dic["txtMac"]))
            {
                whereSql += " and di.AP_MAC like @DeviceMac";
                listParam.Add(new SqlParameter("@DeviceMac", "%" + dic["txtMac"] + "%"));
            }

            if (!string.IsNullOrEmpty(dic["txtAPName"]))
            {
                whereSql += " and di.APName like @APName";
                listParam.Add(new SqlParameter("@APName", "%" + dic["txtAPName"] + "%"));
            }

            if (!string.IsNullOrEmpty(dic["txtLName"]))
            {
                whereSql += " and nbi.PLACE_NAME like @Name";
                listParam.Add(new SqlParameter("@Name", "%" + dic["txtLName"] + "%"));
            }

            if (!string.IsNullOrEmpty(dic["txtNETBAR_WACODE"]))
            {
                whereSql += " and nbi.NETBAR_WACODE like @NETBAR_WACODE";
                listParam.Add(new SqlParameter("@NETBAR_WACODE", "%" + dic["txtNETBAR_WACODE"] + "%"));
            }
            if (!string.IsNullOrEmpty(dic["txtBeginTime"]))
            { whereSql += " and vc.BeginTime >= @BeginTime"; listParam.Add(new SqlParameter("@BeginTime", dic["txtBeginTime"])); }
            if (!string.IsNullOrEmpty(dic["txtEndTime"]))
            { whereSql += " and vc.BeginTime <= @EndTime"; listParam.Add(new SqlParameter("@EndTime", dic["txtEndTime"])); }


            string sql = "SELECT ROW_NUMBER() OVER ( ORDER BY vc.ID DESC) PageNum, vc.*,nbi.PLACE_NAME LName,di.APName FROM Video3CInfo vc LEFT JOIN NetBarInfo nbi ON vc.NETBAR_ID=nbi.ID LEFT JOIN DevInfo di ON di.ID=vc.DevAP_ID " + whereSql;
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, listParam.ToArray());
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据ID获取视频详细信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Video3CInfo GetVideo3CInfoById(int Id)
        {
            return DataITemplate.GetModelByKey<Video3CInfo>("Video3CInfo", "Id", Id);
        }
        public sealed class DataITemplate
        {
            /// <summary>
            /// 无查询条件查询表中所有数据
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="tableName"></param>
            /// <returns></returns>
            public static T GetModelByKey<T>(string tableName) where T : class, new()
            {
                string sql = "select * from " + tableName;
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
                return new DataSetConvert(ds).Get_SingleModel<T>();
            }

            /// <summary>
            /// 带条件具体查询表中数据
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="tableName"></param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static T GetModelByKey<T>(string tableName, object key, object value) where T : class, new()
            {
                string sql = "select * from " + tableName + " where " + key + "=@value";
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@value", value));
                return new DataSetConvert(ds).Get_SingleModel<T>();
            }

            /// <summary>
            /// 带条件模糊查询表中数据
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="tableName"></param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static T GetModelLikeByKey<T>(string tableName, object key, object value) where T : class, new()
            {
                string sql = "select * from " + tableName + " where " + key + " like '%" + value + "%'";
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
                return new DataSetConvert(ds).Get_SingleModel<T>();
            }

            /// <summary>
            /// 添加数据，并返回自增ID
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="model"></param>
            /// <returns></returns>
            public static int InsertModel(string TableName, Dictionary<object, object> dic)
            {
                string sql = "insert into " + TableName;
                List<object> listName = new List<object>();
                List<object> listValue = new List<object>();
                List<SqlParameter> listParam = new List<SqlParameter>();

                foreach (var item in dic)
                {
                    listName.Add(item.Key);
                    listValue.Add("@" + item.Key);
                    listParam.Add(new SqlParameter("@" + item.Key, item.Value));
                }
                sql += "(" + string.Join(",", listName) + ") values (";
                sql += string.Join(",", listValue) + "); SELECT @@IDENTITY;";
                SqlParameter[] param = listParam.ToArray();

                object obj = SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, param);
                if (obj != null && obj != DBNull.Value)
                    return Convert.ToInt32(obj);
                else
                    return 0;
            }
        }
    }
}