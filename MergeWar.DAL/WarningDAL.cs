using HCZZ.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;
using HCZZ.ModeDB;
namespace HCZZ.DAL
{
    /* ==============================================================================
   * 创 建 者：邓佳训
   * 创建日期：2018/4/9 17:25:19
   * 功能描述：
   *
   * 修改者：         
   * 修改时间：       
   * 修改说明:
   * ==============================================================================*/
    public class WarningDAL
    {
        /// <summary>
        /// DAL公用方法
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object GetPersonWaringListDal(Dictionary<string, string> dic)
        {
            string sql = "";
            string wheresql = " WHERE ci.CaseType=" + dic["CaseType"];
            if (Convert.ToInt32(dic["UserType"]) != 1)
            {
                wheresql += " AND  ci.Uid= " + dic["UserID"];
            }
            if (Convert.ToInt32(dic["selWaringType"]) != 0)
            {
                if (Convert.ToInt32(dic["selWaringType"]) == 1)
                {
                    wheresql += " AND wi.WaringType=" + dic["selWaringType"];
                }
                else
                {
                    wheresql += " AND wi.WaringType=" + dic["selWaringType"];
                }
            }
            List<SqlParameter> ListParam = new List<SqlParameter>();

            //类型1.人员报警，2.即时通讯报警，3.CCIC报警，4人员报警导出，5.即时通讯报警导出 ,6.CCIC报警导出
            int ExcelType = Convert.ToInt32(dic["ExcelType"]);
            //审计任务名称
            if (dic.ContainsKey("selTaskName"))
            {
                if (!string.IsNullOrEmpty(dic["txtTaskName"]))
                {
                    if (Convert.ToInt32(dic["selTaskName"]) == 1)
                    {
                        wheresql += " AND ci.TaskName LIKE @txtTaskName ";
                        ListParam.Add(new SqlParameter("@txtTaskName", "%" + dic["txtTaskName"] + "%"));
                    }
                    else
                    {
                        wheresql += " AND ci.TaskName = @txtTaskName ";
                        ListParam.Add(new SqlParameter("@txtTaskName", dic["txtTaskName"]));
                    }
                }
            }
            //场所名称
            if (dic.ContainsKey("selName"))
            {
                if (!string.IsNullOrEmpty(dic["txtName"]))
                {
                    if (Convert.ToInt32(dic["selName"]) == 1)
                    {
                        wheresql += " AND nbi.PLACE_NAME LIKE @txtName ";
                        ListParam.Add(new SqlParameter("@txtName", "%" + dic["txtName"] + "%"));
                    }
                    else
                    {
                        wheresql += " AND nbi.PLACE_NAME =@txtName ";
                        ListParam.Add(new SqlParameter("@txtName", dic["txtName"]));
                    }
                }
            }

            //手机号码
            if (dic.ContainsKey("selMobile"))
            {
                if (!string.IsNullOrEmpty(dic["txtMobile"]))
                {
                    if (Convert.ToInt32(dic["selMobile"]) == 1)
                    {
                        wheresql += " AND ci.CaseValue LIKE @txtMobile AND ci.CaseItem=1";
                        ListParam.Add(new SqlParameter("@txtMobile", "%" + dic["txtMobile"] + "%"));
                    }
                    else
                    {
                        wheresql += " AND ci.CaseValue = @txtMobile AND ci.CaseItem=1";
                        ListParam.Add(new SqlParameter("@txtMobile", dic["txtMobile"]));
                    }
                }
            }
            //MAC地址
            if (dic.ContainsKey("selMac"))
            {
                if (!string.IsNullOrEmpty(dic["txtMac"]))
                {
                    if (Convert.ToInt32(dic["selMac"]) == 1)
                    {
                        wheresql += " AND ci.CaseValue LIKE @txtMac AND ci.CaseItem=2";
                        ListParam.Add(new SqlParameter("@txtMac", "%" + dic["txtMac"] + "%"));
                    }
                    else
                    {
                        wheresql += " AND ci.CaseValue = @txtMac AND ci.CaseItem=2";
                        ListParam.Add(new SqlParameter("@txtMac", dic["txtMac"]));
                    }
                }
            }
            //虚拟身份标识(即时通讯标识)
            if (dic.ContainsKey("selQQ"))
            {
                if (!string.IsNullOrEmpty(dic["txtQQ"]))
                {
                    if (Convert.ToInt32(dic["selQQ"]) == 1)
                    {
                        wheresql += " AND ci.CaseValue LIKE @txtQQ";
                        ListParam.Add(new SqlParameter("@txtQQ", "%" + dic["txtQQ"] + "%"));
                    }
                    else
                    {
                        wheresql += " AND ci.CaseValue = @txtQQ";
                        ListParam.Add(new SqlParameter("@txtQQ", dic["txtQQ"]));
                    }
                }
            }
            //IMEI
            if (dic.ContainsKey("selIMEI"))
            {
                if (!string.IsNullOrEmpty(dic["txtIMEI"]))
                {
                    if (Convert.ToInt32(dic["selIMEI"]) == 1)
                    {
                        wheresql += " AND ci.CaseValue LIKE @txtIMEI AND  ci.CaseItem NOT IN(1,2)";
                        ListParam.Add(new SqlParameter("@txtIMEI", "%" + dic["txtIMEI"] + "%"));
                    }
                    else
                    {
                        wheresql += " AND ci.CaseValue = @txtIMEI AND  ci.CaseItem NOT IN(1,2)";
                        ListParam.Add(new SqlParameter("@txtIMEI", "%" + dic["txtIMEI"] + "%"));
                    }
                }
            }
            if (!string.IsNullOrEmpty(dic["keyWord"]))
            {
                if (dic["selkeyWord"] == "1")
                {
                    wheresql += " AND ci.CaseValue LIKE @keyWord ";
                    ListParam.Add(new SqlParameter("@keyWord", "%" + dic["keyWord"] + "%"));
                }
                else
                {
                    wheresql += " AND ci.CaseValue = @keyWord ";
                    ListParam.Add(new SqlParameter("@keyWord", dic["keyWord"]));
                }
            }

            //IMSI
            //if (dic.ContainsKey(dic["selIMSI"]))
            //{
            //    if (!string.IsNullOrEmpty(dic["txtIMSI"]))
            //    {
            //        if (Convert.ToInt32(dic["selIMSI"]) == 1)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //开始时间
            if (!string.IsNullOrEmpty(dic["txtStartTime"]))
            {
                wheresql += " AND wi.Createtime>=@txtStartTime ";
                ListParam.Add(new SqlParameter("@txtStartTime ", dic["txtStartTime"]));
            }
            //结束时间
            if (!string.IsNullOrEmpty(dic["txtEndTime"]))
            {
                wheresql += " AND wi.Createtime<=@txtEndTime ";
                ListParam.Add(new SqlParameter("@TxtEndTime", dic["txtEndTime"]));
            }
            SqlParameter[] param = ListParam.ToArray();
            if (ExcelType == 1 || ExcelType == 3)
            {
                int pageIndex = Convert.ToInt32(dic["PageIndex"]);
                int pageSize = Convert.ToInt32(dic["PageSize"]);
                sql = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ID DESC)PageNum,* FROM (SELECT DISTINCT wi.*,ci.TaskName,ci.CaseType,ci.CaseItem,ci.CaseValue,ci.WMail,ci.NETWORK_APP,nbi.PLACE_NAME,nbi.SITE_ADDRESS FROM  WaringInfo wi LEFT JOIN CasesInfo ci ON wi.Cid=ci.ID  LEFT JOIN NetBarInfo nbi ON wi.NETBAR_ID=nbi.ID " + wheresql + ")table1)PageNum WHERE PageNum.PageNum BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param);
                sql = " SELECT COUNT(DISTINCT wi.ID) FROM WaringInfo wi LEFT JOIN CasesInfo ci ON wi.Cid=ci.ID  LEFT JOIN NetBarInfo nbi ON wi.NETBAR_ID=nbi.ID  " + wheresql;
                int result = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.DBConnStr, CommandType.Text, sql, param));
                DataSetConvert convert = new DataSetConvert(ds);
                List<BKBJ.WarningInfo> list = convert.Get_ListModel<BKBJ.WarningInfo>();
                return new PagedList<BKBJ.WarningInfo>(list, pageIndex, pageSize, result);
            }
            sql = " SELECT DISTINCT wi.*,ci.TaskName,ci.CaseType,ci.CaseItem,ci.CaseValue,ci.WMail,ci.NETWORK_APP,nbi.PLACE_NAME,nbi.SITE_ADDRESS FROM  WaringInfo wi LEFT JOIN CasesInfo ci ON wi.Cid=ci.ID  LEFT JOIN NetBarInfo nbi ON wi.NETBAR_ID=nbi.ID " + wheresql;
            DataSet ex = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, param);
            return ex;
        }
        /// <summary>
        /// 详情公用方法
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public BKBJ.WarningInfo PwDetailsDal(int ID)
        {
            string sql = "SELECT NETWORK_APP, ci.CaseItem,ci.CaseValue,ci.TaskName,ci.MailWarn,ci.WMail,ci.HeadName,ci.HeadMobile,ci.Createtime as  CaseCreatetime, ci.EndTime ,ci.Remark ,wi.*,nbi.PLACE_NAME FROM WaringInfo wi LEFT JOIN  CasesInfo ci ON wi.Cid=ci.ID LEFT JOIN NetBarInfo nbi ON wi.NETBAR_ID=nbi.ID WHERE wi.ID=@ID";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.DBConnStr, CommandType.Text, sql, new SqlParameter("@ID", ID));
            return new DataSetConvert(ds).Get_SingleModel<BKBJ.WarningInfo>();
        }
    }
}
