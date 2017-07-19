using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    public class Verify_NetBarInfo
    {
        public Verify_NetBarInfo()
        {
            ACCESS_TYPE = 3;
            OPERATOR_NET = "01";
            BUSINESS_NATURE = "1";
        }
        /// <summary>
        /// 场所服务类型
        /// </summary>
        public int NETSITE_TYPE { get; set; }
        /// <summary>
        /// 上网服务场所编码
        /// </summary>
        public string NETBAR_WACODE { get; set; }
        /// <summary>
        /// 上网服务场所名称
        /// </summary>
        public string PLACE_NAME { get; set; }
        /// <summary>
        /// 场所经营法人
        /// </summary>
        public string LAW_PRINCIPAL_NAME { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string RELATIONSHIP_ACCOUNT { get; set; }
        /// <summary>
        /// 经营法人有效证件类型
        /// </summary>
        public string LAW_PRINCIPAL_CERTIFICATE_TYPE { get; set; }
        /// <summary>
        /// 转换后的证件类型
        /// </summary>
        public string certifiName { get; set; }
        /// <summary>
        /// 经营法人有效证件号码
        /// </summary>
        public string LAW_PRINCIPAL_CERTIFICATE_ID { get; set; }
        /// <summary>
        /// 网络接入账号或固定IP地址
        /// </summary>
        public string ACSSES_IP { get; set; }
        /// <summary>
        /// 场所详细地址（包括省市区线路/弄号）
        /// </summary>
        public string SITE_ADDRESS { get; set; }
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
        /// 警区名称
        /// </summary>
        public string SName { get; set; }
        /// <summary>
        /// 派出所名称
        /// </summary>
        public string PName { get; set; }
        /// <summary>
        /// 场所ID
        /// </summary>
        public int NETBAR_ID { get; set; }
        /// <summary>
        /// 场所网络接入方式
        /// </summary>
        private int _ACCESS_TYPE;
        public int ACCESS_TYPE
        {
            get { return _ACCESS_TYPE; }
            set { _ACCESS_TYPE = 3; }
        }
        /// <summary>
        /// 场所网络接入方式
        /// </summary>
        private string _OPERATOR_NET;
        public string OPERATOR_NET
        {
            get { return _OPERATOR_NET; }
            set { _OPERATOR_NET = "01"; }
        }
        /// <summary>
        /// 场所经营性质 0：经营 1：非经营 3：其他
        /// </summary>
        private string _BUSINESS_NATURE;
        public string BUSINESS_NATURE
        {
            get { return _BUSINESS_NATURE; }
            set { _BUSINESS_NATURE = "1"; }
        }
    }
}
