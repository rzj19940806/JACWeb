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
    /// AVIվ�������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.26 10:57</date>
    /// </author>
    /// </summary>
    public class BBdbR_AVIBaseBll : RepositoryFactory<BBdbR_AVIBase>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_AVIBase";//===����ʱ��Ҫ�޸�===
        #endregion

        #region ������

        #region 1.ҳ����
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public DataTable GetPlanList()
        {
            StringBuilder sql1 = new StringBuilder();
            StringBuilder sql2 = new StringBuilder();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            sql1.Append(@"SELECT a.*,b.PlineNm as PlineNm FROM  " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId where a.IsIndependence='0' and a.Enabled=1 ");
            dt1 = Repository().FindTableBySql(sql1.ToString(), false);
            sql2.Append(@"SELECT *,'��' as PlineNm FROM  " + tableName + " where IsIndependence='1' and Enabled=1 ");
            dt2 = Repository().FindTableBySql(sql2.ToString(), false);
            DataTable dt = dt1.Clone();
            object[] obj = new object[dt.Columns.Count];
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                dt1.Rows[i].ItemArray.CopyTo(obj, 0);
                dt.Rows.Add(obj);
            }
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                dt2.Rows[i].ItemArray.CopyTo(obj, 0);
                dt.Rows.Add(obj);
            }
            return dt;
        }
        #endregion

        #region 2.��������
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
        public int Insert(BBdbR_AVIBase entity) //===����ʱ��Ҫ�޸�===
        {
            //entity.Enabled = "1";

            return Repository().Insert(entity);
        }
        #endregion

        #region 3.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_AVIBase entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
        }
        #endregion

        #region 4.���ĳ���ֶε��ֶ�ֵ�Ƿ����
        /// <summary>
        ///   Enabled = 1��������ĳ���ֶΣ�KeyName�����ֶ�ֵ��KeyValue���Ƿ����
        /// </summary>
        /// <param name="KeyName">�ֶ���</param>
        /// <param name="KeyValue">�ֶ�ֵ</param>
        /// <returns>���ص��������ı��а������ֶ�ֵ�ļ�¼����</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + " = '" + KeyValue + "' ";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        public int CheckCount2(string KeyName, string KeyValue, string KeyName2, string KeyValue2)
        {
            string sql = @"select * from " + tableName + " where Enabled='1' and " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            if (a == 0)
            {
                sql = @"select * from " + tableName + " where Enabled='1' and  " + KeyName2 + " = '" + KeyValue2 + "'";
                count = Repository().FindTableBySql(sql);
                if (count.Rows.Count > 0)
                {
                    a = 2;
                    return a;
                }
            }
            return a;
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
            List<BBdbR_AVIBase> listEntity = new List<BBdbR_AVIBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_AVIBase entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
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
        public List<BBdbR_AVIBase> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            List<BBdbR_AVIBase> dt;

            if (Condition == "all" || keywords == "")
            {
                sql = @"select * from " + tableName + " where Enabled = '1' order by AviCd asc";
                dt = Repository().FindListBySql(sql.ToString());
            }
            else
            {
                //����������ѯ
                sql = @"select * from " + tableName + " where Enabled = '1' and " + Condition + " like  '%" + keywords + "%' order by AviCd asc";
                dt = Repository().FindListBySql(sql.ToString());
            }

            return dt;
        }
        #endregion

        #region 7.���������
        //11.��ȡ�������������
        public DataTable GetPlineNm()
        {
            string sql = @"select PlineId as id, PlineNm as plinenm from BBdbR_PlineBase where Enabled='1'";
            return Repository().FindTableBySql(sql);
        }
        #endregion

        #endregion
    }
}