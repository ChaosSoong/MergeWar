using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using HCZZ.Common;
using HCZZ.ModeDB;
using System.Data.SqlClient;


namespace HCZZ.DAL
{
   public class PoliceDAL
    {
        /// <summary>
        /// 获取区域详细信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CityType">2：读取单条信息；1：读取自己和父区域的信息；0：读取区信息和所有父信息</param>
        /// <returns></returns>
        public AreaInfo GetAreaInfoById(int Id, int CityType, int key)
        {
            string sql = "";
            try
            {
                if (CityType == 2)
                    sql = "SELECT ai.ID,ai.Area,ai.Remark,ai.CreateTime,ai.Pid,ai.CityType,ai.ProID,ai.City_code,ai.Aguid FROM AreaInfo ai WHERE ai.ID=" + Id;
                else if (CityType == 1)
                    sql = "SELECT ai2.Area PName,ai2.Aguid Cguid,ai.ID,ai.Area,ai.Remark,ai.CreateTime,ai.Pid,ai.CityType,ai.ProID,ai.City_code FROM AreaInfo ai LEFT JOIN AreaInfo ai2 on ai.Pid=ai2.ID WHERE ai.ID=" + Id;
                else if (CityType == 0)
                    sql = "SELECT ai3.Area PName,ai3.Aguid Pguid,ai2.Aguid Cguid,ai2.Area CName,ai.ID,ai.Area,ai.Remark,ai.CreateTime,ai.Pid,ai.CityType,ai.ProID,ai.City_code,ai.Aguid  FROM AreaInfo ai LEFT JOIN AreaInfo ai2 on ai.Pid=ai2.ID LEFT JOIN AreaInfo ai3 ON ai3.ID=ai2.Pid WHERE ai.ID=" + Id;
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
                return new DataSetConvert(ds).Get_SingleModel<AreaInfo>();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","PoliceDAL.GetAreaInfoById(int Id, int CityType, int key)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
        /// <summary>
        /// 根据警区ID获取省市区派出所ID
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public DataTable GetAreaAllIdBySid(int sid)
        {
            string sql = "SELECT si.ID,si.PId,ai.ID AId,ai.Pid CityID,ai.ProID,ai.City_code,ai.Area,ai2.Area PName FROM ScenicInfo si LEFT JOIN PoliceInfo pi1 ON si.PId=pi1.ID LEFT JOIN AreaInfo ai ON pi1.Aid=ai.ID left join AreaInfo ai2 on ai.pid=ai2.ID WHERE si.ID=@sid";
            return SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@sid", sid)).Tables[0];
        }
        public AreaInfo GetAreaInfoById(int Id, int CityType)
        {
            return GetAreaInfoById(Id, CityType, 0);
        }
        /// <summary>
        /// 派出所详细信息
        /// </summary>
        /// <returns></returns>
        public PoliceInfo GetPoliceInfo(string Name, int Id, int key)
        {
            string sql = "";
            try
            {
                List<SqlParameter> listParam = new List<SqlParameter>();

                string whereSql = " WHERE 1=1 ";

                if (!string.IsNullOrEmpty(Name))
                {
                    whereSql += " AND p.Name=@Name AND p.ID<>@ID";
                    listParam.Add(new SqlParameter("@Name", Name));
                    listParam.Add(new SqlParameter("@ID", Id));
                }
                else
                {

                    if (Id != 0)
                    {
                        whereSql += " AND p.ID=@ID ";
                        listParam.Add(new SqlParameter("@ID", Id));
                    }
                }

                SqlParameter[] param = listParam.ToArray();

                sql = "SELECT p.*,a.Area FROM PoliceInfo p LEFT JOIN  AreaInfo a ON a.ID = p.AId " + whereSql;

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param);
                return new DataSetConvert(ds).Get_SingleModel<PoliceInfo>();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","PoliceDAL.GetPoliceInfo(string Name)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        public PoliceInfo GetPoliceInfo(string Name, int Id)
        {
            return GetPoliceInfo(Name, Id, 0);
        }

    }
}
