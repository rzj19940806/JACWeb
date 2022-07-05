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
    /// ��λ����������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.10.13 20:57</date>
    /// </author>
    /// </summary>
    public class BBdbR_PointFunctionConfigBaseBll : RepositoryFactory<BBdbR_PointFunctionConfigBase>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_PointFunctionConfigBase";//===����ʱ��Ҫ�޸�===
        #endregion

        #region ������

        #region 1.ҳ����
        /// <summary>
        /// չʾҳ����
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public DataTable GetPlanList0(string KeyValue, JqGridParam jqgridparam)//�������±�
        {
            StringBuilder sql = new StringBuilder();
            DataTable dt = new DataTable();
            sql.Append(@"SELECT a.*,b.FunctionCd,b.FunctionNm FROM  " + tableName + " a join BBdbR_FunctionBase b on a.FunctionId=b.FunctionId where a.Enable=1 and a.PointId='"+KeyValue+"' ");
            dt = Repository().FindTableBySql(sql.ToString(), false);
            return dt;
        }
        #endregion

        #region ���ܻ�����Ϣ���ϱ�
        public DataTable GetPlanList()//���ܻ�����Ϣ���ϱ�
        {
            StringBuilder sql1 = new StringBuilder();
            StringBuilder sql2 = new StringBuilder();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            sql1.Append(@"SELECT AviId AS PointId,'1' as PointCatg,AviCd AS PointCd,AviNm AS PointNm FROM  BBdbR_AVIBase where Enabled=1 ");
            dt1 = Repository().FindTableBySql(sql1.ToString(), false);
            sql2.Append(@"SELECT WcId AS PointId,'2' as PointCatg,WcCd AS PointCd,WcNm AS PointNm FROM  BBdbR_WcBase where Enabled=1 ");
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

        #region 2.AVIȥ��������Ϣ���-δ����
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
            sql = @"select FunctionId,FunctionCd,FunctionNm,FunctionType,Enable from BBdbR_FunctionBase where Enable=1 and FunctionId not in (select distinct(FunctionId) from BBdbR_PointFunctionConfigBase where Enable=1 and PointId='" + keywords + "')";
            return (Repository().FindTableBySql(sql.ToString(), false));
        }
        #endregion

        #region 2.AVIȥ��������Ϣ���-������
        /// <summary>
        /// ���AVIվ�������Ϣ����ѯAVIȥ��������Ϣ��
        /// ��ѯ��ʱ�򴫵���keywords
        /// </summary>
        /// <param name="keywords">��λ����PointId</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetConfigList(string keywords, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select a.FunctionId,a.Enable,b.FunctionCd,b.FunctionNm,b.FunctionType from "+tableName+ " a join BBdbR_FunctionBase b on a.FunctionId=b.FunctionId where a.Enable=1 and a.PointId='" + keywords + "'";
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
        public List<BBdbR_PointFunctionConfigBase> GetClassList(string PointId, string FunctionId) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (PointId != "")
            {
                if (FunctionId == "")
                {
                    //����������ѯ
                    sql = @"select * from " + tableName + " where  PointId='" + PointId + "' and Enable=1";
                    return Repository().FindListBySql(sql.ToString());
                }
                else
                {
                    //����������ѯ
                    sql = @"select * from " + tableName + " where  PointId='" + PointId + "' and FunctionId= '" + FunctionId + "' and Enable=1";
                    return Repository().FindListBySql(sql.ToString());
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 4.���ñ�ɾ��
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(List<BBdbR_PointFunctionConfigBase> ClassConfigList)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_PointFunctionConfigBase> listEntity = new List<BBdbR_PointFunctionConfigBase>(); //===����ʱ��Ҫ�޸�===
            for (int i = 0; i < ClassConfigList.Count; i++)
            {
                ClassConfigList[i].Enable = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
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
        public int Insert(string PointId, string FunctionId, string PointCatg) //===����ʱ��Ҫ�޸�===
        {
            StringBuilder strSql = new StringBuilder();
            int Isok = 0;
            if (FunctionId != "")
            {
                BBdbR_PointFunctionConfigBase Classentity = new BBdbR_PointFunctionConfigBase();
                Classentity.PointCatg = PointCatg;
                Classentity.PointId = PointId;
                Classentity.FunctionId = FunctionId;
                Classentity.Create();
                Repository().Insert(Classentity);
                Isok = 1;
            }
            return Isok;
        }
        #endregion

        #region 1.��������µ���ϸ
        /// <summary>
        /// ����ȱ������������ϸ����
        /// </summary>
        /// <returns></returns>
        public int GetConfigCount(string KeyValue) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (KeyValue != "")
            {
                sql = @"select * from BBdbR_PointFunctionConfigBase where Enable='1' and FunctionId='" + KeyValue + "'";
                DataTable dt = Repository().FindTableBySql(sql);
                if (dt.Rows.Count > 0)
                {
                    return Repository().FindTableBySql(sql).Rows.Count;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 1;
            }
        }
        #endregion

        #endregion
    }
}
