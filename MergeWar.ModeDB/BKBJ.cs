using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    /* ==============================================================================
   * 创 建 者：邓佳训
   * 创建日期：2018/4/9 14:18:31
   * 功能描述：
   *
   * 修改者：         
   * 修改时间：       
   * 修改说明:
   * ==============================================================================*/
    public class BKBJ
    {
        public class AuditInfo
        {
            /// <summary>
            /// 序号
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 用户ID
            /// </summary>
            public int Uid { get; set; }
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
            public int AID { get; set; }
            /// <summary>
            /// 任务名称
            /// </summary>
            public string TaskName { get; set; }
            /// <summary>
            /// 布控项 1.手机号码 2.MAC地址 3.批量导入
            /// </summary>
            public int CaseItem { get; set; }
            /// <summary>
            /// 布控类型 1.手机号码或MAC地址布控 3.虚拟身份布控 4.CCIC布控
            /// </summary>
            public int CaseType { get; set; }
            /// <summary>
            /// 布控内容
            /// </summary>
            public string CaseValue { get; set; }
            /// <summary>
            /// 负责人姓名
            /// </summary>
            public string HeadName { get; set; }
            /// <summary>
            /// 负责人联系电话
            /// </summary>
            public string HeadMobile { get; set; }
            /// <summary>
            /// 布控范围
            /// </summary>
            public string DeployArea { get; set; }
            /// <summary>
            /// 状态 1.禁用 2.启用
            /// </summary>
            public int IsEnabled { get; set; }
            /// <summary>
            /// 是否删除 0.未删除 1.已删除
            /// </summary>
            public int IsValid { get; set; }
            /// <summary>
            /// 是否邮件警告 1.是 2.否 
            /// </summary>
            public int MailWarn { get; set; }
            /// <summary>
            /// 告警邮箱
            /// </summary>
            public string WMail { get; set; }
            /// <summary>
            /// 布控开始时间
            /// </summary>
            public DateTime StartTime { get; set; }
            /// <summary>
            /// 布控结束时间
            /// </summary>
            public DateTime EndTime { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }
            /// <summary>
            /// 布控创建时间
            /// </summary>
            public DateTime CreateTime { get; set; }
            /// <summary>
            /// 虚拟身份类型
            /// </summary>
            public int NETWORK_APP { get; set; }
            /// <summary>
            /// 序号
            /// </summary>
            public long PageNum { get; set; }
            /// <summary>
            /// 场所类别，多个用逗号隔开
            /// </summary>
            public string NetbarType { set; get; }
        }
        public class WarningInfo
        {
            /// <summary>
            /// 序号
            /// </summary>
            public long PageNum { get; set; }
            /// <summary>
            /// ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 外键，指向场所信息表NetBarInfo
            /// </summary>
            public int NETBAR_ID { get; set; }
            /// <summary>
            /// 外键，指向案件布控表CasesInfo
            /// </summary>
            public int Cid { get; set; }
            /// <summary>
            /// 手机号码
            /// </summary>
            public string Mobile { get; set; }
            /// <summary>
            /// 移动设备MAC地址
            /// </summary>
            public string MacAddress { get; set; }
            /// <summary>
            /// 虚拟身份类型
            /// </summary>
            public string VtName { get; set; }
            /// <summary>
            /// 虚拟身份值
            /// </summary>
            public string IdValue { get; set; }
            /// <summary>
            /// 场所内网IP地址
            /// </summary>
            public long IP_ADDRESS { get; set; }
            /// <summary>
            /// 源外网IPv4地址
            /// </summary>
            public long SRC_IP { get; set; }
            /// <summary>
            /// AP设备MAC地址
            /// </summary>
            public string AP_MAC { get; set; }
            /// <summary>
            /// 是否已经推送	0:未推送 1:已推送
            /// </summary>
            public int IsPush { get; set; }
            /// <summary>
            /// 是否已确认	0:未确认1:已确认
            /// </summary>
            public int IsAffirm { get; set; }
            /// <summary>
            /// 报警类型	1：采集报警 2：审计报警
            /// </summary>
            public int WaringType { get; set; }
            /// <summary>
            /// 是否已发送邮件告警 1是 2否
            /// </summary>
            public int IsSendMail { get; set; }
            /// <summary>
            /// 报警时间
            /// </summary>
            public DateTime Createtime { get; set; }
            /// <summary>
            /// 布控时间
            /// </summary>
            public DateTime CaseCreatetime { get; set; }
            /// <summary>
            /// 上网服务场所名称
            /// </summary>
            public string PLACE_NAME { get; set; }
            /// <summary>
            /// 场所详细地址
            /// </summary>
            public string SITE_ADDRESS { get; set; }
            /// <summary>
            /// 任务名称
            /// </summary>
            public string TaskName { get; set; }
            /// <summary>
            /// 布控类型    
            /// </summary>
            public int CaseType { get; set; }
            /// <summary>
            /// 布控值
            /// </summary>
            public string CaseValue { get; set; }
            /// <summary>
            /// 警告邮箱
            /// </summary>
            public string WMail { get; set; }
            /// <summary>
            /// 布控条件
            /// </summary>
            public int CaseItem { get; set; }
            /// <summary>
            /// 负责人姓名
            /// </summary>
            public string HeadName { get; set; }
            /// <summary>
            /// 负责人联系电话
            /// </summary>
            public string HeadMobile { get; set; }
            /// <summary>
            /// 布控开始时间
            /// </summary>
            public DateTime StartTime { get; set; }
            /// <summary>
            /// 布控结束时间
            /// </summary>
            public DateTime EndTime { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }
            /// <summary>
            /// 虚拟身份类型
            /// </summary>
            public int NETWORK_APP { get; set; }
            /// <summary>
            /// 场所内网端口号
            /// </summary>
            public int PORT { get; set; }
            /// <summary>
            /// 目的公网IPv4地址
            /// </summary>
            public long DST_IP { get; set; }
            /// <summary>
            /// 是否邮件告警 1是，2否
            /// </summary>
            public int MailWarn { get; set; }
        }
    }
}
