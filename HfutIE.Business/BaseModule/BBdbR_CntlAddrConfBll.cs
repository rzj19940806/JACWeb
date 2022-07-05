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
using System.Web.Mvc;

namespace HfutIE.Business
{
    /// <summary>
    /// ���ɵ�ַ��Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.15 10:02</date>
    /// </author>
    /// </summary>
    public class BBdbR_CntlAddrConfBll : RepositoryFactory<BBdbR_CntlAddrConf>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_CntlAddrConf";//===����ʱ��Ҫ�޸�===
        #endregion

        #region ������

        #region 1.ҳ����
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public DataTable GetPlanList()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"SELECT a.*,b.DvcNm as DvcNm FROM  " + tableName + " a join BBdbR_DvcBase b on a.DvcId=b.DvcId where a.Enabled=1 ");
            sql.Append(@"union SELECT a.*,b.WcNm as WcNm FROM  " + tableName + " a join BBdbR_WcBase b on a.WcId=b.WcId where a.Enabled=1 ");
            return Repository().FindTableBySql(sql.ToString(), false);
        }
        #endregion

        #region 2.��������
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
        public int Insert(BBdbR_CntlAddrConf entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Insert(entity);
        }
        #endregion

        #region 3.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_CntlAddrConf entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
        }
        #endregion

        #region 5.ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(string[] array)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_CntlAddrConf> listEntity = new List<BBdbR_CntlAddrConf>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_CntlAddrConf entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
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
        /// <returns>��ѯ�����ݣ��б�</returns>
        public DataTable GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            
            StringBuilder sql = new StringBuilder();
            DataTable dt =new DataTable();

            if (Condition == "all" || keywords == "")
            {
                //StringBuilder sql1 = new StringBuilder();
                sql.Append(@"SELECT a.*,b.DvcNm as DvcNm FROM  " + tableName + " a join BBdbR_DvcBase b on a.DvcId=b.DvcId where a.Enabled=1 ");
                sql.Append(@"union SELECT a.*,b.WcNm as WcNm FROM  " + tableName + " a join BBdbR_WcBase b on a.WcId=b.WcId where a.Enabled=1 ");
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            else
            {
                //����������ѯ
                //StringBuilder sql1 = new StringBuilder();
                sql.Append(@"SELECT a.*,b.DvcNm as DvcNm FROM  " + tableName + " a join BBdbR_DvcBase b on a.DvcId=b.DvcId where a.Enabled=1 and a." + Condition + " like  '%" + keywords + "%' ");
                sql.Append(@"union SELECT a.*,b.WcNm as WcNm FROM  " + tableName + " a join BBdbR_WcBase b on a.WcId=b.WcId where a.Enabled=1 and a." + Condition + " like  '%" + keywords + "%' ");
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }

            return dt;
        }
        #endregion

        #region 7.���������
        //11.��ȡ�������������
        public DataTable GetDvcNm()
        {
            string sql = @"select DvcId as id, DvcNm as dvcnm from BBdbR_DvcBase where Enabled='1'";
            return Repository().FindTableBySql(sql);
        }
        #endregion

        #region 6.����ģ��
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
        

        #region ����ʱ�豸ID
        public DataTable searchID(string dvcCd)
        {
            try
            {
                string sql = @"select DvcId as id from BBdbR_DvcBase where DvcCd = '" + dvcCd + "' and Enabled = 1";
                DataTable dtID = Repository().FindTableBySql(sql.ToString(), false);
                return dtID != null ? dtID : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        #endregion
    }
}