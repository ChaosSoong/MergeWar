using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    /* ==============================================================================
   * 创 建 者：邓佳训
   * 创建日期：2018/4/10 11:10:55
   * 功能描述：
   *
   * 修改者：         
   * 修改时间：       
   * 修改说明:
   * ==============================================================================*/
    /// <summary>
    /// 视频信息实体类
    /// </summary>
    public class Video3CInfo
    {
        /// <summary>
        /// ID自增变量
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 场所表ID
        /// </summary>
        public int NETBAR_ID { get; set; }
        /// <summary>
        /// 设备表ID
        /// </summary>
        public int DevAP_ID { get; set; }
        /// <summary>
        /// 文件夹名称
        /// </summary>
        public string V3CID { get; set; }
        /// <summary>
        /// 日期文件名
        /// </summary>
        public string DayFile { get; set; }
        /// <summary>
        /// 视频文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件创建时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 文件修改时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 数据创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 场所名称
        /// </summary>
        public string LName { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string APName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public long PageNum { get; set; }
    }
}
