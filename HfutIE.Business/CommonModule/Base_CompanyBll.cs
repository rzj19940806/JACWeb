//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2014
// Software Developers @ HfutIE 2014
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 公司管理
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.06 10:59</date>
    /// </author>
    /// </summary>
    public class Base_CompanyBll : RepositoryFactory<Base_Company>
    {
        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <returns></returns>
        public List<Base_Company> GetList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  c.CompanyId ,
                                    c.ParentId ,
                                    c.Category ,
                                    c.Code ,
                                    c.FullName ,
                                    c.ShortName ,
                                    c.Nature ,
                                    u.RealName AS Manager ,
                                    c.Contact ,
                                    c.Phone ,
                                    c.Fax ,
                                    c.Email ,
                                    c.ProvinceId ,
                                    c.Province ,
                                    c.CityId ,
                                    c.City ,
                                    c.CountyId ,
                                    c.County ,
                                    c.Address ,
                                    c.AccountInfo ,
                                    c.Postalcode ,
                                    c.Web ,
                                    c.Remark ,
                                    c.Enabled ,
                                    c.SortCode ,
                                    c.DeleteMark ,
                                    c.CreateDate ,
                                    c.CreateUserId ,
                                    c.CreateUserName ,
                                    c.ModifyDate ,
                                    c.ModifyUserId ,
                                    c.ModifyUserName
                            FROM    Base_Company c
                                    LEFT JOIN dbo.Base_User u ON u.UserId = c.Manager");
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                strSql.Append(" AND ( c.CompanyId IN ( SELECT ResourceId FROM Base_DataScopePermission WHERE");
                strSql.Append(" ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "') ");
                strSql.Append(" ) )");
            }
            strSql.Append(" ORDER BY c.SortCode ASC");
            return Repository().FindListBySql(strSql.ToString());
        }
    }
}