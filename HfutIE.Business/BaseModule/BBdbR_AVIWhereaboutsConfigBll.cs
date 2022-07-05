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
    public class BBdbR_AVIWhereaboutsConfigBll : RepositoryFactory<BBdbR_AVIWhereaboutsConfig>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_AVIWhereaboutsConfig";//===����ʱ��Ҫ�޸�===
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
            sql = @"select * from BBdbR_PlineBase where Enabled=1 and PlineId not in (select distinct(PlineId) as PlineId from BBdbR_AVIWhereaboutsConfig where Enabled=1 and AviId='" + keywords + "')";
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
                sql = @"select a.*,b.AVIWhereId as AVIWhereId,b.PlineMark as PlineMark,b.IsIndependence as IsIndependence,b.ToAVISequence as ToAVISequence,b.ToAVIId as ToAVIId,b.ToAVICd as ToAVICd,b.ToAVINm as ToAVINm,c.AviCd as AviCd,c.AviNm as AviNm,b.Rem as Rem0 from BBdbR_PlineBase a join " + tableName + " b on a.PlineId=b.PlineId join BBdbR_AVIBase c on b.AviId=c.AviId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and b.AviId='" + keywords + "'";
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
        public List<BBdbR_AVIWhereaboutsConfig> GetClassList(string AVIId, string PlineId) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (AVIId != "")
            {
                if (PlineId == "")
                {
                    //����������ѯ
                    sql = @"select * from " + tableName + " where  AVIId='" + AVIId + "' and Enabled=1";
                    return Repository().FindListBySql(sql.ToString());
                }
                else
                {
                    //����������ѯ
                    sql = @"select * from " + tableName + " where  AVIId='" + AVIId + "' and PlineId= '" + PlineId + "' and Enabled=1";
                    return Repository().FindListBySql(sql.ToString());
                }
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
        public int Delete(List<BBdbR_AVIWhereaboutsConfig> ClassConfigList)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_AVIWhereaboutsConfig> listEntity = new List<BBdbR_AVIWhereaboutsConfig>(); //===����ʱ��Ҫ�޸�===
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
        public int Insert(string AviId, string PlineId) //===����ʱ��Ҫ�޸�===
        {
            StringBuilder strSql = new StringBuilder();
            int Isok = 0;
            if (PlineId != "")
            {
                string sql = "select PlineId,PlineCd,PlineNm from BBdbR_PlineBase where PlineId='"+ PlineId + "'";
                DataTable Plinedt= Repository().FindTableBySql(sql.ToString());
                string sql1 = "select AVISequence,AviCd as AVICd,AviNm as AVINm from BBdbR_AVIBase where AviId='" + AviId + "'";
                DataTable Avidt = Repository().FindTableBySql(sql1.ToString());
                BBdbR_AVIWhereaboutsConfig Classentity = new BBdbR_AVIWhereaboutsConfig();
                Classentity.AviId = AviId;
                Classentity.PlineId = PlineId;
                Classentity.ToPlineCd = Plinedt.Rows[0]["PlineCd"].ToString();
                Classentity.ToPlineNm = Plinedt.Rows[0]["PlineNm"].ToString();
                if (Avidt.Rows[0]["avisequence"].ToString()!="")
                {
                    Classentity.AVISequence = int.Parse(Avidt.Rows[0]["avisequence"].ToString());
                }
                Classentity.Create();
                Repository().Insert(Classentity);
                Isok = 1;
            }
            return Isok;
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
        public BBdbR_AVIWhereaboutsConfig SetConfigInfor(string KeyValue) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            BBdbR_AVIWhereaboutsConfig entity = new BBdbR_AVIWhereaboutsConfig();
            sql = @"select a.*,b.AviNm as AviNm,c.PlineNm as PlineNm,c.PlineCd as PlineCd from BBdbR_AVIWhereaboutsConfig a join BBdbR_AVIBase b on a.AviId=b.AviId join BBdbR_PlineBase c on a.PlineId=c.PlineId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and a.AVIWhereId='" + KeyValue + "'";
            DataTable dt = Repository().FindTableBySql(sql.ToString(), false);
            if (dt.Rows.Count>0)
            {
                entity.AVIWhereId = dt.Rows[0]["AVIWhereId"].ToString();
                entity.ToAVIId = dt.Rows[0]["ToAVIId"].ToString();
                entity.ToAVICd = dt.Rows[0]["ToAVICd"].ToString();
                entity.ToAVINm = dt.Rows[0]["ToAVINm"].ToString();
                entity.AviId = dt.Rows[0]["AviId"].ToString();
                entity.Rem = dt.Rows[0]["Rem"].ToString();
                if (dt.Rows[0]["IsIndependence"].ToString()!="" )
                {
                    entity.IsIndependence = int.Parse(dt.Rows[0]["IsIndependence"].ToString());
                }
                if (dt.Rows[0]["ToAVISequence"].ToString() != "")
                {
                    entity.ToAVISequence = int.Parse(dt.Rows[0]["ToAVISequence"].ToString());
                }
                entity.PlineId = dt.Rows[0]["PlineId"].ToString();
                entity.ToPlineCd = dt.Rows[0]["PlineCd"].ToString();
                entity.ToPlineNm = dt.Rows[0]["PlineNm"].ToString();
                entity.RsvFld1 = dt.Rows[0]["AviNm"].ToString();
                entity.RsvFld2 = dt.Rows[0]["PlineNm"].ToString();
                entity.PlineMark = dt.Rows[0]["PlineMark"].ToString();
            }
            return entity;
        }

        #endregion

        #region 7.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_AVIWhereaboutsConfig entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
        }
        #endregion

        #region 8.��������Ա
        //11.��ȡ������Ա
        public DataTable GetAviNm()
        {
            string sql = @"select AviId as id, AviNm as avinm from BBdbR_AVIBase where Enabled='1' ";
            return Repository().FindTableBySql(sql.ToString(), false);
        }
        //��ȡ��Ա��Ϣ
        public DataTable GetAviNm2(string ToAVIId)
        {
            string sql = @"select AviCd,AviNm,AVISequence from BBdbR_AVIBase where Enabled='1' and AviId=" + "'" + ToAVIId + "'";
            return Repository().FindTableBySql(sql.ToString(), false);
        }
        /// <summary>
        /// �༭����������Դ
        /// </summary>
        /// <returns></returns>
        //public BBdbR_FacBase GetAviList(string KeyValue)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append(@"SELECT * FROM  BBdbR_FacBase where  FacId='" + KeyValue + "' and Enabled = '1'");
        //    List<BBdbR_FacBase> dt = Repository().FindListBySql(strSql.ToString());
        //    BBdbR_FacBase Dvcentity = new BBdbR_FacBase();
        //    Dvcentity = dt[0];
        //    return Dvcentity;
        //}
        #endregion

        #endregion
    }
}