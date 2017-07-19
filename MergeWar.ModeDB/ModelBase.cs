using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/* ==============================================================================
   * 创 建 者：王继伟
   * 创建日期：2017/7/11 15:30:36
   * 功能描述：
   *
   * 修改者：         
   * 修改时间：       
   * 修改说明:
   * ==============================================================================*/


namespace HCZZ.ModeDB
{
    public class ModelBase
    {
        /// <summary>
        /// 序号
        /// </summary>
        public long PageNum { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}