//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Mvc;

namespace HfutIE.Business
{
    /// <summary>
    /// 点位功能配置信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.10.13 20:57</date>
    /// </author>
    /// </summary>
    public class BBdbR_PointFunctionConfigBaseBll : RepositoryFactory<BBdbR_PointFunctionConfigBase>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_PointFunctionConfigBase";//===复制时需要修改===
        #endregion

        #region 方法区

        #region 1.页面表格
        /// <summary>
        /// 展示页面表格
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public DataTable GetPlanList0(string KeyValue, JqGridParam jqgridparam)//已配置下表
        {
            StringBuilder sql = new StringBuilder();
            DataTable dt = new DataTable();
            sql.Append(@"SELECT a.*,b.FunctionCd,b.FunctionNm FROM  " + tableName + " a join BBdbR_FunctionBase b on a.FunctionId=b.FunctionId where a.Enable=1 and a.PointId='"+KeyValue+"' ");
            dt = Repository().FindTableBySql(sql.ToString(), false);
            return dt;
        }
        #endregion

        #region 功能基础信息表（上表）
        public DataTable GetPlanList()//功能基础信息表（上表）
        {
            StringBuilder sql1 = new StringBuilder();
            StringBuilder sql2 = new StringBuilder();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            sql1.Append(@"SELECT AviId AS PointId,'1' as PointCatg,AviCd AS PointCd,AviNm AS PointNm FROM  BBdbR_AVIBase where Enabled=1 ");
            dt1 = Repository().FindTableBySql(sql1.ToString(), false);
            sql2.Append(@"SELECT WcId AS PointId,'2' as PointCatg,WcCd AS PointCd,WcNm AS PointNm FROM  BBdbR_WcBase where Enabled=1 ");
            dt2 = Repository().FindTableBySql(sql2.ToString(), false);
            DataTable dt = dt1.Clone();
            object[] obj = new object[dt.Columns.Count];
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                dt1.Rows[i].ItemArray.CopyTo(obj, 0);
                dt.Rows.Add(obj);
            }
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                dt2.Rows[i].ItemArray.CopyTo(obj, 0);
                dt.Rows.Add(obj);
            }
            return dt;
        }
        #endregion

        #region 2.AVI去向配置信息表格-未配置
        /// <summary>
        /// 点击AVI站点基础信息，查询AVI去向配置信息表
        /// 查询的时候传递了keywords
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable ReGetConfigList(string keywords, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            sql = @"select FunctionId,FunctionCd,FunctionNm,FunctionType,Enable from BBdbR_FunctionBase where Enable=1 and FunctionId not in (select distinct(FunctionId) from BBdbR_PointFunctionConfigBase where Enable=1 and PointId='" + keywords + "')";
            return (Repository().FindTableBySql(sql.ToString(), false));
        }
        #endregion

        #region 2.AVI去向配置信息表格-已配置
        /// <summary>
        /// 点击AVI站点基础信息，查询AVI去向配置信息表
        /// 查询的时候传递了keywords
        /// </summary>
        /// <param name="keywords">点位主键PointId</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable GetConfigList(string keywords, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select a.FunctionId,a.Enable,b.FunctionCd,b.FunctionNm,b.FunctionType from "+tableName+ " a join BBdbR_FunctionBase b on a.FunctionId=b.FunctionId where a.Enable=1 and a.PointId='" + keywords + "'";
                return (Repository().FindTableBySql(sql.ToString(), false));
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 3.配置去向产线
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name="ClassId">查询值</param>
        /// <returns></returns>
        public List<BBdbR_PointFunctionConfigBase> GetClassList(string PointId, string FunctionId) //===复制时需要修改===
        {
            string sql = "";
            if (PointId != "")
            {
                if (FunctionId == "")
                {
                    //根据条件查询
                    sql = @"select * from " + tableName + " where  PointId='" + PointId + "' and Enable=1";
                    return Repository().FindListBySql(sql.ToString());
                }
                else
                {
                    //根据条件查询
                    sql = @"select * from " + tableName + " where  PointId='" + PointId + "' and FunctionId= '" + FunctionId + "' and Enable=1";
                    return Repository().FindListBySql(sql.ToString());
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 4.配置表删除
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(List<BBdbR_PointFunctionConfigBase> ClassConfigList)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<BBdbR_PointFunctionConfigBase> listEntity = new List<BBdbR_PointFunctionConfigBase>(); //===复制时需要修改===
            for (int i = 0; i < ClassConfigList.Count; i++)
            {
                ClassConfigList[i].Enable = "0";//将该实体的IsAvailable属性改为false
                listEntity.Add(ClassConfigList[i]);
            }
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion

        #region 5.新增方法
        //entity实体中的数据是从页面中传来的，它是用户填写的数据
        //entity实体中只有部分字段有值，因为页面中只提供给部分字段赋值
        //将页面中填写的数据以实体（entity）的方式新增到数据库
        //其中，实体中的IsAvailable字段没有在页面中填写
        //IsAvailable字段的作用是做假删除，即数据库中的某一条数据的IsAvailable字段的字段值为true表示该数据存在
        //字段值为false表示该数据被删除
        //在删除数据库中的某一条数据时只要修改IsAvailable字段的字段值为false，并不删除该条数据
        //在新增时将实体的IsAvailable字段的值修改为true
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Insert(string PointId, string FunctionId, string PointCatg) //===复制时需要修改===
        {
            StringBuilder strSql = new StringBuilder();
            int Isok = 0;
            if (FunctionId != "")
            {
                BBdbR_PointFunctionConfigBase Classentity = new BBdbR_PointFunctionConfigBase();
                Classentity.PointCatg = PointCatg;
                Classentity.PointId = PointId;
                Classentity.FunctionId = FunctionId;
                Classentity.Create();
                Repository().Insert(Classentity);
                Isok = 1;
            }
            return Isok;
        }
        #endregion

        #region 1.查找类别下的明细
        /// <summary>
        /// 查找缺陷类别下类别明细数量
        /// </summary>
        /// <returns></returns>
        public int GetConfigCount(string KeyValue) //===复制时需要修改===
        {
            string sql = "";
            if (KeyValue != "")
            {
                sql = @"select * from BBdbR_PointFunctionConfigBase where Enable='1' and FunctionId='" + KeyValue + "'";
                DataTable dt = Repository().FindTableBySql(sql);
                if (dt.Rows.Count > 0)
                {
                    return Repository().FindTableBySql(sql).Rows.Count;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 1;
            }
        }
        #endregion

        #endregion
    }
}
