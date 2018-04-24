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
    public class TermianlAccessLog:BaseDataModel
    {
        // /// <summary>
        // /// solrID
        // /// </summary>
        //public string  Id { get; set; }
        public string AUTH_TYPE
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
        public long? START_TIME
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public long? END_TIME
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
        public string MODEL
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public long? UPDATE_TIME
        {
            set;
            get;
        }
       new public string CreateTime { get; set; }

        public string ID { get; set; }
    }
}
