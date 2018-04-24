using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HCZZ.DAL
{
    public class HomeCountDAL
    {
        public DataTable DataCount()
        {
            string sql = "SELECT DataType AS name,SUM(DataNum) AS data FROM IndexStatisInfo WHERE StatisType=6 "+GetSqlStrByUser()+" GROUP BY DataType";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
            return ds.Tables[0];
        }
        /// <summary>
        /// 分组统计周月数据
        /// </summary>
        /// <param name="StarTime">开始时间</param>
        /// <param name="SumType">统计类别 1：采集数量趋势2：采集设备数量趋势</param>
        /// <returns></returns>
        public DataTable DataCountTrendDal(string StarTime, string SumType)
        {
            string EndTime = DateTime.Now.ToString("yyyyMMdd");
            string sql = "SELECT CONVERT(varchar(10),isi.WriteTime,120) as times,SUM(CASE isi.DataType WHEN '30' THEN isi.DataNum ELSE 0 END) AS 'phone',SUM(CASE isi.DataType WHEN '31' THEN isi.DataNum ELSE 0 END) AS 'mac',SUM(CASE isi.DataType WHEN '32' THEN isi.DataNum ELSE 0 END) AS 'virtuals',SUM(CASE isi.DataType WHEN '33' THEN isi.DataNum ELSE 0 END) AS 'imei',SUM(CASE isi.DataType WHEN '35' THEN isi.DataNum ELSE 0 END) AS ' carNum'FROM IndexStatisInfo AS isi WHERE isi.StatisType=" + SumType + "AND isi.WriteTime<=" + "'" + EndTime + "'" + "AND isi.WriteTime>=" + "'" + StarTime + "' " + GetSqlStrByUser() + "  GROUP BY convert(varchar(10),isi.WriteTime,120)";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
            return ds.Tables[0];
        }
        /// <summary>
        /// 场所类型在线数
        /// </summary>
        /// <returns></returns>
        public DataTable DataOnlineCount()
        {
            //string sql = "SELECT isi.DataType AS name, COUNT(isi.DataType) AS data FROM IndexStatisInfo AS isi ";
            //sql += "  WHERE isi.StatisType='3' GROUP BY isi.DataType ";
            string sql = "SELECT isi.DataType AS name, sum(isi.DataNum) AS data FROM IndexStatisInfo AS isi ";
            sql += "  WHERE isi.StatisType='3' " + GetSqlStrByUser() + " GROUP BY isi.DataType ";
            return SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql).Tables[0];
        }
        /// <summary>
        /// 场所类型在线率
        /// </summary>
        /// <returns></returns>
        public DataTable DataOnlineRate()
        {
            string sql = "SELECT isi.DataType AS name, sum(isi.ZoomNum) as CountNum,sum(isi.DataNum) AS LineNum ";
            sql += " FROM IndexStatisInfo AS isi WHERE   isi.StatisType='3' " + GetSqlStrByUser() + " GROUP BY isi.DataType";
            return SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql).Tables[0];
        }

        /// <summary>
        /// 设备数量，在线数量
        /// </summary>
        /// <returns></returns>
        public DataTable DataOnline()
        {
            string sql = "select sum(DataNum) as DataNum  ,sum(ZoomNum) as ZoomNum from IndexStatisInfo where StatisType=4 " + GetSqlStrByUser() + "";
            return SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql).Tables[0];
        }

        /// <summary>
        /// 地图下方数据
        /// </summary>
        /// <returns></returns>
        public DataTable DataMapInferior()
        {
            string sql = "SELECT ID,Mobile as Mobile,NETWORK_APP as NETWORK_APP,IdValue as IdValue,PLACE_NAME as PLACE_NAME FROM IndexStatisInfo WHERE StatisType=5 " + GetSqlStrByUser() + "";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
            return ds.Tables[0];
        }
        public string GetSqlStrByUser()
        {
            string sqlStr = string.Empty;
            int[] userType = Common.ChangeValue.GetUserType();
            if (userType[0] == 1 || userType[0] == 7)
                sqlStr = "";
            else
                sqlStr = userType[0] == 2 ? "AND Aid=" + userType[1] : userType[0] == 4 ? "AND CityID=" + userType[1] : "AND ProID=" + userType[1];
            return sqlStr;
        }
    }
}
