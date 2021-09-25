//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2014
// Software Developers @ HfutIE 2014
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// ƒ£øÈ…Ë÷√
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.06.22 19:35</date>
    /// </author>
    /// </summary>
    public class Base_ModuleBll : RepositoryFactory<Base_Module>
    {
        public List<Base_Module> GetList()
        {
            return this.Repository().FindList("ORDER BY ParentId ASC,SortCode ASC");
        }
    }
}