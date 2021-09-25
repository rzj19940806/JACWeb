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
    /// 群聊基础表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.25 13:34</date>
    /// </author>
    /// </summary>
    public class BBdbR_GroupChatBll : RepositoryFactory<BBdbR_GroupChat>
    {
        #region 3.获取未配置群聊信息
        public System.Data.DataTable GetNoConList(string Tycd, string ParameterJson, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from BBdbR_GroupChat where Enabled = 1");
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string code = dt.Rows[i]["GroupchatId"].ToString();
                StringBuilder str = new StringBuilder();
                str.Append(@"select * from BBdbR_PushObjectConf where ObjectId='" + code + "' and InforTypCd='" + Tycd + "'and Enabled='" + 1 + "'");
                DataTable dt1 = Repository().FindTableBySql(str.ToString());
                if (dt1.Rows.Count != 0)
                {
                    dt.Rows.Remove(dr);
                }
            }
            return dt;
        }
        #endregion

        #region 获取已配置群聊
        public DataTable GetConGroupList(string Tycd, string Rank, string ParameterJson, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from PushObject_Groupchat_V where InforTypCd='" + Tycd + "'and PushRank = '" + Rank + "' and Enabled='" + 1 + "'");
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            return dt;
        }
        #endregion

        #region 删除之前群聊的配置
        public int DeleteConGroup(string Tycd, string Type, string Rank)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"Delete from BBdbR_PushObjectConf  where InforTypCd = '" + Tycd + "' and PushRank = '" + Rank + "' and ObjectTyp = '" + 3 + "'");
            return Repository().ExecuteBySql(strSql);
        }
        #endregion

        #region  4.查询群聊信息
        public DataTable GetNoConGroupList(string condition, string keywords, string Tycd, string ParameterJson, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT GroupchatId,GroupchatNm,Qrcode,GroupchatAddr FROM BBdbR_GroupChat where Enabled=1 ");
            switch (condition)
            {
                case "qrcode"://群号
                    strSql.Append(" and qrcode LIKE  '%" + keywords + "%'");
                    break;
                case "groupchatnm"://群名称
                    strSql.Append(" and groupchatnm LIKE  '%" + keywords + "%'");
                    break;
                default:
                    break;
            }
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string code = dt.Rows[i]["GroupchatId"].ToString();
                StringBuilder str = new StringBuilder();
                str.Append(@"select * from BBdbR_PushObjectConf where ObjectId='" + code + "' and InforTypCd='" + Tycd + "'and Enabled='" + 1 + "'");
                DataTable dt1 = Repository().FindTableBySql(str.ToString());
                if (dt1.Rows.Count != 0)
                {
                    dt.Rows.Remove(dr);
                }
            }
            return dt;
            //return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion
    }
}