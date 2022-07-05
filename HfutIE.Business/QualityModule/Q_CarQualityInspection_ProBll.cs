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

namespace HfutIE.Business
{
    /// <summary>
    /// Andon数据采集过程表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.11 15:17</date>
    /// </author>
    /// </summary>
    public class Q_CarQualityInspection_ProBll : RepositoryFactory<Q_CarQualityInspection_Pro>
    {
        #region 加载时或根据关键词查询车身基本信息        
        public Q_CarQualityInspection_Pro GetCarInforbykeywords(string keywords)
        {
            StringBuilder strSql = new StringBuilder();
            //string sql = "";
            DataTable dt = new DataTable();
            //sql = @"select VIN,SequenceNo,CarPartNm,CarColor1,CarType from Q_CarQualityInspection_Pro where VIN ='" + keywords + "' and Enabled=1";
            //sql = @"select VIN,SequenceNo,CarPartNm,CarColor1,CarType from Q_CarQualityInspection_Pro where VIN =" + keywords + " and Enabled=1";
            strSql.Append(@"select * from Q_CarQualityInspection_Pro where VIN = '" + keywords + "' and Enabled='1';");
            //BBdbR_DataAcqPro dt = Repository().FindEntityBySql(strSql.ToString());
            //dt = Repository().FindTableBySql(sql.ToString(), false);
            return Repository().FindEntityBySql(strSql.ToString()); ;
        }
        #endregion

        #region 查询质控点名称        
        public DataTable GetWcName()
        {
            StringBuilder strSql = new StringBuilder();           
            DataTable dt = new DataTable();
            //strSql.Append(@"select WcNm  from Q_CarQualityInspection_Pro where WcId = '" + KeyValue + "' and Enabled='1';");
            strSql.Append(@"select WcNm  from Q_CarQualityInspection_Pro where Enabled='1';");
            return Repository().FindTableBySql(strSql.ToString());          
        }
        #endregion
        
        #region 查找计划
        /// <summary>
        /// 查找计划
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable GetTodayPlan()
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            today = "2021-09-07";
            strSql.Append(@"select sum(case when  PlanStatus=2 and Enabled='1' and RecieveTm='"+ today+ "'  then 1 else 0 end) as dayActual,sum(case when  Enabled='1' and RecieveTm='" + today+ "' then 1 else 0 end) as dayPlan from P_ProducePlan_Pro;");
            dt = Repository().FindTableBySql(strSql.ToString(), false);
            return dt;
        }
        #endregion

        #region 根据结果表主键删除主键下的所有缺陷记录
        public int deleteByResult(string CarQualityResultId) //===复制时需要修改===
        {
            string sql = "";
            List<Q_CarQualityInspection_Pro> dtList;
            sql = @"select * from Q_CarQualityInspection_Pro where Enabled = '1' and CarQualityResultId = '" + CarQualityResultId + "' ";
            dtList = Repository().FindListBySql(sql.ToString());
            foreach (var item in dtList)
            {
                item.Enabled = "0";
                //删除
            }
            int num = Repository().Update(dtList);
            return num;//返回结果
        }
        #endregion

        #region 根据结果表主键删除主键下的所有缺陷记录
        public int delete(string CarQualityInspectionId) //===复制时需要修改===
        {
            string sql = "";
            List<Q_CarQualityInspection_Pro> dtList;
            sql = @"select * from Q_CarQualityInspection_Pro where Enabled = '1' and CarQualityInspectionId = '" + CarQualityInspectionId + "' ";
            dtList = Repository().FindListBySql(sql.ToString());
            foreach (var item in dtList)
            {
                item.Enabled = "0";
                //删除
            }
            int num = Repository().Update(dtList);
            return num;//返回结果
        }
        #endregion

        #region
        /// <summary>
        /// 查找班组
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable GetTodayClass()
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            today = "2021-09-07";
            strSql.Append(@"select sum(case when  PlanStatus=2 and Enabled='1' and RecieveTm='" + today + "'  then 1 else 0 end) as dayActual,sum(case when  Enabled='1' and RecieveTm='" + today + "' then 1 else 0 end) as dayPlan from P_ProducePlan_Pro;");
            dt = Repository().FindTableBySql(strSql.ToString(), false);
            return dt;
        }
        #endregion



    }
}