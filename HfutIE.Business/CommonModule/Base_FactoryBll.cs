//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2016
// Software Developers @ HfutIE 2016
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using HfutIE.DataAccess;


namespace HfutIE.Business
{
    /// <summary>
    /// Base_Factory
    /// <author>
    ///		<name>she</name>
    ///		<date>2016.10.04 15:06</date>
    /// </author>
    /// </summary>
    public class Base_FactoryBll : RepositoryFactory<Base_Factory>
    {
        public List<Base_Factory> GetList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  c.FactoryId ,
                                    c.ParentId , 
                                    c.FactoryName,
                                    c.FactoryCode ,
                                    c.Description
                                  
                            FROM    Base_Factory c
                                   ");

            return Repository().FindListBySql(strSql.ToString());
        }
    }
}