//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 停线日志表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.23 14:22</date>
    /// </author>
    /// </summary>
    public class BPIE_SLDocBll : RepositoryFactory<BPIE_SLDoc>
    {
        #region 全局变量定义区
        string tableName = "BPIE_SLDoc";
        #endregion

        #region 搜索
        public List<BPIE_SLDoc> GetPageListByCondition(string keywords, string Condition, string TimeStart, string TimeEnd, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (Condition == "all")
            {
                sql = @"select * from " + tableName + " where Datetime between '" + TimeStart + "' and '" + TimeEnd + "' order by VIN asc ";
                return Repository().FindListBySql(sql);
            }
            else
            {
                sql = @"select * from " + tableName + " where " + Condition + " like '%" + keywords + "%' and PastTime between '" + TimeStart + "' and '" + TimeEnd + "' order by AviCd asc";
                return Repository().FindListPageBySql(sql.ToString(), ref jqgridparam);

            }
        }
        #endregion

        #region 2.高级检索
        public DataTable GridPageStockTryList(List<ConditionAndKey> condition, DateTime? starttime, DateTime? endtime, ref JqGridParam jqgridparam)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select RecId,PlineId,PlineNm,State,SLSource,SLStrtTm,SLCmplTm,SLContTm,Enabled,Rem,RsvFld1,RsvFld2 from " + tableName + " where Enabled='1' and SLCmplTm<='" + endtime + "' and SLStrtTm>='" + starttime + "' ");
            foreach (var item in condition)
            {
                switch (item.Condition)
                {

                    case "PlineNm":
                        sql.Append(" and PlineNm");
                        break;
                    case "State":
                        sql.Append(" and State");
                        break;
                    case "SLSource":
                        sql.Append(" and SLSource");
                        break;
                    case "SLContTm":
                        sql.Append(" and SLContTm");
                        break;
                    
                }
                switch (item.ExpressExtension)
                {
                    case "like":
                        sql.Append(" like '%" + item.Keywords + "%'");
                        break;
                    case "=":
                        sql.Append(" ='" + item.Keywords + "'");
                        break;
                }
            }
            return Repository().FindTableBySql(sql.ToString(), false) ;
        }
        #endregion

    }
}