using HCZZ.ModeDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MergeWar.Models
{
    public class SolrShowModel
    {
        /// <summary>
        /// 列表集合数量
        /// </summary>
        public int numFound { get; set; }
        /// <summary>
        /// 终端上下线
        /// </summary>
        public List<TermianlAccessLog> _taLog { get; set; }
        /// <summary>
        /// 终端特征
        /// </summary>
        public List<Fench_TerminalInfo> _ftInfo { get; set; }
        /// <summary>
        /// 采集热点信息
        /// </summary>
        public List<Fench_TerminalPosOrbit> _ftpbit { get; set; }
        /// <summary>
        /// 上网日志
        /// </summary>
        public List<TermianlNetworkLog> _tnkLog { get; set; }
        /// <summary>
        /// 碰撞分析
        /// </summary>
        public List<Collision> _cl { get; set; }
       
    }
    [Serializable]
    public class Collision
    {
        public string MAC { get; set; }
        public string c { get; set; }
        public string CAPTURE_TIME { get; set; }
        public int NETBAR_ID { get; set; }
        public int DevAP_ID { get; set; }
        public string APName { get; set; }
        public string PLACE_NAME { get; set; }
        public string AP_MAC { get; set; }
    }
}