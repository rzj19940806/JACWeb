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
    /// ��ΰ������ñ�
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.29 22:27</date>
    /// </author>
    /// </summary>
    public class BFacR_ClassTeamConfigBll : RepositoryFactory<BFacR_ClassTeamConfig>
    {
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BFacR_ClassTeamConfig";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 1.����
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="ClassId">��ѯֵ</param>
        /// <returns></returns>
        public List<BFacR_ClassTeamConfig> GetPageListByCondition(string ClassId) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (ClassId != "")
            {
                //����������ѯ
                sql = @"select * from " + tableName + " where  ClassId='" + ClassId + "' and Enabled=1";
                return Repository().FindListBySql(sql.ToString());
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 2.����
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="ClassId">��ѯֵ</param>
        /// <returns></returns>
        public List<BFacR_ClassTeamConfig> GetClassList(string ShiftId, string TeamId) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (ShiftId != "")
            {
                if (TeamId == "")
                {
                    //����������ѯ
                    sql = @"select * from " + tableName + " where  ShiftId='" + ShiftId + "' and Enabled=1";
                    return Repository().FindListBySql(sql.ToString());
                }
                else
                {
                    //����������ѯ
                    sql = @"select * from " + tableName + " where  ShiftId='" + ShiftId + "' and TeamId= '" + TeamId + "' and Enabled=1";
                    return Repository().FindListBySql(sql.ToString());
                }
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
        public int Delete(List<BFacR_ClassTeamConfig> ClassConfigList)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BFacR_ClassTeamConfig> listEntity = new List<BFacR_ClassTeamConfig>(); //===����ʱ��Ҫ�޸�===
            for (int i = 0; i < ClassConfigList.Count; i++)
            {
                ClassConfigList[i].Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(ClassConfigList[i]);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
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
        public int InsertClassConfig(string ShiftId, BFacR_TeamBase entity) //===����ʱ��Ҫ�޸�===
        {
            StringBuilder strSql = new StringBuilder();
            int Isok = 0;
            if (entity.TeamId.ToString() != "")
            {
                BFacR_ClassTeamConfig Classentity = new BFacR_ClassTeamConfig();
                Classentity.ShiftId = ShiftId;
                Classentity.TeamId = entity.TeamId.ToString();
                Classentity.Create();
                Repository().Insert(Classentity);
                Isok = 1;
            }
            return Isok;
        }
        #endregion
    }
}