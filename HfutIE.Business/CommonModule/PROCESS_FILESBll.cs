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
    /// PROCESS_FILES
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.08.04 19:57</date>
    /// </author>
    /// </summary>
    public class PROCESS_FILESBll : RepositoryFactory<PROCESS_FILES>
    {
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select '0' as parent_key,
                                   SITE.site_key as primery_key ,
                                   SITE.site_code as code,
                                   SITE.site_name as name,
                                   'site' as type
                                   from site
                            union all 
                            select SITE_AREA_LIST.site_key as parent_key,
                                   AREA.area_key as primery_key,
                                   AREA.area_code as code,
                                   AREA.area_name as name,
                                   'area' as type
                                   from AREA join SITE_AREA_LIST on  AREA.area_key=SITE_AREA_LIST.area_key 
                            union all 
                            select AREA_WS_LIST.area_key as parent_key,
                                   WORKSHOP.ws_key as primery_key,
                                   WORKSHOP.ws_code as code,
                                   WORKSHOP.ws_name as name,
                                   'workshop' as type
                                   from WORKSHOP join AREA_WS_LIST on  WORKSHOP.ws_key=AREA_WS_LIST.ws_key 
                            union all 
                            select WS_P_LINE_LIST.ws_key as parent_key,
                                   Productionline.p_line_key as primery_key,
                                   Productionline.p_line_code as code,
                                   Productionline.p_line_name as name,
                                   'productionline' as type
                                   from Productionline join WS_P_LINE_LIST on  Productionline.p_line_key=WS_P_LINE_LIST.p_line_key 
                            union all 
                            select P_LINE_WC_LIST.p_line_key as parent_key,
                                   WORK_CENTER.wc_key as primery_key,
                                   WORK_CENTER.wc_code as code,
                                   WORK_CENTER.wc_name as name,
                                   'workcenter' as type
                                   from WORK_CENTER join P_LINE_WC_LIST on  WORK_CENTER.wc_key=P_LINE_WC_LIST.wc_key  order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        public DataTable GetProductTable(string keyword, string selectName, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();

            strSql.Append(@"select      part_key,
                                        part_code,
                                        part_name,
                                        part_type,
                                        part_unit,
                                        remarks  from  PART where part_category='105' ");
            if (!string.IsNullOrEmpty(selectName))
            {
                strSql.Append(@" and part_key in ( select product_key from process_files where  " + selectName + " LIKE @keyword  ) ");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keyword + '%'));
            }
            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        public List<PROCESS_FILES> GetFileList(string wc_key, string product_key, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  [document_key]
                                   ,[site_key]
                                   ,[site_code]
                                   ,[site_name]
                                   ,[area_key]
                                   ,[area_code]
                                   ,[area_name]
                                   ,[ws_key]
                                   ,[ws_code]
                                   ,[ws_name]
                                   ,[p_line_key]
                                   ,[p_line_code]
                                   ,[p_line_name]
                                   ,[wc_key]
                                   ,[wc_code]
                                   ,[wc_name]
                                   ,[product_key]
                                   ,[product_code]
                                   ,[product_name]
                                   ,[document_code]
                                   ,[document_name]
                                   ,[document_type]
                                   ,[document_edition]
                                   ,[document_size]
                                   ,[upload_PC_Mac]
                                   ,[document_storage_path]
                                   ,[upload_PC_IP]
                                   ,[upload_PC_name]
                                   ,[remarks]
                                   ,[reserve01]
                                   ,[reserve02]
                                   ,[reserve03]
                                   ,[reserve04]
                                   ,[reserve05]
                                   ,[reserve06]
                                   ,[reserve07]
                                   ,[reserve08]
                                   ,[reserve09]
                                   ,[reserve10]
                                   ,[CreateDate]
                                   ,[CreateUserId]
                                   ,[CreateUserName]
                                   ,[ModifyDate]
                                   ,[ModifyUserId]
                                   ,[ModifyUserName]
                               FROM [JAC_FrontAxle].[dbo].[PROCESS_FILES] where 1=1 ");
            List<DbParameter> parameter = new List<DbParameter>();
            if (wc_key != null && wc_key != "")
            {
                strSql.Append(" and wc_key = @wc_key ");
                parameter.Add(DbFactory.CreateDbParameter("@wc_key", wc_key));
            }
            if (product_key != null && product_key != "")
            {
                strSql.Append(" and  product_key = @product_key ");
                parameter.Add(DbFactory.CreateDbParameter("@product_key", product_key));
            }
            if (parameter.Count == 0)//如果参数为0，即初始化情况下，无参数，用此签名的方法
            {
                return Repository().FindListPageBySql(strSql.ToString(), ref jqgridparam);
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        public List<PROCESS_FILES> GetFileListBySearch(string selectName, string keywords, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  [document_key]
                                   ,[site_key]
                                   ,[site_code]
                                   ,[site_name]
                                   ,[area_key]
                                   ,[area_code]
                                   ,[area_name]
                                   ,[ws_key]
                                   ,[ws_code]
                                   ,[ws_name]
                                   ,[p_line_key]
                                   ,[p_line_code]
                                   ,[p_line_name]
                                   ,[wc_key]
                                   ,[wc_code]
                                   ,[wc_name]
                                   ,[product_key]
                                   ,[product_code]
                                   ,[product_name]
                                   ,[document_code]
                                   ,[document_name]
                                   ,[document_type]
                                   ,[document_edition]
                                   ,[document_size]
                                   ,[upload_PC_Mac]
                                   ,[document_storage_path]
                                   ,[upload_PC_IP]
                                   ,[upload_PC_name]
                                   ,[remarks]
                                   ,[reserve01]
                                   ,[reserve02]
                                   ,[reserve03]
                                   ,[reserve04]
                                   ,[reserve05]
                                   ,[reserve06]
                                   ,[reserve07]
                                   ,[reserve08]
                                   ,[reserve09]
                                   ,[reserve10]
                                   ,[CreateDate]
                                   ,[CreateUserId]
                                   ,[CreateUserName]
                                   ,[ModifyDate]
                                   ,[ModifyUserId]
                                   ,[ModifyUserName]
                               FROM [JAC_FrontAxle].[dbo].[PROCESS_FILES] where 1=1 ");
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
        public object GetDocumentCode(string data)
        {
            string str = "select max(SUBSTRING(document_code,2,len(document_code))) from process_files where document_code like '%"+ data + "%'";//获取从第二位起当天上传的最大文件编号
            return Repository().FindMaxBySql(str.ToString());
        }

        public DataTable GetBasicInfor(string wc_key)
        {
            StringBuilder str = new StringBuilder();
            str.Append(@"select S.site_key, 
                                S.site_code, 
                                S.site_name, 
                                A.area_key, 
                                A.area_code, 
                                A.area_name, 
                                WS.ws_key, 
                                WS.ws_code, 
                                WS.ws_name, 
                                P.p_line_key, 
                                P.p_line_code, 
                                P.p_line_name, 
                                WC.wc_key, 
                                WC.wc_code, 
                                WC.wc_name
                                from SITE S right join SITE_AREA_LIST ST on S.site_key = ST.site_key RIGHT JOIN AREA A ON A.area_key = ST.area_key 
                                RIGHT JOIN   AREA_WS_LIST AL ON A.area_key = AL.area_key RIGHT JOIN WORKSHOP WS ON WS.ws_key = AL.ws_key 
                                RIGHT JOIN WS_P_LINE_LIST WSL ON WSL.ws_key = WS.ws_key RIGHT JOIN Productionline P ON P.p_line_key = WSL.p_line_key 
                                RIGHT JOIN P_LINE_WC_LIST PL ON PL.p_line_key = P.p_line_key RIGHT JOIN WORK_CENTER WC ON WC.wc_key = PL.wc_key 
                                where  WC.wc_key='" + wc_key + "'");
            return Repository().FindTableBySql(str.ToString());
        }
        public DataTable GetProductInfor(string product_key)
        {
            StringBuilder str = new StringBuilder();
            str.Append(@"select part_key as product_key,
                                part_code as product_code,
                                part_name as product_name
                                from part where part_key='" + product_key + "'");
            return Repository().FindTableBySql(str.ToString());
        }
        
        public object GetDocumentEdition(string document_name)
        {
            StringBuilder str = new StringBuilder();
            str.Append(@"select max(document_edition) from process_files where document_name='" + document_name + "'");
            return Repository().FindMaxBySql(str.ToString());//这里用FindMaxBySql的目的是获取最大版本号
        }
        public DataTable GetBytes(string document_key)
        {
            string str = "select document_file,document_name,document_type from process_files where document_key='" + document_key + "'";
            return Repository().FindTableBySql(str);
        }
        public List<PROCESS_FILES> GetForm(string KeyValue)
        {
            StringBuilder str = new StringBuilder();
            str.Append(@"SELECT  [document_key]
                                ,[site_key]
                                ,[site_code]
                                ,[site_name]
                                ,[area_key]
                                ,[area_code]
                                ,[area_name]
                                ,[ws_key]
                                ,[ws_code]
                                ,[ws_name]
                                ,[p_line_key]
                                ,[p_line_code]
                                ,[p_line_name]
                                ,[wc_key]
                                ,[wc_code]
                                ,[wc_name]
                                ,[product_key]
                                ,[product_code]
                                ,[product_name]
                                ,[document_code]
                                ,[document_name]
                                ,[document_type]
                                ,[document_edition]
                                ,[document_size]
                                ,[upload_PC_Mac]
                                ,[document_storage_path]
                                ,[upload_PC_IP]
                                ,[upload_PC_name]
                                ,[remarks]
                                FROM[JAC_FrontAxle].[dbo].[PROCESS_FILES] where 1=1 "); ;
            str.Append(" and document_key='"+ KeyValue+"'");
            return Repository().FindListBySql(str.ToString());
        }
    }
}