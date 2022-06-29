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
    /// ����������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.29 20:18</date>
    /// </author>
    /// </summary>
    public class BPdb_DtBll : RepositoryFactory<BPdb_Dt>
    {
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BPdb_Dt";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 1.��ȡ������Ҫ�޸�sql���
        public DataTable GetTree(string year)
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("keys", typeof(string));
            DataColumn dc2 = new DataColumn("code", typeof(string));
            DataColumn dc3 = new DataColumn("name", typeof(string));
            DataColumn dc4 = new DataColumn("IsAvailable", typeof(string));
            DataColumn dc5 = new DataColumn("parentId", typeof(string));
            DataColumn dc6 = new DataColumn("sort", typeof(string));
            dt.Columns.Add(dc1); dt.Columns.Add(dc2); dt.Columns.Add(dc3); dt.Columns.Add(dc4); dt.Columns.Add(dc5); dt.Columns.Add(dc6);
            string sql = "select * from " + tableName + " where Enabled = '1' order by Tm asc";     //===����ʱ��Ҫ�޸�===
            List<BPdb_Dt> listEntity = Repository().FindListBySql(sql); //ִ��sql���
            List<BPdb_Dt> listEntity1 = new List<BPdb_Dt>(); //ִ��sql���
            string a = "";
            for (int i = 0; i < listEntity.Count; i++)
            {
                a = listEntity[i].Tm.ToString().Substring(0, 4);
                if (listEntity1.Count == 0)
                {
                    listEntity1.Add(listEntity[i]);
                }
                else
                {
                    int c = 0;
                    for (int j = 0; j < listEntity1.Count; j++)
                    {
                        if (a == listEntity1[j].Tm.ToString().Substring(0, 4))
                        {
                            c = 1;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (c == 0)
                    {
                        listEntity1.Add(listEntity[i]);
                    }
                }
            }
            for (int j = 0; j < listEntity1.Count; j++)
            {
                DataRow dr1 = dt.NewRow();
                dr1["keys"] = listEntity1[j].Tm.ToString().Substring(0, 4) + "��";
                dr1["code"] = listEntity1[j].Tm.ToString().Substring(0, 4) + "��";
                dr1["name"] = listEntity1[j].Tm.ToString().Substring(0, 4) + "��";
                dr1["IsAvailable"] = "1";
                dr1["parentId"] = "0";
                dr1["sort"] = "1";
                dt.Rows.Add(dr1);
                for (int i = 1; i <= 12; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["keys"] = listEntity1[j].Tm.ToString().Substring(0, 4) + "��" + i + "��";//keys����ظ����ᱨ��
                    dr["code"] = i;
                    dr["name"] = i + "��";
                    dr["IsAvailable"] = "1";
                    dr["parentId"] = listEntity1[j].Tm.ToString().Substring(0, 4) + "��";
                    dr["sort"] = "1";
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }
        #endregion

        #region 2.�����չʾ�����Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ�������䡿   --����-->   ��������  --����-->  ����˾��
        ///     ���ݴ���Ĳ�����ͬд��ͬ��sql�����в�ѯ           
        /// </summary>
        /// <param name="areaId">����Ľڵ������</param>
        /// <param name="parentId">�ڵ�ĸ�������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public List<BPdb_Dt> GetList(string areaId, string parentId, ref JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            List<BPdb_Dt> listEntity = new List<BPdb_Dt>();
            List<BPdb_Dt> listEntity1 = new List<BPdb_Dt>();
            string sql = "";
            if (parentId != "0")
            {
                //�ӱ����в�ѯ�ϼ��������봫��������ͬ��ȵ����ݣ��������б�
                sql = "select * from " + tableName + " where Enabled = 1 order by Tm asc";     //===����ʱ��Ҫ�޸�===
                listEntity = Repository().FindListBySql(sql); //ִ��sql���
                for (int i = 0; i < listEntity.Count; i++)
                {
                    string a = listEntity[i].Tm.ToString().Substring(0, 4) + "��";
                    string b = listEntity[i].Tm.ToString().Substring(5, 1) + "��";
                    if (b == areaId && a == parentId)
                    {
                        listEntity1.Add(listEntity[i]);
                    }
                } 
            }
            else
            {
                sql = "select * from BPdb_Dt where Enabled = 1 order by Tm asc";     //===����ʱ��Ҫ�޸�===
                listEntity = Repository().FindListBySql(sql); //ִ��sql���
                string a = "";
                for (int i = 0; i < listEntity.Count; i++)
                {
                    a = listEntity[i].Tm.ToString().Substring(0, 4) + "��";
                    if (a == areaId)
                    {
                        listEntity1.Add(listEntity[i]);
                    }
                }
            }
            return listEntity1;
        }
        #endregion

        #region 3.Formҳ������
        //����
        public DataTable GetClassNm()
        {
            string sql = @"SELECT ClassId AS id, ClassNm
                           FROM BFacR_ClassBase
                           WHERE 1 = 1";
            return Repository().FindTableBySql(sql);
        }
        //����
        public DataTable GetWsbNm()
        {
            string sql = @"SELECT WorkshopId AS id, WorkshopNm
                           FROM BBdbR_WorkshopBase
                           WHERE 1 = 1";
            return Repository().FindTableBySql(sql);
        }

        //����
        public DataTable GetPlineNm()
        {
            string sql = @"SELECT PlineId AS id, PlineNm
                           FROM BBdbR_PlineBase
                           WHERE 1 = 1";
            return Repository().FindTableBySql(sql);
        }
        #endregion

        #region 3.ҳ����
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public List<BPdb_Dt> GetPlanList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BPdb_Dt where 1=1 order by Tm asc");
            List<BPdb_Dt> dt = Repository().FindListBySql(strSql.ToString());
            //for (int i = 0; i < dt.Count; i++)
            //{
            //    if (dt[i].OrgRank!="")
            //    {
            //        if (dt[i].OrgRank == "1")
            //        {
            //            string sql1 = "select * from BBdbR_FacBase where FacId ='" + dt[i].OrgId + "' ";     //===����ʱ��Ҫ�޸�===
            //            DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
            //            if (dt1.Rows.Count > 0)
            //            {
            //                dt[i].RsvFld1 = dt1.Rows[0]["FacNm"].ToString();//��֯��������
            //                dt[i].RsvFld2 = dt[i].Tm.ToString().Substring(0, 10);
            //            }
            //        }
            //        else if (dt[i].OrgRank == "2")
            //        {
            //            string sql1 = "select * from BBdbR_WorkshopBase where WorkshopId ='" + dt[i].OrgId + "' ";     //===����ʱ��Ҫ�޸�===
            //            DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
            //            if (dt1.Rows.Count > 0)
            //            {
            //                dt[i].RsvFld1 = dt1.Rows[0]["WorkshopNm"].ToString();//��֯��������
            //                dt[i].RsvFld2 = dt[i].Tm.ToString().Substring(0, 10);
            //            }
            //        }
            //        else
            //        {
            //            string sql1 = "select * from BBdbR_PlineBase where PlineId ='" + dt[i].OrgId + "' ";     //===����ʱ��Ҫ�޸�===
            //            DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
            //            if (dt1.Rows.Count > 0)
            //            {
            //                dt[i].RsvFld1 = dt1.Rows[0]["PlineNm"].ToString();//��֯��������
            //                dt[i].RsvFld2 = dt[i].Tm.ToString().Substring(0, 10);
            //            }
            //        }
            //        //string sql1 = "select * from BFacR_TeamOrg where OrgId ='" + dt[i].OrgId + "'and Enabled = '1' ";     //===����ʱ��Ҫ�޸�===
            //        //DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
            //        //if (dt1.Rows.Count > 0)
            //        //{
            //        //    dt[i].RsvFld1 = dt1.Rows[0]["OrgNm"].ToString();//��֯��������
            //        //    dt[i].RsvFld2 = dt[i].Tm.ToString().Substring(0, 10);
            //        //}
            //        string sql2 = "select * from BFacR_ClassBase where ClassId ='" + dt[i].ClassId + "' ";     //===����ʱ��Ҫ�޸�===
            //        DataTable dt2 = Repository().FindTableBySql(sql2.ToString());
            //        if (dt2.Rows.Count > 0)
            //        {
            //            dt[i].RsvFld3 = dt2.Rows[0]["ClassNm"].ToString();//��������
            //            dt[i].RsvFld4 = dt2.Rows[0]["ClassTyp"].ToString();//�������
            //        }
            //    }
               
            //}
            return dt;
        }
        #endregion

        #region 3.չʾ���
        /// <summary>
        /// �������������IsAvailable = true������
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>����������������</returns>
        public List<BPdb_Dt> GetPageList(JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            return Repository().FindList();
        }
        #endregion

        #region 4.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BPdb_Dt entity) //===����ʱ��Ҫ�޸�===
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
            string sql = @"select * from " + tableName + " where '" + KeyName + "' = '" + KeyValue + "'";
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
        public int Insert(BPdb_Dt entity) //===����ʱ��Ҫ�޸�===
        {      
            return Repository().Insert(entity);
        }
        #endregion

        #region 7.ɾ������
        public int DeleteUseEnabled(string[] array)
        {            
            List<BPdb_Dt> listEntity = new List<BPdb_Dt>(); 
            foreach (string id in array)
            {
                BPdb_Dt entity = Repository().FindEntity(id);
                entity.Enabled = "0";
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);
        }
        #endregion

        #region 8.��ѯ��������Ҫ�޸�sql���
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
        public List<BPdb_Dt> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            List<BPdb_Dt> dt;
            if (Condition == "all")
            {
                sql = @"select * from " + tableName + " where Enabled = 1 order by Tm asc";
                dt = Repository().FindListBySql(sql.ToString());
            }
            else
            {
                if (keywords!="")
                {
                    //����������ѯ
                    sql = @"select * from " + tableName + " where  " + Condition + " like  '%" + keywords + "%' and Enabled = 1 order by Tm asc";
                    dt = Repository().FindListBySql(sql.ToString());
              
                }
                else
                {
                    sql = @"select * from " + tableName + " where Enabled = 1 order by Tm asc";
                    dt = Repository().FindListBySql(sql.ToString());
                }
            }
            return dt;
        }
        public List<BPdb_Dt> GetDtList(string keywords, string condition)
        {
            List<BPdb_Dt> dt = new List<BPdb_Dt>() ;
            if (keywords !="" && keywords != null)
            {
                string sql = "select * from " + tableName + " where " + keywords + " = '" + condition + "' and Enabled = 1";
                dt = Repository().FindListBySql(sql.ToString());
            }
            return dt;
        }
        #endregion
    }
}