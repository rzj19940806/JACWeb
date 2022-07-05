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
using System.Web.Mvc;

namespace HfutIE.Business
{
    /// <summary>
    /// BBdbR_DecodeRule
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.14 19:38</date>
    /// </author>
    /// </summary>
    public class BBdbR_DecodeRuleBll : RepositoryFactory<BBdbR_DecodeRule>
    {
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_DecodeRule";//===����ʱ��Ҫ�޸�===
        #endregion
        #region 1.չʾ���
        /// <summary>
        /// ���AVIվ�������Ϣ����ѯAVIȥ��������Ϣ��
        /// ��ѯ��ʱ�򴫵���keywords
        /// </summary>
        /// <param name="keywords">��ѯֵ</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetConfigList(string keywords, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select * from " + tableName + "  where BarId='" + keywords + "' and Enabled=1 order by "+ jqgridparam.sidx +" "+jqgridparam.sord;
                return (Repository().FindTableBySql(sql.ToString(), false));
            }
            else
            {
                return null;
            }
        }

        #endregion
        #region 2.��������
        public int Insert(BBdbR_DecodeRule entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Insert(entity);
        }

        public int CheckCount(string Name, string Value)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and BarId='" + Name + "' and CombineNm = '" + Value + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion
        #region 3.�༭����
        public int Update(BBdbR_DecodeRule entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
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
            List<BBdbR_DecodeRule> listEntity = new List<BBdbR_DecodeRule>(); //===����ʱ��Ҫ�޸�===
            foreach (string DecodeRulesId in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_DecodeRule entity = Repository().FindEntity(DecodeRulesId);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
        }
        #endregion
        #region 5.��ѯ��������Ҫ�޸�sql���
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
        public List<BBdbR_DecodeRule> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            List<BBdbR_DecodeRule> dt;
            if (Condition == "all")
            {
                sql = @"select * from BBdbR_BarCodeBase where 1 = 1 and Enabled = '1'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            else
            {
                //����������ѯ
                sql = @"select * from BBdbR_BarCodeBase where 1 = 1 and Enabled = '1' and " + Condition + " like  '%" + keywords + "%'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            return dt;
        }
        #endregion
    }
}