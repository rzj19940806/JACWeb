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
using System.Diagnostics;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 推送信息基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.25 21:11</date>
    /// </author>
    /// </summary>
    public class PushObject_StfBase_VBll : RepositoryFactory<PushObject_StfBase_V>
    {
        #region
        #endregion

        #region 方法区

        #region  点击推送信息获取用户配置
        public DataTable GetConfigUserList(string selectTycd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from PushObject_StfBase_V where InforTypCd='" + selectTycd + "' and Enabled='" + 1 + "' and ObjectTyp= '" + 1 + "' ");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region Index界面查询
        public DataTable SelectUserConIn(string condition, string keywords, string ParameterJson, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT RecId,InforTypCd,InforTyp,ObjectTyp,ObjectId,StfCd,StfNm,PushRank,IntvlTm,TmUnit FROM PushObject_StfBase_V where  ObjectTyp=1 ");
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

        #endregion
    }
}