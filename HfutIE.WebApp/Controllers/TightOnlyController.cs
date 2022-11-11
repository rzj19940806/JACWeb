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
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Controllers
{
    public class TightOnlyController : Controller
    {
        Q_KeyPartsBll q_KeyParts = new Q_KeyPartsBll();
        IDatabase database = DataFactory.Database();
        Tight_OnlineBll Tight_Online = new Tight_OnlineBll();
        // GET: TightOnly
        public ActionResult Only()
        {
            var IP = NetHelper.GetIPAddress();
            IP = "10.138.13.90";
            if (IP == "10.138.13.89"|| IP == "10.138.13.90") { return View("Tg_FDJ"); }
            else if (IP == "10.138.13.94") { return View("Tg_FZ"); }
            else { return View("Tg_Only"); }
            
        }
        /// <summary>
        /// 获取基础信息，主要包括工厂模型，账户信息
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult GetStaticInfo()
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
                IP = "10.138.13.90";
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
                string StfNm = ManageProvider.Provider.Current().Account;
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

            return Content(new { code, msg, props = BaseInfoProps}.ToJson());
        }
        //VIN号入站
        public ActionResult Initdt_Tighten(string inOrOut, string VIN, string DvcTyp, string WcNm, string WcId)
        {
            DataTable dt_Tighten = new DataTable();
            int code = 1;//显示dt表
            string msg = "";
            string Pass = "";
            try
            {
                if (inOrOut == "in")//入站时读取该工位的配置信息
                {
                    //先根据产品编号查找
                    DataTable dt_temp = new DataTable();
                    int result = CheckValid(VIN, WcId, DvcTyp, out dt_temp);
                    if (result == 100 || result == 4 ||result==5)
                    {
                        msg = "拧紧入站成功:【工位：" + WcNm + "】【VIN：" + VIN + "】";
                        //调整扩充行
                        for (int i = 0; i < dt_temp.Rows.Count; i++)
                        {
                            int rownum = dt_temp.Rows.Count;
                            int tg_num = Convert.ToInt16(dt_temp.Rows[i]["Num"].ToString());
                            if (tg_num > 1)
                            {
                                dt_temp.Rows.Add(dt_temp.Rows[i]["WcJobCd"].ToString(), dt_temp.Rows[i]["JobCd"].ToString(), dt_temp.Rows[i]["TorqueSL"].ToString(), dt_temp.Rows[i]["TorqueUL"].ToString(), dt_temp.Rows[i]["TorqueLL"].ToString(), dt_temp.Rows[i]["AngleUL"].ToString(), dt_temp.Rows[i]["AngleLL"].ToString(), tg_num - 1, dt_temp.Rows[i]["Ord"].ToString(), dt_temp.Rows[i]["T_sort"].ToString(), "NG", dt_temp.Rows[i]["Position"].ToString(), dt_temp.Rows[i]["ControllerID"].ToString(), Convert.ToInt32(dt_temp.Rows[i]["ControllerPort"]), dt_temp.Rows[i]["Code"].ToString(), dt_temp.Rows[i]["Type"].ToString(), dt_temp.Rows[i]["RsvFld1"].ToString(), "--", "--", VIN, dt_temp.Rows[i]["G_max"].ToString());
                            }
                        }
                        //重新排序
                        dt_temp.DefaultView.Sort = "WcJobCd,Ord asc,Num asc";
                        dt_Tighten = dt_temp.DefaultView.ToTable();
                        if (result ==4)//非全新进站从过程表取已经拧紧和Pass数据
                        {
                            string str = @"select Serial,RsvFld2,Torque,Angle,ControllerID,ControllerPort from Tg_TightenDataPro where WcId='" + WcId + "'";
                            DataTable dt = database.FindTableBySql(str);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int Num = Convert.ToInt32(dt.Rows[i]["Serial"]);
                                int Ord = Convert.ToInt32(dt.Rows[i]["RsvFld2"]);
                                decimal Torque = Convert.ToDecimal(dt.Rows[i]["Torque"]);
                                decimal Angle = Convert.ToDecimal(dt.Rows[i]["Angle"]);
                                string IP = dt.Rows[i]["ControllerID"].ToString();
                                int Port = Convert.ToInt32(dt.Rows[i]["ControllerPort"]);
                                string str9 = "ControllerID='" + IP + "' AND ControllerPort='" + Port + "' AND Ord='" + Ord + "' AND Num='" + Num + "'";
                                DataRow[] row = dt_Tighten.Select(str9);
                                foreach (DataRow item in row)
                                {
                                    item["is_OK"] = "OK";
                                    item["Torque"] = Torque;
                                    item["Angle"] = Angle;
                                }
                            }
                            string str11 = @"SELECT IP,Port,Code,MatCode,Cate,IsPass FROM dbo.Tg_InEnableDetail WHERE WcId='" + WcId + "' AND Vin='" + VIN + "' AND IsPass='Pass'";
                            DataTable dt11 = database.FindTableBySql(str11);
                            if (dt11.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt11.Rows.Count; i++)
                                {
                                    string IP = dt11.Rows[i]["IP"].ToString();
                                    int Port = Convert.ToInt32(dt11.Rows[i]["Port"]);
                                    string Cate = dt11.Rows[i]["Cate"].ToString();
                                    string Code = dt11.Rows[i]["Code"].ToString();
                                    string str12 = "";
                                    if (Cate == "车型+图号")
                                    {
                                        string MatCode = dt11.Rows[i]["MatCode"].ToString();
                                        str12 = "SELECT WcJobCd FROM dbo.TightConfigView WHERE ControllerID='" + IP + "' AND ControllerPort='" + Port + "' AND WcId='" + WcId + "' AND Code='" + Code + "' AND Type='" + Cate + "' AND RsvFld1='" + MatCode + "'";
                                    }
                                    else
                                    {
                                        str12 = "SELECT WcJobCd FROM dbo.TightConfigView WHERE ControllerID='" + IP + "' AND ControllerPort='" + Port + "' AND WcId='" + WcId + "' AND Code='" + Code + "' AND Type='" + Cate + "'";
                                    }
                                    DataTable dt12 = database.FindTableBySql(str12);
                                    string WcJob = dt12.Rows[0][0].ToString();
                                    if (i == dt11.Rows.Count - 1)
                                    {
                                        Pass += WcJob;
                                    }
                                    else
                                    {
                                        Pass += WcJob + "_";
                                    }
                                }
                            }
                        }
                        //等待出站过程新进站分装点位特殊处理
                        if (result==5)
                        {
                            //非三个分装点位无法进站
                            if (WcId != "118"&&WcId != "199")
                            {
                                code = 0;//界面不处理--异常
                                msg = "当前拧紧任务未完成，无法进站新车";
                            }
                        }
                    }
                    else if (result == 2 || result == 0)
                    {
                        code = 2;//界面不处理--正常
                        msg = "无拧紧任务:【工位：" + WcNm + "】【VIN：" + VIN + "】";
                    }
                    else
                    {
                        code = 0;
                        switch (result)
                        {
                            case 1:
                                msg = "不在队列中";
                                break;
                            case 3:
                                msg = "相同使能类型命中多条配置";
                                break;
                            case 7:
                                msg = "等待出站过程无法进站新车";
                                break;
                        }
                    }
                }
                return Content(new { code, dt_Tighten, msg, Pass }.ToJson());
            }
            catch (Exception ex)
            {
                code = -1;
                msg = ex.Message;
                return Content(new { code, msg }.ToJson());
            }
        }
        //校验和提交Pass
        public ActionResult PassBindTg(Tg_ByPass_Pro KeyByPass, string WcJobCd, string Position, string plineType = "主线", string account = null, string password = null)
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
                            //扫码工位直接出站
                            //恢复逻辑
                            if (POutStatus == 1||WcId== "118" || WcId == "198" || WcId == "199")
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
                                                PlineId='" + PlineId + "' AND WcId='" + WcId + "' AND Status=1";
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
        public int CheckValid(string VIN, string WcId, string DvcTyp, out DataTable ShowTable)
        {
            //0-空车;1-VIN不在队列中;2-没有拧紧任务;3-该车命中多种图号;4-重新加载页面;5-当前拧紧任务未完成无法进站新车;7-等待出站过程扫码工位可以进站,MQTT工位无法进站新车;100-成功
            //1.检查VIN本身有效性
            if (VIN == "9999")
            {
                ShowTable = null;
                return 0;
            }
            else
            {
                //2.检查配置是否存在
                string str = $"select MatCd,CarType from P_ProducePlan_Pro where VIN like '%{VIN}'";
                DataTable dt = database.FindTableBySql(str);
                if (dt.Rows.Count == 0)
                {
                    ShowTable = null;
                    return 1;
                }
                else
                {
                    string ProCd = dt.Rows[0]["MatCd"].ToString();
                    string CarType = (string)dt.Rows[0][1];
                    //综合查找
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"select TightConfigView.WcJobCd,JobCd,TorqueSL,TorqueUL,TorqueLL,AngleUL,AngleLL,Num,Ord,Num as T_sort,'NG'is_OK,Position,ControllerID,ControllerPort,Code,Type,RsvFld1,'--' Torque,'--' Angle ,'" + VIN + @"' as Vin,t.G_max
                                  from TightConfigView join
								  (select count(*) as G_max,WcJobCd from Tg_JobTorqueConfig group by WcJobCd) t on TightConfigView.WcJobCd =t.WcJobCd WHERE Type='产品' and CHARINDEX(Code,'" + ProCd + "')>0 and WcId='" + WcId + "' UNION ALL ");
                    sb.Append(@" SELECT  a.WcJobCd,JobCd,TorqueSL,TorqueUL,TorqueLL,AngleUL,AngleLL,Num,Ord,Num as T_sort,'NG'is_OK,Position,ControllerID,ControllerPort,Code,Type,a.RsvFld1,'--' Torque,'--' Angle  , '" + VIN + @"' as Vin,t.G_max
                                  FROM TightConfigView a join produceBom b on a.Code=b.MatCd join
								  (select count(*) as G_max,WcJobCd from Tg_JobTorqueConfig group by WcJobCd) t on a.WcJobCd =t.WcJobCd where Type='图号' and b.ProductCd='" + ProCd + "' and a.WcId='" + WcId + "' UNION ALL ");
                    sb.Append(@" select TightConfigView.WcJobCd,JobCd,TorqueSL,TorqueUL,TorqueLL,AngleUL,AngleLL,Num,Ord,Num as T_sort,'NG'is_OK,Position,ControllerID,ControllerPort,Code,Type,RsvFld1,'--' Torque,'--' Angle  , '" + VIN + @"' as Vin,t.G_max
                                  from TightConfigView join
								  (select count(*) as G_max,WcJobCd from Tg_JobTorqueConfig group by WcJobCd) t on TightConfigView.WcJobCd =t.WcJobCd WHERE Type='车型' AND Code='" + CarType + "' and WcId='" + WcId + "'  UNION ALL ");
                    sb.Append(@" select c.WcJobCd,JobCd,TorqueSL,TorqueUL,TorqueLL,AngleUL,AngleLL,Num,Ord,Num as T_sort,'NG'is_OK,Position,ControllerID,ControllerPort,Code,Type,c.RsvFld1,'--' Torque,'--' Angle  , '" + VIN + @"' as Vin,t.G_max
                                  from TightConfigView c JOIN dbo.produceBom d ON c.RsvFld1=d.MatCd join
								  (select count(*) as G_max,WcJobCd from Tg_JobTorqueConfig group by WcJobCd) t on c.WcJobCd =t.WcJobCd WHERE Type='车型+图号' and d.ProductCd='" + ProCd + "' AND c.Code='" + CarType + "' and c.WcId='" + WcId + "' ");
                    DataTable result = DbHelperSQL.OpenTable(sb.ToString());
                    string str_sql = "";
                    if (DvcTyp != null && DvcTyp.Contains("左"))
                    {
                        str_sql += "Position like'%左%'";
                    }
                    else if (DvcTyp != null && DvcTyp.Contains("右"))
                    {
                        str_sql += "Position like'%右%'";
                    }
                    DataRow[] datas = result.Select(str_sql, "Ord");
                    if (datas.Length == 0)
                    {
                        ShowTable = null;
                        return 2;
                    }
                    //3.检查配置是否合理
                    //根据IP、Port分类后总条数作为理论逻辑出站总数
                    bool isOk = true;
                    DataTable Configtable = result.DefaultView.ToTable(true, new string[] { "ControllerID", "ControllerPort", "JobCd", "Code", "Type", "RsvFld1" });
                    Configtable = CheckPriority(Configtable);
                    ShowTable = result.Clone();
                    DataTable TempTable = result.Clone();
                    foreach (var item in datas)
                    {
                        TempTable.Rows.Add(item.ItemArray);
                    }
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
                        for (int i = 0; i < Configtable.Rows.Count; i++)
                        {
                            string IP = Configtable.Rows[i]["ControllerID"].ToString();
                            int Port = Convert.ToInt32(Configtable.Rows[i]["ControllerPort"]);
                            int JobCd = Convert.ToInt32(Configtable.Rows[i]["JobCd"]);
                            string Code = Configtable.Rows[i]["Code"].ToString();
                            string Type = Configtable.Rows[i]["Type"].ToString();
                            string MatCd = Configtable.Rows[i]["RsvFld1"].ToString();
                            string str2 = "";
                            if (Type == "车型+图号")
                            {
                                str2 = "ControllerID='" + IP + "' and ControllerPort='" + Port + "' and JobCd='" + JobCd + "' and Code='" + Code + "' and Type='" + Type + "' and RsvFld1='" + MatCd + "'";
                            }
                            else
                            {
                                str2 = "ControllerID='" + IP + "' and ControllerPort='" + Port + "' and JobCd='" + JobCd + "' and Code='" + Code + "' and Type='" + Type + "'";
                            }
                            DataRow[] dataRows = TempTable.Select(str2, "Ord");
                            foreach (DataRow row in dataRows)
                            {
                                ShowTable.Rows.Add(row.ItemArray);
                            }
                        }
                        //4.检查是否有拧完的任务
                        string str5 = "SELECT Vin,LOutStatus FROM dbo.Tg_WcIOStatus WHERE WcId='" + WcId + "'";
                        DataTable dt5 = DbHelperSQL.OpenTable(str5);
                        if (dt5.Rows.Count == 0)
                        {
                            return 100;
                        }
                        else
                        {
                            string Oldvin = dt5.Rows[0][0].ToString();
                            int LOutStatus = Convert.ToInt32(dt5.Rows[0][1]);
                            if (LOutStatus == 0)
                            {
                                if (string.IsNullOrEmpty(Oldvin))
                                {
                                    return 100;
                                }
                                else if (Oldvin == VIN)
                                {
                                    return 4;
                                }
                                else
                                {
                                    return 5;
                                }
                            }
                            else
                            {
                                if (Oldvin == VIN)
                                {
                                    return 4;
                                }
                                else
                                {
                                    return 7;
                                }
                            }
                        }
                    }
                    else
                    {
                        return 3;
                    }
                }
            }
        }

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
        //条码入站
        public ActionResult FZ_Tighten(string TotalCode,string VIN, string WcNm, string WcId)
        {
            DataTable dt_Tighten = new DataTable();
            int code = 1;//显示dt表
            string msg = "";
            try
            {
                string Pass = "";
                int Rst = 0;
                StringBuilder sb = new StringBuilder();
                sb.Append(@"SELECT  a.WcJobCd,JobCd,TorqueSL,TorqueUL,TorqueLL,AngleUL,AngleLL,Num,Ord,Num as T_sort,'NG'is_OK,Position,ControllerID,ControllerPort,Code,Type,a.RsvFld1,'--' Torque,'--' Angle  , '" + TotalCode + @"' as Vin,t.G_max
                        FROM TightConfigView a  join
						(select count(*) as G_max,WcJobCd from Tg_JobTorqueConfig group by WcJobCd) t on a.WcJobCd =t.WcJobCd where Type='图号' and a.Code='" + VIN + "' and a.WcId='" + WcId + "' ORDER BY a.Ord");
                DataTable result = DbHelperSQL.OpenTable(sb.ToString());
                if (result.Rows.Count > 0)
                {
                    bool isOk = true;
                    DataTable Configtable = result.DefaultView.ToTable(true, new string[] { "ControllerID", "ControllerPort", "JobCd", "Code", "Type", "RsvFld1" });
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

                        //4.检查是否有拧完的任务
                        string str5 = "SELECT Vin,LOutStatus FROM dbo.Tg_WcIOStatus WHERE WcId='" + WcId + "'";
                        DataTable dt5 = DbHelperSQL.OpenTable(str5);
                        if (dt5.Rows.Count == 0)
                        {
                            //拧紧
                            Rst = 1;
                        }
                        else
                        {
                            string Oldvin = dt5.Rows[0][0].ToString();
                            if (Oldvin == TotalCode)
                            {
                                //刷新
                                Rst = 2;
                            }
                            else
                            {
                                //拧紧
                                Rst = 1;
                            }

                        }
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
                    msg = "拧紧入站成功:【工位：" + WcNm + "】【条码：" + TotalCode + "】";
                    //调整扩充行
                    for (int i = 0; i < result.Rows.Count; i++)
                    {
                        int rownum = result.Rows.Count;
                        int tg_num = Convert.ToInt16(result.Rows[i]["Num"].ToString());
                        if (tg_num > 1)
                        {
                            result.Rows.Add(result.Rows[i]["WcJobCd"].ToString(), result.Rows[i]["JobCd"].ToString(), result.Rows[i]["TorqueSL"].ToString(), result.Rows[i]["TorqueUL"].ToString(), result.Rows[i]["TorqueLL"].ToString(), result.Rows[i]["AngleUL"].ToString(), result.Rows[i]["AngleLL"].ToString(), tg_num - 1, result.Rows[i]["Ord"].ToString(), result.Rows[i]["T_sort"].ToString(), "NG", result.Rows[i]["Position"].ToString(), result.Rows[i]["ControllerID"].ToString(), Convert.ToInt32(result.Rows[i]["ControllerPort"]), result.Rows[i]["Code"].ToString(), result.Rows[i]["Type"].ToString(), result.Rows[i]["RsvFld1"].ToString(), "--", "--", TotalCode, result.Rows[i]["G_max"].ToString());
                        }
                    }
                    //重新排序
                    result.DefaultView.Sort = "WcJobCd,Ord asc,Num asc";
                    dt_Tighten = result.DefaultView.ToTable();
                    if (Rst > 1)//非全新进站从过程表取已经拧紧和Pass数据
                    {
                        string str = @"select Serial,RsvFld2,Torque,Angle,ControllerID,ControllerPort from Tg_TightenDataPro where WcId='" + WcId + "'";
                        DataTable dt = database.FindTableBySql(str);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int Num = Convert.ToInt32(dt.Rows[i]["Serial"]);
                            int Ord = Convert.ToInt32(dt.Rows[i]["RsvFld2"]);
                            decimal Torque = Convert.ToDecimal(dt.Rows[i]["Torque"]);
                            decimal Angle = Convert.ToDecimal(dt.Rows[i]["Angle"]);
                            string IP = dt.Rows[i]["ControllerID"].ToString();
                            int Port = Convert.ToInt32(dt.Rows[i]["ControllerPort"]);
                            string str9 = "ControllerID='" + IP + "' AND ControllerPort='" + Port + "' AND Ord='" + Ord + "' AND Num='" + Num + "'";
                            DataRow[] row = dt_Tighten.Select(str9);
                            foreach (DataRow item in row)
                            {
                                item["is_OK"] = "OK";
                                item["Torque"] = Torque;
                                item["Angle"] = Angle;
                            }
                        }
                        string str11 = @"SELECT IP,Port,Code,MatCode,Cate,IsPass FROM dbo.Tg_InEnableDetail WHERE WcId='" + WcId + "' AND Vin='" + TotalCode + "' AND IsPass='Pass'";
                        DataTable dt11 = database.FindTableBySql(str11);
                        if (dt11.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt11.Rows.Count; i++)
                            {
                                string IP = dt11.Rows[i]["IP"].ToString();
                                int Port = Convert.ToInt32(dt11.Rows[i]["Port"]);
                                string Cate = dt11.Rows[i]["Cate"].ToString();
                                string Code = dt11.Rows[i]["Code"].ToString();
                                string str12 = "SELECT WcJobCd FROM dbo.TightConfigView WHERE ControllerID='" + IP + "' AND ControllerPort='" + Port + "' AND WcId='" + WcId + "' AND Code='" + Code + "' AND Type='" + Cate + "'"; ;
                                DataTable dt12 = database.FindTableBySql(str12);
                                string WcJob = dt12.Rows[0][0].ToString();
                                if (i == dt11.Rows.Count - 1)
                                {
                                    Pass += WcJob;
                                }
                                else
                                {
                                    Pass += WcJob + "_";
                                }
                            }
                        }
                    }
                }
                else if (Rst == 0)
                {
                    msg = "相同使能类型命中多条配置";
                    code = 0;
                }
                else
                {
                    msg = "拧紧配置发生异常";
                    code = 0;
                }
                return Content(new { code, dt_Tighten, msg, Pass }.ToJson());
            }
            catch (Exception ex)
            {
                code = -1;
                msg = ex.Message;
                return Content(new { code, msg }.ToJson());
            }
        }
       
        public ActionResult NowTask()
        {
            DataTable dt_Tighten = new DataTable();
            string msg = "";
            string Pass = "";
            int code = 1;
            string str = "SELECT Vin FROM dbo.Tg_WcIOStatus WHERE WcId='198' AND InStatus=1";
            DataTable dt = database.FindTableBySql(str);
            string TotalCode = "";
            if (dt.Rows.Count > 0)
            {
                TotalCode = dt.Rows[0][0].ToString();
                string str1 = "SELECT Code FROM dbo.Tg_InEnableDetail WHERE WcId='198' AND Vin='" + TotalCode + "'";
                DataTable dt1 = database.FindTableBySql(str1);
                string MatCd= dt1.Rows[0][0].ToString();
                StringBuilder sb = new StringBuilder();
                sb.Append(@"SELECT  a.WcJobCd,JobCd,TorqueSL,TorqueUL,TorqueLL,AngleUL,AngleLL,Num,Ord,Num as T_sort,'NG'is_OK,Position,ControllerID,ControllerPort,Code,Type,a.RsvFld1,'--' Torque,'--' Angle  , '" + TotalCode + @"' as Vin,t.G_max
                        FROM TightConfigView a  join
						(select count(*) as G_max,WcJobCd from Tg_JobTorqueConfig group by WcJobCd) t on a.WcJobCd =t.WcJobCd where Type='图号' and a.Code='" + MatCd + "' and a.WcId='198' ORDER BY a.Ord");
                DataTable result = DbHelperSQL.OpenTable(sb.ToString());
                msg = "拧紧入站成功:【工位：内饰线下分装】【条码：" + TotalCode + "】";
                //调整扩充行
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    int rownum = result.Rows.Count;
                    int tg_num = Convert.ToInt16(result.Rows[i]["Num"].ToString());
                    if (tg_num > 1)
                    {
                        result.Rows.Add(result.Rows[i]["WcJobCd"].ToString(), result.Rows[i]["JobCd"].ToString(), result.Rows[i]["TorqueSL"].ToString(), result.Rows[i]["TorqueUL"].ToString(), result.Rows[i]["TorqueLL"].ToString(), result.Rows[i]["AngleUL"].ToString(), result.Rows[i]["AngleLL"].ToString(), tg_num - 1, result.Rows[i]["Ord"].ToString(), result.Rows[i]["T_sort"].ToString(), "NG", result.Rows[i]["Position"].ToString(), result.Rows[i]["ControllerID"].ToString(), Convert.ToInt32(result.Rows[i]["ControllerPort"]), result.Rows[i]["Code"].ToString(), result.Rows[i]["Type"].ToString(), result.Rows[i]["RsvFld1"].ToString(), "--", "--", TotalCode, result.Rows[i]["G_max"].ToString());
                    }
                }
                //重新排序
                result.DefaultView.Sort = "WcJobCd,Ord asc,Num asc";
                dt_Tighten = result.DefaultView.ToTable();
                string str2 = @"select Serial,RsvFld2,Torque,Angle,ControllerID,ControllerPort from Tg_TightenDataPro where WcId='198'";
                DataTable dt2 = database.FindTableBySql(str2);
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    int Num = Convert.ToInt32(dt2.Rows[i]["Serial"]);
                    int Ord = Convert.ToInt32(dt2.Rows[i]["RsvFld2"]);
                    decimal Torque = Convert.ToDecimal(dt2.Rows[i]["Torque"]);
                    decimal Angle = Convert.ToDecimal(dt2.Rows[i]["Angle"]);
                    string IP = dt2.Rows[i]["ControllerID"].ToString();
                    int Port = Convert.ToInt32(dt2.Rows[i]["ControllerPort"]);
                    string str9 = "ControllerID='" + IP + "' AND ControllerPort='" + Port + "' AND Ord='" + Ord + "' AND Num='" + Num + "'";
                    DataRow[] row = dt_Tighten.Select(str9);
                    foreach (DataRow item in row)
                    {
                        item["is_OK"] = "OK";
                        item["Torque"] = Torque;
                        item["Angle"] = Angle;
                    }
                }
                string str11 = @"SELECT IP,Port,Code,MatCode,Cate,IsPass FROM dbo.Tg_InEnableDetail WHERE WcId='198' AND Vin='" + TotalCode + "' AND IsPass='Pass'";
                DataTable dt11 = database.FindTableBySql(str11);
                if (dt11.Rows.Count > 0)
                {
                    for (int i = 0; i < dt11.Rows.Count; i++)
                    {
                        string IP = dt11.Rows[i]["IP"].ToString();
                        int Port = Convert.ToInt32(dt11.Rows[i]["Port"]);
                        string Cate = dt11.Rows[i]["Cate"].ToString();
                        string Code = dt11.Rows[i]["Code"].ToString();
                        string str12 = "SELECT WcJobCd FROM dbo.TightConfigView WHERE ControllerID='" + IP + "' AND ControllerPort='" + Port + "' AND WcId='198' AND Code='" + Code + "' AND Type='" + Cate + "'"; ;
                        DataTable dt12 = database.FindTableBySql(str12);
                        string WcJob = dt12.Rows[0][0].ToString();
                        if (i == dt11.Rows.Count - 1)
                        {
                            Pass += WcJob;
                        }
                        else
                        {
                            Pass += WcJob + "_";
                        }
                    }
                }
            }
            else
            {
                //无任务
                code = 0;
            }
            return Content(new { code, dt_Tighten, msg, Pass,TotalCode }.ToJson());
        }
        /// <summary>
        /// 获取基础信息，主要包括工厂模型，账户信息，解析规则,拧紧规则
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult GetStaticInfoFZ()
        {
            int code = 1;
            string msg = "";
            //基础静态信息---工厂模型、账户信息
            Dictionary<string, string> BaseInfoProps = new Dictionary<string, string>();
            //解析规则
            DataTable BarCode = new DataTable();
            //AVI信息
            Dictionary<string, string> AviProps = new Dictionary<string, string>();
            List<string> strs = new List<string>();
            ////产线信息
            //string PlineType = "";
            try
            {
                //2.获取工厂模型
                //2.1获取IP地址
                var IP = NetHelper.GetIPAddress();
                //IP = "10.138.13.94";
                //2.2根据IP地址获取设备-工位---公司
                //获取工位--字典中使用classid代替
                q_KeyParts.GetRowValue("BBdbR_DvcBase", "ClassId,DvcCatg,DvcTyp,DvcLocatn", "IPAddr", IP, ref BaseInfoProps);
                ////获取岗位
                //q_KeyParts.GetRowValue("BBdbR_PostBase", "PostCd,PostNm,WcId", "PostId", BaseInfoProps["ClassId"], ref BaseInfoProps);
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
                //// *****获取产线发布基础信息*****
                //PlineType = q_KeyParts.GetPlineType(BaseInfoProps["PlineId"]);
                if (BaseInfoProps["PlineCd"].ToString() == "Line-17")
                {
                    q_KeyParts.GetRowValue("BBdbR_AVIPlinePublishConfig", "AviId", "PlineId", BaseInfoProps["PlineId"], ref AviProps);
                }
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
                //var s = ManageProvider.Provider.Current();
                string StfNm = ManageProvider.Provider.Current().Account;
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
                //获取需要拧紧的图号
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
            return Content(new { code, msg, props = BaseInfoProps, BarCode, AviProps, strs }.ToJson());
        }
    }
}