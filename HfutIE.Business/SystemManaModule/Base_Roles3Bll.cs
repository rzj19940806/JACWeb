//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
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
    /// Base_Roles3
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.25 13:41</date>
    /// </author>
    /// </summary>
    public class Base_Roles3Bll : RepositoryFactory<Base_Roles3>
    {
        #region
        #endregion

        #region 3.��ȡΪ���ý�ɫ��Ϣ
        public System.Data.DataTable GetNoConList(string Tycd, string ParameterJson, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from Base_Roles where Enabled = 1");
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string code = dt.Rows[i]["RoleId"].ToString();//����
                StringBuilder str = new StringBuilder();
                str.Append(@"select * from BBdbR_PushObjectConf where ObjectId='" + code + "' and InforTypCd='" + Tycd + "'and Enabled='" + 1 + "'");
                DataTable dt1 = Repository().FindTableBySql(str.ToString());
                if (dt1.Rows.Count != 0)
                {
                    dt.Rows.Remove(dr);
                }
            }
            return dt;
        }
        #endregion

        #region  4.��ѯ��ɫ��Ϣ
        public DataTable GetNoConRoleList(string condition, string keywords, string Tycd, string ParameterJson, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT RoleId,RoleCd,RoleNm,RoleCatg FROM Base_Roles where Enabled=1 ");
            switch (condition)
            {
                case "rolecd"://��ɫ���
                    strSql.Append(" and rolecd LIKE  '%" + keywords + "%'");
                    break;
                case "rolenm"://��ɫ����
                    strSql.Append(" and rolenm LIKE  '%" + keywords + "%'");
                    break;
                default:
                    break;
            }
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string code = dt.Rows[i]["roleid"].ToString();
                StringBuilder str = new StringBuilder();
                str.Append(@"select * from BBdbR_PushObjectConf where ObjectId='" + code + "' and InforTypCd='" + Tycd + "'and Enabled='" + 1 + "'");
                DataTable dt1 = Repository().FindTableBySql(str.ToString());
                if (dt1.Rows.Count != 0)
                {
                    dt.Rows.Remove(dr);
                }
            }
            return dt;
            //return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region ��ȡ�����ý�ɫ
        public DataTable GetConRoleList(string Tycd,string Rank, string ParameterJson, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from PushObject_BaseRoles_V where InforTypCd='" + Tycd + "'and PushRank = '" + Rank + "' and Enabled='" + 1 + "'");
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            return dt;
        }
        #endregion

        #region ɾ��֮ǰ�û�������
        public int DeleteConRole(string Tycd, string Type, string Rank)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"Delete from BBdbR_PushObjectConf  where InforTypCd = '" + Tycd + "' and PushRank = '" + Rank + "' and ObjectTyp = '" + 2 + "'");
            return Repository().ExecuteBySql(strSql);
        }
        #endregion

        #region ��ȡ��
        public DataTable GetList()
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT [RoleId] as ID,[RoleNm] as name,'0'AS ParentId,'0' as Sort FROM Base_Roles3 WHERE 1=1");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region �������ڵ���ؽ�ɫ��Ϣ
        public DataTable GetPageList(string RoleId, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM Base_Roles3 WHERE RoleId='"+ RoleId + "'");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region ����ȫ����Ϣ
        public DataTable GetRoleList(JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM Base_Roles3 WHERE 1=1");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region  �༭������Ϣ
        public DataTable SetPushinfor(string RoleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from Base_Roles3 where RoleId='" + RoleId + "'");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region ��ȡ�û���ɫ
        /// <param name="RoleId">��������</param>
        public DataTable GetUserRoleList(string UserId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  r.RoleId ,				
                                    r.Code ,				
                                    r.FullName ,			
                                    r.SortCode ,			
                                    ou.ObjectId			
                            FROM    Base_Roles3 r
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
            //strSql.Append(" AND r.CompanyId = @CompanyId");
            parameter.Add(DbFactory.CreateDbParameter("@UserId", UserId));
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }

        #endregion
    }
}