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
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// ȼ�ͱ�ʶ��Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.14 17:00</date>
    /// </author>
    /// </summary>
    public class AUEX_NEW_FUELOIL_IDBll : RepositoryFactory<AUEX_NEW_FUELOIL_ID>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "AUEX_NEW_FUELOIL_ID";//===����ʱ��Ҫ�޸�===
        public readonly RepositoryFactory<Base_User> repository_User = new RepositoryFactory<Base_User>();
        #endregion

        #region 1.չʾ���
        /// <summary>
        /// �������������Enabled = 1������
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>����������������</returns>
        public List<AUEX_NEW_FUELOIL_ID> GetPageList(JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = @"Select * From " + tableName + "  Where Enabled = '1' order by OPENING_DATE desc";
            List<AUEX_NEW_FUELOIL_ID> list = Repository().FindListBySql(sql);
            return list;
        }
        #endregion

        #region 2.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(AUEX_NEW_FUELOIL_ID entity) //===����ʱ��Ҫ�޸�===
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
        public int Insert(AUEX_NEW_FUELOIL_ID entity) //===����ʱ��Ҫ�޸�===
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
        public  int Delete(string[] array)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<AUEX_NEW_FUELOIL_ID> listEntity = new List<AUEX_NEW_FUELOIL_ID>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                AUEX_NEW_FUELOIL_ID entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//�������ݿ�
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
        public List<AUEX_NEW_FUELOIL_ID> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            List<AUEX_NEW_FUELOIL_ID> dt;
            if (Condition == "all")
            {
                sql = @"select * from " + tableName + " where Enabled = 1 order by OPENING_DATE desc";
                dt = Repository().FindListBySql(sql.ToString());
            }
            else
            {
                //����������ѯ
                sql = @"select * from " + tableName + " where 1 = Enabled and " + Condition + " like  '%" + keywords + "%' order by OPENING_DATE desc";
                dt = Repository().FindListBySql(sql.ToString());
            }
            return dt;
        }
        #endregion
    }
}