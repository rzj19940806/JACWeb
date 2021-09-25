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
    /// 数据范围权限表
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.22 15:28</date>
    /// </author>
    /// </summary>
    public class Base_DataScopePermissionBll : RepositoryFactory<Base_DataScopePermission>
    {
        private static Base_DataScopePermissionBll item;
        /// <summary>
        /// 静态化
        /// </summary>
        public static Base_DataScopePermissionBll Instance
        {
            get
            {
                if (item == null)
                {
                    item = new Base_DataScopePermissionBll();
                }
                return item;
            }
        }
        /// <summary>
        /// 新建的项目数据，默认把数据权限设置了这样就别必要要去数据权限管理里面去打钩
        /// </summary>
        /// <param name="ModuleId">模块主键</param>
        /// <param name="ObjectId">对象主键</param>
        /// <param name="ResourceId">对什么资源</param>
        /// <param name="isOpenTrans">事务</param>
        public void AddScopeDefault(string ModuleId, string ObjectId, string ResourceId, string Category, DbTransaction isOpenTrans = null)
        {
            Base_DataScopePermission entity = new Base_DataScopePermission();
            entity.Create();
            entity.ModuleId = ModuleId;
            entity.ObjectId = ObjectId;
            entity.Category = Category;
            entity.ResourceId = ResourceId;
            if (isOpenTrans != null)
            {
                Repository().Insert(entity, isOpenTrans);
            }
            else
            {
                Repository().Insert(entity);
            }
        }

        #region 公司管理
        /// <summary>
        /// 加载公司列表
        /// <param name="ModuleId">模块主键</param>
        /// <param name="ObjectId">对象主键</param>
        /// <param name="Category">对象分类:1-部门2-角色3-岗位4-群组</param>
        /// </summary>
        /// <returns></returns>
        public DataTable GetScopeCompanyList(string ModuleId, string ObjectId, string Category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  c.CompanyId ,				
                                    c.ParentId ,				
                                    c.Code ,					
                                    c.FullName ,				
                                    c.SortCode ,				
                                    dsp.ObjectId				
                            FROM    Base_Company c
                                    LEFT JOIN Base_DataScopePermission dsp ON c.CompanyId = dsp.ResourceId
												                                AND dsp.ObjectId = @ObjectId
                                                                                AND dsp.Category = @Category
                                                                                AND dsp.ModuleId = @ModuleId");
            strSql.Append(" WHERE   1 = 1");
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( CompanyId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            strSql.Append(" order by c.SortCode ASC");
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", ObjectId));
            parameter.Add(DbFactory.CreateDbParameter("@Category", Category));
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", ModuleId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
        #endregion

        #region 部门管理
        /// <summary>
        /// 加载部门列表
        /// <param name="ModuleId">模块主键</param>
        /// <param name="ObjectId">对象主键</param>
        /// <param name="Category">对象分类:1-部门2-角色3-岗位4-群组</param>
        /// </summary>
        /// <returns></returns>
        public DataTable GetScopeDepartmentList(string ModuleId, string ObjectId, string Category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  T.* ,
                                    dsp.ObjectId
                            FROM    ( SELECT    CompanyId ,				
                                                CompanyId AS DepartmentId ,
                                                Code ,					
                                                FullName ,				
                                                ParentId ,				
                                                SortCode ,				
                                                'Company' AS Sort		
                                      FROM      Base_Company			
                                      UNION
                                      SELECT    CompanyId ,				
                                                DepartmentId ,			
                                                Code ,					
                                                FullName ,				
                                                CompanyId AS ParentId ,	
                                                SortCode ,				
                                                'Department' AS Sort	
                                      FROM      Base_Department			
          
                                    ) T
                                    LEFT JOIN Base_DataScopePermission dsp ON T.DepartmentId = dsp.ResourceId
                                                                              AND dsp.ObjectId = @ObjectId
                                                                              AND dsp.Category = @Category
                                                                              AND dsp.ModuleId = @ModuleId");
            strSql.Append(" WHERE 1 = 1");
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( DepartmentId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            strSql.Append(" order by T.SortCode ASC");
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", ObjectId));
            parameter.Add(DbFactory.CreateDbParameter("@Category", Category));
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", ModuleId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
        #endregion

        #region 角色管理
        /// <summary>
        /// 加载角色列表
        /// <param name="ModuleId">模块主键</param>
        /// <param name="ObjectId">对象主键</param>
        /// <param name="Category">对象分类:1-部门2-角色3-岗位4-群组</param>
        /// </summary>
        /// <returns></returns>
        public DataTable GetScopeRoleList(string ModuleId, string ObjectId, string Category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  T.* ,
                                    dsp.ObjectId
                            FROM    ( SELECT    CompanyId ,				
                                                CompanyId AS RoleId ,	
                                                Code ,					
                                                FullName ,				
                                                ParentId ,				
                                                SortCode ,				
                                                'Company' AS Sort		
                                      FROM      Base_Company			
                                      UNION
                                      SELECT    CompanyId ,				
                                                RoleId ,				
                                                Code ,					
                                                FullName ,				
                                                CompanyId AS ParentId ,	
                                                SortCode ,				
                                                'Roles' AS Sort			
                                      FROM      Base_Roles
                                    ) T
                                    LEFT JOIN Base_DataScopePermission dsp ON T.RoleId = dsp.ResourceId
                                                                              AND dsp.ObjectId = @ObjectId
                                                                              AND dsp.Category = @Category
                                                                              AND dsp.ModuleId = @ModuleId");
            strSql.Append(" WHERE 1 = 1");
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( RoleId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            strSql.Append(" order by T.SortCode ASC");
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", ObjectId));
            parameter.Add(DbFactory.CreateDbParameter("@Category", Category));
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", ModuleId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
        #endregion

        #region 岗位管理
        /// <summary>
        /// 加载岗位列表
        /// <param name="ModuleId">模块主键</param>
        /// <param name="ObjectId">对象主键</param>
        /// <param name="Category">对象分类:1-部门2-角色3-岗位4-群组</param>
        /// </summary>
        /// <returns></returns>
        public DataTable GetScopePostList(string ModuleId, string ObjectId, string Category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  T.* ,
                                    dsp.ObjectId
                            FROM    ( SELECT    CompanyId AS Id ,		
                                                Code ,					
                                                FullName ,				
                                                ParentId ,				
                                                SortCode ,				
                                                'Company' AS Sort		
                                      FROM      Base_Company			
                                      UNION
                                      SELECT    DepartmentId AS Id,		
                                                Code ,					
                                                FullName ,				
                                                CompanyId AS ParentId ,	
                                                SortCode ,				
                                                'Department' AS Sort	
                                      FROM      Base_Department			
                                      UNION
                                      SELECT    PostId AS Id,			
                                                Code ,					
                                                FullName ,				
                                                DepartmentId AS ParentId ,
                                                SortCode ,				
                                                'Post' AS Sort			
                                      FROM      Base_Post				
          
                                    ) T
                                    LEFT JOIN Base_DataScopePermission dsp ON T.Id = dsp.ResourceId
                                                                              AND dsp.ObjectId = @ObjectId
                                                                              AND dsp.Category = @Category
                                                                              AND dsp.ModuleId = @ModuleId");
            strSql.Append(" WHERE 1 = 1");
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( Id IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            strSql.Append(" order by T.SortCode ASC");
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", ObjectId));
            parameter.Add(DbFactory.CreateDbParameter("@Category", Category));
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", ModuleId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
        #endregion

        #region 用户组管理
        /// <summary>
        /// 加载用户组列表
        /// <param name="ModuleId">模块主键</param>
        /// <param name="ObjectId">对象主键</param>
        /// <param name="Category">对象分类:1-部门2-角色3-岗位4-群组</param>
        /// </summary>
        /// <returns></returns>
        public DataTable GetScopeUserGroupList(string ModuleId, string ObjectId, string Category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  T.* ,
                                    dsp.ObjectId
                            FROM    ( SELECT    CompanyId AS Id ,		
                                                Code ,					
                                                FullName ,				
                                                ParentId ,				
                                                SortCode ,				
                                                'Company' AS Sort		
                                      FROM      Base_Company			
                                      UNION
                                      SELECT    DepartmentId AS Id,		
                                                Code ,					
                                                FullName ,				
                                                CompanyId AS ParentId ,	
                                                SortCode ,				
                                                'Department' AS Sort	
                                      FROM      Base_Department			
                                      UNION
                                      SELECT    GroupUserId AS Id,		
                                                Code ,					
                                                FullName ,				
                                                DepartmentId AS ParentId ,
                                                SortCode ,				
                                                'UserGroup' AS Sort		
                                      FROM      Base_GroupUser			
          
                                    ) T
                                    LEFT JOIN Base_DataScopePermission dsp ON T.Id = dsp.ResourceId
                                                                              AND dsp.ObjectId = @ObjectId
                                                                              AND dsp.Category = @Category
                                                                              AND dsp.ModuleId = @ModuleId");
            strSql.Append(" WHERE 1 = 1");
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( Id IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            strSql.Append(" order by T.SortCode ASC");
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", ObjectId));
            parameter.Add(DbFactory.CreateDbParameter("@Category", Category));
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", ModuleId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
        #endregion

        #region 用户管理
        /// <summary>
        /// 加载用户列表
        /// <param name="ModuleId">模块主键</param>
        /// <param name="ObjectId">对象主键</param>
        /// <param name="Category">对象分类:1-部门2-角色3-岗位4-群组</param>
        /// </summary>
        /// <returns></returns>
        public DataTable GetScopeUserList(string ModuleId, string ObjectId, string Category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  T.* ,
                                    dsp.ObjectId
                            FROM    ( SELECT    CompanyId AS Id ,		
                                                Code ,					
                                                FullName ,				
                                                ParentId ,				
                                                '' AS Gender,			
                                                SortCode ,				
                                                'Company' AS Sort		
                                      FROM      Base_Company			
                                      UNION
                                      SELECT    DepartmentId AS Id,		
                                                Code ,					
                                                FullName ,				
                                                CompanyId AS ParentId ,	
                                                '' AS Gender,			
                                                SortCode ,				
                                                'Department' AS Sort	
                                      FROM      Base_Department			
                                      UNION
                                      SELECT    UserId AS Id,			
                                                Code ,					
                                                RealName ,				
                                                DepartmentId AS ParentId ,
                                                Gender,					
                                                SortCode ,				
                                                'User' AS Sort			
                                      FROM      Base_User			    
          
                                    ) T
                                    LEFT JOIN Base_DataScopePermission dsp ON T.Id = dsp.ResourceId
                                                                              AND dsp.ObjectId = @ObjectId
                                                                              AND dsp.Category = @Category
                                                                              AND dsp.ModuleId = @ModuleId");
            strSql.Append(" WHERE 1 = 1");
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( Id IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            strSql.Append(" order by T.SortCode ASC");
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", ObjectId));
            parameter.Add(DbFactory.CreateDbParameter("@Category", Category));
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", ModuleId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
        #endregion
    }
}