using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    /// <summary>
    /// 场所信息实体
    /// 创建人：韩希鹏
    /// 日期：2017-06-19
    /// </summary>
    public class Loc_NetBarInfo
    {
        /// 自增ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 上网服务场所编码
        /// </summary>
        public string NETBAR_WACODE { get; set; }
        /// <summary>
        /// 上网服务场所名称
        /// </summary>
        public string PLACE_NAME { get; set; }
        /// <summary>
        /// 场所详细地址（包括省市区线路/弄号）
        /// </summary>
        public string SITE_ADDRESS { get; set; }
        /// <summary>
        /// 场所经度
        /// </summary>
        public string LONGITUDE { get; set; }
        /// <summary>
        /// 场所纬度
        /// </summary>
        public string LATITUDE { get; set; }
        /// <summary>
        /// 场所服务类型
        /// </summary>
        public int NETSITE_TYPE { get; set; }
        /// <summary>
        /// 场所经营性质 0：经营 1：非经营 3：其他
        /// </summary>
        public string BUSINESS_NATURE { get; set; }
        /// <summary>
        /// 场所经营法人
        /// </summary>
        public string LAW_PRINCIPAL_NAME { get; set; }
        /// <summary>
        /// 经营法人有效证件类型
        /// </summary>
        public string LAW_PRINCIPAL_CERTIFICATE_TYPE { get; set; }
        /// <summary>
        /// 经营法人有效证件号码
        /// </summary>
        public string LAW_PRINCIPAL_CERTIFICATE_ID { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string RELATIONSHIP_ACCOUNT { get; set; }
        /// <summary>
        /// 营业开始时间
        /// </summary>
        public string START_TIME { get; set; }
        /// <summary>
        /// 营业结束时间
        /// </summary>
        public string END_TIME { get; set; }
        /// <summary>
        /// 场所网络接入方式
        /// </summary>
        public int ACCESS_TYPE { get; set; }
        /// <summary>
        /// 场所网络接入服务商
        /// </summary>
        public string OPERATOR_NET { get; set; }
        /// <summary>
        /// 网络接入账号或固定IP地址
        /// </summary>
        public string ACSSES_IP { get; set; }
        /// <summary>
        /// 安全厂商组织机构代码
        /// </summary>
        public string CODE_ALLOCATION_ORGANIZATION { get; set; }
        /// <summary>
        /// 安全厂商组织机构代码
        /// </summary>
        public string SECURITY_SOFTWARE_ORGCODE { get; set; }
        /// <summary>
        /// 场所状态 1：营业状态 2：停业状态
        /// </summary>
        public int Statis { get; set; }
        /// <summary>
        /// 是否删除 0：已删除 1：未删除
        /// </summary>
        public int Valid { get; set; }
        /// <summary>
        /// 创建时间 
        /// </summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        /// 审核状态 1：待审核  3：审核成功 4：审核失败
        /// </summary>
        public int Verified { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public long PageNum { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public int Service_code { get; set; }
        /// <summary>
        /// 省ID
        /// </summary>
        public int ProID { get; set; }
        /// <summary>
        /// 市ID
        /// </summary>
        public int CityID { get; set; }
        /// <summary>
        /// 区ID
        /// </summary>
        public int Aid { get; set; }
        /// <summary>
        /// 派出所ID
        /// </summary>
        public int Pid { get; set; }
        /// <summary>
        /// 警区ID
        /// </summary>
        public int Sid { get; set; }
        /// <summary>
        /// 派出所名称
        /// </summary>
        public string PName { get; set; }
        /// <summary>
        /// 警区名称
        /// </summary>
        public string SName { get; set; }
        /// <summary>
        /// AP设备数量
        /// </summary>
        public int APNum { get; set; }
        /// <summary>
        /// 厂商数据来源
        /// </summary>
        public string MakeType { get; set; }
        /// <summary>
        /// 表关键字
        /// </summary>
        public string TableKey { get; set; }
        /// <summary>
        /// 场所名称
        /// </summary>
        public string Name { get; set; }
        public int fuwu { get; set; }
        public int shuju { get; set; }
        /// <summary>
        /// 是否更新场所编码	0：否   1：是
        /// </summary>
        public int IsUpdate_NETBAR_WACODE { get; set; }
        /// <summary>
        /// 是否是含有视频设备的场所  0：否  1：是
        /// </summary>
        public int IsVoid { get; set; }
        /// <summary>
        /// 安全厂商ID
        /// </summary>
        public int SecId { get; set; }
        /// <summary>
        /// 设备MAC地址
        /// </summary>
        public string AP_MAC { get; set; }
        /// <summary>
        /// 安全厂商名称
        /// </summary>
        public string SECURITY_SOFTWARE_ORGNAME { get; set; }
    }
}
