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
    /// ���Ż�����Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.22 16:10</date>
    /// </author>
    /// </summary>
    public class DepartmentBll : RepositoryFactory<BBdbR_DepartmentBase>
    {
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_DepartmentBase";
        public readonly RepositoryFactory<BBdbR_DepartmentBase> repositoryFactory = new RepositoryFactory<BBdbR_DepartmentBase>();
        #endregion

        #region 1.��ȡ������Ҫ�޸�sql���
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===����ʱ��Ҫ�޸�=== CompanyId AS Attribute,
            strSql.Append(@"select    
                              CompanyId AS keys,
                              CompanyCd AS code,
                              CompanyNm AS name,
                              Enabled As IsAvailable,
                              '0' AS parentId,
                              CompanyId AS companyid,
                              '1' as sort 
                              from BBdbR_CompanyBase where Enabled = '1'
							  union
							  select  DepartmentID AS keys,     
                              DepartmentCode AS code,
                              DepartmentName AS name,
                              Enabled As IsAvailable,
                              ParentDepartmentID as parentId,  
                              CompanyId AS companyid,
                              '1' as sort 
                              from BBdbR_DepartmentBase where Enabled = '1'  order by code asc");
                
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.�����չʾ�����Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ�������š�   --����-->   �������� 
        ///     ���ݴ���Ĳ�����ͬд��ͬ��sql�����в�ѯ           
        /// </summary>
        /// <param name="areaId">����Ľڵ������</param>
        /// <param name="parentId">�ڵ�ĸ�������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetList(string areaId, string parentId, string Condition, string keywords, ref JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            DataTable dt = new DataTable();
            List<DbParameter> parameter = new List<DbParameter>();
            if (parentId == "0"||String.IsNullOrEmpty(parentId))//�����˾�ڵ�
            {
                
                sql = "select a.*,b.CompanyCd as CompanyCd,b.CompanyNm as CompanyNm from " + tableName + " a join BBdbR_CompanyBase b on a.CompanyId=b.CompanyId  where a.Enabled=1 and b.Enabled=1  and ParentDepartmentID = b.CompanyId  ";     //===����ʱ��Ҫ�޸�===  and a.ParentDepartmentID = '" + areaId + "'
                
                if (Condition == "all"||String.IsNullOrEmpty(Condition))
                {

                }
                else
                {

                    sql += " and  a." + Condition + " like  @keywords "; 
                    parameter.Add(DbFactory.CreateDbParameter("@keywords", "%" + keywords + "%"));
                    //sql += " and  a." + Condition + " like  '%" + keywords + "%' ";
                }
                sql += "  order by DepartmentCode asc";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            else
            {
                //���ĳ����
                sql = "select a.*,c.CompanyCd as CompanyCd,c.CompanyNm as CompanyNm from " + tableName + " a join BBdbR_CompanyBase c on a.CompanyId=c.CompanyId  where a.Enabled=1 and c.Enabled=1  and a.ParentDepartmentID='" + areaId + "'";     //===����ʱ��Ҫ�޸�===
                if (Condition == "all" || String.IsNullOrEmpty(Condition))
                {

                }
                else
                {
                    sql += " and  a." + Condition + " like  @keywords ";
                    parameter.Add(DbFactory.CreateDbParameter("@keywords", "%" + keywords + "%"));
                    //sql += " and  a." + Condition + " like  '%" + keywords + "%' ";
                }
                sql += "  order by DepartmentCode asc";
                //dt = Repository().FindTableBySql(sql.ToString(), false);
                dt = DataFactory.Database().FindTableBySql(sql, parameter.ToArray(), false);
            }
            return dt;           
        }

        #endregion

        #region 3.չʾ���
        ///// <summary>
        ///// �������������Enabled = true������, ��Ϊ��Ч�Ĺ�����ϢGetDepartmentBase
        ///// </summary>
        ///// <param name="jqgridparam">��ҳ����</param>
        ///// <returns>����������������</returns>
        //public List<BBdbR_DepartmentBase> GetPageList(JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append(@"SELECT  * FROM  BBdbR_DepartmentBase where 1=1");
        //    List<BBdbR_DepartmentBase> dt = Repository().FindListBySql(strSql.ToString());
        //    for (int i = 0; i < dt.Count; i++)
        //    {
        //        string sql = "select * from BBdbR_FacBase where FacId='" + dt[i].FacId + "'";
        //        DataTable dt1 = Repository().FindTableBySql(sql.ToString());
        //        if (dt1.Rows.Count > 0)
        //        {
        //            dt[i].FacId = dt1.Rows[0]["FacNm"].ToString();
        //        }
        //    }
        //    return dt;
        //}

       /// <summary>
       /// �������༭��������Դ
       /// </summary>
       /// <param name="KeyValue"></param>
       /// <returns></returns>
        public DataTable GetDepartmentBase(string KeyValue) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            DataTable dt = new DataTable();
            if (KeyValue != "")
            {
                sql = "select a.*,b.CompanyCd as CompanyCd,b.CompanyNm as CompanyNm from " + tableName + " a join BBdbR_CompanyBase b on a.CompanyId=b.CompanyId where a.Enabled=1 and b.Enabled=1 and a.DepartmentID='" + KeyValue+"'";     //===����ʱ��Ҫ�޸�===
                dt = Repository().FindTableBySql(sql.ToString(), false);
                return dt;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 4.�����༭����
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
        public int Update(BBdbR_DepartmentBase entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
        }
        public int CheckCount(string tableName, string KeyName, string KeyValue)
        {
            //string sql = @"select * from " + tableName + " where Enabled = 1 and " + KeyName + " = '" + KeyValue + "'";
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(BBdbR_DepartmentBase entity) //===����ʱ��Ҫ�޸�===
        {      
            return Repository().Insert(entity);
        }
        #endregion

        #region 5.ɾ������
        public int Delete(string[] array)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_DepartmentBase> listEntity = new List<BBdbR_DepartmentBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            { 
                //===����ʱ��Ҫ�޸�===
                BBdbR_DepartmentBase entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = 0;          
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
        }
        #endregion

        #region 6.��ѯ��������Ҫ�޸�sql���
        /// <summary>
        ///     ��ѯʱ�ṩ�������ؼ��֣�һ����Condition����һ����keywords 
        ///     Condition�ǹؼ��֣�����ѯ��������Ӧ���ݿ��е�һ���ֶ�
        ///     keywords�ǲ�ѯֵ������ѯ�����ľ���ֵ����Ӧ���ݿ��в�ѯ�����ֶε�ֵ
        ///     ��ѯ��ʱ�򴫵���Condition��keywords
        /// 
        /// </summary>
        /// <param name="keywords">��ѯֵ</param>
        /// <param name="Condition">�ؼ��֣���ѯ������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetPageListByCondition(string areaId, string parentId, string Condition, string keywords, ref JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            #region ԭ�汾
            //string sql = "";
            //DataTable dt = new DataTable();
            //if (Condition == "all")
            //{
            //    sql = "select a.*,b.CompanyCd as CompanyCd,b.CompanyNm as CompanyNm,c.DepartmentName as ParentDepartmentName,c.DepartmentCode as ParentDepartmentCode from " + tableName + " a join BBdbR_CompanyBase b on a.CompanyId=b.CompanyId left join BBdbR_DepartmentBase c on  a.ParentDepartmentID=c.DepartmentID where a.Enabled=1 and b.Enabled=1";     //===����ʱ��Ҫ�޸�===
            //}
            //else
            //{
            //    if (keywords == "all")
            //    {
            //        sql = "select a.*,b.CompanyCd as CompanyCd,b.CompanyNm as CompanyNm,c.DepartmentName as ParentDepartmentName,c.DepartmentCode as ParentDepartmentCode from " + tableName + " a join BBdbR_CompanyBase b on a.CompanyId=b.CompanyId left join BBdbR_DepartmentBase c on  a.ParentDepartmentID=c.DepartmentID where a.Enabled=1 and b.Enabled=1";     //===����ʱ��Ҫ�޸�===
            //    }
            //    else
            //    {
            //        sql = "select a.*,b.CompanyCd as CompanyCd,b.CompanyNm as CompanyNm,c.DepartmentName as ParentDepartmentName,c.DepartmentCode as ParentDepartmentCode from " + tableName + " a join BBdbR_CompanyBase b on a.CompanyId=b.CompanyId left join BBdbR_DepartmentBase c on  a.ParentDepartmentID=c.DepartmentID where a.Enabled=1 and b.Enabled=1 and a." + Condition + " like  '%" + keywords + "%'";      //===����ʱ��Ҫ�޸�===
            //    }
            //}
            //return Repository().FindTableBySql(sql.ToString(), false);
            #endregion

            #region �޸İ汾
            string sql = "";
            DataTable dt = new DataTable();
            List<DbParameter> parameter = new List<DbParameter>();
            if (parentId == "0" || String.IsNullOrEmpty(parentId))//�����˾�ڵ�
            {

                sql = "select a.*,b.CompanyCd as CompanyCd,b.CompanyNm as CompanyNm from " + tableName + " a join BBdbR_CompanyBase b on a.CompanyId=b.CompanyId  where a.Enabled=1 and b.Enabled=1  and ParentDepartmentID = b.CompanyId  ";     //===����ʱ��Ҫ�޸�===  and a.ParentDepartmentID = '" + areaId + "'
                if (Condition == "all" || String.IsNullOrEmpty(Condition))
                {

                }
                else
                {
                    sql += " and  a." + Condition + " like  @keywords ";
                    parameter.Add(DbFactory.CreateDbParameter("@keywords", "%" + keywords + "%"));
                    //sql += " and  a." + Condition + " like  '%" + keywords + "%' ";
                }
                sql += "  order by DepartmentCode asc";
                

            }
            else
            {
                //���ĳ����
                sql = "select a.*,c.CompanyCd as CompanyCd,c.CompanyNm as CompanyNm from " + tableName + " a join BBdbR_CompanyBase c on a.CompanyId=c.CompanyId  where a.Enabled=1 and c.Enabled=1  and a.ParentDepartmentID='" + areaId + "'";     //===����ʱ��Ҫ�޸�===
                if (Condition == "all" || String.IsNullOrEmpty(Condition))
                {

                }
                else
                {
                    sql += " and  a." + Condition + " like  @keywords ";
                    parameter.Add(DbFactory.CreateDbParameter("@keywords", "%" + keywords + "%"));
                    //sql += " and  a." + Condition + " like  '%" + keywords + "%' ";
                }
                sql += "  order by DepartmentCode asc";
                
            }
            //dt = Repository().FindTableBySql(sql.ToString(), false);
            dt = DataFactory.Database().FindTableBySql(sql, parameter.ToArray(), false);
            return dt;
            #endregion
        }

        #endregion

        #region 7.ɾ��ʱʹ�ã����ұ�����ĳһ�����������Ƿ������������е�����
        //�ж�Ҫɾ�������������Ƿ�����������е�����
        //�磺��˾��Ϣ����һ����˾��Ϣ����һ���󶨶���������Ϣ
        //    ��ʾ��Щ���������ڸù�˾�ġ�
        //    ��ô��ɾ������һ����˾��Ϣ֮ǰ����Ҫ�жϸù�˾��Ϣ�����Ƿ���˹�����Ϣ
        //
        //�����������þ����жϸù�˾�����Ƿ��й�����Ϣ
        //Condition��ʾҪ��ѯ�ı��е��ֶ����������б�ʾ����������Ϣ���еĹ�˾����
        //keyValue��ʾҪ��ѯ�ı����ֶε�ֵ��
        //����ֵ��ʾ��ѯ���˶���������
        public int FindChildCount(string tableName, string Condition, string keyValue)
        {
            string sql = "select * from " + tableName + " where " + Condition + " = '" + keyValue + "'";
            int count = Repository().FindCountBySql(sql);
            return count;
        }

        /// �༭����������Դ
        /// </summary>
        /// <returns></returns>
        public BBdbR_DepartmentBase GetPlanList(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_DepartmentBase where DepartmentID='" + KeyValue + "'");
            List<BBdbR_DepartmentBase> dt = Repository().FindListBySql(strSql.ToString());
            BBdbR_DepartmentBase Dvcentity = new BBdbR_DepartmentBase();
            Dvcentity = dt[0];
            return Dvcentity;
        }
        #endregion

        #region 8.��������Ա
        /// <summary>
        /// ��ȡ������Ա
        /// </summary>
        /// <returns></returns>
        public DataTable GetPlineNm(string StfId)
        {
            string sql = "";
            if (string.IsNullOrEmpty(StfId))
            {
                 sql = @"select UserId as id,Code,RealName,Telephone,Email from Base_User where Enabled='1' order by RealName desc";
            }
            else
            {
                sql = @"select UserId as id,Code,RealName,Telephone,Email from Base_User where Enabled='1' and UserId=" + "'" + StfId + "' order by RealName desc";
            }
            return Repository().FindTableBySql(sql, false);
        }
        

        /// <summary>
        /// ��ȡ���и�����
        /// </summary>
        /// <returns></returns>
        public DataTable GetPDepartMent()
        {
            string sql = @"select DepartmentID,DepartmentName from BBdbR_DepartmentBase where Enabled='1'
                          union 
                          select CompanyId as DepartmentID,CompanyNm as DepartmentName from  BBdbR_CompanyBase where Enabled = '1'
";
            return Repository().FindTableBySql(sql, false);
        }
        #endregion

        #region 9.���Ҹ��������Ƿ����Ӳ���
        /// <summary>
        /// ��������й��ڸ��������µ��Ӳ�������
        /// </summary>
        /// <param name="KeyValue">��ҳ����</param>
        /// <returns>����������������</returns>
        public int GetParentDepartCount(string KeyValue) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            int count = 0;
            if (KeyValue != "")
            {
                sql = @"select * from " + tableName + " where Enabled='1' and ParentDepartmentID='" + KeyValue + "'";
                DataTable dt = Repository().FindTableBySql(sql);
                
                count = dt.Rows.Count;
            }
            return count;
        }
        #endregion

        #region 9.��ȡ����������

        public DataTable GetDepartmentList()
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();

            strSql.Append(@"SELECT DepartmentID, b.CompanyId, DepartmentCode,	DepartmentName  ,CompanyNm
                                                			
                                      FROM    BBdbR_DepartmentBase a left join BBdbR_CompanyBase b on a.CompanyId = b.CompanyId  where a.Enabled = '1' and b.Enabled = '1' ");
            return dt = Repository().FindTableBySql(strSql.ToString(), false);
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

    }
}




