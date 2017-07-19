using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
  public  class UserInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID{  set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string TrueName
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Password
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Mobile
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Email
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int  idType
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string idNumber
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int ProID
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int CityID
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int AId
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int    PId
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int    Type
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int    Valid
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int    CreateId
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int    AddNum
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime    LastTime
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string usbkey
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime FailureTime
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string ToKen
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int isLog
        { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int  JId
        { set; get; }

      public  long PageNum { get; set; }
      /// <summary>
      /// 派出所名称
      /// </summary>
      public string Pname { get; set; }

        public List<Sys_UserPowerInfo> PowerPathList { get; set; }
      /// <summary>
      /// 用户可见的页面
      /// </summary>
        public List<Sys_UserPowerInfo> _UserShowPage { get; set; }
    }
}
