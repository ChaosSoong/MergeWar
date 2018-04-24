using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HCZZ.ModeDB;
using HCZZ.Common;
using Webdiyer.WebControls.Mvc;

namespace HCZZ.DAL
{
    /* ==============================================================================
   * 创 建 者：邓佳训
   * 创建日期：2018/4/9 14:33:15
   * 功能描述：
   *
   * 修改者：         
   * 修改时间：       
   * 修改说明:
   * ==============================================================================*/
    public class AuditDAL
    {
        /// <summary>
        /// 查询布控任务列表(页面显示)
        /// </summary>
        /// <param name="dic"> 查询条件</param>
        /// <param name="Type">查询类型</param>
        /// <returns></returns>
        public PagedList<BKBJ.AuditInfo> GetAuditList(Dictionary<string, string> dic)
        {
            int PageIndex = Convert.ToInt32(dic["PageIndex"]);
            int PageSize = Convert.ToInt32(dic["PageSize"]);
            string TaskName = dic.ContainsKey("TaskName") ? dic["TaskName"] : "";
            string Mobile = dic.ContainsKey("Mobile") ? dic["Mobile"] : "";
            string Mac = dic.ContainsKey("Mac") ? dic["Mac"] : "";
            string IMEI = dic.ContainsKey("IMEI") ? dic["IMEI"] : "";
            string CaseType = dic.ContainsKey("CaseType") ? dic["CaseType"] : "";
            string NETWORK_APP = dic.ContainsKey("NETWORK_APP") ? dic["NETWORK_APP"] : "";
            string IM = dic.ContainsKey("IM") ? dic["IM"] : "";
            string HeadName = dic.ContainsKey("HeadName") ? dic["HeadName"] : "";
            string StartCreateTime = dic.ContainsKey("StartCreateTime") ? dic["StartCreateTime"] : "";
            string EndCreateTime = dic.ContainsKey("EndCreateTime") ? dic["EndCreateTime"] : "";
            string StartEndTime = dic.ContainsKey("StartEndTime") ? dic["StartEndTime"] : "";
            string EndEndTime = dic.ContainsKey("EndEndTime") ? dic["EndEndTime"] : "";
            string wheresql = " WHERE 1=1";
            List<SqlParameter> paramlist = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(dic["Uid"]) && dic["Uid"] != "0")
            {
                wheresql += " AND ci.Uid=@uid";
                paramlist.Add(new SqlParameter("@uid", dic["Uid"]));
            }
            if (!string.IsNullOrEmpty(TaskName))
            {
                if (dic["selTaskName"] == "1") //为1则为模糊，否则为精确，下同
                {
                    wheresql += " AND ci.TaskName LIKE @TaskName";
                    paramlist.Add(new SqlParameter("@TaskName", "%" + TaskName + "%"));
                }
                else
                {
                    wheresql += " AND ci.TaskName = @TaskName";
                    paramlist.Add(new SqlParameter("@TaskName", TaskName));
                }
            }
            if (!string.IsNullOrEmpty(Mobile))
            {
                if (dic["selMobile"] == "1") //为1则为模糊，否则为精确，下同
                {
                    wheresql += " AND ci.CaseValue LIKE @Mobile AND ci.CaseItem=1";
                    paramlist.Add(new SqlParameter("@Mobile", "%" + Mobile + "%"));
                }
                else
                {
                    wheresql += " AND ci.CaseValue = @Mobile AND ci.CaseItem=1";
                    paramlist.Add(new SqlParameter("@Mobile", Mobile));
                }
            }
            if (!string.IsNullOrEmpty(Mac))
            {
                if (dic["selMac"] == "1") //为1则为模糊，否则为精确，下同
                {
                    wheresql += " AND ci.CaseValue LIKE @Mac AND ci.CaseItem=2";
                    paramlist.Add(new SqlParameter("@Mac", "%" + Mac + "%"));
                }
                else
                {
                    wheresql += " AND ci.CaseValue = @Mac AND ci.CaseItem=2";
                    paramlist.Add(new SqlParameter("@Mac", Mac));
                }
            }
            if (!string.IsNullOrEmpty(IMEI))
            {
                if (dic["selIMEI"] == "1") //为1则为模糊，否则为精确，下同
                {
                    wheresql += " AND ci.CaseValue LIKE @IMEI AND ci.CaseItem=3";
                    paramlist.Add(new SqlParameter("@IMEI", "%" + IMEI + "%"));
                }
                else
                {
                    wheresql += " AND ci.CaseValue = @IMEI AND ci.CaseItem=3";
                    paramlist.Add(new SqlParameter("@IMEI", IMEI));
                }
            }
            if (!string.IsNullOrEmpty(dic["keyWord"]))
            {
                if (dic["selkeyWord"] == "1")
                {
                    wheresql += " AND ci.CaseValue LIKE @keyWord ";
                    paramlist.Add(new SqlParameter("@keyWord", "%" + dic["keyWord"] + "%"));
                }
                else
                {
                    wheresql += " AND ci.CaseValue = @keyWord ";
                    paramlist.Add(new SqlParameter("@keyWord", dic["keyWord"]));
                }
            }

            if (!string.IsNullOrEmpty(HeadName))
            {
                if (dic["selHeadName"] == "1") //为1则为模糊，否则为精确，下同
                {
                    wheresql += " AND ci.HeadName LIKE @HeadName";
                    paramlist.Add(new SqlParameter("@HeadName", "%" + HeadName + "%"));
                }
                else
                {
                    wheresql += " AND ci.HeadName = @HeadName";
                    paramlist.Add(new SqlParameter("@HeadName", HeadName));
                }
            }
            if (!string.IsNullOrEmpty(IM))
            {
                if (dic["selIM"] == "1") //为1则为模糊，否则为精确，下同
                {
                    wheresql += " AND ci.CaseValue LIKE @IM AND ci.CaseItem=3";
                    paramlist.Add(new SqlParameter("@IM", "%" + IM + "%"));
                }
                else
                {
                    wheresql += " AND ci.CaseValue = @IM AND ci.CaseItem=3";
                    paramlist.Add(new SqlParameter("@IM", IM));
                }
            }
            if (!string.IsNullOrEmpty(StartCreateTime))
            {
                wheresql += " AND ci.CreateTime > @StartCreateTime";
                paramlist.Add(new SqlParameter("@StartCreateTime", StartCreateTime));
            }
            if (!string.IsNullOrEmpty(EndCreateTime))
            {
                wheresql += " AND ci.CreateTime<@EndCreateTime";
                paramlist.Add(new SqlParameter("@EndCreateTime", EndCreateTime));

            }
            if (!string.IsNullOrEmpty(StartEndTime))
            {
                wheresql += " AND ci.EndTime > @StartEndTime";
                paramlist.Add(new SqlParameter("@StartEndTime", StartEndTime));
            }
            if (!string.IsNullOrEmpty(EndEndTime))
            {
                wheresql += " AND ci.EndTime<@EndEndTime";
                paramlist.Add(new SqlParameter("@EndEndTime", EndEndTime));
            }
            if (!string.IsNullOrEmpty(CaseType))
            {
                wheresql += " AND ci.CaseType=@casetype";
                paramlist.Add(new SqlParameter("@casetype", CaseType));
            }
            if (!string.IsNullOrEmpty(NETWORK_APP))
            {
                wheresql += " AND ci.NETWORK_APP=@network_app";
                paramlist.Add(new SqlParameter("@network_app", NETWORK_APP));
            }
            string sql = "SELECT * FROM ( SELECT ROW_NUMBER () OVER (ORDER BY ci.ID DESC) PageNum,ci.ID,ci.TaskName,ci.CaseValue,ci.CaseItem,ci.HeadName,ci.NETWORK_APP,ci.CreateTime,ci.EndTime,ci.IsEnabled FROM CasesInfo ci " + wheresql + ") temp WHERE temp.PageNum BETWEEN " + ((PageIndex - 1) * PageSize + 1) + " AND " + PageIndex * PageSize;
            Logger.writeLog("SQL语句------" + sql);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, paramlist.ToArray());
            sql = "SELECT COUNT(ID) FROM CasesInfo ci " + wheresql;
            int Count = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, paramlist.ToArray()));
            return new PagedList<BKBJ.AuditInfo>(new DataSetConvert(ds).Get_ListModel<BKBJ.AuditInfo>(), PageIndex, PageSize, Count);
        }
        /// <summary>
        /// 查询布控任务列表（导出）
        /// </summary>
        /// <param name="dic"> 查询条件</param>
        /// <param name="Type">查询类型</param>
        /// <returns></returns>
        public DataTable ExportAuditDT(Dictionary<string, string> dic)
        {
            string TaskName = dic.ContainsKey("TaskName") ? dic["TaskName"] : "";
            string Mobile = dic.ContainsKey("Mobile") ? dic["Mobile"] : "";
            string Mac = dic.ContainsKey("Mac") ? dic["Mac"] : "";
            string IMEI = dic.ContainsKey("IMEI") ? dic["IMEI"] : "";
            string CaseType = dic.ContainsKey("CaseType") ? dic["CaseType"] : "";
            string NETWORK_APP = dic.ContainsKey("NETWORK_APP") ? dic["NETWORK_APP"] : "";
            string IM = dic.ContainsKey("IM") ? dic["IM"] : "";
            string HeadName = dic.ContainsKey("HeadName") ? dic["HeadName"] : "";
            string StartCreateTime = dic.ContainsKey("StartCreateTime") ? dic["StartCreateTime"] : "";
            string EndCreateTime = dic.ContainsKey("EndCreateTime") ? dic["EndCreateTime"] : "";
            string StartEndTime = dic.ContainsKey("StartEndTime") ? dic["StartEndTime"] : "";
            string EndEndTime = dic.ContainsKey("EndEndTime") ? dic["EndEndTime"] : "";
            string wheresql = " WHERE  1=1";
            List<SqlParameter> paramlist = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(dic["Uid"]) && dic["Uid"] != "0")
            {
                wheresql += " AND ci.Uid=@uid";
                paramlist.Add(new SqlParameter("@uid", dic["Uid"]));
            }
            if (!string.IsNullOrEmpty(TaskName))
            {
                if (dic["selTaskName"] == "1") //为1则为模糊，否则为精确，下同
                {
                    wheresql += " AND ci.TaskName LIKE @TaskName";
                    paramlist.Add(new SqlParameter("@TaskName", "%" + TaskName + "%"));
                }
                else
                {
                    wheresql += " AND ci.TaskName = @TaskName";
                    paramlist.Add(new SqlParameter("@TaskName", TaskName));
                }
            }
            if (!string.IsNullOrEmpty(Mobile))
            {
                if (dic["selMobile"] == "1") //为1则为模糊，否则为精确，下同
                {
                    wheresql += " AND ci.CaseValue LIKE @Mobile AND ci.CaseItem=1";
                    paramlist.Add(new SqlParameter("@Mobile", "%" + Mobile + "%"));
                }
                else
                {
                    wheresql += " AND ci.CaseValue = @Mobile AND ci.CaseItem=1";
                    paramlist.Add(new SqlParameter("@Mobile", Mobile));
                }
            }
            if (!string.IsNullOrEmpty(Mac))
            {
                if (dic["selMac"] == "1") //为1则为模糊，否则为精确，下同
                {
                    wheresql += " AND ci.CaseValue LIKE @Mac AND ci.CaseItem=2";
                    paramlist.Add(new SqlParameter("@Mac", "%" + Mac + "%"));
                }
                else
                {
                    wheresql += " AND ci.CaseValue = @Mac AND ci.CaseItem=2";
                    paramlist.Add(new SqlParameter("@Mac", Mac));
                }
            }
            if (!string.IsNullOrEmpty(IMEI))
            {
                if (dic["selIMEI"] == "1") //为1则为模糊，否则为精确，下同
                {
                    wheresql += " AND ci.CaseValue LIKE @IMEI AND ci.CaseItem=3";
                    paramlist.Add(new SqlParameter("@IMEI", "%" + IMEI + "%"));
                }
                else
                {
                    wheresql += " AND ci.CaseValue = @IMEI AND ci.CaseItem=3";
                    paramlist.Add(new SqlParameter("@IMEI", IMEI));
                }
            }
            if (!string.IsNullOrEmpty(dic["keyWord"]))
            {
                if (dic["selkeyWord"] == "1")
                {
                    wheresql += " AND ci.CaseValue LIKE @keyWord ";
                    paramlist.Add(new SqlParameter("@keyWord", "%" + dic["keyWord"] + "%"));
                }
                else
                {
                    wheresql += " AND ci.CaseValue = @keyWord ";
                    paramlist.Add(new SqlParameter("@keyWord", dic["keyWord"]));
                }
            }
            if (!string.IsNullOrEmpty(HeadName))
            {
                if (dic["selHeadName"] == "1") //为1则为模糊，否则为精确，下同
                {
                    wheresql += " AND ci.HeadName LIKE @HeadName";
                    paramlist.Add(new SqlParameter("@HeadName", "%" + HeadName + "%"));
                }
                else
                {
                    wheresql += " AND ci.HeadName = @HeadName";
                    paramlist.Add(new SqlParameter("@HeadName", HeadName));
                }
            }
            if (!string.IsNullOrEmpty(IM))
            {
                if (dic["selIM"] == "1") //为1则为模糊，否则为精确，下同
                {
                    wheresql += " AND ci.CaseValue LIKE @IM AND ci.CaseItem=3";
                    paramlist.Add(new SqlParameter("@IM", "%" + IM + "%"));
                }
                else
                {
                    wheresql += " AND ci.CaseValue = @IM AND ci.CaseItem=3";
                    paramlist.Add(new SqlParameter("@IM", IM));
                }
            }
            if (!string.IsNullOrEmpty(StartCreateTime))
            {
                wheresql += " AND ci.CreateTime > @StartCreateTime";
                paramlist.Add(new SqlParameter("@StartCreateTime", StartCreateTime));
            }
            if (!string.IsNullOrEmpty(EndCreateTime))
            {
                wheresql += " AND ci.CreateTime<@EndCreateTime";
                paramlist.Add(new SqlParameter("@EndCreateTime", EndCreateTime));

            }
            if (!string.IsNullOrEmpty(StartEndTime))
            {
                wheresql += " AND ci.EndTime > @StartEndTime";
                paramlist.Add(new SqlParameter("@StartEndTime", StartEndTime));
            }
            if (!string.IsNullOrEmpty(EndEndTime))
            {
                wheresql += " AND ci.EndTime<@EndEndTime";
                paramlist.Add(new SqlParameter("@EndEndTime", EndEndTime));
            }
            if (!string.IsNullOrEmpty(CaseType))
            {
                wheresql += " AND ci.CaseType=@casetype";
                paramlist.Add(new SqlParameter("@casetype", CaseType));
            }
            if (!string.IsNullOrEmpty(NETWORK_APP))
            {
                wheresql += " AND ci.NETWORK_APP=@network_app";
                paramlist.Add(new SqlParameter("@network_app", NETWORK_APP));
            }
            string sql = "";
            if (CaseType == "1")
                sql = "SELECT  ROW_NUMBER () OVER (ORDER BY ci.ID DESC) ID,ci.TaskName,CASE WHEN ci.CaseItem=1 THEN ci.CaseValue ELSE '' END Mobile,CASE WHEN ci.CaseItem=2 THEN ci.CaseValue ELSE '' END MAC,CASE WHEN ci.CaseItem=3 THEN ci.CaseValue ELSE '' END IMEI,ci.CaseItem,ci.HeadName,ci.CreateTime,ci.EndTime,CASE WHEN ci.IsEnabled=1 THEN '禁用' ELSE '启用' END IsEnabled,ci.CaseValue FROM CasesInfo ci " + wheresql + "  ";
            else
                sql = "SELECT  ROW_NUMBER () OVER (ORDER BY ci.ID DESC) ID,ci.NETWORK_APP,ci.TaskName,ci.CaseValue,ci.CaseItem,ci.HeadName,ci.CreateTime,ci.EndTime,CASE WHEN ci.IsEnabled=1 THEN '禁用' ELSE '启用' END IsEnabled FROM CasesInfo ci " + wheresql + " ORDER BY ci.ID";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, paramlist.ToArray());
            DataTable dt = ds.Tables[0];
            if (CaseType == "3")
            {
                dt.Columns.Add("ST_NETWORK_APP", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    row["ST_NETWORK_APP"] = row["NETWORK_APP"].ToString() == "0" ? "所有" : ChangeValue.VidContentType(row["NETWORK_APP"].ToString());
                }
            }
            return dt;
        }
        /// <summary>
        /// 添加新的布控信息
        /// </summary>
        /// <param name="audit">新布控信息</param>
        /// <returns>返回1为成功</returns>
        public int AddAudit(BKBJ.AuditInfo audit)
        {
            string sql = "INSERT INTO CasesInfo (Uid,ProID,CityID,AID,TaskName,CaseItem,NETWORK_APP,CaseType,CaseValue,HeadName,HeadMobile,DeployArea,IsEnabled,IsValid,MailWarn,WMail,StartTime,EndTime,Remark,CreateTime,NetbarType) VALUES (@uid,@proid,@cityid,@aid,@taskname,@caseitem,@networkapp,@casetype,@casevalue,@headname,@headmobile,@deployarea,2,0,@mailwarn,@wmail,GETDATE(),@endtime,@remark,GETDATE(),@NetbarType)";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@uid",audit.Uid),
                new SqlParameter("@proid",audit.ProID),
                new SqlParameter("@cityid",audit.CityID),
                new SqlParameter("@aid",audit.AID),
                new SqlParameter("@taskname",audit.TaskName),
                new SqlParameter("@caseitem",audit.CaseItem),
                new SqlParameter("@networkapp",audit.NETWORK_APP),
                new SqlParameter("@casetype",audit.CaseType),
                new SqlParameter("@casevalue",audit.CaseValue),
                new SqlParameter("@headname",audit.HeadName),
                new SqlParameter("@headmobile",audit.HeadMobile),
                new SqlParameter("@deployarea",audit.DeployArea),
                new SqlParameter("@mailwarn",audit.MailWarn),
                new SqlParameter("@wmail",audit.WMail),
                new SqlParameter("@endtime",audit.EndTime),
                new SqlParameter("@remark",audit.Remark),
                new SqlParameter("@NetbarType",audit.NetbarType)
            };
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
        }

        /// <summary>
        /// 判断审计名称是否重复
        /// </summary>
        /// <param name="TaskName"></param>
        /// <param name="ID"></param>
        /// <param name="Uid"></param>
        /// <returns></returns>
        public int GetCaseTaskNameCount(string TaskName, int ID, int Uid)
        {
            string sql = "";
            sql = "SELECT COUNT(0) FROM CasesInfo ci WHERE ci.TaskName =@TaskName AND ci.IsValid=0 ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@TaskName", TaskName));
            if (ID != 0)
            {
                sql += " AND ci.ID!=@ID";
                list.Add(new SqlParameter("@ID", ID));
            }
            if (Uid != 0)
            {
                sql += " AND ci.Uid=@UID";
                list.Add(new SqlParameter("@UID", Uid));
            }
            SqlParameter[] param = list.ToArray();
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, param));
        }
        /// <summary>
        /// 查询布控内容是否存在
        /// </summary>
        /// <param name="Id">审计ID</param>
        /// <param name="aid">区ID</param>
        /// <param name="CaseItem">布控项 1.手机号码 2.MAC地址 3.虚拟身份</param>
        /// <param name="CaseValue">布控值</param>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        public int GetCaseMobileCount(int Id, int CaseItem, string CaseValue, int NETWORK_APP, int uid, int CaseType)
        {
            string sql = "";
            sql = "SELECT COUNT(0) FROM CasesInfo ci WHERE ci.IsValid=0 ";
            List<SqlParameter> list = new List<SqlParameter>();
            if (Id != 0)
            {
                sql += " AND ci.ID!=@ID";
                list.Add(new SqlParameter("@ID", Id));
            }
            if (uid != 0)
            {
                sql += " AND ci.Uid=@UID";
                list.Add(new SqlParameter("@UID", uid));
            }
            if (CaseItem != 0)
            {
                sql += " AND ci.CaseItem=@caseitem";
                list.Add(new SqlParameter("@caseitem", CaseItem));
            }
            if (!string.IsNullOrEmpty(CaseValue))
            {
                sql += " AND ci.CaseValue=@casevalue";
                list.Add(new SqlParameter("@casevalue", CaseValue));
            }
            if (NETWORK_APP != -1)
            {
                sql += " AND ci.NETWORK_APP=@networkapp";
                list.Add(new SqlParameter("@networkapp", NETWORK_APP));
            }
            if (CaseType != 0)
            {
                sql += " AND ci.CaseType=@casetype";
                list.Add(new SqlParameter("@casetype", CaseType));
            }
            SqlParameter[] param = list.ToArray();
            return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, param));
        }
        /// <summary>
        /// 通过ID获取布控信息
        /// </summary>
        /// <param name="ID">布控ID</param>
        /// <returns></returns>
        public BKBJ.AuditInfo GetAuditInfoById(int ID, int uid)
        {
            string sql = "SELECT * FROM CasesInfo WHERE ID=@id and Uid=@uid";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter[] { new SqlParameter("@id", ID), new SqlParameter("@uid", uid) });
            return new DataSetConvert(ds).Get_SingleModel<BKBJ.AuditInfo>();
        }

        /// <summary>
        /// 删除布控信息
        /// </summary>
        /// <param name="ID">布控信息ID</param>
        /// <param name="Uid">用户ID，如果为管理员则为0</param>
        /// <returns></returns>
        public int DelAudit(int ID, int Uid)
        {
            string sql = "DELETE FROM CasesInfo WHERE ID=@id";
            if (Uid != 0)
            {
                sql += " AND Uid=@uid";
            }
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@id",ID),
                new SqlParameter("@uid",Uid),
            };
            return Convert.ToInt32(SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param));
        }
        /// <summary>
        /// 获取场所信息列表
        /// </summary>
        /// <returns></returns>
        public List<AreaInfo> GetAreaList()
        {
            string sql = "SELECT * FROM AreaInfo";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql);
            return new DataSetConvert(ds).Get_ListModel<AreaInfo>();
        }
        /// <summary>
        /// 修改审计任务使用状态
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Uid"></param>
        /// <returns></returns>
        public int ChangeAuditAbled(int ID, int Uid)
        {
            string sql = "UPDATE CasesInfo SET IsEnabled = ( CASE(SELECT IsEnabled FROM CasesInfo ci WHERE id=@id) WHEN 1 THEN 2 ELSE 1 END) WHERE id=@id AND uid=@uid";
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter[] { new SqlParameter("@id", ID), new SqlParameter("@uid", Uid) });
        }
        /// <summary>
        /// 修改审计任务
        /// </summary>
        /// <param name="audit"></param>
        /// <returns></returns>
        public int UpdateAudit(BKBJ.AuditInfo audit)
        {
            string sql = "UPDATE CasesInfo SET TaskName=@taskname,CaseItem=@caseitem,CaseValue=@casevalue,HeadName=@headname,HeadMobile=@headmobile,DeployArea=@deployarea,MailWarn=@mailwarn,WMail=@wmail,EndTime=@endtime,Remark=@remark,NETWORK_APP=@network_app,NetbarType=@NetbarType WHERE ID=@id";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@taskname",audit.TaskName),
                new SqlParameter("@caseitem",audit.CaseItem),
                new SqlParameter("@casevalue",audit.CaseValue),
                new SqlParameter("@headname",audit.HeadName),
                new SqlParameter("@headmobile",audit.HeadMobile),
                new SqlParameter("@deployarea",audit.DeployArea),
                new SqlParameter("@mailwarn",audit.MailWarn),
                new SqlParameter("@wmail",audit.WMail),
                new SqlParameter("@endtime",audit.EndTime),
                new SqlParameter("@remark",audit.Remark),
                new SqlParameter("@network_app",audit.NETWORK_APP),
                new SqlParameter("@NetbarType",audit.NetbarType),
                new SqlParameter("@id",audit.ID)
            };
            return SqlHelper.ExecuteNonQuery(SqlHelper.DBConnStr, CommandType.Text, sql, param);
        }
        /// <summary>
        /// 根据sessionId查询手机号码
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public string RefMobileBySessionId(string sessionId)
        {
            string sql = "SELECT TOP 1 tal.AUTH_ACCOUNT FROM TermianlAccessLog tal WHERE tal.SESSIONID=@sessionId";
            object obj = SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@sessionId", sessionId));
            if (obj == null || obj == DBNull.Value)
                return "";
            return obj.ToString();
        }
    }
}
