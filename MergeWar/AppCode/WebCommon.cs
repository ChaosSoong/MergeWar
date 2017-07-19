using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HCZZ.ModeDB;
using HCZZ.DAL;
using Common;

namespace HCZZ.AppCode
{
    public class WebCommon
    {
        /// <summary>
        /// 重新获取缓存的场所信息
        /// </summary>
        /// <param name="isReset"></param>
        public static void GetCacheLoca(bool isReset)
        {
            if (HttpContext.Current.Cache["ShowCacheLoca"] == null || isReset)
            {
                List<Loc_NetBarInfo> list = new LocationDAL().GetLocationList();
                HttpContext.Current.Cache.Insert("ShowCacheLoca", list, null, DateTime.Now.AddHours(6), TimeSpan.Zero);
            }
        }
        /// <summary>
        /// 获取设备缓存信息
        /// </summary>
        /// <param name="p"></param>
        public static void GetCacheMacAllList(bool isReset)
        {
            if (HttpContext.Current.Cache["CacheMacAll"] == null || isReset)
            {
                List<Loc_DevInfo> list = new LocationDAL().GetDevList();
                HttpContext.Current.Cache.Insert("CacheMacLoca", list, null, DateTime.Now.AddHours(6), TimeSpan.Zero);
            }
        }
        /// <summary>
        /// 缓存未使用的Mac地址集合
        /// </summary>
        public static void GetCacheMac(bool isReset)
        {
            if (HttpContext.Current.Cache["CacheMac"] == null || isReset)
            {
                List<MacInfo> list = new MacDAL().GetNoMacList();
                HttpContext.Current.Cache.Insert("CacheMac", list, null, DateTime.Now.AddHours(5), TimeSpan.Zero);
            }
        }
        /// <summary>
        /// 获取缓存中Mac地址匹配的列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<MacInfo> GetMacList(string keyword)
        {
            List<MacInfo> list = null;
            if (HttpContext.Current.Cache["CacheMac"] == null)
            {
                list = new MacDAL().GetNoMacList();
                HttpContext.Current.Cache.Insert("CacheMac", list, null, DateTime.Now.AddHours(5), TimeSpan.Zero);
                Logger.writeLog(list.Count.ToString());
            }
            else
                list = (List<MacInfo>)HttpContext.Current.Cache["CacheMac"];

            if (!string.IsNullOrEmpty(keyword))
            {
                IEnumerable<MacInfo> IEList = list;
                IEList = IEList.Where(m => m.COLLECTION_EQUIPMENT_ID.Contains(keyword)).Take(10);
                if (IEList.ToList().Count() > 0)
                    return IEList.ToList();
                else
                    return null;
            }
            else
                return null;
        }
        /// <summary>
        /// 根据场所ID获取场所对象
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Loc_NetBarInfo GetLocaInfoByNetBar_ID(int NETBAR_ID)
        {
            return GetLocaInfoByNetBar_ID(NETBAR_ID, "");
        }

                /// <summary>
        /// 获取缓存中场所对象信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static Loc_NetBarInfo GetLocaInfoByNetBar_ID(int NETBAR_ID, string Lname)
        {
            Loc_NetBarInfo loca = GetLocaInfoByNetBar_ID_Two(NETBAR_ID, Lname);
            if (loca == null)
                return GetLocaInfoByNetBar_ID_Two(NETBAR_ID, Lname);
            return loca;
        }

        /// <summary>
        /// 获取缓存中场所对象信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static Loc_NetBarInfo GetLocaInfoByNetBar_ID_Two(int NETBAR_ID, string Lname)
        {
            List<Loc_NetBarInfo> list = null;
            if (HttpContext.Current.Cache["ShowCacheLoca"] == null)
                GetCacheLoca();
            else
                list = (List<Loc_NetBarInfo>)HttpContext.Current.Cache["ShowCacheLoca"];

            if (NETBAR_ID > 0 || !string.IsNullOrEmpty(Lname))
            {
                IEnumerable<Loc_NetBarInfo> IEList = list;
                try
                {
                    if (NETBAR_ID > 0)
                        IEList = IEList.Where(m => m.ID == NETBAR_ID);
                    else
                        IEList = IEList.Where(m => m.PLACE_NAME == Lname);
                }
                catch (Exception)
                {
                    return null;
                }

                list = IEList.ToList();
                if (list != null && list.Count != 0)
                    return list.First();
                else
                {
                    Loc_NetBarInfo loca = new LocationDAL().GetLocaByIdOrName(NETBAR_ID, Lname);
                    if (loca != null)
                    {
                        list.Add(loca);
                        HttpContext.Current.Cache.Insert("ShowCacheLoca", list, null, DateTime.Now.AddHours(6), TimeSpan.Zero);
                        return loca;
                    }
                    return null;
                }
            }
            else
                return null;
        }
        /// <summary>
        /// 获取缓存的场所信息
        /// </summary>
        public static void GetCacheLoca()
        {
            GetCacheLoca(false);
        }

        /// <summary>
        /// 获取缓存信息
        /// </summary>
        /// <returns></returns>
        public static List<SecurityOrg> RefSecurityList()
        {
            if (HttpContext.Current.Cache["ShowCacheSecurity"] == null)
            {
                List<SecurityOrg> list = new HBHtmlDAL().GetAQList();
                HttpContext.Current.Cache.Insert("ShowCacheSecurity", list, null, DateTime.Now.AddHours(6), TimeSpan.Zero);
                return list;
            }
            else
                return (List<SecurityOrg>)HttpContext.Current.Cache["ShowCacheSecurity"];
        }


        #region  获取设备信息
        /// <summary>
        /// 获取缓存中设备对象信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static Loc_DevInfo GetMacInfoByDevAP_ID(int DevAP_ID, string AP_MAC)
        {
            Loc_DevInfo mac = GetMacInfoByDevAP_ID_Two(DevAP_ID, AP_MAC);
            if (mac == null)
                return GetMacInfoByDevAP_ID_Two(DevAP_ID, AP_MAC);
            return mac;
        }
        /// <summary>
        /// 获取缓存中设备对象信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static Loc_DevInfo GetMacInfoByDevAP_ID_Two(int DevAP_ID, string AP_MAC)
        {
            List<Loc_DevInfo> list = null;
            if (HttpContext.Current.Cache["CacheMacAll"] == null)
                GetCacheMacAllList();
            else
                list = (List<Loc_DevInfo>)HttpContext.Current.Cache["CacheMacAll"];

            if (DevAP_ID > 0 || !string.IsNullOrEmpty(AP_MAC))
            {
                IEnumerable<Loc_DevInfo> IEList = list;
                try
                {
                    if (DevAP_ID > 0)
                        IEList = IEList.Where(m => m.ID == DevAP_ID);
                    else if (!string.IsNullOrEmpty(AP_MAC))
                        IEList = IEList.Where(m => m.AP_MAC == AP_MAC);
                }
                catch (Exception)
                {
                    return null;
                }

                list = IEList.ToList();
                if (list != null && list.Count != 0)
                    return list.First();
                else
                {
                    Loc_DevInfo mac = new LocationDAL().GetMacInfoByIdOrMac(DevAP_ID, AP_MAC);
                    if (mac != null)
                    {
                        list.Add(mac);
                        HttpContext.Current.Cache.Insert("CacheMacAll", list, null, DateTime.Now.AddHours(6), TimeSpan.Zero);
                        return mac;
                    }
                    return null;
                }
            }
            else
                return null;
        }
        /// <summary>
        /// 获取缓存的全部设备信息
        /// </summary>
        public static void GetCacheMacAllList()
        {
            GetCacheMacAllList(false);
        }


        /// <summary>
        /// 获取安全厂商的缓存
        /// </summary>
        public static void GetSecurityList(bool isReset)
        {
            if (HttpContext.Current.Cache["ShowCacheSecurity"] == null || isReset)
            {
                List<SecurityOrg> list = new HBHtmlDAL().GetAQList();
                HttpContext.Current.Cache.Insert("ShowCacheSecurity", list, null, DateTime.Now.AddHours(6), TimeSpan.Zero);
            }
        }
        #endregion
    }
}