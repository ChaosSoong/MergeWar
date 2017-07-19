 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    /// <summary>
    /// 派出所信息实体类
    /// </summary>
    public class PoliceInfo
    {
        /// <summary>
        /// ID自增变量
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 外键，指向区域信息表AreaInfo
        /// </summary>
        public int Aid { get; set; }

        /// <summary>
        /// 派出所名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 删除标示  0：已删除 1：未删除
        /// </summary>
        public int Valid { get; set; }
        /// <summary>
        /// 派出所编号
        /// </summary>
        public string PoliceInfo_code { get; set; }

        /// <summary>
        /// 区名称
        /// </summary>
        public string Area { get; set; }
        public string PName
        {
            get;
            set;
        }
        public string CName
        {
            get;
            set;
        }
        public long PageNum { get; set; }

    }
}
