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
    /// 部门管理
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.07 12:34</date>
    /// </author>
    /// </summary>
    public class Base_DepartmentBll : RepositoryFactory<Base_Department>
    {
        #region
        /// <summary>
        /// 获取 公司、部门 列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    CompanyId,				
        				CompanyId AS DepartmentId ,
                                                Code ,					
                                                FullName ,				
                                                ParentId ,				
                                                SortCode,				
                                                'Company' AS Sort		
                                      FROM      Base_Company			
                                      UNION
                                      SELECT    CompanyId,				
        				DepartmentId,			
                                                Code ,					
                                                FullName ,				
                                                CompanyId AS ParentId ,	
                                                SortCode,				
                                                'Department' AS Sort	
                                      FROM      Base_Department			
                                    ) T WHERE 1=1 ");
            //if (!ManageProvider.Provider.Current().IsSystem)
            //{
            //    strSql.Append(" AND ( DepartmentId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
            //    strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
            //    strSql.Append(" ) )");
            //}
            strSql.Append(" ORDER BY SortCode ASC");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion
        /// <summary>
        /// 获取 公司、部门 列表
        /// </summary>
        /// <returns></returns>
        //public DataTable GetTree()
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append(@"  SELECT           a.site_key as Treekey ,
        //                                       a.site_name as  FullName,
        //                                       a.description as Code ,                                  
        //                                       '0' as parentid  ,
        //                                       '1' as Type  ,       
        //                                         'Company' AS Sort		                                                                  
        //                              FROM     SITE  a			
        //                              UNION						   
        //                              SELECT   
        //                                        a.area_key as Treekey ,
        //                                       a.area_name as  FullName,
        //                                       a.description as Code ,     
        //                                        b.parent_key as parentid ,   
        //                                         '2' as Type  ,
        //                                       'AREA' AS Sort		
        //                               FROM     AREA  a,SITE_AREA  b 
        //	  WHERE    a.area_key=b.child_key      
        //                              UNION                                    
        //                              SELECT    a.depart_key as Treekey ,
        //                                         a. depart_name as FullName ,   
        //                                        a. depart_code as Code ,                                                                              
        //                                        b.area_key as parentid ,
        //                                        '3' as Type ,
        //                                        'Department' AS Sort	                                           
        //                              FROM      AD_depart_basic  a, AREA  b
        //                              WHERE 1=1     ");//直接组合一行数据，即在现有的表中加了一行数据，用代码写死的数据（就是前面一行）
        //    return Repository().FindTableBySql(strSql.ToString());
        //}

        /// <summary>
        /// 根据公司id获取部门 列表
        /// </summary>
        /// <param name="CompanyId">公司ID</param>
        /// <returns></returns>
        public DataTable GetList(string CompanyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    d.DepartmentId ,
                                                c.FullName AS CompanyName ,
                                                d.CompanyId ,
                                                d.Code ,
                                                d.FullName ,
                                                d.ShortName ,
                                                d.Nature ,
                                                u.RealName AS Manager ,
                                                d.Phone ,
                                                d.Fax ,
                                                d.Enabled ,
                                                d.SortCode ,
                                                d.Remark
                                      FROM      Base_Department d
                                                LEFT JOIN Base_Company c ON c.CompanyId = d.CompanyId
                                                LEFT JOIN dbo.Base_User u ON u.UserId = d.Manager
                                    ) T
                            WHERE   1 = 1 ");
            List<DbParameter> parameter = new List<DbParameter>();
            if (!string.IsNullOrEmpty(CompanyId))
            {
                strSql.Append(" AND CompanyId = @CompanyId");
                parameter.Add(DbFactory.CreateDbParameter("@CompanyId", CompanyId));
            }
            //if (!ManageProvider.Provider.Current().IsSystem)
            //{
            //    strSql.Append(" AND ( DepartmentId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
            //    strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
            //    strSql.Append(" ) )");
            //}
            strSql.Append(" ORDER BY CompanyId ASC,SortCode ASC");
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
    }
}