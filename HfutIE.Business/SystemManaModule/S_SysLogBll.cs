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
    /// 操作日志表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.04.03 09:49</date>
    /// </author>
    /// </summary>
    public class S_SysLogBll : RepositoryFactory<S_SysLog>
    {
        #region
        #endregion

        #region 搜索日志列表
        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="StartTime">制单开始时间</param>
        /// <param name="EndTime">制单结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable GetLogList(string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM S_SysLog WHERE 1=1 ");
            //制单开始
            if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
            {

                parameter.Add(DbFactory.CreateDbParameter("@StartTime", CommonHelper.GetDateTime(StartTime + " 00:00")));
                parameter.Add(DbFactory.CreateDbParameter("@EndTime", CommonHelper.GetDateTime(EndTime + " 23:59")));
                strSql.Append(" AND CreTm Between '"+ StartTime + "' AND '" + EndTime + "' ");
                //strSql.Append("AND ORDER BY SequenceNum ASC ");
            }
            return Repository().FindTableBySql(strSql.ToString());
        }

        public DataTable SelectLogIn(string condition, string keywords, string ParameterJson, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT SysLogId,ObjectId,LogType,ModuleId,Remark,CreTm,CreCd,CreNm,MdfTm,MdfCd,MdfNm,Status,IPAddress,IPAddressName FROM S_SysLog where 1=1 ");
            switch (condition)
            {
                case "LogType":
                    strSql.Append(" and LogType LIKE  '%" + keywords + "%'");
                    break;
                default:
                    break;
            }
            return Repository().FindTableBySql(strSql.ToString());
        }

        #endregion

        #region 获取日志
        /// <summary>
        /// 获取系统日志列表
        /// </summary>
        /// <param name="ModuleId">模块ID</param>
        /// <param name="ParameterJson">搜索条件</param>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public DataTable GetPageList(string ModuleId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  
                                                l.SysLogId ,
                                                l.ObjectId ,
                                                l.LogType ,
                                                l.IPAddress ,
                                                l.IPAddressName ,                                                
                                                l.CreTm ,
                                                l.CreCd ,
                                                l.CreNm ,
                                                m.FullName AS ModuleName ,
                                                m.ModuleId,
                                                l.Remark ,
                                                l.Status
                                      FROM      S_SysLog l                                                                                              
                                                LEFT JOIN Base_Module m ON m.ModuleId = '" + ModuleId + "'");
            strSql.Append("   WHERE 1 = 1 ");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 获取日志明细
        /// <summary>
        /// 获取系统日志明细列表
        /// </summary>
        /// <param name="SysLogId">系统日志主键</param>
        /// <returns></returns>
        public DataTable GetSysLogDetailList(string SysLogId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from S_SysLogDetail where SysLogId = '" + SysLogId + "' Order By CreTm ASC");
            return Repository().FindTableBySql(strSql.ToString());

        }
        #endregion

        #region 清空日志
        /// <summary>
        /// 清空操作日志
        /// </summary>
        /// <param name="CreateDate"></param>
        /// <returns></returns>
        public int RemoveLog(string KeepTime)
        {
            StringBuilder strSql = new StringBuilder();
            DateTime CreateDate = DateTime.Now;
            if (KeepTime == "7")//保留近一周
            {
                CreateDate = DateTime.Now.AddDays(-7);
            }
            else if (KeepTime == "1")//保留近一个月
            {
                CreateDate = DateTime.Now.AddMonths(-1);
            }
            else if (KeepTime == "3")//保留近三个月
            {
                CreateDate = DateTime.Now.AddMonths(-3);
            }
            if (KeepTime == "0")//不保留，全部删除
            {
                strSql.Append("DELETE FROM S_SysLog");
                return DataFactory.Database().ExecuteBySql(strSql);
            }
            else
            {
                strSql.Append("DELETE FROM S_SysLog WHERE 1=1 ");
                strSql.Append("AND CreTm <= '"+ CreateDate + "'");                
                return DataFactory.Database().ExecuteBySql(strSql);
            }
        }
        #endregion

    }
}