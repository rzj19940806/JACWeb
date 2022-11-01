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
    /// ȱ�������������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.10.09 20:06</date>
    /// </author>
    /// </summary>
    public class BBdbR_DefectCatgGroupBase_AddBll : RepositoryFactory<BBdbR_DefectCatgGroupBase_Add>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_DefectCatgGroupBase_Add";//===����ʱ��Ҫ�޸�===
        #endregion

        #region ������
        #region 1.��ȡ������Ҫ�޸�sql���
        public DataTable GetTree(string Type)
        {
            StringBuilder strSql = new StringBuilder();
            if (Type == "CH")//�庸
            {
                //===����ʱ��Ҫ�޸�===
                strSql.Append(@"select DefectCatgId as keys,
                                   DefectCatgCd as code,
                                   DefectCatgNm as name,
                                   '0'as parentId,
                                    '0'as sort 
                            from BBdbR_DefectCatgBase_Add 
                            where Enabled=1 and type='CH' order by code asc ");
            }
            else//Ϳװ
            {        
                //===����ʱ��Ҫ�޸�===
                strSql.Append(@"select DefectCatgId as keys,
                                   DefectCatgCd as code,
                                   DefectCatgNm as name,
                                   '0'as parentId,
                                    '0'as sort 
                            from BBdbR_DefectCatgBase_Add 
                            where Enabled=1 and type='TZ'order by code asc ");

            }
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.�����չʾ�����Ҫ�޸�sql���
        /// <summary>
        ///     ���ݴ���Ĳ�����ͬд��ͬ��sql�����в�ѯ
        ///             
        /// </summary>
        /// <param name="areaId">����Ľڵ������</param>
        /// <param name="sort">����Ľڵ�Ĳ㼶���</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetList(string areaId, string text, string value, string sort, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from BBdbR_DefectCatgGroupBase_Add where  DefectCatgId='" + areaId + "' and Enabled=1 order by DefectCatgGroupCd asc");
            return Repository().FindTableBySql(strSql.ToString(),false);
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
            string sql = @"select * from " + tableName + " where Enabled = '1' and  " + KeyName + "  = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion

        #region 4.��������
        public int Insert(BBdbR_DefectCatgGroupBase_Add entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Insert(entity);
        }
        #endregion

        #region 5.�༭����
        public int Update(BBdbR_DefectCatgGroupBase_Add entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity);
        }
        #endregion

        #region 6.��ѯ����
        public List<BBdbR_DefectCatgGroupBase_Add> GetGridList(string Condition,string  keywords,string  Nodeareaid, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (Condition != "all")
            {
                 sql = @"Select * from BBdbR_DefectCatgGroupBase_Add where Enabled=1 and " + Condition + " like '%" + keywords + "%' and DefectCatgId='" + Nodeareaid + "'";
            }
            else
            {
                sql = @"Select * from BBdbR_DefectCatgGroupBase_Add where Enabled=1 and  DefectCatgId='" + Nodeareaid + "'";
            }
            return Repository().FindListBySql(sql);
        }
        #endregion

        #region 7.ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(string[] array)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_DefectCatgGroupBase_Add> listEntity = new List<BBdbR_DefectCatgGroupBase_Add>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_DefectCatgGroupBase_Add entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
        }
        #endregion
        #endregion
    }
}
