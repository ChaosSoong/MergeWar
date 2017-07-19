using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    /// <summary>
    /// 创建人：邓佳训
    /// 创建时间：2017-06-14
    /// 最后修改时间：2017-06-14
    /// 首页数据统计
    /// </summary>
  public class IndexStatisInfo
    {
      /// <summary>
      /// ID
      /// </summary>
      public  int ID { get; set; }
      /// <summary>
      /// 外键，指向区域信息表AreaInfo
      /// </summary>
      public int Aid { get; set; }
      /// <summary>
      /// 统计类型
      /// 1：采集数量趋势2：采集设备数量趋势3：场所服务类型数据4：设备数量数据5：地图下方实时数据
      /// </summary>
      public int StatisType { get; set; }
      /// <summary>
      /// 数据类型 采集数量趋势/采集设备数量趋势：30-35 场所服务类型：1-13
      ///1：旅店宾馆类2：图书馆阅览室3：电脑培训中心4：娱乐休闲场所5：交通枢纽6：公共交通工具7：餐饮服务场所8：金融服务场所10：购物场所11：公共服务场所12：文化服务场所13：公共休闲场所9：其他30：手机号码31：Mac32：虚拟身份33：IMEI34：IMSI35：车牌号码
      /// </summary>
      public int DataType { get; set; }
      /// <summary>
      /// 采集/设备在线数量（场所）
      /// </summary>
      public  long DataNum { get; set; }
      /// <summary>
      /// 设备总数/场所总量
      /// </summary>
      public  long ZoomNum { get; set; }
      /// <summary>
      /// 手机号码
      /// </summary>
        public string Mobile { get; set; }
      /// <summary>
        /// 网络应用服务类型
      /// </summary>
        public string NETWORK_APP { get; set; }
      /// <summary>
        /// 网络应用服务类型值
      /// </summary>
        public string IdValue { get; set; }
      /// <summary>
        /// 上网服务场所名称
      /// </summary>
        public string PLACE_NAME { get; set; }
      /// <summary>
        /// 统计时间
      /// </summary>
        public DateTime StatisTime { get; set; }
      /// <summary>
        /// 写入时间
      /// </summary>
        public DateTime WriteTime { get; set; }
      /// <summary>
        /// 整型写入时间
      /// </summary>
        public  long NWriteTime { get; set; }
    }
}
