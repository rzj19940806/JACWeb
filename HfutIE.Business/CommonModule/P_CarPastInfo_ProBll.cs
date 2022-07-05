//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

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
    /// 车身过点过程表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.10.21 20:14</date>
    /// </author>
    /// </summary>
    public class P_CarPastInfo_ProBll : RepositoryFactory<P_CarPastInfo_Pro>
    {
        public DataTable getAllCarInfo()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 5 VIN,CarType,CarColor1,AviCd,AviNm,CONVERT(nvarchar,PastTime,20) PastTime from P_CarPastRecordInfo where VIN!='' order by PastTime desc ");
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            return dt;
        }

        public DataTable getAllCarInfo2()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 5 VIN,AviCd,CONVERT(nvarchar,PastTime,20) PastTime from P_CarPastRecordInfo where VIN!='' order by PastTime desc ");
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            return dt;
        }

        public DataTable getAlarmInfo()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 5 DvcCd,DvcNm,CONVERT(nvarchar,AlarmStartTime,20) AlarmStartTime from E_EquipmentAlarmRecord a join BBdbR_DvcBase b on a.DvcId=b.DvcId order by AlarmStartTime ");
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            return dt;
        }

        public DataTable getAllCarQuene()
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select top 18 AVICd,ToAVICd,VIN,CarQuene from P_LineProductionQueue_Pro with(nolock) where AVICd = 'TRIM-1' " +
            //    "UNION " +
            //    "select top 18 AVICd, ToAVICd, VIN, CarQuene from P_LineProductionQueue_Pro with(nolock) where AVICd = 'TRIM-2' " +
            //    "UNION " +
            //    "select top 16 AVICd, ToAVICd, VIN, CarQuene from P_LineProductionQueue_Pro with(nolock) where AVICd = 'CHASSIS-1' " +
            //    "UNION " +
            //    "select top 16 AVICd, ToAVICd, VIN, CarQuene from P_LineProductionQueue_Pro with(nolock) where AVICd = 'CHASSIS-2' " +
            //    "UNION " +
            //    "select top 25 AVICd, ToAVICd, VIN, CarQuene from P_LineProductionQueue_Pro with(nolock) where AVICd = 'FIT-1' " +
            //    "UNION " +
            //    "select top 10 AVICd, ToAVICd, VIN, CarQuene from P_LineProductionQueue_Pro with(nolock) where AVICd = 'FIT-2' " +
            //    "UNION " +
            //    "select top 10 AVICd, ToAVICd, VIN, CarQuene from P_LineProductionQueue_Pro with(nolock) where AVICd = 'OK-1' " +
            //    "UNION " +
            //    "select top 12 AVICd, ToAVICd, VIN, CarQuene from P_LineProductionQueue_Pro with(nolock) where AVICd = 'SUBMIT-1' " +
            //    "order by AVICd, CarQuene");
            strSql.Append(@"select * from(
select 
case 
when PlineCd = 'Line-10' then 'TRIM-1'
when PlineCd = 'Line-11' then 'TRIM-2'
when PlineCd = 'Line-12' then 'CHASSIS-1'
when PlineCd = 'Line-13' then 'CHASSIS-2'
when PlineCd = 'Line-14' then 'FIT-1'
end AVICd,
case 
when PlineCd = 'Line-10' then 'TRIM-2'
when PlineCd = 'Line-11' then 'CHASSIS-1'
when PlineCd = 'Line-12' then 'CHASSIS-2'
when PlineCd = 'Line-13' then 'FIT-1'
when PlineCd = 'Line-14' then 'FIT-2'
end ToAVICd
, PlineCd, TA.PlineNm, ROW_NUMBER()over(partition by PlineCd order by CodeValue) No, TA.WcCd, TA.WcNm, TA.VIN VIN1, TB.VIN VIN, TA.CodeValue from(select ROW_NUMBER()over(partition by A.PlineCd order by CodeValue) No, FitEnCodeInfoId, VIN, CodeValue, PlineNm, WcCd, WcNm, A.PlineId, C.StartPoint, C.EndPoint from P_FitEnCodeInfo A with(nolock) join BBdbR_PlineBase B with(nolock) on A.PlineId = B.PlineId left join BBdbR_WcBase C with(nolock) on CodeValue > C.StartPoint and CodeValue < C.EndPoint and A.PlineId = C.PlineId) TA join(select ROW_NUMBER()over(partition by PlineCd order by CarQuene desc) No, VIN, PlineCd, CarQuene, PlineId from P_LineProductionQueue_Pro with (nolock) where PlineCd in ('Line-10', 'Line-11', 'Line-12', 'Line-13', 'Line-14') and AVICd<>'FIT-2') TB on TA.No = TB.No and TA.PlineId = TB.PlineId where CodeValue >= 0) A where(
                    PlineCd= 'Line-10' and No<= '18' OR
                      PlineCd = 'Line-11' and No<= '18' OR
                         PlineCd = 'Line-12' and No<= '16' OR
                            PlineCd = 'Line-13' and No<= '16' OR
                               PlineCd = 'Line-14' and No<= '24')

UNION
                select top 10 AVICd, ToAVICd,PlineCd,PlineNm,ROW_NUMBER()over(partition by PlineCd order by CarQuene) No,WcCd,WcNm, VIN1 = VIN,VIN,CodeValue = 0  from P_LineProductionQueue_Pro with(nolock)
                                                                                                                                                                  where AVICd = 'FIT-2'
                UNION
                select top 10 AVICd, ToAVICd, PlineCd,PlineNm,ROW_NUMBER()over(partition by PlineCd order by CarQuene) No,WcCd,WcNm, VIN1 = VIN,VIN,CodeValue = 0  from P_LineProductionQueue_Pro with(nolock)
                                                                                                                                                                   where AVICd = 'OK-1'
                UNION
                select top 12 AVICd, ToAVICd, PlineCd,PlineNm,ROW_NUMBER()over(partition by PlineCd order by CarQuene) No,WcCd,WcNm, VIN1 = VIN,VIN,CodeValue = 0  from P_LineProductionQueue_Pro with(nolock)
                                                                                                                                                                   where AVICd = 'SUBMIT-1'
                order by PlineCd,AVICd,No");



            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            return dt;
        }


        public DataTable getAllDataAcq()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select WcCd from (select WcId from (select * from BBdbR_DataAcqPro_Today where WcId!='') " +
                "as tab where EndTm IS NULL group by WcId) as wc join BBdbR_WcBase wcinfo on wc.WcId = wcinfo.WcId");
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            return dt;
        }

        public DataTable getAllOutPut()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select '日实际' 名称,count(distinct(VIN)) 数值 from P_CarPastRecordInfo where AviCd = 'TRIM-1' and datediff(day, CreTm, getdate()) = 0 and VIN != '9999' union select * from(SELECT top 1 '日排产' 名称, DayPlanNo 数值 FROM S_PlanBoardInfo where datediff(day, Date, getdate()) = 0 order by CreTm DESC) as tab");
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            return dt;
        }

    }
}