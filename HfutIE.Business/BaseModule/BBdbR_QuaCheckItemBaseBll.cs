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
    /// �����Ŀ������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.28 16:56</date>
    /// </author>
    /// </summary>
    public class BBdbR_QuaCheckItemBaseBll : RepositoryFactory<BBdbR_QuaCheckItemBase>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı��ı�������Ϊ����
        string tableName = "BBdbR_QuaCheckItemBase";//===����ʱ��Ҫ�޸�===
        public readonly RepositoryFactory<BBdbR_QuaCheckItemBase> repository_User = new RepositoryFactory<BBdbR_QuaCheckItemBase>();
        #endregion

        #region 1.չʾ����
        /// <summary>
        /// ��������������IsAvailable = true������
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>����������������</returns>
        public List<BBdbR_QuaCheckItemBase> GetPageList(JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===

        {
            string sql = @"select * from " + tableName + " where Enabled = '1'";
            return Repository().FindListPageBySql(sql, ref jqgridparam);
        }
        #endregion
        #region 2.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_QuaCheckItemBase entity) //===����ʱ��Ҫ�޸�===
        {
            
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
        }
        #endregion
        #region 3.���ĳ���ֶε��ֶ�ֵ�Ƿ����
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
        public int Insert(BBdbR_QuaCheckItemBase entity) //===����ʱ��Ҫ�޸�===
        {
            
            return Repository().Insert(entity);
        }
        #endregion

        #region 5.ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(string[] array)
        {
            //����һ�������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_QuaCheckItemBase> listEntity = new List<BBdbR_QuaCheckItemBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_QuaCheckItemBase entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                entity.Modify(entity.QuaCheckItemId);
                listEntity.Add(entity);
            }

            return Repository().Update(listEntity);//��ɾ���ݿ�
        }
        #endregion

        #region 6.��ѯ��������Ҫ�޸�sql���
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
        /// <returns>��ѯ�����ݣ��б���</returns>
        public List<BBdbR_QuaCheckItemBase> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            List<BBdbR_QuaCheckItemBase> dt;
            if (Condition == "all")
            {
                sql = @"select * from " + tableName + " where 1 = 1 and Enabled = '1'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            else
            {
                //����������ѯ
                sql = @"select * from " + tableName + " where 1 = 1 and Enabled = '1' and " + Condition + " like  '%" + keywords + "%'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            return dt;
        }
        #endregion

        #region 7.��ѯ�����Ŀ
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ�����
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        //public List<BBdbR_QuaCheckItemBase> GetPlanList()
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append(@"SELECT  * FROM  BBdbR_QuaCheckItemBase where Enabled = '1' and MatCatg = '��Ʒ'");
        //    List<BBdbR_QuaCheckItemBase> dt = Repository().FindListBySql(strSql.ToString());
        //    return dt;
        //}
        /// <summary>
        /// ��ѯ�����Ŀ
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public BBdbR_QuaCheckItemBase GetPlanList1(string QuaCheckItemId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_QuaCheckItemBase where Enabled = '1'  and QuaCheckItemId='" + QuaCheckItemId + "'");
            List<BBdbR_QuaCheckItemBase> dt = Repository().FindListBySql(strSql.ToString());
            if (dt.Count > 0)
            {
                return dt[0];
            }
            else
            {
                return null;
            }

        }
        #endregion

        #region ��ѯ���ñ��в����ڵļ����Ŀ��Ϣ
        public List<BBdbR_QuaCheckItemBase> GetCheckItemList(string CarPartId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_QuaCheckItemBase where Enabled = '1'  and QuaCheckItemId not in (SELECT QuaCheckItemId  FROM  BBdbR_CarPartQuaCheckItemConfig where Enabled = '1' and CarPartId = '" + CarPartId + "' )");
            List<BBdbR_QuaCheckItemBase> dt = Repository().FindListBySql(strSql.ToString());
            return dt;

        }

        #endregion

        #region ��õ���ģ��
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

        #region ��������
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="dt">Excel����</param>
        /// <returns></returns>
        public int ImportExcel(string moduleId, DataTable dt, out DataTable Result)
        {
            //���쵼�뷵�ؽ����
            DataTable Newdt = new DataTable("Result");
            Newdt.Columns.Add("rowid", typeof(System.String));                 //�к�
            Newdt.Columns.Add("locate", typeof(System.String));                 //λ��
            Newdt.Columns.Add("reason", typeof(System.String));                 //ԭ��
            int IsOk = 0;
            //��õ���ģ��
            //ģ������
            Base_ExcelImport base_excellimport = DataFactory.Database().FindEntity<Base_ExcelImport>("ModuleId", moduleId);
            if (base_excellimport.ImportId == null)
            {
                IsOk = 0;
            }
            else
            {
                //string pkName = new Base_DataBaseBll().GetPrimaryKey(base_excellimport.ImportTable);//��������
                //ģ����ϸ��
                List<Base_ExcelImportDetail> listBase_ExcelImportDetail = DataFactory.Database().FindList<Base_ExcelImportDetail>("ImportId", base_excellimport.ImportId);
                //ȡ��Ҫ����ı���
                string tableName = base_excellimport.ImportTable;
                if (dt != null && dt.Rows.Count > 0)
                {
                    bool isExit = false;
                    IDatabase database = DataFactory.Database();
                    DbTransaction isOpenTrans = database.BeginTrans();
                    try
                    {
                        #region ����Excel������
                        int rowNum = 1;
                        int errorNum = 1;
                        foreach (DataRow item in dt.Rows)
                        {
                            Hashtable entity = new Hashtable();//����Ҫ�������ݿ��hashtable
                            StringBuilder sb = new StringBuilder();

                            //if (tableName == "A_OrderBase")
                            //{
                            //    entity[pkName] = int.Parse(GetNextKey(tableName, pkName.ToString()).ToString());
                            //}
                            //else
                            //{
                            //    entity[pkName] = Guid.NewGuid().ToString();//���ȸ�������ֵ
                            //}



                            StringBuilder rowSb = new StringBuilder();//�ۼ�ÿһ����Ԫ���ֵ��һ��ȫ�վ�ֹͣ����
                            #region ����ģ�壬Ϊÿһ����ÿ���ֶ��ҵ�ģ���в���ֵ
                            foreach (Base_ExcelImportDetail excelImportDetail in listBase_ExcelImportDetail)
                            {
                                string value = "";
                                value = item[excelImportDetail.ColumnName].ToString();
                                rowSb.Append(value);//�ۼ�ÿһ����Ԫ���ֵ��һ��ȫ�վ�ֹͣ����
                                DateTime dateTime = DateTime.Now;
                                decimal num = 0;
                                #region �����ֶθ�ֵ
                                switch (excelImportDetail.DataType)
                                {
                                    //�ַ���
                                    case "0":

                                        entity[excelImportDetail.FieldName] = value;
                                        break;
                                    //����
                                    case "1":
                                        if (decimal.TryParse(value, out num))
                                        {
                                            entity[excelImportDetail.FieldName] = value;
                                        }
                                        else
                                        {
                                            if (base_excellimport.ErrorHanding == "0")
                                            {
                                                isExit = true;
                                            }
                                            DataRow dr = Newdt.NewRow();
                                            dr = Newdt.NewRow();
                                            dr[0] = errorNum;
                                            dr[1] = "��[" + rowNum.ToString() + "]��[" + excelImportDetail.ColumnName + "]";
                                            dr[2] = "���ָ�ʽ����ȷ";
                                            Newdt.Rows.Add(dr);
                                            errorNum++;
                                            continue;
                                        }
                                        break;
                                    //����
                                    case "2":
                                        if (DateTime.TryParse(value, out dateTime))
                                        {
                                            entity[excelImportDetail.FieldName] = value;
                                        }
                                        else
                                        {
                                            if (base_excellimport.ErrorHanding == "0")
                                            {
                                                isExit = true;
                                            }
                                            DataRow dr = Newdt.NewRow();
                                            dr = Newdt.NewRow();
                                            dr[0] = errorNum;
                                            dr[1] = "��[" + rowNum.ToString() + "]��[" + excelImportDetail.ColumnName + "]";
                                            dr[2] = "���ڸ�ʽ����ȷ";
                                            Newdt.Rows.Add(dr);
                                            errorNum++;
                                            continue;
                                        }
                                        break;
                                    //���
                                    case "3":
                                        sb.Clear();
                                        sb.Append(" and ");
                                        sb.Append(excelImportDetail.CompareField);
                                        sb.Append("='");
                                        sb.Append(value);
                                        sb.Append("' ");
                                        Hashtable htf = new Hashtable();
                                        //�ֶ�ֵ�ǿղ�ȥ�����
                                        if (!string.IsNullOrEmpty(value))
                                        {
                                            htf = database.FindHashtable(excelImportDetail.ForeignTable, sb);
                                        }


                                        if (htf.Count > 0)
                                        {
                                            entity[excelImportDetail.FieldName] = htf[excelImportDetail.BackField.ToLower()];
                                        }
                                        else
                                        {
                                            if (base_excellimport.ErrorHanding == "0")
                                            {
                                                isExit = true;
                                            }
                                            DataRow dr = Newdt.NewRow();
                                            dr = Newdt.NewRow();
                                            dr[0] = errorNum;
                                            dr[1] = "��[" + rowNum.ToString() + "]��[" + excelImportDetail.ColumnName + "]";
                                            dr[2] = excelImportDetail.ColumnName + "��ϵͳ�в�����";
                                            Newdt.Rows.Add(dr);
                                            errorNum++;
                                            continue;
                                        }
                                        break;
                                    //Ψһʶ��
                                    case "4":
                                        //�жϸ�ֵ�Ƿ��ڱ����Ѵ���
                                        sb.Clear();
                                        sb.Append(" and ");
                                        sb.Append(excelImportDetail.FieldName);
                                        sb.Append("='");
                                        sb.Append(value);
                                        sb.Append("' ");
                                        Hashtable htm = database.FindHashtable(base_excellimport.ImportTable, sb);
                                        if (htm.Count > 0)
                                        {
                                            if (base_excellimport.ErrorHanding == "0")
                                            {
                                                isExit = true;
                                            }
                                            DataRow dr = Newdt.NewRow();
                                            dr = Newdt.NewRow();
                                            dr[0] = errorNum;
                                            dr[1] = "��[" + rowNum.ToString() + "]��[" + excelImportDetail.ColumnName + "]";
                                            dr[2] = excelImportDetail.ColumnName + "��ϵͳ���Ѵ��ڲ����ظ�����";
                                            Newdt.Rows.Add(dr);
                                            errorNum++;
                                            continue;
                                        }
                                        entity[excelImportDetail.FieldName] = value;
                                        break;
                                    default:
                                        break;
                                }
                                #endregion ���ֶθ�ֵ����
                            }
                            #endregion ����ģ�����
                            //����������У�˵����Excel����������ж��ǿյģ����ٵ��룬��������һ������
                            if (string.IsNullOrEmpty(rowSb.ToString()))
                            {
                                Newdt.Rows.RemoveAt(Newdt.Rows.Count - 1);
                                break;
                            }
                            if (isExit)
                            {
                                break;
                            }

                            database.Insert(base_excellimport.ImportTable, entity, isOpenTrans);
                            rowNum++;
                        }
                        #endregion ����Excel�����н���
                        database.Commit();
                        IsOk = 1;
                    }
                    catch (System.Exception ex)
                    {
                        database.Rollback();
                        Base_SysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "�쳣����" + ex.Message);
                        IsOk = -1;
                    }
                }
            }
            Result = Newdt;
            return IsOk;
        }
        #endregion
    }
}