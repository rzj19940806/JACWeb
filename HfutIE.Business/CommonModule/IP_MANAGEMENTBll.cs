//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2017
// Software Developers @ HfutIE 2017
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// IP_MANAGEMENT
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.08.17 09:37</date>
    /// </author>
    /// </summary>
    public class IP_MANAGEMENTBll : RepositoryFactory<IP_MANAGEMENT>
    {
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select '0' as parent_key,
                                   '1' as primery_key,
                                   '生产线' as code,
                                   '生产线' as name,
                                   'parent' as type
                            union all 
                            select '1' as parent_key,
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
                                   from WORK_CENTER join P_LINE_WC_LIST on  WORK_CENTER.wc_key=P_LINE_WC_LIST.wc_key 
                            union all 
                            select WC_EQUIPMENT_LIST.wc_key as parent_key,
                                   EQUIPMENT.equip_key as primery_key ,
                                   EQUIPMENT.equip_code as code,
                                   EQUIPMENT.equip_name as name,
                                   'equipment' as type
                                   from EQUIPMENT join WC_EQUIPMENT_LIST on EQUIPMENT.equip_key=WC_EQUIPMENT_LIST.equip_key
                            union all 
                            select P_LINE_EQUIP_LIST.p_line_key as parent_key,
                                   EQUIPMENT.equip_key as primery_key ,
                                   EQUIPMENT.equip_code as code,
                                   EQUIPMENT.equip_name as name,
                                   'equipment' as type
                                   from EQUIPMENT join P_LINE_EQUIP_LIST on EQUIPMENT.equip_key=P_LINE_EQUIP_LIST.equip_key order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        public List<IP_MANAGEMENT> GridList(string key, string type, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  [ip_management_key]
                                   ,[p_line_key]
                                   ,[p_line_code]
                                   ,[p_line_name]
                                   ,[wc_key]
                                   ,[wc_code]
                                   ,[wc_name]
                                   ,[equip_key]
                                   ,[equip_code]
                                   ,[equip_name]
                                   ,[equip_type]
                                   ,[IP_addr]
                                   ,[remark]
                                   ,[CreateDate]
                                   ,[CreateUserId]
                                   ,[CreateUserName]
                                   ,[ModifyDate]
                                   ,[ModifyUserId]
                                   ,[ModifyUserName]
                               FROM [JAC_FrontAxle].[dbo].[IP_MANAGEMENT] where 1=1 ");
            if (type == "productionline")
            {
                strSql.Append(@" and  p_line_key='" + key + "'");
            }
            else if (type == "workcenter")
            {
                strSql.Append(@" and  wc_key='" + key + "'");
            }
            else if (type == "equipment")
            {
                strSql.Append(@" and  equip_key='" + key + "'");
            }
            return Repository().FindListPageBySql(strSql.ToString(), ref jqgridparam);
        }
        public DataTable GetWC(string p_line_key)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select WORK_CENTER.wc_key,
                                   WORK_CENTER.wc_code,
                                   WORK_CENTER.wc_name
                              from WORK_CENTER join P_LINE_WC_LIST on WORK_CENTER.wc_key=P_LINE_WC_LIST.wc_key
                              where P_LINE_WC_LIST.p_line_key='" + p_line_key + "' order by wc_code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        public DataTable GetEquip(string parent_key,string type)
        {
            StringBuilder strSql = new StringBuilder();
            if(type== "productionline")
            {
                strSql.Append(@"select EQUIPMENT.equip_key,
                                       EQUIPMENT.equip_code,
                                       EQUIPMENT.equip_name
                                  from EQUIPMENT join P_LINE_EQUIP_LIST on EQUIPMENT.equip_key=P_LINE_EQUIP_LIST.equip_key
                                 where P_LINE_EQUIP_LIST.p_line_key='" + parent_key + "' ");
            }
            else if (type == "workcenter")
            {
                strSql.Append(@"select EQUIPMENT.equip_key,
                                       EQUIPMENT.equip_code,
                                       EQUIPMENT.equip_name
                                  from EQUIPMENT join WC_EQUIPMENT_LIST on EQUIPMENT.equip_key=WC_EQUIPMENT_LIST.equip_key
                                 where WC_EQUIPMENT_LIST.wc_key='" + parent_key + "' ");
            }
            return Repository().FindTableBySql(strSql.ToString());
        }
    }
}