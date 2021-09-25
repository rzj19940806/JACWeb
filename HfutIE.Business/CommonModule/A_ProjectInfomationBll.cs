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
    public class A_ProjectInfomationBll : RepositoryFactory<A_ProjectInfomation>
    {
        //获取项目列表
        public List<A_ProjectInfomation> GetProjectList(string ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  A_ProjectInfomation WHERE OrderID = " + ID + " and IsAvailable = 1");
            return Repository().FindListBySql(strSql.ToString());
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int DeleteProject(int value)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE  A_ProjectInfomation set IsAvailable = 0 where ID = " + value);
            //return Repository().FindListBySql(strSql.ToString());
            return DbHelperSQL.ExecuteSql(strSql.ToString());
        }
    }
}