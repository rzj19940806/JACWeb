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
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// ����λ���������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.10.09 15:36</date>
    /// </author>
    /// </summary>
    public class BBdbR_QualityCarPositionGroupBaseBll : RepositoryFactory<BBdbR_QualityCarPositionGroupBase>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_QualityCarPositionGroupBase";//===����ʱ��Ҫ�޸�===
        public readonly RepositoryFactory<BBdbR_QualityCarPositionGroupBase> repository_User = new RepositoryFactory<BBdbR_QualityCarPositionGroupBase>();
        #endregion

        #region 1.��ȡ������Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ��������λ���������Ϣ��   --����-->   ������λ��
        /// </summary>
        /// <returns></returns>
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===����ʱ��Ҫ�޸�===��һ������
            //����λ����ڵ�
            strSql.Append(@" select    
                                    CarPositionId AS keys,
                                    CarPositionCd AS code,
                                    CarPositionNm AS name,
                                    Enabled As IsAvailable,
                                    '0' AS parentId,
                                    '0' as sort 
                                    from BBdbR_QualityCarPositionBase where Enabled = '1' order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.�����չʾ�����Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ��������λ���������Ϣ��   --����-->   ������λ��
        ///     ���ݴ���Ĳ�����ͬд��ͬ��sql�����в�ѯ           
        /// </summary>
        /// <param name="areaId">����Ľڵ������</param>
        /// <param name="parentId">�ڵ�ĸ�������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetList(string areaId, string parentId, string CarPositionGroupCd, string CarPositionGroupNm, ref JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId))
            {
                sql = "select a.*,b.CarPositionCd AS CarPositionCd,b.CarPositionNm AS CarPositionNm from " + tableName + " a join BBdbR_QualityCarPositionBase b on a.CarPositionId=b.CarPositionId where a.Enabled=1 and b.Enabled = '1' ";     //===����ʱ��Ҫ�޸�===
            }
            else if(areaId != "all")
            {
                sql = "select a.*,b.CarPositionCd AS CarPositionCd,b.CarPositionNm AS CarPositionNm from " + tableName + " a join BBdbR_QualityCarPositionBase b on a.CarPositionId=b.CarPositionId where a.Enabled=1 and a.CarPositionId = '" + areaId + "'  ";     //===����ʱ��Ҫ�޸�===
            }
            else
            {
                sql = "select a.*,b.CarPositionCd AS CarPositionCd,b.CarPositionNm AS CarPositionNm from " + tableName + " a join BBdbR_QualityCarPositionBase b on a.CarPositionId=b.CarPositionId where a.Enabled=1 ";     //===����ʱ��Ҫ�޸�===
                
            }



            //if (CarPositionGroupCd != "" && CarPositionGroupCd != null)
            //{
            //    sql = sql + " and CarPositionGroupCd like '%" + CarPositionGroupCd + "%' ";
            //}
            //else { }
            //if (CarPositionGroupNm != "" && CarPositionGroupNm != null)
            //{
            //    sql = sql + " and CarPositionGroupNm like '%" + CarPositionGroupNm + "%' ";
            //}
            //else { }

            List<DbParameter> parameter = new List<DbParameter>();

            if (!String.IsNullOrEmpty(CarPositionGroupCd))
            {
                sql += " and CarPositionGroupCd like @CarPositionGroupCd ";
                parameter.Add(DbFactory.CreateDbParameter("@CarPositionGroupCd", "%" + CarPositionGroupCd + "%"));
            }
            if (!String.IsNullOrEmpty(CarPositionGroupNm))
            {
                sql += " and CarPositionGroupNm like @CarPositionGroupNm ";
                parameter.Add(DbFactory.CreateDbParameter("@CarPositionGroupNm", "%" + CarPositionGroupNm + "%"));
            }

            sql = sql + " order by CarPositionGroupNm ";
            //dt = Repository().FindTableBySql(sql.ToString(), false);
            dt = DataFactory.Database().FindTableBySql(sql, parameter.ToArray(), false);

            return dt;
        }
        #endregion

        #region 3.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_QualityCarPositionGroupBase entity) //===����ʱ��Ҫ�޸�===
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

        public int GetCheckList(string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and C '" + KeyValue + "'";
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
        public int Insert(BBdbR_QualityCarPositionGroupBase entity) //===����ʱ��Ҫ�޸�===
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
            List<BBdbR_QualityCarPositionGroupBase> listEntity = new List<BBdbR_QualityCarPositionGroupBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_QualityCarPositionGroupBase entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
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
            DataTable dt;
            if (Condition == "all")
            {
                sql = @"select a.*,b.CarPositionCd AS CarPositionCd,b.CarPositionNm AS CarPositionNm from " + tableName + " a join BBdbR_QualityCarPositionBase b on a.CarPositionId=b.CarPositionId where a.Enabled = '1'";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            else
            {
                //����������ѯ
                sql = @"select a.*,b.CarPositionCd AS CarPositionCd,b.CarPositionNm AS CarPositionNm from " + tableName + " a join BBdbR_QualityCarPositionBase b on a.CarPositionId=b.CarPositionId where a.Enabled = '1' and a." + Condition + " like  '%" + keywords + "%'";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
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