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
    public class BBdbR_AVIActionConfigBll : RepositoryFactory<BBdbR_AVIActionConfig>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_AVIActionConfig";//===����ʱ��Ҫ�޸�===
        //BBdbR_AVIAcitonConfig
        #endregion

        #region ������

        #region 1.AVIȥ��������Ϣ���-δ����
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
            sql = @"select * from BBdbR_PlineBase where Enabled=1 and PlineId not in (select distinct(PlineId) from BBdbR_AVIWhereaboutsConfig where Enabled=1 and AviId='" + keywords + "')";
            return (Repository().FindTableBySql(sql.ToString(), false));
        }
        #endregion

        #region 2.AVIȥ��������Ϣ���-������
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
                sql = @"select a.* from BBdbR_PlineBase a join " + tableName + " b on a.PlineId=b.PlineId where a.Enabled=1 and b.Enabled=1 and b.AviId='" + keywords + "'";
                return (Repository().FindTableBySql(sql.ToString(), false));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ��ѯ�����Ӧ�Ĳ���
        /// ��ѯ��ʱ�򴫵���keywords
        /// </summary>
        /// <param name="keywords">��ѯֵ</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetPageConfigList(string keywords, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select * from " + tableName + " where Enabled=1 and AviCd='" + keywords + "'";
                //sql = @"select a.*,b.AVIWhereId as AVIWhereId,b.PlineMark as PlineMark,c.AviCd as AviCd,c.AviNm as AviNm from BBdbR_PlineBase a join " + tableName + " b on a.PlineId=b.PlineId join BBdbR_AVIBase c on b.AviId=c.AviId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and b.AviId='" + keywords + "'";
                return (Repository().FindTableBySql(sql.ToString(), false));
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 3.����ȥ�����
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="ClassId">��ѯֵ</param>
        /// <returns></returns>
        //public List<BBdbR_AVIWhereaboutsConfig> GetClassList(string AVIId, string PlineId) //===����ʱ��Ҫ�޸�===
        //{
        //    string sql = "";
        //    if (AVIId != "")
        //    {
        //        if (PlineId == "")
        //        {
        //            //����������ѯ
        //            sql = @"select * from " + tableName + " where  AVIId='" + AVIId + "' and Enabled=1";
        //            return Repository().FindListBySql(sql.ToString());
        //        }
        //        else
        //        {
        //            //����������ѯ
        //            sql = @"select * from " + tableName + " where  AVIId='" + AVIId + "' and PlineId= '" + PlineId + "' and Enabled=1";
        //            return Repository().FindListBySql(sql.ToString());
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        #endregion

        #region 4.ɾ��
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(List<BBdbR_AVIActionConfig> ClassConfigList)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_AVIActionConfig> listEntity = new List<BBdbR_AVIActionConfig>(); //===����ʱ��Ҫ�޸�===
            for (int i = 0; i < ClassConfigList.Count; i++)
            {
                ClassConfigList[i].Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(ClassConfigList[i]);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
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
        public int Insert(BBdbR_AVIActionConfig entity) //===����ʱ��Ҫ�޸�===
        {
            //StringBuilder strSql = new StringBuilder();
            //int Isok = 0;
           
                //BBdbR_AVIAcitonConfig Classentity = new BBdbR_AVIAcitonConfig();
                //Classentity.AviCd = AviCd;
                //Classentity.PlineId = AviCd;
                //Classentity.Create();
                
                //Isok = 1;
            
            return Repository().Insert(entity);
        }
        #endregion

        #region 6.AVI���������
        /// <summary>
        /// ���AVIվ�������Ϣ����ѯAVIȥ��������Ϣ��
        /// ��ѯ��ʱ�򴫵���keywords
        /// </summary>
        /// <param name="keywords">��ѯֵ</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public BBdbR_AVIActionConfig SetConfigInfor(string AviActionConfigId) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            BBdbR_AVIActionConfig entity = new BBdbR_AVIActionConfig();
            sql = @"select a.*,b.AviNm as AviNm from " + tableName + " a join BBdbR_AviBase b on a.AviCd=b.AviCd where a.Enabled=1 and a.AviActionConfigId='" + AviActionConfigId + "'";
            DataTable dt = Repository().FindTableBySql(sql.ToString(), false);
            if (dt.Rows.Count>0)
            {
                entity.AviActionConfigId = dt.Rows[0]["AviActionConfigId"].ToString();
                entity.AviCd = dt.Rows[0]["AviCd"].ToString();
                entity.AviCatg = dt.Rows[0]["AviCatg"].ToString();
                entity.AviAction = dt.Rows[0]["AviAction"].ToString();
                entity.AviAddr = dt.Rows[0]["AviAddr"].ToString();
            }
            return entity;
        }

        #endregion

        #region 7.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_AVIActionConfig entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
        }
        #endregion

        #endregion
    }
}