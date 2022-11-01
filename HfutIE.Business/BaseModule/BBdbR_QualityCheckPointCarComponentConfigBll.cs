//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 工位基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.23 21:51</date>
    /// </author>
    /// </summary>
    public class BBdbR_QualityCheckPointCarComponentConfigBll : RepositoryFactory<BBdbR_QualityCheckPointCarComponentConfig>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_QualityCheckPointCarComponentConfig";//===复制时需要修改===
        string tableName1 = "BBdbR_WcBase";//===复制时需要修改===
        #endregion

        #region 1.获取树，需要修改sql语句
        /// <summary>
        /// 基本信息：【工位】   --属于-->   【产线】  --属于-->  【工段】  --属于-->  【车间】
        /// </summary>
        /// <returns></returns>
        public DataTable GetTreeQuality()
        {
            StringBuilder strSql = new StringBuilder();
            //===复制时需要修改===（三级数）
            //车间节点
            strSql.Append(@"select c.WorkshopId As keys,
                                   c.WorkshopCd As code,
                                   c.WorkshopNm As name,
                                   c.Enabled As IsAvailable,
                                   '0' as parentId,
                                   '0' as sort             
                                   from BBdbR_WorkshopBase c where c.Enabled = '1' and c.WorkshopCd='ZLCJXN01'  ");
            //工段节点
            strSql.Append(@"union select 
                                    b.WorkSectionId AS keys,
                                    b.WorkSectionCd AS code, 
                                    b.WorkSectionNm AS name, 
                                    b.Enabled As IsAvailable,
                                    b.WorkshopId as parentId,
                                    '1' as sort 
                                    from BBdbR_WorkSectionBase b,BBdbR_WorkshopBase c where b.WorkshopId = c.WorkshopId and b.Enabled= '1'  ");
            //产线节点
            strSql.Append(@" union select    
                                    a.PlineId AS keys,
                                    a.PlineCd AS code,
                                    a.PlineNm AS name,
                                    a.Enabled As IsAvailable,
                                    a.WorkSectionId AS parentId,
                                    '2' as sort 
                             from  BBdbR_PlineBase a,BBdbR_WorkSectionBase b 
                             where a.WorkSectionId=b.WorkSectionId and a.Enabled = '1' and a.Enabled = '1' order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.点击树展示表格，需要修改sql语句
        /// <summary>
        /// 基本信息：
        ///     根据传入的参数不同写不同的sql语句进行查询           
        /// </summary>
        /// <param name="areaId">点击的节点的主键</param>
        /// <param name="parentId">节点的父级主键</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable GetListQuality(string areaId, string parentId, string sort, ref JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            DataTable dt = new DataTable();
            var a = string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId);
            if (areaId == "''" && parentId == "''")
            {
                sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName1 + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 and d.WorkshopCd='ZLCJXN01' order by a.sort asc";     //===复制时需要修改===
            }
            else
            {
                if (parentId != "0")
                {
                    if (sort == "0")
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName1 + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + areaId + "' order by a.sort asc";     //===复制时需要修改===
                    }
                    else if (sort == "1")
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName1 + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + areaId + "' order by a.sort asc";     //===复制时需要修改===
                    }
                    else
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName1 + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId where a.Enabled=1 and b.Enabled=1 and a.PlineId='" + areaId + "' order by a.sort asc";     //===复制时需要修改===要修改===
                    }
                }
                else
                {
                    sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName1 + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on c.WorkshopId=d.WorkshopId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 and d.WorkshopId='" + areaId + "' order by a.sort asc";     //===复制时需要修改===
                }
            }
            return Repository().FindTableBySql(sql.ToString(), false);
        }
        #endregion

        #region 3.在首页加载已经配置过的组件信息

        public DataTable GetMatList(string KeyValue, string isCH)//在首页加载已经配置过的组件信息
        {
            string sql = "";
            if (isCH == "CH")
            {
                sql = $"select a.*,c.CarPositionNm,c.CarPositionCd from {tableName} a left join BBdbR_QualityCarPositionBase_Add c on a.CarPositionId=c.CarPositionId where a.Enabled=1 and a.QualityCheckPointId= '{KeyValue}' and c.Type='CH'  ";

            }
            else if (isCH == "TZ")
            {
                sql = $"select a.*,c.CarPositionNm,c.CarPositionCd from {tableName} a left join BBdbR_QualityCarPositionBase_Add c on a.CarPositionId=c.CarPositionId where a.Enabled=1 and a.QualityCheckPointId= '{KeyValue}' and c.Type='TZ' ";
            }
            else
            {
                sql = $"select a.*,c.CarPositionNm,c.CarPositionCd from {tableName} a left join BBdbR_QualityCarPositionBase c on a.CarPositionId=c.CarPositionId where a.Enabled=1 and a.QualityCheckPointId= '{KeyValue}' ";
            }
            return Repository().FindTableBySql(sql, false);
        }
        #endregion

        #region 4.在表单中加载及搜索没配置过的组件清单CarPositionInsert
        public DataTable GetNotConfig(string WcId, string Condition, string keywords, string isCH, ref JqGridParam jqgridparam)//在表单中加载没配置过的组件清单
        {
            StringBuilder strSql = new StringBuilder();
            if (isCH == "CH")
            {
                if (Condition == "all" || Condition == "")
                {
                    strSql.Append(@"select CarPositionId,CarPositionCd,CarPositionNm,Enabled,Rem from BBdbR_QualityCarPositionBase_Add where CarPositionId not in(select distinct CarPositionId from " + tableName + " where Enabled=1 and QualityCheckPointId='" + WcId + "' ) and Enabled=1  and Type='CH'" );
                }
                else
                {
                    strSql.Append(@"select CarPositionId,CarPositionCd,CarPositionNm,Enabled,Rem from BBdbR_QualityCarPositionBase_Add where CarPositionId not in(select distinct CarPositionId from " + tableName + " where Enabled=1 and QualityCheckPointId='" + WcId + "' ) and " + Condition + " like '%" + keywords + "%' and Enabled=1  and Type='CH'");
                }
            }
            else if(isCH == "TZ")
            {
                if (Condition == "all" || Condition == "")
                {
                    strSql.Append(@"select CarPositionId,CarPositionCd,CarPositionNm,Enabled,Rem from BBdbR_QualityCarPositionBase_Add where CarPositionId not in(select distinct CarPositionId from " + tableName + " where Enabled=1 and QualityCheckPointId='" + WcId + "' ) and Enabled=1  and Type='TZ'");
                }
                else
                {
                    strSql.Append(@"select CarPositionId,CarPositionCd,CarPositionNm,Enabled,Rem from BBdbR_QualityCarPositionBase_Add where CarPositionId not in(select distinct CarPositionId from " + tableName + " where Enabled=1 and QualityCheckPointId='" + WcId + "' ) and " + Condition + " like '%" + keywords + "%' and Enabled=1  and Type='TZ'");
                }
            }
            else
            {
                if (Condition == "all" || Condition == "")
                {
                    strSql.Append(@"select CarPositionId,CarPositionCd,CarPositionNm,Enabled,Rem from BBdbR_QualityCarPositionBase where CarPositionId not in(select distinct CarPositionId from " + tableName + " where Enabled=1 and QualityCheckPointId='" + WcId + "' ) and Enabled=1");
                }
                else
                {
                    strSql.Append(@"select CarPositionId,CarPositionCd,CarPositionNm,Enabled,Rem from BBdbR_QualityCarPositionBase where CarPositionId not in(select distinct CarPositionId from " + tableName + " where Enabled=1 and QualityCheckPointId='" + WcId + "' ) and " + Condition + " like '%" + keywords + "%' and Enabled=1");
                }
            }
            return Repository().FindTableBySql(strSql.ToString(), false);
        }


        #endregion

        #region 5.提交配置
        public int CarPositionInsert(BBdbR_QualityCheckPointCarComponentConfig entity)
        {
            return Repository().Insert(entity);
            //return 1;
        }
        #endregion

        #region 6.删除配置
        public int ConfigDelete(string KeyValue)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"update "+tableName+ " set Enabled=0 where ConfigId='" + KeyValue + "'");
            int isok = Repository().ExecuteBySql(sql);
            return isok;
        }

        #endregion

        #region 7.查询方法，需要修改sql语句
        /// <summary>
        ///     查询时提供了两个关键字，一个是Condition，另一个是keywords
        ///     
        ///     Condition是关键字，即查询条件，对应数据库中的一个字段
        ///     keywords是查询值，即查询条件的具体值，对应数据库中查询条件字段的值
        ///     查询的时候传递了Condition和keywords
        /// 
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="Condition">关键字（查询条件）</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns>查询的数据（列表）</returns>
        public DataTable GetPageListByConditionQuality(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (Condition == "all")
            {
                sql = @"select * from " + tableName1 + " where Enabled = 1 order by sort asc";
                return Repository().FindTableBySql(sql,false);
            }
            else
            {
                if (keywords != "all")
                {
                    //根据条件查询
                    sql = @"select * from " + tableName1 + " where  " + Condition + " like  '%" + keywords + "%' and Enabled = 1 order by sort asc";
                    return Repository().FindTableBySql(sql.ToString(), false);
                }
                else
                {
                    sql = @"select * from " + tableName1 + " where Enabled = 1 order by sort asc";
                    return Repository().FindTableBySql(sql.ToString(), false);
                }
            }
        }
        #endregion
    }
}