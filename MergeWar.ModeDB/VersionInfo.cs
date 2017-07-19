using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
   public class VersionInfo
    {
       /// <summary>
        /// 终端版本信息实体类
       /// </summary>
       ///主键
        public int ID
        {
            get;
            set;
        }
       /// <summary>
       /// 1：asp程序 2：审计程序 3：APP监管版 4：APP商户版 5：web网站
       /// </summary>
        public int Type
        {
            get;
            set;
        }
       /// <summary>
        /// 区域表外键
       /// </summary>
        public int Aid
        {
            get;
            set;
        }
       /// <summary>
        /// 升级类型	1：全部升级 2：指定省升级 3：指定市升级 4：指定区升级
       /// </summary>
        public int UpdateType
        {
            get;
            set;
        }
       /// <summary>
        /// 升级范围	1：无 2：除城中村升级 3：除旁路升级 4：除城中村和旁路升级
       /// </summary>
        public int otherType
        {
            get;
            set;
        }
       /// <summary>
        /// 验证类型，支持多选，多选之间使用逗号分割，00,01,02	00：关闭审计 01：开启审计 02：第三方验证 03：微信验证               04：WIFIDOG验证
       /// </summary>
        public string VerifyType
        {
            get;
            set;
        }
       /// <summary>
        /// 硬件方案类型	1：Atheros 2：7620N 3：X86
       /// </summary>
        public int CasesType
        {
            get;
            set;
        }
        public string AuditType
        {
            get;
            set;
        }
       /// <summary>
        /// 外部版本
       /// </summary>
        public string OutVersion
        {
            get;
            set;
        }
       /// <summary>
        /// 内部版本版本信息
       /// </summary>
        public string Version
        {
            get;
            set;
        }
       /// <summary>
        /// 修改日志
       /// </summary>
        public string ChangeLog
        {
            get;
            set;
        }
       /// <summary>
        /// 是否强制升级	1:强制升级 0:非强制升级
       /// </summary>
        public int FocusUpdate
        {
            get;
            set;
        }
       /// <summary>
        /// 更新包下载地址
       /// </summary>
        public string DownloadUrl
        {
            get;
            set;
        }
       /// <summary>
        /// 创建时间
       /// </summary>
        public DateTime CreateTime
        {
            get;
            set;
        }
        public long PageNum
        {
            get;
            set;
        }
    }
}
