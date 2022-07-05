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
    /// ��˾������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.25 10:18</date>
    /// </author>
    /// </summary>
    public class BBdbR_CompanyBaseBll : RepositoryFactory<BBdbR_CompanyBase>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_CompanyBase";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 1.��ȡ������Ҫ�޸�sql���
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===����ʱ��Ҫ�޸�===

            strSql.Append(@"select    
                             '10' AS keys,
                             '10'AS code,
                             '����ε�����ó����칫˾' AS name,
                             '1' As IsAvailable,
                             '0' AS parentId,
                             '1' as sort 
                          from BBdbR_FacBase where 1=1");
            strSql.Append(@"union select  FacId AS keys,     
                             FacCd AS code,
                             FacNm AS name,
                             Enabled As IsAvailable,
                             '10' as parentId,  
                             '1' as sort    
                         from BBdbR_FacBase where Enabled = '1' ");
            strSql.Append(@" union select    
                             a.WorkshopId AS keys,
                             a.WorkshopCd AS code,
                             a.WorkshopNm AS name,
                             a.Enabled As IsAvailable,
                             a.FacId AS parentId,
                              '1' as sort 
                          from  BBdbR_WorkshopBase a,BBdbR_FacBase b 
                          where a.FacId=b.FacId and a.Enabled = '1' order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_CompanyBase entity) //===����ʱ��Ҫ�޸�===
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
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + " = '" + KeyValue + "' ";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        public int CheckCount2(string KeyName, string KeyValue, string KeyName2, string KeyValue2)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + " = '" + KeyValue + "' and " + KeyName2 + " = '" + KeyValue2 + "'";
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
        public int Insert(BBdbR_CompanyBase entity) //===����ʱ��Ҫ�޸�===
        {

            return Repository().Insert(entity);
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
            List<BBdbR_CompanyBase> listEntity = new List<BBdbR_CompanyBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_CompanyBase entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Modify(entity.CompanyId);
                entity.Enabled = "0";
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
        public List<BBdbR_CompanyBase> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            List<BBdbR_CompanyBase> dt;
            string sql1 = "select * from BBdbR_CompanyBase where Enabled = '1'";
            DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
            if (Condition == "")
            {
                sql = @"select * from " + tableName + " where  Enabled = '1' order by sort asc";
                dt = Repository().FindListBySql(sql.ToString());
            }
            else
            {
                //����������ѯ
                sql = @"select * from " + tableName + " where  Enabled = '1' and " + Condition + " like  '%" + keywords + "%' order by sort asc";
                dt = Repository().FindListBySql(sql.ToString());
            }
            return dt;
        }
        #endregion

        #region 7.�༭��������
        /// <summary>
        /// �༭����������Դ
        /// </summary>
        /// <returns></returns>
        public BBdbR_CompanyBase GetPlanList1(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_CompanyBase where  CompanyId='" + KeyValue + "' and Enabled = '1'");
            List<BBdbR_CompanyBase> dt = Repository().FindListBySql(strSql.ToString());
            BBdbR_CompanyBase BBdbR_CompanyBase1 = new BBdbR_CompanyBase();
          
            BBdbR_CompanyBase1 = dt[0];
            return BBdbR_CompanyBase1;
        }
        #endregion

        #region 9.��ȡ��˾������

        public DataTable GetCompanyList()
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();

            strSql.Append(@"SELECT  CompanyId, CompanyCd,	CompanyNm				
                                                			
                                      FROM    BBdbR_CompanyBase where Enabled = '1' ");
            return dt = Repository().FindTableBySql(strSql.ToString(), false);
        }

        #endregion
    }
}