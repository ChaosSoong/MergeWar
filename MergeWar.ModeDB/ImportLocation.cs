using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    public class ImportLocation: ModelBase
    {
        public ImportLocation()
        {
            LONGITUDE = "114.061513";
            LATITUDE = "22.549316";
        }
        /// <summary>
        /// 场所名称
        /// </summary>
        public string LocaName { get; set; }
        /// <summary>
        /// 场所类型
        /// </summary>
        public int LocaType { get; set; }
        /// <summary>
        /// 警区ID
        /// </summary>
        public int Sid { get; set; }
        /// <summary>
        /// 设备mac
        /// </summary>
        public string Mac { get; set; }
        /// <summary>
        /// 硬件产品类型
        /// </summary>
        public int ProjectType { get; set; }
        /// <summary>
        /// 硬件方案类型
        /// </summary>
        public int CasesType { get; set; }
        /// <summary>
        /// APID
        /// </summary>
        public int DveId { get; set; }
        /// <summary>
        /// 场所ID
        /// </summary>
        public int NetBarId { get; set; }
        /// <summary>
        /// 场所地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 场所企业法人
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// AP名称
        /// </summary>
        public string ApName { get; set; }
        /// <summary>
        /// AP设备类型
        /// </summary>
        public int ApType { get; set; }
        /// <summary>
        /// 默认经度
        /// </summary>
        public string LONGITUDE { get; set; }
        /// <summary>
        /// 默认纬度
        /// </summary>
        public string LATITUDE { get; set; }
        public int Service_code { get; set; }
        public string NETBAR_WACODE { get; set; }
        public int Aid { get; set; }
        public int CityId { get; set; }
        public int ProId { get; set; }
        public int Pid { get; set; }
        /// <summary>
        /// 厂商组织机构代码
        /// </summary>
        public string SECURITY_SOFTWARE_ORGCODE { get; set; }
        /// <summary>
        /// 厂商名称
        /// </summary>
        public string QecurityName { get; set; }
        /// <summary>
        /// 1：待审核 3：审核成功  4：审核失败
        /// </summary>
        public int verify { get; set; }
    }

    public class Json_DevInfo
    {
        /// <summary>
        /// 设备mac
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// AP名称
        /// </summary>
        public string APName { get; set; }
    }
}
