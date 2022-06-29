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
    /// ������֯�������ñ�
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.30 19:32</date>
    /// </author>
    /// </summary>
    public class BFacR_TeamOrgBll : RepositoryFactory<BFacR_TeamOrg>
    {
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BFacR_TeamOrg";//===����ʱ��Ҫ�޸�===
        #endregion

        //#region 1.��ѯ������֯������Ӧ����id
        ///// <summary>
        ///// ���ϲ�ѯ��չʾҳ����
        ///// </summary>
        ///// <param name="CheckId"></param>
        ///// <returns></returns>
        //public List<BFacR_TeamOrg> GetStaffList(string OrgNm)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    List<BFacR_TeamOrg> dt;
        //    if (OrgNm != "")
        //    {
        //        strSql.Append(@"SELECT  * FROM  BFacR_TeamOrg where Enabled=1 and OrgNm='" + OrgNm + "'");
        //        dt = Repository().FindListBySql(strSql.ToString());
        //    }
        //    else
        //    {
        //        strSql.Append(@"SELECT  * FROM  BFacR_TeamOrg where Enabled=1 ");
        //        dt = Repository().FindListBySql(strSql.ToString());
        //    }
        //    return dt;
        //}
        //#endregion

        #region 1.��ѯ������֯������Ӧ����id
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public List<BFacR_TeamOrg> GetOrgList(string TeamId)
        {
            StringBuilder strSql = new StringBuilder();
            List<BFacR_TeamOrg> dt=new List<BFacR_TeamOrg>  ();
            if (TeamId != "")
            {
                strSql.Append(@"SELECT  * FROM  BFacR_TeamOrg where Enabled='1' and TeamId='" + TeamId + "'");
                dt = Repository().FindListBySql(strSql.ToString());
                if (dt.Count > 0)
                {
                    for (int i = 0; i < dt.Count; i++)
                    {
                        StringBuilder strSql1 = new StringBuilder();
                        strSql1.Append(@"SELECT  * FROM  BFacR_TeamBase where  TeamId='" + dt[i].TeamId + "'");
                        DataTable dt1 = Repository().FindTableBySql(strSql1.ToString());
                        if (dt1.Rows.Count > 0)
                        {
                            dt[i].RsvFld1 = dt1.Rows[0]["TeamNm"].ToString();
                        }
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public List<BFacR_TeamOrg> GetPlineList(string TeamId, string PlineId)
        {
            StringBuilder strSql = new StringBuilder();
            List<BFacR_TeamOrg> dt;
            if (TeamId != "" && PlineId != "")
            {
                strSql.Append(@"SELECT  * FROM  BFacR_TeamOrg where Enabled='1' and TeamId='" + TeamId + "'and PlineId='" + PlineId + "'");
                dt = Repository().FindListBySql(strSql.ToString());
            }
            else
            {
                if (PlineId == "")
                {
                    strSql.Append(@"SELECT  * FROM  BFacR_TeamOrg where Enabled='1' and TeamId='" + TeamId + "'");
                    dt = Repository().FindListBySql(strSql.ToString());
                }
                else
                {
                    strSql.Append(@"SELECT  * FROM  BFacR_TeamOrg where Enabled='1' ");
                    dt = Repository().FindListBySql(strSql.ToString());
                }
            }
            return dt;
        }

        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="ClassId">��ѯֵ</param>
        /// <returns></returns>
        public List<BFacR_TeamOrg> GetClassList(string TeamId, string PlineId) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (TeamId != "")
            {
                if (PlineId == "")
                {
                    //����������ѯ
                    sql = @"select * from " + tableName + " where  TeamId='" + TeamId + "' and Enabled=1";
                    return Repository().FindListBySql(sql.ToString());
                }
                else
                {
                    //����������ѯ
                    sql = @"select * from " + tableName + " where  TeamId='" + TeamId + "' and PlineId= '" + PlineId + "' and Enabled=1";
                    return Repository().FindListBySql(sql.ToString());
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        public List<BBdbR_PlineBase> GetPlineList(List<BBdbR_PlineBase> ListData)
        {
            string strSql1 = $"SELECT  * FROM  BFacR_TeamOrg where Enabled='1' ";

            return null;
        }

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
        public int Insert(BBdbR_PlineBase entity, string TeamId) //===����ʱ��Ҫ�޸�===
        {
            StringBuilder strSql = new StringBuilder();
            if (entity.PlineId.ToString() != "")
            {
                BFacR_TeamOrg TeamStaffentity = new BFacR_TeamOrg();
                TeamStaffentity.TeamId = TeamId;
                TeamStaffentity.PlineId = entity.PlineId.ToString();
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
        public int Delete(List<BFacR_TeamOrg> TeamStfList)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BFacR_TeamOrg> listEntity = new List<BFacR_TeamOrg>(); //===����ʱ��Ҫ�޸�===
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