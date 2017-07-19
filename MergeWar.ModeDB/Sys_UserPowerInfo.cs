using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
      [Serializable]
      public class Sys_UserPowerInfo
      {
          /// <summary>
          /// 主键ID
          /// </summary>
          public int Id { get; set; }
          /// <summary>
          /// 权限值
          /// </summary>
          public string SelValues { get; set; }
          /// <summary>
          /// 页面的权限值
          /// </summary>
          public string IdValues { get; set; }
          /// <summary>
          /// 用户类型
          /// </summary>
          public string TypeName { get; set; }
          /// <summary>
          /// 页面名称
          /// </summary>
          public string Name { get; set; }
          /// <summary>
          /// 页面路径
          /// </summary>
          public string FilePath { get; set; }
          /// <summary>
          /// 父ID
          /// </summary>
          public int Pid { get; set; }
          /// <summary>
          /// 页面目录ID
          /// </summary>
          public int SpId { get; set; }
          /// <summary>
          /// 页面序号
          /// </summary>
          public int Indexs { get; set; }
          /// <summary>
          /// 点击显示背景的标识文字
          /// </summary>
          public string SelShowValue { get; set; }
          /// <summary>
          /// 图标数字
          /// </summary>
          public string IconNum { get; set; }
          /// <summary>
          /// 角色名称
          /// </summary>
          public string RoleName { get; set; }
          /// <summary>
          /// 角色ID
          /// </summary>
          public int Jid { get; set; }
          /// <summary>
          /// 创建时间
          /// </summary>
          public DateTime CreateTime { get; set; }
          /// <summary>
          /// 角色在权限关系表中的数量
          /// </summary>
          public int JNum { get; set; }
          /// <summary>
          /// 该角色下存在几个用户
          /// </summary>
          public int JUserNum { get; set; }
          /// <summary>
          /// 权限关系表ID
          /// </summary>
          public int Qid { get; set; }
          /// <summary>
          /// 当前登录失败次数
          /// </summary>
          public int ErrorNum { get; set; }
          /// <summary>
          /// 上次登录成功之后登录失败次数
          /// </summary>
          public int AtErrorNum { get; set; }

          /// <summary>
          /// 列表目录的路径
          /// </summary>
          public string FPath { get; set; }
          /// <summary>
          /// 页面类型
          /// </summary>
          public int PageType { get; set; }
          public string Class { get; set; }
      }
    }

