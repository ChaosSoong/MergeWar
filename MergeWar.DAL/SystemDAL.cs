using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using HCZZ.Common;
using HCZZ.ModeDB;
using Webdiyer.WebControls.Mvc;

namespace HCZZ.DAL
{
    public class SystemDAL
    {
        #region 省市区列表,添加,修改功能
        /// <summary>
        ///列表详细
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public PagedList<AreaInfo> GetAreaManage(Dictionary<string, string> dic)
        {
            string sql = "";
            try
            {
                int pageIndex = Convert.ToInt32(dic["pageIndex"]);
                int pageSize = Convert.ToInt32(dic["pageSize"]);
                string txtName = dic["txtName"];
                string selName = dic["selName"].ToString();
                string CityType = dic["CityType"].ToString();
                string ProId = dic.ContainsKey("ProId") ? dic["ProId"].ToString() : "";
                string Cid = dic.ContainsKey("Cid") ? dic["Cid"].ToString() : "";
                List<SqlParameter> listParam = new List<SqlParameter>();
                string whereSql = " WHERE ai.CityType=" + CityType;
                if (ProId != "")
                    whereSql += " AND ai.Pid = " + ProId;
                if (Cid != "")
                    whereSql += " AND ai.Pid = " + Cid;
                if (selName == "1" && !string.IsNullOrEmpty(txtName))
                    whereSql += " AND ai.Area LIKE '%" + txtName + "%'";
                if (selName == "2" && !string.IsNullOrEmpty(txtName))
                {
                    whereSql += " AND ai.Area=@Name";
                    listParam.Add(new SqlParameter("@Name", txtName));
                }
                SqlParameter[] param = listParam.ToArray();
                //if (CityType == "2")
                //{
                //    sql = "SELECT * FROM ( SELECT ROW_NUMBER () OVER (ORDER BY ai.ID DESC) PageNum,ai.ID, ai.Area, ai.CreateTime, ai.City_code, ai.CityType from AreaInfo ai " + whereSql + " ) temp WHERE temp.PageNum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;
                //    sql += " SELECT COUNT(0) from AreaInfo ai " + whereSql;
                //}
                //else
                //{
                //    sql = "SELECT * FROM ( SELECT  ROW_NUMBER () OVER (ORDER BY ai.ID DESC) PageNum,ai2.Area PName,ai.ID,ai.Area,ai.CreateTime,ai.Pid,ai.ProID,ai.City_code FROM AreaInfo ai LEFT JOIN AreaInfo ai2 on ai.Pid=ai2.ID " + whereSql + " ) temp WHERE temp.PageNum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;
                //    sql += " SELECT COUNT(0) FROM AreaInfo ai LEFT JOIN AreaInfo ai2 on ai.Pid=ai2.ID " + whereSql;
                //}
                if (CityType == "2")
                {
                    sql = "SELECT* FROM (SELECT ROW_NUMBER() OVER(ORDER BY ai.ID DESC) PageNum, ai.ID, ai.Area, ai.CreateTime, ai.City_code,ai.CityType,(SELECT COUNT(0) FROM AreaInfo AS ai1 WHERE ai1.Pid = ai.ID)Num from AreaInfo ai " + whereSql + " ) temp WHERE temp.PageNum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;
                    sql += " SELECT COUNT(0) from AreaInfo ai " + whereSql;
                }
                else if (CityType == "1")
                {
                    sql = "SELECT* FROM (SELECT ROW_NUMBER() OVER(ORDER BY ai.ID DESC) PageNum, ai2.Area PName,ai.ID,ai.Area,ai.CreateTime,ai.Pid,ai.ProID,ai.City_code,(SELECT COUNT(0) FROM AreaInfo AS ai1 WHERE ai1.Pid = ai.ID)Num FROM AreaInfo ai LEFT JOIN AreaInfo ai2 on ai.Pid=ai2.ID " + whereSql + " ) temp WHERE temp.PageNum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;
                    sql += " SELECT COUNT(0) FROM AreaInfo ai LEFT JOIN AreaInfo ai2 on ai.Pid=ai2.ID " + whereSql;
                }
                else if (CityType == "0")
                {
                    sql = "SELECT* FROM (SELECT ROW_NUMBER() OVER(ORDER BY ai.ID DESC) PageNum,ai2.Area PName,ai.ID, ai.Area, ai.CreateTime, ai.City_code,(SELECT COUNT(0) FROM PoliceInfo AS pi1 WHERE pi1.Aid = ai.ID AND pi1.Valid = 1)Num FROM AreaInfo ai LEFT JOIN AreaInfo ai2 on ai.Pid=ai2.ID  " + whereSql + " ) temp WHERE temp.PageNum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;
                    sql += " SELECT COUNT(0) FROM AreaInfo ai LEFT JOIN AreaInfo ai2 on ai.Pid=ai2.ID " + whereSql;
                }
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param);
                DataSetConvert convert = new DataSetConvert(ds);
                List<AreaInfo> list = convert.Get_ListModel<AreaInfo>();
                return new PagedList<AreaInfo>(list, pageIndex, pageSize, Convert.ToInt32(ds.Tables[1].Rows[0][0]));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据ID获取省市区页面详细
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CityType"></param>
        /// <returns></returns>
        public AreaInfo GetAreaInfoById(int Id, int CityType, int key)
        {
            string sql = "";
            try
            {
                if (CityType == 2)
                    sql = "SELECT ai.ID,ai.Area,ai.City_code,ai.Aguid FROM AreaInfo ai WHERE  Id=" + Id;
                if (CityType == 1)
                    sql = "SELECT ai2.Area PName,ai2.Aguid Cguid,ai.ID,ai.Area,ai.Pid,ai.ProID,ai.City_code FROM AreaInfo ai LEFT JOIN AreaInfo ai2 on ai.Pid=ai2.ID WHERE ai.ID=" + Id;
                if (CityType == 0)
                    sql = "SELECT ai3.Area PName,ai3.Aguid Pguid,ai2.Aguid Cguid,ai2.Area CName,ai.ID,ai.Area,ai.Remark,ai.CreateTime,ai.Pid,ai.CityType,ai.ProID,ai.City_code,ai.Aguid  FROM AreaInfo ai LEFT JOIN AreaInfo ai2 on ai.Pid=ai2.ID LEFT JOIN AreaInfo ai3 ON ai3.ID=ai2.Pid WHERE ai.ID=" + Id;
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
                return new DataSetConvert(ds).Get_SingleModel<AreaInfo>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AreaInfo GetAreaInfoById(int Id, int CityType)
        {
            return GetAreaInfoById(Id, CityType, 0);
        }

        /// <summary>
        /// 编辑省/市/区信息
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public bool UpdateProArea(AreaInfo area)
        {
            string sql = "";
            try
            {
                sql = "UPDATE AreaInfo SET Area = @Area,City_code=@City_code WHERE ID=@ID";
                SqlParameter[] param = new SqlParameter[]
                 {
                    new SqlParameter("@Area",area.Area),
                    new SqlParameter("@ID",area.ID),
                    new SqlParameter("@City_code",area.City_code)
                 };
                int result = SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","SystemDAL.UpdateProArea(AreaInfo area)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
        /// <summary>
        /// 添加省/市/区信息
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public int InsertProArea(AreaInfo area)
        {
            string sql = "";
            string strGUID = System.Guid.NewGuid().ToString().ToUpper();
            try
            {
                sql = "INSERT INTO AreaInfo(Area,CreateTime,Pid,CityType,ProID,City_code,Aguid)VALUES(@Area,GETDATE(),@Pid,@CityType,@ProID,@City_code,@Aguid);select @@Identity;";
                SqlParameter[] param = new SqlParameter[]
                 {
                    new SqlParameter("@Area",area.Area),
                    new SqlParameter("@Pid",area.Pid),
                    new SqlParameter("@CityType",area.CityType),
                    new SqlParameter("@ProID",area.ProID),
                    new SqlParameter("@City_code",area.City_code),
                    new SqlParameter("@Aguid",strGUID)
                 };
                int result = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, param));
                return result;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                {"Function","SystemDAL.InsertProArea(AreaInfo area)"},
                {"SQL",sql}
                });
                throw ex;
            }
        }
        /// <summary>
        /// 判断省代码/市代码是否存在
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Code"></param>
        /// <param name="CityType"></param>
        /// <returns></returns>
        public bool JudgeProExist(int Id, string City_code, int CityType)
        {
            string sql = "SELECT COUNT(0) FROM AreaInfo ai WHERE ai.CityType =@CityType and ai.City_code=@City_code";
            if (Id > 0)
                sql += " AND ai.ID!=@Id";
            int result = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter[] { new SqlParameter("@CityType", CityType), new SqlParameter("@City_code", City_code), new SqlParameter("@Id", Id) }));
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 判断省/市是否存在
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Code"></param>
        /// <param name="CityType"></param>
        /// <returns></returns>
        public bool JudgeProvince(string Area, int CityType, int Id, int pid)
        {
            string sql = "SELECT COUNT(0) FROM AreaInfo ai WHERE ai.Area =@Area and ai.CityType=@CityType";
            if (pid != 0)
                sql += " AND ai.Pid=@pid";
            if (Id != 0)
                sql += " AND ai.ID!=@Id";
            int result = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter[] { new SqlParameter("@CityType", CityType), new SqlParameter("@Area", Area), new SqlParameter("@pid", pid), new SqlParameter("@Id", Id) }));
            return result > 0 ? true : false;
        }
        #endregion

        #region 派出所列表,添加,修改功能
        public PagedList<PoliceInfo> GetPoliceInfoList(Dictionary<string, string> dic)
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

                string txtAddress = dic["txtAddress"];
                string selAddress = dic["selAddress"].ToString();

                string txtContact = dic["txtContact"];
                string selContact = dic["selContact"].ToString();

                int selArea = Convert.ToInt32(dic["Aid"].ToString());

                List<SqlParameter> listParam = new List<SqlParameter>();

                string whereSql = " WHERE p.Valid=1 ";

                if (selArea != 0)
                {
                    whereSql += " AND p.Aid=@Aid ";
                    listParam.Add(new SqlParameter("@Aid", selArea));
                }

                if (selName == "1" && !string.IsNullOrEmpty(txtName))
                    whereSql += " AND p.Name LIKE '%" + txtName + "%'";
                else if (selName == "2" && !string.IsNullOrEmpty(txtName))
                {
                    whereSql += " AND p.Name=@Name";
                    listParam.Add(new SqlParameter("@Name", txtName));
                }

                if (selMobile == "1" && !string.IsNullOrEmpty(txtMobile))
                    whereSql += " AND p.Mobile LIKE '%" + txtMobile + "%'";
                else if (selMobile == "2" && !string.IsNullOrEmpty(txtMobile))
                {
                    whereSql += " AND p.Mobile=@Mobile";
                    listParam.Add(new SqlParameter("@Mobile", txtMobile));
                }

                if (selAddress == "1" && !string.IsNullOrEmpty(txtAddress))
                    whereSql += " AND p.Address LIKE '%" + txtAddress + "%'";
                else if (selAddress == "2" && !string.IsNullOrEmpty(txtAddress))
                {
                    whereSql += " AND p.Address=@Address";
                    listParam.Add(new SqlParameter("@Address", txtAddress));
                }

                if (selContact == "1" && !string.IsNullOrEmpty(txtContact))
                    whereSql += " AND p.Contact LIKE '%" + txtContact + "%'";
                else if (selContact == "2" && !string.IsNullOrEmpty(txtContact))
                {
                    whereSql += " AND p.Contact=@Contact";
                    listParam.Add(new SqlParameter("@Contact", txtContact));
                }
                SqlParameter[] param = listParam.ToArray();
                sql = "SELECT * FROM (SELECT ROW_NUMBER () OVER (ORDER BY p.CreateTime DESC) PageNum, p.*,a.Area,(SELECT COUNT(0) FROM ScenicInfo AS si WHERE si.PId = p.ID AND si.Valid = 1)Num  FROM PoliceInfo p LEFT JOIN  AreaInfo a ON a.ID = p.AId " + whereSql + " ) temp WHERE temp.PageNum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;
                sql += " SELECT COUNT(0) from PoliceInfo p LEFT JOIN  AreaInfo a ON a.ID = p.AId " + whereSql;
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param);
                DataSetConvert convert = new DataSetConvert(ds);
                List<PoliceInfo> list = convert.Get_ListModel<PoliceInfo>();
                return new PagedList<PoliceInfo>(list, pageIndex, pageSize, Convert.ToInt32(ds.Tables[1].Rows[0][0]));
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","SystemDAL.GetPoliceInfoList(Dictionary<string,string> dic)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
        /// <summary>
        /// 添加派出所
        /// </summary>
        /// <param name="Police"></param>
        /// <returns></returns>
        public int InsertPoliceInfo(PoliceInfo PoliceInfo)
        {
            string sql = "";
            string strGUID = System.Guid.NewGuid().ToString().ToUpper();
            try
            {
                sql = "INSERT INTO PoliceInfo(CreateTime,[Name],Aid,Address,Mobile,Contact,Valid,PoliceInfo_code,Pguid)VALUES(@CreateTime,@Name,@Aid,@Address,@Mobile,@Contact,1,@PoliceInfo_code,@Pguid)";
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@CreateTime",PoliceInfo.CreateTime),
                    new SqlParameter("@Aid",PoliceInfo.Aid),
                    new SqlParameter("@Address",PoliceInfo.Address),
                    new SqlParameter("@Contact",PoliceInfo.Contact),
                    new SqlParameter("@Name",PoliceInfo.Name),
                    new SqlParameter("@Mobile",PoliceInfo.Mobile),
                    new SqlParameter("@PoliceInfo_code",PoliceInfo.PoliceInfo_code),
                    new SqlParameter("@Pguid",strGUID)
                };
                return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","SystemDAL.InsertPoliceInfo(PoliceInfo PoliceInfo)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        /// <summary>
        /// 查询该派出所是否存在
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Id"></param>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public bool GetIsTruePolice(string Name, int Id, int Aid)
        {
            string sql = "SELECT COUNT(0) FROM PoliceInfo pi1 WHERE pi1.Name=@Name AND pi1.Aid=@Aid";
            if (Id != 0)
                sql += " AND pi1.ID!=" + Id;
            int result = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql,
                new SqlParameter[] {
                    new SqlParameter("@Name", Name),
                    new SqlParameter("@Aid", Aid)
                }));
            return result > 0 ? true : false;

        }
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
        /// <summary>
        /// 根据ID获取派出所页面详细
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CityType"></param>
        /// <returns></returns>
        public PoliceInfo GetPoliceInfoById(int Id)
        {
            string sql = "";
            try
            {
                sql = "SELECT ai3.Area PName,ai3.Aguid Pguid,ai2.Aguid Cguid,ai2.Area CName,ai.Area,ai.Pid,ai.CityType,ai.ProID,";
                sql += "ai.City_code,ai.Aguid,pil.Name,pil.[Address],pil.Contact,pil.Mobile,pil.ID,pil.PoliceInfo_code,pil.Aid ";
                sql += "FROM AreaInfo ai LEFT JOIN AreaInfo ai2 on ai.Pid=ai2.ID LEFT JOIN AreaInfo ai3 ON ai3.ID=ai2.Pid ";
                sql += "LEFT JOIN PoliceInfo AS pil ON ai.ID=pil.Aid WHERE pil.ID=" + Id;
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
                return new DataSetConvert(ds).Get_SingleModel<PoliceInfo>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改派出所信息
        /// </summary>
        /// <param name="vid"></param>
        /// <returns></returns>
        public int UPdatePoliceInfo(PoliceInfo policeInfo)
        {
            string sql = "";
            try
            {
                sql = "UPDATE PoliceInfo SET [Name] = @Name,Mobile = @Mobile,Address = @Address,Contact = @Contact,PoliceInfo_code=@PoliceInfo_code WHERE ID = @Id";
                SqlParameter[] param = new SqlParameter[]
                 {
                    new SqlParameter("@Id",policeInfo.ID),
                    new SqlParameter("@Name",policeInfo.Name),
                    new SqlParameter("@Address",policeInfo.Address),
                    new SqlParameter("@Contact",policeInfo.Contact),
                    new SqlParameter("@Mobile",policeInfo.Mobile),
                    new SqlParameter("@PoliceInfo_code",policeInfo.PoliceInfo_code)
                 };
                return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","PoliceDAL.UPdatePolice(PoliceInfo police)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
        #endregion

        #region 警区列表,添加,修改功能
        /// <summary>
        /// 警区列表
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public PagedList<ScenicInfo> GetScenicInfoList(Dictionary<string, string> dic)
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
                string txtAddress = dic["txtAddress"];
                string selAddress = dic["selAddress"].ToString();
                string txtContact = dic["txtContact"];
                string selContact = dic["selContact"].ToString();
                int pid = Convert.ToInt32(dic["PId"]);
                List<SqlParameter> listParam = new List<SqlParameter>();
                string whereSql = " WHERE s.Valid=1 ";
                if (selName == "1" && !string.IsNullOrEmpty(txtName))
                {
                    whereSql += " AND s.SName LIKE '%" + txtName + "%'";
                }
                if (selName == "2" && !string.IsNullOrEmpty(txtName))
                {
                    whereSql += " AND s.SName=@Name";
                    listParam.Add(new SqlParameter("@Name", txtName));
                }
                if (selMobile == "1" && !string.IsNullOrEmpty(txtMobile))
                {
                    whereSql += " AND s.Mobile LIKE '%" + txtMobile + "%'";
                }
                if (selMobile == "2" && !string.IsNullOrEmpty(txtMobile))
                {
                    whereSql += " AND s.Mobile=@Mobile";
                    listParam.Add(new SqlParameter("@Mobile", txtMobile));
                }

                if (selAddress == "1" && !string.IsNullOrEmpty(txtAddress))
                {
                    whereSql += " AND s.Address LIKE '%" + txtAddress + "%'";
                }
                if (selAddress == "2" && !string.IsNullOrEmpty(txtAddress))
                {
                    whereSql += " AND s.Address=@Address";
                    listParam.Add(new SqlParameter("@Address", txtAddress));
                }

                if (selContact == "1" && !string.IsNullOrEmpty(txtContact))
                {
                    whereSql += " AND s.Contact LIKE '%" + txtContact + "%'";
                }
                if (selContact == "2" && !string.IsNullOrEmpty(txtContact))
                {
                    whereSql += " AND s.Contact=@Contact";
                    listParam.Add(new SqlParameter("@Contact", txtContact));
                }
                whereSql += " AND s.PID=@PID";
                listParam.Add(new SqlParameter("@PID", pid));
                SqlParameter[] param = listParam.ToArray();
                sql = "SELECT * FROM ( SELECT ROW_NUMBER () OVER (ORDER BY s.CreateTime DESC) PageNum, s.* FROM ScenicInfo s " + whereSql + " ) temp WHERE temp.PageNum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;
                sql += " SELECT COUNT(0) FROM  ScenicInfo s " + whereSql;
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param);
                DataSetConvert convert = new DataSetConvert(ds);
                List<ScenicInfo> list = convert.Get_ListModel<ScenicInfo>();
                return new PagedList<ScenicInfo>(list, pageIndex, pageSize, Convert.ToInt32(ds.Tables[1].Rows[0][0]));
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","SystemDAL.GetScenicInfoList(Dictionary<string,string> dic)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
        /// <summary>
        /// 添加警区
        /// </summary>
        /// <param name="Scenic"></param>
        /// <returns></returns>
        public int InsertScenicInfo(ScenicInfo ScenicInfo)
        {
            string sql = "";
            string strGUID = System.Guid.NewGuid().ToString().ToUpper();
            try
            {
                sql = "INSERT INTO ScenicInfo(CreateTime,SName,Pid,Address,Mobile,Contact,Valid,Sguid)VALUES(@CreateTime,@Name,@Pid,@Address,@Mobile,@Contact,1,@Sguid)";
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@CreateTime",ScenicInfo.CreateTime),
                    new SqlParameter("@Pid",ScenicInfo.Pid),
                    new SqlParameter("@Address",ScenicInfo.Address),
                    new SqlParameter("@Contact",ScenicInfo.Contact),
                    new SqlParameter("@Name",ScenicInfo.SName),
                    new SqlParameter("@Mobile",ScenicInfo.Mobile),
                    new SqlParameter("@Sguid",strGUID)
                };
                return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","SystemDAL.InsertScenicInfo(ScenicInfo ScenicInfo)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        /// <summary>
        /// 判断该警区是否存在
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Id"></param>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public bool GetScenicInfo(string Name, int Id, int Pid)
        {
            string sql = "SELECT count(0) FROM ScenicInfo  WHERE SName =@Name AND PId=@Pid";
            if (Id != 0)
                sql += " AND ID!=" + Id;
            int result = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql,
                new SqlParameter[] { new SqlParameter("@Name", Name),
               new SqlParameter("@Pid", Pid) }));
            return result > 0 ? true : false;

        }

        /// <summary>
        /// 根据ID获取警区详细
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CityType"></param>
        /// <returns></returns>
        public ScenicInfo GetScenicInfoById(int Id)
        {
            string sql = "";
            try
            {
                sql = "SELECT ai3.Area PName,ai3.Aguid Pguid,ai2.Aguid Cguid,ai2.Area CName,ai.Area,";
                sql += "ai.CityType,ai.ProID,ai.City_code,ai.Aguid,pi1.Name,si.SName,si.[Address],si.Contact,si.Mobile,si.Sguid,si.ID,si.Pid  FROM AreaInfo ai ";
                sql += "LEFT JOIN AreaInfo ai2 on ai.Pid=ai2.ID  LEFT JOIN AreaInfo ai3 ON ai3.ID=ai2.Pid LEFT JOIN ";
                sql += "PoliceInfo AS pi1 ON ai.ID=pi1.Aid  LEFT JOIN ScenicInfo AS si ON si.PId=pi1.ID WHERE si.ID=" + Id;
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
                return new DataSetConvert(ds).Get_SingleModel<ScenicInfo>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改警区信息
        /// </summary>
        /// <param name="vid"></param>
        /// <returns></returns>
        public int UPdateScenicInfo(ScenicInfo scenicInfo)
        {
            string sql = "";
            try
            {
                sql = "UPDATE ScenicInfo SET SName = @Name,Mobile = @Mobile,Address = @Address,Pid = @Pid,Contact = @Contact WHERE ID = @Id";
                SqlParameter[] param = new SqlParameter[]
                 {
                    new SqlParameter("@Id",scenicInfo.ID),
                    new SqlParameter("@Name",scenicInfo.SName),
                    new SqlParameter("@Address",scenicInfo.Address),
                    new SqlParameter("@Pid",scenicInfo.Pid),
                    new SqlParameter("@Contact",scenicInfo.Contact),
                    new SqlParameter("@Mobile",scenicInfo.Mobile)
                 };
                return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","SystemDAL.UPdateScenicInfo(ScenicInfo scenicInfo)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        #endregion

        #region 系统参数配置
        /// <summary>
        /// 系统参数配置列表
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public List<SystemConfigInfo> GetSysConfigList()
        {
            string sql = "";
            try
            {
                sql = "SELECT * FROM SystemConfigInfo sci";
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
                return new DataSetConvert(ds).Get_ListModel<SystemConfigInfo>();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","VidTypeDAL.GetSysConfigList()"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        /// <summary>
        /// 获取全部的系统参数配置类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetSysConfigTable()
        {
            string sql = "";
            try
            {
                sql = "SELECT sci.ConfigType,sci.ConfigValue FROM SystemConfigInfo sci ORDER BY sci.ConfigType ASC";
                return SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql).Tables[0];
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","VidTypeDAL.GetSysConfigTable()"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
        /// <summary>
        /// 修改系统配置信息
        /// </summary>
        /// <param name="sysconfig"></param>
        /// <returns></returns>
        public int UpdateSysConfig(Dictionary<int, string> dic)
        {
            string sql = "";
            try
            {
                List<SqlParameter> list = new List<SqlParameter>();
                int i = 0;
                foreach (var item in dic)
                {
                    sql += string.Format(" UPDATE SystemConfigInfo SET ConfigValue=@ConfigValue{0} WHERE ConfigType=@ConfigType{1}", i, i);
                    list.Add(new SqlParameter("@ConfigType" + i, item.Key));
                    list.Add(new SqlParameter("@ConfigValue" + i, item.Value));
                    i++;
                }

                SqlParameter[] param = list.ToArray();
                return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","VidTypeDAL.UpdateSysConfig(SystemConfigInfo sysconfig)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
        #endregion

        #region 厂商数据管理
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
                string ORGNAME = dic["orgName"];
                string ORGCODE = dic["orgCode"];
                List<SqlParameter> listParam = new List<SqlParameter>();
                string whereSql = " WHERE 1=1 ";
                if (!string.IsNullOrEmpty(ORGNAME))
                    whereSql += " AND  SECURITY_SOFTWARE_ORGNAME LIKE '%" + ORGNAME + "%'";
                if (!string.IsNullOrEmpty(ORGCODE))
                    whereSql += " AND  SECURITY_SOFTWARE_ORGCODE LIKE '%" + ORGCODE + "%'";
                SqlParameter[] param = listParam.ToArray();
                sql = "SELECT * FROM ( SELECT ROW_NUMBER () OVER (ORDER BY ID DESC) PageNum ,*,(SELECT count(0) FROM TermianlNetworkLog tnl WHERE tnl.SecId=so.id AND tnl.NETWORK_APP='24' GROUP BY tnl.SecId) IMEINum,(SELECT count(0) FROM TermianlNetworkLog tnl WHERE tnl.SecId = so.id AND tnl.NETWORK_APP = '25' GROUP BY tnl.SecId) IMSINum,(SELECT COUNT(0) FROM TermianlNetworkLog tnl WHERE tnl.SecId = so.ID AND tnl.NETWORK_APP NOT IN(24, 25)) virtualNum,(SELECT count(0) FROM TermianlNetworkLog tnl WHERE tnl.SecId = so.id GROUP BY tnl.SecId) MACNum FROM SecurityOrg so " + whereSql + " ) temp WHERE temp.PageNum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;
                //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param);
                List<SecurityOrg> list = SqlHelper.ExecuteListModel<SecurityOrg>(sql, param);
                sql = " SELECT COUNT(0) from SecurityOrg as si" + whereSql;
                int result = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, param));
                //DataSetConvert convert = new DataSetConvert(ds);
                //List<SecurityOrg> list = convert.Get_ListModel<SecurityOrg>();
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

        public List<SecurityOrg> GetFirmNameDal()
        {
            string sql = "SELECT ID, SECURITY_SOFTWARE_ORGNAME FROM SecurityOrg";
            return SqlHelper.ExecuteListModel<SecurityOrg>(sql);
        }
        #endregion

        #region 区域管理删除功能
        public int GetAreaCountByID(int ID, string key)
        {
            string sql = "";
            //省
            if (key == "AreaManage")
                sql = "select count(0) from areainfo where Pid=@ID and cityType=1";
            //市
            if (key == "AreaManageCity")
                sql = "select count(0) from areainfo where Pid=@ID and cityType=0";
            //区
            if (key == "Area")
                sql = "select count(0) from policeInfo where aid=@ID and valid=1";
            //派出所
            if (key == "Police")
                sql = "select count(0) from ScenicInfo where Pid=@ID and valid=1";
            //警区
            if (key == "Scenic")
                sql = "SELECT COUNT(0) FROM NetBarInfo WHERE Sid=@ID AND Valid=1";
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@ID", ID)));
        }

        public int DeleteProArea(int Id, string key)
        {
            string sql = "";
            try
            {
                if (key == "AreaManage")
                    sql = "delete FROM AreaInfo  WHERE ID=@Id ";
                if (key == "AreaManageCity")
                    sql = "DELETE FROM AreaInfo WHERE ID=@Id ";
                if (key == "Area")
                    sql = "DELETE FROM AreaInfo WHERE ID=@Id ";
                if (key == "Police")
                    sql = "update PoliceInfo set Valid=0 WHERE ID=@Id";
                if (key == "Scenic")
                    sql = "update ScenicInfo set Valid=0 WHERE ID=@Id";
                return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Id", Id));
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","SystemDAL.DeleteProArea(int Id)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
        #endregion

        #region 终端版本管理
        /// <summary>
        /// 列表详情
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public PagedList<VersionInfo> GetVersionList(Dictionary<string, string> dic)
        {
            string sql = "";
            try
            {
                int pageIndex = Convert.ToInt32(dic["pageIndex"]);
                int pageSize = Convert.ToInt32(dic["pageSize"]);
                sql = "SELECT * FROM (	SELECT ROW_NUMBER() OVER (ORDER BY vi.CreateTime DESC) PageNum,* FROM VersionInfo vi) temp WHERE temp.PageNum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageSize * pageIndex;
                sql += " SELECT COUNT(0) FROM VersionInfo vi";
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
                DataSetConvert convert = new DataSetConvert(ds);
                List<VersionInfo> list = convert.Get_ListModel<VersionInfo>();
                return new PagedList<VersionInfo>(list, pageIndex, pageSize, Convert.ToInt32(ds.Tables[1].Rows[0][0]));
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","SystemDAL.GetVersionList(Dictionary<string,string> dic)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
        /// <summary>
        /// 添加终端版本
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public int InsertVersionInfo(VersionInfo version)
        {
            string sql = "";
            try
            {
                sql = "INSERT INTO VersionInfo(Type,CasesType,CreateTime,OutVersion,Version,ChangeLog,";
                sql += "DownloadUrl,FocusUpdate,Aid,UpdateType,otherType,VerifyType)VALUES(@Type,@CasesType,";
                sql += "GETDATE(),@OutVersion,@Version,@ChangeLog,@DownloadUrl,@FocusUpdate,@Aid,";
                sql += "@UpdateType,@otherType,@VerifyType)";
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@Type",version.Type),
                    new SqlParameter("@OutVersion",version.OutVersion),
                    new SqlParameter("@Version",version.Version),
                    new SqlParameter("@ChangeLog",version.ChangeLog),
                    new SqlParameter("@DownloadUrl",version.DownloadUrl),
                    new SqlParameter("@CasesType",version.CasesType),
                    new SqlParameter("@FocusUpdate",version.FocusUpdate),
                    new SqlParameter("@Aid",version.Aid),
                    new SqlParameter("@UpdateType",version.UpdateType),
                    new SqlParameter("@otherType",version.otherType),
                    new SqlParameter("@VerifyType",version.VerifyType)
                };
                return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","SystemDAL.InsertVersionInfo(VersionInfo version)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
        /// <summary>
        /// 判断版本是否存在
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Id"></param>
        /// <param name="Aid"></param>
        /// <returns></returns>
        public VersionInfo GetVersionInfo(int Type, string txtOutVersion, int CasesType, int Id)
        {

            string sql = "";
            try
            {
                SqlParameter[] param = null;
                sql = "SELECT * FROM VersionInfo vi WHERE 1=1";
                if (Id != 0)
                    sql += " AND vi.ID <> @Id";

                if (Type != 0)
                {
                    sql += " AND vi.Type=@Type";
                    if (Type == 2)
                        sql += "  AND vi.CasesType=@CasesType";
                }
                if (txtOutVersion != "")
                    sql += " AND vi.OutVersion=@OutVersion";
                param = new SqlParameter[] { new SqlParameter("@Type", Type), new SqlParameter("@Id", Id), new SqlParameter("@OutVersion", txtOutVersion), new SqlParameter("@CasesType", CasesType) };
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param);
                return new DataSetConvert(ds).Get_SingleModel<VersionInfo>();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","SystemDAL.GetVersionInfo(int Type, int Id)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        /// <summary>
        /// 根据Id获取详细信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public VersionInfo GetVersionInfoById(int Id)
        {
            string sql = "";
            try
            {
                sql = "SELECT * FROM VersionInfo vi WHERE vi.ID=@Id";
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Id", Id));
                return new DataSetConvert(ds).Get_SingleModel<VersionInfo>();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","SystemDAL.GetVersionInfoById(int Id)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }

        /// <summary>
        /// 编辑终端管理
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public int UpdateVersionInfo(VersionInfo version)
        {
            string sql = "";
            try
            {
                sql = "UPDATE VersionInfo SET Type= @Type,OutVersion = @OutVersion,CasesType = @CasesType,Version = @Version,	ChangeLog = @ChangeLog, DownloadUrl = @DownloadUrl,	FocusUpdate = @FocusUpdate,	Aid = @Aid,	UpdateType = @UpdateType,otherType = @otherType,VerifyType = @VerifyType WHERE ID=@Id";
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@Type",version.Type),
                    new SqlParameter("@OutVersion",version.OutVersion),
                    new SqlParameter("@Version",version.Version),
                    new SqlParameter("@ChangeLog",version.ChangeLog),
                    new SqlParameter("@DownloadUrl",version.DownloadUrl),
                    new SqlParameter("@FocusUpdate",version.FocusUpdate),
                    new SqlParameter("@CasesType",version.CasesType),
                    new SqlParameter("@Id",version.ID),
                    new SqlParameter("@Aid",version.Aid),
                    new SqlParameter("@UpdateType",version.UpdateType),
                    new SqlParameter("@otherType",version.otherType),
                    new SqlParameter("@VerifyType",version.VerifyType)
                };
                return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","SystemDAL.UpdateVersionInfo(VersionInfo version)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
        ///<summary>
        ///删除终端版本 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int DeleteVersionInfo(int ID)
        {
            string sql = "DELETE VersionInfo WHERE ID=@ID";
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@ID", ID));
        }


        public VersionInfo GetMaxVersionInfoByType(int Type)
        {
            string sql = "";
            try
            {
                sql = "SELECT TOP 1 vi.* FROM VersionInfo vi WHERE  vi.Type=@Type ORDER BY vi.ID DESC";
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@Type", Type));
                return new DataSetConvert(ds).Get_SingleModel<VersionInfo>();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, new Dictionary<string, string>()
                {
                    {"Function","SystemDAL.GetVersionInfoById(int Id)"},
                    {"SQL",sql}
                });
                throw ex;
            }
        }
        #endregion

    }
}
