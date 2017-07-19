using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    /// <summary>
    /// 设备信息实体
    /// 创建人：韩希鹏
    /// 2017-06-19
    /// </summary>
    public class Loc_DevInfo
    {
        public int DevInf_ID { get; set; }
        public int ID { get; set; }
        public string COLLECTION_EQUIPMENT_ID { get; set; }
        public string AP_MAC { get; set; }
        public string NETBAR_WACODE { get; set; }
        public string LONGITUDE { get; set; }
        public string LATITUDE { get; set; }
        public string FLOOR { get; set; }
        public string SUBWAY_STATION { get; set; }
        public string SUBWAY_LINE_INFO { get; set; }
        public string SUBWAY_VEHICLE_INFO { get; set; }
        public string SUBWAY_COMPARTMENT_NUMBER { get; set; }
        public string CAR_CODE { get; set; }
        public int NETBAR_ID { get; set; }
        public int APType { get; set; }
        public int Verified { get; set; }
        public DateTime CreateTime { get; set; }
        public int DevStatis { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime AuditTime { get; set; }
        public string Lguid { get; set; }
        public string SN { get; set; }
        public int IsTrial { get; set; }
        public int Supplier { get; set; }
        public int ProjectType { get; set; }
        public int CasesType { get; set; }
        public int ModeType { get; set; }
        public int LogCapture { get; set; }
        public string APName { get; set; }
        public int UPLOAD_TIME_INTERVAL { get; set; }
        public int COLLECTION_RADIUS { get; set; }
        public int Channel { get; set; }
        public int IsReboot { get; set; }
        public int ForcedOfflineTime { get; set; }
        public int FenceOffTime { get; set; }
        public int Isopen { get; set; }
        public int PastTime { get; set; }
        public string WIFIPwd { get; set; }
        public int SSIDHidden { get; set; }
        public string OriginalMac { get; set; }
        public string KernelVersion { get; set; }
        public string SysName { get; set; }
        public string MemInfo { get; set; }
        public string SysType { get; set; }
        public string SSID { get; set; }
        public string Version { get; set; }
        public string V3CID { get; set; }
        public string DeviceName { get; set; }
        public string ProjectAccessNum { get; set; }
        public string MatchTime { get; set; }
        public string AP_ACSSES_IP { get; set; }
        public string Remark { get; set; }
        public long PageNum { get; set; }

    }
}
