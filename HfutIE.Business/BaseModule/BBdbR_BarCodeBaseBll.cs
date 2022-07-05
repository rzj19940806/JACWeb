//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// BBdbR_BarCodeBase
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.13 14:41</date>
    /// </author>
    /// </summary>
    public class BBdbR_BarCodeBaseBll : RepositoryFactory<BBdbR_BarCodeBase>
    {
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_BarCodeBase";//===����ʱ��Ҫ�޸�===
        #endregion #region 1.ҳ����
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public List<BBdbR_BarCodeBase> GetPlanList(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            List<BBdbR_BarCodeBase> dt;
            if (KeyValue == "")
            {
                strSql.Append(@"SELECT  * FROM  " + tableName + " where Enabled=1 ");
                dt = Repository().FindListBySql(strSql.ToString());
            }
            else
            {
                strSql.Append(@"SELECT  * FROM  " + tableName + " where Enabled=1 and BarId='" + KeyValue + "'");
                dt = Repository().FindListBySql(strSql.ToString());
            }

            return dt;
        }
        #region 2.չʾ���
        /// <summary>
        /// �������������Enabled = true������, ��Ϊ��Ч�Ĺ�����Ϣ
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>����������������</returns>
        public List<BBdbR_BarCodeBase> GetPageList(JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            return Repository().FindList("Enabled", "1");
        }
        #endregion

        #region 3.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_BarCodeBase entity) //===����ʱ��Ҫ�޸�===
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
            string sql = @"select * from " + tableName + " where Enabled = '1' and'" + KeyName + "' = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion
        #region 5.��������
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
   
        #endregion
        #region 6.ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(string[] array)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_BarCodeBase> listEntity = new List<BBdbR_BarCodeBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_BarCodeBase entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
        }
        #endregion
        #region 7.��ѯ��������Ҫ�޸�sql���
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
        public List<BBdbR_BarCodeBase> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            List<BBdbR_BarCodeBase> dt;
            if (Condition == "all")
            {
                sql = @"select * from " + tableName + " where Enabled = '1'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            else
            {
                //����������ѯ
                sql = @"select * from " + tableName + " where Enabled = '1' and " + Condition + " like  '%" + keywords + "%'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            return dt;
        }
        #endregion

        public DataTable CodeWcList(string BarId)
        {
            return Repository().FindTableBySql($"select a.WcId,a.WcCd,a.WcNm,C.CodeId from BBdbR_WcBase A with(nolock) join BBdbR_PlineBase B  with(nolock) on A.PlineId = B.PlineId and (B.PlineTyp = '��������' OR B.PlineTyp='��������' OR B.PlineTyp='������') Left join BBdbR_CodeWcConfig C with(nolock) on A.WcId = C.WcId and C.CodeId='{BarId}' order by CodeId desc,B.Sort, A.Sort");
        }
        public int SubmitCodeWcList(string CodeId, string[] arrayObjectId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append($"DELETE FROM BBdbR_CodeWcConfig WHERE CodeId = '{CodeId}';");

                foreach (string item in arrayObjectId)
                {
                    sbDelete.Append($"insert BBdbR_CodeWcConfig(Id, CodeId, WcId) values(NEWID(),'{CodeId}','{item}');");
                }
                database.ExecuteBySql(sbDelete);
                return 1;
            }
            catch(Exception ex)
            {
                database.Rollback();
                return -1;
            }
        }
    }
}