using HfutIE.DataAccess;
using HfutIE.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace HfutIE.WebApp
{
    /// <summary>
    /// E_SCREW 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class E_SCREW : System.Web.Services.WebService
    {

        IDatabase db = DataFactory.Database();

        //P_FitEnCodeInfo
        static int sqe ;
        static string curVin;
        static DataTable carTable = null;
        [WebMethod]
        public string GetCarInfo()
        {
            carTable = db.FindTableBySql($@"select P.PlineId,W.WcId,W.WcCd,IIF(F.VIN is null,'00000000000000000',F.VIN) VIN,IIF(F.CodeValue is not null,'FI','00') Status,IIF(F.CodeValue>W.StopPoint,'1','0') WcStop,'' NJstatus
                from BBdbR_WcBase W with(nolock)
                join BBdbR_PlineBase P with(nolock) on W.PlineId=P.PlineId and P.PlineTyp ='生产主线'
                left join P_FitEnCodeInfo F with(nolock) on F.PlineId=W.PlineId and F.CodeValue>=W.StartPoint and F.CodeValue<=W.EndPoint and F.VIN<>'9999' and F.VIN<>'####'
                order by P.Sort,W.Sort");
            if (++sqe == 1000)
            {
                sqe = 1;
            }
            string message = sqe.ToString().PadLeft(3, '0');//报文顺序号1-3
            message += "FI";//报文类型4-5
            message += carTable.Rows.Count.ToString().PadLeft(3, '0');//工位长度6-8
            message = message.PadRight(50, ' ');//填充空格

            for (int i = 0; i < carTable.Rows.Count; i++)
            {
                DataRow carInfo = carTable.Rows[i];
                message += carInfo["Status"];
                message += carInfo["WcStop"];
                message += carInfo["VIN"];
                message += carInfo["WcCd"];
                message = message.PadRight(50 * (i + 2), ' ');//填充空格
            }
            return message + "#";
        }

        [WebMethod]
        public void PushCarInfo(string carInfoStr)
        {

            for (int i = 0; i < Convert.ToInt32(carInfoStr.Substring(5, 3)); i++)
            {

                var s = Convert.ToInt32(carInfoStr.Substring(5, 3));
                DataRow[] carTab = carTable.Select($"WcCd='{carInfoStr.Substring(50 * (i + 1) + 20, 4)}'");
                if (carTab.Count() == 0)
                {
                    break;
                }
                DataRow carTabInfo = carTab[0];
                if (carTabInfo["WcStop"].ToString() == "1" && carInfoStr.Substring(50 * (i + 1) + 2, 1) != "0" && carInfoStr.Substring(50 * (i + 1) + 2, 1) != "1")
                {
                    DbHelper.ExecuteNonQuery(CommandType.Text, $"insert Q_ScanStatus_Timely(PlineId,WcId,VIN,MatId,ScanStatus,RemainStationNo,MatNo) select '{carTabInfo["PlineId"]}', '{carTabInfo["WcId"]}', '{carTabInfo["VIN"]}', '拧紧设备', '未扫码', '0', '1' where not exists(select 1 from Q_ScanStatus_Timely where WcId = '{carTabInfo["WcId"]}' and MatId = '拧紧设备')");
                }
                else if (carTabInfo["WcStop"].ToString() == "1" && (carInfoStr.Substring(50 * (i + 1) + 2, 1) == "0" || carInfoStr.Substring(50 * (i + 1) + 2, 1) == "1"))
                {
                    DbHelper.ExecuteNonQuery(CommandType.Text, $"update Q_ScanStatus_Timely set ScanStatus='ByPass' where WcId='{carTabInfo["WcId"]}' and MatId='拧紧设备'");
                }
            }
        }
    }

}
