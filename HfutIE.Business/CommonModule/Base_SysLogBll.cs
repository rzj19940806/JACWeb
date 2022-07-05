//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2014
// Software Developers @ HfutIE 2014
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
using System.Reflection;
using System.Text;
using System.Threading;

namespace HfutIE.Business
{
    /// <summary>
    /// 系统日志表
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.07.22 22:43</date>
    /// </author>
    /// </summary>
    public class Base_SysLogBll : RepositoryFactory<Base_SysLog>
    {
        #region 静态实例化
        private static Base_SysLogBll item;
        public static Base_SysLogBll Instance
        {
            get
            {
                //if (item == null)
                //{
                //    item = new Base_SysLogBll();
                //}
                return new Base_SysLogBll();
            }
        }
        #endregion

        public Base_SysLog SysLog = new Base_SysLog();

        #region 写入操作日志
        /// <summary>
        /// 写入作业日志
        /// </summary>
        /// <param name="ObjectId">对象主键</param>
        /// <param name="OperationType">操作类型</param>
        /// <param name="Status">状态</param>
        /// <param name="Remark">操作说明</param>
        /// <returns></returns>
        public void WriteLog(string ObjectId, OperationType OperationType, string Status, string Remark = "")
        {
            if (ConfigHelper.AppSettings("IsLog") == "true")
            {

                SysLog.SysLogId = CommonHelper.GetGuid;
                SysLog.ObjectId = ObjectId;
                SysLog.LogType = CommonHelper.GetString((int)OperationType);
                if (ManageProvider.Provider.IsOverdue())
                {
                    SysLog.IPAddress = ManageProvider.Provider.Current().IPAddress;
                    SysLog.IPAddressName = ManageProvider.Provider.Current().IPAddressName;
                    SysLog.CompanyId = ManageProvider.Provider.Current().CompanyId;
                    SysLog.DepartmentId = ManageProvider.Provider.Current().DepartmentId;
                    SysLog.CreateUserId = ManageProvider.Provider.Current().UserId;
                    SysLog.CreateUserName = ManageProvider.Provider.Current().UserName;
                }
                SysLog.ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
                SysLog.Remark = Remark;
                SysLog.Status = Status;
                ThreadPool.QueueUserWorkItem(new WaitCallback(WriteLogUsu), SysLog);//放入异步执行
            }
        }
        private void WriteLogUsu(object obSysLog)
        {
            Base_SysLog VSysLog = (Base_SysLog)obSysLog;
            DataFactory.Database().Insert(VSysLog);
        }
        /// <summary>
        /// 写入作业日志（新增操作）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="OperationType">操作类型</param>
        /// <param name="Status">状态</param>
        /// <param name="Remark">操作说明</param>
        /// <returns></returns>
        public void WriteLog<T>(T entity, OperationType OperationType, string Status, string Remark = "")
        {
            if (ConfigHelper.AppSettings("IsLog") == "true")
            {
                IDatabase database = DataFactory.Database();
                DbTransaction isOpenTrans = database.BeginTrans();
                try
                {
                    SysLog.SysLogId = CommonHelper.GetGuid;
                    SysLog.ObjectId = DatabaseCommon.GetKeyFieldValue(entity).ToString();
                    SysLog.LogType = CommonHelper.GetString((int)OperationType);
                    SysLog.IPAddress = ManageProvider.Provider.Current().IPAddress;
                    SysLog.IPAddressName = ManageProvider.Provider.Current().IPAddressName;
                    SysLog.CompanyId = ManageProvider.Provider.Current().CompanyId;
                    SysLog.DepartmentId = ManageProvider.Provider.Current().DepartmentId;
                    SysLog.CreateUserId = ManageProvider.Provider.Current().UserId;
                    SysLog.CreateUserName = ManageProvider.Provider.Current().UserName;
                    SysLog.ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
                    if (Remark == "")
                    {
                        SysLog.Remark = DatabaseCommon.GetClassName<T>();
                    }
                    SysLog.Remark = Remark;
                    SysLog.Status = Status;
                    database.Insert(SysLog, isOpenTrans);
                    //添加日志详细信息
                    Type objTye = typeof(T);
                    foreach (PropertyInfo pi in objTye.GetProperties())
                    {
                        object value = pi.GetValue(entity, null);
                        if (value != null && value.ToString() != "&nbsp;" && value.ToString() != "")
                        {

                            Base_SysLogDetail syslogdetail = new Base_SysLogDetail();
                            syslogdetail.SysLogDetailId = CommonHelper.GetGuid;
                            syslogdetail.SysLogId = SysLog.SysLogId;
                            syslogdetail.PropertyField = pi.Name;
                            syslogdetail.PropertyName = DatabaseCommon.GetFieldText(pi);
                            syslogdetail.NewValue = "" + value + "";
                            database.Insert(syslogdetail, isOpenTrans);
                        }
                    }
                    database.Commit();
                }
                catch
                {
                    database.Rollback();
                }
            }
        }
        /// <summary>
        /// 写入作业日志（更新操作）
        /// </summary>
        /// <param name="oldObj">旧实体对象</param>
        /// <param name="newObj">新实体对象</param>
        /// <param name="OperationType">操作类型</param>
        /// <param name="Status">状态</param>
        /// <param name="Remark">操作说明</param>
        /// <returns></returns>
        public void WriteLog<T>(T oldObj, T newObj, OperationType OperationType, string Status, string Remark = "")
        {
            if (ConfigHelper.AppSettings("IsLog") == "true")
            {
                IDatabase database = DataFactory.Database();
                DbTransaction isOpenTrans = database.BeginTrans();
                try
                {
                    SysLog.SysLogId = CommonHelper.GetGuid;
                    SysLog.ObjectId = DatabaseCommon.GetKeyFieldValue(newObj).ToString();
                    SysLog.LogType = CommonHelper.GetString((int)OperationType);
                    SysLog.IPAddress = ManageProvider.Provider.Current().IPAddress;
                    SysLog.IPAddressName = ManageProvider.Provider.Current().IPAddressName;
                    SysLog.CompanyId = ManageProvider.Provider.Current().CompanyId;
                    SysLog.DepartmentId = ManageProvider.Provider.Current().DepartmentId;
                    SysLog.CreateUserId = ManageProvider.Provider.Current().UserId;
                    SysLog.CreateUserName = ManageProvider.Provider.Current().UserName;
                    SysLog.ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
                    if (Remark == "")
                    {
                        SysLog.Remark = DatabaseCommon.GetClassName<T>();
                    }
                    SysLog.Remark = Remark;
                    SysLog.Status = Status;
                    database.Insert(SysLog, isOpenTrans);
                    //添加日志详细信息
                    Type objTye = typeof(T);
                    foreach (PropertyInfo pi in objTye.GetProperties())
                    {
                        object oldVal = pi.GetValue(oldObj, null);
                        object newVal = pi.GetValue(newObj, null);
                        if (!Equals(oldVal, newVal))
                        {
                            if (oldVal != null && oldVal.ToString() != "&nbsp;" && oldVal.ToString() != "" && newVal != null && newVal.ToString() != "&nbsp;" && newVal.ToString() != "")
                            {
                                Base_SysLogDetail syslogdetail = new Base_SysLogDetail();
                                syslogdetail.SysLogDetailId = CommonHelper.GetGuid;
                                syslogdetail.SysLogId = SysLog.SysLogId;
                                syslogdetail.PropertyField = pi.Name;
                                syslogdetail.PropertyName = DatabaseCommon.GetFieldText(pi);
                                syslogdetail.NewValue = "" + newVal + "";
                                syslogdetail.OldValue = "" + oldVal + "";
                                database.Insert(syslogdetail, isOpenTrans);
                            }
                        }
                    }
                    database.Commit();
                }
                catch
                {
                    database.Rollback();
                }
            }
        }
        /// <summary>
        /// 写入作业日志（删除操作）
        /// </summary>
        /// <param name="oldObj">旧实体对象</param>
        /// <param name="KeyValue">对象主键</param>
        /// <param name="Status">状态</param>
        /// <param name="Remark">操作说明</param>
        public void WriteLog<T>(string[] KeyValue, string Status, string Remark = "") where T : new()
        {
            if (ConfigHelper.AppSettings("IsLog") == "true")
            {
                IDatabase database = DataFactory.Database();
                DbTransaction isOpenTrans = database.BeginTrans();
                try
                {
                    foreach (var item in KeyValue)
                    {
                        T Oldentity = database.FindEntity<T>(item.ToString());
                        SysLog.SysLogId = CommonHelper.GetGuid;
                        SysLog.ObjectId = item;
                        SysLog.LogType = CommonHelper.GetString((int)OperationType.Delete);
                        SysLog.IPAddress = ManageProvider.Provider.Current().IPAddress;
                        SysLog.IPAddressName = ManageProvider.Provider.Current().IPAddressName;
                        SysLog.CompanyId = ManageProvider.Provider.Current().CompanyId;
                        SysLog.DepartmentId = ManageProvider.Provider.Current().DepartmentId;
                        SysLog.CreateUserId = ManageProvider.Provider.Current().UserId;
                        SysLog.CreateUserName = ManageProvider.Provider.Current().UserName;
                        SysLog.ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
                        if (Remark == "")
                        {
                            SysLog.Remark = DatabaseCommon.GetClassName<T>();
                        }
                        SysLog.Remark = Remark;
                        SysLog.Status = Status;
                        database.Insert(SysLog, isOpenTrans);
                        //添加日志详细信息
                        Type objTye = typeof(T);
                        foreach (PropertyInfo pi in objTye.GetProperties())
                        {
                            object value = pi.GetValue(Oldentity, null);
                            if (value != null && value.ToString() != "&nbsp;" && value.ToString() != "")
                            {
                                Base_SysLogDetail syslogdetail = new Base_SysLogDetail();
                                syslogdetail.SysLogDetailId = CommonHelper.GetGuid;
                                syslogdetail.SysLogId = SysLog.SysLogId;
                                syslogdetail.PropertyField = pi.Name;
                                syslogdetail.PropertyName = DatabaseCommon.GetFieldText(pi);
                                syslogdetail.NewValue = "" + value + "";
                                database.Insert(syslogdetail, isOpenTrans);
                            }
                        }
                    }
                    database.Commit();
                }
                catch
                {
                    database.Rollback();
                }
            }
        }
        #endregion


        /// <summary>
        /// 清空操作日志
        /// </summary>
        /// <param name="CreateDate"></param>
        /// <returns></returns>
        public int RemoveLog(string KeepTime)
        {
            StringBuilder strSql = new StringBuilder();
            DateTime CreateDate = DateTime.Now.AddMonths(-(Convert.ToInt32(KeepTime)));
            
            if (KeepTime == "0")//不保留，全部删除
            {
                strSql.Append("DELETE FROM Base_SysLog WHERE LogType != '0'");
                return DataFactory.Database().ExecuteBySql(strSql);
            }
            else
            {
                strSql.Append("DELETE FROM Base_SysLog WHERE LogType != '0' ");
                strSql.Append("AND CreateDate <= @CreateDate");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@CreateDate", CreateDate));
                return DataFactory.Database().ExecuteBySql(strSql, parameter.ToArray());
            }
        }
        /// <summary>
        /// 获取系统日志列表——系统日志
        /// </summary>
        /// <param name="ModuleId">模块ID</param>
        /// <param name="ParameterJson">搜索条件</param>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public DataTable GetPageList(string ModuleId,string LogType, string CreateUserName, string IPAddress, string StartTime, string EndTime, string ParameterJson, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM    (SELECT    l.SysLogId ,
                                                l.ObjectId ,
                                                l.LogType ,
                                                l.IPAddress ,
                                                l.IPAddressName ,
                                                c.FullName AS CompanyId ,
                                                D.FullName AS DepartmentId ,
                                                l.CreateDate ,
                                                l.CreateUserId ,
                                                l.CreateUserName ,
                                                m.FullName AS ModuleName ,
                                                m.ModuleId,
                                                l.Remark,
                                                l.Status,
                                                ld.SysLogDetailId,
												ld.PropertyField,
												ld.PropertyName,
												ld.NewValue,
												ld.OldValue

                                      FROM      Base_SysLog l
                                                LEFT JOIN Base_Department d ON d.DepartmentId = l.DepartmentId
                                                LEFT JOIN Base_Company c ON c.CompanyId = l.CompanyId
                                                LEFT JOIN Base_Module m ON m.ModuleId = l.ModuleId
                                                LEFT JOIN Base_SysLogDetail ld ON ld.SysLogId = l.SysLogId

                                    ) A WHERE LogType != '0' ");
            if (!string.IsNullOrEmpty(ModuleId))
            {
                strSql.Append(" AND ModuleId = @ModuleId");
                parameter.Add(DbFactory.CreateDbParameter("@ModuleId", ModuleId));
            }
            if (!string.IsNullOrEmpty(LogType))
            {
                strSql.Append(" AND LogType = @LogType");
                parameter.Add(DbFactory.CreateDbParameter("@LogType", LogType));
            }
            if (!string.IsNullOrEmpty(CreateUserName))
            {
                strSql.Append(" AND CreateUserName LIKE @CreateUserName");
                parameter.Add(DbFactory.CreateDbParameter("@CreateUserName", "%" + CreateUserName + "%"));
            }
            if (!string.IsNullOrEmpty(IPAddress))
            {
                strSql.Append(" AND IPAddress LIKE @IPAddress");
                parameter.Add(DbFactory.CreateDbParameter("@IPAddress", "%" + IPAddress + "%"));
            }
            if (!string.IsNullOrEmpty(StartTime))
            {
                parameter.Add(DbFactory.CreateDbParameter("@StartTime", CommonHelper.GetDateTime(StartTime)));
                strSql.Append(" AND DATEDIFF(ss, @StartTime,A.CreateDate)>=0 ");
            }
            if (!string.IsNullOrEmpty(EndTime))
            {

                parameter.Add(DbFactory.CreateDbParameter("@EndTime", CommonHelper.GetDateTime(EndTime)));
                strSql.Append(" AND DATEDIFF(ss, @EndTime,A.CreateDate)<=0 ");
            }
            //开始时间
            //if (!string.IsNullOrEmpty(StartTime))
            //{
            //    //strSql.Append(" and DateDiff(dd,AcqTm,getdate()) <= DateDiff(dd,'" + TimeStart + "',getdate()) ");
            //    //开始时间把@放在前面
            //    strSql.Append(" and DateDiff(dd,@StartTime,CreateDate) >=0 ");
            //    parameter.Add(DbFactory.CreateDbParameter("@StartTime", StartTime));
            //}
            ////结束时间
            //if (!string.IsNullOrEmpty(EndTime))
            //{
            //    //strSql.Append(" and DateDiff(dd,AcqTm,getdate()) >= DateDiff(dd,'" + TimeEnd + "',getdate()) ");
            //    //结束时间把@放在后面
            //    strSql.Append(" and DateDiff(dd,CreateDate,@EndTime) >=0 ");
            //    parameter.Add(DbFactory.CreateDbParameter("@EndTime", EndTime));
            //}

            if (!string.IsNullOrEmpty(ParameterJson))
            {
                List<DbParameter> outparameter = new List<DbParameter>();
                strSql.Append(ConditionBuilder.GetWhereSql(ParameterJson.JonsToList<Condition>(), out outparameter));
                parameter.AddRange(outparameter);
            }
            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        /// <summary>
        /// 获取系统日志明细列表
        /// </summary>
        /// <param name="SysLogId">系统日志主键</param>
        /// <returns></returns>
        public List<Base_SysLogDetail> GetSysLogDetailList(string SysLogId)
        {
            string WhereSql = " AND SysLogId = @SysLogId Order By CreateDate ASC";
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@SysLogId", SysLogId));
            return DataFactory.Database().FindList<Base_SysLogDetail>(WhereSql, parameter.ToArray());
        }

        #region 获取登录日志
        //public DataTable GetLoginList( ref JqGridParam jqgridparam)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    List<DbParameter> parameter = new List<DbParameter>();
        //    strSql.Append(@"SELECT  *
        //                    FROM    (SELECT    l.SysLogId ,
        //                                        l.ObjectId ,
        //                                        l.LogType ,
        //                                        l.IPAddress ,
        //                                        l.IPAddressName ,
        //                                        c.FullName AS CompanyId ,
        //                                        D.FullName AS DepartmentId ,
        //                                        l.CreateDate ,
        //                                        l.CreateUserId ,
        //                                        l.CreateUserName ,
        //                                        m.FullName AS ModuleName ,
        //                                        m.ModuleId,
        //                                        l.Remark ,
        //                                        l.Status
        //                              FROM      Base_SysLog l
        //                                        LEFT JOIN Base_Department d ON d.DepartmentId = l.DepartmentId
        //                                        LEFT JOIN Base_Company c ON c.CompanyId = l.CompanyId
        //                                        LEFT JOIN Base_Module m ON m.ModuleId = l.ModuleId
        //                            ) A WHERE LogType = '0'");
            
        //    return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        //}
        /// <summary>
        /// 根据查询条件搜索——登录日志
        /// </summary>
        /// <param name="StartTime">制单开始时间</param>
        /// <param name="EndTime">制单结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable GetLogList(string CreateUserName, string IPAddress, string StartTime, string EndTime, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM Base_SysLog WHERE LogType = '0' ");

            if (!string.IsNullOrEmpty(CreateUserName))
            {
                strSql.Append(" AND CreateUserName LIKE @CreateUserName");
                parameter.Add(DbFactory.CreateDbParameter("@CreateUserName", "%" + CreateUserName + "%"));
            }
            if (!string.IsNullOrEmpty(IPAddress))
            {
                strSql.Append(" AND IPAddress LIKE @IPAddress");
                parameter.Add(DbFactory.CreateDbParameter("@IPAddress", "%" + IPAddress + "%"));
            }
            
            if (!string.IsNullOrEmpty(StartTime))
            {
                parameter.Add(DbFactory.CreateDbParameter("@StartTime", CommonHelper.GetDateTime(StartTime)));
                strSql.Append(" AND DATEDIFF(ss, @StartTime,CreateDate)>=0 ");
            }
            if (!string.IsNullOrEmpty(EndTime))
            {

                parameter.Add(DbFactory.CreateDbParameter("@EndTime", CommonHelper.GetDateTime(EndTime)));
                strSql.Append(" AND DATEDIFF(ss, @EndTime,CreateDate)<=0 ");
            }

            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
            //return  DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
        }
        public int RemoveLoginLog(string KeepTime)
        {
            StringBuilder strSql = new StringBuilder();
            DateTime CreateDate = DateTime.Now.AddMonths(-(Convert.ToInt32(KeepTime)));
            
            if (KeepTime == "0")//不保留，全部删除
            {
                strSql.Append("DELETE FROM Base_SysLog WHERE LogType = '0' ");
                return DataFactory.Database().ExecuteBySql(strSql);
            }
            else
            {
                strSql.Append("DELETE FROM Base_SysLog WHERE LogType = '0' ");
                strSql.Append("AND CreateDate <= @CreateDate");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@CreateDate", CreateDate));
                return DataFactory.Database().ExecuteBySql(strSql, parameter.ToArray());
            }
        }
        #endregion


    }
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 登陆
        /// </summary>
        Login = 0,
        /// <summary>
        /// 新增
        /// </summary>
        Add = 1,
        /// <summary>
        /// 修改
        /// </summary>
        Update = 2,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 3,
        /// <summary>
        /// 其他
        /// </summary>
        Other = 4,
        /// <summary>
        /// 访问
        /// </summary>
        Visit = 5,
        /// <summary>
        /// 离开
        /// </summary>
        Leave = 6,
        /// <summary>
        /// 查询
        /// </summary>
        Query = 7,
        /// <summary>
        /// 安全退出
        /// </summary>
        Exit = 8,
    }
}