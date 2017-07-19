using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    public class SecurityOrg
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 厂商名称
        /// </summary>
        public string SECURITY_SOFTWARE_ORGNAME { get; set; }
        /// <summary>
        /// 厂商组织结构代码
        /// </summary>
        public string SECURITY_SOFTWARE_ORGCODE { get; set; }
        /// <summary>
        /// 厂商地址
        /// </summary>
        public string SECURITY_SOFTWARE_ORG_ADDRESS { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string CONTACTOR { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string CONTACTOR_TEL { get; set; }
        /// <summary>
        /// 联系人邮件
        /// </summary>
        public string CONTACTOR_MAIL { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public long PageNum { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
