using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Common;
using HCZZ.ModeDB;
using Webdiyer.WebControls.Mvc;

namespace HCZZ.DAL
{
     public class VersionDAL
     {
         #region 角色管理
         /// <summary>
         /// 角色信息查询
         /// </summary>
         /// <param name=""></param>
         /// <returns></returns>
         public PagedList<Sys_UserPowerInfo> GetRoleList(Dictionary<string, string> dic)
         {
             string sql = "";
             int pageIndex = Convert.ToInt32(dic["PageIndex"]);
             int pageSize = Convert.ToInt32(dic["PageSize"]);

             string whereSql = " WHERE 1 = 1 ";
             List<SqlParameter> listParam = new List<SqlParameter>();

             if (!string.IsNullOrEmpty(dic["txtkeyword"]))
             {
                 whereSql += " and ri.Name like @keyword ";
                 listParam.Add(new SqlParameter("@keyword", "%" + dic["txtkeyword"] + "%"));
             }
             if (!string.IsNullOrEmpty(dic["CreateTime"]))
             {
                 whereSql += " AND ri.CreateTime >= @txtCreateTime";
                 listParam.Add(new SqlParameter("@txtCreateTime", dic["CreateTime"]));
             }
             if (!string.IsNullOrEmpty(dic["EndTime"]))
             {
                 whereSql += " AND ri.CreateTime <= @txtEndTime";
                 listParam.Add(new SqlParameter("@txtEndTime", dic["EndTime"]));
             }
             SqlParameter[] param = listParam.ToArray();
             sql = " SELECT * FROM (SELECT  ROW_NUMBER() OVER (ORDER BY ri.Id DESC) PageNum,ri.Id Jid,ri.Name RoleName,ri.CreateTime,(SELECT COUNT(0) FROM Sys_RelaInfo sri WHERE sri.JId=ri.Id)JNum,(SELECT COUNT(0) FROM UserInfo sui WHERE sui.Jid=ri.Id)JUserNum FROM Sys_RoleInfo ri " + whereSql + " )temp WHERE temp.PageNum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;
            //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param);
            List<Sys_UserPowerInfo> list = SqlHelper.ExecuteListModel<Sys_UserPowerInfo>(sql, param);
            sql = " SELECT COUNT(0) FROM Sys_RoleInfo ri " + whereSql;
             int result = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, param));
            //DataSetConvert convert = new DataSetConvert(ds);
            //List<Sys_UserPowerInfo> list = convert.Get_ListModel<Sys_UserPowerInfo>();
            //return new PagedList<Sys_UserPowerInfo>(list, pageIndex, pageSize, result);
            
            return new PagedList<Sys_UserPowerInfo>(list, pageIndex, pageSize, result);
        }

         /// <summary>
         /// 删除角色
         /// </summary>
         /// <param name="Jid"></param>
         /// <returns></returns>
         public int DeleteRoleById(int Jid)
         {
             string sql = "DELETE FROM Sys_RoleInfo Where Id=@id;DELETE FROM Sys_RelaInfo WHERE JId=@Jid";
             SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@id",Jid),
                new SqlParameter("@Jid",Jid)
            };
             return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr,CommandType.Text, sql, param);
         }

        /// <summary>
        /// 通过角色获取角色名称
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public Sys_UserPowerInfo GetUserRoleName(int Jid)
        {
            string sql = "SELECT * FROM Sys_RoleInfo WHERE Id=@id";
            DataSetConvert convert = new DataSetConvert(SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@id", Jid)));
            //return convert.Get_SingleModel<Sys_UserPowerInfo>();
            Sys_UserPowerInfo sysu = SqlHelper.ExecuteModel<Sys_UserPowerInfo>(sql, new SqlParameter("@id", Jid));
            return sysu;
        }
        /// <summary>
        /// 修改角色名称
        /// </summary>
        /// <param name="Jid"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public int ChangeRoleNameById(int Jid, string Name)
         {
             string sql = "UPDATE Sys_RoleInfo SET Name=@name WHERE Id=@id";
             SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@name",Name),
                new SqlParameter("@id",Jid)
            };
             return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
         }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int InsertRole(string name)
        {
            string sql = "INSERT INTO Sys_RoleInfo (Name,CreateTime) VALUES (@name,getdate())";
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter[] { new SqlParameter("@name", name) });
        }
        /// <summary>
        /// 判断角色名称是否存在
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool GetRoleNameCountByNameOrId(string Name, int Id)
         {
             string sql = "SELECT COUNT(0) FROM Sys_RoleInfo sri WHERE sri.Name =@Name";
             if (Id > 0) sql += " AND sri.Id!=@Id";
             int result = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter[] { new SqlParameter("@Name", Name), new SqlParameter("@Id", Id) }));
             return result > 0 ? true : false;
         }
         #endregion
         #region 角色关系

         /// <summary>
         /// 获取所有一级和二级页面信息
         /// </summary>
         /// <returns></returns>
         public List<Sys_UserPowerInfo> GetPowerList()
         {
             string sql = "SELECT spi.* FROM Sys_PowerInfo spi WHERE spi.Pid=0 UNION ALL SELECT spi.* FROM Sys_PowerInfo spi LEFT JOIN Sys_PowerInfo spi2 ON spi2.Id=spi.Pid WHERE spi2.Pid=0 ";
             DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr,CommandType.Text,sql);
             return new DataSetConvert(ds).Get_ListModel<Sys_UserPowerInfo>();
         }

         /// <summary>
         /// 获取角色所对应的权限列表
         /// </summary>
         /// <param name="Jid"></param>
         /// <returns></returns>
         public List<Sys_UserPowerInfo> GetPowerListByJid(int Jid)
         {
             string sql = "SELECT Qid,sri.SelValues,sri2.Name RoleName FROM Sys_RelaInfo sri LEFT JOIN Sys_RoleInfo sri2 ON sri.JId=sri2.Id WHERE sri.JId=@Jid";
             DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr,CommandType.Text, sql, new SqlParameter("@Jid", Jid));
             return new DataSetConvert(ds).Get_ListModel<Sys_UserPowerInfo>();
         }

         /// <summary>
         /// 删除权限对应关系
         /// </summary>
         /// <param name="Jid"></param>
         /// <returns></returns>
         public void DelRelaByJid(int Jid)
         {
             string sql = "DELETE FROM Sys_RelaInfo WHERE JId=@Jid";
             SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Jid", Jid));
         }

         /// <summary>
         /// 添加权限对应关系
         /// </summary>
         /// <param name="dic"></param>
         /// <returns></returns>
         public int InsertRelaGX(int Jid, Dictionary<string, string> dic)
         {
             string sql = "INSERT INTO Sys_RelaInfo(JId,Qid,SelValues,CreateTime)VALUES";
             List<string> list = new List<string>();
             foreach (var item in dic)
             {
                 list.Add("(@Jid," + item.Key + ",'" + item.Value + "',GETDATE())");
             }
             sql += string.Join(",", list);
             return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Jid", Jid));
         }
         #endregion
     }
}
