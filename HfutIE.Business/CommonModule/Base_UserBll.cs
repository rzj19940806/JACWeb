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
    /// �û�����
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.11 15:45</date>
    /// </author>
    /// </summary>
    public class Base_UserBll : RepositoryFactory<Base_User>
    {
        /// <summary>
        /// ��ȡ�û��б�
        /// </summary>
        /// <param name="keyword">ģ���ѯ</param>
        /// <param name="CompanyId">��˾ID</param>
        /// <param name="DepartmentId">����ID</param>
        /// <param name="ParameterJson">�߼���ѯ����JSON</param>
        /// <param name="jqgridparam">��ҳ����</param>
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
            //��������Ȩ��
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( UserId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        /// <summary>
        /// �ж��Ƿ����ӷ�����
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
        /// ��½��֤��Ϣ
        /// </summary>
        /// <param name="Account">�˻�</param>
        /// <param name="Password">����</param>
        /// <param name="result">���ؽ��</param>
        /// <returns></returns>
        public Base_User UserLogin(string Account, string Password, out string result)
        {
            if (!this.IsLinkServer())
            {
                throw new Exception("���������Ӳ��ϣ�" + DbResultMsg.ReturnMsg);
            }
            Base_User entity = Repository().FindEntity("Account", Account);
            if (entity != null && entity.UserId != null)
            {
                if (entity.Enabled == 1)
                {
                    string dbPassword = Md5Helper.MD5(DESEncrypt.Encrypt(Password.ToLower(), entity.Secretkey).ToLower(), 32).ToLower();//����
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
        /// ��ȡ�û���ɫ�б�
        /// </summary>
        /// <param name="CompanyId">��˾ID</param>
        /// <param name="UserId">�û�Id</param>
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
        /// ѡ���û��б�
        /// </summary>
        /// <param name="keyword">ģ���ѯ</param>
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
        /// ���� �û��б�
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
        //                              WHERE 1=1     ");//ֱ�����һ�����ݣ��������еı��м���һ�����ݣ��ô���д�������ݣ�����ǰ��һ�У�
        //    return Repository().FindTableBySql(strSql.ToString());
        //}

        #region 5.ɾ�û���
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(string[] array)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<Base_User> listEntity = new List<Base_User>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                Base_User entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                //entity.Enabled = 0;//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(entity);
            }
            return Repository().Delete(listEntity);//ɾ���ݿ�
        }
        #endregion

        #region 6.��ѯ��������Ҫ�޸�sql���
        /// <summary>
        ///     ��ѯʱ�ṩ�������ؼ��֣�һ����Condition����һ����keywords
        ///     
        ///     Condition�ǹؼ��֣�����ѯ��������Ӧ���ݿ��е�һ���ֶ�
        ///     keywords�ǲ�ѯֵ������ѯ�����ľ���ֵ����Ӧ���ݿ��в�ѯ�����ֶε�ֵ
        ///     ��ѯ��ʱ�򴫵���Condition��keywords
        /// 
        /// </summary>
        /// <param name="keywords">��ѯֵ</param>
        /// <param name="Condition">�ؼ��֣���ѯ������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>��ѯ�����ݣ��б�</returns>
        public DataTable GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
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
                //����������ѯ
                sql = @"select * from Base_User where 1 = 1 and " + Condition + " like  '%" + keywords + "%'";
                return Repository().FindTableBySql(sql.ToString());
            }
            //return dt;
        }
        #endregion

        #region 7.�༭�û���Ϣ

        #endregion


    }

}