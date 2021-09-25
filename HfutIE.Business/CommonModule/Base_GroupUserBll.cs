//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2014
// Software Developers @ HfutIE 2014
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 用户组管理
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.07 15:36</date>
    /// </author>
    /// </summary>
    public class Base_GroupUserBll : RepositoryFactory<Base_GroupUser>
    {
        /// <summary>
        /// 获取用户组列表
        /// </summary>
        /// <param name="CompanyId">公司ID</param>
        /// <param name="DepartmentId">部门ID</param>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public DataTable GetPageList(string CompanyId, string DepartmentId, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    gu.GroupUserId ,              
                                                gu.Code ,                     
                                                gu.FullName ,                 
                                                gu.DepartmentId ,            
                                                dep.FullName AS DepartmentName ,
                                                gu.CompanyId ,                
                                                cpy.FullName AS CompanyName , 
                                                gu.Enabled ,                  
                                                gu.Remark ,                  
                                                gu.SortCode                   
                                      FROM      Base_GroupUser gu
                                                LEFT JOIN Base_Department dep ON dep.DepartmentId = gu.DepartmentId
                                                LEFT JOIN Base_Company cpy ON cpy.CompanyId = gu.CompanyId
                                    ) T WHERE   1 = 1 ");
            if (!string.IsNullOrEmpty(CompanyId))
            {
                strSql.Append(" AND CompanyId = @CompanyId");
                parameter.Add(DbFactory.CreateDbParameter("@CompanyId", CompanyId));
            }
            if (!string.IsNullOrEmpty(DepartmentId))
            {
                strSql.Append(" AND DepartmentId = @DepartmentId");
                parameter.Add(DbFactory.CreateDbParameter("@DepartmentId", DepartmentId));
            }
            if (!ManageProvider.Provider.Current().IsSystem)//判断是否有权限
            {
                strSql.Append(" AND ( GroupUserId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}