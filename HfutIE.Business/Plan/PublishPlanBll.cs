//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace HfutIE.Business
{
    /// <summary>
    /// 发布序列过程表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.29 11:22</date>
    /// </author>
    /// </summary>
    public class PublishPlanBll : RepositoryFactory<P_PublishPlan_Pro>
    {
        #region 展示表格
        public List<P_PublishPlan_Pro> GridPageJson(string Condition,string StartTime, string EndTime,string State,string Keywords, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM P_PublishPlan_Pro WHERE Enabled='1' AND "+ Condition);

            if (Condition == "PlanTime" || Condition == "PlanPublishTime")
            {
                parameter.Add(DbFactory.CreateDbParameter("@StartTime", CommonHelper.GetDateTime(StartTime + " 00:00")));
                parameter.Add(DbFactory.CreateDbParameter("@EndTime", CommonHelper.GetDateTime(EndTime + " 23:59")));
                strSql.Append(" Between @StartTime AND @EndTime ");
            }
            else if (Condition == "OrderStatus" || Condition == "PlanStatus")
            {
                strSql.Append(" = '"+State+"'");
            }
            else
            {
                strSql.Append(" Like '%" + Keywords + "%'");
            }
            
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        #endregion

        #region
        //public int UpdatePlanSequence(string upId, string downId, string upQuene, string downQuene)
        //{
        //    IDatabase database = DataFactory.Database();
        //    DbTransaction isOpenTrans = database.BeginTrans();
        //    try
        //    {
        //        StringBuilder strSqlUp = new StringBuilder();//上方一行的数据
        //        StringBuilder strSqlDown = new StringBuilder(); //下方一行的数据
        //        strSqlUp.Append(@"UPDATE P_PublishPlan_Pro SET Quene='" + downQuene + "' where PublishPlanProId = '" + upId + "'");
        //        strSqlDown.Append(@"UPDATE P_PublishPlan_Pro SET Quene='" + upQuene + "' where PublishPlanProId = '" + downId + "'");
        //        int extorder = database.ExecuteBySql(strSqlUp, isOpenTrans);//执行Sql语句更新
        //        if (extorder > 0)
        //        {
        //            database.ExecuteBySql(strSqlDown, isOpenTrans);
        //        }
        //        database.Commit();//提交事务  
        //        return 1;
        //    }
        //    catch (System.Exception)
        //    {
        //        database.Rollback();
        //        return 0;
        //    }
            
        //}
        #endregion

        #region 上调顺序号
        public int UpQuene(string id, int num, string Quene)//上调
        {
            
            try
            {
                ArrayList sqllist = new ArrayList();
                int operationnum = num + 1;//
                string SqlUpOld = @"SELECT TOP " + operationnum + " PublishPlanProId,Quene FROM P_PublishPlan_Pro where Quene <= '" + Quene.Trim() + "' AND Enabled=1 ORDER BY Quene DESC";//向上查询num+1条数据
                DataTable UpOlddt = Repository().FindTableBySql(SqlUpOld, false);//执行Sql语句,查询要调整的所有列
                if (UpOlddt.Rows.Count > 0)
                {
                    string sqlTop = @"Update P_PublishPlan_Pro Set Quene= '" + UpOlddt.Rows[UpOlddt.Rows.Count-1]["Quene"].ToString().Trim() + "' where PublishPlanProId = '" + UpOlddt.Rows[0]["PublishPlanProId"].ToString().Trim() + "' ";//第一行下调至最后
                    sqllist.Add(sqlTop);
                    for (int i = 1; i < UpOlddt.Rows.Count; i++)
                    {
                        string sql = @"Update P_PublishPlan_Pro Set Quene= '" + UpOlddt.Rows[i - 1]["Quene"].ToString().Trim() + "' where PublishPlanProId = '" + UpOlddt.Rows[i]["PublishPlanProId"].ToString().Trim() + "' ";
                        sqllist.Add(sql);

                    }

                }
                string insertrow = DbHelperSQL.ExecuteSqlTran1(sqllist); //同时执行，实现事务回滚
                return 1;
            }
            catch (System.Exception)
            {
                return 0;
            }

        }
        #endregion


        #region 下调顺序号
        public int DownQuene(string id, int num, string Quene)//下调
        {

            try
            {
                ArrayList sqllist = new ArrayList();
                int operationnum = num + 1;//
                string SqlUpOld = @"SELECT TOP " + operationnum + " PublishPlanProId,Quene FROM P_PublishPlan_Pro where Quene >= '" + Quene.Trim() + "' AND Enabled=1 ORDER BY Quene asc";//向下查询num+1条数据
                DataTable UpOlddt = Repository().FindTableBySql(SqlUpOld, false);//执行Sql语句,查询要调整的所有列
                if (UpOlddt.Rows.Count > 0)
                {
                    
                    string sqlTop = @"Update P_PublishPlan_Pro Set Quene= '" + UpOlddt.Rows[UpOlddt.Rows.Count - 1]["Quene"].ToString().Trim() + "' where PublishPlanProId = '" + UpOlddt.Rows[0]["PublishPlanProId"].ToString().Trim() + "' ";//第一行下调至最后
                    sqllist.Add(sqlTop);
                    for (int i = 1; i < UpOlddt.Rows.Count; i++)
                    {
                        string sql = @"Update P_PublishPlan_Pro Set Quene= '" + UpOlddt.Rows[i-1]["Quene"].ToString().Trim() + "' where PublishPlanProId = '" + UpOlddt.Rows[i]["PublishPlanProId"].ToString().Trim() + "' ";
                        sqllist.Add(sql);
                        
                    }

                }
                else
                {
                    return 0;
                }
                string insertrow = DbHelperSQL.ExecuteSqlTran1(sqllist); //同时执行，实现事务回滚
                return 1;
            }
            catch (System.Exception)
            {
                return 0;
            }

        }
        #endregion

        #region 7.删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string[] array)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<P_PublishPlan_Pro> listEntity = new List<P_PublishPlan_Pro>(); //===复制时需要修改===
            foreach (string keyValue in array)
            {
                //===复制时需要修改===
                P_PublishPlan_Pro entity = Repository().FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                entity.Enabled = "0";//将该实体的IsAvailable属性改为false
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion
    }
}