//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Mvc;

namespace HfutIE.Business
{
    /// <summary>
    /// AVI去向配置信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.31 12:49</date>
    /// </author>
    /// </summary>
    public class BBdbR_AVIActionConfigBll : RepositoryFactory<BBdbR_AVIActionConfig>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_AVIActionConfig";//===复制时需要修改===
        //BBdbR_AVIAcitonConfig
        #endregion

        #region 方法区

        #region 1.AVI去向配置信息表格-未配置
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
            sql = @"select * from BBdbR_PlineBase where Enabled=1 and PlineId not in (select distinct(PlineId) from BBdbR_AVIWhereaboutsConfig where Enabled=1 and AviId='" + keywords + "')";
            return (Repository().FindTableBySql(sql.ToString(), false));
        }
        #endregion

        #region 2.AVI去向配置信息表格-已配置
        /// <summary>
        /// 点击AVI站点基础信息，查询AVI去向配置信息表
        /// 查询的时候传递了keywords
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable GetConfigList(string keywords, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select a.* from BBdbR_PlineBase a join " + tableName + " b on a.PlineId=b.PlineId where a.Enabled=1 and b.Enabled=1 and b.AviId='" + keywords + "'";
                return (Repository().FindTableBySql(sql.ToString(), false));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询车身对应的产线
        /// 查询的时候传递了keywords
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable GetPageConfigList(string keywords, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select * from " + tableName + " where Enabled=1 and AviCd='" + keywords + "'";
                //sql = @"select a.*,b.AVIWhereId as AVIWhereId,b.PlineMark as PlineMark,c.AviCd as AviCd,c.AviNm as AviNm from BBdbR_PlineBase a join " + tableName + " b on a.PlineId=b.PlineId join BBdbR_AVIBase c on b.AviId=c.AviId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and b.AviId='" + keywords + "'";
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
        //public List<BBdbR_AVIWhereaboutsConfig> GetClassList(string AVIId, string PlineId) //===复制时需要修改===
        //{
        //    string sql = "";
        //    if (AVIId != "")
        //    {
        //        if (PlineId == "")
        //        {
        //            //根据条件查询
        //            sql = @"select * from " + tableName + " where  AVIId='" + AVIId + "' and Enabled=1";
        //            return Repository().FindListBySql(sql.ToString());
        //        }
        //        else
        //        {
        //            //根据条件查询
        //            sql = @"select * from " + tableName + " where  AVIId='" + AVIId + "' and PlineId= '" + PlineId + "' and Enabled=1";
        //            return Repository().FindListBySql(sql.ToString());
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        #endregion

        #region 4.删除
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(List<BBdbR_AVIActionConfig> ClassConfigList)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<BBdbR_AVIActionConfig> listEntity = new List<BBdbR_AVIActionConfig>(); //===复制时需要修改===
            for (int i = 0; i < ClassConfigList.Count; i++)
            {
                ClassConfigList[i].Enabled = "0";//将该实体的IsAvailable属性改为false
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
        public int Insert(BBdbR_AVIActionConfig entity) //===复制时需要修改===
        {
            //StringBuilder strSql = new StringBuilder();
            //int Isok = 0;
           
                //BBdbR_AVIAcitonConfig Classentity = new BBdbR_AVIAcitonConfig();
                //Classentity.AviCd = AviCd;
                //Classentity.PlineId = AviCd;
                //Classentity.Create();
                
                //Isok = 1;
            
            return Repository().Insert(entity);
        }
        #endregion

        #region 6.AVI与产线配置
        /// <summary>
        /// 点击AVI站点基础信息，查询AVI去向配置信息表
        /// 查询的时候传递了keywords
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public BBdbR_AVIActionConfig SetConfigInfor(string AviActionConfigId) //===复制时需要修改===
        {
            string sql = "";
            BBdbR_AVIActionConfig entity = new BBdbR_AVIActionConfig();
            sql = @"select a.*,b.AviNm as AviNm from " + tableName + " a join BBdbR_AviBase b on a.AviCd=b.AviCd where a.Enabled=1 and a.AviActionConfigId='" + AviActionConfigId + "'";
            DataTable dt = Repository().FindTableBySql(sql.ToString(), false);
            if (dt.Rows.Count>0)
            {
                entity.AviActionConfigId = dt.Rows[0]["AviActionConfigId"].ToString();
                entity.AviCd = dt.Rows[0]["AviCd"].ToString();
                entity.AviCatg = dt.Rows[0]["AviCatg"].ToString();
                entity.AviAction = dt.Rows[0]["AviAction"].ToString();
                entity.AviAddr = dt.Rows[0]["AviAddr"].ToString();
            }
            return entity;
        }

        #endregion

        #region 7.编辑方法
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(BBdbR_AVIActionConfig entity) //===复制时需要修改===
        {
            return Repository().Update(entity); //将修改后的实体跟新到数据库中
        }
        #endregion

        #endregion
    }
}