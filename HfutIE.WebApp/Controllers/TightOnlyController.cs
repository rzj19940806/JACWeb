using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HfutIE.WebApp.Controllers
{
    public class TightOnlyController : Controller
    {
        Q_KeyPartsBll q_KeyParts = new Q_KeyPartsBll();
        IDatabase database = DataFactory.Database();
        Tight_OnlineBll Tight_Online = new Tight_OnlineBll();
        public ActionResult Only()
        {
            var IP = NetHelper.GetIPAddress();
            //IP = "10.138.13.48";
            if (IP == "10.138.13.90") { return View("Tg_FDJ"); }
            else if (IP == "10.138.13.94"|| IP == "10.138.13.89") { return View("Tg_FZ"); }
            else { return View("Tg_Only"); }

        }
        /// <summary>
        /// 获取基础信息，主要包括工厂模型，账户信息
        /// </summary>
        public  ActionResult GetStaticInfo()
        {
            int code = 1;
            string msg = "";
            //基础静态信息---工厂模型、账户信息
            Dictionary<string, string> BaseInfoProps = new Dictionary<string, string>();
            try
            {
                //2.获取工厂模型
                //2.1获取IP地址
                var IP = NetHelper.GetIPAddress();
                //IP = "10.138.13.48";
                //2.2根据IP地址获取设备-工位---公司
                //获取工位--字典中使用classid代替
                q_KeyParts.GetRowValue("BBdbR_DvcBase", "ClassId,DvcCatg,DvcTyp,DvcLocatn", "IPAddr", IP, ref BaseInfoProps);
                //获取工位
                q_KeyParts.GetRowValue("BBdbR_WcBase", "WcCd,WcNm,PlineId", "WcId", BaseInfoProps["ClassId"], ref BaseInfoProps);
                //获取产线
                q_KeyParts.GetRowValue("BBdbR_PlineBase", "PlineCd,PlineNm,EndPoint,WorkSectionId", "PlineId", BaseInfoProps["PlineId"], ref BaseInfoProps);
                //获取工艺段
                q_KeyParts.GetRowValue("BBdbR_WorkSectionBase", "WorkSectionCd,WorkSectionNm,WorkshopId", "WorkSectionId", BaseInfoProps["WorkSectionId"], ref BaseInfoProps);
                //获取车间
                q_KeyParts.GetRowValue("BBdbR_WorkshopBase", "WorkshopCd,WorkshopNm,FacId", "WorkshopId", BaseInfoProps["WorkshopId"], ref BaseInfoProps);
                //获取工厂
                q_KeyParts.GetRowValue("BBdbR_FacBase", "FacCd,FacNm,CompanyId", "FacId", BaseInfoProps["FacId"], ref BaseInfoProps);
                //获取公司
                q_KeyParts.GetRowValue("BBdbR_CompanyBase", "CompanyCd,CompanyNm", "CompanyId", BaseInfoProps["CompanyId"], ref BaseInfoProps);
                //修正
                BaseInfoProps.Add("WcId", BaseInfoProps["ClassId"]);
                BaseInfoProps.Remove("ClassId");

            }
            catch (Exception ex)
            {
                code = -1;
                msg += "获取工厂模型信息时发生错误:" + ex.Message;
            }
            try
            {
                //1.获取账户基础信息
                //1.1获取用户名与人员主键
                string UserId = ManageProvider.Provider.Current().UserId;
                //1.2根据用户主键查找人员主键编号名称
                if (UserId != "System")
                {
                    q_KeyParts.GetRowValue("Base_User", "UserId StfId,Code StfCd,RealName StfNm", "UserId", UserId, ref BaseInfoProps);
                }
                else
                {
                    BaseInfoProps.Add("StfId", "System");
                    BaseInfoProps.Add("StfCd", "System");
                    BaseInfoProps.Add("StfNm", "超级管理员");
                }
            }
            catch (Exception ex)
            {
                code = -1;
                msg += "获取人员信息时发生错误:" + ex.Message;
            }

            return Content(new { code, msg, props = BaseInfoProps }.ToJson());
        }
        /// <summary>
        /// 获取基础信息，主要包括工厂模型，账户信息，条码解析规则,拧紧配置规则
        /// </summary>
        public  ActionResult GetStaticInfoFZ()
        {
            int code = 1;
            string msg = "";
            //基础静态信息---工厂模型、账户信息
            Dictionary<string, string> BaseInfoProps = new Dictionary<string, string>();
            //解析规则
            DataTable BarCode = new DataTable();
            List<string> strs = new List<string>();
            try
            {
                //2.获取工厂模型
                //2.1获取IP地址
                var IP = NetHelper.GetIPAddress();
                //IP = "10.138.13.48";
                //2.2根据IP地址获取设备-工位---公司
                //获取工位--字典中使用classid代替
                q_KeyParts.GetRowValue("BBdbR_DvcBase", "ClassId,DvcCatg,DvcTyp,DvcLocatn", "IPAddr", IP, ref BaseInfoProps);
                //获取工位
                q_KeyParts.GetRowValue("BBdbR_WcBase", "WcCd,WcNm,PlineId", "WcId", BaseInfoProps["ClassId"], ref BaseInfoProps);
                //获取产线
                q_KeyParts.GetRowValue("BBdbR_PlineBase", "PlineCd,PlineNm,EndPoint,WorkSectionId", "PlineId", BaseInfoProps["PlineId"], ref BaseInfoProps);
                //获取工艺段
                q_KeyParts.GetRowValue("BBdbR_WorkSectionBase", "WorkSectionCd,WorkSectionNm,WorkshopId", "WorkSectionId", BaseInfoProps["WorkSectionId"], ref BaseInfoProps);
                //获取车间
                q_KeyParts.GetRowValue("BBdbR_WorkshopBase", "WorkshopCd,WorkshopNm,FacId", "WorkshopId", BaseInfoProps["WorkshopId"], ref BaseInfoProps);
                //获取工厂
                q_KeyParts.GetRowValue("BBdbR_FacBase", "FacCd,FacNm,CompanyId", "FacId", BaseInfoProps["FacId"], ref BaseInfoProps);
                //获取公司
                q_KeyParts.GetRowValue("BBdbR_CompanyBase", "CompanyCd,CompanyNm", "CompanyId", BaseInfoProps["CompanyId"], ref BaseInfoProps);
                //修正
                BaseInfoProps.Add("WcId", BaseInfoProps["ClassId"]);
                BaseInfoProps.Remove("ClassId");
            }
            catch (Exception ex)
            {
                code = -1;
                msg += "获取工厂模型信息时发生错误:" + ex.Message;
            }
            try
            {
                //1.获取账户基础信息
                //1.1获取用户名与人员主键
                string UserId = ManageProvider.Provider.Current().UserId;
                //1.2根据用户主键查找人员主键编号名称
                if (UserId != "System")
                {
                    q_KeyParts.GetRowValue("Base_User", "UserId StfId,Code StfCd,RealName StfNm", "UserId", UserId, ref BaseInfoProps);
                }
                else
                {
                    BaseInfoProps.Add("StfId", "System");
                    BaseInfoProps.Add("StfCd", "System");
                    BaseInfoProps.Add("StfNm", "超级管理员");
                }
            }
            catch (Exception ex)
            {
                code = -1;
                msg += "获取人员信息时发生错误:" + ex.Message;
            }
            try
            {
                //3.获取解码规则相关线下
                BarCode = q_KeyParts.GetBarCode(BaseInfoProps["WcId"]);
            }
            catch (Exception ex)
            {
                code = -1;
                msg += "获取解析规则时发生错误:" + ex.Message;
            }
            try
            {
                //获取需要拧紧的图号---还需要考虑发动机分装待修改
                string sql = " SELECT DISTINCT Code FROM dbo.TightConfigView WHERE WcId='198' AND Type='图号'";
                DataTable dt = database.FindTableBySql(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strs.Add(dt.Rows[i][0].ToString());
                }
            }
            catch (Exception ex)
            {
                code = -1;
                msg += "获取拧紧规则时发生错误:" + ex.Message;
            }
            return Content(new { code, msg, props = BaseInfoProps, BarCode, strs }.ToJson());
        }
        //发动机获取静态信息
        public ActionResult GetStaticInfoFDJ(string IP)
        {
            int code = 1;
            string msg = "";
            //基础静态信息---工厂模型、账户信息
            Dictionary<string, string> BaseInfoProps = new Dictionary<string, string>();
            try
            {
                //2.获取工厂模型
                //2.2根据IP地址获取设备-工位---公司
                //获取工位--字典中使用classid代替
                q_KeyParts.GetRowValue("BBdbR_DvcBase", "ClassId,DvcCatg,DvcTyp,DvcLocatn", "IPAddr", IP, ref BaseInfoProps);
                //获取工位
                q_KeyParts.GetRowValue("BBdbR_WcBase", "WcCd,WcNm,PlineId", "WcId", BaseInfoProps["ClassId"], ref BaseInfoProps);
                //获取产线
                q_KeyParts.GetRowValue("BBdbR_PlineBase", "PlineCd,PlineNm,EndPoint,WorkSectionId", "PlineId", BaseInfoProps["PlineId"], ref BaseInfoProps);
                //获取工艺段
                q_KeyParts.GetRowValue("BBdbR_WorkSectionBase", "WorkSectionCd,WorkSectionNm,WorkshopId", "WorkSectionId", BaseInfoProps["WorkSectionId"], ref BaseInfoProps);
                //获取车间
                q_KeyParts.GetRowValue("BBdbR_WorkshopBase", "WorkshopCd,WorkshopNm,FacId", "WorkshopId", BaseInfoProps["WorkshopId"], ref BaseInfoProps);
                //获取工厂
                q_KeyParts.GetRowValue("BBdbR_FacBase", "FacCd,FacNm,CompanyId", "FacId", BaseInfoProps["FacId"], ref BaseInfoProps);
                //获取公司
                q_KeyParts.GetRowValue("BBdbR_CompanyBase", "CompanyCd,CompanyNm", "CompanyId", BaseInfoProps["CompanyId"], ref BaseInfoProps);
                //修正
                BaseInfoProps.Add("WcId", BaseInfoProps["ClassId"]);
                BaseInfoProps.Remove("ClassId");

            }
            catch (Exception ex)
            {
                code = -1;
                msg += "获取工厂模型信息时发生错误:" + ex.Message;
            }
            try
            {
                //1.获取账户基础信息
                //1.1获取用户名与人员主键
                string UserId = ManageProvider.Provider.Current().UserId;
                //1.2根据用户主键查找人员主键编号名称
                if (UserId != "System")
                {
                    q_KeyParts.GetRowValue("Base_User", "UserId StfId,Code StfCd,RealName StfNm", "UserId", UserId, ref BaseInfoProps);
                }
                else
                {
                    BaseInfoProps.Add("StfId", "System");
                    BaseInfoProps.Add("StfCd", "System");
                    BaseInfoProps.Add("StfNm", "超级管理员");
                }
            }
            catch (Exception ex)
            {
                code = -1;
                msg += "获取人员信息时发生错误:" + ex.Message;
            }

            return Content(new { code, msg, props = BaseInfoProps }.ToJson());
        }
        //VIN码入站
        public ActionResult VinIn(string VIN,string WcId,string ShowNm,bool IsScan)
        {
            DataTable ResultDt;
            int Code = 1;
            string Msg;
            string Show;
            try
            {
                string str = "SELECT Vin FROM dbo.Tg_WcIOStatus WHERE WcId='" + WcId + "' AND Vin IS NOT NULL";
                DataTable dt = DbHelperSQL.OpenTable(str);
                if(dt.Rows.Count > 0&&!IsScan)
                {
                    string OldVin = dt.Rows[0][0].ToString();
                    //加载数据库数据--包括是否Pass
                    ResultDt = OldVinIn(OldVin, WcId);
                    ResultDt=DistinctGw(ResultDt, ShowNm);
                    //补充拧紧数据
                    GetOldData(ResultDt, WcId, OldVin);
                    Msg = "自动加载未完成任务:【工位：" + ShowNm + "】【VIN：" + OldVin + "】";
                    Show = OldVin;
                }
                else
                {
                    //以输入VIN码入站
                    NewVinIn(VIN, WcId, ShowNm, out ResultDt, out Code, out Msg);
                    Show = VIN;
                }
                return Content(new { Code, ResultDt, Msg,Show }.ToJson());
            }
            catch (Exception ex)
            {
                Code = -1;
                Msg = ex.Message;
                return Content(new { Code, Msg }.ToJson());
            }
        }

        //VIN码入站
        public ActionResult BarIn(string VIN, string WcId, string ShowNm, string Pic)
        {
            DataTable ResultDt;
            int Code = 1;
            string Msg;
            string Show;
            try
            {
                string str = "SELECT Vin FROM dbo.Tg_WcIOStatus WHERE WcId='" + WcId + "' AND Vin IS NOT NULL";
                DataTable dt = DbHelperSQL.OpenTable(str);
                if (dt.Rows.Count > 0)
                {
                    string OldVin = dt.Rows[0][0].ToString();
                    if (VIN == "8888")
                    {
                        ResultDt = OldVinIn(OldVin, WcId);
                        Msg = "自动加载未完成任务:【工位：" + ShowNm + "】【条码：" + OldVin + "】";
                        Show = OldVin;
                        GetOldData(ResultDt, WcId,OldVin);
                    }
                    else
                    {
                        NewBarIn(VIN, Pic, WcId, ShowNm, out ResultDt, out Code, out Msg);
                        Show = VIN;
                    }
                }
                else
                {
                    NewBarIn(VIN, Pic, WcId, ShowNm, out ResultDt, out Code, out Msg);
                    Show = VIN;
                }
                return Content(new { Code, ResultDt, Msg,Show }.ToJson());
            }
            catch (Exception ex)
            {
                Code = -1;
                Msg = ex.Message;
                return Content(new { Code, Msg }.ToJson());
            }
        }
        //现有VIN,条码入站
        public DataTable OldVinIn(string VIN,string WcId)
        {
            string str = "SELECT IP,Port,Code,MatCode,Cate,IsPass FROM dbo.Tg_InEnableDetail WHERE WcId='" + WcId + "' AND Vin='" + VIN + "'";
            DataTable dt = database.FindTableBySql(str);
            DataTable Result = new DataTable();
            if (dt.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                //有任务则数据存在
                for (int i=0; i < dt.Rows.Count; i++)
                {
                    string IP = dt.Rows[i]["IP"].ToString();
                    int Port = Convert.ToInt32(dt.Rows[i]["Port"]);
                    string Cate = dt.Rows[i]["Cate"].ToString();
                    string Code = dt.Rows[i]["Code"].ToString();
                    sb.Append(@"select a.WcJobCd,JobCd,TorqueSL,TorqueUL,TorqueLL,AngleUL,AngleLL,Num,Ord,Num as T_sort,'NG'is_OK,Position,ControllerID,ControllerPort,a.Code,Type,RsvFld1,'--' Torque,'--' Angle,d.IsPass,t.G_max
                                from TightConfigView a join
                                (select count(*) as G_max,WcJobCd from Tg_JobTorqueConfig group by WcJobCd) t on a.WcJobCd =t.WcJobCd 
                                JOIN dbo.Tg_InEnableDetail d ON d.IP = a.ControllerID AND d.Port=a.ControllerPort ");
                    StringBuilder Where = new StringBuilder();
                    Where.Append($" WHERE ControllerID='{IP}' and ControllerPort='{Port}' and a.Code='{Code}' and Type='{Cate}' and a.WcId={WcId}");
                    if (Cate == "车型+图号")
                    {
                        string MatCode = dt.Rows[i]["MatCode"].ToString();
                        Where.Append($" AND RsvFld1 = '{MatCode}'");
                    }
                    sb.Append(Where.ToString());
                    if(i!= dt.Rows.Count - 1)
                    {
                        sb.Append(" UNION ALL ");
                    }
                }
                Result = DbHelperSQL.OpenTable(sb.ToString());
                for (int i = 0; i < Result.Rows.Count; i++)
                {
                    int tg_num = Convert.ToInt16(Result.Rows[i]["Num"].ToString());
                    if (tg_num > 1)
                    {
                        Result.Rows.Add(Result.Rows[i]["WcJobCd"].ToString(), Result.Rows[i]["JobCd"].ToString(), Result.Rows[i]["TorqueSL"].ToString(), Result.Rows[i]["TorqueUL"].ToString(), Result.Rows[i]["TorqueLL"].ToString(), Result.Rows[i]["AngleUL"].ToString(), Result.Rows[i]["AngleLL"].ToString(), tg_num - 1, Result.Rows[i]["Ord"].ToString(), Result.Rows[i]["T_sort"].ToString(), "NG", Result.Rows[i]["Position"].ToString(), Result.Rows[i]["ControllerID"].ToString(), Convert.ToInt32(Result.Rows[i]["ControllerPort"]), Result.Rows[i]["Code"].ToString(), Result.Rows[i]["Type"].ToString(), Result.Rows[i]["RsvFld1"].ToString(), "--", "--", "0", Result.Rows[i]["G_max"].ToString());
                    }
                }
                Result.DefaultView.Sort = "WcJobCd,Ord asc,Num asc";
                Result = Result.DefaultView.ToTable();
            }
            return Result;
        }
        public void GetOldData(DataTable Result, string WcId,string VIN)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"select Serial,RsvFld2,Torque,Angle,ControllerID,ControllerPort from Tg_TightenDataPro where WcId='{WcId}' and IsOK=1 and");
            if (WcId == "198" || WcId == "199")
            {
                //线下分装不存VIN号
                sb.Append($" RsvFld3='{VIN}'");
            }
            else
            {
                sb.Append($" Vin='{VIN}'");
            }
            DataTable dt = database.FindTableBySql(sb.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int Num = Convert.ToInt32(dt.Rows[i]["Serial"]);
                int Ord = Convert.ToInt32(dt.Rows[i]["RsvFld2"]);
                decimal Torque = Convert.ToDecimal(dt.Rows[i]["Torque"]);
                decimal Angle = Convert.ToDecimal(dt.Rows[i]["Angle"]);
                string IP = dt.Rows[i]["ControllerID"].ToString();
                int Port = Convert.ToInt32(dt.Rows[i]["ControllerPort"]);
                string str9 = "ControllerID='" + IP + "' AND ControllerPort='" + Port + "' AND Ord='" + Ord + "' AND Num='" + Num + "'";
                DataRow[] row = Result.Select(str9);
                foreach (DataRow item in row)
                {
                    item["is_OK"] = "OK";
                    item["Torque"] = Torque;
                    item["Angle"] = Angle;
                }
            }
        }
        public DataTable DistinctGw(DataTable dt,string ShowNm)
        {
            string str_sql = "";
            if (ShowNm != null && ShowNm.Contains("左"))
            {
                str_sql += "Position like'%左%'";
            }
            else if (ShowNm != null && ShowNm.Contains("右"))
            {
                str_sql += "Position like'%右%'";
            }
            DataRow[] datas = dt.Select(str_sql, "Ord");
            //精确到左右岗位配置
            DataTable GwDt = dt.Clone();
            foreach (var item in datas)
            {
                GwDt.Rows.Add(item.ItemArray);
            }
            return GwDt;
        }
        //新VIN入站
        public void NewVinIn(string VIN, string WcId,string ShowNm,out DataTable ResultDt,out int Code,out string Msg)
        {
            //先根据产品编号查找
            int result = CheckValid(VIN, WcId, ShowNm, out ResultDt);
            if (result == 100)
            {
                Code = 1;
                Msg = "拧紧入站成功:【工位：" + ShowNm + "】【VIN：" + VIN + "】";
                //调整扩充行
                for (int i = 0; i < ResultDt.Rows.Count; i++)
                {
                    int tg_num = Convert.ToInt16(ResultDt.Rows[i]["Num"].ToString());
                    if (tg_num > 1)
                    {
                        ResultDt.Rows.Add(ResultDt.Rows[i]["WcJobCd"].ToString(), ResultDt.Rows[i]["JobCd"].ToString(), ResultDt.Rows[i]["TorqueSL"].ToString(), ResultDt.Rows[i]["TorqueUL"].ToString(), ResultDt.Rows[i]["TorqueLL"].ToString(), ResultDt.Rows[i]["AngleUL"].ToString(), ResultDt.Rows[i]["AngleLL"].ToString(), tg_num - 1, ResultDt.Rows[i]["Ord"].ToString(), ResultDt.Rows[i]["T_sort"].ToString(), "NG", ResultDt.Rows[i]["Position"].ToString(), ResultDt.Rows[i]["ControllerID"].ToString(), Convert.ToInt32(ResultDt.Rows[i]["ControllerPort"]), ResultDt.Rows[i]["Code"].ToString(), ResultDt.Rows[i]["Type"].ToString(), ResultDt.Rows[i]["RsvFld1"].ToString(), "--", "--", "0", ResultDt.Rows[i]["G_max"].ToString());
                    }
                }
                //重新排序
                ResultDt.DefaultView.Sort = "WcJobCd,Ord asc,Num asc";
                ResultDt = ResultDt.DefaultView.ToTable();
            }
            else if (result == 2 || result == 0)
            {
                Code = 2;//界面不处理--正常
                Msg = "无拧紧任务:【工位：" + ShowNm + "】【VIN：" + VIN + "】";
                ResultDt = null;
            }
            else
            {
                Code = 0;
                switch (result)
                {
                    case 1:
                        Msg = "不在队列中";
                        break;
                    case 3:
                        Msg = "相同使能类型命中多条配置";
                        break;
                    default:
                        Msg = "";
                        break;
                }
                ResultDt = null;
            }
        }
        //新VIN入站
        public void NewBarIn(string VIN,string Pic, string WcId, string ShowNm, out DataTable ResultDt, out int Code, out string Msg)
        {
            try
            {
                if (VIN == "8888")
                {
                    Code = 2;//界面不处理--正常
                    Msg = "无拧紧任务:【工位：" + ShowNm + "】";
                    ResultDt = null;
                    return;
                }
                int Rst = 0;
                StringBuilder sb = new StringBuilder();
                sb.Append(@"SELECT  a.WcJobCd,JobCd,TorqueSL,TorqueUL,TorqueLL,AngleUL,AngleLL,Num,Ord,Num as T_sort,'NG'is_OK,Position,ControllerID,ControllerPort,Code,Type,a.RsvFld1,'--' Torque,'--' Angle  , NULL IsPass,t.G_max
                        FROM TightConfigView a  join
                        (select count(*) as G_max,WcJobCd from Tg_JobTorqueConfig group by WcJobCd) t on a.WcJobCd =t.WcJobCd where Type='图号' and a.Code='" + Pic + "' and a.WcId='" + WcId + "' ORDER BY a.Ord");
                DataTable result = DbHelperSQL.OpenTable(sb.ToString());
                if (result.Rows.Count > 0)
                {
                    bool isOk = true;
                    DataTable Configtable = result.DefaultView.ToTable(true, new string[] { "ControllerID", "ControllerPort", "JobCd"});
                    //工位根据根据IP、Port存在多条Job配置
                    for (int i = 0; i < Configtable.Rows.Count; i++)
                    {
                        string IP = Configtable.Rows[i]["ControllerID"].ToString();
                        int Port = Convert.ToInt32(Configtable.Rows[i]["ControllerPort"]);
                        string str2 = "ControllerID='" + IP + "' and ControllerPort='" + Port + "'";
                        if (Configtable.Select(str2).Length > 1)
                        {
                            isOk = false;
                            break;
                        }
                    }
                    if (isOk)
                    {

                        Rst = 1;
                    }
                    else
                    {
                        //多条配置
                        Rst = 0;
                    }
                }
                else
                {
                    //配置丢失
                    Rst = -1;
                }
                if (Rst > 0)
                {
                    Code = 1;
                    Msg = "拧紧入站成功:【工位：" + ShowNm + "】【条码：" + VIN + "】";
                    //调整扩充行
                    for (int i = 0; i < result.Rows.Count; i++)
                    {
                        int rownum = result.Rows.Count;
                        int tg_num = Convert.ToInt16(result.Rows[i]["Num"].ToString());
                        if (tg_num > 1)
                        {
                            result.Rows.Add(result.Rows[i]["WcJobCd"].ToString(), result.Rows[i]["JobCd"].ToString(), result.Rows[i]["TorqueSL"].ToString(), result.Rows[i]["TorqueUL"].ToString(), result.Rows[i]["TorqueLL"].ToString(), result.Rows[i]["AngleUL"].ToString(), result.Rows[i]["AngleLL"].ToString(), tg_num - 1, result.Rows[i]["Ord"].ToString(), result.Rows[i]["T_sort"].ToString(), "NG", result.Rows[i]["Position"].ToString(), result.Rows[i]["ControllerID"].ToString(), Convert.ToInt32(result.Rows[i]["ControllerPort"]), result.Rows[i]["Code"].ToString(), result.Rows[i]["Type"].ToString(), result.Rows[i]["RsvFld1"].ToString(), "--", "--", "0", result.Rows[i]["G_max"].ToString());
                        }
                    }
                    //重新排序
                    result.DefaultView.Sort = "WcJobCd,Ord asc,Num asc";
                    ResultDt = result.DefaultView.ToTable();
                }
                else if (Rst == 0)
                {
                    Msg = "相同使能类型命中多条配置";
                    Code = 0;
                    ResultDt = null;
                }
                else
                {
                    Msg = "拧紧配置错误";
                    Code = 0;
                    ResultDt = null;
                }
            }
            catch (Exception ex)
            {
                Code = -1;
                Msg = ex.Message;
                ResultDt = null;
            }
        }

        //新VIN校验合理性
        public int CheckValid(string VIN, string WcId, string ShowNm, out DataTable ShowDt)
        {
            //0-空车;1-VIN不在队列中;2-没有拧紧任务;3-同类型命中多条配置;100-成功
            //1.检查VIN本身有效性
            if (VIN == "9999")
            {
                ShowDt = null;
                return 0;
            }
            else
            {
                //2.检查配置是否存在
                string str = $"select MatCd,CarType from P_LineProductionQueue_Pro where VIN like '%{VIN}'";
                DataTable dt = database.FindTableBySql(str);
                if (dt.Rows.Count == 0)
                {
                    ShowDt = null;
                    return 1;
                }
                else
                {
                    string ProCd = dt.Rows[0]["MatCd"].ToString();
                    string CarType = (string)dt.Rows[0][1];
                    //综合查找
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"select TightConfigView.WcJobCd,JobCd,TorqueSL,TorqueUL,TorqueLL,AngleUL,AngleLL,Num,Ord,Num as T_sort,'NG'is_OK,Position,ControllerID,ControllerPort,Code,Type,RsvFld1,'--' Torque,'--' Angle ,NULL IsPass,t.G_max
                                  from TightConfigView join
                                  (select count(*) as G_max,WcJobCd from Tg_JobTorqueConfig group by WcJobCd) t on TightConfigView.WcJobCd =t.WcJobCd WHERE Type='产品' and CHARINDEX(Code,'" + ProCd + "')>0 and WcId='" + WcId + "' UNION ALL ");
                    sb.Append(@" SELECT  a.WcJobCd,JobCd,TorqueSL,TorqueUL,TorqueLL,AngleUL,AngleLL,Num,Ord,Num as T_sort,'NG'is_OK,Position,ControllerID,ControllerPort,Code,Type,a.RsvFld1,'--' Torque,'--' Angle  , NULL IsPass,t.G_max
                                  FROM TightConfigView a join produceBom b on a.Code=b.MatCd join
                                  (select count(*) as G_max,WcJobCd from Tg_JobTorqueConfig group by WcJobCd) t on a.WcJobCd =t.WcJobCd where Type='图号' and b.ProductCd='" + ProCd + "' and a.WcId='" + WcId + "' UNION ALL ");
                    sb.Append(@" select TightConfigView.WcJobCd,JobCd,TorqueSL,TorqueUL,TorqueLL,AngleUL,AngleLL,Num,Ord,Num as T_sort,'NG'is_OK,Position,ControllerID,ControllerPort,Code,Type,RsvFld1,'--' Torque,'--' Angle  , NULL IsPass,t.G_max
                                  from TightConfigView join
                                  (select count(*) as G_max,WcJobCd from Tg_JobTorqueConfig group by WcJobCd) t on TightConfigView.WcJobCd =t.WcJobCd WHERE Type='车型' AND Code='" + CarType + "' and WcId='" + WcId + "'  UNION ALL ");
                    sb.Append(@" select c.WcJobCd,JobCd,TorqueSL,TorqueUL,TorqueLL,AngleUL,AngleLL,Num,Ord,Num as T_sort,'NG'is_OK,Position,ControllerID,ControllerPort,Code,Type,c.RsvFld1,'--' Torque,'--' Angle  , NULL IsPass,t.G_max
                                  from TightConfigView c JOIN dbo.produceBom d ON c.RsvFld1=d.MatCd join
                                  (select count(*) as G_max,WcJobCd from Tg_JobTorqueConfig group by WcJobCd) t on c.WcJobCd =t.WcJobCd WHERE Type='车型+图号' and d.ProductCd='" + ProCd + "' AND c.Code='" + CarType + "' and c.WcId='" + WcId + "' ");
                    //初筛选配置数据
                    DataTable IniDt = DbHelperSQL.OpenTable(sb.ToString());
                    string str_sql = "";
                    if (ShowNm != null && ShowNm.Contains("左"))
                    {
                        str_sql += "Position like'%左%'";
                    }
                    else if (ShowNm != null && ShowNm.Contains("右"))
                    {
                        str_sql += "Position like'%右%'";
                    }
                    DataRow[] datas = IniDt.Select(str_sql, "Ord");
                    //精确到左右岗位配置
                    DataTable GwDt = IniDt.Clone();
                    foreach (var item in datas)
                    {
                        GwDt.Rows.Add(item.ItemArray);
                    }
                    if (GwDt.Rows.Count == 0)
                    {
                        ShowDt = null;
                        return 2;
                    }
                    //3.检查配置是否合理
                    //根据IP、Port分类后总条数作为理论逻辑出站总数
                    bool isOk = true;
                    //不区分不同Ord
                    DataTable PrioDt = GwDt.DefaultView.ToTable(true, new string[] { "ControllerID", "ControllerPort", "JobCd", "Code", "Type", "RsvFld1" });
                    //筛选出优先级最高的配置
                    PrioDt = CheckPriority(PrioDt);
                    //工位根据根据IP、Port存在多条Job号不一样的配置
                    DataTable CheckDt = PrioDt.DefaultView.ToTable(true, new string[] { "ControllerID", "ControllerPort", "JobCd" });
                    for (int i = 0; i < CheckDt.Rows.Count; i++)
                    {
                        string IP = PrioDt.Rows[i]["ControllerID"].ToString();
                        int Port = Convert.ToInt32(PrioDt.Rows[i]["ControllerPort"]);
                        string str2 = $"ControllerID='{IP}' and ControllerPort='{Port}'";
                        if (CheckDt.Select(str2).Length > 1)
                        {
                            isOk = false;
                            break;
                        }
                    }
                    if (isOk)
                    {
                        ShowDt = IniDt.Clone();
                        for (int i = 0; i < PrioDt.Rows.Count; i++)
                        {
                            string IP = PrioDt.Rows[i]["ControllerID"].ToString();
                            int Port = Convert.ToInt32(PrioDt.Rows[i]["ControllerPort"]);
                            int JobCd = Convert.ToInt32(PrioDt.Rows[i]["JobCd"]);
                            string Code = PrioDt.Rows[i]["Code"].ToString();
                            string Type = PrioDt.Rows[i]["Type"].ToString();
                            StringBuilder sb1 = new StringBuilder();
                            sb1.Append($"ControllerID='{IP}' and ControllerPort='{Port}' and JobCd='{JobCd}' and Code='{Code}' and Type='{Type}'");
                            if (Type == "车型+图号")
                            {
                                string MatCd = PrioDt.Rows[i]["RsvFld1"].ToString();
                                sb1.Append($" and RsvFld1='{MatCd}'");
                            }
                            DataRow[] dataRows = GwDt.Select(sb1.ToString(), "Ord");
                            foreach (DataRow row in dataRows)
                            {
                                ShowDt.Rows.Add(row.ItemArray);
                            }
                        }
                        return 100;
                    }
                    else
                    {
                        ShowDt = null;
                        return 3;
                    }
                }
            }
        }
        //优先级检查
        public DataTable CheckPriority(DataTable sourceDt)
        {
            if (sourceDt.Rows.Count == 1)
            {
                return sourceDt;
            }
            else
            {
                DataTable resultDt = sourceDt.Clone();
                DataTable distinctDt = sourceDt.DefaultView.ToTable(true, new string[] { "ControllerID", "ControllerPort" }); ;
                for (int i = 0; i < distinctDt.Rows.Count; i++)
                {
                    string IP = distinctDt.Rows[i]["ControllerID"].ToString();
                    int Port = Convert.ToInt32(distinctDt.Rows[i]["ControllerPort"]);
                    DataRow[] dr = sourceDt.Select("");
                    string str = "ControllerID='" + IP + "' and ControllerPort='" + Port + "' and Type='产品'";
                    string str1 = "ControllerID='" + IP + "' and ControllerPort='" + Port + "' and Type='车型+图号'";
                    string str2 = "ControllerID='" + IP + "' and ControllerPort='" + Port + "' and Type='车型'";
                    string str3 = "ControllerID='" + IP + "' and ControllerPort='" + Port + "' and Type='图号'";
                    DataRow[] row = sourceDt.Select(str);
                    DataRow[] row1 = sourceDt.Select(str1);
                    DataRow[] row2 = sourceDt.Select(str2);
                    DataRow[] row3 = sourceDt.Select(str3);
                    if (row.Length > 0)
                    {
                        foreach (var item in row)
                        {
                            resultDt.Rows.Add(item.ItemArray);
                        }
                    }
                    else
                    {
                        if (row1.Length > 0)
                        {
                            foreach (var item in row1)
                            {
                                resultDt.Rows.Add(item.ItemArray);
                            }
                        }
                        else
                        {
                            if (row2.Length > 0)
                            {
                                foreach (var item in row2)
                                {
                                    resultDt.Rows.Add(item.ItemArray);
                                }
                            }
                            else
                            {
                                foreach (var item in row3)
                                {
                                    resultDt.Rows.Add(item.ItemArray);
                                }
                            }
                        }
                    }
                }
                return resultDt;
            }
        }

        //加载车辆信息和指导图片
        public ActionResult ShowInfo(string VIN, string WcId)
        {
            DataTable vinInfo = q_KeyParts.GetVINInfoByStf(VIN);
            DataTable Product = q_KeyParts.GetProduct(vinInfo.Rows[0]["ProductMatCd"].ToString());
            string str = $"select * from produceBom with(nolock) where ProductId='{Product.Rows[0]["MatId"]}' and WcId='{WcId}' and MatImg IS NOT NULL";
            DataTable PartImgs = database.FindTableBySql(str);
            return Content(new { vinInfo, Product, PartImgs }.ToJson());
        }

        public ActionResult PassBindTg(Tg_ByPass_Pro KeyByPass, string WcJobCd, string Position, string account = null, string password = null)
        {
            int code = 1;//-2.程序异常错误，0.其他完成作业，1.正常执行，2.无PASS权限且次数使用完，3.账号异常或无权限
            string msg = "";
            try
            {
                int passRestTimes = 0;
                if (account == null)
                {
                    //pass次数校核
                    string passCount = $"select IIF((select count(*) from Base_User U join Base_ObjectUserRelation OU ON OU.UserId=U.UserId and U.UserId='System' join Base_ButtonPermission BP on OU.ObjectId = BP.ObjectId join Base_Button B on BP.ModuleButtonId=B.ButtonId  and B.FullName='PASS')= 0,(select isnull((select top 1 isnull(rsvfld2, 3) from BBdbR_WcBase where WcCd = '{KeyByPass.WcCd}' and Enabled = 1),0)-count(*) from Tg_ByPass_Pro where DATEDIFF(DD, Datetime, getdate()) = 0 and WcCd = '{KeyByPass.WcCd}' and Enabled = 1),999)";
                    passRestTimes = Convert.ToInt32(DataFactory.Database().FindTableBySql(passCount).Rows[0][0]);
                    if (passRestTimes <= 0)
                    {
                        code = 2;
                        msg = "当前登录账号无PASS权限且当前工位无剩余PASS次数";
                    }
                }
                else
                {
                    Base_User entity = DataFactory.Database().FindEntity<Base_User>("Account", account);
                    if (entity != null && entity.Enabled == "1" && entity.Password == Md5Helper.MD5(DESEncrypt.Encrypt(password.ToLower(), entity.Secretkey).ToLower(), 32).ToLower())
                    {
                        string sql = $"select U.RealName,B.FullName buttonName,M.FullName moduleName from Base_User U join Base_ObjectUserRelation OU ON OU.UserId=U.UserId and U.UserId='{entity.UserId}' join Base_ButtonPermission BP on OU.ObjectId = BP.ObjectId join Base_Button B on BP.ModuleButtonId = B.ButtonId and B.Enabled = 1 and B.FullName = 'PASSTG' join Base_Module M on B.ModuleId = M.ModuleId and M.FullName = '拧紧'";
                        if (DataFactory.Database().FindTableBySql(sql).Rows.Count == 0)
                        {
                            code = 3;
                            msg = "账号无PASS权限";
                        }
                        else
                        {
                            KeyByPass.StfId = entity.UserId;
                            KeyByPass.StfCd = entity.Code;
                            KeyByPass.StfNm = entity.RealName;
                            passRestTimes = 999;
                        }
                    }
                    else
                    {
                        code = 3;
                        msg = "账号密码错误";
                    }
                }
                if (code == 1)
                {
                    ArrayList list = new ArrayList();
                    string WcId = KeyByPass.WcId;
                    string PlineId = KeyByPass.PlineId;
                    string str = "SELECT ControllerID,ControllerPort,JobCd FROM dbo.Tg_WcJobConfig WHERE WcJobCd='" + WcJobCd + "'";
                    DataTable dt = database.FindTableBySql(str);
                    string IP = "";
                    int Port = 0;
                    int JobCode = 0;
                    if (dt.Rows.Count > 0)
                    {
                        IP = dt.Rows[0]["ControllerID"].ToString();
                        Port = Convert.ToInt32(dt.Rows[0]["ControllerPort"]);
                        JobCode = Convert.ToInt32(dt.Rows[0]["JobCd"]);
                    }
                    KeyByPass.MatId = WcJobCd;
                    KeyByPass.MatCd = JobCode.ToString();
                    KeyByPass.MatNm = IP;
                    KeyByPass.SupplierId = Port.ToString();
                    KeyByPass.SupplierCd = Position;
                    string str1 = @"SELECT PosNum,LOutNum,POutStatus,Vin,LOutStatus FROM dbo.Tg_WcIOStatus WHERE WcId='" + WcId + "' AND Vin IS NOT NULL";
                    DataTable dt1 = database.FindTableBySql(str1);
                    if (dt1.Rows.Count > 0)
                    {
                        int PosNum = (int)dt1.Rows[0][0];//理论逻辑出站总数
                        int LOutNum = (int)dt1.Rows[0][1];//当前满足的逻辑出站数
                        int POutStatus = (int)dt1.Rows[0][2];//当前满足的逻辑出站数
                        string Vin = dt1.Rows[0]["Vin"].ToString();
                        string str10 = @"SELECT Code,MatCode,Cate,IsPass FROM dbo.Tg_InEnableDetail WHERE IP='" + IP + "'AND Port='" + Port + "' AND Vin='" + Vin + "'";
                        DataTable dt10 = DbHelperSQL.OpenTable(str10);
                        if (dt10.Rows.Count > 0)
                        {
                            string str11 = "UPDATE dbo.Tg_InEnableDetail SET IsPass='Pass' WHERE IP='" + IP + "' AND Port='" + Port + "' AND  Vin='" + Vin + "'";
                            list.Add(str11);
                        }
                        //完全出站
                        if (PosNum == LOutNum + 1)
                        {
                            string str0 = "SELECT Status FROM dbo.Tg_WcControl WHERE WcId='" + WcId + "'";
                            DataTable dt0 = DbHelperSQL.OpenTable(str0);
                            string Status = dt0.Rows[0][0].ToString();
                            //非主线工位直接出站
                            //恢复逻辑
                            if (POutStatus == 1 || WcId == "118"|| WcId == "119" || WcId == "198" || WcId == "199")
                            {
                                string str9 = @"UPDATE dbo.Tg_WcIOStatus SET InStatus=0,LOutStatus=0,POutStatus=0,
                                        PosNum=NULL,LOutNum=NULL,Vin=NULL WHERE WcId='" + WcId + "'";
                                list.Add(str9);
                                //清除入站逻辑详情表
                                string str12 = "DELETE dbo.Tg_InEnableDetail WHERE WcId='" + WcId + "' AND Vin='" + Vin + "'";
                                list.Add(str12);
                                //转档
                                string str13 = @"insert into Tg_TightenDataDoc select * from Tg_TightenDataPro where WcId='" + WcId + "'";
                                list.Add(str13);
                                string str14 = "delete Tg_TightenDataPro where  WcId='" + WcId + "'";
                                list.Add(str14);
                                //更新当前工位停线状态
                                string str8 = @"UPDATE dbo.Tg_LineStopRec SET Status=0,ReStartTime=GETDATE() WHERE 
                                                PlineId='" + PlineId + "' AND WcId='" + WcId + "' AND Status='"+ Status + "'";
                                list.Add(str8);
                            }
                            else
                            {
                                string str6 = "update Tg_WcIOStatus set LOutStatus=1,LOutNum=PosNum where WcId='" + WcId + "'";
                                list.Add(str6);
                            }
                        }
                        //部分出站
                        else
                        {
                            string str7 = "update Tg_WcIOStatus set LOutNum+=1 where WcId='" + WcId + "'";
                            list.Add(str7);
                        }
                        DbHelperSQL.ExecuteSqlTran(list);
                        KeyByPass.Create();
                        int num = Tight_Online.TgPartPass(KeyByPass);
                        if (num <= 0)
                        {
                            code = 0;
                            msg = "已完成作业，当前拧紧作业已取消";
                        }
                        else
                        {
                            msg = $"剩余拧紧PASS次数：{passRestTimes}";
                        }
                    }
                    else
                    {
                        code = -2;
                        msg = "无法Pass该Job";
                    }
                }
            }
            catch (Exception ex)
            {
                code = -2;
                msg = "Pass过程发生错误:" + ex.Message;
            }
            return Content(new { code, msg }.ToJson());
        }
      
        public ActionResult NowTaskFDJ(string WcId)
        {
            string str = "SELECT Vin FROM dbo.Tg_WcIOStatus WHERE WcId='"+WcId+"' AND InStatus=1";
            DataTable dt = database.FindTableBySql(str);
            if (dt.Rows.Count == 1)
            {
                string VIN = dt.Rows[0][0].ToString();
                return Content(new { VIN }.ToJson());
            }
            else
            {
                return Content("9999");
            }
        }
    }
}