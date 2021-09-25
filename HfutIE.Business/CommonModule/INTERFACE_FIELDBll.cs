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
    /// INTERFACE_FIELD
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.06.29 19:29</date>
    /// </author>
    /// </summary>
    public class INTERFACE_FIELDBll : RepositoryFactory<INTERFACE_FIELD>
    {
        public DataTable GetUnconfigedField(string interface_key, ref JqGridParam jqgridparam)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("Select name as to_table_field FROM SysColumns Where id=Object_Id((select to_table_name from interface_c where interface_key='" + interface_key + "')) and name not in (select to_table_field from INTERFACE_FIELD where interface_key='" + interface_key + "')");
            return Repository().FindTablePageBySql(strsql.ToString(), ref jqgridparam);
        }
        public DataTable GetConfigedField(string interface_key, ref JqGridParam jqgridparam)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append(@"SELECT   [interface_field_key]
                                    ,[interface_key]
                                    ,[interface_code]
                                    ,[interface_name]
                                    ,[from_table_field]
                                    ,[to_table_field]
                                    ,[is_identify_column]
                                    ,[reserve4]
                                    ,[reserve5]
                                    ,[reserve6]
                                    ,[reserve7]
                                    ,[reserve8]
                                    ,[reserve9]
                                    ,[reserve10]
                                    ,[CreateDate]
                                    ,[CreateUserId]
                                    ,[CreateUserName]
                                    ,[ModifyDate]
                                    ,[ModifyUserId]
                                    ,[ModifyUserName]
                                    FROM[JAC_FrontAxle].[dbo].[INTERFACE_FIELD] where 1=1 ");
            strsql.Append(" and interface_key='" + interface_key + "'");
            return Repository().FindTablePageBySql(strsql.ToString(), ref jqgridparam);
        }
    }
}