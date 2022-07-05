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
    public class Q_KeyPartsBind_ProBll : RepositoryFactory<Q_KeyPartsBind_Pro>
    {
        #region 全局变量定义区
        string tableName = "Q_KeyPartsBind_Pro";
        #endregion



        #region 2.高级检索
        public DataTable GetPageListByCondition(string VIN, string OrderCd, string CarType, string ProductMatCd, string MatCd, string MatNm, string SupplierCd, string PlineNm, string WcCd, string TimeStart, string TimeEnd, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = $"select * from Q_KeyPartsBind_Pro  where VIN like '%{VIN}%' and OrderCd like '%{OrderCd}%' and CarType like '%{CarType}%' and ProductMatCd like '%{ProductMatCd}%' and MatCd like '%{MatCd}%' and MatNm like '%{MatNm}%' and SupplierCd like '%{SupplierCd}%' and PlineNm like '%{PlineNm}%' and WcCd like '%{WcCd}%' and Datetime between '{TimeStart}' and '{TimeEnd} 23:59:59' order by {jqgridparam.sidx} {jqgridparam.sord}";
            return Repository().FindTableBySql(sql.ToString(), false);

        }
        #endregion

    }
}