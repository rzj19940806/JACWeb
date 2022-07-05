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
    /// Andon���ݲɼ����̱�
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.11 15:17</date>
    /// </author>
    /// </summary>
    public class BBdbR_DataAcqProBll : RepositoryFactory<BBdbR_DataAcqPro>
    {
        #region ȫ�ֱ���������
        string tableName = "BBdbR_DataAcqPro";
        #endregion

        #region ����ʱ�����������ѯAndon��¼��Ϣ
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable GetAndonList(string KeyValue,string PlineId)
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();
            if (KeyValue == "")
            {
                //����IPȷ�����������ߵ�pad������չʾĬ��Andon����
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

        #region ����
        public DataTable GetPageListByCondition(string PlineNm, string WcNm, string SignalSource, string SignalDetail, string CurValue, string HandleSts, string HandlerCode, string ExceptionType, string TimeStart, string TimeEnd, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = $"select PlineNm,WcNm,SignalSource,SignalDetail,CurValue,AcqTm,HandleSts,EndTm,HandleTm,a.HandlerCode,a.HandlerName,a.ExceptionType,a.ExceptionDes,HandleResult,a.Enabled,a.MdfTm,a.MdfNm,a.Rem from BBdbR_DataAcqPro a join BBdbR_PlineBase b on a.PlineId = b.PlineId and a.Enabled = '1' and PlineNm like '%{PlineNm}%' and SignalSource like '%{SignalSource}%' and SignalDetail like '%{SignalDetail}%' and CurValue like '%{CurValue}%' and HandleSts like '%{HandleSts}%' and (HandlerCode like '%{HandlerCode}%' or '{HandlerCode}'='') and (ExceptionType like '%{ExceptionType}%' or '{ExceptionType}'='') and AcqTm between '{TimeStart}' and '{TimeEnd} 23:59:59' left join BBdbR_WcBase c on a.WcId = C.WcId where WcNm like '%{WcNm}%' order by {jqgridparam.sidx} {jqgridparam.sord}";
            return Repository().FindTableBySql(sql.ToString(), false);

        }
        #endregion

        #region ���ݲ�ѯ������ѯAndon��¼��Ϣ
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable GetAndonListByCondition(string KeyValue,string SearchTime)
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();
            if (KeyValue == "ȫ��")
            {
                if (SearchTime=="")
                {
                    //��ȷ����λ
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
                    //��ȷ����λ
                    strSql.Append(@"select a.RecId,a.SignalSource,a.SignalDetail,a.AcqTm,a.CurValue,a.PostId,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PlineBase d on a.PlineId=d.PlineId left join BBdbR_WcBase c on a.WcId =c.WcId left join BBdbR_PostBase b on a.PostId=b.PostId  where d.PlineNm  LIKE '%" + KeyValue + "%'  and DateDiff(dd,AcqTm,getdate())=0 and a.Enabled='1' and  c.Enabled='1' and d.Enabled='1' and SignalSource = 'Andon' order by AcqTm desc;");
                }
                else
                {
                    //��ȷ����λ
                    strSql.Append(@"select a.RecId,a.SignalSource,a.SignalDetail,a.AcqTm,a.CurValue,a.PostId,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PlineBase d on a.PlineId=d.PlineId left join BBdbR_WcBase c on a.WcId =c.WcId left join BBdbR_PostBase b on a.PostId=b.PostId  where d.PlineNm  LIKE '%" + KeyValue + "%' and convert(varchar(10),a.[AcqTm],120)  = '" + SearchTime + "' and a.Enabled='1' and  c.Enabled='1' and d.Enabled='1' and SignalSource = 'Andon' order by AcqTm desc;");
                }
            }
            dt = Repository().FindTableBySql(strSql.ToString(), false);
            return dt;
        }
        #endregion

        //#region ���ݲ�ѯ������ѯAndon��¼��Ϣ-text
        ///// <summary>
        ///// ���ϲ�ѯ��չʾҳ����
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns></returns>
        //public DataTable GetAndonListByConditiontext(string KeyValue, string Condition)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    DataTable dt = new DataTable();
        //    if (Condition == "SignalSource")
        //    {
        //        //��ȷ����λ
        //        //strSql.Append(@"select a.RecId,a.SignalSource,a.AcqTm,a.CurValue,b.PostNm,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PostBase b on a.PostId=b.PostId left join BBdbR_WcBase c on b.WcId =c.WcId left join BBdbR_PlineBase d on c.PlineId=d.PlineId where a.SignalSource LIKE '%" + KeyValue + "%' and a.Enabled='1' and b.Enabled='1' and c.Enabled='1' and d.Enabled='1' and SignalSource = 'Andon' order by AcqTm desc;");
        //        //��ȷ����λ
        //        strSql.Append(@"select a.RecId,a.SignalSource,a.SignalDetail,a.AcqTm,a.CurValue,a.PostId,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PlineBase d on a.PlineId=d.PlineId left join BBdbR_WcBase c on a.WcId =c.WcId left join BBdbR_PostBase b on a.PostId=b.PostId  where a.SignalSource LIKE '%" + KeyValue + "%' and a.Enabled='1'   and SignalSource = 'Andon' order by AcqTm desc;");
        //    }
        //    else if (Condition == "PlineNm")
        //    {
        //        //��ȷ����λ
        //        //strSql.Append(@"select a.RecId,a.SignalSource,a.AcqTm,a.CurValue,b.PostNm,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PostBase b on a.PostId=b.PostId left join BBdbR_WcBase c on b.WcId =c.WcId left join BBdbR_PlineBase d on c.PlineId=d.PlineId where d.PlineNm  LIKE '%" + KeyValue + "%' and a.Enabled='1' and b.Enabled='1' and c.Enabled='1' and d.Enabled='1' and SignalSource = 'Andon' order by AcqTm desc;");
        //        //��ȷ����λ
        //        strSql.Append(@"select a.RecId,a.SignalSource,a.SignalDetail,a.AcqTm,a.CurValue,a.PostId,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PlineBase d on a.PlineId=d.PlineId left join BBdbR_WcBase c on a.WcId =c.WcId left join BBdbR_PostBase b on a.PostId=b.PostId  where d.PlineNm  LIKE '%" + KeyValue + "%' and a.Enabled='1' and  c.Enabled='1' and d.Enabled='1' and SignalSource = 'Andon' order by AcqTm desc;");
        //    }
        //    else if (Condition == "PostNm")
        //    {
        //        //��ȷ����λ
        //        strSql.Append(@"select a.RecId,a.SignalSource,a.SignalDetail,a.AcqTm,a.CurValue,b.PostNm,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PostBase b on a.PostId=b.PostId left join BBdbR_WcBase c on b.WcId =c.WcId left join BBdbR_PlineBase d on c.PlineId=d.PlineId where b.PostNm LIKE '%" + KeyValue + "%' and a.Enabled='1' and b.Enabled='1' and c.Enabled='1' and d.Enabled='1' and SignalSource = 'Andon' order by AcqTm desc;");

        //    }
        //    else if (Condition == "AcqTm")
        //    {
        //        //��ȷ����λ
        //        //strSql.Append(@"select a.RecId,a.SignalSource,a.AcqTm,a.CurValue,b.PostNm,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PostBase b on a.PostId=b.PostId left join BBdbR_WcBase c on b.WcId =c.WcId left join BBdbR_PlineBase d on c.PlineId=d.PlineId where convert(varchar(10),a.[AcqTm],120)  = '" + KeyValue + "'and a.Enabled='1' and b.Enabled='1' and c.Enabled='1' and d.Enabled='1' and SignalSource = 'Andon' order by AcqTm desc;");
        //        //��ȷ����λ
        //        strSql.Append(@"select a.RecId,a.SignalSource,a.SignalDetail,a.AcqTm,a.CurValue,a.PostId,c.WcNm,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PlineBase d on a.PlineId=d.PlineId left join BBdbR_WcBase c on a.WcId =c.WcId left join BBdbR_PostBase b on a.PostId=b.PostId  where convert(varchar(10),a.[AcqTm],120)  = '" + KeyValue + "'and a.Enabled='1'  and c.Enabled='1' and d.Enabled='1' and SignalSource = 'Andon' order by AcqTm desc;");
        //    }

        //    dt = Repository().FindTableBySql(strSql.ToString(), false);

        //    return dt;
        //}
        //#endregion

        #region  �༭ͨ��������ȡʵ��
        public DataTable GetMatInfor(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select a.RecId,a.SignalSource,a.SignalDetail,a.AcqTm,a.CurValue,a.PostId,d.PlineNm from BBdbR_DataAcqPro a left join BBdbR_PlineBase d on a.PlineId=d.PlineId left join BBdbR_WcBase c on a.WcId =c.WcId left join BBdbR_PostBase b on a.PostId=b.PostId  where a.RecId='" + KeyValue+ "=' 001'and a.Enabled='1'  and c.Enabled='1' and d.Enabled='1';");
            return Repository().FindTableBySql(strSql.ToString(), false);
        }
        #endregion


        #region ��¼�༭����
        //���޸ĺ��ʵ����µ����ݿ���
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Update(BBdbR_DataAcqPro entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Update(entity); //���޸ĺ��ʵ����µ����ݿ���
        }
        #endregion


        #region ���Ҽƻ�
        /// <summary>
        /// ���Ҽƻ�
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


        #region ���Ұ���
        /// <summary>
        /// ���Ұ���
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

        #region ����ͣ�ߴ���
        /// <summary>
        /// ����ͣ�ߴ���
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

        #region ���ݲ���/��λ/��λId��ѯName
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
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


        #region ���ݹ�λId��ѯ�����λName
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
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
        

        #region ���ݲ���Id��ѯ����Name
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
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

        #region �����豸IP��ѯ����Id������
        /// <summary>
        /// ���ϲ�ѯ��չʾҳ����
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

        #region 2.�߼�����
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

        #region ��������
        //entityʵ���е������Ǵ�ҳ���д����ģ������û���д������
        //entityʵ����ֻ�в����ֶ���ֵ����Ϊҳ����ֻ�ṩ�������ֶθ�ֵ
        //��ҳ������д��������ʵ�壨entity���ķ�ʽ���������ݿ�
        //���У�ʵ���е�IsAvailable�ֶ�û����ҳ������д
        //IsAvailable�ֶε�����������ɾ���������ݿ��е�ĳһ�����ݵ�IsAvailable�ֶε��ֶ�ֵΪtrue��ʾ�����ݴ���
        //�ֶ�ֵΪfalse��ʾ�����ݱ�ɾ��
        //��ɾ�����ݿ��е�ĳһ������ʱֻҪ�޸�IsAvailable�ֶε��ֶ�ֵΪfalse������ɾ����������
        //������ʱ��ʵ���IsAvailable�ֶε�ֵ�޸�Ϊtrue
        //����ֵΪ1������0
        //1��ʾ�����ɹ���0��ʾ����ʧ��
        public int Insert(BBdbR_DataAcqPro entity) //===����ʱ��Ҫ�޸�===
        {
            return Repository().Insert(entity);
        }
        #endregion
    }
}