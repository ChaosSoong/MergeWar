using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
  public class SystemConfigInfo
    {
        /// <summary>
        /// 自增 ，主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 1：心跳包间隔时间 (min：5，max：30)
        /// 2：配置信息查询间隔时间 (min:10,max:480)
        /// 3：电子围栏信息发送时间 (min:1,max:5)
        /// 4：信息采集粒度 (包括：http日志、电子围栏、虚拟账户)
        /// 5：电子围栏离线时间 (min:3,max:120)
        /// 6：设备离线时间 (min:10,max:120)
        /// </summary>
        public int ConfigType { get; set; }
        /// <summary>
        /// 时间单位为分钟   http日志：1  电子围栏：2  虚拟账户：4
        /// </summary>
        public string ConfigValue { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
