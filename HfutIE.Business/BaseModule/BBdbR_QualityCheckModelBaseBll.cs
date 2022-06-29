//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// ���ģ�������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.25 22:00</date>
    /// </author>
    /// </summary>
    public class BBdbR_QualityCheckModelBaseBll : RepositoryFactory<BBdbR_QualityCheckModelBase>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_QualityCheckModelBase";//===����ʱ��Ҫ�޸�===
        public readonly RepositoryFactory<BBdbR_QualityCheckModelBase> repository_User = new RepositoryFactory<BBdbR_QualityCheckModelBase>();
        #endregion

        #region 1.��ȡ������Ҫ�޸�sql���
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===����ʱ��Ҫ�޸�===
            strSql.Append(@"select    
                                    '10' AS keys,
                                    '10'AS code,
                                    '�ʿص�' AS name,
                                    '1' As IsAvailable,
                                    '0' AS parentId,
                                    '1' as sort");
            strSql.Append(@" union  select    
                             QualityCheckPointId AS keys,
                             QualityCheckPointCd AS code,
                             QualityCheckPointNm AS name,
                             '1' As IsAvailable,
                             '10' AS parentId,
                             '1' as sort 
                          from BBdbR_QualityCheckPointBase where Enabled = '1'");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion
        #region 2.�����չʾ�����Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ��AVI  
        /// </summary>
        /// <param name="areaId">����Ľڵ������</param>
        /// <param name="parentId">�ڵ�ĸ�������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public List<BBdbR_QualityCheckModelBase> GetList(string areaId, string parentId, ref JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            List<BBdbR_QualityCheckModelBase> listEntity = new List<BBdbR_QualityCheckModelBase>();
            string sql = "";
            if (parentId != "0")
            {
                //�ӱ����в�ѯ�ϼ��������봫��������ͬ��ȵ����ݣ��������б�
                sql = "select * from " + tableName + " where QualityCheckPointId ='" + areaId + "' and Enabled = '1'";     //===����ʱ��Ҫ�޸�===
                listEntity = Repository().FindListPageBySql(sql, ref jqgridparam); ; //ִ��sql���
                for (int i = 0; i < listEntity.Count; i++)
                {
                    string sql1 = "select * from BBdbR_QualityCheckPointBase where QualityCheckPointId='" + listEntity[i].QualityCheckPointId + "'";
                    DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        listEntity[i].QualityCheckPointId = dt1.Rows[0]["QualityCheckPointNm"].ToString();
                    }
                }
            }
            else
            {
                sql = "select * from " + tableName + " where 1 = 1 and Enabled = '1'";     //===����ʱ��Ҫ�޸�===
                listEntity = Repository().FindListPageBySql(sql, ref jqgridparam); //ִ��sql���
                for (int i = 0; i < listEntity.Count; i++)
                {
                    string sql1 = "select * from BBdbR_QualityCheckPointBase where QualityCheckPointId='" + listEntity[i].QualityCheckPointId + "'";
                    DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        listEntity[i].QualityCheckPointId = dt1.Rows[0]["QualityCheckPointNm"].ToString();
                    }
                }
            }
            return listEntity;
        }
        #endregion
        #region 1.չʾ���
        /// <summary>
        /// �������������IsAvailable = true������
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>����������������</returns>
        public List<BBdbR_QualityCheckModelBase> GetPageList(JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = @"select * from " + tableName + " where Enabled = '1'";
            return Repository().FindListPageBySql(sql, ref jqgridparam);
        }
        #endregion
        #region 2.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_QualityCheckModelBase entity) //===����ʱ��Ҫ�޸�===
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
        public int Insert(BBdbR_QualityCheckModelBase entity) //===����ʱ��Ҫ�޸�===
        {
            
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
            List<BBdbR_QualityCheckModelBase> listEntity = new List<BBdbR_QualityCheckModelBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_QualityCheckModelBase entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                entity.Modify(entity.QualityCheckModelId);
                listEntity.Add(entity);
            }

            return Repository().Update(listEntity);//��ɾ���ݿ�
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
        public List<BBdbR_QualityCheckModelBase> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            List<BBdbR_QualityCheckModelBase> dt;
            if (Condition == "all")
            {
                sql = @"select * from " + tableName + " where 1 = 1 and Enabled = '1'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            else
            {
                //����������ѯ
                sql = @"select * from " + tableName + " where 1 = 1 and Enabled = '1' and " + Condition + " like  '%" + keywords + "%'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            return dt;
        }
        #endregion

        //#region 7.ҳ����
        ///// <summary>
        ///// ���ϲ�ѯ��չʾҳ����
        ///// </summary>
        ///// <param name="CheckId"></param>
        ///// <returns></returns>
        //public List<BBdbR_MatBase> GetPlanList()
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append(@"SELECT  * FROM  BBdbR_MatBase where Enabled = '1' and MatCatg = '��Ʒ'");
        //    List<BBdbR_MatBase> dt = Repository().FindListBySql(strSql.ToString());
        //    return dt;
        //}
        ///// <summary>
        ///// ���ϲ�ѯ��չʾҳ����
        ///// </summary>
        ///// <param name="CheckId"></param>
        ///// <returns></returns>
        //public BBdbR_QualityCheckModelBase GetPlanList1(string StfId)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append(@"SELECT  * FROM  BBdbR_QualityCheckModelBase where 1=1 and StfId='" + StfId + "'");
        //    List<BBdbR_QualityCheckModelBase> dt = Repository().FindListBySql(strSql.ToString());
        //    return dt[0];
        //}
        //#endregion


    }
}