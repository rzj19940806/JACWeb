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
    /// ȱ����ϸ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.31 16:55</date>
    /// </author>
    /// </summary>
    public class BBdbR_DefectCatgDeTailBll : RepositoryFactory<BBdbR_DefectCatgDeTail>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_DefectCatgDeTail";//===����ʱ��Ҫ�޸�===

        //�����ӱ����
        //string subTableName = "";//===����ʱ��Ҫ�޸�===

        //�����ϼ������
        string superTableName = "BBdbR_DefectCatgBase";//===����ʱ��Ҫ�޸�===

        //�����ϼ�������
        string superID = "DefectCatgId";

        //�����ϼ�������
        string superName = "DefectCatgNm";
        #endregion

        #region 1.��ȡ������Ҫ�޸�sql���
        /// <summary>
        ///     keys-------------���ڵ�����
        ///     code-------------���ڵ���
        ///     name-------------���ڵ�����
        ///     myId-------------���ڵ����
        ///     parentId---------���ڵ����
        ///     sort-------------�ڵ�㼶
        /// </summary>
        /// <returns></returns>
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===����ʱ��Ҫ�޸�===
            strSql.Append(@"select  DefectCatgId AS keys,     
                        DefectCatgCd AS code,
                        DefectCatgNm AS name,
                        Enabled As Enabled,
                        DefectCatgId As myId,
                        '0' as parentId,  
                        '0' as sort    
                        from BBdbR_DefectCatgBase where Enabled = '1' ");
            return Repository().FindTableBySql(strSql.ToString(),false);
        }
        #endregion

        #region 2.�����չʾ�����Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ����ȱ�����   --����-->   ��ȱ����ϸ��
        ///     ���ݴ���Ĳ�����ͬд��ͬ��sql�����в�ѯ           
        /// </summary>
        /// <param name="areaId">����Ľڵ������</param>
        /// <param name="sort">����Ľڵ�Ĳ㼶</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public List<BBdbR_DefectCatgDeTail> GetList(string areaId, string sort, ref JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            List<BBdbR_DefectCatgDeTail> listEntity = new List<BBdbR_DefectCatgDeTail>();//===����ʱ��Ҫ�޸�===
            string sql = "";
            if (sort == "30")
            {
                //�ӱ����в�ѯ�ϼ��������봫��������ͬ��ȵ����ݣ��������б�
                sql = "select * from " + tableName + " where " + superID + " = " + areaId + " and Enabled = '1' ";     //===����ʱ��Ҫ�޸�===
                listEntity = Repository().FindList(sql); //ִ��sql���
            }
            return Repository().FindListPageBySql(sql, ref jqgridparam);
        }
        #endregion

        #region 3.չʾ���
        /// <summary>
        /// �������������IsAvailable = true������
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>����������������</returns>
        public List<BBdbR_DefectCatgDeTail> GetPageList(JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            return Repository().FindList("Enabled", "1");
        }
        #endregion

        #region 4.���ĳ���ֶε��ֶ�ֵ�Ƿ����
        /// <summary>
        ///   ������IsAvailable = true��������ĳ���ֶΣ�KeyName�����ֶ�ֵ��KeyValue���Ƿ����
        /// </summary>
        /// <param name="KeyName">�ֶ���</param>
        /// <param name="KeyValue">�ֶ�ֵ</param>
        /// <returns>���ص��������ı��а������ֶ�ֵ�ļ�¼����</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + " = " + KeyValue + "";
            int count = Repository().FindCountBySql(sql);
            return count;
        }
        #endregion

        #region 5.��ѯ��������Ҫ�޸�sql���
        //��ѯʱ�ṩ�������ؼ��֣�һ����Condition����һ����keywords
        //Condition�ǹؼ��֣�����ѯ��������Ӧ���ݿ��е�һ���ֶ�
        //keywords�ǲ�ѯֵ������ѯ�����ľ���ֵ����Ӧ���ݿ��в�ѯ�����ֶε�ֵ
        //���ݲ�ͬ�Ĺؼ���(Condition)�����ַ���ƴ�ӵķ�ʽƴ�Ӳ�ͬ��sql��䣬���н��Ʋ�ѯ��like����

        //�÷����ڲ�ѯ��չʾ������ݵ�ʱ��ʹ��
        //��ѯ��ʱ�򴫵���Condition��keywords��չʾ���ʱδ���ݲ���
        public List<BBdbR_DefectCatgDeTail> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (Condition == "all")
            {
                sql = @"select * from " + tableName + " where IsAvailable = 'true'";
                return Repository().FindListBySql(sql);
            }
            else
            {
                //����������ѯ
                sql = @"select * from " + tableName + " where Enabled = '1' and " + Condition + " like  '%" + keywords + "%'";
                return Repository().FindListPageBySql(sql.ToString(), ref jqgridparam);
            }


        }
        #endregion

        #region 6.�༭����

        #endregion

        #region 7.��������

        #endregion

        #region 8.ɾ��ʱʹ�ã����ұ�����ĳһ�������¼������Ƿ��������

        #endregion

        #region 9.ɾ������

        #endregion

    }
}