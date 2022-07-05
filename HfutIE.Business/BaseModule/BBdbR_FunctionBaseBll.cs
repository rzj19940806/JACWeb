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
    /// ����ģ�������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.10.09 19:50</date>
    /// </author>
    /// </summary>
    public class BBdbR_FunctionBaseBll : RepositoryFactory<BBdbR_FunctionBase>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_FunctionBase";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 1.չʾ���
        /// <summary>
        /// �������������IsAvailable = true������
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>����������������</returns>
        public List<BBdbR_FunctionBase> GetPageList(JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = @"select * from " + tableName + " where Enable = '1'";
            return Repository().FindListPageBySql(sql, ref jqgridparam);
        }
        #endregion
        #region 2.���ĳ���ֶε��ֶ�ֵ�Ƿ����
        /// <summary>
        ///   Enabled = 1��������ĳ���ֶΣ�KeyName�����ֶ�ֵ��KeyValue���Ƿ����
        /// </summary>
        /// <param name="KeyName">�ֶ���</param>
        /// <param name="KeyValue">�ֶ�ֵ</param>
        /// <returns>���ص��������ı��а������ֶ�ֵ�ļ�¼����</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enable = '1' and  " + KeyName + "  = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion

        #region 3.��������
        public int Insert(BBdbR_FunctionBase entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Insert(entity);
        }
        #endregion

        #region 4.�༭����
        public int Update(BBdbR_FunctionBase entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity);
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
            List<BBdbR_FunctionBase> listEntity = new List<BBdbR_FunctionBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_FunctionBase entity = FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enable = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
        }
        #endregion

        #region 6.�༭�������
        /// <summary>
        /// ���AVIվ�������Ϣ����ѯAVIȥ��������Ϣ��
        /// ��ѯ��ʱ�򴫵���keywords
        /// </summary>
        /// <param name="keywords">��ѯֵ</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public BBdbR_FunctionBase SetConfigInfor(string KeyValue) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            BBdbR_FunctionBase entity = new BBdbR_FunctionBase();
            sql = @"select * from " + tableName + " where Enable=1 and FunctionId='" + KeyValue + "'";
            DataTable dt = Repository().FindTableBySql(sql.ToString(), false);
            if (dt.Rows.Count > 0)
            {
                entity.FunctionId = dt.Rows[0]["FunctionId"].ToString();
                entity.FunctionCd = dt.Rows[0]["FunctionCd"].ToString();
                entity.FunctionNm = dt.Rows[0]["FunctionNm"].ToString();
                entity.FunctionType = int.Parse(dt.Rows[0]["FunctionType"].ToString());
                entity.FunctionDsc = dt.Rows[0]["FunctionDsc"].ToString();
                entity.Rem = dt.Rows[0]["Rem"].ToString();
            }
            return entity;
        }

        #endregion
        #region 7.����Ҫ�༭����
        public BBdbR_FunctionBase FindEntity(string KeyValue)
        {
            string sql = "";
            BBdbR_FunctionBase entity = new BBdbR_FunctionBase();
            sql = @"select * from " + tableName + " where Enable=1 and FunctionId='" + KeyValue + "'";
            return Repository().FindEntityBySql(sql);
        }
        #endregion
        #region 7.������ѯ
        public DataTable GridPageByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            DataTable dt;
            if (Condition=="all")
            {
                 sql = @"select * from " + tableName + " where Enable = '1'";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            else
            {
                //����������ѯ
                sql = @"select * from " + tableName + " where Enable = '1' and " + Condition + " like  '%" + keywords + "%'";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            return dt;
        }
        #endregion
    }
}