//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2017
// Software Developers @ HfutIE 2017
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// AD_Document
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.09.01 22:24</date>
    /// </author>
    /// </summary>
    public class AD_DocumentBll : RepositoryFactory<AD_Document>
    {
        public object GetDocumentEdition(string document_name)
        {
            StringBuilder str = new StringBuilder();
            str.Append(@"select max(document_edition) from  AD_Document  where document_name='" + document_name + "'");
            return Repository().FindMaxBySql(str.ToString());//这里用FindMaxBySql的目的是获取最大版本号
        }
        public object GetDocumentCode(string data)
        {
            string str = "select max(SUBSTRING(document_code,2,len(document_code))) from AD_Document where document_code like '%" + data + "%'";//获取从第二位起当天上传的最大文件编号
            return Repository().FindMaxBySql(str.ToString());
        }

        public DataTable GetBytes(string document_key)
        {
            string str = "select   document_file,document_format,document_name,document_type from AD_Document where document_key='" + document_key + "'";
            return Repository().FindTableBySql(str);
        }
        public List<AD_Document> GetForm(string KeyValue)
        {
            StringBuilder str = new StringBuilder();
            str.Append(@"SELECT  [document_key]
                                ,[document_code]
                                ,[document_name]
                                ,document_format
                                ,[document_type]
                                ,[document_edition]
                                ,[document_size]
                                ,[upload_PC_Mac]
                                ,[document_storage_path]
                                ,[upload_PC_IP]
                                ,[upload_PC_name]
                                ,[remarks]
                                FROM AD_Document] where 1=1 "); ;
            str.Append(" and document_key='" + KeyValue + "'");
            return Repository().FindListBySql(str.ToString());
        }

        public List<AD_Document> GetFileListBySearch(string selectName, string keywords, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  [document_key]
                                   ,[document_code]
                                   ,[document_name]
                                   ,document_format
                                   ,[document_type]
                                   ,[document_edition]
                                   ,[document_size]
                                   ,[upload_PC_Mac]
                                   ,[document_storage_path]
                                   ,[upload_PC_IP]
                                   ,[upload_PC_name]
                                   ,[remarks]
                               FROM AD_Document where 1=1 ");
            List<DbParameter> parameter = new List<DbParameter>();
            if (selectName != null && selectName != "")
            {
                strSql.Append(" and " + selectName + " like @keywords ");
                parameter.Add(DbFactory.CreateDbParameter("@keywords", '%' + keywords + '%'));
            }
                    
            if (parameter.Count == 0)//如果参数为0，即初始化情况下，无参数，用此签名的方法
            {
                return Repository().FindListPageBySql(strSql.ToString(), ref jqgridparam);
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        //搜索文档
        public DataTable GetList(string document_type_key, string ParameterJson, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  [document_key]
                                   ,[document_code]
                                   ,[document_name]
                                   ,document_format
                                   ,[document_type]
                                   ,[document_edition]
                                   ,[document_size]
                                   ,[upload_PC_Mac]
                                   ,[document_storage_path]
                                   ,[upload_PC_IP]
                                   ,[upload_PC_name]
                                   ,[remarks]
                               FROM AD_Document where 1=1 ");
            if (document_type_key != ""&& document_type_key!=null)
            {
                strSql.Append(@"and document_type ='"+ document_type_key + "'");
            }
            return Repository().FindTableBySql(strSql.ToString());
        }





        public DataTable GetPageList(string ID, string ParameterJson, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();

            strSql.Append(@"SELECT  
                                    [document_key]
                                   ,[document_code]
                                   ,[document_name]
                                   ,document_format
                                   ,[document_type]
                                   ,[document_edition]
                                   ,[document_size]
                                   ,[upload_PC_Mac]
                                   ,[document_storage_path]
                                   ,[upload_PC_IP]
                                   ,[upload_PC_name]
                                   ,[remarks]                                   
                            FROM   AD_Document   where document_type  ='" + ID + "'");
            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}