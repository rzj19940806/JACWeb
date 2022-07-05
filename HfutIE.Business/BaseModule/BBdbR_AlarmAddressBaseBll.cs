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
    /// �豸������ַ�����
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.17 19:38</date>
    /// </author>
    /// </summary>
    public class BBdbR_AlarmAddressBaseBll : RepositoryFactory<BBdbR_AlarmAddressBase>
    {
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_AlarmAddressBase";//===����ʱ��Ҫ�޸�===
        #endregion
        #region 1.ҳ����
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public DataTable GetPlanList()
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt;
            strSql.Append(@"SELECT a.*,b.DvcNm as DvcNm FROM  " + tableName + " a join BBdbR_DvcBase b on a.DvcId=b.DvcId where a.Enabled=1 ");
            dt = Repository().FindTableBySql(strSql.ToString(),false);
            return dt;
        }
        #endregion
        #region 2.չʾ���
        /// <summary>
        /// �������������Enabled = true������, ��Ϊ��Ч�Ĺ�����Ϣ
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>����������������</returns>
        public List<BBdbR_AlarmAddressBase> GetPageList(JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            return Repository().FindList("Enabled", "1");
        }
        #endregion
        #region 3.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_AlarmAddressBase entity) //===����ʱ��Ҫ�޸�===
        {
            //entity .AndonRuleTm= ConvertHelper . entity.AndonRuleTm
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
        }
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion
        #region 4.���ĳ���ֶε��ֶ�ֵ�Ƿ����
        /// <summary>
        ///   Enabled = 1��������ĳ���ֶΣ�KeyName�����ֶ�ֵ��KeyValue���Ƿ����
        /// </summary>
        /// <param name="KeyName">�ֶ���</param>
        /// <param name="KeyValue">�ֶ�ֵ</param>
        /// <returns>���ص��������ı��а������ֶ�ֵ�ļ�¼����</returns>
        public int CheckCount(string KeyName1, string KeyValue1, string KeyName2, string KeyValue2)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and'" + KeyName1 + "' = '" + KeyValue1 + "'and'" + KeyName2 + "' = '" + KeyValue2 + "'";
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
        public int Insert(BBdbR_AlarmAddressBase entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Insert(entity);
        }
        #endregion
        #region 7.ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Delete(string[] array)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<BBdbR_AlarmAddressBase> listEntity = new List<BBdbR_AlarmAddressBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_AlarmAddressBase entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//�޸����ݿ�
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
        public DataTable GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            DataTable dt;
            if (Condition == "all")
            { 
                sql = @"select a.*,b.DvcNm as DvcNm from " + tableName + " a join BBdbR_DvcBase b on a.DvcId = b.DvcId where a.Enabled = '1'";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            else if (Condition == "Class")
            {
                sql = @"select a.*,b.DvcNm as DvcNm from " + tableName + " a join BBdbR_DvcBase b on a.DvcId = b.DvcId where a.Enabled = '1' and a." + Condition + " like  '%" + keywords + "%'";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            else
            {
                //����������ѯ
                //string sql1 = @"select ";
                sql = @"select a.*,b.DvcNm as DvcNm from " + tableName + " a join BBdbR_DvcBase b on a.DvcId = b.DvcId where a.Enabled = '1' and b." + Condition + " like  '%" + keywords + "%'";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            return dt;
        }
        #endregion
        #region 10.�������豸
        public DataTable GetDvcNm()
        {
            string sql = @"select DvcId as id, DvcNm as dvcnm from BBdbR_DvcBase where Enabled='1'";
            return Repository().FindTableBySql(sql);
        }
        
        #endregion
        #region 10.�༭�������
        //�༭�������
        public DataTable SetAlarmInfor(string keywords) //===����ʱ��Ҫ�޸�===
        {
            string strSql = "";
            DataTable dt;

            strSql = @"select * from " + tableName + " where Enabled = '1'";
            dt = Repository().FindTableBySql(strSql.ToString());
            dt.Columns.Add(new DataColumn("PlineNm", typeof(string)));
            dt.Columns.Add(new DataColumn("WcNm", typeof(string)));
            dt.Columns.Add(new DataColumn("AviNm", typeof(string)));
            dt.Columns.Add(new DataColumn("DvcNm", typeof(string)));

            string PlineId = dt.Rows[0]["PlineId"].ToString();
            string WcId = dt.Rows[0]["PlineId"].ToString();
            string AviId = dt.Rows[0]["PlineId"].ToString();
            string DvcId = dt.Rows[0]["PlineId"].ToString();

           string  sql1= @"select * from BBdbR_PlineBase where Enabled = '1' and PlineId='" + PlineId + "'";
            DataTable dt1 = Repository().FindTableBySql(strSql.ToString());
            dt.Rows [0]["PlineNm"]  = dt1.Rows[0]["PlineNm"];

            string sql2 = @"select * from BBdbR_WcBase where Enabled = '1' and PlineId='" + PlineId + "'";
            DataTable dt2 = Repository().FindTableBySql(strSql.ToString());
            dt.Rows[0]["WcNm"] = dt1.Rows[0]["WcNm"];

            string sql3 = @"select * from BBdbR_AVIBase where Enabled = '1' and PlineId='" + PlineId + "'";
            DataTable dt3 = Repository().FindTableBySql(strSql.ToString());
            dt.Rows[0]["AviNm"] = dt1.Rows[0]["AviNm"];

            string sql4 = @"select * from BBdbR_DvcBase where Enabled = '1' and PlineId='" + PlineId + "'";
            DataTable dt4 = Repository().FindTableBySql(strSql.ToString());
            dt.Rows[0]["DvcNm"] = dt1.Rows[0]["DvcNm"];

            return dt;
        }
        #endregion
        
        
        #region 9.����ȫ�������б�
        /// <summary>
        /// ���ϲ�ѯ
        /// </summary>
        /// <returns></returns>
        public List<BBdbR_AlarmAddressBase> GetFaultList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_AlarmAddressBase where Enabled=1 ");
            List<BBdbR_AlarmAddressBase> dt = Repository().FindListBySql(strSql.ToString());
            return dt;
        }
        #endregion
    
        #region 10.����ģ��
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

       
        #region �����õ��豸ID
        public DataTable searchID(string DvcCd)
        {
            try
            {
                string sql = @"select DvcId as id from BBdbR_DvcBase where DvcCd = '" + DvcCd + "' and Enabled = 1";
                DataTable dtID = Repository().FindTableBySql(sql.ToString(), false);
                return dtID != null ? dtID : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}