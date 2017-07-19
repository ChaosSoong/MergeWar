using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    /// <summary>
    /// 创建人：邓佳训
    /// 创建时间：2017-06-15
    /// </summary>
   public class Sys_PowerInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID
       { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FilePath
        { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Pid
        { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Indexs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Valid
        { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SelShowValue
        { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IconNum
        { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IdValues
        { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        { get; set; }
    }
}
