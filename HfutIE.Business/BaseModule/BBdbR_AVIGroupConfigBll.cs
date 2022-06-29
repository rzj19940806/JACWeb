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
using System.Web.Mvc;

namespace HfutIE.Business
{
    /// <summary>
    /// AVIȥ��������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.31 12:49</date>
    /// </author>
    /// </summary>
    public class BBdbR_AVIGroupConfigBll : RepositoryFactory<BBdbR_AVIGroupConfig>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_AVIGroupConfig";//===����ʱ��Ҫ�޸�===
        //BBdbR_AVIAcitonConfig
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
            StringBuilder strSql = new StringBuilder();
            DataTable dt;
            strSql.Append(@"SELECT * FROM  " + tableName + " where AVIId is NULL and Enabled=1 ");
            dt = Repository().FindTableBySql(strSql.ToString(), false);
            return dt;
        }
        #endregion

        #region 2.AVI����������Ϣ���-δ�ڸ���
        /// <summary>
        /// ���AVIվ�������Ϣ����ѯAVIȥ��������Ϣ��
        /// ��ѯ��ʱ�򴫵���keywords
        /// </summary>
        /// <param name="keywords">��ѯֵ</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable ReGetConfigList(string keywords, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            sql = @"select AviId as AVIId,AviCd as AVICd,AviNm as AVINm from BBdbR_AVIBase where Enabled=1 and AviId not in (select ISNULL(AVIId,'') from BBdbR_AVIGroupConfig where Enabled=1 and AVIGroupCd in (select AVIGroupCd from BBdbR_AVIGroupConfig where Enabled=1 and AVIGroupId='" + keywords + "'))";
            return (Repository().FindTableBySql(sql.ToString(), false));
        }
        #endregion

        #region 3.AVI����������Ϣ���-���ڸ���
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
                sql = @"select * from " + tableName + " where AVIId is not null and Enabled=1 and AVIGroupCd in (select AVIGroupCd from " + tableName + " where AVIGroupId='" + keywords + "')";
                return (Repository().FindTableBySql(sql.ToString(), false));
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 4.ɾ��
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(string keyvalue)//ɾ������վ��dt.Rows[0]["AVIGroupCd"]
        {
            BBdbR_AVIGroupConfig entity = Repository().FindEntity(keyvalue);
            entity.AVIGroupCount = "0";
            Repository().Update(entity);
            StringBuilder deletesql = new StringBuilder();
            deletesql.Append(@"update " + tableName + " set Enabled = 0 where AVIId is not null and AVIGroupCd='" + entity.AVIGroupCd + "'");
            return Repository().ExecuteBySql(deletesql);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
         public int Delete1(string keyvalue)//ɾ������
        {
            DataTable dt = new DataTable();
            string sql = @"select * from "+tableName+" where Enabled=1 and AVIGroupId='"+keyvalue+"'";
            dt = Repository().FindTableBySql(sql);
            StringBuilder deletesql = new StringBuilder();
            deletesql.Append(@"update " + tableName + " set Enabled = 0 where AVIGroupCd='" + dt.Rows[0]["AVIGroupCd"] + "'");
            return Repository().ExecuteBySql(deletesql);
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
        public int Insert(BBdbR_AVIGroupConfig entity,string keyvalue,int i) //===����ʱ��Ҫ�޸�===
        {
            List<BBdbR_AVIGroupConfig> listEntity = new List<BBdbR_AVIGroupConfig>(); //===����ʱ��Ҫ�޸�===
            //i = i + 1;
            //BBdbR_AVIGroupConfig entity1 = Repository().FindEntity(keyvalue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
            //entity1.AVICount = i.ToString();//����ʵ���IsAvailable���Ը�Ϊfalse
            //Repository().Update(entity1);
            return Repository().Insert(entity);
        }
        public int Insert1(BBdbR_AVIGroupConfig entity, int i) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Insert(entity);
        }
        #endregion

        #region 6.����ֶ��Ƿ�Ψһ
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enabled='1' and " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }

        public int CheckCount2(string KeyName, string KeyValue,string KeyName2,string KeyValue2)
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

        #region 7.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_AVIGroupConfig entity,string KeyValue) //===����ʱ��Ҫ�޸�===
        {
            StringBuilder updatesql = new StringBuilder();
            DataTable dt = new DataTable();
            string sql=@"select AVIGroupCd from "+tableName+" where AVIGroupId='"+ KeyValue + "'";
            dt = Repository().FindTableBySql(sql.ToString(), false);
             updatesql.Append(@"update " + tableName + " set AVIGroupCd='" + entity.AVIGroupCd + "',AVIGroupNm='" + entity.AVIGroupNm + "' where AVIGroupCd='" + dt.Rows[0][0] + "' and Enabled=1");
             int a=Repository().ExecuteBySql(updatesql);
            
             return Repository().Update(entity);  //���޸ĺ��ʵ����µ����ݿ���
           
        }
        #endregion

        #region 8.��ѯ��������Ҫ�޸�sql���
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
        public DataTable GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            DataTable dt;
            if (Condition == "all")
            {
                sql = @"SELECT * FROM  " + tableName + " where AVIId is NULL and Enabled=1 ";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            
            else 
            {
                //����������ѯ
                //string sql1 = @"select ";
                sql = @"SELECT * FROM  " + tableName + " where AVIId is NULL and Enabled = '1' and " + Condition + " like  '%" + keywords + "%'";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            return dt;
        }
        #endregion


        #endregion
    }
}