﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    [Serializable]
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
        /// 用户类型，1：管理员，2：分局用户，3：派出所用户，4：市用户，5：分中心用户，6：省用户，7：全国用户，8：运营人员
        /// 分局用户：  除系统管理中部分功能其他都可见
        /// 派出所用户：只能查看运营管理，并且只能查看自己派出所范围内信息
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

        /// <summary>
        /// 用户所拥有的页面列表
        /// </summary>
        public List<Sys_UserPowerInfo> PowerList { get; set; }
        /// <summary>
        /// 用户所拥有的父页面和子页面列表
        /// </summary>
        public List<Sys_UserPowerInfo> PowerPathList { get; set; }
    }
}
