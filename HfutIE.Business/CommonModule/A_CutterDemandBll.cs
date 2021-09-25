//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2014
// Software Developers @ HfutIE 2014
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 刀具齐套管理
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.06 10:59</date>
    /// </author>
    /// </summary>
    public class A_CutterDemandBll : RepositoryFactory<A_CutterDemand>
    {
        /// <summary>
        /// 获取刀具列表
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<A_CutterDemand> GetList(string value)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  A_CutterDemand where IsAvailable = 1 and Cstate !=3 and PlanID = " + value + "");
            //if (!string.IsNullOrEmpty(value))
            //{
            //    switch (value.Substring(0,1))
            //    {
            //        case "1":
            //            break;
            //        case "O":
            //            strSql.Append("and ProjectID in (SELECT ID FROM  A_ProjectInfomation where IsAvailable = 1 and OrderID = " + value.Substring(1) + ")");
            //            break;
            //        case "P":
            //            if (value.Substring(0, 4) == "PROJ")
            //            {
            //                strSql.Append("and ProjectID = " + value.Substring(4) + "");
            //            }
            //            else
            //            {
            //                strSql.Append("and PlanID = " + value.Substring(4) + "");
            //            }
            //            break;
            //    }
            //}
            return Repository().FindListBySql(strSql.ToString());
        }
        
        
        /// <summary>
        /// 确认齐套
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int ConfirmState(string value)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE A_CutterDemand SET Cstate = '3' where IsAvailable = 1 and PlanID = " + value);
            return Repository().ExecuteBySql(strSql);
        }
        /// <summary>
        /// 备注信息赋值
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public A_CutterDemand SetList(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  A_CutterDemand where IsAvailable = 1 and ID =" + KeyValue);
            return Repository().FindEntityBySql(strSql.ToString());
        }
        /// <summary>
        /// 更新备注信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="Remarks"></param>
        /// <returns></returns>
        public int UpdateRemarks(string KeyValue, string Remarks)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE A_CutterDemand SET Remarks = '" + Remarks + "' where ID = " + KeyValue);
            return Repository().ExecuteBySql(strSql);
        }
    }
}