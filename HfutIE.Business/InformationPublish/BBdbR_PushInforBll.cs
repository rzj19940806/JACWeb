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
    /// ������Ϣ������Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.25 21:11</date>
    /// </author>
    /// </summary>
    public class BBdbR_PushInforBll : RepositoryFactory<BBdbR_PushInfor>
    {
        #region
        #endregion

        #region ������

        #region  ����������Ϣ�� 
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select [Code] as ID,[FullName] as name,'0'AS ParentId,'0' as Sort from [Base_DataDictionaryDetail]where DataDictionaryId=(select [DataDictionaryId] from [Base_DataDictionary] where FullName='������Ϣ����')");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #region  ���������Ϣ��ȡ������Ϣ
        public DataTable GetInforList(string InforTypID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from BBdbR_PushInfor where InforTypCd='" + InforTypID + "'");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion
        #region  �༭������Ϣ
        public DataTable SetPushinfor(string keyvalue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from BBdbR_PushInfor where RecId='" + keyvalue + "'");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion
        #endregion

        #region ����������Ϣ
        public System.Data.DataTable GetInforPage()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from BBdbR_PushInfor where 1 = 1");
            DataTable dt = Repository().FindTableBySql(strSql.ToString());            
            return dt;
        }

        #endregion

        #region  4.��ѯ��Ϣ
        public DataTable SelectPushIn(string condition, string keywords, string ParameterJson, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT InforTypCd,InforTyp,PushRank,IntvlTm,TmUnit FROM BBdbR_PushInfor where 1=1 ");
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