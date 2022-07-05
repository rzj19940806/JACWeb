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
    /// ȱ����ϸ��Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.10.09 19:51</date>
    /// </author>
    /// </summary>
    public class BBdbR_DefectCatgDeTailBll : RepositoryFactory<BBdbR_DefectCatgDeTail>
    {
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_DefectCatgDeTail";//===����ʱ��Ҫ�޸�===
        #endregion

        #region ������
        #region 1.��������µ���ϸ
        /// <summary>
        /// ����ȱ������������ϸ����
        /// </summary>
        /// <returns></returns>
        public int GetDetailCount(string KeyValue) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (KeyValue != "")
            {
                sql = @"select * from BBdbR_DefectCatgDeTail where Enabled='1' and DefectCatgId='" + KeyValue + "'";
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
        public int GetDetailCountbygroup(string KeyValue) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (KeyValue != "")
            {
                sql = @"select * from BBdbR_DefectCatgDeTail where Enabled='1' and DefectCatgGroupId='" + KeyValue + "'";
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

        #region 2.��ȡ������Ҫ�޸�sql���
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===����ʱ��Ҫ�޸�===
            strSql.Append(@"select DefectCatgId as keys,
                                   DefectCatgCd as code,
                                   DefectCatgNm as name,
                                   '0'as parentId,
                                    '0'as sort 
                            from BBdbR_DefectCatgBase 
                            where Enabled=1  ");
            strSql.Append(@" union select DefectCatgGroupId as keys, 
                                          DefectCatgGroupCd as code,
                                          DefectCatgGroupNm as name,
                                          BBdbR_DefectCatgBase.DefectCatgId as parentId,
                                          '1' as sort 
                                   from BBdbR_DefectCatgGroupBase,BBdbR_DefectCatgBase 
                                    where BBdbR_DefectCatgBase.DefectCatgId=BBdbR_DefectCatgGroupBase.DefectCatgId and BBdbR_DefectCatgGroupBase.Enabled=1 order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 3.�����չʾ�����Ҫ�޸�sql���
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
            if (sort == "0")
            {               
                strSql.Append(@"select * from BBdbR_DefectCatgDeTail where  DefectCatgId='" + areaId + "' and Enabled=1 order by DefectCd asc");
            }
            else
            {
                strSql.Append(@"select * from BBdbR_DefectCatgDeTail where  DefectCatgGroupId='" + areaId + "' and Enabled=1 order by DefectCd asc");
            }
            
            return Repository().FindTableBySql(strSql.ToString(), false);
        }
        #endregion

        #region 4.��ѯ����
        public List<BBdbR_DefectCatgDeTail> GetGridList(string Condition, string keywords, string Nodeareaid,string treesort, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (treesort == "0")//�����ʱѡ�е����ڵ�Ϊȱ�����
            {
                if (Condition != "all")
                {
                    sql = @"Select * from BBdbR_DefectCatgDeTail where Enabled=1 and " + Condition + " like '%" + keywords + "%' and DefectCatgId='" + Nodeareaid + "'";
                }
                else
                {
                    sql = @"Select * from BBdbR_DefectCatgDeTail where Enabled=1 and  DefectCatgId='" + Nodeareaid + "'";
                }
            }
            else//�����ʱѡ�е����ڵ�Ϊ������
            {
                if (Condition != "all")
                {
                    sql = @"Select * from BBdbR_DefectCatgDeTail where Enabled=1 and " + Condition + " like '%" + keywords + "%' and DefectCatgGroupId='" + Nodeareaid + "'";
                }
                else
                {
                    sql = @"Select * from BBdbR_DefectCatgDeTail where Enabled=1 and  DefectCatgGroupId='" + Nodeareaid + "'";
                }
            }
            return Repository().FindListBySql(sql);
        }
        #endregion

        #region 5.���ĳ���ֶε��ֶ�ֵ�Ƿ����
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

        #region 6.��������

        public string GetCatgId(string catggroupid)
        {
            string sql = @"select DefectCatgId from BBdbR_DefectCatgGroupBase where Enabled=1 and DefectCatgGroupId='" + catggroupid + "'";
            DataTable dt = Repository().FindTableBySql(sql,false);
            var defectCatgId = dt.Rows[0][0].ToString();
            return defectCatgId;
        }
        public int Insert(BBdbR_DefectCatgDeTail entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Insert(entity);
        }
        #endregion

        #region 7.�༭����
        public int Update(BBdbR_DefectCatgDeTail entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity);
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
            List<BBdbR_DefectCatgDeTail> listEntity = new List<BBdbR_DefectCatgDeTail>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_DefectCatgDeTail entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
        }
        #endregion

        #endregion
    }
}