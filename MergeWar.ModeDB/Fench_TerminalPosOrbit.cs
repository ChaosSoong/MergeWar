using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    /* ==============================================================================
   * 创 建 者：邓佳训
   * 创建日期：2017/7/20 14:00:37
   * 功能描述：移动采集设备活动轨迹信息
   *
   * 修改者：         
   * 修改时间：       
   * 修改说明:
   * ==============================================================================*/
    public class Fench_TerminalPosOrbit : BaseDataModel
    {
        /// <summary>
        /// 采集时间
        /// </summary>
        public long? TIME { get; set; }
        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime? ITime { get; set; }
        /// <summary>
        /// 采集设备MAC
        /// </summary>
        public string WMacAddress { get; set; }
    }
}
