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

        #region 1.AVI����������Ϣ���-δ�ڸ���
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
            sql = @"select AviId as AVIId,AviCd as AVICd,AviNm as AVINm from BBdbR_AVIBase where Enabled=1 and AviId not in (select ISNULL(AVIId,'') from BBdbR_AVIGroupConfig where Enabled=1 and AVIGroupId='" + keywords + "')";
            return (Repository().FindTableBySql(sql.ToString(), false));
        }
        #endregion

        #region 2.AVI����������Ϣ���-���ڸ���
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
                sql = @"select * from " + tableName + " where AVIId is not null and Enabled=1 and AVIGroupId='" + keywords + "')";
                return (Repository().FindTableBySql(sql.ToString(), false));
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 3.ɾ��
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(string keyvalue)//ɾ������վ��dt.Rows[0]["AVIGroupCd"]
        {
            BBdbR_AVIGroupConfig entity = Repository().FindEntity(keyvalue);
            if (entity.AviId != null )
            {
                StringBuilder deletesql = new StringBuilder();
                deletesql.Append(@"update " + tableName + " set Enabled = 0 where AVIId is not null and AVIGroupId='" + entity.AVIGroupId + "'");
                return Repository().ExecuteBySql(deletesql);
            }
            return 1;
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
        public int Insert(BBdbR_AVIGroupConfig entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Insert(entity);
        }      
        #endregion

        #region 5.����ֶ��Ƿ�Ψһ
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

        #region 6.��ȡAVIվ����������AVIվ��
        /// <summary>
        /// ��ȡAVIվ����������AVIվ��
        /// ��ѯ��ʱ�򴫵���keywords
        /// </summary>
        /// <param name="keywords">��ѯֵ</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetAviGroupConfigList(string keywords, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            sql = @"select a.*,b.AVIGroupCd,b.AVIGroupNm,b.AVIGroupCount from BBdbR_AviGroupConfig a join BBdbR_AviGroupBase b on a.AVIGroupId=b.AVIGroupId where a.Enabled=1 and b.Enabled=1 and a.AVIGroupId='" + keywords + "'";
            return (Repository().FindTableBySql(sql.ToString(), false));
        }
        #endregion

        #endregion
    }
}