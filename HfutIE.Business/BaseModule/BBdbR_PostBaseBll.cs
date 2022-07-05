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
    /// ��λ������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.23 20:57 by ckl</date>
    /// </author>
    /// </summary>
    public class BBdbR_PostBaseBll : RepositoryFactory<BBdbR_PostBase>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_PostBase";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 1.��ȡ������Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ������λ��   --����-->   ����λ��  --����-->  �����ߡ�  --����-->  �����Ρ�
        /// </summary>
        /// <returns></returns>
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===����ʱ��Ҫ�޸�===����������
            //����ڵ�
            strSql.Append(@"select c.WorkshopId As keys,
                                   c.WorkshopCd As code,
                                   c.WorkshopNm As name,
                                   c.Enabled As IsAvailable,
                                   '0' as parentId,
                                   '0' as sort             
                                   from BBdbR_WorkshopBase c where c.Enabled = '1'");
            //���νڵ�
            strSql.Append(@"union select 
                                    b.WorkSectionId AS keys,
                                    b.WorkSectionCd AS code, 
                                    b.WorkSectionNm AS name, 
                                    b.Enabled As IsAvailable,
                                    b.WorkshopId as parentId,
                                    '1' as sort 
                                    from BBdbR_WorkSectionBase b,BBdbR_WorkshopBase c where b.WorkshopId = c.WorkshopId and b.Enabled= '1'  ");
            //���߽ڵ�
            strSql.Append(@" union select    
                                    a.PlineId AS keys,
                                    a.PlineCd AS code,
                                    a.PlineNm AS name,
                                    a.Enabled As IsAvailable,
                                    a.WorkSectionId AS parentId,
                                    '2' as sort 
                             from  BBdbR_PlineBase a,BBdbR_WorkSectionBase b 
                             where a.WorkSectionId=b.WorkSectionId and a.Enabled = '1' and a.Enabled = '1' order by code asc");
            ////��λ�ڵ�
            //strSql.Append(@" union select    
            //                        d.WcId AS keys,
            //                        d.WcCd AS code,
            //                        d.WcNm AS name,
            //                        d.Enabled As IsAvailable,
            //                        d.PlineId AS parentId,
            //                        '3' as sort 
            //                 from BBdbR_WcBase d,BBdbR_PlineBase a
            //                 where  d.PlineId=a.PlineId and d.Enabled = '1' and a.Enabled = '1' order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.�����չʾ�����Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ������λ��   --����-->   ����λ��  --����-->  �����ߡ�  --����-->  �����Ρ�
        ///     ���ݴ���Ĳ�����ͬд��ͬ��sql�����в�ѯ           
        /// </summary>
        /// <param name="areaId">����Ľڵ������</param>
        /// <param name="parentId">�ڵ�ĸ�������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetList(string areaId, string parentId, string sort, ref JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            DataTable dt = new DataTable();
            if(string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId))
            {
                sql = "select a.*,b.WcCd as WcCd,b.WcNm as WcNm,b.WcTyp as WcTyp from " + tableName + " a join BBdbR_WcBase b on a.WcId=b.WcId where a.Enabled=1 and b.Enabled=1";     //===����ʱ��Ҫ�޸�===
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            else
            {
                if (parentId != "0")//������߻��λ�ڵ�
                {
                    if (sort == "1")//���߽ڵ�
                    {
                        sql = "select a.*,b.WcCd as WcCd,b.WcNm as WcNm,b.WcTyp as WcTyp from " + tableName + " a join BBdbR_WcBase b on a.WcId=b.WcId join BBdbR_PlineBase c on b.PlineId=c.PlineId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.PlineId='" + areaId + "'";     //===����ʱ��Ҫ�޸�===
                        dt = Repository().FindTableBySql(sql.ToString(), false);
                    }
                    else//��λ�ڵ�
                    {
                        sql = "select a.*,b.WcCd as WcCd,b.WcNm as WcNm,b.WcTyp as WcTyp from " + tableName + " a join BBdbR_WcBase b on a.WcId=b.WcId where a.Enabled=1 and b.Enabled=1 and a.WcId='"+ areaId+"'";     //===����ʱ��Ҫ�޸�===
                        dt = Repository().FindTableBySql(sql.ToString(), false);
                    }
                }
                else
                {
                    sql = "select a.*,b.WcCd as WcCd,b.WcNm as WcNm,b.WcTyp as WcTyp from " + tableName + " a join BBdbR_WcBase b on a.WcId=b.WcId join BBdbR_PlineBase c on b.PlineId=c.PlineId join BBdbR_WorkSectionBase d on c.WorkSectionId=d.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 and d.WorkSectionId='" + areaId + "'";     //===����ʱ��Ҫ�޸�===
                    dt = Repository().FindTableBySql(sql.ToString(), false);
                }
            }
            return dt;
        }
        #endregion

        #region 3.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_PostBase entity) //===����ʱ��Ҫ�޸�===
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
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + " = '" + KeyValue + "'";
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
        public int Insert(BBdbR_PostBase entity) //===����ʱ��Ҫ�޸�===
        {
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
            List<BBdbR_PostBase> listEntity = new List<BBdbR_PostBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_PostBase entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
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
                sql = "select a.*,b.WcCd as WcCd,b.WcNm as WcNm,b.WcTyp as WcTyp from " + tableName + " a join BBdbR_WcBase b on a.WcId=b.WcId where a.Enabled=1 and b.Enabled=1";     //===����ʱ��Ҫ�޸�===
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            else
            {
                if (keywords == "all")
                {
                    sql = "select a.*,b.WcCd as WcCd,b.WcNm as WcNm,b.WcTyp as WcTyp from " + tableName + " a join BBdbR_WcBase b on a.WcId=b.WcId where a.Enabled=1 and b.Enabled=1";     //===����ʱ��Ҫ�޸�===
                    dt = Repository().FindTableBySql(sql.ToString(), false);
                }
                else{
                    sql = "select a.*,b.WcCd as WcCd,b.WcNm as WcNm,b.WcTyp as WcTyp from " + tableName + " a join BBdbR_WcBase b on a.WcId=b.WcId where a.Enabled=1 and b.Enabled=1 and  " + Condition + " like  '%" + keywords + "%'";     //===����ʱ��Ҫ�޸�===
                    dt = Repository().FindTableBySql(sql.ToString(), false);
                }
            }
            return dt;
            #region ԭ��
            //string sql = "";
            //List<BBdbR_PostBase> listEntity = new List<BBdbR_PostBase>();
            //if (Condition == "all")
            //{
            //    sql = @"select * from " + tableName + " where Enabled ='1'";
            //    listEntity = Repository().FindListBySql(sql);
            //    for (int i = 0; i < listEntity.Count; i++)
            //    {
            //        string sql1 = "select * from BBdbR_WcBase where WcCd='" + listEntity[i].WcCd + "'";
            //        DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
            //        if (dt1.Rows.Count > 0)
            //        {
            //            listEntity[i].WcCd = dt1.Rows[0]["WcNm"].ToString();
            //        }
            //    }
            //}
            //else
            //{
            //    if (keywords == "all")
            //    {
            //        sql = @"select * from " + tableName + " where Enabled ='1'";
            //        listEntity = Repository().FindListBySql(sql);
            //        for (int i = 0; i < listEntity.Count; i++)
            //        {
            //            string sql1 = "select * from BBdbR_WcBase where WcCd='" + listEntity[i].WcCd + "'";
            //            DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
            //            if (dt1.Rows.Count > 0)
            //            {
            //                listEntity[i].WcCd = dt1.Rows[0]["WcNm"].ToString();
            //            }
            //        }
            //    }
            //    else
            //    {
            //        //����������ѯ
            //        sql = @"select * from " + tableName + " where Enabled ='1' and  " + Condition + " like  '%" + keywords + "%'";
            //        listEntity = Repository().FindListBySql(sql);
            //        for (int i = 0; i < listEntity.Count; i++)
            //        {
            //            string sql1 = "select * from BBdbR_WcBase where WcCd='" + listEntity[i].WcCd + "'";
            //            DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
            //            if (dt1.Rows.Count > 0)
            //            {
            //                listEntity[i].WcCd = dt1.Rows[0]["WcNm"].ToString();
            //            }
            //        }
            //    }
            //}
            //return listEntity;
            #endregion
        }

        /// <summary>
        /// �༭����������Դ
        /// </summary>
        /// <returns></returns>
        public BBdbR_PostBase GetPlanList(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_PostBase where  PostId='" + KeyValue + "'");
            List<BBdbR_PostBase> dt = Repository().FindListBySql(strSql.ToString());
            BBdbR_PostBase Dvcentity = new BBdbR_PostBase();
            Dvcentity = dt[0];
            return Dvcentity;
        }
        #endregion

        #region 8.��õ���ģ��
        public void GetExcellTemperature(string ImportId, out DataTable data, out string DataColumn, out string fileName)
        {
            DataColumn = "";
            data = new DataTable();
            Base_ExcelImport base_excelimport = DataFactory.Database().FindEntity<Base_ExcelImport>(ImportId);
            fileName = base_excelimport.ImportFileName;
            List<Base_ExcelImportDetail> listBase_ExcelImportDetail = DataFactory.Database().FindList<Base_ExcelImportDetail>("ImportId", ImportId);
            object[] rows = new object[listBase_ExcelImportDetail.Count];
            int i = 0;
            foreach (Base_ExcelImportDetail excelImportDetail in listBase_ExcelImportDetail)
            {
                if (DataColumn == "")
                {
                    DataColumn = DataColumn + excelImportDetail.ColumnName;
                }
                else
                {
                    DataColumn = DataColumn + "|" + excelImportDetail.ColumnName;
                }
                switch (excelImportDetail.DataType)
                {
                    //�ַ���
                    case "0":
                        data.Columns.Add(excelImportDetail.ColumnName, typeof(string));
                        rows[i] = "";
                        break;
                    //����
                    case "1":
                        data.Columns.Add(excelImportDetail.ColumnName, typeof(decimal));
                        rows[i] = "";
                        break;
                    //����
                    case "2":
                        data.Columns.Add(excelImportDetail.ColumnName, typeof(DateTime));
                        rows[i] = DateTime.Now;
                        break;
                    //���
                    case "3":
                        data.Columns.Add(excelImportDetail.ColumnName, typeof(string));
                        rows[i] = "";
                        break;
                    //Ψһʶ��
                    case "4":
                        data.Columns.Add(excelImportDetail.ColumnName, typeof(string));
                        rows[i] = "";
                        break;
                    default:
                        break;
                }
                i++;
            }
            data.Rows.Add(rows);

        }
        #endregion

    }
}