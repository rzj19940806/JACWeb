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
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// ���ϻ�����Ϣ
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.04.02 16:27</date>
    /// </author>
    /// </summary>
    public class BBdbR_MatBaseBll : RepositoryFactory<BBdbR_MatBase>
    {
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_MatBase";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 1.ҳ����
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public List<BBdbR_MatBase> GetPlanList(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            List<BBdbR_MatBase> dt;
            if (KeyValue == "")
            {
                strSql.Append(@"SELECT  * FROM  BBdbR_MatBase where Enabled = 1 order by IsScan,WcCd");
                dt = Repository().FindListBySql(strSql.ToString());
            }
            else
            {
                strSql.Append(@"SELECT  * FROM  BBdbR_MatBase where  MatId='" + KeyValue + "'");
                dt = Repository().FindListBySql(strSql.ToString());
            }

            return dt;
        }
        #endregion

        #region 2.չʾ���
        /// <summary>
        /// �������������IsAvailable = true������
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>����������������</returns>
        public List<BBdbR_MatBase> GetPageList(JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            return Repository().FindList("Enabled", "1");
        }
        #endregion

        #region 3.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_MatBase entity) //===����ʱ��Ҫ�޸�===
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
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + "  = '" + KeyValue + "'";
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
        public int Insert(BBdbR_MatBase entity) //===����ʱ��Ҫ�޸�===
        {
            //entity.Enabled = "1";

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
            StringBuilder sql = new StringBuilder();
            int isok = 0;
            List<BBdbR_MatBase> listEntity = new List<BBdbR_MatBase>();
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            //List<BBdbR_CarPartBase> listEntity = new List<BBdbR_CarPartBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {

                //===����ʱ��Ҫ�޸�===
                BBdbR_MatBase entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
                                                                         //entity.Enabled = "0";//����ʵ���IsAvailable���Ը�Ϊfalse
                                                                         //entity.Modify(keyValue);
                if (entity.MatImg == null)
                {
                    entity.Enabled = "0";
                    listEntity.Add(entity);

                }
                else
                {
                    sql.Append(@"update " + tableName + " set MatImg = @content,Enabled = '0' where MatId = '" + keyValue + "'");
                    var dbparameter = new SqlParameter("@content", entity.MatImg);
                    isok += Repository().ExecuteBySql(sql, new[] { dbparameter });
                    return isok;//��ɾ���ݿ�
                }

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
        public List<BBdbR_MatBase> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            List<BBdbR_MatBase> dt;
            if (Condition == "all")
            {
                sql = @"select * from " + tableName + " where Enabled = '1'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            else
            {
                //����������ѯ
                sql = @"select * from " + tableName + " where Enabled = '1' and " + Condition + " like  '%" + keywords + "%'";
                dt = Repository().FindListBySql(sql.ToString());
            }
            return dt;
        }
        #endregion

        #region 8.����ͼƬ
        public int InsertPicture(string MatCd, byte[] image, string name) //===����ʱ��Ҫ�޸�===
        {

            //long longKeyValue = Convert.ToInt64(KeyValue);
            StringBuilder sql = new StringBuilder();
            byte[] img = image;
            Image images = Image.FromStream(new MemoryStream(img));
            int width = images.Width;
            int height = images.Height;
            string Name = "";
            if (name != null && !String.IsNullOrEmpty(name))
            {
                Name = name.Substring(1, name.Length - 1);
            }

            sql.Append(@"update " + tableName + " set MatImg = @content where MatCd = '" + MatCd + "'");
            var dbparameter = new SqlParameter("@content", image);
            return Repository().ExecuteBySql(sql, new[] { dbparameter });
        }
        #endregion

        #region 9.��ȡͼƬ
        public DataTable getPicture(string strID)
        {
            DataTable dt = new DataTable();
            if (strID != null && !String.IsNullOrEmpty(strID))
            {
                Stopwatch watch = CommonHelper.TimerStart();
                string sql = "select MatImg,MdfCd,MdfNm,Rem from A_ProductBase where MatId = " + strID + "";
                //string sql = "select Image,Reserve1 from A_ProductBase where ID = " + ID + "";
                dt = Repository().FindTableBySql(sql);
                string costtime = CommonHelper.TimerEnd(watch);
            }
            return dt;
        }
        #endregion

        #region 10.�༭
        public DataTable GetMatInfor(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from BBdbR_MatBase where MatId='" + KeyValue + "'");
            return Repository().FindTableBySql(strSql.ToString(),false);

        }
        public DataTable GetPartMatInfor(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append($@"select * from BBdbR_PartMatConfig where Id='{KeyValue}'");
            return Repository().FindTableBySql(strSql.ToString(), false);

        }
        #endregion

        #region 11.ҳ����
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        //public List<BBdbR_MatBase> GetPlanList()
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append(@"SELECT  * FROM  BBdbR_MatBase where Enabled = '1' and MatCatg = '��Ʒ'");
        //    List<BBdbR_MatBase> dt = Repository().FindListBySql(strSql.ToString());
        //    return dt;
        //}
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public BBdbR_MatBase GetPlanList1(string MatId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_MatBase where Enabled = '1' and MatCatg = '��Ʒ' and MatId='" + MatId + "'");
            List<BBdbR_MatBase> dt = Repository().FindListBySql(strSql.ToString());
            if(dt.Count > 0)
            {
                return dt[0];
            }
            else
            {
                return null;
            }
            
        }
        #endregion

        #region 12.��ѯ���ñ��в����ڵ�������Ϣ
        public List<BBdbR_MatBase> GetMatList(string QualityCheckModelId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_MatBase where Enabled = '1' and MatCatg = '��Ʒ' and MatId not in (SELECT MatId  FROM  BBdbR_QuaCheckModelCarConfig where Enabled = '1' and QualityCheckModelId = '"+ QualityCheckModelId + "' )");
            List<BBdbR_MatBase> dt = Repository().FindListBySql(strSql.ToString());
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
    }
}