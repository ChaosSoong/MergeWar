using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    /// <summary>
    /// 警区信息实体类
    /// </summary>
    public class ScenicInfo
    {
        /// <summary>
        /// ID自增变量
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 外键，指向派出所信息表PoliceInfo
        /// </summary>
        public int Pid { get; set; }

        /// <summary>
        /// 警区名称
        /// </summary>
        public string SName { get; set; }


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

        public long PageNum { get; set; }
        /// <summary>
        /// 区名称
        /// </summary>
        public string Area
        {
            get;
            set;
        }
     
        /// <summary>
        /// 省名称
        /// </summary>
        public string PName
        {
            get;
            set;
        }
        /// <summary>
        /// 市名称
        /// </summary>
        public string CName
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 区域实体类
    /// </summary>
    public class AreaInfo
    {
        /// <summary>
        /// 自增
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 类型 0：区  1：市  2：省
        /// </summary>
        public int CityType { get; set; }
        /// <summary>
        /// 省ID
        /// </summary>
        public int ProID { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public int Pid { get; set; }
        /// <summary>
        /// 省名称
        /// </summary>
        public string PName { get; set; }
        /// <summary>
        /// 市名称
        /// </summary>
        public string CName { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public long PageNum { get; set; }
        /// <summary>
        /// js变量,省市区名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 市区编号
        /// </summary>
        public string City_code { get; set; }
        public string Aguid { get; set; }
        public string Cguid{get;set;}
    }
}
