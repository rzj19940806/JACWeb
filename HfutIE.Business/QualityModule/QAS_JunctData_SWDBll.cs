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
    /// ���ӵ������������_SWD
    /// <author>
    ///	<name>CHFAS</name>
    ///	<date>2021.07.20 12:00</date>
    /// </author>
    /// </summary>
    public class QAS_JunctData_SWDBll : RepositoryFactory<QAS_JunctData_SWD>
    {
        #region
        #endregion

        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı��ı�������Ϊ����
        string tableName = "QAS_JunctData_SWD";

        #endregion

        #region ������
        public int ImportSWDData(string moduleId, DataTable dt, string BodyNo, string CarType, DateTime CheckTm, out DataTable Result)
        {
            int IsOk = 0;//����״̬
            bool CheckEx = false;
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();

            //���쵼�뷵�ؽ����
            DataTable Newdt = new DataTable("Result");
            Newdt.Columns.Add("rowid", typeof(System.String));//�к�
            Newdt.Columns.Add("locate", typeof(System.String));//λ��
            Newdt.Columns.Add("reason", typeof(System.String));//ԭ��
            //Newdt = JudgeOrder(dt, Newdt);//�ظ����ж�
            //bool isExit = Newdt.Rows.Count > 0 ? true : false;
            int errorNum = 1;
            try
            {
                //ģ������
                Base_ExcelImport base_excellimport = DataFactory.Database().FindEntity<Base_ExcelImport>("ModuleId", moduleId);
                if (base_excellimport.ImportId == null)
                {
                    IsOk = 0;
                }
                else
                {
                    string pkName = new Base_DataBaseBll().GetPrimaryKey(base_excellimport.ImportTable);
                    //ģ����ϸ��
                    List<Base_ExcelImportDetail> listBase_ExcelImportDetail = DataFactory.Database().FindList<Base_ExcelImportDetail>("ImportId", base_excellimport.ImportId);
                    string tableName = base_excellimport.ImportTable;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        #region ��ɾ��ԭʼ����
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("delete QAS_JunctData_SWD where 1=1 and BodyNo = '" + BodyNo + "'");
                        int ext = database.ExecuteBySql(strSql, isOpenTrans);
                        #endregion

                        #region ����Excel������
                        int rowNum = 1;
                        foreach (DataRow item in dt.Rows)
                        {
                            Hashtable entity = new Hashtable();//����Ҫ�������ݿ��hashtable
                            StringBuilder sb = new StringBuilder();
                            entity[pkName] = Guid.NewGuid().ToString();//���ȸ�������ֵ
                            StringBuilder rowSb = new StringBuilder();//�ۼ�ÿһ����Ԫ���ֵ��һ��ȫ�վ�ֹͣ����
                            #region ����ģ�壬Ϊÿһ����ÿ���ֶ��ҵ�ģ���в���ֵ
                            foreach (Base_ExcelImportDetail excelImportDetail in listBase_ExcelImportDetail)
                            {
                                string value = "";
                                value = item[excelImportDetail.ColumnName].ToString();
                                rowSb.Append(value);//�ۼ�ÿһ����Ԫ���ֵ��һ��ȫ�վ�ֹͣ����
                                DateTime dateTime = DateTime.Now;
                                float num = 0;
                                #region �����ֶθ�ֵ
                                switch (excelImportDetail.DataType)
                                {
                                    //�ַ���
                                    case "0":
                                        entity[excelImportDetail.FieldName] = value;
                                        break;
                                    //����
                                    case "1":
                                        if (Single.TryParse(value, out num))
                                        {
                                            entity[excelImportDetail.FieldName] = value;
                                        }
                                        else
                                        {
                                            if (base_excellimport.ErrorHanding == "0")
                                            {
                                                CheckEx = true;
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
                                                CheckEx = true;
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
                                                CheckEx = true;
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
                                                CheckEx = true;
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
                            if (CheckEx)  //�����д�������ع�
                            {
                                database.Rollback();
                                break;
                            }
                            #region �Զ����ֶθ�ֵ
                            entity["CarType"] = CarType;
                            entity["BodyNo"] = BodyNo;
                            entity["CheckTm"] = CheckTm;
                            entity["Category"] = "SWD";
                            entity["CreTm"] = DateTime.Now;
                            entity["CreCd"] = ManageProvider.Provider.Current().UserId;
                            entity["CreNm"] = ManageProvider.Provider.Current().UserName;
                            #endregion
                            database.Insert(base_excellimport.ImportTable, entity, isOpenTrans);
                            rowNum++;
                        }
                        #endregion ����Excel�����н���
                        if (!CheckEx) database.Commit();
                        IsOk = 1;
                    }
                }
            }
            catch (System.Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "�쳣����" + ex.Message);
                IsOk = -1;
            }
            Result = Newdt;
            return IsOk;
        }
        /// <summary>
        /// ��ȡSWD���������
        /// </summary>
        /// <param name="PropertyName"></param>
        /// <param name="PropertyValue"></param>
        /// <returns></returns>
        public List<QAS_JunctData_SWD> GetPageList(string PropertyName, string PropertyValue)
        {
            List<QAS_JunctData_SWD> dt;
            string sql = "";
            if (PropertyName == "")
            {
                sql = "select * from " + tableName + " where 1=1 order by Code";
            }
            else
            {
                //����������ѯ
                sql = "select * from " + tableName + " where " + PropertyName + " = '" + PropertyValue + "' order by Code";         
            }
            dt = Repository().FindListBySql(sql);
            return dt;
        }
        #endregion

    }
}