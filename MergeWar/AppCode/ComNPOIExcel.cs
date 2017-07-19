using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Data;
using Common;
using HCZZ.AppCode;

namespace HCZZ.Common
{
    /// <summary>
    /// Excel页(Sheet)模板
    /// </summary>
    public class NPOISheetModel
    {
        /// <summary>
        /// 实例化对象（空实现）
        /// </summary>
        public NPOISheetModel()
        {
             
        }

        /// <summary>
        /// Excel页标签名称
        /// </summary>
        public string SheetName { set; get; }

        /// <summary>
        /// Excel标题
        /// </summary>
        public string ExcelTitle { set; get; }

        /// <summary>
        /// 表格标题 
        /// key:标题名称(行业)
        /// value:数据库中字段类型
        /// </summary>
        public Dictionary<string, string> TableTitle { set; get; }

        /// <summary>
        /// 查询条件
        /// key:查询条件字段
        /// value:查询条件值
        /// </summary>
        public Dictionary<string,string> TableSearch { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public DataTable dt { set; get; }
        /// <summary>
        /// 场所ID是否进行转换
        /// </summary>
        public bool IsNETBAR_ID { get; set; }
        /// <summary>
        /// 成本统计数据转换
        /// </summary>
        public bool IsCost { get; set; }
        /// <summary>
        /// 设备ID信息转换
        /// </summary>
        public bool IsDevAP_ID { get; set; }
        /// 设备MAC信息转换
        /// </summary>
        public bool IsDevAP_MAC { get; set; }
    }

    public class ComNPOIExcel
    {
        #region 导出Excel

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="path">当前应用程序的物理路径 Server.MapPath("~/");</param>
        /// <param name="sheet">已封装好的Excel Sheet对象</param>
        /// <returns>Excel文件名</returns>
        public string Export(NPOISheetModel sheet, string path)
        {
            DirectoryInfo dinfo = new DirectoryInfo(path);
            if (!dinfo.Exists)
            {
                dinfo.Create();
            }
            //内容行开始的索引
            int rowIndes = 0;
            int rowCount = sheet.TableTitle.Count - 1;
            HSSFWorkbook wk = new HSSFWorkbook();
            //IWorkbook wk = new HSSFWorkbook();
            //创建一个名称为mySheet的表
            ISheet tb = wk.CreateSheet(sheet.ExcelTitle + "报表");

            //创建一行，此行为第二行
            tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 2, 0, rowCount));
            IRow row = tb.CreateRow(0);
            ICell cell = row.CreateCell(0);
            cell.SetCellValue(sheet.ExcelTitle);
            IFont font = wk.CreateFont();
            font.FontName = "微软雅黑";
            font.Boldweight = (short)FontBoldWeight.Bold;
            //font.Color = HSSFColor.OliveGreen.Black.Index;
            font.FontHeightInPoints = 14;

            ICellStyle cellstyle = wk.CreateCellStyle();
            cellstyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            cellstyle.VerticalAlignment = VerticalAlignment.Center;
            cellstyle.SetFont(font);
            //为标题添加样式
            cell.CellStyle = cellstyle;

            //添加统计
            tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 3, 0, rowCount));
            row = tb.CreateRow(3);
            cell = row.CreateCell(0);
            cell.SetCellValue("统计：共" + sheet.dt.Rows.Count.ToString() + "行");

            //添加查询条件
            rowIndes = 4;
            foreach (KeyValuePair<string, string> item in sheet.TableSearch)
            {
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndes, rowIndes, 0, rowCount));
                row = tb.CreateRow(rowIndes);
                cell = row.CreateCell(0);
                cell.SetCellValue(item.Key + "：" + item.Value);
                rowIndes++;
            }

            //填充标题
            row = tb.CreateRow(rowIndes);
            int y = 0;
            foreach (KeyValuePair<string, string> item in sheet.TableTitle)
            {
                cell = row.CreateCell(y);
                cell.SetCellValue(item.Key);
                //单位格字体加粗
                IFont fontTitle = wk.CreateFont();
                fontTitle.Boldweight = (short)FontBoldWeight.Bold;
                fontTitle.FontHeightInPoints = 12;
                cellstyle = wk.CreateCellStyle();
                cellstyle.SetFont(fontTitle);
                cell.CellStyle = cellstyle;

                //设定列宽度
                if (item.Key == "序号")
                    tb.SetColumnWidth(y, 10 * 200);
                else if (item.Key == "生产商" || item.Key == "URL")
                    tb.SetColumnWidth(y, 25 * 300);
                else
                    tb.SetColumnWidth(y, 25 * 200);

                y++;
            }

            rowIndes++;

            //创建行/单元格，填充数据
            for (int k = 0; k < sheet.dt.Rows.Count; k++)
            {
                row = tb.CreateRow(k + rowIndes);
                y = 0;

                foreach (KeyValuePair<string, string> item in sheet.TableTitle)
                {
                    //在第k行中创建单元格
                    cell = row.CreateCell(y);  
                    //循环往第二行的单元格中添加数据
                    cell.SetCellValue(FormatDataByKey(item.Value, sheet.dt.Rows[k][item.Value].ToString(), sheet, y));
                    y++;
                }
            }
            //打开一个xls文件，如果没有则自行创建，如果存在myxls.xls文件则在创建是不要打开该文件！
            path += "/" + Guid.NewGuid() + ".xlsx";

            MemoryStream stream = new MemoryStream();
            wk.Write(stream);
            var buf = stream.ToArray();

            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);   //向打开的这个xls文件中写入mySheet表并保存。
            }
            return path.Substring(path.IndexOf("UserData"));
        }

        /// <summary>
        /// 内容格式化
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string FormatDataByKey(string key, string value, NPOISheetModel sheet, int k)
        {
            key = key.ToLower();
            if (key == "createtime" || key == "itime" || key == "begintime" || key == "endtime" || key == "connecttime" || key == "offlinetime" || key == "updatetime")
                return value == "" ? "" : Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss");
            else if (key == "valid")
                return (value == "1" ? "未删除" : "已删除");
            else if (key == "isap")
                return (value == "0" ? "是" : "否");
            else if (key == "isopen")
                return ChangeValue.GetLocaCheckValue(Convert.ToInt32(value));
            else if (key == "filesize")
                return ChangeValue.FormatFileSize(Convert.ToInt64(value));
            else if (key == "spassword")
                return ChangeValue.RefSendPwd(value);
            else if (key == "statis")
                return value == "1" ? "营业状态" : "停业状态";
            else if (key == "netsite_type")
                return ChangeValue.GetLocaTypeValue(Convert.ToInt32(value));
            else if (key == "pullortrade")
                return value == "0" ? "正常" : (value == "1" ? "拆机" : "换机");
            else if (key == "pottype")
            {
                if (value == "0")
                    return "正常";
                else
                {
                    return value == "1" ? "待审核" : (value == "2" ? "审核成功" : "审核失败");
                }
            }  
            else if (key == "verified")
                return ChangeValue.RefVerifiedStrExcel(Convert.ToInt32(value));
            else if (key == "caseitem")
                return ChangeValue.pwItemvalue(Convert.ToInt32(value));
            else if (key == "vtname")
                return ChangeValue.VidContentType(value);
            else if (key == "dst_ip")
                return ChangeValue.RefIPvlong(Convert.ToInt64(value));
            else if (key == "netbar_id")
            {
                if (sheet.IsNETBAR_ID)
                {
                    HCZZ.ModeDB.Loc_NetBarInfo loca = WebCommon.GetLocaInfoByNetBar_ID(Convert.ToInt32(value));
                    if (loca != null) return loca.PLACE_NAME; else return "";
                }
                else return value;
            }
            else if (key == "ap_mac" && sheet.IsDevAP_MAC)
            {
                HCZZ.ModeDB.Loc_DevInfo mac = WebCommon.GetMacInfoByDevAP_ID(0, value);
                if (mac != null)
                    return mac.APName;
                return "";
            }
            else if (key == "devap_id" && sheet.IsDevAP_ID)
            {
                HCZZ.ModeDB.Loc_DevInfo mac = WebCommon.GetMacInfoByDevAP_ID(Convert.ToInt32(value), "");
                if (mac != null)
                {
                    if (k < 2)
                        return mac.APName;
                    else
                        return mac.AP_MAC;
                }
                return "";
            }
            else if (sheet.IsCost)
            {
                if (key == "dt_day")
                    return value.Insert(6, "-").Insert(4, "-");
                else if (key == "auditnum")
                    return value + " | " + ChangeValue.FormatFileSize(Convert.ToInt32(value) * 150);
                else if (key == "httpnum")
                    return value + " | " + ChangeValue.FormatFileSize(Convert.ToInt32(value) * 400);
                else if (key == "macnum")
                    return value + " | " + ChangeValue.FormatFileSize(Convert.ToInt32(value) * 200);
                else if (key == "apmacnum")
                    return value + " | " + ChangeValue.FormatFileSize(Convert.ToInt32(value) * 200);
                else
                    return "";
            }
            else if (key == "ip_address" || key == "src_ip" || key == "dst_ip")
                return ChangeValue.RefIPvlong(Convert.ToInt64(value));
            else if (key == "network_app")
                return ChangeValue.GetDetailHttpLog(value);
            else if (key == "start_time" || key == "end_time" || key == "capture_time")
                return value == "0" ? "N/A" : ChangeValue.RefAuditTime(Convert.ToInt64(value));
            else if (key == "terminal_field_strength" || key == "ap_field_strength")
                return string.IsNullOrEmpty(value) ? "" : ("-" + value + "dBm");
            else if (key == "access_type")
                return string.IsNullOrEmpty(value) ? "" : ChangeValue.GetConnectTypeValue(Convert.ToInt32(value));
            else if (key == "operator_net")
                return string.IsNullOrEmpty(value) ? "" : ChangeValue.GetServiceBusinesValue(value);
           
            else if (key == "law_principal_certificate_type")
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Dictionary<string, string> dic = ChangeValue.GetCertifiCateList();
                    IEnumerable<KeyValuePair<string, string>> ilist = dic.Where(m => m.Key.ToLower() == value);
                    if (ilist != null && ilist.Count() > 0)
                        return ilist.First().Value;
                }
                return "";
            }
            else
                return value;
        }

        #endregion

        #region 导入Excel

        /// <summary>
        /// 读取2003Excel文件内容返回DataTable
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DataTable ComXLSToTable(string path, string fileName)
        {
            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(fs);
                var sheet1 = workbook.GetSheetAt(0);
                var row1 = sheet1.GetRow(0);  //获取列头

                int cellCount = row1.LastCellNum; //列数
                dt.Columns.Add(new DataColumn("UId"));

                //将列头添加到DataTable中
                for (int i = row1.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn dc = new DataColumn(row1.GetCell(i).StringCellValue);
                    dt.Columns.Add(dc);
                }
                int rowCount = sheet1.LastRowNum; //总行数
                //循环将数据添加到DataTable中
                for (int i = (sheet1.FirstRowNum + 1); i <= rowCount; i++)
                {
                    var row = sheet1.GetRow(i);
                    if (row != null)
                    {
                        DataRow dr = dt.NewRow();
                        bool isKong = false;
                        dr[0] = fileName;
                        for (int k = row.FirstCellNum; k < cellCount; k++)
                        {
                            if (row.GetCell(k) != null && !string.IsNullOrEmpty(row.GetCell(k).ToString()))
                            {
                                dr[k + 1] = row.GetCell(k).ToString(); isKong = true;
                            }
                        }
                        if (isKong)
                            dt.Rows.Add(dr);
                    }
                }
                workbook = null;
                sheet1 = null;
            }
            return dt;
        }

        /// <summary>
        /// 读取2007Excel文件内容返回DataTable
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DataTable ComXLSXToTable(string path, string fileName)
        {
            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                XSSFWorkbook workbook = new XSSFWorkbook(fs);
                var sheet1 = workbook.GetSheetAt(0);
                var row1 = sheet1.GetRow(0);  //获取列头

                int cellCount = row1.LastCellNum; //列数
                dt.Columns.Add(new DataColumn("UId"));

                //将列头添加到DataTable中
                for (int i = row1.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn dc = new DataColumn(row1.GetCell(i).StringCellValue);
                    dt.Columns.Add(dc);
                }
                int rowCount = sheet1.LastRowNum; //总行数
                //循环将数据添加到DataTable中
                for (int i = (sheet1.FirstRowNum + 1); i <= rowCount; i++)
                {
                    var row = sheet1.GetRow(i);
                    if (row != null)
                    {
                        DataRow dr = dt.NewRow();
                        bool isKong = false;
                        dr[0] = fileName;
                        for (int k = row.FirstCellNum; k < cellCount; k++)
                        {
                            if (row.GetCell(k) != null && !string.IsNullOrEmpty(row.GetCell(k).ToString()))
                            {
                                dr[k + 1] = row.GetCell(k).ToString(); isKong = true;
                            }
                        }
                        if (isKong)
                            dt.Rows.Add(dr);
                    }
                }
                workbook = null;
                sheet1 = null;
            }
            return dt;
        }

        /// <summary>
        /// 读取2003/2007Excel文件内容返回DataTable
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DataTable ComXLSXOrXLSToTable(string path, string fileName)
        {
            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = WorkbookFactory.Create(fs); //自动识别2003、2007
                var sheet1 = workbook.GetSheetAt(0);
                var row1 = sheet1.GetRow(0);  //获取列头

                int cellCount = row1.LastCellNum; //列数

                dt.Columns.Add(new DataColumn("UId"));
                //将列头添加到DataTable中
                for (int i = row1.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn dc = new DataColumn(row1.GetCell(i).StringCellValue);
                    dt.Columns.Add(dc);
                }
                int rowCount = sheet1.LastRowNum; //总行数
                //循环将数据添加到DataTable中
                for (int i = (sheet1.FirstRowNum + 1); i <= rowCount; i++)
                {
                    var row = sheet1.GetRow(i);
                    if (row != null)
                    {
                        DataRow dr = dt.NewRow();
                        bool isKong = false;
                        dr[0] = fileName;
                        for (int k = row.FirstCellNum; k < cellCount; k++)
                        {
                            if (row.GetCell(k) != null && !string.IsNullOrEmpty(row.GetCell(k).ToString()))
                            {
                                dr[k + 1] = row.GetCell(k).ToString(); isKong = true;
                            }
                        }
                        if (isKong)
                            dt.Rows.Add(dr);
                    }
                }
                workbook = null;
                sheet1 = null;
            }
            return dt;
        }



        /// <summary>
        /// 读取2003/2007Excel文件内容返回DataTable，只读数据，不添加任何列
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DataTable ComXLSXOrXLSToTable(string path)
        {
            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = WorkbookFactory.Create(fs); //自动识别2003、2007
                var sheet1 = workbook.GetSheetAt(0);
                var row1 = sheet1.GetRow(0);  //获取列头

                int cellCount = row1.LastCellNum; //列数

                //将列头添加到DataTable中
                for (int i = row1.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn dc = new DataColumn(row1.GetCell(i).StringCellValue);
                    dt.Columns.Add(dc);
                }
                int rowCount = sheet1.LastRowNum; //总行数
                //循环将数据添加到DataTable中
                for (int i = (sheet1.FirstRowNum + 1); i <= rowCount; i++)
                {
                    var row = sheet1.GetRow(i);
                    if (row != null)
                    {
                        DataRow dr = dt.NewRow();
                        bool isKong = false;
                        for (int k = row.FirstCellNum; k < cellCount; k++)
                        {
                            if (row.GetCell(k) != null && !string.IsNullOrEmpty(row.GetCell(k).ToString()))
                            {
                                dr[k] = row.GetCell(k).ToString(); isKong = true;
                            }
                        }
                        if (isKong)
                            dt.Rows.Add(dr);
                    }
                }
                workbook = null;
                sheet1 = null;
            }
            return dt;
        }
        #endregion
    }
}