using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    /// <summary>
    /// AP设备公共基础信息
    /// </summary>
    public class MacInfo
    {
        /// <summary>
        /// ID自增变量
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// AP设备编号
        /// </summary>
        public string COLLECTION_EQUIPMENT_ID { get; set; }
        /// <summary>
        /// AP设备MAC地址
        /// </summary>
        public string AP_MAC { get; set; }
        /// <summary>
        /// 上网服务厂商编号
        /// </summary>
        public string NETBAR_WACODE { get; set; }

        /// <summary>
        /// 设备型号（电信推过来的数据，2017-04-06添加）
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// AP设备经度
        /// </summary>
        public string LONGITUDE { get; set; }
        /// <summary>
        /// AP设备纬度
        /// </summary>
        public string LATITUDE { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        public string FLOOR { get; set; }

        /// <summary>
        /// 网络接入账号或固定IP地址
        /// </summary>
        public string AP_ACSSES_IP { get; set; }
        /// <summary>
        /// 站点信息
        /// </summary>
        public string SUBWAY_STATION { get; set; }
        /// <summary>
        /// 地铁线路信息（可选）
        /// </summary>
        public string SUBWAY_LINE_INFO { get; set; }
        /// <summary>
        /// 地铁车辆信息（可选）
        /// </summary>
        public string SUBWAY_VEHICLE_INFO { get; set; }
        /// <summary>
        /// 地铁车厢编号（可选）
        /// </summary>
        public string SUBWAY_COMPARTMENT_NUMBER { get; set; }
        /// <summary>
        /// 车牌号码（可选）
        /// </summary>
        public string CAR_CODE { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public long PageNum { get; set; }
        /// <summary>
        /// 场所ID
        /// </summary>
        public int NETBAR_ID { get; set; }
        /// <summary>
        /// AP设备类型 1：固定AP设备  2：移动AP设备
        /// </summary>
        public int APType { get; set; }
        /// <summary>
        /// 审核状态 1：待审核 3：审核成功 4：审核失败
        /// </summary>
        public int Verified { get; set; }
        /// <summary>
        /// 设备状态 1：设备工作，2：设备维护，3：设备异常
        /// </summary>
        public int DevStatis { get; set; }
        /// <summary>
        /// 心跳时间 用于判断服务在线、服务离线
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 数据时间 用于判断数据在线、数据离线
        /// </summary>
        public DateTime AuditTime { get; set; }
        /// <summary>
        /// 设备序号
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 设备Guid标识
        /// </summary>
        public string Lguid { get; set; }



        /// <summary>
        /// 场所模式
        /// </summary>
        public int ModeType { get; set; }
        /// <summary>
        /// 采集粒度
        /// </summary>
        public int LogCapture { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public int supplier { get; set; }
        /// <summary>
        /// 硬件产品类型
        /// </summary>
        public int ProjectType { get; set; }
        /// <summary>
        /// 硬件方案类型
        /// </summary>
        public int CasesType { get; set; }
        /// <summary>
        /// 是否试用期，当场所模式为城中村时写入   1：是  2：否
        /// </summary>
        public int IsTrial { get; set; }
        /// <summary>
        /// AP名称
        /// </summary>
        public string APName { get; set; }
        /// <summary>
        /// 场所名称
        /// </summary>
        public string LocaName { get; set; }


        #region 采集项目所用

        /// <summary>
        /// 采集设备名称
        /// </summary>
        public string COLLECTION_EQUIPMENT_NAME { get; set; }
        /// <summary>
        /// 采集设备地址
        /// </summary>
        public string COLLECTION_EQUIPMENT_ADDRESS { get; set; }
        /// <summary>
        /// 采集设备类型
        /// </summary>
        public int COLLECTION_EQUIPMENT_TYPE { get; set; }
        /// <summary>
        /// 安全厂商组织机构代码
        /// </summary>
        public string SECURITY_SOFTWARE_ORGCODE { get; set; }
        /// <summary>
        /// 采集经度
        /// </summary>
        public string COLLECTION_EQUIPMENT_LONGITUDE { get; set; }
        /// <summary>
        /// 采集纬度
        /// </summary>
        public string COLLECTION_EQUIPMENT_LATITUDE { get; set; }
        /// <summary>
        /// 上传数据间隔
        /// </summary>
        public int UPLOAD_TIME_INTERVAL { get; set; }
        /// <summary>
        /// 采集半径
        /// </summary>
        public int COLLECTION_RADIUS { get; set; }
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string VEHICLE_CODE { get; set; }
        #endregion
        /// <summary>
        /// 表关键字
        /// </summary>
        public string TableKey { get; set; }
        /// <summary>
        /// 生产商
        /// </summary>
        public string BusName { get; set; }
        /// <summary>
        /// 强制离线时间
        /// </summary>
        public int ForcedOfflineTime { get; set; }
        /// <summary>
        /// 电子围栏离线时间
        /// </summary>
        public int FenceOffTime { get; set; }
        /// <summary>
        /// 信道
        /// </summary>
        public int Channel { get; set; }
        /// <summary>
        /// 是否重启
        /// </summary>
        public int IsReboot { get; set; }
        /// <summary>
        /// 审计状态
        /// </summary>
        public int isopen { get; set; }
        /// <summary>
        /// 验证周期
        /// </summary>
        public int PastTime { get; set; }
        /// <summary>
        /// wifi密码
        /// </summary>
        public string WIFIPwd { get; set; }
        /// <summary>
        /// 是否隐藏SSID
        /// </summary>
        public int SSIDHidden { get; set; }
        /// <summary>
        /// 源Mac
        /// </summary>
        public string OriginalMac { get; set; }
        /// <summary>
        /// 内核版本号
        /// </summary>
        public string KernelVersion { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SysName { get; set; }
        /// <summary>
        /// 内存信息
        /// </summary>
        public string MemInfo { get; set; }
        /// <summary>
        /// 系统硬件类型
        /// </summary>
        public string SysType { get; set; }
        /// <summary>
        /// SSID
        /// </summary>
        public string SSID { get; set; }
        /// <summary>
        /// 终端版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// AP名称，省市区级联时用到
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 视频设备标识(3CID)
        /// </summary>
        public string V3CID { get; set; }

        /// <summary>
        /// 拆换机状态1：待审核2：审核成功3：审核失败 默认：0
        /// </summary>
        public int PotType { get; set; }

        public  string ProjectAccessNum { get; set; }
    }

    public class SelMacInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

}
