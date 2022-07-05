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
    /// ���������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.22 21:04</date>
    /// </author>
    /// </summary>
    public class BBdbR_WorkshopBaseBll : RepositoryFactory<BBdbR_WorkshopBase>
    {

        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_WorkshopBase";//===����ʱ��Ҫ�޸�===

        ////�����ӱ����
        //string subTableName = "_EquipmentGroupBase";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 1.��ȡ������Ҫ�޸�sql���
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===����ʱ��Ҫ�޸�===
            strSql.Append(@"select    CompanyId AS keys,
                        CompanyCd AS code,
                        CompanyNm AS name,
                        '0' as parentId,  
                        '0' as sort    
                        from BBdbR_CompanyBase where Enabled = '1' ");
            strSql.Append(@" union select  
                                    a.FacId AS keys,     
                                    a.FacCd AS code,
                                    a.FacNm AS name,
                                    a.CompanyId AS parentId,
                                    '1' as sort 
                             from  BBdbR_FacBase a,BBdbR_CompanyBase b 
                             where a.CompanyId=b.CompanyId and a.Enabled = '1' order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.�����չʾ�����Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ�������䡿   --����-->   ��������  --����-->  ����˾��
        ///     ���ݴ���Ĳ�����ͬд��ͬ��sql�����в�ѯ           
        /// </summary>
        /// <param name="area_key">����Ľڵ������</param>
        /// <param name="parentId">�ڵ�ĸ�������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetWorkShopList(string areaId, string parentId, ref JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId))
            {
                sql = "select a.*,b.FacCd as FacCd,b.FacNm as FacNm from " + tableName + " a join BBdbR_FacBase b on a.FacId=b.FacId where a.Enabled=1 and b.Enabled=1 order by a.sort asc";     //===����ʱ��Ҫ�޸�===
            }
            else
            {
                //�����˾
                if (parentId=="0")
                {
                    sql = "select a.*,b.FacCd as FacCd,b.FacNm as FacNm from " + tableName + " a join BBdbR_FacBase b on a.FacId=b.FacId join BBdbR_CompanyBase c on b.CompanyId=c.CompanyId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.CompanyId='"+ areaId+ "' order by a.sort asc";     //===����ʱ��Ҫ�޸�===
                }
                //�������
                else
                {
                    sql = "select a.*,b.FacCd as FacCd,b.FacNm as FacNm from " + tableName + " a join BBdbR_FacBase b on a.FacId=b.FacId where a.Enabled=1 and b.Enabled=1 and a.FacId='"+ areaId + "' order by a.sort asc";     //===����ʱ��Ҫ�޸�===
                }     
            }
            return Repository().FindTableBySql(sql.ToString(), false);
        }
        #endregion

        #region 3.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_WorkshopBase entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
        }
        #endregion

        #region 4.���ĳ���ֶε��ֶ�ֵ�Ƿ����
        /// <summary>
        ///   Enabled = 1��������ĳ���ֶΣ�KeyName�����ֶ�ֵ��KeyValue���Ƿ����
        /// </summary>
        /// <param name="KeyName">�ֶ���</param>
        /// <param name="KeyValue">�ֶ�ֵ</param>
        /// <returns>���ص��������ı��а������ֶ�ֵ�ļ�¼����</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where  " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }

        public int hasChildNode(string WorkshopId)
        {
            string sql = @"select * from BBdbR_WorkSectionBase where WorkshopId = '" + WorkshopId + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
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
        public int Insert(BBdbR_WorkshopBase entity) //===����ʱ��Ҫ�޸�===
        {
           // entity.Enabled = "1";

            return Repository().Insert(entity);
        }
        #endregion

        #region 6.ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(string[] array)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_WorkshopBase> listEntity = new List<BBdbR_WorkshopBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_WorkshopBase entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                entity.Modify(keyValue);
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
        }
        #endregion

        #region 7.��ѯ��������Ҫ�޸�sql���
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
                sql = "select a.*,b.FacCd as FacCd,b.FacNm as FacNm from " + tableName + " a join BBdbR_FacBase b on a.FacId=b.FacId where a.Enabled=1 and b.Enabled=1 order by a.sort asc";     //===����ʱ��Ҫ�޸�===
            }
            else
            {
                if (keywords == "all")
                {
                    sql = "select a.*,b.FacCd as FacCd,b.FacNm as FacNm from " + tableName + " a join BBdbR_FacBase b on a.FacId=b.FacId where a.Enabled=1 and b.Enabled=1 order by a.sort asc";     //===����ʱ��Ҫ�޸�===
                }
                //�������
                else
                {
                    sql = "select a.*,b.FacCd as FacCd,b.FacNm as FacNm from " + tableName + " a join BBdbR_FacBase b on a.FacId=b.FacId where a.Enabled=1 and b.Enabled=1 and " + Condition + " like  '%" + keywords + "%' order by a.sort asc";     //===����ʱ��Ҫ�޸�===
                }
            }
            return Repository().FindTableBySql(sql.ToString(), false);
        }
        #endregion

        #region 8.����ȫ�������б�
        /// <summary>
        /// ���ϲ�ѯ
        /// </summary>
        /// <returns></returns>
        public List<BBdbR_WorkshopBase> GetWorkShopList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_WorkshopBase where Enabled=1 ");
            List<BBdbR_WorkshopBase> dt = Repository().FindListBySql(strSql.ToString());
            for (int i = 0; i < dt.Count; i++)
            {
                string sql1 = "select * from BBdbR_FacBase where FacId='" + dt[i].FacId + "'and Enabled = 1 order by sort asc";
                DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
                if (dt1.Rows.Count > 0)
                {
                    dt[i].FacId = dt1.Rows[0]["FacNm"].ToString();
                }
            }
            return dt;
        }
        public List<BBdbR_WorkshopBase> GetWorkList(string WorkshopNm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_WorkshopBase where  WorkshopNm='" + WorkshopNm + "'");
            List<BBdbR_WorkshopBase> dt = Repository().FindListBySql(strSql.ToString());
            for (int i = 0; i < dt.Count; i++)
            {
                string sql1 = "select * from BBdbR_FacBase where FacId='" + dt[i].FacId + "'";
                DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
                if (dt1.Rows.Count > 0)
                {
                    dt[i].FacId = dt1.Rows[0]["FacNm"].ToString();
                }
            }
            return dt;
        }
        public List<BBdbR_WorkshopBase> GetWorkList1(string WorkshopId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_WorkshopBase where  WorkshopId='" + WorkshopId + "'");
            List<BBdbR_WorkshopBase> dt = Repository().FindListBySql(strSql.ToString());
            for (int i = 0; i < dt.Count; i++)
            {
                string sql1 = "select * from BBdbR_FacBase where FacId='" + dt[i].FacId + "'";
                DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
                if (dt1.Rows.Count > 0)
                {
                    dt[i].FacId = dt1.Rows[0]["FacNm"].ToString();
                }
            }
            return dt;
        }
        /// <summary>
        /// �༭����������Դ
        /// </summary>
        /// <returns></returns>
        public BBdbR_WorkshopBase GetPlanList(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_WorkshopBase where  WorkshopId='" + KeyValue + "' order by sort asc");
            List<BBdbR_WorkshopBase> dt = Repository().FindListBySql(strSql.ToString());
            BBdbR_WorkshopBase Dvcentity = new BBdbR_WorkshopBase();
            Dvcentity = dt[0];
            return Dvcentity;
        }
        //11.��ȡ������Ա
        public DataTable GetPlineNm()
        {
            string sql = @"select StfId as id, stfnm from BBdbR_StfBase where Enabled='1' and StfPosn='���为����'";
            return Repository().FindTableBySql(sql);
        }
        //��ȡ��Ա��Ϣ
        public DataTable GetPlineNm2(string StfId)
        {
            string sql = @"select * from BBdbR_StfBase where Enabled='1' and StfId=" + "'" + StfId + "'";
            return Repository().FindTableBySql(sql);
        }
        #endregion

        #region 9.������������
        /// <summary>
        /// ��������й��ڹ������µĳ�������
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>����������������</returns>
        public int GetWorkshopCount(string KeyValue) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (KeyValue != "")
            {
                sql = @"select * from BBdbR_WorkshopBase where Enabled='1' and FacId='" + KeyValue + "'";
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
    }
}