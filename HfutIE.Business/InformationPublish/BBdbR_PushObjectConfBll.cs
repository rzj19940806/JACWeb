//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
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
    /// 信息推送对象配置表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.24 09:38</date>
    /// </author>
    /// </summary>
    public class BBdbR_PushObjectConfBll : RepositoryFactory<BBdbR_PushObjectConf>
    {
        #region
        #endregion

        #region 3.展示表格
        /// <summary>
        /// 搜索表格中所有IsAvailable = true的数据
        /// </summary>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns>返回搜索到的数据</returns>
        public List<BBdbR_PushObjectConf> GetPageList(JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder(); //创建字符串拼接对象
            strSql.Append(@"select * from BBdbR_PushObjectConf where Enabled = 1");
            return Repository().FindListPageBySql(strSql.ToString(), ref jqgridparam);
        }
        #endregion

        

        #region 查询
        public DataTable SelectConIn(string condition, string keywords, string ParameterJson, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT InforTypCd,InforTyp,ObjectTyp,ObjectId,PushRank,IntvlTm,TmUnit FROM BBdbR_PushObjectConf where Enabled=1 ");
            switch (condition)
            {
                case "infortypcd":
                    strSql.Append(" and infortypcd LIKE  '%" + keywords + "%'");
                    break;
                case "infortyp":
                    strSql.Append(" and infortyp LIKE  '%" + keywords + "%'");
                    break;
                default:
                    break;
            }
            return Repository().FindTableBySql(strSql.ToString());
        }

        #endregion

    }
}