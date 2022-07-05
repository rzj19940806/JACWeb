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
    /// 车身过点记录信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.23 14:22</date>
    /// </author>
    /// </summary>
    public class P_CarPastRecordInfoBll : RepositoryFactory<P_CarPastRecordInfo>
    {
        #region 全局变量定义区
        string tableName = "P_CarPastRecordInfo";
        #endregion

        #region 搜索
        public List<P_CarPastRecordInfo> GetPageListByCondition(string AviCd, string VIN, string CarType, string MatCd, string ProducePlanCd, string TimeStart, string TimeEnd, JqGridParam jqgridparam) //===复制时需要修改===
        {
                string sql = $"select * from { tableName} where AviCd  like '%{AviCd}%' and VIN like '%{VIN}%' and CarType like '%{CarType}%' and MatCd like '%{MatCd}%' and ProducePlanCd like '%{ProducePlanCd}%' and PastTime between '{TimeStart}' and '{TimeEnd} 23:59:59' and VIN !='9999' and (AviCd not like'PBS%' or AviCd = 'PBS-1') order by {jqgridparam.sidx} {jqgridparam.sord}";
                return Repository().FindListBySql(sql);

        }
        #endregion

        #region 2.高级检索
        public List<P_CarPastRecordInfo> GridPageStockTryList(List<ConditionAndKey> condition, DateTime? starttime, DateTime? endtime, ref JqGridParam jqgridparam)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select CarPastRecordId,AviId,AviCd,AviNm,AviCatg,AviType,BodyNo,CarType,MatCd,CarColor1,CarColor2,VIN,SequenceNo,ProducePlanCd,CarRoute,PlineId,PlineCd,PlineNm,PastType,PastDate,PastTime,PastNo,RecordType,SpecialTag,CreTm,CreStaff,IsBack,Rem,RsvFld1,RsvFld2 from "+tableName+ " where VIN !='9999'  and (AviCd not like'PBS%' or AviCd = 'PBS-1') and PastTime<='" + endtime + "' and PastTime>='" + starttime + "' ");
            foreach (var item in condition)
            {
                switch (item.Condition)
                {
                    case "AviCd":
                        sql.Append(" and AviCd");
                        break;
                    case "AviNm":
                        sql.Append(" and AviNm");
                        break;
                    case "CarColor1":
                        sql.Append(" and CarColor1");
                        break;
                    case "MatCd":
                        sql.Append(" and MatCd");
                        break;
                    case "PlineCd":
                        sql.Append(" and PlineCd");
                        break;
                    case "PlineNm":
                        sql.Append(" and PlineNm");
                        break;
                    case "VIN":
                        sql.Append(" and VIN");
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
            return Repository().FindListBySql(sql.ToString());
        }
        #endregion

    }
}