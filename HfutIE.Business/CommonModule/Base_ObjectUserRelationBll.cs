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
    /// 对象用户关系表
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.18 14:01</date>
    /// </author>
    /// </summary>
    public class Base_ObjectUserRelationBll : RepositoryFactory<Base_ObjectUserRelation>
    {
        /// <summary>
        /// 成员列表
        /// </summary>
        /// <param name="CompanyId">公司ID</param>
        /// <param name="DepartmentId">部门ID</param>
        /// <param name="ObjectId">对象主键</param>
        /// <param name="Category">对象分类:1-部门2-角色3-岗位4-群组</param>
        /// <returns></returns>
        public DataTable GetList(string CompanyId, string DepartmentId, string ObjectId, string Category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    u.UserId ,				
                                                u.Account ,				
                                                u.RealName ,			
                                                u.Code ,				
                                                u.Gender ,				
                                                u.CompanyId ,			
                                                u.DepartmentId ,		
                                                u.SortCode ,			
                                                ou.ObjectId				
                                      FROM      Base_User u
                                                LEFT JOIN Base_ObjectUserRelation ou ON ou.UserId = u.UserId
                                                                                        AND ou.ObjectId = @ObjectId
                                                                                        AND ou.Category = @Category
                                    ) T WHERE 1=1");
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
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", ObjectId));
            parameter.Add(DbFactory.CreateDbParameter("@Category", Category));
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( UserId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            strSql.Append(" ORDER BY ObjectId DESC,SortCode ASC");
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
        /// <summary>
        /// 批量添加成员
        /// </summary>
        /// <param name="arrayUserId">选中用户ID: 1,2,3,4,5,6</param>
        /// <param name="ObjectId">对象主键</param>
        /// <param name="Category">对象分类:1-部门2-角色3-岗位4-群组</param>
        /// <returns></returns>
        public int BatchAddMember(string[] arrayUserId, string ObjectId, string Category)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                StringBuilder sbDelete = new StringBuilder("DELETE FROM Base_ObjectUserRelation WHERE ObjectId = @ObjectId AND Category=@Category");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@ObjectId", ObjectId));
                parameter.Add(DbFactory.CreateDbParameter("@Category", Category));
                database.ExecuteBySql(sbDelete, parameter.ToArray(), isOpenTrans);
                //20210623加_处理Base_StfRoleConf表
                StringBuilder sbDelete2 = new StringBuilder("DELETE FROM Base_StfRoleConf WHERE RoleId = @ObjectId");
                List<DbParameter> parameter2 = new List<DbParameter>();
                parameter2.Add(DbFactory.CreateDbParameter("@ObjectId", ObjectId));
                database.ExecuteBySql(sbDelete2, parameter2.ToArray(), isOpenTrans);
                int index = 1;
                foreach (string item in arrayUserId)
                {
                    if (item.Length > 0)
                    {
                        string guid = CommonHelper.GetGuid;
                        Base_ObjectUserRelation entity = new Base_ObjectUserRelation();
                        entity.ObjectUserRelationId = guid;
                        entity.ObjectId = ObjectId;
                        entity.UserId = item;
                        entity.Category = Category;
                        entity.SortCode = index;
                        entity.Create();                      
                        database.Insert(entity, isOpenTrans);
                        //20210623加_处理Base_StfRoleConf表
                        Base_StfRoleConf stfroleconf = new Base_StfRoleConf();
                        stfroleconf.RecId = guid;
                        stfroleconf.StfId = item;
                        stfroleconf.RoleId = ObjectId;
                        stfroleconf.SortCode = index;
                        stfroleconf.Enabled = "1";
                        database.Insert(stfroleconf, isOpenTrans);
                        index++;
                    }
                }
                database.Commit();
                return 1;
            }
            catch
            {
                database.Rollback();
                return -1;
            }
        }
        /// <summary>
        /// 批量添加对象用户关系
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <param name="arrayObjectId">对象ID</param>
        /// <param name="Category">对象分类:1-部门2-角色3-岗位4-群组</param>
        /// <returns></returns>
        public int BatchAddObject(string UserId, string[] arrayObjectId, string Category)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                StringBuilder sbDelete = new StringBuilder("DELETE FROM Base_ObjectUserRelation WHERE UserId = @UserId AND Category=@Category");
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@UserId", UserId));
                parameter.Add(DbFactory.CreateDbParameter("@Category", Category));
                database.ExecuteBySql(sbDelete, parameter.ToArray(), isOpenTrans);
                //20210623加_处理Base_StfRoleConf表
                StringBuilder sbDelete2 = new StringBuilder("DELETE FROM Base_StfRoleConf WHERE StfId = @UserId");
                List<DbParameter> parameter2 = new List<DbParameter>();
                parameter2.Add(DbFactory.CreateDbParameter("@UserId", UserId));
                database.ExecuteBySql(sbDelete2, parameter2.ToArray(), isOpenTrans);
                int index = 1;
                foreach (string item in arrayObjectId)
                {
                    if (item.Length > 0)
                    {
                        string guid = CommonHelper.GetGuid;
                        Base_ObjectUserRelation entity = new Base_ObjectUserRelation();
                        entity.ObjectUserRelationId = guid;
                        entity.UserId = UserId;
                        entity.ObjectId = item;
                        entity.Category = Category;
                        entity.SortCode = index;
                        entity.Create();                     
                        database.Insert(entity, isOpenTrans);
                        //20210623加_处理Base_StfRoleConf表
                        Base_StfRoleConf stfroleconf = new Base_StfRoleConf();
                        stfroleconf.RecId = guid;
                        stfroleconf.StfId = UserId;
                        stfroleconf.RoleId = item;
                        stfroleconf.SortCode = index;
                        stfroleconf.Enabled = "1";
                        database.Insert(stfroleconf, isOpenTrans);
                        index++;
                    }
                }
                database.Commit();
                return 1;
            }
            catch
            {
                database.Rollback();
                return -1;
            }
        }
        /// <summary>
        /// 根据用户ID查询对象列表
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public List<Base_ObjectUserRelation> GetList(string UserId)
        {
            return Repository().FindList("UserId", UserId);
        }
        /// <summary>
        /// 根据用户ID查询对象列表
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public string GetObjectId(string UserId)
        {
            StringBuilder sb_ObjectId = new StringBuilder();
            List<Base_ObjectUserRelation> list = GetList(UserId);
            if (list.Count > 0)
            {
                foreach (Base_ObjectUserRelation item in list)
                {
                    sb_ObjectId.Append(item.ObjectId + ",");
                }
                sb_ObjectId.Append(UserId);
            }
            else
            {
                sb_ObjectId.Append(UserId + ",");
            }
            return sb_ObjectId.ToString();
        }
        /// <summary>
        /// 根据对象Id 获取用户列表
        /// </summary>
        /// <param name="ObjectId"></param>
        /// <returns></returns>
        public List<Base_User> GetUserList(string ObjectId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  u.UserId ,				
                                    u.Account ,				
                                    u.RealName ,			
                                    u.Code ,				
                                    u.Gender ,				
                                    u.CompanyId ,			
                                    u.DepartmentId ,		
                                    u.SortCode 			    
                            FROM    Base_User u
                                    INNER JOIN Base_ObjectUserRelation ou ON ou.UserId = u.UserId
                                                                            AND ou.ObjectId = @ObjectId");
            parameter.Add(DbFactory.CreateDbParameter("@ObjectId", ObjectId));
            return DataFactory.Database().FindListBySql<Base_User>(strSql.ToString(), parameter.ToArray());
        }
    }
}