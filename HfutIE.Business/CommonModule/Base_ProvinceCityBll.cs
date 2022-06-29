//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2014
// Software Developers @ HfutIE 2014
//=====================================================================================

using HfutIE.DataAccess;
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
    /// ʡ����
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.06 15:51</date>
    /// </author>
    /// </summary>
    public class Base_ProvinceCityBll : RepositoryFactory<Base_ProvinceCity>
    {
        /// <summary>
        /// ��ȡʡ���С��� �б�
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public List<Base_ProvinceCity> GetList(string ParentId)
        {
            StringBuilder WhereSql = new StringBuilder();
            WhereSql.Append(" AND ParentId = @ParentId Order By SortCode ASC");
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@ParentId", ParentId));
            return this.Repository().FindList(WhereSql.ToString(), parameter.ToArray());
        }
    }
}