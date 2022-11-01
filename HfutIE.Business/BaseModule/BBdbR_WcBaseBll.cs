//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// ��λ������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.23 21:51</date>
    /// </author>
    /// </summary>
    public class BBdbR_WcBaseBll : RepositoryFactory<BBdbR_WcBase>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_WcBase";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 1.��ȡ������Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ������λ��   --����-->   �����ߡ�  --����-->  �����Ρ�  --����-->  �����䡿
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
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.�����չʾ�����Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ��
        ///     ���ݴ���Ĳ�����ͬд��ͬ��sql�����в�ѯ           
        /// </summary>
        /// <param name="areaId">����Ľڵ������</param>
        /// <param name="parentId">�ڵ�ĸ�������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetList(string areaId,string parentId,string sort, ref JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            DataTable dt = new DataTable();
            var a = string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId);
            if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId))
            {
                sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId where a.Enabled=1 and b.Enabled=1 order by a.sort asc";     //===����ʱ��Ҫ�޸�===
            }
            else
            {
                if (parentId != "0")
                {
                    if (sort == "0")
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + areaId + "' order by a.sort asc";     //===����ʱ��Ҫ�޸�===
                    }
                    else if (sort == "1")
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + areaId + "' order by a.sort asc";     //===����ʱ��Ҫ�޸�===
                    }
                    else
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId where a.Enabled=1 and b.Enabled=1 and a.PlineId='" + areaId + "' order by a.sort asc";     //===����ʱ��Ҫ�޸�===Ҫ�޸�===
                    }
                }
                else
                {
                    sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on c.WorkshopId=d.WorkshopId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 and d.WorkshopId='" + areaId + "' order by a.sort asc";     //===����ʱ��Ҫ�޸�===
                }
            }
            return Repository().FindTableBySql(sql.ToString(), false);
        }
        #endregion

        #region 3.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_WcBase entity) //===����ʱ��Ҫ�޸�===
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

        public int hasChildNode(string WcId)
        {
            string sql = @"select * from BBdbR_PostBase where WcId = '" + WcId + "'";
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
        public int Insert(BBdbR_WcBase entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Insert(entity);
        }
        #endregion

        #region 6.ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int DeleteUseEnabled(string id)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_WcBase> listEntity = new List<BBdbR_WcBase>(); //===����ʱ��Ҫ�޸�===            
                //===����ʱ��Ҫ�޸�===
                BBdbR_WcBase entity = Repository().FindEntity(id);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = "0";
                listEntity.Add(entity);
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
            if (Condition == "all")
            {
                sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1  order by a.sort asc";
                return Repository().FindTableBySql(sql,false);
            }
            else
            {
                if (keywords != "all")
                {
                    //����������ѯ
                    sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 and a." + Condition + " like  '%" + keywords + "%' order by a.sort asc ";
                    return Repository().FindTableBySql(sql, false);
                }
                else
                {
                    sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 order by a.sort asc";
                    return Repository().FindTableBySql(sql, false);
                }
            }
        }
        //11.��ȡ������Ա
        public DataTable GetPlineNm(string DepartmentID)
        {
            string sql = @"select StfId as id, StfNm from BBdbR_StfBase where Enabled='1' and DepartmentID=" + "'" + DepartmentID + "'";
            return Repository().FindTableBySql(sql);
        }
        //��ȡ��Ա��Ϣ
        public DataTable GetPlineNm2(string StfId)
        {
            string sql = @"select stfnm,phn,email from BBdbR_StfBase where Enabled='1' and StfId=" + "'" + StfId + "'";
            return Repository().FindTableBySql(sql);
        }
        //11.��ȡ����
        public DataTable GetPlineNm1()
        {
            string sql = @"select DepartmentID as id, DepartmentName from BBdbR_DepartmentBase where Enabled=1";
            return Repository().FindTableBySql(sql);
        }
        #endregion

        #region 8.�༭����������Դ
        /// <summary>
        /// �༭����������Դ
        /// </summary>
        /// <returns></returns>
        public BBdbR_WcBase GetPlanList1(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_WcBase where  WcId='" + KeyValue + "'");
            List<BBdbR_WcBase> dt = Repository().FindListBySql(strSql.ToString());
            BBdbR_WcBase Dvcentity = new BBdbR_WcBase();         
            Dvcentity = dt[0];
            return Dvcentity;
        }
        #endregion

        #region 9.��õ���ģ��
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

        #region 10.���Ҳ����¹�λ��
        /// <summary>
        /// ���ҳ����¹��ն�����
        /// </summary>
        /// <returns></returns>
        public int GetWcCount(string KeyValue) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (KeyValue != "")
            {
                sql = @"select * from BBdbR_WcBase where Enabled='1' and PlineId='" + KeyValue + "'";
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

        #region �ʿص�
        #region 1.��ȡ������Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ������λ��   --����-->   �����ߡ�  --����-->  �����Ρ�  --����-->  �����䡿
        /// </summary>
        /// <returns></returns>
        public DataTable GetTreeQuality()
        {
            StringBuilder strSql = new StringBuilder();
            //===����ʱ��Ҫ�޸�===����������
            //����ڵ�
            strSql.Append(@"select c.WorkshopId As keys,
                                   c.WorkshopCd As code,
                                   c.WorkshopNm As name,
                                   c.Enabled As IsAvailable,
                                   '0' as parentId,
                                   '0' as sort ,c.sort as que             
                                   from BBdbR_WorkshopBase c where c.Enabled = '1' and c.WorkshopCd='ZLCJXN01'  ");
            //���νڵ�
            strSql.Append(@"union select 
                                    b.WorkSectionId AS keys,
                                    b.WorkSectionCd AS code, 
                                    b.WorkSectionNm AS name, 
                                    b.Enabled As IsAvailable,
                                    b.WorkshopId as parentId,
                                    '1' as sort  ,b.sort as que
                                    from BBdbR_WorkSectionBase b,BBdbR_WorkshopBase c where b.WorkshopId = c.WorkshopId and b.Enabled= '1'  ");
            //���߽ڵ�
            strSql.Append(@" union select    
                                    a.PlineId AS keys,
                                    a.PlineCd AS code,
                                    a.PlineNm AS name,
                                    a.Enabled As IsAvailable,
                                    a.WorkSectionId AS parentId,
                                    '2' as sort ,a.sort as que
                             from  BBdbR_PlineBase a,BBdbR_WorkSectionBase b 
                             where a.WorkSectionId=b.WorkSectionId and a.Enabled = '1' and a.Enabled = '1' order by que asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.�����չʾ�����Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ��
        ///     ���ݴ���Ĳ�����ͬд��ͬ��sql�����в�ѯ           
        /// </summary>
        /// <param name="areaId">����Ľڵ������</param>
        /// <param name="parentId">�ڵ�ĸ�������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetListQuality(string areaId, string parentId, string sort, ref JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            DataTable dt = new DataTable();
            var a = string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId);
            if (areaId == "''" && parentId == "''")
            {
                sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 and d.WorkshopCd='ZLCJXN01' order by a.sort asc";     //===����ʱ��Ҫ�޸�===
            }
            else
            {
                if (parentId != "0")
                {
                    if (sort == "0")
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + areaId + "' order by a.sort asc";     //===����ʱ��Ҫ�޸�===
                    }
                    else if (sort == "1")
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + areaId + "' order by a.sort asc";     //===����ʱ��Ҫ�޸�===
                    }
                    else
                    {
                        sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId where a.Enabled=1 and b.Enabled=1 and a.PlineId='" + areaId + "' order by a.sort asc";     //===����ʱ��Ҫ�޸�===Ҫ�޸�===
                    }
                }
                else
                {
                    sql = "select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on c.WorkshopId=d.WorkshopId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 and d.WorkshopId='" + areaId + "' order by a.sort asc";     //===����ʱ��Ҫ�޸�===
                }
            }
            return Repository().FindTableBySql(sql.ToString(), false);
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
        public DataTable GetPageListByConditionQuality(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (Condition == "all")
            {
                sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 and d.WorkshopCd='ZLCJXN01' order by a.sort asc";
                return Repository().FindTableBySql(sql, false);
            }
            else
            {
                if (keywords != "all")
                {
                    //����������ѯ
                    sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 and d.WorkshopCd='ZLCJXN01' and a." + Condition + " like  '%" + keywords + "%' order by a.sort asc ";
                    return Repository().FindTableBySql(sql.ToString(), false);
                }
                else
                {
                    sql = @"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from " + tableName + " a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 and d.WorkshopCd='ZLCJXN01' order by a.sort asc";
                    return Repository().FindTableBySql(sql.ToString(), false);
                }
            }
        }
        #endregion
        #endregion
    }
}