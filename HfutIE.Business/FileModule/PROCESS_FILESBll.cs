//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2017
// Software Developers @ HfutIE 2017
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
        public DataTable GetTreeTPM()//加载工厂产线工位的办法TPM用到的
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select '0' as parent_key,
                                   SITE.site_key as primery_key ,
                                   SITE.site_name as name,
                                   'site' as type
                                   from site
                            union all 
                            select SITE_AREA.[parent_key] as parent_key,
                                   [UDA_Area].object_key as primery_key,
                                   [UDA_Area].area_mc_u_S as name,
                                   'area' as type
                                   from SITE_AREA join UDA_Area on  [UDA_Area].object_key=[SITE_AREA].child_key 
                            union all 
                            select [AREA_PRODUCTION_LINE].parent_key as parent_key,
                                   [UDA_ProductionLine].object_key as primery_key,
                                   [UDA_ProductionLine].p_line_mc_u_S as name,
                                   'ProductionLine' as type
                                   from [AREA_PRODUCTION_LINE] join [UDA_ProductionLine] on  [AREA_PRODUCTION_LINE].child_key=[UDA_ProductionLine].object_key
                            union all 
                            select   P_LINE_WORK_CENTER.parent_key as parent_key,
                          P_LINE_WORK_CENTER.child_key as primery_key,

              WORK_CENTER.[wc_name] + '('+UDA_WorkCenter.wc_mc_u_S +')'  as name, 
                                   'workcenter' as type
                                   from  P_LINE_WORK_CENTER ,[AREA_PRODUCTION_LINE],UDA_WorkCenter,WORK_CENTER where   [AREA_PRODUCTION_LINE].child_key=P_LINE_WORK_CENTER.parent_key  and P_LINE_WORK_CENTER.child_key  = UDA_WorkCenter.object_key  and WORK_CENTER.[wc_key] =[UDA_WorkCenter].object_key
                         ");
            return Repository().FindTableBySql(strSql.ToString());
        }
        public DataTable GetTreeNode2()//加载树的第三个节点
        {


            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT c.p_line_key,c.p_line_name,A.[parent_key] ,A.[child_key],b.wc_key,b.wc_name,b.wc_mc_u_S ,d.[parent_key] parent_key1, d.[child_key] child_key1 ,e.equip_mc_u_S equip_mc_u_S,e.equip_key ,e.equip_name
             from[PRODUCTIONLINE_UV] c ,P_LINE_WORK_CENTER A, WORKCENTER_UV B ,WORK_CENTER_EQUIPMENT d, EQUIPMENT_UV e where c.[p_line_mc_u_S] != '' and A.[parent_key] = c.p_line_key and A.[child_key] =b.wc_key and  b.wc_key =d.parent_key and  e.equip_key = d.child_key");
            string sql = strSql.ToString();
            return Repository().FindTableBySql(strSql.ToString());

        }
        public DataTable GetTreeNode1()//加载树的第二个节点
        {

            
           StringBuilder strSql = new StringBuilder();
           
            strSql.Append(@"SELECT c.p_line_key,c.p_line_name,A.[parent_key] ,A.[child_key],b.wc_key,b.wc_name,b.wc_mc_u_S
                 from [PRODUCTIONLINE_UV] c ,P_LINE_WORK_CENTER A, WORKCENTER_UV B  where c.[p_line_mc_u_S] != '' and A.[parent_key] = c.p_line_key and A.[child_key] =b.wc_key");

           
            string sql = strSql.ToString();
            return Repository().FindTableBySql(strSql.ToString());

        }
        public DataTable GetTreeNode0()//加载树的第一个节点
        {

            StringBuilder strSql = new StringBuilder();
            //List<DbParameter> parameter = new List<DbParameter>();

            strSql.Append("SELECT p_line_key,[p_line_mc_u_S], p_line_name from[PRODUCTIONLINE_UV] where [p_line_mc_u_S] != '' ");
            //strSql.Append("SELECT A.part_key,A .part_number ,B.part_mc_u_S ,B .part_type_u_S ");
            //strSql.Append(" FROM PART A,UDA_Part B");
            //strSql.Append(" WHERE A.part_key =B.object_key   AND B.part_type_u_S='产品' ");
            string sql = strSql.ToString();
            return Repository().FindTableBySql(strSql.ToString());
        }

        //获取产品下工位的信息
        public DataTable GetWcInfor1(string part_key, string ParameterJson, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();//PRODUCT_RS_WC_REL 这个是产品的工位信息配置表没错
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT B.wc_key,B.wc_name,B.category,C.wc_mc_u_S,B.uda_0
                          FROM   PRODUCT_RS_WC_REL A ,WORK_CENTER B ,UDA_WorkCenter C ,UDA_Part D
                          WHERE  A.wc_key =C.object_key  and B.wc_key =C.object_key and  D.route_30 =A.route_key 
                  ");
            strSql.Append(" AND  D.object_key = '" + part_key + "' ");
            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }

        //查询文件用的方法
        public DataTable searchfile1(string type, string keywords, string ParameterJson, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            if (type != "" && type != null)
            {
                //[document_key], [document_code],[document_name],[document_type] , [document_edition], [document_size]
                switch (type)
                {
                    case "1"://查所有
                        strSql.Append(@"select [document_key], [document_code],[document_name],[document_type] , [document_edition], [document_size]   FROM AD_Document");
                        break;
                    case "2"://查所有
                        strSql.Append(@"select [document_key], [document_code],[document_name],[document_type] , [document_edition], [document_size]   FROM AD_Document ,AD_document_type where AD_document_type .document_type_key  = AD_Document.document_type and AD_document_type.type_name like '%JES文件%' ");
                        break;
                    case "3"://查所有
                        strSql.Append(@"select [document_key], [document_code],[document_name],[document_type] , [document_edition], [document_size]   FROM AD_Document ,AD_document_type where AD_document_type .document_type_key  = AD_Document.document_type and AD_document_type.type_name like '%通用文件%'");
                        break;
                    case "4"://查所有
                        strSql.Append(@"select [document_key], [document_code],[document_name],[document_type] , [document_edition], [document_size]   FROM AD_Document ,AD_document_type where AD_document_type .document_type_key  = AD_Document.document_type and AD_document_type.type_name like '%TPM文件%'");
                        break;
                    case "5"://按编号
                        strSql.Append(@"select [document_key], [document_code],[document_name],[document_type] , [document_edition], [document_size]   FROM AD_Document where document_code like '%" + keywords + "%'");
                        break;
                    case "6"://按名称
                        strSql.Append(@"select [document_key], [document_code],[document_name],[document_type] , [document_edition], [document_size]   FROM AD_Document where document_name like '%" + keywords + "%'");
                        break;
                }
            }
            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        public DataTable GetFilesTable(string wc_key, string part_key, string ParameterJson, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();//PRODUCT_RS_WC_REL 这个是产品的工位信息配置表没错
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT A.atr_key
                          , B.[document_code]
                          , B.[document_name]
                          , B.[document_type]
                          , B.[document_edition]
                          , B.[document_size]
                          , B.[document_file]
                          , B.[upload_PC_Mac]
                          , B.[document_storage_path]
                          , B.[upload_PC_IP]
                          , B.[upload_PC_name]
                          , B.[remarks]
                          FROM   PROCESS_FILES A ,AD_Document B
                          WHERE  A.[file_key] =B.[document_key]  
                  ");
            if (!string.IsNullOrEmpty(wc_key))
            {
                strSql.Append(" AND A.wc_key = '" + wc_key + "' ");
            }
            if (!string.IsNullOrEmpty(part_key))
            {
                strSql.Append(" AND A.product_key = '" + part_key + "' ");
            }

            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }

        public DataTable searchwc(string type, string keywords, string part_key, string ParameterJson, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            if (type != "" && type != null)
            {
                switch (type)
                {
                    case "1"://查所有
                        strSql.Append(@"SELECT B.wc_key,B.wc_name,B.category,C.wc_mc_u_S,B.uda_0
                          FROM   PRODUCT_RS_WC_REL A ,WORK_CENTER B ,UDA_WorkCenter C ,UDA_Part D
                          WHERE  A.wc_key =C.object_key  and B.wc_key =C.object_key and  D.route_30 =A.route_key AND  D.object_key = '" + part_key + "' ");
                        break;
                    case "2"://按编号
                        strSql.Append(@"SELECT B.wc_key,B.wc_name,B.category,C.wc_mc_u_S,B.uda_0
                          FROM   PRODUCT_RS_WC_REL A ,WORK_CENTER B ,UDA_WorkCenter C ,UDA_Part D
                          WHERE  A.wc_key =C.object_key  and B.wc_key =C.object_key and  D.route_30 =A.route_key AND  D.object_key = '" + part_key + "' AND B. wc_name like '%" + keywords + "%'");//AND wc_name like '%" + keywords + "%'
                        break;
                    case "3"://按名称
                        strSql.Append(@"SELECT B.wc_key,B.wc_name,B.category,C.wc_mc_u_S,B.uda_0
                          FROM   PRODUCT_RS_WC_REL A ,WORK_CENTER B ,UDA_WorkCenter C ,UDA_Part D
                          WHERE  A.wc_key =C.object_key  and B.wc_key =C.object_key and  D.route_30 =A.route_key AND  D.object_key = '" + part_key + "' AND C.wc_mc_u_S like '%" + keywords + "%'");// AND category like '%" + keywords + "%'
                        break;
                }
            }
            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }


        public DataTable showfile(string wc_key, string part_key, string ParameterJson, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();//PRODUCT_RS_WC_REL 这个是产品的工位信息配置表没错
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  A.atr_key
                          , A.product_key
                          , A.product_name
                          , A.product_model
                          , B.[document_code]
                          , B.[document_name]
                          , B.[document_type]
                          , B.[document_edition]
                          , B.[document_size]
                          , A.[Is_default]
						  , C.[type_name]
                          , B.[upload_PC_Mac]
                          , B.[document_storage_path]
                          , B.[upload_PC_IP]
                          , B.[upload_PC_name]
                          , B.[remarks]
                          FROM   PROCESS_FILES A ,AD_Document B,AD_document_type C
                          WHERE  A.[file_key] =B.[document_key] and B.document_type =C.document_type_key 
                  ");
            if (!string.IsNullOrEmpty(wc_key))
            {
                strSql.Append(" AND A.wc_key = '" + wc_key + "' ");
            }
            if (!string.IsNullOrEmpty(part_key))
            {
                strSql.Append(" AND A.product_key = '" + part_key + "' ");
            }
            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        public DataTable showTPMfile(string wc_key, string ParameterJson, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();//PRODUCT_RS_WC_REL 这个是产品的工位信息配置表没错
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT A.[document_type_key]
                           ,A.[type_name]
                          ,B.document_key
                          , B.[document_code]
                          , B.[document_name]
                          , B.[document_type]
                          , B.[document_edition]
                          , B.[document_size]
                      
                          , B.[upload_PC_Mac]
                          , B.[document_storage_path]
                          , B.[upload_PC_IP]
                          , B.[upload_PC_name]
                          , B.[remarks]
                          FROM   AD_document_type A,AD_Document B
                          WHERE  B.[document_type] =  A.[document_type_key] and A.[type_name] like  '%TPM%'
                  ");
            if (!string.IsNullOrEmpty(wc_key))
            {
                strSql.Append(" AND B.document_key not in  (select [file_key] from PROCESS_FILES where [wc_key]='" + wc_key + "') ");
            }
            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }

        /// <summary>
        /// 修改原来相同名称版本号信息
        /// </summary>
        /// <param name="version_key"></param>
        //public void updateISdefault1(string document_name)
        //{
        //    StringBuilder strSql = new StringBuilder();

        //    strSql.Append(" update [PROCESS_FILES] set Is_default ='否' where document_name= '" + document_name + "' ");
        //    //strSql.Append(" Is_default ='true'");
        //    string sql = strSql.ToString();
        //    Repository().ExecuteBySql(strSql);

        //}
        /// <summary>
        /// 修改当前的文件版本号信息
        /// </summary>
        /// <param name="version_key"></param>
        public int updateISdefault2(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" update [PROCESS_FILES] set Is_default ='是' where atr_key= '" + KeyValue + "' ");
            string sql = strSql.ToString();
            return Repository().ExecuteBySql(strSql);
        }
        /// <summary>
        /// 将当前机型工位下配置的文件全部设置为否
        /// </summary>
        /// <param name="version_key"></param>
        public void updateISdefault1(string product_key, string equip_key, string wc_key)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" update PROCESS_FILES set Is_default ='否' where product_key= '" + product_key + "' ");
            if (equip_key != "" && equip_key != null)
            {
                strSql.Append(@" and euipment_key='" + equip_key + "'");
            }
            if (wc_key != "" && wc_key != null)
            {
                strSql.Append(@" and wc_key='" + wc_key + "'");
            }
            string sql = strSql.ToString();
            Repository().ExecuteBySql(strSql);
        }


        /// </summary>
        /// <param name="version_key"></param>获取工位信息的通用方法
        /// <summary>
        /// 根据工位号，获取工厂建模的相关基本信息part_key就是工位的名称
        /// </summary>
        /// <param name="wccode"></param>
        /// <returns></returns>
        public Hashtable GetFactoryInfoByWC(string work_key)
        {
            Hashtable factoryinfo = new Hashtable();

            string sql_wc = "select wc.wc_key,wc.wc_name,uwc.wc_mc_u_S,wc.category,wc.uda_0,wc.description,wce.child_key  ";
            sql_wc += " from WORK_CENTER wc  LEFT OUTER JOIN UDA_WorkCenter uwc ON wc.wc_key=uwc.object_key ";
            sql_wc += " LEFT OUTER JOIN WORK_CENTER_EQUIPMENT wce ON wc.wc_key=wce.parent_key ";
            sql_wc += "  where wc.wc_key='" + work_key + "'";
            DataTable dt_wc = Repository().FindTableBySql(sql_wc);
            if (dt_wc != null && dt_wc.Rows.Count > 0)
            {
                DataRow dr_wc = dt_wc.Rows[0];
                string wc_key = dr_wc[0].ToString();
                factoryinfo.Add("wc_key", dr_wc[0].ToString());
                factoryinfo.Add("wc_code", dr_wc[1].ToString());
                factoryinfo.Add("wc_name", dr_wc[2].ToString());
                factoryinfo.Add("wc_category", dr_wc[3].ToString());
                factoryinfo.Add("wc_type", dr_wc[4].ToString());
                factoryinfo.Add("wc_description", dr_wc[5].ToString());
                string equip_key = dr_wc[6].ToString();
                if (!string.IsNullOrWhiteSpace(equip_key))
                {
                    string sql_equip = "select e.equip_key,e.equip_name,ue.equip_mc_u_S,e.category,e.description ";
                    sql_equip += " from EQUIPMENT e LEFT OUTER JOIN UDA_Equipment ue ON e.equip_key = ue.object_key ";
                    sql_equip += " where e.equip_key = '" + equip_key + "'";
                    DataTable dt_equip = Repository().FindTableBySql(sql_equip);
                    if (dt_equip != null && dt_equip.Rows.Count > 0)
                    {
                        DataRow dr_equip = dt_equip.Rows[0];
                        factoryinfo.Add("equip_key", dr_equip[0].ToString());
                        factoryinfo.Add("equip_code", dr_equip[1].ToString());
                        factoryinfo.Add("equip_name", dr_equip[2].ToString());
                        factoryinfo.Add("equip_category", dr_equip[3].ToString());
                        factoryinfo.Add("equip_description", dr_equip[4].ToString());
                    }

                    string sql_proline = "select pl.p_line_key,pl.p_line_name,upl.p_line_mc_u_S,pl.category,pl.uda_0,pl.description ";
                    sql_proline += " from PRODUCTION_LINE pl LEFT OUTER JOIN UDA_ProductionLine upl ON pl.p_line_key = upl.object_key ";
                    sql_proline += " LEFT OUTER JOIN P_LINE_WORK_CENTER plwc ON pl.p_line_key = plwc.parent_key ";
                    sql_proline += " where plwc.child_key = '" + wc_key + "'";
                    DataTable dt_proline = Repository().FindTableBySql(sql_proline);
                    if (dt_proline != null && dt_proline.Rows.Count > 0)
                    {
                        DataRow dr_proline = dt_proline.Rows[0];
                        string proline_key = dr_proline[0].ToString();
                        factoryinfo.Add("proline_key", dr_proline[0].ToString());
                        factoryinfo.Add("proline_code", dr_proline[1].ToString());
                        factoryinfo.Add("proline_name", dr_proline[2].ToString());
                        factoryinfo.Add("proline_category", dr_proline[3].ToString());
                        factoryinfo.Add("proline_type", dr_proline[4].ToString());
                        factoryinfo.Add("proline_description", dr_proline[5].ToString());

                        string sql_area = "select a.area_key,a.area_name,ua.area_mc_u_S,a.description ";
                        sql_area += " from AREA a LEFT OUTER JOIN UDA_Area ua ON a.area_key = ua.object_key ";
                        sql_area += " LEFT OUTER JOIN AREA_PRODUCTION_LINE apl ON a.area_key = apl.parent_key ";
                        sql_area += " where apl.child_key = '" + proline_key + "'";
                        DataTable dt_area = Repository().FindTableBySql(sql_area);
                        if (dt_area != null && dt_area.Rows.Count > 0)
                        {
                            DataRow dr_area = dt_area.Rows[0];
                            string area_key = dr_area[0].ToString();
                            factoryinfo.Add("area_key", dr_area[0].ToString());
                            factoryinfo.Add("area_code", dr_area[1].ToString());
                            factoryinfo.Add("area_name", dr_area[2].ToString());
                            factoryinfo.Add("area_description", dr_area[3].ToString());

                            string sql_site = "select s.site_key,s.site_name,us.site_mc_u_S,s.description ";
                            sql_site += " from SITE s LEFT OUTER JOIN UDA_Site us ON s.site_key = us.object_key ";
                            sql_site += " LEFT OUTER JOIN SITE_AREA sa ON s.site_key = sa.parent_key ";
                            sql_site += " where sa.child_key  = '" + area_key + "'";
                            DataTable dt_site = Repository().FindTableBySql(sql_site);
                            if (dt_site != null && dt_site.Rows.Count > 0)
                            {
                                DataRow dr_site = dt_site.Rows[0];
                                factoryinfo.Add("site_key", dr_site[0].ToString());
                                factoryinfo.Add("site_code", dr_site[1].ToString());
                                factoryinfo.Add("site_name", dr_site[2].ToString());
                                factoryinfo.Add("site_description", dr_site[3].ToString());
                            }
                        }
                    }
                }
            }
            return factoryinfo;
        }
       
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"select '100000' as ID, '文档类别' as name,'1' as code,'0'  as parentid,'0' as sort
                         union 
                         select document_type_key as ID,type_name as name,type_code as code, '100000'as parentid,'1' as sort from AD_document_type
                         where 1=1");
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
        
        public DataTable peizhiGetPageList(string files_key, string ParameterJson, ref JqGridParam jqgridparam)//点击文件夹获取文件
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
                            FROM   AD_Document   where document_type  ='" + files_key + "'");
            return Repository().FindTablePageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        public DataTable SearModel( string ParameterJson)//点击文件夹获取文件
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            //A.part_key, b.object_key
            strSql.Append(@"SELECT b.part_mc_u_S,part_key
                             FROM PART A,UDA_Part B
                             WHERE A.part_key =B.object_key  AND B.part_type_u_S='产品' 
                                 
                            ");
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }









    }
}