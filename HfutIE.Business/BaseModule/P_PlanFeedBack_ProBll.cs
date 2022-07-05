//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
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
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace HfutIE.Business
{
    /// <summary>
    /// P_PlanFeedBack_Pro
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.12.20 10:52</date>
    /// </author>
    /// </summary>
    public class P_PlanFeedBack_ProBll : RepositoryFactory<P_PlanFeedBack_Pro>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "P_PlanFeedBack_Pro";//===复制时需要修改===
        public readonly RepositoryFactory<P_PlanFeedBack_Pro> repository_feedbackbase = new RepositoryFactory<P_PlanFeedBack_Pro>();
        #endregion

        #region 方法区

        #region 1.查询未反馈数据
        /// <summary>
        ///     查询时提供了两个关键字，一个是Condition，另一个是keywords
        ///     
        ///     Condition是关键字，即查询条件，对应数据库中的一个字段
        ///     keywords是查询值，即查询条件的具体值，对应数据库中查询条件字段的值
        ///     查询的时候传递了Condition和keywords
        /// 
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="Condition">关键字（查询条件）</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns>查询的数据（列表）</returns>
        public DataTable GetPageListByCondition1(string AviCd, string VIN, string MatCd, string ProducePlanCd, string ColletionTimeStart, string ColletionTimeEnd, JqGridParam jqgridparam) //===复制时需要修改===
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append($@"select SUBSTRING(A.VIN,10,8)as ChassisCd,A.*,C.MatNm,B.AviCd,B.AviNm from P_PlanFeedBack_Pro A left join BBdbR_AVIBase B on A.OP_CODE = B.OP_CODE left join BBdbR_ProductBase C on A.MatCd = C.MatCd where B.AviCd like @AviCd and VIN like @VIN and A.MatCd like @MatCd and ProducePlanCd like @ProducePlanCd and FeedbackTime  IS NULL  ");
            parameter.Add(DbFactory.CreateDbParameter("@AviCd", "%" + AviCd + "%"));
            parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN ));
            parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + MatCd + "%"));
            parameter.Add(DbFactory.CreateDbParameter("@ProducePlanCd", "%" + ProducePlanCd + "%"));

            DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
            return dt;
        }
        #endregion

        #region 1.查询已反馈数据
        /// <summary>
        ///     查询时提供了两个关键字，一个是Condition，另一个是keywords
        ///     
        ///     Condition是关键字，即查询条件，对应数据库中的一个字段
        ///     keywords是查询值，即查询条件的具体值，对应数据库中查询条件字段的值
        ///     查询的时候传递了Condition和keywords
        /// 
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="Condition">关键字（查询条件）</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns>查询的数据（列表）</returns>
        public DataTable GetPageListByCondition2(string AviCd, string VIN, string MatCd, string ProducePlanCd, string ColletionTimeStart, string ColletionTimeEnd, JqGridParam jqgridparam) //===复制时需要修改===
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append($@"select SUBSTRING(A.VIN,10,8)as ChassisCd,A.*,C.MatNm,B.AviCd,B.AviNm from P_PlanFeedBack_Pro A left join BBdbR_AVIBase B on A.OP_CODE = B.OP_CODE left join BBdbR_ProductBase C on A.MatCd = C.MatCd where B.AviCd like @AviCd and VIN like @VIN and A.MatCd like @MatCd and ProducePlanCd like @ProducePlanCd  ");
            parameter.Add(DbFactory.CreateDbParameter("@AviCd", "%" + AviCd + "%"));
            parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN));
            parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + MatCd + "%"));
            parameter.Add(DbFactory.CreateDbParameter("@ProducePlanCd", "%" + ProducePlanCd + "%"));
            //过点反馈时间开始时间
            if (ColletionTimeStart != "" && ColletionTimeStart != null)
            {
                strSql.Append(" and DateDiff(dd,@ColletionTimeStart,FeedbackTime) >=0 ");
                parameter.Add(DbFactory.CreateDbParameter("@ColletionTimeStart", ColletionTimeStart));
            }
            else { }

            //过点反馈时间结束时间
            if (ColletionTimeEnd != "" && ColletionTimeEnd != null)
            {
                strSql.Append(" and DateDiff(dd,FeedbackTime,@ColletionTimeEnd) >=0 ");
                parameter.Add(DbFactory.CreateDbParameter("@ColletionTimeEnd", ColletionTimeEnd));
            }
            else { }
            strSql.Append(" order by FeedbackTime desc");
            DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
            return dt;


           
        }


        

        #endregion

        #region 2.待补录信息-采集补录界面

        public DataTable GetPageAddRecord(string AviCd, string VIN, string MatCd, string ProducePlanCd, string ColletionTimeStart, string ColletionTimeEnd, JqGridParam jqgridparam) //===复制时需要修改===
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append($@"select A.*,SUBSTRING(A.VIN,10,8)as ChassisCd,C.MatNm,B.AviCd,B.AviNm from P_PlanFeedBack_Pro  A left join BBdbR_AVIBase B on A.OP_CODE = B.OP_CODE  left join BBdbR_ProductBase C on A.MatCd = C.MatCd left join P_ProducePlan_Pro D on A.VIN = D.VIN where  A.OP_CODE< (select max(OP_CODE) from (select OP_CODE,FeedbackTime from P_PlanFeedBack_Pro as B where A.VIN=B.VIN and FeedbackTime is not null  ) ac ) and FeedbackTime is null AND  (FeedBackState!=1 or FeedBackState is null) and B.AviCd like @AviCd and A.VIN like @VIN and A.MatCd like @MatCd and A.ProducePlanCd like @ProducePlanCd ");
            parameter.Add(DbFactory.CreateDbParameter("@AviCd", "%" + AviCd + "%"));
            parameter.Add(DbFactory.CreateDbParameter("@VIN", "%" + VIN ));
            parameter.Add(DbFactory.CreateDbParameter("@MatCd", "%" + MatCd + "%"));
            parameter.Add(DbFactory.CreateDbParameter("@ProducePlanCd", "%" + ProducePlanCd + "%"));
            if (ColletionTimeStart != "" && ColletionTimeStart != null)
            {
                strSql.Append(" and DateDiff(dd,@ColletionTimeStart,PlanTime) >=0 ");
                parameter.Add(DbFactory.CreateDbParameter("@ColletionTimeStart", ColletionTimeStart));
            }
            else { }
            if (ColletionTimeEnd != "" && ColletionTimeEnd != null)
            {
                strSql.Append(" and DateDiff(dd,PlanTime,@ColletionTimeEnd) >=0 ");
                parameter.Add(DbFactory.CreateDbParameter("@ColletionTimeEnd", ColletionTimeEnd));
            }
            else { }
            DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
            return dt;

            //string sql = "";
            //sql = $@"select A.*,SUBSTRING(A.VIN,10,8)as ChassisCd,C.MatNm,B.AviCd,B.AviNm from P_PlanFeedBack_Pro  A left join BBdbR_AVIBase B on A.OP_CODE = B.OP_CODE  left join BBdbR_ProductBase C on A.MatCd = C.MatCd left join P_ProducePlan_Pro D on A.VIN = D.VIN where  A.OP_CODE< (select max(OP_CODE) from (select OP_CODE,FeedbackTime from P_PlanFeedBack_Pro as B where A.VIN=B.VIN and FeedbackTime is not null  ) ac ) and FeedbackTime is null AND  (FeedBackState!=1 or FeedBackState is null) and B.AviCd like '%{AviCd}%' and A.VIN like '%{VIN}%' and A.MatCd like '%{MatCd}%' and A.ProducePlanCd like '%{ProducePlanCd}%' ";
            //if (ColletionTimeStart != "" && ColletionTimeStart != null)
            //{
            //    string StartTime = DateDiff(ColletionTimeStart);    //计划开始时间距离当天的天数 大
            //    sql+= $@" and DateDiff(dd,PlanTime,getdate()) <=  " + StartTime;
            //}
            //else { }
            //if (ColletionTimeEnd != "" && ColletionTimeEnd != null)
            //{
            //    string EndTime = DateDiff(ColletionTimeEnd);    //计划结束时间距离当天的天数 小
            //    sql += $@" and DateDiff(dd,PlanTime,getdate()) >=  " + EndTime;
            //}
            //else { }
            //sql += $@"  order by a.ProducePlanCd DESC,a.OP_CODE  " ;
            //return Repository().FindTableBySql(sql.ToString(), false);

        }

        #region 计算日期与今天日期的天数
        public string DateDiff(string DateTime1)
        {
            try
            {
                DateTime today = DateTime.Now;//获取当前时间
                TimeSpan ts1 = new TimeSpan(Convert.ToDateTime(DateTime1).Ticks);
                TimeSpan ts2 = new TimeSpan(today.Ticks);
                TimeSpan ts = ts2.Subtract(ts1);
                string dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
                string day = ts.Days.ToString(); //获取datatime类型数据距离当前时间的天数
                return day;
            }
            catch (Exception)
            {
                return null;
            }

        }
        #endregion
        #endregion

        #region 3.编辑界面填充
        //编辑界面填充
        //public P_PlanFeedBack_Pro SetFeedbackInfor(string keywords) //===复制时需要修改===
        //{
        //    string strSql = "";
        //    strSql = @"select * from " + tableName + " where PlanFeedBackProId = '"+keywords+"'";
        //    DataTable dt = Repository().FindTableBySql(strSql.ToString(),false);


        //    P_PlanFeedBack_Pro entity = new P_PlanFeedBack_Pro();

        //    if (dt.Rows.Count > 0)
        //    {
        //        entity.PlanFeedBackProId = dt.Rows[0]["PlanFeedBackProId"].ToString();
        //        if (!string.IsNullOrEmpty(dt.Rows[0]["FeedbackTime"].ToString()))
        //        {
        //            entity.FeedbackTime = DateTime.Parse(dt.Rows[0]["FeedbackTime"].ToString().Trim());
        //        }

        //        entity.VIN = dt.Rows[0]["VIN"].ToString();
        //        entity.Rem = dt.Rows[0]["Rem"].ToString();
        //    }
        //    return entity;
        //}
        #endregion

        #region 4.编辑过点反馈方法
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        //public int Update(P_PlanFeedBack_Pro entity) //===复制时需要修改===
        //{
        //    StringBuilder Sql = new StringBuilder();
        //    Sql.Append($"UPDATE P_PlanFeedBack_Pro SET FeedbackTime = '{entity.FeedbackTime}' WHERE PlanFeedBackProId='{entity.PlanFeedBackProId}' ");
        //    Sql.Append($"UPDATE P_CarPastRecordInfo SET PastTime  = '{entity.FeedbackTime}' WHERE AviId=(SELECT AviId FROM BBdbR_AVIBase WHERE OP_CODE=(SELECT OP_CODE FROM P_PlanFeedBack_Pro WHERE PlanFeedBackProId='{entity.PlanFeedBackProId}')) AND VIN=(SELECT VIN FROM P_PlanFeedBack_Pro WHERE PlanFeedBackProId='{entity.PlanFeedBackProId}') ");
        //    int a = Repository().ExecuteBySql(Sql);
        //    return a; //将修改后的实体跟新到数据库中
        //}
        #endregion

        #region 5.新增过点反馈时间方法
        //entity实体中的数据是从页面中传来的，它是用户填写的数据
        //entity实体中只有部分字段有值，因为页面中只提供给部分字段赋值
        //将页面中填写的数据以实体（entity）的方式新增到数据库
        //其中，实体中的IsAvailable字段没有在页面中填写
        //IsAvailable字段的作用是做假删除，即数据库中的某一条数据的IsAvailable字段的字段值为true表示该数据存在
        //字段值为false表示该数据被删除
        //在删除数据库中的某一条数据时只要修改IsAvailable字段的字段值为false，并不删除该条数据
        //在新增时将实体的IsAvailable字段的值修改为true
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int addTime(P_PlanFeedBack_Pro entity) //===复制时需要修改===
        {
            
            IDatabase database = DataFactory.Database();
            StringBuilder Sql = new StringBuilder();
            

            //BBdbR_AVIBase aviinfo = database.FindEntity<BBdbR_AVIBase>("OP_CODE", entity.OP_CODE);

            Sql.Append($"UPDATE P_PlanFeedBack_Pro SET FeedbackTime = '{entity.FeedbackTime}',FeedBackState='0',MODIFY_ID = '{ManageProvider.Provider.Current().Account}',MODIFY_Time = getdate()  WHERE PlanFeedBackProId='{entity.PlanFeedBackProId}' ");
            int a = Repository().ExecuteBySql(Sql);
            //插入车身过点记录信息表
            string getsql = $"SELECT a.VIN,a.FeedbackTime,b.AviId,b.AviCd,b.AviNm,b.AviType,c.PlineId,d.PlineCd,d.PlineNm FROM P_PlanFeedBack_Pro a LEFT JOIN BBdbR_AVIBase b ON a.OP_CODE=b.OP_CODE left join BBdbR_AVIWhereaboutsConfig c on b.AviId = c.AviId left join BBdbR_PlineBase d on  c.PlineId = d.PlineId WHERE PlanFeedBackProId='{entity.PlanFeedBackProId}' ";
            DataTable InsertData = Repository().FindTableBySql(getsql.ToString(), false);
            
            P_CarPastRecordInfo recordinfo = database.FindEntityBySql<P_CarPastRecordInfo>("select * from P_CarPastRecordInfo where VIN = '" + InsertData.Rows[0]["VIN"].ToString() + "' and AviCd = '" + InsertData.Rows[0]["AviCd"].ToString() +"'");
            int b = 0;
            if (recordinfo.CarPastRecordId != null)//判断实体是否存在
            {
                #region 更新车身过点记录的过点时间
                //List<P_CarPastRecordInfo> recordlist = database.FindList<P_CarPastRecordInfo>("VIN", InsertData.Rows[0]["VIN"].ToString());
                //if (recordlist.Count > 0)
                //{
                //    recordinfo.ProducePlanCd = recordlist[0].ProducePlanCd;
                //    recordinfo.CarType = recordlist[0].CarType;
                //    recordinfo.CarColor1 = recordlist[0].CarColor1;
                //    recordinfo.MatCd = recordlist[0].MatCd;
                //}
                P_ProducePlan_Pro carinfo = database.FindEntity<P_ProducePlan_Pro>("VIN", InsertData.Rows[0]["VIN"].ToString());
                if (carinfo.PlanProId != null)
                {
                    recordinfo.ProducePlanCd = carinfo.ProducePlanCd;
                    recordinfo.CarType = carinfo.CarType;
                    recordinfo.CarColor1 = carinfo.CarColor1;
                    recordinfo.MatCd = carinfo.MatCd;
                }
                else
                {
                    recordinfo.ProducePlanCd = "";
                    recordinfo.CarType = "";
                    recordinfo.CarColor1 = "";
                    recordinfo.MatCd = "";
                }
                

                recordinfo.PastTime = entity.FeedbackTime;
                recordinfo.PastType = 2;//后台补录数据
                recordinfo.IsBack = 0;
                recordinfo.CreStaff = ManageProvider.Provider.Current().Account;
                b = database.Update(recordinfo);
                #endregion
            }
            else
            {
                #region 新增车身过点记录信息
                P_CarPastRecordInfo carpastentity = new P_CarPastRecordInfo();

                List<P_CarPastRecordInfo> recordlist = database.FindListBySql<P_CarPastRecordInfo>($"select * from P_CarPastRecordInfo where  VIN = '{InsertData.Rows[0]["VIN"].ToString()}' and SequenceNo!=''");

                if (recordlist.Count > 0)
                {
                    recordlist = recordlist.OrderByDescending(it => it.PastTime).ToList(); //从大到小
                    
                    carpastentity.SequenceNo = recordlist[0].SequenceNo;
                    carpastentity.PastNo = recordlist[0].PastNo;
                }
                else
                {
                    //if (InsertData.Rows[0]["AviCd"].ToString() == "TRIM-1")
                    //{
                    //    carpastentity.SequenceNo = recordlist[0].SequenceNo;
                    //    string PastDate = DateTime.Now.ToString("yyyy-MM-dd");
                    //    string sqlcarseq = "select count(distinct VIN) from P_CarPastRecordInfo where AviCd = 'TRIM-1' AND VIN!='9999' and DateDiff(dd,PastTime,getdate())=0";//查找当天过点车

                    //    DataTable counttable = DbHelperSQL.OpenTable(sqlcarseq);//找到当日过点记录的个数，加1则为本次过点的顺序
                    //    int pastnm = Convert.ToInt32(counttable.Rows[0][0].ToString()) + 1;

                    //    carpastentity.SequenceNo = PastDate.Replace("-", "") + string.Format("{0:d4}", pastnm);
                    //}
                }
                
                P_ProducePlan_Pro carinfo = database.FindEntity<P_ProducePlan_Pro>("VIN", InsertData.Rows[0]["VIN"].ToString());
                if (carinfo.PlanProId != null)
                {
                    carpastentity.ProducePlanCd = carinfo.ProducePlanCd;
                    carpastentity.CarType = carinfo.CarType;
                    carpastentity.CarColor1 = carinfo.CarColor1;
                    carpastentity.MatCd = carinfo.MatCd;
                }
                else
                {
                    carpastentity.ProducePlanCd = "";
                    carpastentity.CarType = "";
                    carpastentity.CarColor1 = "";
                    carpastentity.MatCd = "";
                }
                

                carpastentity.VIN = InsertData.Rows[0]["VIN"].ToString();

                carpastentity.AviId = InsertData.Rows[0]["AviId"].ToString();
                carpastentity.AviCd = InsertData.Rows[0]["AviCd"].ToString();
                carpastentity.AviNm = InsertData.Rows[0]["AviNm"].ToString();
                carpastentity.AviType = InsertData.Rows[0]["AviType"].ToString();
                carpastentity.PlineCd = InsertData.Rows[0]["PlineCd"].ToString();
                carpastentity.PlineNm = InsertData.Rows[0]["PlineNm"].ToString();

                carpastentity.CarRoute = 1;
                carpastentity.PastType = 2;//后台补录数据
                carpastentity.PlineId = InsertData.Rows[0]["PlineId"].ToString();
                carpastentity.PastDate = entity.FeedbackTime.ToString();
                carpastentity.PastTime = entity.FeedbackTime;

                carpastentity.CreTm = entity.FeedbackTime;
                carpastentity.CreStaff = ManageProvider.Provider.Current().Account;
                carpastentity.IsBack = 0;
                carpastentity.Create();

                b = database.Insert(carpastentity);
                #endregion
            }



            return b; 
        }
        #endregion

        #region 7.删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int OneRecord(string[] array)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<P_PlanFeedBack_Pro> listEntity = new List<P_PlanFeedBack_Pro>(); //===复制时需要修改===
            foreach (string keyValue in array)
            {
                //===复制时需要修改===
                P_PlanFeedBack_Pro entity = Repository().FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                entity.FeedbackTime = DateTime.Now;//将该实体的IsAvailable属性改为false
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion

        #endregion
    }
}