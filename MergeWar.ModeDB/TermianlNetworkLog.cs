using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    /* ==============================================================================
   * 创 建 者：邓佳训
   * 创建日期：2017/7/20 13:42:59
   * 功能描述：
   *
   * 修改者：         
   * 修改时间：       
   * 修改说明:
   * ==============================================================================*/
    /// <summary>
    /// 上网日志信息实体
    /// </summary>
    public class TermianlNetworkLog:BaseDataModel
    {
        /// <summary>
        /// 上网服务场所编码
        /// </summary>
        public string NETSERVERPORT_WACODE { get; set; }
        /// <summary>
        /// 网络应用服务类型
        /// </summary>
        public string NETWORK_APP { get; set; }
        /// <summary>
        /// 网络应用服务值
        /// </summary>
        public string NETWORK_APP_VALUES { get; set; }
        /// <summary>
        /// 场所内网端口号
        /// </summary>
        public int? PORT { get; set; }
        /// <summary>
        /// 目的公网IPv4地址
        /// </summary>
        public long? DST_IP { get; set; }
        /// <summary>
        /// 目的公网IPv6地址
        /// </summary>
        public string DST_IPV6 { get; set; }
        /// <summary>
        /// 目的公网IPv4端口号
        /// </summary>
        public int? DST_PORT { get; set; }
        /// <summary>
        /// 目的公网IPv6端口号
        /// </summary>
        public int? DST_PORT_V6 { get; set; }
    }
}
