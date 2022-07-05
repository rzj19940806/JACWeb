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
    /// ȱ����������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.10.09 19:50</date>
    /// </author>
    /// </summary>
    public class BBdbR_DefectCatgBaseBll : RepositoryFactory<BBdbR_DefectCatgBase>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_DefectCatgBase";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 1.���ĳ���ֶε��ֶ�ֵ�Ƿ����
        /// <summary>
        ///   Enabled = 1��������ĳ���ֶΣ�KeyName�����ֶ�ֵ��KeyValue���Ƿ����
        /// </summary>
        /// <param name="KeyName">�ֶ���</param>
        /// <param name="KeyValue">�ֶ�ֵ</param>
        /// <returns>���ص��������ı��а������ֶ�ֵ�ļ�¼����</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and  " + KeyName + "  = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }

        #region 2.��������
        public int Insert(BBdbR_DefectCatgBase entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Insert(entity);
        }
        #endregion

        #region 3.�༭����
        public int Update(BBdbR_DefectCatgBase entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity);
        }
        #endregion

        #region 4.ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(string[] array)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_DefectCatgBase> listEntity = new List<BBdbR_DefectCatgBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_DefectCatgBase entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
        }
        #endregion
        #endregion

    }
}