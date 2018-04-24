using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    /* ==============================================================================
   * 创 建 者：邓佳训
   * 创建日期：2017/8/16 14:37:54
   * 功能描述：一键查询数据接收
   *
   * 修改者：         
   * 修改时间：       
   * 修改说明:
   * ==============================================================================*/
    public class DatasInfo
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 关系表Mac地址
        /// </summary>
        public string MacAddress { get; set; }

        /// <summary>
        /// Mac地址
        /// </summary>
        public string MAC { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public long? OperDate { get; set; }

        /// <summary>
        /// 心跳时间
        /// </summary>
        public long? HeartTime { get; set; }
        /// <summary>
        ///虚拟身份类型
        /// </summary>
        public string NETWORK_APP { get; set; }
        /// <summary>
        /// 虚拟身份值
        /// </summary>
        public string NETWORK_APP_VALUES { get; set; }
        /// <summary>
        /// 认证账号
        /// </summary>
        public string AUTH_ACCOUNT { get; set; }
    }
}
