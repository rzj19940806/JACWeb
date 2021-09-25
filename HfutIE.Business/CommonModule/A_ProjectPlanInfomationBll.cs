//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2014
// Software Developers @ HfutIE 2014
//=====================================================================================

using HfutIE.DataAccess;
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
    /// 计划管理
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.06 10:59</date>
    /// </author>
    /// </summary>
    public class A_ProjectPlanInfomationBll : RepositoryFactory<A_ProjectPlanInfomation>
    {
        //加载树
        public DataTable GetOrderTree()
        {
            var strSql = new StringBuilder();
            strSql.Append(@"select a.ID as EntityId,a.OrderCode as EntityName, '0' as EntityParentId,'Order' AS Sort from A_OrderBase a where a.State in (0,1,2) and IsAvailable = 1
                            UNION 
                            select b.ID as  EntityId,b.ProjectCode as EntityName, b.OrderID as EntityParentId,'Project' AS Sort from A_ProjectInfomation b where IsAvailable = 1
                            UNION 
                            select '0' as EntityId,'全部订单' as EntityName, '-1' as EntityParentId,'AllOrder' AS Sort from A_OrderBase where ID = 1");
            // select c.ID as EntityId,c.PlanCode as EntityName, c.ProductID as EntityParentId ,'Plan' AS Sort from A_ProjectPlanInfomation c where IsAvailable = 1
            var rList = DataFactory.Database().FindTableBySql(strSql.ToString());
            //var rowv = rList.NewRow();
            //rowv["EntityId"] = "0";
            //rowv["EntityName"] = "全部订单";
            //rowv["ParentId"] = "-1";
            //rList.Rows.InsertAt(rowv,0);
            return rList;

        }
        //获取计划列表
        public List<A_ProjectPlanInfomation> GetPlanList(string CheckId, string Role)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append(@"SELECT  * FROM  A_ProjectInfomation WHERE OrderID = " + ID + " and IsAvailable = 1");
            //strSql.Append(@"select * from A_ProjectPlanInfomation where ID in (select PlanID from A_MaterialProgramDemand where Cstate !=3 ");
            if (!string.IsNullOrEmpty(Role))
            {
                switch (Role)
                {
                    case "物料管理员":
                        strSql.Append(@"select * from A_ProjectPlanInfomation where ID in (select PlanID from A_MaterialProgramDemand where Cstate !=3 and Type = 0");//物料
                        break;
                    case "程序管理员":
                        strSql.Append(@"select * from A_ProjectPlanInfomation where ID in (select PlanID from A_MaterialProgramDemand where Cstate !=3 and Type = 1");//程序
                        break;
                    case "刀具管理员":
                        strSql.Append(@"select * from A_ProjectPlanInfomation where ID in (select PlanID from A_CutterDemand where Cstate !=3 ");//刀具
                        break;
                }
            }
            if (!string.IsNullOrEmpty(CheckId))
            {
                switch (CheckId.Substring(0, 1))
                {
                    case "1":
                        strSql.Append(" )");
                        break;
                    case "O":
                        strSql.Append(" and ProjectID in (SELECT ID FROM  A_ProjectInfomation where IsAvailable = 1 and OrderID = " + CheckId.Substring(1) + "))");
                        break;
                    case "P":
                        strSql.Append(" and ProjectID = " + CheckId.Substring(4) + ")");
                        break;
                }
            }
            else
            {
                strSql.Append(" )");
            }


            return Repository().FindListBySql(strSql.ToString());
        }
        /// <summary>
        /// 查询刀具
        /// </summary>
        /// <param name="Condition"></param>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        public List<A_ProjectPlanInfomation> GetList(string Condition, string Keyword)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append(@"SELECT  * FROM  A_CutterDemand where IsAvailable = 1 ");

            if (Condition == "OrderCode")
            {
                strSql.Append(@"select * from A_ProjectPlanInfomation where ID in (select PlanID from A_CutterDemand where Cstate !=3 and ProjectID in (SELECT ID FROM  A_ProjectInfomation where IsAvailable = 1 and OrderID in (SELECT ID FROM A_OrderBase where IsAvailable = 1 and OrderCode like '%" + Keyword + "%')))");//刀具
                //strSql.Append(" and Cstate !=3 and ProjectID in (SELECT ID FROM  A_ProjectInfomation where IsAvailable = 1 and OrderID in " +
                //    "(SELECT ID FROM A_OrderBase where IsAvailable = 1 and OrderCode like '%" + Keyword + "%'))");
            }
            else if (Condition == "ProjectCode")
            {
                strSql.Append(@"select * from A_ProjectPlanInfomation where ID in (select PlanID from A_CutterDemand where Cstate !=3 and ProjectCode like '%" + Keyword + "%')");

            }
            else if (Condition == "Cstate")
            {
                if (Keyword == "" || Keyword == "1")
                {
                    strSql.Append(@"select * from A_ProjectPlanInfomation where ID in (select PlanID from A_CutterDemand where Cstate = 1 )");
                }
                else if (Keyword == "2")
                {
                    strSql.Append(@"select * from A_ProjectPlanInfomation where ID in (select PlanID from A_CutterDemand where Cstate = 2 )");
                }
                else
                {
                    strSql.Append(@"select * from A_ProjectPlanInfomation where ID in (select PlanID from A_CutterDemand where Cstate = 3 )");
                }
            }
            else
            {
                strSql.Append(@"select * from A_ProjectPlanInfomation where ID in (select PlanID from A_CutterDemand where Cstate !=3) ");//刀具
            }


            return Repository().FindListBySql(strSql.ToString());
        }
        /// <summary>
        /// 查询物料程序
        /// </summary>
        /// <param name="Condition"></param>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        public List<A_ProjectPlanInfomation> GetList(string Condition, string Keyword, string Role)
        {
            StringBuilder strSql = new StringBuilder();

            if (Condition == "OrderCode")
            {
                strSql.Append(@"select * from A_ProjectPlanInfomation where ID in (select PlanID from A_MaterialProgramDemand where Cstate !=3 ");
                if (Role == "物料管理员")
                {
                    strSql.Append(" and Type = 0");
                }
                else
                {
                    strSql.Append(" and Type = 1");
                }
                strSql.Append(" and ProjectID in (SELECT ID FROM  A_ProjectInfomation where IsAvailable = 1 and OrderID in (SELECT ID FROM A_OrderBase where IsAvailable = 1 and OrderCode like '%" + Keyword + "%')))");
                
            }
            else if (Condition == "ProjectCode")
            {
                strSql.Append(@"select * from A_ProjectPlanInfomation where ID in (select PlanID from A_MaterialProgramDemand where Cstate !=3 ");
                if (Role == "物料管理员")
                {
                    strSql.Append(" and Type = 0");
                }
                else
                {
                    strSql.Append(" and Type = 1");
                }
                strSql.Append(" and ProjectCode like '%" + Keyword + "%')");
            }
            else if (Condition == "Cstate")
            {
                if (Keyword == "" || Keyword == "1")
                {
                    strSql.Append(@"select * from A_ProjectPlanInfomation where ID in (select PlanID from A_MaterialProgramDemand where Cstate = 1 ");
                }
                else if (Keyword == "2")
                {
                    strSql.Append(@"select * from A_ProjectPlanInfomation where ID in (select PlanID from A_MaterialProgramDemand where Cstate = 2 ");
                }
                else
                {
                    strSql.Append(@"select * from A_ProjectPlanInfomation where ID in (select PlanID from A_MaterialProgramDemand where Cstate = 3 ");
                }

                if (Role == "物料管理员")
                {
                    strSql.Append(" and Type = 0)");
                }
                else
                {
                    strSql.Append(" and Type = 1)");
                }
            }
            else
            {
                strSql.Append(@"select * from A_ProjectPlanInfomation where ID in (select PlanID from A_MaterialProgramDemand where Cstate !=3 ");
                if (Role == "物料管理员")
                {
                    strSql.Append(" and Type = 0)");
                }
                else
                {
                    strSql.Append(" and Type = 1)");
                }
            }


            return Repository().FindListBySql(strSql.ToString());
        }

    }
}