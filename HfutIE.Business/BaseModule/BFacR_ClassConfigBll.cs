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
    /// ���ư��������Ϣ��GetReConfigList
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.29 22:26</date>
    /// </author>
    /// </summary>
    public class BFacR_ClassConfigBll : RepositoryFactory<BFacR_ClassConfig>
    {
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BFacR_ClassConfig";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 1.����
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="ClassId">��ѯֵ</param>
        /// <returns></returns>
        public List<BFacR_ClassConfig> GetClassList(string ClassId,string ShiftId) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (ClassId != "")
            {
                if (ShiftId=="")
                {
                    //����������ѯ
                    sql = @"select * from " + tableName + " where  ClassId='" + ClassId + "' and Enabled=1";
                    return Repository().FindListBySql(sql.ToString());
                }
                else
                {
                    //����������ѯ
                    sql = @"select * from " + tableName + " where  ClassId='" + ClassId + "' and ShiftId= '"+ ShiftId+"' and Enabled=1";
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
        public int Delete(List<BFacR_ClassConfig> ClassConfigList)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BFacR_ClassConfig> listEntity = new List<BFacR_ClassConfig>(); //===����ʱ��Ҫ�޸�===
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
        public int InsertClassConfig(string ClassId , BFacR_ShiftBase entity) //===����ʱ��Ҫ�޸�===
        {
            StringBuilder strSql = new StringBuilder();
            int Isok = 0;
            if (entity.ShiftId.ToString() != "")
            {
                BFacR_ClassConfig Classentity = new BFacR_ClassConfig();
                Classentity.ClassId = ClassId;
                Classentity.ShiftId = entity.ShiftId.ToString();
                Classentity.ShiftCd = entity.ShiftCd.ToString();
                Classentity.ShiftNm = entity.ShiftNm.ToString();
                Classentity.Create();
                Repository().Insert(Classentity);
                Isok = 1;
            }
            return Isok;
        }
        #endregion
    }
}