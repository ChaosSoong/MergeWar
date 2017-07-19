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
   public class UserInfoDAL
    {
        /// <summary>
        /// 查询列表和所有页面的路径信息，数据中包含当前页面和父页面路径
        /// </summary>
        /// <param name="Jid"></param>
        /// <returns></returns>
       public List<Sys_UserPowerInfo> GetUserPowerFSListToJid(int Jid)
        {
            /*
             * 1、查所有导航数据下的列表信息
             * 2、查所有列表下的所有路径
             * 3、将两次数据合并起来
             */
            string sql = "SELECT spi.FilePath FPath,sri.SelValues,spi.FilePath,spi.IdValues,spi.PageType,spi.Indexs FROM Sys_PowerInfo spi LEFT JOIN Sys_PowerInfo spi2 ON spi.pid=spi2.Id LEFT JOIN Sys_RelaInfo sri ON sri.Qid=spi.Id WHERE spi2.pid=0 AND sri.JId=@jid";
            sql += "  UNION ALL ";
            sql += "  SELECT (SELECT TOP 1 spi2.FilePath FROM Sys_PowerInfo spi2 where spi2.Id=spi.pid	)  FPath,sri2.SelValues,spi.FilePath,spi.IdValues,spi.PageType,spi.Indexs FROM Sys_PowerInfo spi LEFT JOIN Sys_RelaInfo sri2 ON sri2.Qid=spi.Pid   WHERE spi.pid IN( SELECT spi.Id FROM Sys_PowerInfo spi LEFT JOIN Sys_PowerInfo spi2 ON spi.pid=spi2.Id LEFT JOIN Sys_RelaInfo sri ON sri.Qid=spi.Id WHERE spi2.pid=0 AND sri.JId=@jid) AND sri2.JId=@jid";
            //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@jid", Jid));
            //return new DataSetConvert(ds).Get_ListModel<Sys_UserPowerInfo>();
            List<Sys_UserPowerInfo> list = SqlHelper.ExecuteListModel<Sys_UserPowerInfo>(sql, new SqlParameter("@jid", Jid));
            return list;
        }
       public List<Sys_UserPowerInfo> GetUserShowPageByJid(int Jid)
       {
           string sql =
               " SELECT sri.SelValues,sri2.Name TypeName,spi.*  FROM Sys_RelaInfo sri LEFT JOIN Sys_PowerInfo spi ON sri.Qid=spi.Id LEFT JOIN Sys_RoleInfo sri2 ON sri.JId=sri2.Id WHERE sri.Jid=@jid ";
            List<Sys_UserPowerInfo> list = SqlHelper.ExecuteListModel<Sys_UserPowerInfo>(sql, new SqlParameter("@jid", Jid));
            return list;
        }
       /// <summary>
       /// 登录方法
       /// </summary>
       /// <param name="Name"></param>
       /// <param name="pwd"></param>
       /// <returns></returns>
       public UserInfo GetUserInfoDSByLogin(string Name, string pwd)
       {
           SqlParameter[] param = null;
           string sql = " SELECT * FROM UserInfo ui WHERE Valid=1 AND ui.UserName=@UserName AND ui.[Password]=@Password";
           param = new SqlParameter[] { new SqlParameter("@UserName", Name), new SqlParameter("@Password", pwd) };
           //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param);
           //return new DataSetConvert(ds).Get_SingleModel<UserInfo>();
            UserInfo userinfo = SqlHelper.ExecuteModel<UserInfo>(sql, param);
            return userinfo;
       }
       /// <summary>
       /// 修改最后登录时间
       /// </summary>
       /// <param name="Id"></param>
       public void UpdateListtime(int Id)
       {
           string sql = "";
           try
           {
               sql = "UPDATE UserInfo SET	LastTime = GETDATE()	WHERE ID=@Id";
               SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Id", Id));
           }
           catch (Exception ex)
           {
               Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","UserInfoDAL.UpdateListtime(int Id)"},
                    {"SQL",sql}
                });
               throw ex;
           }
       }

       /// <summary>
       /// 用户列表
       /// </summary>
       /// <param name="dic"></param>
       /// <returns></returns>
       public PagedList<UserInfo> GetList(Dictionary<string, string> dic)
       {
           string sql = "";
           try
           {
               int pageIndex = Convert.ToInt32(dic["pageIndex"]);
               int pageSize = Convert.ToInt32(dic["pageSize"]);

               string txtName = dic["txtName"];
               string selName = dic["selName"].ToString();

               string txtMobile = dic["txtMobile"];
               string selMobile = dic["selMobile"].ToString();

               //string txtIdnumber = dic["txtIdnumber"];
               //string selIdnumber = dic["selIdnumber"].ToString();

               string txtTrueName = dic["txtTrueName"];
               string selTrueName = dic["selTrueName"].ToString();

                //string CreateId = dic["CreateId"].ToString();

                //string UserType = dic["UserType"].ToString();//用户类型
                string AreaId = dic["AreaId"].ToString();//用户区域

               List<SqlParameter> listParam = new List<SqlParameter>();

               string whereSql = " WHERE 1=1 ";

                //if (UserType != "0")
                //    whereSql += " AND Type = " + UserType;

                //if (CreateId == "0")
                //    whereSql += " AND u.Valid=1 ";

                if (selName == "1" && !string.IsNullOrEmpty(txtName))
                   whereSql += " AND  u.UserName LIKE '%" + txtName + "%'";
               else if (selName == "2" && !string.IsNullOrEmpty(txtName))
               {
                   whereSql += " AND  u.UserName =@UserName";
                   listParam.Add(new SqlParameter("@UserName", txtName));
               }

               if (selTrueName == "1" && !string.IsNullOrEmpty(txtTrueName))
                   whereSql += " AND u.TrueName LIKE '%" + txtTrueName + "%'";
               else if (selTrueName == "2" && !string.IsNullOrEmpty(txtTrueName))
               {
                   whereSql += " AND  u.TrueName =@TrueName";
                   listParam.Add(new SqlParameter("@TrueName", txtTrueName));
               }

               if (selMobile == "1" && !string.IsNullOrEmpty(txtMobile))
                   whereSql += " AND u.Mobile LIKE '%" + txtMobile + "%'";
               else if (selMobile == "2" && !string.IsNullOrEmpty(txtMobile))
               {
                   whereSql += " AND u.Mobile=@Mobile";
                   listParam.Add(new SqlParameter("@Mobile", txtMobile));
               }


               //if (selIdnumber == "1" && !string.IsNullOrEmpty(txtIdnumber))
               //    whereSql += " AND u.Idnumber LIKE '%" + txtIdnumber + "%'";
               //else if (selIdnumber == "2" && !string.IsNullOrEmpty(txtIdnumber))
               //{
               //    whereSql += " AND u.Idnumber=@Idnumber";
               //    listParam.Add(new SqlParameter("@Idnumber", txtIdnumber));
               //}

               //if (CreateId != "0")
               //{
               //    whereSql += " AND u.CreateId=@CreateId";
               //    listParam.Add(new SqlParameter("@CreateId", CreateId));
               //}

               SqlParameter[] param = listParam.ToArray();

               sql = "SELECT * FROM ( SELECT ROW_NUMBER () OVER (ORDER BY u.CreateTime DESC) PageNum ,u.*,pi1.Name Pname FROM UserInfo u LEFT JOIN PoliceInfo pi1 ON u.PId=pi1.ID " + whereSql + " ) temp WHERE temp.PageNum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;
               //sql += " SELECT COUNT(0) from UserInfo u LEFT JOIN PoliceInfo pi1 ON u.PId=pi1.ID " + whereSql;
                //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param);
                //DataSetConvert convert = new DataSetConvert(ds);
                List<UserInfo> list = SqlHelper.ExecuteListModel<UserInfo>(sql, param);
                sql = " SELECT COUNT(0) from UserInfo u LEFT JOIN PoliceInfo pi1 ON u.PId=pi1.ID " + whereSql;
                int Count =Convert.ToInt32( SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, param));
                return new PagedList<UserInfo>(list, pageIndex, pageSize, Count);
           }
           catch (Exception ex)
           {
               Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","UserInfoDAL.GetList(Dictionary<string,string> dic)"},
                    {"SQL",sql}
                });
               throw ex;
           }
       }

       /// <summary>
       /// 用户详细信息
       /// </summary>
       /// <param name="Name"></param>
       /// <returns></returns>
       public UserInfo GetUserInfoByName(string Name, int Id)
       {
           string sql = "";
           try
           {
               SqlParameter[] param = null;
               sql = "SELECT * FROM UserInfo WHERE UserName=@UserName AND Valid=1 ";
               if (Id != 0)
               {
                   sql += " AND ID <> @ID";
               }
               param = new SqlParameter[] { new SqlParameter("@UserName", Name), new SqlParameter("@ID", Id) };
                //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param);
                //return new DataSetConvert(ds).Get_SingleModel<UserInfo>();
                UserInfo userinfo = SqlHelper.ExecuteModel<UserInfo>(sql, param);
                return userinfo;
           }
           catch (Exception ex)
           {
               Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","UserInfoDAL.GetUserInfoByName(string Name)"},
                    {"SQL",sql}
                });
               throw ex;
           }
       }

       /// <summary>
       /// 添加用户
       /// </summary>
       /// <param name="user"></param>
       /// <returns></returns>
       public int InsertUser(UserInfo user)
       {
           string sql = "";
           try
           {
               sql = "INSERT INTO UserInfo(CreateTime,UserName,Password,TrueName,Mobile,Email,idType,idNumber,Type,Valid,AId,pId,ProID,CityID,CreateId,AddNum,JId)VALUES(@CreateTime,@UserName,@Password,@TrueName,@Mobile,@Email,@idType,@idNumber,@Type,1,@AID,@PID,@ProID,@CityID,@CreateId,0,@JId)";
               SqlParameter[] param = new SqlParameter[] 
               { 
                    new SqlParameter("@CreateTime",user.CreateTime),
                    new SqlParameter("@UserName",user.UserName),
                    new SqlParameter("@Password",user.Password),
                    new SqlParameter("@TrueName",user.TrueName),
                    new SqlParameter("@Mobile",user.Mobile),
                    new SqlParameter("@Email",user.Email),
                    new SqlParameter("@idType",user.idType),
                    new SqlParameter("@idNumber",user.idNumber),
                    new SqlParameter("@Type",user.Type),
                    new SqlParameter("@AID",user.AId),
                    new SqlParameter("@PID",user.PId),
                    new SqlParameter("@ProID",user.ProID),
                    new SqlParameter("@CityID",user.CityID),
                    new SqlParameter("@CreateId",user.CreateId),
                    new SqlParameter("@JId",user.JId), 
               };
               return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
           }
           catch (Exception ex)
           {
               Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","UserInfoDAL.InsertUser(UserInfo  user)"},
                    {"SQL",sql}
                });
               throw ex;
           }
       }

       /// <summary>
       /// 为该用户创建用户数加1
       /// </summary>
       /// <param name="uId"></param>
       /// <returns></returns>
       public void AddCreateUserNum(int uId)
       {
           string sql = "";
           try
           {
               sql = " UPDATE UserInfo SET AddNum = AddNum + 1 WHERE ID=@uId";
               SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@uId", uId));
           }
           catch (Exception ex)
           {
               Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","UserInfoDAL.AddCreateUserNum(int uId)"},
                    {"SQL",sql}
                });
               throw ex;
           }
       }

       /// <summary>
       /// 用户详细信息
       /// </summary>
       /// <param name="Id"></param>
       /// <returns></returns>
       public UserInfo GetUserInfoById(int Id)
       {
           string sql = "";
           try
           {
               SqlParameter[] param = null;
               sql = "SELECT * FROM UserInfo ui WHERE Valid=1 AND  ";
               //if (Id != 0)
               //{
                   sql += " ui.ID = @ID";
                   param = new SqlParameter[] { new SqlParameter("@ID", Id) };
                //}
                //else
                //{
                //    sql += "  ui.UserName=@UserName AND ui.[Password]=@Password";
                //    param = new SqlParameter[] { new SqlParameter("@UserName", Name), new SqlParameter("@Password", pwd) };
                //}
                //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param);
                UserInfo userinfo= SqlHelper.ExecuteModel<UserInfo>(sql,param);
                return userinfo;
           }
           catch (Exception ex)
           {
               Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","UserInfoDAL.GetUserInfoById(int Id, string Name, string pwd)"},
                    {"SQL",sql}
                });
               throw ex;
           }
       }

       /// <summary>
       /// 修用户信息
       /// </summary>
       /// <param name="vid"></param>
       /// <returns></returns>
       public int UPdateUser(UserInfo user)
       {
           string sql = "";
           try
           {
               sql = "UPDATE UserInfo SET TrueName = @TrueName,Mobile = @Mobile,Email = @Email,idType = @idType,idNumber = @idNumber,Type = @Type ,Aid = @Aid,Pid = @Pid,ProID = @ProID,CityID = @CityID";
               if (!string.IsNullOrEmpty(user.Password))
               {
                   sql += ",Password = @Password";
               }
               if(!string.IsNullOrEmpty(user.JId.ToString()))
               {
                   sql += ",JId = @JId";
               }
               sql += " WHERE ID = @Id";
               SqlParameter[] param = new SqlParameter[] 
                { 
                    new SqlParameter("@Id",user.ID),
                    new SqlParameter("@idType",user.idType),
                    new SqlParameter("@idNumber",user.idNumber),
                    new SqlParameter("@TrueName",user.TrueName),
                    new SqlParameter("@Mobile",user.Mobile),
                    new SqlParameter("@Email",user.Email),
                    new SqlParameter("@Type",user.Type),
                    new SqlParameter("@Password", user.Password),
                    new SqlParameter("@Aid",user.AId),
                    new SqlParameter("@Pid", user.PId),
                    new SqlParameter("@CityID", user.CityID),
                    new SqlParameter("@ProID", user.ProID),
                    new SqlParameter("@JId",user.JId)
                    //new SqlParameter("@isLog",user.isLog),
                    //new SqlParameter("@usbkey",user.USBKey)
                };
               return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
           }
           catch (Exception ex)
           {
               Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","UserInfoDAL.UPdateUser(UserInfo user)"},
                    {"SQL",sql}
                });
               throw ex;
           }
       }
       /// <summary>
       /// 修改删除标识
       /// </summary>
       /// <param name="Id"></param>
       /// <returns></returns>
       public int DeleteUser(int Id)
       {
           string sql = "";
           try
           {
               sql = "UPDATE UserInfo SET Valid = 0 WHERE ID=@Id";
               return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Id", Id));
           }
           catch (Exception ex)
           {
               Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","UserInfoDAL.Delete(int Id)"},
                    {"SQL",sql}
                });
               throw ex;
           }
       }

    }
}
