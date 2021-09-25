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
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 用户管理
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.11 15:45</date>
    /// </author>
    /// </summary>
    public class Base_UserBll : RepositoryFactory<Base_User>
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="keyword">模块查询</param>
        /// <param name="CompanyId">公司ID</param>
        /// <param name="DepartmentId">部门ID</param>
        /// <param name="ParameterJson">高级查询条件JSON</param>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public DataTable GetPageList(string keyword, string CompanyId, string DepartmentId, string ParameterJson, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    u.UserId ,					
                                                u.Code ,					
                                                u.Account ,					
                                                u.RealName ,				
                                                u.Spell,                    
                                                u.Gender ,					
                                                u.Mobile ,					
                                                u.Telephone ,				
                                                u.Email ,
                                                u.OICQ ,					
                                                u.CompanyId ,			    
                                                c.FullName AS CompanyName ,	
                                                u.DepartmentId,				
                                                d.FullName AS DepartmentName,
                                                e.Duty,                     
                                                u.Enabled ,					
                                                u.LogOnCount ,				
                                                u.LastVisit ,				
                                                u.SortCode,                
                                                u.CreateUserId,				
                                                u.Remark					
                                      FROM      Base_User u
                                                LEFT JOIN Base_Company c ON c.CompanyId = u.CompanyId
                                                LEFT JOIN Base_Department d ON d.DepartmentId = u.DepartmentId
                                                LEFT JOIN Base_Employee e ON e.UserId = u.UserId
                                    ) T WHERE 1=1");
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@" AND (RealName LIKE @keyword
                                    OR Account LIKE @keyword
                                    OR Code LIKE @keyword
                                    OR Spell LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keyword + '%'));
            }
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
            if (!string.IsNullOrEmpty(ParameterJson))
            {
                List<DbParameter> outparameter = new List<DbParameter>();
                strSql.Append(ConditionBuilder.GetWhereSql(ParameterJson.JonsToList<Condition>(), out outparameter));
                parameter.AddRange(outparameter);
            }
            //关联数据权限
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( UserId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        /// <summary>
        /// 判断是否连接服务器
        /// </summary>
        /// <returns></returns>
        public bool IsLinkServer()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  GETDATE()");
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 登陆验证信息
        /// </summary>
        /// <param name="Account">账户</param>
        /// <param name="Password">密码</param>
        /// <param name="result">返回结果</param>
        /// <returns></returns>
        public Base_User UserLogin(string Account, string Password, out string result)
        {
            if (!this.IsLinkServer())
            {
                throw new Exception("服务器连接不上，" + DbResultMsg.ReturnMsg);
            }
            Base_User entity = Repository().FindEntity("Account", Account);
            if (entity != null && entity.UserId != null)
            {
                if (entity.Enabled == 1)
                {
                    string dbPassword = Md5Helper.MD5(DESEncrypt.Encrypt(Password.ToLower(), entity.Secretkey).ToLower(), 32).ToLower();//加密
                    if (dbPassword == entity.Password)
                    {
                        DateTime PreviousVisit = CommonHelper.GetDateTime(entity.LastVisit);
                        DateTime LastVisit = DateTime.Now;
                        int LogOnCount = CommonHelper.GetInt(entity.LogOnCount) + 1;
                        entity.PreviousVisit = PreviousVisit;
                        entity.LastVisit = LastVisit;
                        entity.LogOnCount = LogOnCount;
                        entity.Online = 1;
                        Repository().Update(entity);
                        result = "succeed";
                    }
                    else
                    {
                        result = "error";
                    }
                }
                else
                {
                    result = "lock";
                }
                return entity;
            }
            result = "-1";
            return null;
        }
        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <param name="CompanyId">公司ID</param>
        /// <param name="UserId">用户Id</param>
        /// <returns></returns>
        public DataTable UserRoleList(string CompanyId, string UserId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  r.RoleId ,				
                                    r.Code ,				
                                    r.FullName ,			
                                    r.SortCode ,			
                                    ou.ObjectId			
                            FROM    Base_Roles r
                                    LEFT JOIN Base_ObjectUserRelation ou ON ou.ObjectId = r.RoleId
                                                                            AND ou.UserId = @UserId
                                                                            AND ou.Category = 2
                            WHERE 1 = 1");
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( RoleId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            strSql.Append(" AND r.CompanyId = @CompanyId");
            parameter.Add(DbFactory.CreateDbParameter("@UserId", UserId));
            parameter.Add(DbFactory.CreateDbParameter("@CompanyId", CompanyId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
        /// <summary>
        /// 选择用户列表
        /// </summary>
        /// <param name="keyword">模块查询</param>
        /// <returns></returns>
        public DataTable OptionUserList(string keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@"SELECT TOP 50 * FROM ( SELECT    
                                        u.UserId ,
                                        u.Account ,
                                        u.code,
                                        u.RealName ,
                                        u.DepartmentId ,
                                        d.FullName AS DepartmentName,
                                        u.Gender
                                FROM    Base_User u
                                LEFT JOIN Base_Department d ON d.DepartmentId = u.DepartmentId
                                WHERE   u.RealName LIKE @keyword
                                        OR u.Account LIKE @keyword
                                        OR u.Code LIKE @keyword
                                        OR u.Spell LIKE @keyword
                                        OR u.UserId IN (
                                        SELECT  u.UserId
                                        FROM    Base_User u
                                                INNER JOIN Base_ObjectUserRelation oc ON u.UserId = oc.UserId
                                                INNER JOIN dbo.Base_Company c ON c.CompanyId = oc.ObjectId
                                        WHERE   c.FullName LIKE @keyword
                                        UNION
                                        SELECT  u.UserId
                                        FROM    Base_User u
                                                INNER JOIN Base_ObjectUserRelation od ON u.UserId = od.UserId
                                                INNER JOIN Base_Department d ON d.DepartmentId = od.ObjectId
                                        WHERE   d.FullName LIKE @keyword
                                        UNION
                                        SELECT  u.UserId
                                        FROM    Base_User u
                                                INNER JOIN Base_ObjectUserRelation oro ON u.UserId = oro.UserId
                                                INNER JOIN Base_Roles r ON r.RoleId = oro.ObjectId
                                        WHERE   r.FullName LIKE @keyword
                                        UNION
                                        SELECT  u.UserId
                                        FROM    Base_User u
                                                INNER JOIN Base_ObjectUserRelation op ON u.UserId = op.UserId
                                                INNER JOIN Base_Post p ON p.PostId = op.ObjectId
                                        WHERE   p.FullName LIKE @keyword
                                        UNION
                                        SELECT  u.UserId
                                        FROM    Base_User u
                                                INNER JOIN Base_ObjectUserRelation og ON u.UserId = og.UserId
                                                INNER JOIN Base_GroupUser g ON g.GroupUserId = og.ObjectId
                                        WHERE   g.FullName LIKE @keyword )
                            ) a WHERE 1 = 1");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keyword + '%'));
            }
            else
            {
                strSql.Append(@"SELECT TOP 50
                                        u.UserId ,
                                        u.Account ,
                                        u.code ,
                                        u.RealName ,
                                        u.DepartmentId ,
                                        d.FullName AS DepartmentName ,
                                        u.Gender
                                FROM    Base_User u
                                        LEFT JOIN Base_Department d ON d.DepartmentId = u.DepartmentId
                                WHERE   1 = 1");
            }
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( UserId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }

        /// <summary>
        /// 树形 用户列表
        /// </summary>
        /// <returns></returns>
        public DataTable TreeUserList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    ( SELECT    CompanyId AS Id ,
                                                Code ,
                                                FullName ,
                                                ParentId ,
                                                SortCode ,
                                                '' AS Gender ,
                                                'Company' AS Sort
                                      FROM      Base_Company
                                      UNION
                                      SELECT    DepartmentId AS Id ,
                                                Code ,
                                                FullName ,
                                                CompanyId AS ParentId ,
                                                SortCode ,
                                                '' AS Gender ,
                                                'Department' AS Sort
                                      FROM      Base_Department
                                      UNION
                                      SELECT    UserId AS Id ,
                                                Code ,
                                                RealName ,
                                                DepartmentId AS ParentId ,
                                                SortCode ,
                                                Gender ,
                                                '1' AS Sort
                                      FROM      Base_User
                                    ) T WHERE 1=1");
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( Id IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            strSql.Append("ORDER BY SortCode ASC;");
            return Repository().FindTableBySql(strSql.ToString());
        }
        //public DataTable GetTree()
        //{
        //    StringBuilder strSql = new StringBuilder();
        //     strSql.Append(@"  SELECT   a.site_key as Treekey ,
        //                                       a.site_name as  Name,
        //                                       a.description as Code ,                                  
        //                                       '0' as parentid  ,
        //                                       '1' as image                                                                           
        //                              FROM     SITE  a			
        //                              UNION						   
        //                              SELECT   
        //                                        a.area_key as Treekey ,
        //                                       a.area_name as  Name,
        //                                       a.description as Code ,     
        //                                        b.parent_key as parentid ,   
        //                                         '2' as image  
        //                               FROM     AREA  a,SITE_AREA  b 
        //	  WHERE    a.area_key=b.child_key      
        //                              UNION                                    
        //                              SELECT    a.depart_key as Treekey ,
        //                                         a. depart_name as Name ,   
        //                                        a. depart_code as Code ,                                                                              
        //                                        b.area_key as parentid ,
        //                                        '3' as image                                            
        //                              FROM      AD_depart_basic  a, AREA  b
        //                              WHERE 1=1     ");//直接组合一行数据，即在现有的表中加了一行数据，用代码写死的数据（就是前面一行）
        //    return Repository().FindTableBySql(strSql.ToString());
        //}

        #region 5.删用户表
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string[] array)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<Base_User> listEntity = new List<Base_User>(); //===复制时需要修改===
            foreach (string keyValue in array)
            {
                //===复制时需要修改===
                Base_User entity = Repository().FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                //entity.Enabled = 0;//将该实体的IsAvailable属性改为false
                listEntity.Add(entity);
            }
            return Repository().Delete(listEntity);//删数据库
        }
        #endregion

        #region 6.查询方法，需要修改sql语句
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
        public DataTable GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            //List<Base_User> dt;
            if (Condition == "all")
            {
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                strSql.Append(@"SELECT * FROM Base_User WHERE 1=1");
                //sql = @"select * from Base_User where 1 = 1";
                return Repository().FindTableBySql(strSql.ToString());
            }
            else
            {
                //根据条件查询
                sql = @"select * from Base_User where 1 = 1 and " + Condition + " like  '%" + keywords + "%'";
                return Repository().FindTableBySql(sql.ToString());
            }
            //return dt;
        }
        #endregion

        #region 7.编辑用户信息

        #endregion


    }

}