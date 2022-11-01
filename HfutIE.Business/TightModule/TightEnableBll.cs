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
    public class TightEnableBll : RepositoryFactory<Tg_JobEnableConfig>
    {
        #region ȫ�ֱ���������

        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "Tg_JobEnableConfig";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 3.�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(Tg_JobEnableConfig entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
        }
        #endregion

        #region 4.����Ƿ��Ѵ���JOBʹ�����ù�ϵ
        /// <summary>
        ///   Enabled = 1��������ĳ���ֶΣ�KeyName�����ֶ�ֵ��KeyValue���Ƿ����
        /// </summary>
        /// <param name="KeyName">�ֶ���</param>
        /// <param name="KeyValue">�ֶ�ֵ</param>
        /// <returns>���ص��������ı��а������ֶ�ֵ�ļ�¼����</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from Tg_JobEnableConfig where code='" + KeyName + "' and WJCId='" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        public int CheckCount2(string KeyName, string KeyValue)
        {
            string sql = "";
            if(KeyName== "tg_wcjobconfig")
            {
                sql = @"select * from tg_wcjobconfig where wcjobcd='" + KeyValue + "'";

            }
            else if (KeyName == "����")
            {
                sql = @"select * from BBdbR_MatBase where MatCd='" + KeyValue + "'";

            }
            else 
            {
                sql = @"select * from BBdbR_ProductBase where MatCd='" + KeyValue + "'";

            }
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


        #region 6.ɾ������
        //array ��Ҫɾ������Ϣ������������
        //ɾ������ĳһ�����ݱ�ʾ�����и������ݵ�Enabled����Ϊ0�����������ɾ��������
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int DeleteUseEnabled(string id)
        {
            //����һ������list�����ڴ洢ͨ��������ѯ������Ϣ
            List<Tg_JobEnableConfig> listEntity = new List<Tg_JobEnableConfig>(); //===����ʱ��Ҫ�޸�===            
                                                                    //===����ʱ��Ҫ�޸�===

            return Repository().Delete(listEntity);//�޸����ݿ�
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
        public DataTable GetPageListByCondition(string Condition, string keywords, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===��ѯJOB��λ��Ϣ
        {
            string sql = "";
            if (Condition == "All")
            {
                sql = @"select * from tg_wcjobconfig  order by wcJobcd asc";
                return Repository().FindTableBySql(sql,false);
            }
            else
            {
                if(Condition=="Code")//ͨ��JOBʹ�����ò�ѯJOB��Ϣ
                {
                    sql = @"select a.* from tg_wcjobconfig a right join Tg_JobEnableConfig  b on  a.wcjobcd=b.WJCId where b.code like '%"+keywords+"%' order by a.wcJobcd asc";
                    return Repository().FindTableBySql(sql, false);

                }
                else
                {
                sql = @"select * from tg_wcjobconfig where "+ Condition + " like '%"+ keywords + "%' order by wcJobcd asc";
                return Repository().FindTableBySql(sql, false);
                }
            }
        }
        public DataTable GetPageListByConditionEnable(string Condition, string keywords, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===��ѯJOBʹ����Ϣ
        {
            string sql = "";
            if (Condition == "All")
            {
                sql = @"select * from Tg_JobEnableConfig  order by Code asc";
                return Repository().FindTableBySql(sql, false);
            }
            else
            {
                if (Condition == "Code")
                {
                    sql = @"select * from Tg_JobEnableConfig  where code like '%" + keywords + "%'order by Code asc";
                    return Repository().FindTableBySql(sql, false);

                }
                else//���JOB��ѯʹ����Ϣ
                {
                    sql = @"select * from Tg_JobEnableConfig where WJCId = '" + keywords + "' order by Code asc";
                    return Repository().FindTableBySql(sql, false);
                }
            }
        }
        #endregion

        #region 8.�༭����������Դ
        /// <summary>
        /// �༭����������Դ
        /// </summary>
        /// <returns></returns>
        public Tg_JobEnableConfig GetPlanList1(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_WcBase where  WcId='" + KeyValue + "'");
            List<Tg_JobEnableConfig> dt = Repository().FindListBySql(strSql.ToString());
            Tg_JobEnableConfig Dvcentity = new Tg_JobEnableConfig();         
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
        #region 4.�ڱ��м��ؼ�����û���ù���ʵ���嵥
        public DataTable GetNotConfig(string WcId, string Condition, string keywords, ref JqGridParam jqgridparam)//�ڱ��м���û���ù�������嵥
        {
            StringBuilder strSql = new StringBuilder();
            if (Condition == "" || Condition == null)//��������ͼ�ţ���BOM���л�ȡ�������ϣ�
            {
                strSql.Append(@"select MatId,a.Code ,a.EtName ,a.type as Type,'' as Rem from 
( select  distinct MatCd as Code,MatId,MatNm as EtName,'ͼ��' as type from produceBom )
as a where a.Code not in (select Code from  Tg_JobEnableConfig b  where b.WJCId='" + WcId + "')");
                if (keywords != "" && keywords != null)
                {
                    strSql.Append(" and  a.Code like '%" + keywords + "%'");
                }

            }
            else if (Condition == "Num")//��������ͼ��
            {
                strSql.Append(@"select MatId,a.Code ,a.EtName ,a.type as Type,'' as Rem from ( select MatId, MatCd as Code,MatNm as EtName,'ͼ��' as type,Rem as Rem  from BBdbR_MatBase where Enabled='1' )as a where a.Code not in (select Code from  Tg_JobEnableConfig b  where b.WJCId='" + WcId + "') ");
                if (keywords != "" && keywords != null)
                {
                    strSql.Append(" and  a.Code like '%" + keywords + "%'");
                }

            }
            else if (Condition == "Prod")
            {
                strSql.Append(@"select MatId,a.Code ,a.EtName ,a.type as Type,''as Rem from (  select MatId, MatCd as Code,MatNm as EtName,'��Ʒ' as type,Notification as Rem  from BBdbR_ProductBase where Enabled='1' )as a where a.Code not in (select Code from  Tg_JobEnableConfig b  where b.WJCId='" + WcId + "') ");
                if (keywords != "" && keywords != null)
                {
                    strSql.Append(" and  a.Code like '%" + keywords + "%'");
                }
            }
            else //if (Condition == "����"|| Condition == "����&ͼ��")
            {
                if (Condition == "���� ͼ��")
                {
                    Condition = "����+ͼ��";
                }
                strSql.Append(@"select ROW_NUMBER() over (order by code ) MatId,Code ,EtName ,type as Type,''as Rem from 
                                 (SELECT  DISTINCT CarType as Code,CarType as EtName, '"+ Condition + "' as Type,'' as Rem from dbo.P_ProducePlan_Pro   ) as a order by code ");
            }
            return Repository().FindTableBySql(strSql.ToString(), false);
        }


        #endregion

        #region 5.�ύ����
        public int JOBEnableInsert(Tg_JobEnableConfig entity)
        {
            return Repository().Insert(entity);
            //return 1;
        }
        #endregion

        #region 6.ɾ������
        public int ConfigDelete(string KeyValue)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"delete Tg_JobEnableConfig where ID ='" + KeyValue + "'");
            int isok = Repository().ExecuteBySql(sql);
            return isok;
        }

        #endregion


    }
}