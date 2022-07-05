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
using System.Data.Common;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// ���նλ�����Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.23 20:22</date>
    /// </author>
    /// </summary>
    public class BBdbR_WorkSectionBaseBll : RepositoryFactory<BBdbR_WorkSectionBase>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_WorkSectionBase";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 1.��ȡ������Ҫ�޸�sql���
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===����ʱ��Ҫ�޸�===
            strSql.Append(@"select  FacId AS keys,     
                        FacCd AS code,
                        FacNm AS name,
                        Enabled As IsAvailable,
                        '0' as parentId,  
                        '0' as sort    
                        from BBdbR_FacBase where Enabled = '1' ");
            strSql.Append(@" union select    
                        a.WorkshopId AS keys,
                        a.WorkshopCd AS code,
                        a.WorkshopNm AS name,
                        a.Enabled As IsAvailable,
                        a.FacId AS parentId,
                        '1' as sort 
                      from  BBdbR_WorkshopBase a,BBdbR_FacBase b 
                      where a.FacId=b.FacId and a.Enabled = '1' order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.�����չʾ�����Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ�������Ρ�   --����-->   �����䡿  --����-->  ��������
        ///     ���ݴ���Ĳ�����ͬд��ͬ��sql�����в�ѯ           
        /// </summary>
        /// <param name="areaId">����Ľڵ������</param>
        /// <param name="parentId">�ڵ�ĸ�������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetWorkSectionList(string areaId, string parentId, ref JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            DataTable dt = new DataTable();
            //չʾҳ����
            if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId))
            {
                sql = "select a.*,b.WorkshopCd as WorkshopCd,b.WorkshopNm as WorkshopNm,b.WorkshopTyp as WorkshopTyp from " + tableName + " a join BBdbR_WorkshopBase b on a.WorkshopId=b.WorkshopId where a.Enabled=1 order by a.sort asc";     //===����ʱ��Ҫ�޸�===
            }
            else
            {
                //����
                if (parentId != "0")
                {
                    sql = "select a.*,b.WorkshopCd as WorkshopCd,b.WorkshopNm as WorkshopNm,b.WorkshopTyp as WorkshopTyp from " + tableName + " a join BBdbR_WorkshopBase b on a.WorkshopId=b.WorkshopId where a.Enabled=1 and a.WorkshopId ='" + areaId + "' order by a.sort asc ";     //===����ʱ��Ҫ�޸�===
                }
                //����
                else
                {
                    sql = "select a.*,b.WorkshopCd as WorkshopCd,b.WorkshopNm as WorkshopNm,b.WorkshopTyp as WorkshopTyp from " + tableName + " a join BBdbR_WorkshopBase b on a.WorkshopId=b.WorkshopId join BBdbR_FacBase c on b.FacId=c.FacId where a.Enabled=1 and c.Enabled=1 and c.FacId='" + areaId + "' order by a.sort asc";     //===����ʱ��Ҫ�޸�===                    
                }
            }
            dt = Repository().FindTableBySql(sql.ToString(), false);
            return dt;          
        }
        #endregion

        #region 3.��ѯ��������Ҫ�޸�sql���
        /// <summary>
        ///     ��ѯʱ�ṩ�������ؼ��֣�һ����Condition����һ����keywords
        ///     
        ///     Condition�ǹؼ��֣�����ѯ��������Ӧ���ݿ��е�һ���ֶ�
        ///     keywords�ǲ�ѯֵ������ѯ�����ľ���ֵ����Ӧ���ݿ��в�ѯ�����ֶε�ֵ
        ///     ��ѯ��ʱ�򴫵���Condition��keywords
        /// 
        /// </summary>
        /// <param name="keywords">��ѯֵ</param>
        /// <param name="Condition">�ؼ��֣���ѯ������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>��ѯ�����ݣ��б�</returns>
        public DataTable GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            DataTable dt = new DataTable();
            if (Condition == "all")
            {
                sql = "select a.*,b.WorkshopCd as WorkshopCd,b.WorkshopNm as WorkshopNm from " + tableName + " a join BBdbR_WorkshopBase b on a.WorkshopId=b.WorkshopId where a.Enabled=1 and b.Enabled=1 order by a.sort asc";     //===����ʱ��Ҫ�޸�===
            }
            else
            {
                if (keywords == "all")
                {
                    sql = "select a.*,b.WorkshopCd as WorkshopCd,b.WorkshopNm as WorkshopNm from " + tableName + " a join BBdbR_WorkshopBase b on a.WorkshopId=b.WorkshopId where a.Enabled=1 and b.Enabled=1 order by a.sort asc";     //===����ʱ��Ҫ�޸�===
                }
                else
                {
                    sql = "select a.*,b.WorkshopCd as WorkshopCd,b.WorkshopNm as WorkshopNm from " + tableName + " a join BBdbR_WorkshopBase b on a.WorkshopId=b.WorkshopId where a.Enabled=1 and b.Enabled=1 and  " + Condition + " like  '%" + keywords + "%' order by a.sort asc";  //===����ʱ��Ҫ�޸�===
                }
            }
            return Repository().FindTableBySql(sql.ToString(), false);
        }
        #endregion

        #region 4.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_WorkSectionBase entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
        }
        #endregion

        #region 5.���ĳ���ֶε��ֶ�ֵ�Ƿ����
        /// <summary>
        ///   Enabled = 1��������ĳ���ֶΣ�KeyName�����ֶ�ֵ��KeyValue���Ƿ����
        /// </summary>
        /// <param name="KeyName">�ֶ���</param>
        /// <param name="KeyValue">�ֶ�ֵ</param>
        /// <returns>���ص��������ı��а������ֶ�ֵ�ļ�¼����</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and  " + KeyName + "  = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion

        #region 6.��������
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
        public int Insert(BBdbR_WorkSectionBase entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Insert(entity);
        }
        #endregion

        #region 7.ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(string[] array)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_WorkSectionBase> listEntity = new List<BBdbR_WorkSectionBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_WorkSectionBase entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
        }
        #endregion

        #region 8.�༭�������Դ
        /// <summary>
        /// �༭����������Դ
        /// </summary>
        /// <returns></returns>
        public BBdbR_WorkSectionBase GetPlanList1(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_WorkSectionBase where  WorkSectionId='" + KeyValue + "' order by sort asc");
            List<BBdbR_WorkSectionBase> dt = Repository().FindListBySql(strSql.ToString());
            BBdbR_WorkSectionBase Dvcentity = new BBdbR_WorkSectionBase();
            Dvcentity = dt[0];
            return Dvcentity;
        }
        #endregion

        #region 9.���ҳ����¹��ն�����
        /// <summary>
        /// ���ҳ����¹��ն�����
        /// </summary>
        /// <returns></returns>
        public int GetWorkSectionCount(string KeyValue) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (KeyValue != "")
            {
                sql = @"select * from BBdbR_WorkSectionBase where Enabled='1' and WorkshopId='" + KeyValue + "'";
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

        #region 10.��ȡ��Ա��Ϣ

        //10.��ȡ������Ա
        public DataTable GetWorkSectionNm()
        {
            string sql = @"select StfId as id, StfNm from BBdbR_StfBase where Enabled='1' and StfPosn='" + "���նθ�����" + "'";
            return Repository().FindTableBySql(sql);
        }
        //��ȡ��Ա��Ϣ
        public DataTable GetWorkSectionNm2(string StfId)
        {
            string sql = @"select stfnm,phn from BBdbR_StfBase where Enabled='1' and StfId='" + StfId + "'";
            return Repository().FindTableBySql(sql);
        }
      
        #endregion
    }
}
