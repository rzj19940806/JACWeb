//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

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
    /// ��Ա������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.26 19:27</date>
    /// </author>
    /// </summary>
    public class BBdbR_StfBaseBll : RepositoryFactory<BBdbR_StfBase>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_StfBase";//===����ʱ��Ҫ�޸�===
        public readonly RepositoryFactory<Base_User> repository_User = new RepositoryFactory<Base_User>();
        #endregion

        #region 1.��ȡ������Ҫ�޸�sql���
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===����ʱ��Ҫ�޸�===
            strSql.Append(@"select  
                        DepartmentID AS keys,     
                        DepartmentCode AS code,
                        DepartmentName AS name,
                        Enabled As IsAvailable,
                        '0' as parentId,  
                        0 as sort    
                        from BBdbR_DepartmentBase where Enabled = '1' and DepartmentType='������'");
            strSql.Append(@" union select    
                             DepartmentID AS keys,
                             DepartmentCode AS code,
                             DepartmentName AS name,
                             Enabled As IsAvailable,
                             ParentDepartmentID AS parentId,
                             1 as sort 
                          from  BBdbR_DepartmentBase where Enabled = '1' and DepartmentType='�Ӳ���' order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_StfBase entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
        }
        #endregion

        #region 3.���ĳ���ֶε��ֶ�ֵ�Ƿ����
        /// <summary>
        ///   Enabled = 1��������ĳ���ֶΣ�KeyName�����ֶ�ֵ��KeyValue���Ƿ����
        /// </summary>
        /// <param name="KeyName">�ֶ���</param>
        /// <param name="KeyValue">�ֶ�ֵ</param>
        /// <returns>���ص��������ı��а������ֶ�ֵ�ļ�¼����</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion

        #region 4.��������
        //entityʵ���е������Ǵ�ҳ���д����ģ������û���д������
        //entityʵ����ֻ�в����ֶ���ֵ����Ϊҳ����ֻ�ṩ�������ֶθ�ֵ
        //��ҳ������д��������ʵ�壨entity���ķ�ʽ���������ݿ�
        //���У�ʵ���е�IsAvailable�ֶ�û����ҳ������д
        //IsAvailable�ֶε�����������ɾ���������ݿ��е�ĳһ�����ݵ�IsAvailable�ֶε��ֶ�ֵΪtrue��ʾ�����ݴ���
        //�ֶ�ֵΪfalse��ʾ�����ݱ�ɾ��
        //��ɾ�����ݿ��е�ĳһ������ʱֻҪ�޸�IsAvailable�ֶε��ֶ�ֵΪfalse������ɾ����������
        //������ʱ��ʵ���IsAvailable�ֶε�ֵ�޸�Ϊtrue
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Insert(BBdbR_StfBase entity) //===����ʱ��Ҫ�޸�===
        {
            entity.Enabled = "1";

            return Repository().Insert(entity);
        }
        #endregion

        #region 5.ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(string[] array)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_StfBase> listEntity = new List<BBdbR_StfBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_StfBase entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
        }
        #endregion

        #region 6.ҳ����
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>

        public List<BBdbR_StfBase> GetPlineList(string WorkSectionNm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_WorkSectionBase where Enable=1 and WorkSectionNm='" + WorkSectionNm + "'");
            List<BBdbR_StfBase> dt = Repository().FindListBySql(strSql.ToString());
            return dt;
        }
        public List<BBdbR_StfBase> GetPlineList2(string WorkSectionCd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_WorkSectionBase where Enable=1 and WorkSectionCd='" + WorkSectionCd + "'");
            List<BBdbR_StfBase> dt = Repository().FindListBySql(strSql.ToString());
            return dt;
        }

        #endregion

        #region 7.�����������
        /// <summary>
        /// ��ѯ��λ�����Ϣ������ڣ���δ�������õİ���б�
        /// ��ѯ��ʱ�򴫵���keywords
        /// </summary>
        /// <param name="keywords">��ѯֵ</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public List<BBdbR_StfBase> GetReStaffList(string keywords, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select * from " + tableName + " where Enabled=1 and StfId not in (select Distinct(StfId) from BFacR_TeamStfConfig where Enabled=1 and TeamId='" + keywords + "')";
                return Repository().FindListPageBySql(sql.ToString(), ref jqgridparam);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ��ѯ��λ�����Ϣ������ڣ�Ҳ�������õİ���б�
        /// ��ѯ��ʱ�򴫵���keywords
        /// </summary>
        /// <param name="keywords">��ѯֵ</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public List<BBdbR_StfBase> GetStaffList(string keywords, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select a.* from " + tableName + " a join BFacR_TeamStfConfig b on a.StfId=b.StfId where a.Enabled=1 and b.Enabled=1 and b.TeamId='" + keywords + "'";
                return Repository().FindListPageBySql(sql.ToString(), ref jqgridparam);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ��������й��ڲ������µ���Ա����
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>����������������</returns>
        public int GetStfCount(string KeyValue) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (KeyValue != "")
            {
                sql = @"select * from " + tableName + " where Enabled='1' and DepartmentID='" + KeyValue + "'";
                DataTable dt = Repository().FindTableBySql(sql);
                if (dt.Rows.Count > 0)
                {
                    return Repository().FindTableBySql(sql).Rows.Count;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 1;
            }
        }
        #endregion

        #region ϵͳ����

        #region  ���������Ϣ��ȡ�û���Ϣ
        public DataTable GetUserInforList(string DeptCd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from BBdbR_StfBase where DeptCd='" + DeptCd + "'");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion
        #region  ��ѯ�û���Ϣ
        public DataTable GetAllUserList(string condition, string keywords, string Tycd, string ParameterJson, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT StfId,StfCd,StfNm,Email FROM BBdbR_StfBase where Enabled=1 ");
            switch (condition)
            {
                case "stfcd"://�û����
                    strSql.Append(" and stfcd LIKE  '%" + keywords + "%'");
                    break;
                case "stfnm"://�û�����
                    strSql.Append(" and stfnm LIKE  '%" + keywords + "%'");
                    break;
                default:
                    break;
            }
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string code = dt.Rows[i]["StfId"].ToString();
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
        #region ��ȡδ���øý�ɫ���û���Ϣ
        public DataTable GetNoRoleUserList(string ObjectId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from BBdbR_StfBase where Enabled=1");
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string code = dt.Rows[i]["StfId"].ToString();
                StringBuilder str = new StringBuilder();
                str.Append(@"select * from Base_StfRoleConf where StfId='" + code + "' and RoleId='" + ObjectId + "' ");
                DataTable dt1 = Repository().FindTableBySql(str.ToString());
                if (dt1.Rows.Count != 0)
                {
                    dt.Rows.Remove(dr);
                }
            }
            return dt;
        }
        #endregion
        #region ��ȡ��ɫ��Ա
        /// <param name="RoleId">��������</param>
        public DataTable GetRoleUserList(string RoleId)
        {
            StringBuilder strSql = new StringBuilder();
            //List<DbParameter> parameter = new List<DbParameter>();
            //parameter.Add(DbFactory.CreateDbParameter("@RoleId", RoleId));
            strSql.Append(@"SELECT  r.StfId ,				
                                    r.DeptCd ,				
                                    r.StfNm ,			
                                    r.StfCd ,	
                                    r.StfGndr ,
                                    r.Account ,	
                                    ou.RoleId			
                            FROM    BBdbR_StfBase r
                                    LEFT JOIN Base_StfRoleConf ou ON ou.StfId = r.StfId
                                                                            AND ou.RoleId = '" + RoleId + "'");
            strSql.Append(" WHERE 1 = 1");

            //strSql.Append(@"SELECT  *
            //                FROM    ( SELECT    u.StfId ,				
            //                                    u.DeptCd ,				
            //                                    u.StfNm ,			
            //                                    u.StfCd ,				
            //                                    u.Account ,				
            //                                    u.StfGndr ,			
            //                                    u.SortCode ,					
            //                                    ou.RoleId				
            //                          FROM      BBdbR_StfBase u
            //                                    LEFT JOIN Base_StfRoleConf ou ON ou.StfId = u.StfId
            //AND ou.RoleId = @RoleId                                                                                        
            //                        ) T WHERE 1=1");

            return Repository().FindTableBySql(strSql.ToString());
        }

        #endregion
        #region ����ȫ���û�
        public DataTable GetUserList(JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM Base_User WHERE 1=1");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion
        #region ��ȡδ�����û���Ϣ
        public DataTable GetNoConList(string Tycd, string ParameterJson, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from BBdbR_StfBase where Enabled = 1");
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string code = dt.Rows[i]["StfId"].ToString();
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
        #region ��ȡ�������û�
        public DataTable GetConUserList(string Tycd, string Rank, string ParameterJson, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from PushObject_StfBase_V where InforTypCd='" + Tycd + "' and PushRank='" + Rank + "' and Enabled='" + 1 + "'");
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            return dt;
        }
        #endregion
        #region ɾ��֮ǰ�û�������
        public int DeleteConUser(string Tycd, string Type, string Rank)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"Delete from BBdbR_PushObjectConf  where InforTypCd = '" + Tycd + "' and PushRank = '" + Rank + "' and ObjectTyp = '" + 1 + "'");
            return Repository().ExecuteBySql(strSql);
        }
        #endregion

        #endregion
    }
}
