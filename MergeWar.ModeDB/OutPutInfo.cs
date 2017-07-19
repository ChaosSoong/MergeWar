using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    /// <summary>
    /// 数据输出管理
    /// </summary>
    public class OutPutInfo
    {
        /// <summary>
        /// 自增长ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string UserPwd { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ  { get; set; }
        /// <summary>
        /// Email邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 传输模式
        /// </summary>
        public int Pattern { get; set; }
        /// <summary>
        /// 传输目地
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime EstablishTime { get; set; }
        public long PageNum { get; set; }  
    }
}
