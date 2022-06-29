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
    /// ���ݷ�ΧȨ�ޱ�
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.22 15:28</date>
    /// </author>
    /// </summary>
    public class Base_DataScopePermissionBll : RepositoryFactory<Base_DataScopePermission>
    {
        private static Base_DataScopePermissionBll item;
        /// <summary>
        /// ��̬��
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
        /// �½�����Ŀ���ݣ�Ĭ�ϰ�����Ȩ�������������ͱ��ҪҪȥ����Ȩ�޹�������ȥ��
        /// </summary>
        /// <param name="ModuleId">ģ������</param>
        /// <param name="ObjectId">��������</param>
        /// <param name="ResourceId">��ʲô��Դ</param>
        /// <param name="isOpenTrans">����</param>
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

        #region ��˾����
        /// <summary>
        /// ���ع�˾�б�
        /// <param name="ModuleId">ģ������</param>
        /// <param name="ObjectId">��������</param>
        /// <param name="Category">�������:1-����2-��ɫ3-��λ4-Ⱥ��</param>
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

        #region ���Ź���
        /// <summary>
        /// ���ز����б�
        /// <param name="ModuleId">ģ������</param>
        /// <param name="ObjectId">��������</param>
        /// <param name="Category">�������:1-����2-��ɫ3-��λ4-Ⱥ��</param>
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

        #region ��ɫ����
        /// <summary>
        /// ���ؽ�ɫ�б�
        /// <param name="ModuleId">ģ������</param>
        /// <param name="ObjectId">��������</param>
        /// <param name="Category">�������:1-����2-��ɫ3-��λ4-Ⱥ��</param>
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

        #region ��λ����
        /// <summary>
        /// ���ظ�λ�б�
        /// <param name="ModuleId">ģ������</param>
        /// <param name="ObjectId">��������</param>
        /// <param name="Category">�������:1-����2-��ɫ3-��λ4-Ⱥ��</param>
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

        #region �û������
        /// <summary>
        /// �����û����б�
        /// <param name="ModuleId">ģ������</param>
        /// <param name="ObjectId">��������</param>
        /// <param name="Category">�������:1-����2-��ɫ3-��λ4-Ⱥ��</param>
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

        #region �û�����
        /// <summary>
        /// �����û��б�
        /// <param name="ModuleId">ģ������</param>
        /// <param name="ObjectId">��������</param>
        /// <param name="Category">�������:1-����2-��ɫ3-��λ4-Ⱥ��</param>
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