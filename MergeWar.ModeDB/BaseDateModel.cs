using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    /* ==============================================================================
   * 创 建 者：邓佳训
   * 创建日期：2017/7/20 9:51:09
   * 功能描述：四类基础数据共有字段
   *
   * 修改者：         
   * 修改时间：       
   * 修改说明:
   * ==============================================================================*/
    public class BaseDataModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 场所编码
        /// </summary>
        public string NETBAR_WACODE { get; set; }
        /// <summary>
        /// 身份证件号码-身份内容
        /// </summary>
        public string CERTIFICATE_CODE { get; set; }
        /// <summary>
        /// 场所内网IP地址
        /// </summary>
        public long IP_ADDRESS { get; set; }
        /// <summary>
        /// 源外网IPv4地址
        /// </summary>
        public long SRC_IP { get; set; }
        /// <summary>
        /// 源外网IPv6地址
        /// </summary>
        public string SRC_IPV6 { get; set; }
        /// <summary>
        /// 源外网IPv4起始端口号
        /// </summary>
        public int? SRC_PORT_START { get; set; }
        /// <summary>
        /// 源外网IPv4结束端口号
        /// </summary>
        public int? SRC_PORT_END { get; set; }
        /// <summary>
        ///源外网IPv6起始端口号
        /// </summary>
        public int? SRC_PORT_START_V6 { get; set; }
        /// <summary>
        /// 源外网IPv6结束端口号
        /// </summary>
        public int? SRC_PORT_END_V6 { get; set; }
        /// <summary>
        /// 热点Mac地址;终端MAC地址
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// AP设备MAC地址
        /// </summary>
        public string AP_MAC { get; set; }
        /// <summary>
        /// AP设备编号;采集设备编号
        /// </summary>
        public string COLLECTION_EQUIPMENT_ID { get; set; }
        /// <summary>
        /// 移动AP经度
        /// </summary>
        public string LONGITUDE { get; set; }
        /// <summary>
        /// 移动AP纬度
        /// </summary>
        public string LATITUDE { get; set; }
        /// <summary>
        /// 会话ID
        /// </summary>
        public string SESSIONID { get; set; }
        /// <summary>
        /// 场强
        /// </summary>
        public string TERMINAL_FIELD_STRENGTH { get; set; }
        /// <summary>
        /// X坐标
        /// </summary>
        public string X_COORDINATE { get; set; }
        /// <summary>
        /// Y坐标
        /// </summary>
        public string Y_COORDINATE { get; set; }
        /// <summary>
        /// 终端品牌
        /// </summary>
        public string BRAND { get; set; }
        /// <summary>
        /// 外键，场所ID
        /// </summary>
        public int? NETBAR_ID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 厂商ID
        /// </summary>
        public int? SecId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 采集时间;日志记录时间
        /// </summary>
        public long? CAPTURE_TIME { get; set; }
        /// <summary>
        /// 采集设经度
        /// </summary>
        public string COLLECTION_EQUIPMENT_LONGITUDE { get; set; }
        /// <summary>
        /// 采集设备维度
        /// </summary>
        public string COLLECTION_EQUIPMENT_LATITUDE { get; set; }
        /// <summary>
        /// 采集设备ID
        /// </summary>
        public int? DevAP_ID { get; set; }
        /// <summary>
        /// 场所名称
        /// </summary>
        public string PLACE_NAME { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string APName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AUTH_ACCOUNT
        {
            set;
            get;
        }


    }
}
