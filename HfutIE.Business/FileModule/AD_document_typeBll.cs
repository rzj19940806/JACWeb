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
    /// AD_document_type
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.09.03 19:27</date>
    /// </author>
    /// </summary>
    public class AD_document_typeBll : RepositoryFactory<AD_document_type>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sorder">层级</param>
        /// <returns></returns>
        //public DataTable GetTree()
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    List<DbParameter> parameter = new List<DbParameter>();
        //    strSql.Append(@"SELECT  *
        //                    FROM    (select  '1' as document_type_key,
        //                                     '1' as type_code,
        //                                     '文档' as type_name,
        //                                      '0' as parent_key,
        //                                      '0' as sorder,
        //                                      '' as description
        //                           union
        //                           SELECT document_type_key,
        //                               type_code,
        //                                  type_name,
        //                                   parent_key,
        //                                    sorder,
        //                                 description
        //                           from AD_document_type  
        //                            ) T WHERE 1=1  ");
        //    if (!string.IsNullOrEmpty(sorder))
        //    {
        //        parameter.Add(DbFactory.CreateDbParameter("@sorder", '%' + sorder + '%'));
        //    }
        //    return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        //}


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



    }
}