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
    /// �豸������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.25 21:56</date>
    /// </author>
    /// </summary>
    public class BBdbR_DvcBaseBll : RepositoryFactory<BBdbR_DvcBase>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "BBdbR_DvcBase";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 1.��ȡ������Ҫ�޸�sql���
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();    
            //===����ʱ��Ҫ�޸�===
       
            strSql.Append(@"select    
                             '10' AS keys,
                             '10'AS code,
                             '����ε�����ó����칫˾' AS name,
                             '1' As IsAvailable,
                             '0' AS parentId,
                             '1' as sort 
                          from BBdbR_FacBase where 1=1");
            strSql.Append(@"union select  FacId AS keys,     
                             FacCd AS code,
                             FacNm AS name,
                             Enabled As IsAvailable,
                             '10' as parentId,  
                             '1' as sort    
                         from BBdbR_FacBase where Enabled = '1' ");
            strSql.Append(@" union select    
                             a.WorkshopId AS keys,
                             a.WorkshopCd AS code,
                             a.WorkshopNm AS name,
                             a.Enabled As IsAvailable,
                             a.FacId AS parentId,
                              '1' as sort 
                          from  BBdbR_WorkshopBase a,BBdbR_FacBase b 
                          where a.FacId=b.FacId and a.Enabled = '1' order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 3.չʾ���
        /// <summary>
        /// �������������IsAvailable = true������
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>����������������</returns>
        public List<BBdbR_DvcBase> GetPageList(JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            return Repository().FindList("Enabled", "1");
        }
        #endregion

        #region 4.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_DvcBase entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
        }
        #endregion

        #region 5.���ĳ���ֶε��ֶ�ֵ�Ƿ����
        /// <summary>
        ///   Enabled = 1��������ĳ���ֶΣ�KeyName�����ֶ�ֵ��KeyValue���Ƿ����
        /// </summary>
        /// <param name="KeyName">�ֶ���</param>
        /// <param name="KeyValue">�ֶ�ֵ</param>
        /// <returns>���ص��������ı��а������ֶ�ֵ�ļ�¼����</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enabled = 1 and " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion

        #region 6.��������
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
        public int Insert(BBdbR_DvcBase entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Insert(entity);
        }
        #endregion

        #region 6.������

        //��ȡ���г���
        public DataTable GetWorkshopNm(string classv)
        {
            string sql = "";
            if (classv == "����")
            {
                 sql = @"select WorkshopId as id, WorkshopNm as name from BBdbR_WorkshopBase where Enabled=1 order by sort asc";
                
            } else if(classv == "��λ")
            {
                sql = @"select WcId as id, WcNm as name from BBdbR_WcBase where Enabled=1 order by WcCd asc";
            } else if(classv == "����")
            {
                sql = @"select PlineId as id, PlineNm as name from BBdbR_PlineBase where Enabled=1 order by PlineCd asc";
            } 
            else if (classv == "AVI�豸")
            {
                sql = @"select AviId as id, AviNm as name from BBdbR_AVIBase where Enabled=1 and OP_CODE is not null  order by OP_CODE,AviCd asc";
            }
            return Repository().FindTableBySql(sql);
        }
        //��ȡ���в���
        
        //��ȡ������Ϣ
        public DataTable GetWorkshopNm2(string ClassId)
        {
            string sql = @"select WorkshopNm as classnm from BBdbR_WorkshopBase where Enabled='1' and WorkshopId=" + "'" + ClassId + "'";
            return Repository().FindTableBySql(sql);
        }
        public DataTable GetPlineNm2(string ClassId)
        {
            string sql = @"select PlineNm as classnm from BBdbR_PlineBase where Enabled='1' and PlineId=" + "'" + ClassId + "'";
            return Repository().FindTableBySql(sql);
        }
        public DataTable GetWcNm2(string ClassId)
        {
            string sql = @"select WcNm as classnm from BBdbR_WcBase where Enabled='1' and WcId=" + "'" + ClassId + "'";
            return Repository().FindTableBySql(sql);
        }
        public DataTable GetPostNm2(string ClassId)
        {
            string sql = @"select PostNm as classnm from BBdbR_PostBase where Enabled='1' and PostId=" + "'" + ClassId + "'";
            return Repository().FindTableBySql(sql);
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
            List<BBdbR_DvcBase> listEntity = new List<BBdbR_DvcBase>(); //===����ʱ��Ҫ�޸�===
            foreach (string keyValue in array)
            {
                //===����ʱ��Ҫ�޸�===
                BBdbR_DvcBase entity = Repository().FindEntity(keyValue);//����������keyValue�������ݿ��в���ʵ�� //===����ʱ��Ҫ�޸�===
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
            DataTable dt = new DataTable();
            if (Condition == "all" || Condition == null)
            {
                sql = @"select * from " + tableName +" where Enabled=1 order by "+ jqgridparam.sidx+" "+ jqgridparam.sord;
                dt = Repository().FindTableBySql(sql.ToString(), false);
                dt.Columns.Add("ClassNm");
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    if(dt.Rows[i]["Class"].ToString() == "����")
                    {
                        sql = @"select WorkshopNm from BBdbR_WorkshopBase where WorkshopId='" + dt.Rows[i]["ClassId"].ToString() + "'and Enabled=1 ";
                        DataTable dt1 = Repository().FindTableBySql(sql.ToString(), false);
                        if (dt1.Rows.Count>0)
                        {
                            dt.Rows[i]["ClassNm"] = dt1.Rows[0][0];
                        }
                    }
                    else if(dt.Rows[i]["Class"].ToString() == "��λ")
                    {
                        sql = @"select WcNm from BBdbR_WcBase where WcId='" + dt.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ";
                        DataTable dt2 = Repository().FindTableBySql(sql.ToString(), false);
                        if (dt2.Rows.Count > 0)
                        {
                            dt.Rows[i]["ClassNm"] = dt2.Rows[0][0];
                        }
                    }
                    else if(dt.Rows[i]["Class"].ToString() == "����")
                    {
                        sql = @"select PlineNm from BBdbR_PlineBase where PlineId='" + dt.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ";
                        DataTable dt3 = Repository().FindTableBySql(sql.ToString(), false);
                        if (dt3.Rows.Count > 0)
                        {
                            dt.Rows[i]["ClassNm"] = dt3.Rows[0][0];
                        }
                    }
                    else if(dt.Rows[i]["Class"].ToString() == "��λ")
                    {
                        sql = @"select PostNm from BBdbR_PostBase where PostId='" + dt.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ";
                        DataTable dt4 = Repository().FindTableBySql(sql.ToString(), false);
                        if (dt4.Rows.Count > 0)
                        {
                            dt.Rows[i]["ClassNm"] = dt4.Rows[0][0];
                        }
                    }
                    else if (dt.Rows[i]["Class"].ToString() == "AVI�豸")
                    {
                        sql = @"select AviNm from BBdbR_AVIBase where AviId ='" + dt.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ";
                        DataTable dt5 = Repository().FindTableBySql(sql.ToString(), false);
                        if (dt5.Rows.Count > 0)
                        {
                            dt.Rows[i]["ClassNm"] = dt5.Rows[0][0];
                        }
                    }
                }
            }
            else
            {
                sql = @"select * from " + tableName + " where Enabled = 1 and " + Condition + " like  '%" + keywords + "%'" + jqgridparam.sidx + " " + jqgridparam.sord;
                dt = Repository().FindTableBySql(sql.ToString(), false);
                dt.Columns.Add("ClassNm");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Class"].ToString() == "����")
                    {
                        sql = @"select WorkshopNm from BBdbR_WorkshopBase where WorkshopId='" + dt.Rows[i]["ClassId"].ToString() + "'and Enabled=1 ";
                        DataTable dt1 = Repository().FindTableBySql(sql.ToString(), false);
                        dt.Rows[i]["ClassNm"] = dt1.Rows[0][0];
                    }
                    else if (dt.Rows[i]["Class"].ToString() == "��λ")
                    {
                        sql = @"select WcNm from BBdbR_WcBase where WcId='" + dt.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ";
                        DataTable dt2 = Repository().FindTableBySql(sql.ToString(), false);
                        dt.Rows[i]["ClassNm"] = dt2.Rows[0][0];
                    }
                    else if (dt.Rows[i]["Class"].ToString() == "����")
                    {
                        sql = @"select PlineNm from BBdbR_PlineBase where PlineId='" + dt.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ";
                        DataTable dt3 = Repository().FindTableBySql(sql.ToString(), false);
                        dt.Rows[i]["ClassNm"] = dt3.Rows[0][0];
                    }
                    else if (dt.Rows[i]["Class"].ToString() == "��λ")
                    {
                        sql = @"select PostNm from BBdbR_PostBase where PostId='" + dt.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ";
                        DataTable dt4 = Repository().FindTableBySql(sql.ToString(), false);
                        dt.Rows[i]["ClassNm"] = dt4.Rows[0][0];
                    }
                    else if (dt.Rows[i]["Class"].ToString() == "AVI�豸")
                    {
                        sql = @"select AviNm from BBdbR_AVIBase where AviId ='" + dt.Rows[i]["ClassId"].ToString() + "' and Enabled=1 ";
                        DataTable dt5 = Repository().FindTableBySql(sql.ToString(), false);
                        if (dt5.Rows.Count > 0)
                        {
                            dt.Rows[i]["ClassNm"] = dt5.Rows[0][0];
                        }
                    }
                }
            }
            return dt;
        }
        #endregion

        #region 9.ҳ����
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <returns></returns>
        public List<BBdbR_DvcBase> GetPlanList1()
        {
            return Repository().FindList("Enabled", "1");
        }
      
        /// <summary>
        /// �༭����������Դ
        /// </summary>
        /// <returns></returns>
        public BBdbR_DvcBase GetPlanList1(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_DvcBase where  DvcId='"+ KeyValue + "'");
            List<BBdbR_DvcBase> dt = Repository().FindListBySql(strSql.ToString());
            BBdbR_DvcBase Dvcentity = new BBdbR_DvcBase();
            Dvcentity = dt[0];
            return Dvcentity;
        }
        #endregion

        #region 10.��õ���ģ��
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

        #region 11.��������
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

        #region ����ʱ�Ļ�����Ϣ
        public DataTable searchClass(string dvcClass, string devClassICd)
        {
            try
            {
                string sql = "";
                DataTable dt = new DataTable();
                if (dvcClass == "����")
                {
                    sql = @"select WorkshopId as id from BBdbR_WorkshopBase where WorkshopCd = '" + devClassICd + "'Enabled=1";
                    dt = Repository().FindTableBySql(sql.ToString(), false);
                }
                else if (dvcClass == "��λ")
                {
                    sql = @"select WcId  as id from BBdbR_WcBase where WcCd = '" + devClassICd + "'Enabled=1";
                    dt = Repository().FindTableBySql(sql.ToString(), false);
                }
                else if (dvcClass == "����")
                {
                    sql = @"select PlineId as id from BBdbR_PlineBase wherre PlineCd = '" + devClassICd + "'Enabled=1";
                    dt = Repository().FindTableBySql(sql.ToString(), false);
                }
                else if (dvcClass == "��λ")
                {
                    sql = @"select PostId as id from BBdbR_PostBase where PostCd = '" + devClassICd + "'Enabled=1";
                    dt = Repository().FindTableBySql(sql.ToString(), false);
                }
                else if (dvcClass == "AVI�豸")
                {
                    sql = @"select AviId as id from BBdbR_AVIBase where AviCd = '" + devClassICd + "'Enabled=1";
                    dt = Repository().FindTableBySql(sql.ToString(), false);
                }
                return dt != null?dt:null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

    }
}