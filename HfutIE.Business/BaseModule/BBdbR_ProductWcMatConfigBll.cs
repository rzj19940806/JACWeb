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
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 产品工位物料配置
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.06 20:03</date>
    /// </author>
    /// </summary>
    public class BBdbR_ProductWcMatConfigBll : RepositoryFactory<BBdbR_ProductWcMatConfig>
    {
        #region 7.物料配置
        public DataTable GetMatList(string productMatId,string wcId)//在首页加载已经配置过的物料信息
        {
            string sql = $"select ProductClassMatId,ProductId,ProductCd,ProductNm,A.MatId,A.MatCd,A.MatNm,A.WcId,A.WcCd,A.WcNm,A.IsScan,A.MatNum,A.IsPrint,A.ShortCode,B.WcId matWcId, B.WcCd matWcCd, B.WcNm matWcNm, B.IsScan matIsScan, B.MatNum matMatNum, B.IsPrint matIsPrint, B.ShortCode matShortCode,B.RsvFld1 from BBdbR_ProductWcMatConfig A with(nolock) join BBdbR_MatBase B with(nolock) on A.MatId = B.MatId and A.Enabled = 1 and B.Enabled = 1 and A.ProductId = '{productMatId}' left join BBdbR_WcBase C with(nolock) on B.WcId = C.WcId left join BBdbR_PlineBase D with(nolock) on C.PlineId = D.PlineId where B.WcCd = '{wcId}' or '{wcId}' = '' or D.PlineCd = '{wcId}'  or B.RsvFld1 in (select WcCd from BBdbR_WcBase where PlineId in (select PlineId from BBdbR_PlineBase where PlineCd = '{wcId}'))  order by case when D.PlineCd is null then 'Z' else D.PlineCd end,B.WcCd";
            return Repository().FindTableBySql(sql, false);
        }
        public DataTable GetNotConfigMat(string productclassid, ref JqGridParam jqgridparam)//在表单中加载没配置过的物料清单
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append($"select * from BBdbR_MatBase where MatId not in (select distinct MatId from BBdbR_ProductWcMatConfig where Enabled = 1 and ProductId = '{productclassid}') and MatCatg = '0' and Enabled = 1");
            return Repository().FindTableBySql(strSql.ToString(), false);
        }
        public DataTable GetNotConfigMatbycondition(string keywords, string Condition, string productclassid, ref JqGridParam jqgridparam)//查询得到没配置过的物料清单
        {
            StringBuilder strSql = new StringBuilder();
            if (Condition == "all")
            {
                strSql.Append(@"select *from BBdbR_MatBase where MatId not in(select distinct MatId from BBdbR_ProductWcMatConfig where Enabled=1 and ProductclassId='" + productclassid + "' ) and MatCatg='零部件' and Enabled=1");
            }
            else
            {
                strSql.Append(@"select * from BBdbR_MatBase where MatId not in(select distinct MatId from BBdbR_ProductWcMatConfig where Enabled=1 and ProductclassId='" + productclassid + "') and MatCatg='零部件' and " + Condition + " like '%" + keywords + "%' and Enabled=1");
            }
            return Repository().FindTableBySql(strSql.ToString(), false);
        }
        public DataTable GetPlineId(string WcId)//根据传入的工位主键查询工位产线信息
        {
            string sql = $"select * from BBdbR_WcBase where Enabled=1 and WcId= '{WcId}' ";
            return Repository().FindTableBySql(sql, false);
        }
        public int MatInsert(BBdbR_ProductWcMatConfig entity)
        {
            Repository().Insert(entity);
            return 1;
        }
        #endregion 
    }
}
