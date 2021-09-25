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
    /// 物料程序齐套管理
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.06 10:59</date>
    /// </author>
    /// </summary>
    public class A_MaterialProgramDemandBll : RepositoryFactory<A_MaterialProgramDemand>
    {
        /// <summary>
        /// 获取物料程序列表
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<A_MaterialProgramDemand> GetList(string value,string type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  A_MaterialProgramDemand where IsAvailable = 1 and Cstate !=3 and PlanID = " + value + "");
            if (!string.IsNullOrEmpty(value))
            {
                //switch (value.Substring(0,1))
                //{
                //    case "1":
                //        break;
                //    case "O":
                //        strSql.Append("and ProjectID in (SELECT ID FROM  A_ProjectInfomation where IsAvailable = 1 and OrderID = " + value.Substring(1) + ")");
                //        break;
                //    case "P":
                //        if (value.Substring(0, 4) == "PROJ")
                //        {
                //            strSql.Append("and ProjectID = " + value.Substring(4) + "");
                //        }
                //        else
                //        {
                //            strSql.Append("and PlanID = " + value.Substring(4) + "");
                //        }
                //        break;
                //}
                if (!string.IsNullOrEmpty(type))
                {
                    switch (type)
                    {
                        case "物料管理员":
                            strSql.Append("and Type = 0");//物料
                            break;
                        case "程序管理员":
                            strSql.Append("and Type = 1");//程序
                            break;
                    }
                }

            }
            return Repository().FindListBySql(strSql.ToString());
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="Condition"></param>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        public List<A_MaterialProgramDemand> GetList(string Condition, string Keyword,string Role)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  A_MaterialProgramDemand where IsAvailable = 1");
            if (Condition == "OrderCode")
            {
                strSql.Append(" and Cstate !=3 and ProjectID in (SELECT ID FROM  A_ProjectInfomation where IsAvailable = 1 and OrderID in " +
                    "(SELECT ID FROM A_OrderBase where IsAvailable = 1 and OrderCode like '%" + Keyword + "%') ");
            }
            else if (Condition == "ProjectCode")
            {
                strSql.Append(" and Cstate !=3 and ProjectCode like '%" + Keyword + "%'");
            }
            else if (Condition == "Cstate")
            {
                if (Keyword == "" || Keyword == "1")
                {
                    strSql.Append(" and Cstate = 1");
                }
                else if (Keyword == "2")
                {
                    strSql.Append(" and Cstate = 2");
                }
                else
                {
                    strSql.Append(" and Cstate = 3");
                }
            }
            else
            {
                strSql.Append(" and Cstate !=3");
            }
            if(Role == "物料管理员")
            {
                strSql.Append(" and Type = 0");
            }
            else
            {
                strSql.Append(" and Type = 1");
            }
            return Repository().FindListBySql(strSql.ToString());
        }
        /// <summary>
        /// 确认齐套
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int ConfirmState(string value, string Role)
        {
            StringBuilder strSql = new StringBuilder();
            if(Role == "物料管理员")
            {
                strSql.Append(@"UPDATE A_MaterialProgramDemand SET Cstate = '3' where IsAvailable = 1 and Type =0 and PlanID = " + value);
            }
            else
            {
                strSql.Append(@"UPDATE A_MaterialProgramDemand SET Cstate = '3' where IsAvailable = 1 and Type =1  and ID = " + value);
            }
            
            return Repository().ExecuteBySql(strSql);
        }
        /// <summary>
        /// 备注信息赋值
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="Role"></param>
        /// <returns></returns>
        public A_MaterialProgramDemand SetList( string KeyValue, string Role)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  A_MaterialProgramDemand where IsAvailable = 1 and ID =" + KeyValue);
            if (Role == "物料管理员")
            {
                strSql.Append(" and Type = 0");
            }
            else
            {
                strSql.Append(" and Type = 1");
            }
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
            strSql.Append(@"UPDATE A_MaterialProgramDemand SET Remarks = '" + Remarks + "' where ID = " + KeyValue);
            return Repository().ExecuteBySql(strSql);
        }
    }
}