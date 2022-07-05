//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2022
// Software Developers @ HfutIE 2022
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
    /// �庸Ϳװ�������������̱�
    /// <author>
    ///		<name>she</name>
    ///		<date>2022.04.25 09:55</date>
    /// </author>
    /// </summary>
    public class Q_HTCarQualityOutput_ProBll : RepositoryFactory<Q_HTCarQualityOutput_Pro>
    {
        #region ��õ���ģ��
        /// <summary>
        /// ����ģ��
        /// </summary>
        /// <param name="ImportId"></param>
        /// <param name="data"></param>
        /// <param name="DataColumn"></param>
        /// <param name="fileName"></param>
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

        #region ��VIN�ź͹��β�ѯ
        public List<Q_HTCarQualityOutput_Pro> GetListByVIN(string VIN, string QualityCheckPointGroupNm) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            List<Q_HTCarQualityOutput_Pro> dt;
            sql = @"select * from Q_HTCarQualityOutput_Pro where Enabled = '1' and VIN = '" + VIN + "' and QualityCheckPointGroupNm = '" + QualityCheckPointGroupNm + "'";
            dt = Repository().FindListBySql(sql.ToString());
            return dt;
        }

        #endregion

        #region ���ݽ��������ɾ�������µ�����ȱ�ݼ�¼
        public int deleteByResult(string CarQualityResultId) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            List<Q_HTCarQualityOutput_Pro> dtList;
            sql = @"select * from Q_HTCarQualityOutput_Pro where Enabled = '1' and CarQualityResultId = '" + CarQualityResultId + "' ";
            dtList = Repository().FindListBySql(sql.ToString());
            foreach (var item in dtList)
            {
                item.Enabled = "0";
            }
            int num = Repository().Update(dtList);
            return num;//���ؽ��
        }
        #endregion

        #region ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(string KeyValue)
        {

            ////����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<Q_HTCarQualityOutput_Pro> listEntity = new List<Q_HTCarQualityOutput_Pro>(); //===����ʱ��Ҫ�޸�===
            //===����ʱ��Ҫ�޸�===
            Q_HTCarQualityOutput_Pro entity = Repository().FindEntity(KeyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
            entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
            entity.Modify(KeyValue);
            listEntity.Add(entity);
            return Repository().Update(listEntity);//�޸����ݿ�
        }
        #endregion



    }
}