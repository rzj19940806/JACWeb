//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2016
// Software Developers @ HfutIE 2016
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace HfutIE.Business
{
    /// <summary>
    /// Base_ProductLine
    /// <author>
    ///		<name>she</name>
    ///		<date>2016.10.04 15:10</date>
    /// </author>
    /// </summary>
    public class Base_ProductLineBll : RepositoryFactory<Base_ProductLine>
    {
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FactoryId">factory id</param>
        /// <returns></returns>
        public DataTable GetList(string FactoryId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"
SELECT  *
                            FROM    (SELECT  d.ProductLineId,
                                    d.FactoryId,
                                    d.ProductLineName,
                                    d.ProdcutLineCode,
                                    d.Description
                            FROM   Base_ProductLine d) T
                            WHERE   1 = 1 ");
            List<DbParameter> parameter = new List<DbParameter>();
            if (!string.IsNullOrEmpty(FactoryId))
            {
                strSql.Append(" AND FactoryId = @FactoryId");
                parameter.Add(DbFactory.CreateDbParameter("@FactoryId", FactoryId));
            }           
          
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
    }
}