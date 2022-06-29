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
    /// �����������ɹ������ñ�
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.26 15:14</date>
    /// </author>
    /// </summary>
    public class BBdbR_PushRuleBll : RepositoryFactory<BBdbR_PushRule>
    {
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_PushRule";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 1.����
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
        /// <returns></returns>
        public DataTable  GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (Condition == "all")
            {
                sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm,c.WcCd as WcCd,c.WcNm as WcNm from " + tableName + " a left join BBdbR_PlineBase b on a.PlineId=b.PlineId left join BBdbR_WcBase c on a.WcId=c.WcId where a.Enabled=1";
            }
            else
            {
                if (keywords != "all")
                {
                    sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm,c.WcCd as WcCd,c.WcNm as WcNm from " + tableName + " a left join BBdbR_PlineBase b on a.PlineId=b.PlineId left join BBdbR_WcBase c on a.WcId=c.WcId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and a." + Condition + " like  '%" + keywords + "%'";
                    //����������ѯ
                }
                else
                {
                    sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm,c.WcCd as WcCd,c.WcNm as WcNm from " + tableName + " a left join BBdbR_PlineBase b on a.PlineId=b.PlineId left join BBdbR_WcBase c on a.WcId=c.WcId where a.Enabled=1";

                }              
            }
            return Repository().FindTableBySql(sql.ToString(), false);
        }

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
        /// <returns></returns>
        public DataTable GetPageList() //===����ʱ��Ҫ�޸�===
        {
            string sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm,c.WcCd as WcCd,c.WcNm as WcNm from " + tableName + " a left join BBdbR_PlineBase b on a.PlineId=b.PlineId left join BBdbR_WcBase c on a.WcId=c.WcId where a.Enabled=1";
            return Repository().FindTableBySql(sql.ToString(), false);
        }
        #endregion

        #region 2.�༭
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_PushRule entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
        }
        #endregion

        #region 3.���ĳ���ֶε�ֵ�Ƿ����
        /// <summary>
        ///   ������IsAvailable = true��������ĳ���ֶΣ�KeyName�����ֶ�ֵ��KeyValue���Ƿ����
        /// </summary>
        /// <param name="KeyName">�ֶ���</param>
        /// <param name="KeyValue">�ֶ�ֵ</param>
        /// <returns>���ص��������ı��а������ֶ�ֵ�ļ�¼����</returns>
        public int CheckCount(string tableName, string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where  " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion

        #region 4.����
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
        public int Insert(BBdbR_PushRule entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Insert(entity);
        }
        #endregion

        #region 5.����������

        //����
        public DataTable GetPlineNm()
        {
            string sql = @"select PlineId as id, PlineNm from BBdbR_PlineBase where 1=1";
            return Repository().FindTableBySql(sql);
        }

        public DataTable GetWcNm()
        {
            string sql = @"select WcId as id, WcNm from BBdbR_WcBase where 1=1";
            return Repository().FindTableBySql(sql);
        }
        #endregion

        #region 6.ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(string[] array)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_PushRule> listEntity = new List<BBdbR_PushRule>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_PushRule entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
        }
        #endregion
    }
}