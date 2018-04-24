using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    /* ==============================================================================
   * 创 建 者：邓佳训
   * 创建日期：2017/7/20 13:55:07
   * 功能描述：终端信息实体
   *
   * 修改者：         
   * 修改时间：       
   * 修改说明:
   * ==============================================================================*/
    public class Fench_TerminalInfo:BaseDataModel
    {
        /// <summary>
        /// 终端历史SSID列表
        /// </summary>
        public string CACHE_SSID { get; set; }
        /// <summary>
        /// 身份类型
        /// </summary>
        public int? IDENTIFICATION_TYPE { get; set; }
        /// <summary>
        /// 接入热点SSID
        /// </summary>
        public string SSID_POSITION { get; set; }
        /// <summary>
        /// 接入热点MAC
        /// </summary>
        public string ACCESS_AP_MAC { get; set; }
        /// <summary>
        /// 接入热点频道
        /// </summary>
        public string ACCESS_AP_CHANNEL { get; set; }
        /// <summary>
        /// 接入热点加密类型
        /// </summary>
        public string ACCESS_AP_ENCRYPTION_TYPE { get; set; }
    }
}
