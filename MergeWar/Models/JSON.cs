using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCZZ.Models
{
    public class JSON
    {
        public JSON()
        {
            phone = new long[5];
            mac = new long[5];
            virtuals = new long[5];
            imei = new long[5];
            carNum = new long[5];
            times = new string[5];
        }
        public string name { get; set; }
        public string data { get; set; }
        public long[] phone { get; set; }
        public long[] mac { get; set; }
        public long[] virtuals { get; set; }
        public long[] imei { get; set; }
        public long[] carNum { get; set; }
        public string[] times { get; set; }
        /// <summary>
        /// 场所在线量数组
        /// </summary>
        public int[] datanum
        {
            get;
            set;
        }
        /// <summary>
        /// 场所名称数组
        /// </summary>
        public string[] namenum
        {
            get;
            set;
        }
        /// <summary>
        ///场所在线率数组
        /// </summary>
        public double[] ratenum
        {
            get;
            set;
        }

        public string Mobile
        {
            get;
            set;
        }
        public string NETWORK_APP
        {
            get;
            set;
        }
        public string IdValue
        {
            get;
            set;
        }
        public string PLACE_NAME
        {
            get;
            set;
        }
    }
    /// <summary>
    /// 用于接收四类碰撞数据
    /// </summary>
    public class AnalyzeModel
    {
        /// <summary>
        /// mac地址
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// 出现次数
        /// </summary>
        public string NUM { get; set; }
        /// <summary>
        /// 场所ID
        /// </summary>
        public string NETBAR_ID { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public string DevAP_ID { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string CAPTURE_TIME { get; set; }
    }
}