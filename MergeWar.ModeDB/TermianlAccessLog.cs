using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    /* ==============================================================================
   * 创 建 者：邓佳训
   * 创建日期：2017/7/14 15:59:02
   * 功能描述：
   *
   * 修改者：         
   * 修改时间：       
   * 修改说明:
   * ==============================================================================*/
    /// <summary>
    /// 终端上下线信息表:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public class TermianlAccessLog:ModelBase
    {
        public string AUTH_TYPE
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string AUTH_ACCOUNT
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string NETBAR_WACODE
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int NETSITE_TYPE
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string CERTIFICATE_TYPE
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string CERTIFICATE_CODE
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string APP_COMPANY_NAME
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string APP_SOFTWARE_NAME
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string APP_VERSION
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string APPID
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public long START_TIME
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public long END_TIME
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public long IP_ADDRESS
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public long SRC_IP
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string SRC_IPV6
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int SRC_PORT_START
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int SRC_PORT_END
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int SRC_PORT_START_V6
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int SRC_PORT_END_V6
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string MAC
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string COLLECTION_EQUIPMENT_ID
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string AP_MAC
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string LONGITUDE
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string LATITUDE
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string SESSIONID
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string TERMINAL_FIELD_STRENGTH
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string X_COORDINATE
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Y_COORDINATE
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string NAME
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string IMSI
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string iMEI
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string OS_NAME
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string BRAND
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string MODEL
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int NETBAR_ID
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public long UPDATE_TIME
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int SecId
        {
            set;
            get;
        }
    }
}
