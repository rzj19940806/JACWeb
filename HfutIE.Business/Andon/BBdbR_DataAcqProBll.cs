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
    /// Andon数据采集过程表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.11 15:17</date>
    /// </author>
    /// </summary>
    public class BBdbR_DataAcqProBll : RepositoryFactory<BBdbR_DataAcqPro>
    {
        #region 全局变量定义区
        string tableName = "BBdbR_DataAcqPro";
        #endregion

        #region 加载时或根据主键查询Andon补录信息
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable GetAndonList(string KeyValue,string PlineId)
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();
            if (KeyValue == "")
            {
                //根据IP确定是哪条产线的pad，进而展示默认Andon数据
                strSql.Append(@"select a.RecId,a.SignalSource,a.AcqTm,a.SignalDetail,d.PlineNm, c.WcNm,a.CurValue ,b.PostNm from BBdbR_DataAcqPro a left join BBdbR_PlineBase d on a.PlineId=d.PlineId left join BBdbR_WcBase c on a.WcId =c.WcId left join BBdbR_PostBase b on a.PostId=b.PostId    where a.Enabled='1' and DateDiff(dd,AcqTm,getdate())=0 and SignalSource = 'Andon'and a.PlineId = '" + PlineId + "' order by AcqTm desc;");
                dt = Repository().FindTableBySql(strSql.ToString(), false);
            }
            else
            {
                strSql.Append(@"select a.RecId,a.SignalSource,a.SignalDetail,a.AcqTm,a.EndTm,a.HandlerCode,a.HandlerName,a.ExceptionType,a.ExceptionDes,a.HandleResult,a.Rem,a.CurValue,c.WcId,a.PostId,b.PostNm,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PlineBase d on a.PlineId=d.PlineId left join BBdbR_WcBase c on a.WcId =c.WcId left join BBdbR_PostBase b on a.PostId=b.PostId  where a.RecId = '" + KeyValue + "' and a.Enabled='1';");
                dt = Repository().FindTableBySql(strSql.ToString(),false);
            }
            return dt;
        }
        #endregion  

        #region 搜索
        public DataTable GetPageListByCondition(string PlineNm, string WcNm, string SignalSource, string SignalDetail, string CurValue, string HandleSts, string HandlerCode, string ExceptionType, string TimeStart, string TimeEnd, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = $"select PlineNm,WcNm,SignalSource,SignalDetail,CurValue,AcqTm,HandleSts,EndTm,HandleTm,a.HandlerCode,a.HandlerName,a.ExceptionType,a.ExceptionDes,HandleResult,a.Enabled,a.MdfTm,a.MdfNm,a.Rem from BBdbR_DataAcqPro a join BBdbR_PlineBase b on a.PlineId = b.PlineId and a.Enabled = '1' and PlineNm like '%{PlineNm}%' and SignalSource like '%{SignalSource}%' and SignalDetail like '%{SignalDetail}%' and CurValue like '%{CurValue}%' and HandleSts like '%{HandleSts}%' and (HandlerCode like '%{HandlerCode}%' or '{HandlerCode}'='') and (ExceptionType like '%{ExceptionType}%' or '{ExceptionType}'='') and AcqTm between '{TimeStart}' and '{TimeEnd} 23:59:59' left join BBdbR_WcBase c on a.WcId = C.WcId where WcNm like '%{WcNm}%' order by {jqgridparam.sidx} {jqgridparam.sord}";
            return Repository().FindTableBySql(sql.ToString(), false);

        }
        #endregion

        #region 根据查询条件查询Andon补录信息
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable GetAndonListByCondition(string KeyValue,string SearchTime)
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();
            if (KeyValue == "全部")
            {
                if (SearchTime=="")
                {
                    //精确到工位
                    strSql.Append(@"select a.RecId,a.SignalSource,a.AcqTm,a.PostId,a.SignalDetail,d.PlineNm, c.WcNm,a.CurValue ,b.PostNm from BBdbR_DataAcqPro a left join BBdbR_PlineBase d on a.PlineId=d.PlineId left join BBdbR_WcBase c on a.WcId =c.WcId left join BBdbR_PostBase b on a.PostId=b.PostId where a.Enabled='1'  and DateDiff(dd,AcqTm,getdate())=0 and SignalSource = 'Andon' order by AcqTm desc;");
                }
                else
                {
                    strSql.Append(@"select a.RecId,a.SignalSource,a.AcqTm,a.PostId,a.SignalDetail,d.PlineNm, c.WcNm,a.CurValue ,b.PostNm from BBdbR_DataAcqPro a left join BBdbR_PlineBase d on a.PlineId=d.PlineId left join BBdbR_WcBase c on a.WcId =c.WcId left join BBdbR_PostBase b on a.PostId=b.PostId where convert(varchar(10),a.[AcqTm],120)  = '" + SearchTime + "' and a.Enabled='1'   and SignalSource = 'Andon' order by AcqTm desc;");
                }
                
            }
            else 
            {
                if (SearchTime == "")
                {
                    //精确到工位
                    strSql.Append(@"select a.RecId,a.SignalSource,a.SignalDetail,a.AcqTm,a.CurValue,a.PostId,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PlineBase d on a.PlineId=d.PlineId left join BBdbR_WcBase c on a.WcId =c.WcId left join BBdbR_PostBase b on a.PostId=b.PostId  where d.PlineNm  LIKE '%" + KeyValue + "%'  and DateDiff(dd,AcqTm,getdate())=0 and a.Enabled='1' and  c.Enabled='1' and d.Enabled='1' and SignalSource = 'Andon' order by AcqTm desc;");
                }
                else
                {
                    //精确到工位
                    strSql.Append(@"select a.RecId,a.SignalSource,a.SignalDetail,a.AcqTm,a.CurValue,a.PostId,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PlineBase d on a.PlineId=d.PlineId left join BBdbR_WcBase c on a.WcId =c.WcId left join BBdbR_PostBase b on a.PostId=b.PostId  where d.PlineNm  LIKE '%" + KeyValue + "%' and convert(varchar(10),a.[AcqTm],120)  = '" + SearchTime + "' and a.Enabled='1' and  c.Enabled='1' and d.Enabled='1' and SignalSource = 'Andon' order by AcqTm desc;");
                }
            }
            dt = Repository().FindTableBySql(strSql.ToString(), false);
            return dt;
        }
        #endregion

        //#region 根据查询条件查询Andon补录信息-text
        ///// <summary>
        ///// 联合查询，展示页面表格
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns></returns>
        //public DataTable GetAndonListByConditiontext(string KeyValue, string Condition)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    DataTable dt = new DataTable();
        //    if (Condition == "SignalSource")
        //    {
        //        //精确到岗位
        //        //strSql.Append(@"select a.RecId,a.SignalSource,a.AcqTm,a.CurValue,b.PostNm,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PostBase b on a.PostId=b.PostId left join BBdbR_WcBase c on b.WcId =c.WcId left join BBdbR_PlineBase d on c.PlineId=d.PlineId where a.SignalSource LIKE '%" + KeyValue + "%' and a.Enabled='1' and b.Enabled='1' and c.Enabled='1' and d.Enabled='1' and SignalSource = 'Andon' order by AcqTm desc;");
        //        //精确到工位
        //        strSql.Append(@"select a.RecId,a.SignalSource,a.SignalDetail,a.AcqTm,a.CurValue,a.PostId,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PlineBase d on a.PlineId=d.PlineId left join BBdbR_WcBase c on a.WcId =c.WcId left join BBdbR_PostBase b on a.PostId=b.PostId  where a.SignalSource LIKE '%" + KeyValue + "%' and a.Enabled='1'   and SignalSource = 'Andon' order by AcqTm desc;");
        //    }
        //    else if (Condition == "PlineNm")
        //    {
        //        //精确到岗位
        //        //strSql.Append(@"select a.RecId,a.SignalSource,a.AcqTm,a.CurValue,b.PostNm,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PostBase b on a.PostId=b.PostId left join BBdbR_WcBase c on b.WcId =c.WcId left join BBdbR_PlineBase d on c.PlineId=d.PlineId where d.PlineNm  LIKE '%" + KeyValue + "%' and a.Enabled='1' and b.Enabled='1' and c.Enabled='1' and d.Enabled='1' and SignalSource = 'Andon' order by AcqTm desc;");
        //        //精确到工位
        //        strSql.Append(@"select a.RecId,a.SignalSource,a.SignalDetail,a.AcqTm,a.CurValue,a.PostId,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PlineBase d on a.PlineId=d.PlineId left join BBdbR_WcBase c on a.WcId =c.WcId left join BBdbR_PostBase b on a.PostId=b.PostId  where d.PlineNm  LIKE '%" + KeyValue + "%' and a.Enabled='1' and  c.Enabled='1' and d.Enabled='1' and SignalSource = 'Andon' order by AcqTm desc;");
        //    }
        //    else if (Condition == "PostNm")
        //    {
        //        //精确到岗位
        //        strSql.Append(@"select a.RecId,a.SignalSource,a.SignalDetail,a.AcqTm,a.CurValue,b.PostNm,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PostBase b on a.PostId=b.PostId left join BBdbR_WcBase c on b.WcId =c.WcId left join BBdbR_PlineBase d on c.PlineId=d.PlineId where b.PostNm LIKE '%" + KeyValue + "%' and a.Enabled='1' and b.Enabled='1' and c.Enabled='1' and d.Enabled='1' and SignalSource = 'Andon' order by AcqTm desc;");

        //    }
        //    else if (Condition == "AcqTm")
        //    {
        //        //精确到岗位
        //        //strSql.Append(@"select a.RecId,a.SignalSource,a.AcqTm,a.CurValue,b.PostNm,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PostBase b on a.PostId=b.PostId left join BBdbR_WcBase c on b.WcId =c.WcId left join BBdbR_PlineBase d on c.PlineId=d.PlineId where convert(varchar(10),a.[AcqTm],120)  = '" + KeyValue + "'and a.Enabled='1' and b.Enabled='1' and c.Enabled='1' and d.Enabled='1' and SignalSource = 'Andon' order by AcqTm desc;");
        //        //精确到工位
        //        strSql.Append(@"select a.RecId,a.SignalSource,a.SignalDetail,a.AcqTm,a.CurValue,a.PostId,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PlineBase d on a.PlineId=d.PlineId left join BBdbR_WcBase c on a.WcId =c.WcId left join BBdbR_PostBase b on a.PostId=b.PostId  where convert(varchar(10),a.[AcqTm],120)  = '" + KeyValue + "'and a.Enabled='1'  and c.Enabled='1' and d.Enabled='1' and SignalSource = 'Andon' order by AcqTm desc;");
        //    }

        //    dt = Repository().FindTableBySql(strSql.ToString(), false);

        //    return dt;
        //}
        //#endregion

        #region  编辑通过主键获取实体
        public DataTable GetMatInfor(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select a.RecId,a.SignalSource,a.SignalDetail,a.AcqTm,a.CurValue,a.PostId,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PlineBase d on a.PlineId=d.PlineId left join BBdbR_WcBase c on a.WcId =c.WcId left join BBdbR_PostBase b on a.PostId=b.PostId  where a.RecId='" + KeyValue+ "=' 001'and a.Enabled='1'  and c.Enabled='1' and d.Enabled='1';");
            return Repository().FindTableBySql(strSql.ToString(), false);
        }
        #endregion


        #region 补录编辑方法
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(BBdbR_DataAcqPro entity) //===复制时需要修改===
        {
            return Repository().Update(entity); //将修改后的实体跟新到数据库中
        }
        #endregion


        #region 查找计划
        /// <summary>
        /// 查找计划
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable GetTodayPlan()
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            strSql.Append(@"select sum(case when  PlanStatus=2 and Enabled='1' and  convert(varchar(10),[RecieveTm],120)='" + today+ "'  then 1 else 0 end) as dayActual,sum(case when  Enabled='1' and  convert(varchar(10),[RecieveTm],120)='" + today+ "' then 1 else 0 end) as dayPlan from P_ProducePlan_Pro;");
            dt = Repository().FindTableBySql(strSql.ToString(), false);
            return dt;
        }
        #endregion


        #region 查找班组
        /// <summary>
        /// 查找班组
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable GetTodayClass()
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            strSql.Append(@"select d.TeamNm from BPdb_Dt a left join BFacR_ClassConfig b on a.ClassId =b.ClassId left join BFacR_ClassTeamConfig c on b.ShiftId = c.ShiftId left join BFacR_TeamBase d on c.TeamId = d.TeamID where convert(varchar(10),a.[Tm],120) = '" + today + "' and a.Enabled='1'and b.Enabled='1'and c.Enabled='1'and d.Enabled='1';");
            dt = Repository().FindTableBySql(strSql.ToString(), false);
            return dt;
        }
        #endregion

        #region 查找停线次数
        /// <summary>
        /// 查找停线次数
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable GetTodayNumber()
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            strSql.Append(@"select sum(case when  SignalSource = 'Andon' and Enabled='1' and  convert(varchar(10),[AcqTm],120)= '"+today+ "'  then 1 else 0 end) as AndonStopNumber,sum(case when  SignalSource = 'FAS' and Enabled='1' and convert(varchar(10),[AcqTm],120)= '"+today+"'  then 1 else 0 end) as AutoStopNumber from BBdbR_DataAcqPro");
            dt = Repository().FindTableBySql(strSql.ToString(), false);
            return dt;
        }
        #endregion

        #region 根据产线/工位/岗位Id查询Name
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable GetAndonNameById(string PlineId, string WcId, string PostId)
        {
            if (PostId!="")
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select b.PostNm,c.WcNm,d.PlineNm from  BBdbR_PostBase b left join BBdbR_WcBase c on b.WcId =c.WcId left join BBdbR_PlineBase d on c.PlineId=d.PlineId where d.PlineId = '" + PlineId + "'and b.PostId = '" + PostId + "' and b.Enabled='1'and c.Enabled='1'and d.Enabled='1';");
                dt = Repository().FindTableBySql(strSql.ToString(), false);
                return dt;
            }
            else 
            {
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select c.WcNm,d.PlineNm from  BBdbR_WcBase c  left join BBdbR_PlineBase d on c.PlineId=d.PlineId where d.PlineId = '" + PlineId + "'and c.WcId = '" + WcId + "' and  c.Enabled='1'and d.Enabled='1';");
                dt = Repository().FindTableBySql(strSql.ToString(), false);
                return dt;
            }
            
        }
        #endregion


        #region 根据工位Id查询具体岗位Name
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable GetAndonPostNameByWcId(string WcId)
        {
            
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();
            strSql.Append(@"select PostNm,PostId from  BBdbR_PostBase where WcId = '" + WcId + "' and  Enabled='1'");
            dt = Repository().FindTableBySql(strSql.ToString(), false);
            return dt;
            

        }
        #endregion
        

        #region 根据产线Id查询产线Name
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable GetAndonPlineNameById(string PlineId)
        {
           
                StringBuilder strSql = new StringBuilder();
                DataTable dt = new DataTable();
                strSql.Append(@"select PlineNm from  BBdbR_PlineBase  where PlineId = '" + PlineId + "' and Enabled='1';");
                dt = Repository().FindTableBySql(strSql.ToString(), false);
                return dt;
           

        }
        #endregion

        #region 根据设备IP查询产线Id和名称
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable GetPlineIdByIp(string IP)
        {

            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();
            strSql.Append(@"select a.ClassId as PlineId,b.PlineCd,b.PlineNm from  BBdbR_DvcBase a  left join BBdbR_PlineBase b on a.ClassId=b.PlineId where IPAddr = '" + IP + "' and a.Enabled='1'and b.Enabled='1';");
            dt = Repository().FindTableBySql(strSql.ToString(), false);
            return dt;


        }
        #endregion

        #region 2.高级检索
        public DataTable GridPageStockTryList(List<ConditionAndKey> condition, DateTime? starttime, DateTime? endtime, ref JqGridParam jqgridparam)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select a.RecId,a.PlineId,b.PlineNm,a.WcId,c.WcNm,a.PostId,d.PostNm,a.KepServerId,a.KepServerNm,a.KepServerNd,a.KepServerSource,a.IsMonitoring,a.MonitoringRate,a.CntlAddrDsc,a.SignalTyp,a.SignalSource,a.SignalDetail,a.CurValue,a.AcqTm,a.HandleSts,a.EndTm,a.HandleTm,a.HandlerCode,a.HandlerName,a.ExceptionType,a.ExceptionDes,a.HandleResult,a.Enabled,a.MdfTm,a.MdfCd,a.MdfNm,a.Rem,a.RsvFld1,a.RsvFld2 from " + tableName + " a left join BBdbR_PlineBase b on a.PlineId=b.PlineId left join BBdbR_WcBase c on a.WcId=C.WcId left join BBdbR_PostBase d on a.PostId=d.PostId  where a.Enabled='1' and a.AcqTm<='" + endtime + "' and a.AcqTm>='" + starttime + "' ");
            foreach (var item in condition)
            {
                switch (item.Condition)
                {
                    case "PlineId":
                        sql.Append(" and a.PlineId");
                        break;
                    case "WcId":
                        sql.Append(" and a.WcId");
                        break;
                    case "PostId":
                        sql.Append(" and a.PostId");
                        break;
                    case "SignalTyp":
                        sql.Append(" and a.SignalTyp");
                        break;
                    case "SignalSource":
                        sql.Append(" and a.SignalSource");
                        break;
                    case "SignalDetail":
                        sql.Append(" and a.SignalDetail");
                        break;
                    case "CurValue":
                        sql.Append(" and a.CurValue");
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
            return Repository().FindTableBySql(sql.ToString(),false);
        }
        #endregion

        #region 新增方法
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
        public int Insert(BBdbR_DataAcqPro entity) //===复制时需要修改===
        {
            return Repository().Insert(entity);
        }
        #endregion
    }
}