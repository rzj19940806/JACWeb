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
    /// ������Ա���ñ�
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.28 15:29</date>
    /// </author>
    /// </summary>
    public class BFacR_TeamStfBll : RepositoryFactory<BFacR_TeamStfConfig>
    {
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BFacR_TeamStfConfig";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 1.ҳ����
        
        public List<BFacR_TeamStfConfig> GetTeamStaffList(string TeamId)
        {
            StringBuilder strSql = new StringBuilder();
            List<BFacR_TeamStfConfig> dt = new List<BFacR_TeamStfConfig>();
            if (TeamId != "")
            {
                strSql.Append(@"SELECT  * FROM  BFacR_TeamStfConfig where Enabled='1' and TeamId='" + TeamId + "'");
                dt = Repository().FindListBySql(strSql.ToString());
                if (dt.Count > 0)
                {
                    for (int i = 0; i < dt.Count; i++)
                    {
                        StringBuilder strSql1 = new StringBuilder();
                        strSql1.Append(@"SELECT  * FROM  BFacR_TeamBase where Enabled='1' and TeamId='" + dt[i].TeamId + "'");
                        DataTable dt1 = Repository().FindTableBySql(strSql1.ToString());
                        if (dt1.Rows.Count > 0)
                        {
                            dt[i].RsvFld1 = dt1.Rows[0]["TeamNm"].ToString();
                        }
                    }
                    for (int i = 0; i < dt.Count; i++)
                    {
                        string strSql1 = $"SELECT  * FROM  BBdbR_StfBase where Enabled='1' ";
                        //strSql1.Append(@"SELECT  * FROM  BFacR_TeamBase where  TeamId='" + dt[i].TeamId + "'");
                        DataTable dt1 = Repository().FindTableBySql(strSql1);
                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            if (dt1.Rows[j]["StfId"].ToString() == dt[i].StfId)
                            {
                                dt[i].RsvFld2 = dt1.Rows[j]["StfNm"].ToString();
                            }
                        }

                    }
                }

            }
            return dt;
        }


        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="ClassId">��ѯֵ</param>
        /// <returns></returns>
        public List<BFacR_TeamStfConfig> GetClassList(string TeamId, string StfId) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (TeamId != "")
            {
                if (StfId == "")
                {
                    //����������ѯ
                    sql = @"select * from " + tableName + " where  TeamId='" + TeamId + "' and Enabled=1";
                    return Repository().FindListBySql(sql.ToString());
                }
                else
                {
                    //����������ѯ
                    sql = @"select * from " + tableName + " where  TeamId='" + TeamId + "' and StfId= '" + StfId + "' and Enabled=1";
                    return Repository().FindListBySql(sql.ToString());
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 2.�����ð�����Ա�嵥
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public List<BFacR_TeamStfConfig> GetStaffList(string TeamId, string StfId)
        {
            StringBuilder strSql = new StringBuilder();
            List<BFacR_TeamStfConfig> dt;
            if (TeamId != "" && StfId != "")
            {
                strSql.Append(@"SELECT  * FROM  BFacR_TeamStfConfig where Enabled='1' and TeamId='" + TeamId + "'and StfId='" + StfId + "'");
                dt = Repository().FindListBySql(strSql.ToString());
            }
            else
            {
                if (StfId == "")
                {
                    strSql.Append(@"SELECT  * FROM  BFacR_TeamStfConfig where Enabled='1' and TeamId='" + TeamId + "'");
                    dt = Repository().FindListBySql(strSql.ToString());
                }
                else
                {
                    strSql.Append(@"SELECT  * FROM  BFacR_TeamStfConfig where Enabled='1' ");
                    dt = Repository().FindListBySql(strSql.ToString());
                }
            }
            return dt;
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
        public int Insert(BBdbR_StfBase entity, string TeamId) //===����ʱ��Ҫ�޸�===
        {
            StringBuilder strSql = new StringBuilder();
            if (entity.StfCd.ToString() != "")
            {
                BFacR_TeamStfConfig TeamStaffentity = new BFacR_TeamStfConfig();
                TeamStaffentity.TeamId = TeamId;
                TeamStaffentity.StfId = entity.StfId.ToString();
                TeamStaffentity.Create();
                Repository().Insert(TeamStaffentity);
            }
            return 1;
        }
        #endregion

        #region 5.ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(List<BFacR_TeamStfConfig> TeamStfList)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BFacR_TeamStfConfig> listEntity = new List<BFacR_TeamStfConfig>(); //===����ʱ��Ҫ�޸�===
            for (int i = 0; i < TeamStfList.Count; i++)
            {
                TeamStfList[i].Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(TeamStfList[i]);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
        }
        #endregion


    }
}