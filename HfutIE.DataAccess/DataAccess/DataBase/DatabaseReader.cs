using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HfutIE.DataAccess
{
    /// <summary>
    /// 利用反射实现通用的DataReader转List、DataReader转实体类 
    /// </summary>
    public class DatabaseReader
    {
        /// <summary>
        ///  将IDataReader转换为DataTable
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static DataTable ReaderToDataTable(IDataReader dr)
        {
            using (dr)
            {
                DataTable objDataTable = new DataTable("Table");
                int intFieldCount = dr.FieldCount;
                for (int intCounter = 0; intCounter < intFieldCount; ++intCounter)
                {
                    objDataTable.Columns.Add(dr.GetName(intCounter).ToLower(), dr.GetFieldType(intCounter));
                }
                objDataTable.BeginLoadData();
                object[] objValues = new object[intFieldCount];
                while (dr.Read())
                {
                    dr.GetValues(objValues);
                    objDataTable.LoadDataRow(objValues, true);
                }
                dr.Close();
                dr.Dispose();
                objDataTable.EndLoadData();
                return objDataTable;
            }
        }
        /// <summary>
        /// 将IDataReader转换为 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static List<T> ReaderToList<T>(IDataReader dr)
        {
            using (dr)
            {
                List<string> field = new List<string>(dr.FieldCount);
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    field.Add(dr.GetName(i).ToLower());
                }
                List<T> list = new List<T>();
                while (dr.Read())
                {
                    T model = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (field.Contains(property.Name.ToLower()))
                        {
                            if (!IsNullOrDBNull(dr[property.Name]))
                            {
                                var s = HackType(dr[property.Name], property.PropertyType);
                                property.SetValue(model, HackType(dr[property.Name], property.PropertyType), null);
                            }
                        }
                    }
                    list.Add(model);
                }
                dr.Close();
                dr.Dispose();
                return list;
            }
        }
        /// <summary>
        /// 将IDataReader转换为 实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T ReaderToModel<T>(IDataReader dr)
        {
            using (dr)
            {
                T model = Activator.CreateInstance<T>();
                while (dr.Read())
                {
                    foreach (PropertyInfo pi in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                    {  
                        if (!IsNullOrDBNull(dr[pi.Name]))
                        {
                            pi.SetValue(model, HackType(dr[pi.Name], pi.PropertyType), null);
                        }
                    }
                }
                dr.Close();
                dr.Dispose();
                return model;
            }
        }
        /// <summary>
        /// 将IDataReader转换为 哈希表
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static Hashtable ReaderToHashtable(IDataReader dr)
        {
            using (dr)
            {
                Hashtable ht = new Hashtable();
                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        string strfield = dr.GetName(i).ToLower();
                        ht[strfield] = dr[strfield];
                    }
                }
                dr.Close();
                dr.Dispose();
                return ht;
            }
        }
        //这个类对可空类型进行判断转换，要不然会报错
        public static object HackType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }
        public static bool IsNullOrDBNull(object obj)
        {
            return ((obj is DBNull) || string.IsNullOrEmpty(obj.ToString())) ? true : false;
        }

        /// <summary>
        ///   将IDataReader转换为DataTable  此方法未强制转化搜索条件为小写，避免上层出现大小写问题不匹配  F
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="iSLower">是否将搜索条件转化为小写的标识</param>
        /// <returns></returns>
        public static DataTable ReaderToDataTable(IDataReader dr, bool iSLower)//为避免影响程序中其他地方正常运行，选择重载这个方法
        {
            using (dr)
            {
                DataTable objDataTable = new DataTable("Table");
                int intFieldCount = dr.FieldCount;
                for (int intCounter = 0; intCounter < intFieldCount; ++intCounter)
                {
                    if (iSLower)
                    {
                        objDataTable.Columns.Add(dr.GetName(intCounter).ToLower(), dr.GetFieldType(intCounter));
                    }
                    else
                    {
                        objDataTable.Columns.Add(dr.GetName(intCounter), dr.GetFieldType(intCounter));
                    }

                }
                objDataTable.BeginLoadData();
                object[] objValues = new object[intFieldCount];
                while (dr.Read())
                {
                    dr.GetValues(objValues);
                    objDataTable.LoadDataRow(objValues, true);
                }
                dr.Close();
                dr.Dispose();
                objDataTable.EndLoadData();
                return objDataTable;
            }
        }

    }
}
