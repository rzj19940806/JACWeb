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

namespace HfutIE.Business
{
    /// <summary>
    /// ���ģ�峵��λ������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.27 10:45</date>
    /// </author>
    /// </summary>
    public class BBdbR_QuaCheckModelCarPartConfigBll : RepositoryFactory<BBdbR_QuaCheckModelCarPartConfig>
    {
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_QuaCheckModelCarPartConfig";//===����ʱ��Ҫ�޸�===
        #endregion
        #region 1.��ѯ�����ó���λ��Ϣ
        /// <summary>
        /// ��ѯ�����ó���λ��Ϣ
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public List<BBdbR_QuaCheckModelCarPartConfig> GetPlanList(string QualityCheckModelId)
        {
            StringBuilder strSql = new StringBuilder();
            List<BBdbR_QuaCheckModelCarPartConfig> dt;
            if (QualityCheckModelId == "")
            {
                strSql.Append(@"SELECT  * FROM  BBdbR_QuaCheckModelCarPartConfig where Enabled=1 ");
                dt = Repository().FindListBySql(strSql.ToString());

            }
            else
            {
                strSql.Append(@"SELECT  * FROM  BBdbR_QuaCheckModelCarPartConfig where Enabled=1 and QualityCheckModelId='" + QualityCheckModelId + "'");
                dt = Repository().FindListBySql(strSql.ToString());

            }

            return dt;
        }

        #endregion

        #region 2.�����ü��ģ�峵��λ������Ϣ
        /// <summary>
        /// �����ü��ģ�峵��λ������Ϣ
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public List<BBdbR_QuaCheckModelCarPartConfig> GetCarPartList(string QualityCheckModelId, string CarPartId)
        {
            StringBuilder strSql = new StringBuilder();
            List<BBdbR_QuaCheckModelCarPartConfig> dt;
            if (QualityCheckModelId != "" && CarPartId != "")
            {
                strSql.Append(@"SELECT  * FROM  BBdbR_QuaCheckModelCarPartConfig where Enabled=1 and QualityCheckModelId='" + QualityCheckModelId + "'and CarPartId='" + CarPartId + "'");
                dt = Repository().FindListBySql(strSql.ToString());
            }
            else
            {
                if (QualityCheckModelId != "")
                {
                    strSql.Append(@"SELECT  * FROM  BBdbR_QuaCheckModelCarPartConfig where Enabled=1 and QualityCheckModelId='" + QualityCheckModelId + "'");
                    dt = Repository().FindListBySql(strSql.ToString());
                }
                else if(CarPartId != "")
                {
                    strSql.Append(@"SELECT  * FROM  BBdbR_QuaCheckModelCarPartConfig where Enabled=1 and CarPartId='" + CarPartId + "'");
                    dt = Repository().FindListBySql(strSql.ToString());
                }
                else
                {
                    strSql.Append(@"SELECT  * FROM  BBdbR_QuaCheckModelCarPartConfig where Enabled=1 ");
                    dt = Repository().FindListBySql(strSql.ToString());
                }
            }
            return dt;
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
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + "  = '" + KeyValue + "'";
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
        public int Insert(BBdbR_CarPartBase entity, string QualityCheckModelId) //===����ʱ��Ҫ�޸�===
        {
            StringBuilder strSql = new StringBuilder();
            if (entity.CarPartId.ToString() != "")
            {
                BBdbR_QuaCheckModelCarPartConfig Configentity = new BBdbR_QuaCheckModelCarPartConfig();
                Configentity.QuaCheckModelCarPartConfigId = System.Guid.NewGuid().ToString();
                Configentity.QualityCheckModelId = QualityCheckModelId;
                Configentity.CarPartId = entity.CarPartId.ToString();
                Configentity.Enabled = "1";
                Configentity.VersionNumber = "V1.0";
                Configentity.CreTm = DateTime.Now;
                Configentity.CreCd = ManageProvider.Provider.Current().UserId;
                Configentity.CreNm = ManageProvider.Provider.Current().UserName;
                Repository().Insert(Configentity);
            }
            return 1;
        }
        #endregion
        #region 5.ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(List<BBdbR_QuaCheckModelCarPartConfig> TeamStfList)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_QuaCheckModelCarPartConfig> listEntity = new List<BBdbR_QuaCheckModelCarPartConfig>(); //===����ʱ��Ҫ�޸�===
            for (int i = 0; i < TeamStfList.Count; i++)
            {
                TeamStfList[i].Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                TeamStfList[i].Modify(TeamStfList[i].QuaCheckModelCarPartConfigId);
                listEntity.Add(TeamStfList[i]);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
        }
        #endregion
    }
}