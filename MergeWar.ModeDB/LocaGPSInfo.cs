using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    /// <summary>
    /// 移动经纬度
    /// </summary>
    public class LocaGPSInfo
    {
        /// <summary>
        /// 主键，自增长
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// MAC地址ID
        /// </summary>
        public int WMacAddress { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public long QTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LONGITUDE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LATITUDE { get; set; }
        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
