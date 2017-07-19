using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common
{
    public class DataSetConvert
    {
        /// <summary>
        /// 结果集对象
        /// </summary>
        private DataSet _data;

        /// <summary>
        /// 结果集对象中Table索引值
        /// </summary>
        private int _tableIndex;
        /// <summary>
        /// 构造器私有，禁止外部访问无参构造器来创建对象
        /// </summary>
        private DataSetConvert()
        {

        }

        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <param name="ds">想转换的结果集</param>
        public DataSetConvert(DataSet ds)
        {
            this._data = ds;
            this._tableIndex = 0;
        }

        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <param name="ds">想转换的结果集</param>
        /// <param name="tableindex">结果集Table下标值</param>
        public DataSetConvert(DataSet ds, int tableindex)
        {
            this._data = ds;
            this._tableIndex = tableindex;
        }

        /// <summary>
        /// 返回单条Model对象
        /// </summary>
        /// <typeparam name="T">model范型</typeparam>
        /// <returns>model对象</returns>
        public T Get_SingleModel<T>() where T : class , new()
        {
            try
            {
                if (_data.Tables[_tableIndex].Rows.Count > 0)
                {
                    T model = new T();
                    DataColumnCollection columns = _data.Tables[_tableIndex].Columns;
                    DataRow row = _data.Tables[_tableIndex].Rows[0];
                    PropertyInfo[] properties = typeof(T).GetProperties();
                    foreach (PropertyInfo pro in properties)
                    {
                        if (columns.Contains(pro.Name))
                        {
                            if (row[pro.Name] == DBNull.Value)
                            {
                                pro.SetValue(model, null, null);
                            }
                            else
                            {
                                pro.SetValue(model, row[pro.Name], null);
                            }
                        }
                    }
                    return model;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回单条Model对象
        /// </summary>
        /// <typeparam name="T">model范型</typeparam>
        /// <param name="tableIndex">结果集Table下标值</param>
        /// <returns>model对象</returns>
        public T Get_SingleModel<T>(int tableIndex) where T : class , new()
        {
            try
            {
                this._tableIndex = tableIndex;
                return this.Get_SingleModel<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回Model集合对象
        /// </summary>
        /// <typeparam name="T">model范型</typeparam>
        /// <returns>Model集合对象</returns>
        public List<T> Get_ListModel<T>() where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();
                List<PropertyInfo> prolist = new List<PropertyInfo>();
                T model = new T();
                DataColumnCollection collist = _data.Tables[_tableIndex].Columns;
                DataRowCollection rows = _data.Tables[_tableIndex].Rows;
                PropertyInfo[] tempprolist = typeof(T).GetProperties();
                foreach (PropertyInfo pro in tempprolist)
                {
                    if (collist.Contains(pro.Name))
                        prolist.Add(pro);
                }

                foreach (DataRow row in rows)
                {
                    T tempT = new T();
                    foreach (PropertyInfo p in prolist)
                    {
                        if (row[p.Name] == DBNull.Value)
                        {
                            p.SetValue(tempT, null, null);
                        }
                        else
                        {
                            p.SetValue(tempT, row[p.Name], null);
                        }
                    }
                    list.Add(tempT);
                }
                return list;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回Model集合对象
        /// </summary>
        /// <typeparam name="T">model范型</typeparam>
        /// <param name="tableIndex">结果集Table下标值</param>
        /// <returns>Model集合对象</returns>
        public List<T> Get_ListModel<T>(int tableIndex) where T : class, new()
        {
            try
            {
                this._tableIndex = tableIndex;
                return this.Get_ListModel<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
