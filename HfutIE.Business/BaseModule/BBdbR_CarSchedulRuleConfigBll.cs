//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// ������ȹ������ñ�
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.06 21:50</date>
    /// </author>
    /// </summary>
    public class BBdbR_CarSchedulRuleConfigBll : RepositoryFactory<BBdbR_CarSchedulRuleConfig>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_CarSchedulRuleConfig";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 1.���ó�����ȹ���
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="ClassId">��ѯֵ</param>
        /// <returns></returns>
        public List<BBdbR_CarSchedulRuleConfig> GetClassList(string AVIId, string SchedulRuleId) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (AVIId != "")
            {
                if (SchedulRuleId == "")
                {
                    //����������ѯ
                    sql = @"select * from " + tableName + " where  AVIId='" + AVIId + "' and Enabled=1";
                    return Repository().FindListBySql(sql.ToString());
                }
                else
                {
                    //����������ѯ
                    sql = @"select * from " + tableName + " where  AVIId='" + AVIId + "' and SchedulRuleId= '" + SchedulRuleId + "' and Enabled=1";
                    return Repository().FindListBySql(sql.ToString());
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 2.ɾ��
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(List<BBdbR_CarSchedulRuleConfig> ClassConfigList)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_CarSchedulRuleConfig> listEntity = new List<BBdbR_CarSchedulRuleConfig>(); //===����ʱ��Ҫ�޸�===
            for (int i = 0; i < ClassConfigList.Count; i++)
            {
                ClassConfigList[i].Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(ClassConfigList[i]);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
        }
        #endregion

        #region 3.��������
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
        public int Insert(string AviId, string SchedulRuleId) //===����ʱ��Ҫ�޸�===
        {
            StringBuilder strSql = new StringBuilder();
            int Isok = 0;
            if (SchedulRuleId != "")
            {
                BBdbR_CarSchedulRuleConfig Classentity = new BBdbR_CarSchedulRuleConfig();
                Classentity.AviId = AviId;
                Classentity.SchedulRuleId = SchedulRuleId;
                Classentity.Create();
                Repository().Insert(Classentity);
                Isok = 1;
            }
            return Isok;
        }
        #endregion
    }
}