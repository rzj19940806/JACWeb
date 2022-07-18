using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
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
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Controllers
{
    public class OTController : Controller
    {
        Q_KeyPartsBll q_KeyParts = new Q_KeyPartsBll();
        Base_UserBll base_userbll = new Base_UserBll();
        Repository.RepositoryFactory<Q_KeyByPass_Pro> ByPassRepository = new Repository.RepositoryFactory<Q_KeyByPass_Pro>();
        Repository.RepositoryFactory<Q_KeyPartsBind_Pro> Q_KeyPartsBindRepository = new Repository.RepositoryFactory<Q_KeyPartsBind_Pro>();
        Repository.RepositoryFactory<BBdbR_AVIBase> AVIRepository = new Repository.RepositoryFactory<BBdbR_AVIBase>();
        /// <summary>
        /// 关重件录入界面
        /// </summary>
        /// <returns>关重件录入界面</returns>
        [LoginAuthorize]
        public ActionResult Q_KeyParts()
        {
            var IP = NetHelper.GetIPAddress();
            DataTable type = q_KeyParts.GetPlineTypeByIp(IP);

            if (IP == "10.138.13.147")
                return View("Q_KeyPartsChars");
            else if (IP == "10.138.13.146")
                return View("Q_KeyPartsTire");

            if (type.Rows.Count > 0 && type.Rows[0]["PlineCd"].ToString() == "Line-19")
                return View("Q_KeyPartsWindows");
            else if (type.Rows.Count > 0 && type.Rows[0]["PlineCd"].ToString() == "Line-17")
                return View("Q_KeyPartsGasBag");
            else if(type.Rows.Count > 0 && type.Rows[0]["PlineTyp"].ToString() == "生产辅线")
                return View("Q_KeyPartsAssist");
            else
                return View("Q_KeyPartsAny");
        }
        [LoginAuthorize]
        public ActionResult Any()
        {
            return View("Q_KeyPartsAny");
        }
        [LoginAuthorize]
        public ActionResult Assist()
        {
            return View("Q_KeyPartsAssist");
        }
        [LoginAuthorize]
        public ActionResult E()
        {
            return View("Q_KeyPartsE");
        }
        [LoginAuthorize]
        public ActionResult Windows()
        {
            return View("Q_KeyPartsWindows");
        }
        [LoginAuthorize]
        public ActionResult GasBag()
        {
            return View("Q_KeyPartsGasBag");
        }
        [LoginAuthorize]
        public ActionResult Char()
        {
            return View("Q_KeyPartsChars");
        }

        [LoginAuthorize]
        public ActionResult Tire()
        {
            return View("Q_KeyPartsTire");
        }

        public ActionResult Screen()
        {
            return View("Q_KeyPartsScreen"); 
        }

        #region 关重件录入-主线
        /// <summary>
        /// 获取基础信息，主要包括工厂模型，账户信息，解析规则
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult GetStaticInfo()
        {
            int code = 1;
            string msg = "";
            //基础静态信息---工厂模型、账户信息
            Dictionary<string, string> BaseInfoProps = new Dictionary<string, string>();
            //解析规则
            DataTable BarCode = new DataTable();
            //AVI信息
            Dictionary<string, string> AviProps = new Dictionary<string, string>();
            ////产线信息
            //string PlineType = "";
            try
            {
                //2.获取工厂模型
                //2.1获取IP地址
                var IP = NetHelper.GetIPAddress();
                //IP = "10.138.13.62";
                //2.2根据IP地址获取设备-工位---公司
                //获取工位--字典中使用classid代替
                q_KeyParts.GetRowValue("BBdbR_DvcBase", "ClassId,DvcCatg", "IPAddr", IP, ref BaseInfoProps);
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
                if (BaseInfoProps["PlineCd"].ToString()== "Line-17")
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

            return Content(new { code, msg, props=BaseInfoProps, BarCode, AviProps }.ToJson());
        }

        //public ActionResult GetCarInfo(string AVIId, string CarQuene, string WcId)
        //{
        //    DataTable vinInfo = new DataTable();
        //    try
        //    {
        //        vinInfo = q_KeyParts.getvininfo(AVIId, CarQuene);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new { code = -1, msg = "获取车身实时队列信息时发生异常：" + ex.Message }.ToJson());
        //    }

        //    if (vinInfo == null||vinInfo.Rows.Count==0)
        //    {
        //        return Content(new { code = -2, msg = "车身实时队列暂无当前工位数据！" }.ToJson());
        //    }
        //    else if (vinInfo.Rows[0]["vin"]==DBNull.Value)
        //    {
        //        return Content(new { code = -2, msg = "车身实时队列数据异常！" }.ToJson());
        //    }
        //    else if (vinInfo.Rows[0]["vin"].ToString() == "9999")
        //    {
        //        return Content(new { code = -2, msg = "当前工位无车！" }.ToJson());
        //    }
        //    else
        //    {
        //        return GetCarInfoByVIN(vinInfo, WcId);
        //    }
        //}
        public ActionResult GetCarInfoByStf(string VIN,string WcId ,bool del = true, string plineType = "主线")
        {
            DataTable vinInfo = new DataTable();
            try
            {
                string sql = "delete Q_ScanStatus_Timely  where WcId = '"+ WcId+"'";
                DbHelperSQL.ExecuteSql(sql);
                if (VIN=="9999")
                {
                    return Content(new { code = 2, msg = "空车！" }.ToJson());
                }
                vinInfo = q_KeyParts.GetVINInfoByStf(VIN); //获取小时中的信息
                if (vinInfo.Rows.Count==0||vinInfo == null)
                {
                    return Content(new { code = -2, msg = "对应AVI站点车身队列中未找到该VIN！" }.ToJson());
                }
                else
                {
                    return GetCarInfoByVIN(vinInfo, WcId, del, plineType);
                }
            }
            catch (Exception ex)
            {
                return Content(new { code = -1, msg = "获取VIN码信息时发生异常：" + ex.Message }.ToJson());
            }
        }


        [LoginAuthorize]
        public ActionResult GetCarInfoByVIN(DataTable vinInfo, string WcId, bool del = true,string plineType = "主线")
        {
            int code = 1;
            string msg = "";
            DataTable Product = null;
            //DataTable ProductMat = null;
            DataTable Parts = null;
            DataTable PartImgs = null;
            DataTable PartBond = null;
            try//获取当前工位车身对应信息
            {
                Product = q_KeyParts.GetProduct(vinInfo.Rows[0]["ProductMatCd"].ToString());//获取产品基本信息中的产品信息
                DataTable partTable = q_KeyParts.GetParts(Product.Rows[0]["MatId"].ToString(),WcId);//获取产品工位物料配置信息
                PartImgs = q_KeyParts.GetPartImgs(partTable, vinInfo.Rows[0]["PlineId"].ToString(), WcId, vinInfo.Rows[0]["Vin"].ToString(),del, plineType);//获取产品工位物料配置信息//顺便刷新实时扫码表
                PartBond = q_KeyParts.GetBondParts(vinInfo.Rows[0]["VIN"].ToString(), WcId);
                DataRow[] partrows = partTable.Select("IsScan='1'");
                Parts = partTable.Clone();
                for (int i = 0; i < partrows.Length; i++)
                {
                    Parts.ImportRow(partrows[i]);
                }
            }
            catch (Exception ex)
            {
                code = -1;
                msg = "获取车身BOM信息、更新扫码状态时发生错误:" + ex.Message;
            }
            return Content(new { code, msg, vinInfo, Product, Parts, PartImgs, PartBond }.ToJson());
        }

        public ActionResult PartBind(Q_KeyPartsBind_Pro KeyPartsBind,int MatNo,string plineType="主线")
        {
            int code = 1;
            string msg = "关重件绑定成功";
            try
            {
                string tableName = plineType == "主线" ? "Q_ScanStatus_Timely" : "Q_ScanStatus_Assist_Timely";
                if (q_KeyParts.PartVerify(KeyPartsBind) > 0)
                {
                    code = -1;
                    msg = "该条码已绑定";
                }
                else
                {
                    KeyPartsBind.Create();
                    int num = q_KeyParts.PartBind(KeyPartsBind, MatNo, tableName);
                    if (num<=0)
                    {
                        code = 0;
                        msg = "多屏协同同时操作，对方已完成作业，本屏作业已取消";
                    }
                }
            }
            catch (Exception ex)
            {
                code = -1;
                msg = "更新扫码实时状态表时发生错误:" + ex.Message;
            }
            return Content(new { code, msg }.ToJson());
        }

        public ActionResult EnvironmentalCodeBind(string VIN, string strCode)
        {
            int code = 1;
            string msg = "环保码绑定成功";
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append($"insert into P_EnvironmentalCode VALUES (newid(),'{VIN}','{strCode}',getdate(),'1')");
                if (q_KeyParts.ExecuteSql(sql)!=1)
                {
                    code = 0;
                    msg = "环保码绑定失败";
                }
            }
            catch (Exception ex)
            {
                code = -1;
                msg = "环保码绑定时发生错误:" + ex.Message;
            }
            return Content(new { code, msg }.ToJson());
        }

        public ActionResult PassBind(Q_KeyByPass_Pro KeyByPass, int MatNo, string plineType = "主线",string account=null,string password=null)
        {
            int code=1;//-2.程序异常错误，0.其他完成作业，1.正常执行，2.无PASS权限且次数使用完，3.账号异常或无权限
            string msg="";
            try
            {
                int passRestTimes = 0;
                if (account==null)
                {
                    //pass次数校核
                    string passCount = $"select IIF((select count(*) from Base_User U join Base_ObjectUserRelation OU ON OU.UserId=U.UserId and U.UserId='System' join Base_ButtonPermission BP on OU.ObjectId = BP.ObjectId join Base_Button B on BP.ModuleButtonId=B.ButtonId  and B.FullName='PASS')= 0,(select isnull((select top 1 isnull(Seq, 3) from BBdbR_WcBase where WcCd = '{KeyByPass.WcCd}' and Enabled = 1),0)-count(*) from Q_KeyByPass_Pro where DATEDIFF(DD, Datetime, getdate()) = 0 and WcCd = '{KeyByPass.WcCd}' and Enabled = 1),999)";
                    passRestTimes = Convert.ToInt32(DataFactory.Database().FindTableBySql(passCount).Rows[0][0]);
                    if (passRestTimes<=0)
                    {
                        code = 2;
                        msg = "当前登录账号无PASS权限且当前工位无剩余PASS次数";
                    }
                }
                else
                {
                    Base_User entity = DataFactory.Database().FindEntity<Base_User>("Account", account);
                    var a1 = entity.Enabled == "1";
                    var a2 = Md5Helper.MD5(DESEncrypt.Encrypt(password.ToLower(), entity.Secretkey).ToLower(), 32).ToLower();
                    var a3 = entity.Password;
                    var a4 = a2 == a3;
                    if (entity != null && entity.Enabled=="1" && entity.Password == Md5Helper.MD5(DESEncrypt.Encrypt(password.ToLower(),entity.Secretkey).ToLower(), 32).ToLower())
                    {
                        string sql= $"select U.RealName,B.FullName buttonName,M.FullName moduleName from Base_User U join Base_ObjectUserRelation OU ON OU.UserId=U.UserId and U.UserId='{entity.UserId}' join Base_ButtonPermission BP on OU.ObjectId = BP.ObjectId join Base_Button B on BP.ModuleButtonId = B.ButtonId and B.Enabled = 1 and B.FullName = 'PASS' join Base_Module M on B.ModuleId = M.ModuleId and M.FullName = '关重件'";
                        if (DataFactory.Database().FindTableBySql(sql).Rows.Count==0)
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
                    string tableName = plineType == "主线" ? "Q_ScanStatus_Timely" : "Q_ScanStatus_Assist_Timely";
                    KeyByPass.Create();
                    int num = q_KeyParts.PartPass(KeyByPass, MatNo, tableName);
                    if (num <= 0)
                    {
                        code = 0;
                        msg = "已完成作业，当前作业已取消";
                    }
                    else
                    {
                        msg = $"剩余PASS次数：{passRestTimes}";
                    }
                }
            }
            catch (Exception ex)
            {
                code = -2;
                msg = "更新扫码实时状态表时发生错误:" + ex.Message;
            }
            return Content(new { code, msg }.ToJson());
        }



        //底盘线及合装线获取实时VIN码
        public ActionResult NowVin_ANY(string wcId)
        {
            string getVin = $"select top 1 VIN from P_FitEnCodeInfo a with(nolock) join (select * from BBdbR_WcBase with(nolock) where wcid = '{wcId}') as tab on a.PlineId = tab.PlineId and CodeValue between tab.StartPoint and tab.EndPoint";
            DataTable table= DataFactory.Database().FindTableBySql(getVin);
            return Content(new { Code = 1, Message = table.Rows.Count > 0 ? table.Rows[0][0].ToString() : "9999"}.ToJson());
        }
        #endregion

        //#region 打印
        //public ActionResult Print(string PlineId)
        //{
        //    try
        //    {
        //        Dictionary<string, string> props = new Dictionary<string, string>();
        //        q_KeyParts.GetRowValue("BBdbR_AVIPlinePublishConfig", "AviId,Queue", "PlineId", PlineId, ref props);

        //        //得到该产线上该产品所有配置的物料
        //        string productName = "";
        //        DataTable carInfo = q_KeyParts.getvininfo(props["AviId"], props["Queue"]);
        //        q_KeyParts.GetRowValue("BBdbR_ProductBase", "MatId,MatNm,Notification", "MatCd", props["MatCd"], ref props);
        //        DataTable unScanTable = q_KeyParts.GetPrintData(PlineId, props["MatId"].ToString(),"IsPrint=1");
        //        DataTable scanTable = q_KeyParts.GetPrintData(PlineId, props["MatId"].ToString(), "IsScan=1");
                
        //        DataTable realScanTable = scanTable.Clone();
        //        foreach (DataRow item in scanTable.Rows)
        //        {
        //            int matnum = 0;
        //            if (item["MatNum"]!=System.DBNull.Value&&int.TryParse(item["MatNum"].ToString(),out matnum))
        //            {
        //                for (int j = 0; j < matnum; j++)
        //                {
        //                    realScanTable.Rows.Add(item);
        //                }
        //            }
        //            else
        //            {
        //                realScanTable.Rows.Add(item);
        //            }
        //        }
        //        return Content(new {
        //            Code = "1",
        //            carInfo,
        //            MatCd= props["MatCd"],
        //            MatNm = props["MatNm"],
        //            VIN = props["VIN"],
        //            DateTime=DateTime.Now,
        //            Notification = props["Notification"],
        //            unScanTable,
        //            realScanTable,
        //        }.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = ex.Message }.ToString());
        //    }
        //}
        //#endregion

        #region 辅助
        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="PlineCd"></param>
        /// <param name="text"></param>
        static string FASLogTxtPath = AppDomain.CurrentDomain.BaseDirectory + @"\Document\KeyPartsLog";
        public ActionResult WriteLog(string PlineCd,string text)
        {
            string path = FASLogTxtPath+ $@"\{DateTime.Now.ToString("yyyy-MM")}";
            //string path = Server.MapPath($"~/bin/KeyPartsLog/{DateTime.Now.ToString("yyyy-MM")}");
            string fileName = $"{path}\\{DateTime.Now.ToString("dd")}_{PlineCd}_log.txt";
            q_KeyParts.WriteLog(path, fileName, text);
            return Content(new { code = 1, msg = "写入成功" }.ToJson());
        }
        /// <summary>
        /// 从服务端获取时间
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTime()
        {
            return Content(new {code= 1, msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}.ToJson());
        }
        #endregion

        #region 关重件录入-辅线
        //界面加载获取数据
        public ActionResult getScanStatus(string WcId)
        {
            try
            {
                DataTable scan = q_KeyParts.db.FindTableBySql($"select top 1 VIN from Q_ScanStatus_Assist_Timely where WcId='{WcId}'");
                if (scan.Rows.Count>0)
                {
                    return Content(new { Code = 1, VIN = scan.Rows[0]["VIN"] }.ToJson());
                }
                else
                {
                    return Content(new { Code = 0 }.ToJson());
                }
            }
            catch (Exception ex)
            {
                return Content(new { Code = -1, Message = "获取扫码状态时发生异常："+ex.Message }.ToJson());
            }
        }
        //辅线获取新车
        public ActionResult getScanStatus2( string Vin)
        {
            try
            {
                DataTable newCarTable = q_KeyParts.getNewCar(Vin);
                if (newCarTable.Rows.Count > 0)
                {
                    return Content(new { Code = 1, Message = "辅线车身队列有数据", VIN = newCarTable.Rows[0]["VIN"] }.ToJson());
                }
                else
                {
                    return Content(new { Code = 2, Message = "辅线车身队列无数据" }.ToJson());
                }
            }
            catch (Exception ex)
            {
                return Content(new { Code = -1, Message = "获取扫码状态时发生异常：" + ex.Message }.ToJson());
            }
        }

        public ActionResult getChar(string AviId, string WcId, string type)
        {
            try
            {
                DataTable VINStatus;
                DataTable newCarTable;
                //找到扫码实时状态表中找到对应工位所有的扫码信息和已扫码信息
                DataTable all = q_KeyParts.getAllScan("Q_ScanStatus_Assist_Timely", WcId);
                DataTable scaned = q_KeyParts.getScaned("Q_ScanStatus_Assist_Timely", WcId);

                if (all.Rows.Count==0)
                {
                    newCarTable = q_KeyParts.db.FindTableBySql("select top 2 * from P_LineProductionQueue_Pro with(nolock) where len(VIN) = 17 and plinecd <> '' and PlineNm not like 'PBS%' and len(vin) = 17  order by plinecd desc, carquene ");
                }
                else if ( all.Rows.Count != scaned.Rows.Count)
                {
                    //DataTable scanedLeft = scaned.Select($"VIN = '{all.Rows[0]["VIN"]}'").Count() == 0 ? new DataTable() : scaned.Select($"VIN = '{all.Rows[0]["VIN"]}'").CopyToDataTable();

                    //DataTable scanedRight = scaned.Select($"VIN <> '{all.Rows[0]["VIN"]}'").Count()==0?new DataTable(): scaned.Select($"VIN <> '{all.Rows[0]["VIN"]}'").CopyToDataTable();
                    string VIN2 = all.Select($"VIN <> '{all.Rows[0]["VIN"]}'").Length == 0 ? "" : all.Select($"VIN <> '{all.Rows[0]["VIN"]}'")[0]["VIN"].ToString();
                    return Content(new { Code = 0, Message = "扫码状态表中有数据",VIN1= all.Rows[0]["VIN"], VIN2/*, scanedLeft, scanedRight */}.ToJson());
                }
                else
                {
                    var s = $"vin <> '{all.Rows[0]["VIN"]}'";
                    DataRow[] scanedRight = scaned.Select(s);
                    if (scanedRight.Count()==0)
                    {
                        newCarTable = q_KeyParts.getCharCar(all.Rows[0]["VIN"].ToString(),"");
                    }
                    else
                    {
                        newCarTable = q_KeyParts.getCharCar(all.Rows[0]["VIN"].ToString(), scanedRight[0]["VIN"].ToString());
                    }
                }
                if (newCarTable.Rows.Count == 0)
                {
                    return Content(new { Code = 2, Message = "车身队列无数据" }.ToJson());
                }
                else if (newCarTable.Rows.Count == 1)
                {
                    return Content(new { Code = 1, Message = "车身队列有数据", VIN1 = newCarTable.Rows[0]["VIN"], VIN2 = "" }.ToJson());
                }
                else
                {                                                                                                 
                    return Content(new { Code = 1, Message = "车身队列有数据", VIN1 = newCarTable.Rows[0]["VIN"], VIN2 = newCarTable.Rows[1]["VIN"] }.ToJson());
                }
            }
            catch (Exception ex)
            {
                return Content(new { Code = -1, Message = "获取扫码状态时发生异常：" + ex.Message }.ToJson());
            }
        }        
        //刷新实时扫码表
        public ActionResult CharScanStatus(string wcId, string vin, string matId, string status, int MatNo)
        {
            int code = 1;
            string msg = "成功";
            try
            {
                q_KeyParts.ScanStatus(wcId, vin, matId, status, MatNo, "Q_ScanStatus_Assist_Timely");
            }
            catch (Exception ex)
            {
                code = -1;
                msg = "更新扫码实时状态表时发生错误:" + ex.Message;
            }
            return Content(new { code, msg }.ToJson());
        }
        public ActionResult GetCharCarInfo(string newVIN, string oledVIN, string WcId, bool del = true)
        {
            DataTable vinInfo = new DataTable();
            try
            {
                vinInfo = q_KeyParts.GetVINInfoByStf(newVIN); //获取小时中的信息
                if (vinInfo.Rows.Count == 0 || vinInfo == null)
                {
                    return Content(new { code = -2, msg = "对应AVI站点车身队列中未找到该VIN！" }.ToJson());
                }
                else
                {
                    DataTable Product = q_KeyParts.GetProduct(vinInfo.Rows[0]["ProductMatCd"].ToString());//获取产品基本信息中的产品信息//为了描述//82fd38bf-d5e6-4c2e-a62e-255344708fd4
                    //DataTable WcProduct = q_KeyParts.GetWcProduct(Product.Rows[0]["MatId"].ToString(), WcId);//获取产品工位配置信息
                    DataTable Parts = q_KeyParts.GetParts(Product.Rows[0]["MatId"].ToString(),WcId);//获取产品工位物料配置信息
                    q_KeyParts.GetCharPart(Parts, vinInfo.Rows[0]["PlineId"].ToString(), WcId, vinInfo.Rows[0]["VIN"].ToString(), oledVIN, del);//顺便刷新实时扫码表
                    return Content(new { code=1, msg="成功", vinInfo, Product, Parts }.ToJson());
                }
            }
            catch (Exception ex)
            {
                return Content(new { code = -1, msg = "获取VIN码信息时发生异常：" + ex.Message }.ToJson());
            }
        }
        #endregion

        #region 补录
        //
        public JsonResult GetByPassParts(JqGridParam jqgridparam,string vin)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                string WhereSql = $" and BarCode='ByPass' and VIN='{vin}'";
                var dataList = ByPassRepository.Repository().FindListPage(WhereSql, ref jqgridparam);
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dataList,
                };
                return Json(JsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }


        public ActionResult GetByPassVinInfo(string VIN)
        {
            return Content(ByPassRepository.Repository().FindEntity("and VIN like '%"+ VIN + ",").ToJson());
        }

        public ActionResult PassBindBind(string VIN,string BarCode,string StfCd)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                DataTable passTable = ByPassRepository.Repository().FindTableTop(1,$" and VIN='{VIN}' and MatCd='{BarCode.Substring(14)}'");
                if (passTable.Rows.Count == 0)
                {
                    passTable = ByPassRepository.Repository().FindTableTop(1, $" and VIN='{VIN}' and MatCd='{BarCode.Substring(0, 12)}'");
                    if (passTable.Rows.Count > 0)
                    {
                        string month = BarCode.Substring(20, 2);
                        if (month == "11")
                        {
                            month = "A";
                        }
                        else if (month == "12")
                        {
                            month = "B";
                        }
                        else
                        {
                            month = month.Substring(1, 1);
                        }
                        BarCode = BarCode.Substring(18, 2) + month + BarCode.Substring(22) + BarCode.Substring(12, 6) + BarCode.Substring(0, 12);
                    }
                }
                if (passTable.Rows.Count > 0)
                {
                    passTable.Rows[0]["BarCode"] = BarCode;
                    Q_KeyByPass_Pro partsPass = DataTableToList<Q_KeyByPass_Pro>(passTable).First();
                    ByPassRepository.Repository().Update(partsPass, isOpenTrans);

                    passTable.Rows[0]["Datetime"] = DateTime.Now;
                    passTable.Rows[0]["Rem"] = StfCd ?? ManageProvider.Provider.Current().Account;
                    passTable.Rows[0]["SupplierCd"] = BarCode.Substring(8,6);

                    passTable.Columns.Add("KeyPartsBindProId", typeof(string));
                    passTable.Rows[0]["KeyPartsBindProId"] = passTable.Rows[0]["KeyByPassProId"];
                    passTable.Columns.Remove("KeyByPassProId");

                    Q_KeyPartsBind_Pro partsBind = DataTableToList<Q_KeyPartsBind_Pro>(passTable).First();
                    Q_KeyPartsBindRepository.Repository().Insert(partsBind, isOpenTrans);

                    database.Commit();//提交事务 
                    return Content(new { code = 1}.ToJson());
                }
                else
                {
                    database.Rollback();
                    return Content(new { code = 0 }.ToJson());
                }
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new { code = -1, msg = ex.Message }.ToJson());
            }
        }
        public ActionResult MustBind(string KeyByPassProId, string BarCode, string StfCd)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                DataTable passTable = ByPassRepository.Repository().FindTable("KeyByPassProId", KeyByPassProId);
                if (passTable.Rows.Count == 0)
                {
                    return Content(new { code = 0,msg="数据传输错误" }.ToJson());
                }
                else
                {
                    passTable.Rows[0]["BarCode"] = BarCode;
                    Q_KeyByPass_Pro partsPass = DataTableToList<Q_KeyByPass_Pro>(passTable).First();
                    ByPassRepository.Repository().Update(partsPass, isOpenTrans);

                    passTable.Rows[0]["Datetime"] = DateTime.Now;
                    passTable.Rows[0]["Rem"] = StfCd ?? ManageProvider.Provider.Current().Account;

                    passTable.Columns.Add("KeyPartsBindProId", typeof(string));
                    passTable.Rows[0]["KeyPartsBindProId"] = passTable.Rows[0]["KeyByPassProId"];
                    passTable.Columns.Remove("KeyByPassProId");

                    Q_KeyPartsBind_Pro partsBind = DataTableToList<Q_KeyPartsBind_Pro>(passTable).First();
                    Q_KeyPartsBindRepository.Repository().Insert(partsBind, isOpenTrans);

                    database.Commit();//提交事务 
                    return Content(new { code = 1 }.ToJson());
                }
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new { code = -1, msg = ex.Message }.ToJson());
            }
        }
        public List<T> DataTableToList<T>(DataTable dt) where T : class, new()
        {
            // 定义集合 
            List<T> ts = new List<T>();
            //定义一个临时变量 
            string tempName = string.Empty;
            //遍历DataTable中所有的数据行 
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性 
                PropertyInfo[] propertys = t.GetType().GetProperties();
                //遍历该对象的所有属性 
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;//将属性名称赋值给临时变量 
                                       //检查DataTable是否包含此列（列名==对象的属性名）  
                    if (dt.Columns.Contains(tempName))
                    {
                        //取值 
                        object value = dr[tempName];
                        //如果非空，则赋给对象的属性 
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(t, value, null);
                        }
                    }
                }
                //对象添加到泛型集合中 
                ts.Add(t);
            }
            return ts;
        }
        #endregion

        #region 电视机
        public ActionResult GetMatImg(string WcId, string VIN)
        {
            try
            {
                DataTable vinInfo = q_KeyParts.getvininfo(WcId, VIN);
                if (vinInfo.Rows.Count == 0)
                {
                    return Content(new { code = 0, msg = "车未变化" }.ToJson());
                }
                else if (vinInfo.Rows[0]["VIN"].ToString() == "9999")
                {
                    return Content(new { code = 1, msg = "无车" }.ToJson());
                }
                else
                {
                    DataTable Product = q_KeyParts.GetProduct(vinInfo.Rows[0]["MatCd"].ToString());
                    DataTable PartImgs = q_KeyParts.GetMatImgs(vinInfo, WcId);
                    return Content(new { code = 2, vinInfo, Product, PartImgs }.ToJson());
                }
            }
            catch (Exception ex)
            {
                WriteLog("电视机", ex.Message);
                return Content(new { code = -1, msg = ex.Message }.ToJson());
            }
        }
        #endregion

        #region 报工反馈

        public ActionResult getVINInfoPlan(string VIN, string PlineId, string WcId, bool del, string plineTyle)
        {
            IDatabase database = DataFactory.Database();


            string sql = $"select *,MatCd as ProductMatCd from P_PublishPlan_Pro with(nolock) where VIN like '%{VIN}'";
            DataTable vinInfo = database.FindTableBySql(sql); //获取当前车身队列中的信息
            if (vinInfo.Rows.Count == 0 || vinInfo == null)
            {
                return Content(new { code = -2, msg = "未找到该VIN！" }.ToJson());
            }
            else
            {
                int code = 1;
                string msg = "";
                DataTable Product = null;
                //DataTable ProductMat = null;
                DataTable Parts = null;
                DataTable PartImgs = null;
                DataTable PartBond = null;
                try//获取当前工位车身对应信息
                {
                    Product = q_KeyParts.GetProduct(vinInfo.Rows[0]["ProductMatCd"].ToString());//获取产品基本信息中的产品信息
                    Parts = q_KeyParts.GetParts(Product.Rows[0]["MatId"].ToString(), WcId);//获取产品工位物料配置信息
                    PartImgs = q_KeyParts.GetPartImgs(Parts, PlineId, WcId, vinInfo.Rows[0]["Vin"].ToString(), del, plineTyle);//获取产品工位物料配置信息//顺便刷新实时扫码表

                    PartBond = q_KeyParts.GetBondParts(vinInfo.Rows[0]["VIN"].ToString(), WcId);
                }
                catch (Exception ex)
                {
                    code = -1;
                    msg = "获取车身BOM信息、更新扫码状态时发生错误:" + ex.Message;
                }
                return Content(new { code, msg, vinInfo, Product, Parts, PartImgs, PartBond }.ToJson());
            }
        }

        public string WorkBack(string VIN,string AviId)
        {
            string FaultReason = "";
            string Message = "";
            try
            {
                string FeedBackState = "0";
                string str = "select * from BBdbR_AVIBase where AviId='" + AviId + "' and Enabled = '1'";
                List<BBdbR_AVIBase> avibase = AVIRepository.Repository().FindListBySql(str);
                if (avibase.Count > 0)
                {
                    string isreport = avibase[0].IsReport.ToString();
                    string op_code = avibase[0].OP_CODE;
                    string op_name = avibase[0].OP_NAME;
                    if (isreport == "1")
                    {
                        string sql_planinfo = "select ProducePlanCd from P_ProducePlan_Pro where VIN = '" + VIN + "' and IsFile = '0' and Enabled = '1' order by PlanTime desc ";
                        DataTable plantable = DbHelperSQL.OpenTable(sql_planinfo);
                        if (plantable.Rows.Count > 0)//如果日计划表中有则表示报工反馈表也有
                        {
                            //判断报工反馈表中是否有该数据
                            string sql_feedback = "select PlanFeedBackProId from P_PlanFeedBack_Pro where ProducePlanCd = '" + plantable.Rows[0]["ProducePlanCd"].ToString() + "' and OP_CODE = '" + op_code + "' and VIN = '" + VIN + "'";
                            DataTable feedbacktable = DbHelperSQL.OpenTable(sql_feedback);
                            int isupdate = 0;
                            if (feedbacktable.Rows.Count > 0)
                            {
                                string sql_workback = "update P_PlanFeedBack_Pro set  MODIFY_ID = '" + avibase[0].AviNm + "',FeedbackTime = '" + DateTime.Now + "',FeedBackState = '" + FeedBackState + "' where PlanFeedBackProId = '" + feedbacktable.Rows[0]["PlanFeedBackProId"].ToString() + "'";
                                isupdate = DbHelperSQL.ExecuteSql(sql_workback);
                            }
                            else
                            {
                                string sql_workback = "insert into P_PlanFeedBack_Pro(PlanFeedBackProId,ProducePlanCd,OP_CODE,OP_NAME,MODIFY_ID,FeedbackTime,FeedBackState,VIN,OrderCd,MatCd) values('" + System.Guid.NewGuid().ToString() + "','" + plantable.Rows[0]["ProducePlanCd"].ToString() + "','" + op_code + "','" + op_name + "','" + avibase[0].AviNm + "','" + DateTime.Now + "','" + FeedBackState + "','" + VIN + "','','')";
                                isupdate = DbHelperSQL.ExecuteSql(sql_workback);
                            }
                            //记录本地报工反馈是否成功
                            if (isupdate == 0)
                            {
                                return "记录报工反馈信息失败";
                            }
                            else
                            {
                                string sql_isback = "update P_CarPastRecordInfo set IsBack = '1' where VIN = '" + VIN + "' and AviId = '" + AviId + "' ";
                                int isback = DbHelperSQL.ExecuteSql(sql_isback);
                            }
                        }
                        else  //没有计划编号则将编号默认为 0000——后续在接到发布序列时进行更新信息
                        {
                            string sql_workback = "insert into P_PlanFeedBack_Pro(PlanFeedBackProId,ProducePlanCd,OP_CODE,OP_NAME,MODIFY_ID,FeedbackTime,FeedBackState,VIN,OrderCd,MatCd) values('" + System.Guid.NewGuid().ToString() + "','0000','" + op_code + "','" + op_name + "','" + avibase[0].AviNm + "','" + DateTime.Now + "','" + FeedBackState + "','" + VIN + "','" + plantable.Rows[0]["OrderCd"].ToString() + "','" + plantable.Rows[0]["MatCd"].ToString() + "')";
                            int isinsert = DbHelperSQL.ExecuteSql(sql_workback);

                            if (isinsert == 0)
                            {
                                return "记录报工反馈信息失败";
                            }
                            else
                            {
                                string sql_isback = "update P_CarPastRecordInfo set IsBack = '1' where VIN = '" + VIN + "' and AviId = '" + AviId + "' ";
                                int isback = DbHelperSQL.ExecuteSql(sql_isback);
                            }
                        }
                    }
                    return "记录报工反馈信息成功";
                }
                else
                {
                    return "记录报工反馈信息失败";
                }
            }
            catch (Exception ex)
            {
                return "记录报工反馈信息失败";
            }
        }


        #region 获取当天所有过点车
        public ActionResult GridRecordList(string AVIId, JqGridParam jqgridparam)
        {
            RepositoryFactory<P_CarPastRecordInfo> carpastrecordinfo = new RepositoryFactory<P_CarPastRecordInfo>();
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                //List<DbParameter> parameter = new List<DbParameter>();
                string sql = "";
                sql = "select Row_Number() over ( order by PastTime desc ) as 'No', VIN,CarType,MatCd,CarColor1,PastTime from P_CarPastRecordInfo WITH(NOLOCK) where AviId= '" + AVIId + "' and DateDiff(dd,PastTime,getdate()) = 0 and VIN != '9999'";
                DataTable recordinfo = carpastrecordinfo.Repository().FindTablePageBySql(sql, ref jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = recordinfo,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage
                {
                    Success = false,
                    Code = "-1",
                    Message = "操作失败：" + ex.Message
                }.ToJson());
            }
        }

        #region 根据17位VIN码获取正确VIN码
        public string getRightVIN(string VIN, string AVIId)
        {
            string Message = "";
            try
            {
                string sql_record = "select VIN from P_CarPastRecordInfo where VIN like '%" + VIN + "' and AviId = '" + AVIId + "'";
                DataTable recordtable = DbHelperSQL.OpenTable(sql_record);
                if (recordtable.Rows.Count > 0)
                {
                    return "已采集";
                }
                return "未采集";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
        #endregion


        #region 车身过点
        public ActionResult carPassRecord(string VIN, string account, string AviId)
        {
            try
            {
                #region 获取AviCd/AviNm
                string AviCd = "";
                string AviNm = "";
                string AviType = "";
                StringBuilder strSqlAvi = new StringBuilder();
                strSqlAvi.Append(@"select * from BBdbR_AVIBase where AviId = '" + AviId +  "' and Enabled =1");
                DataTable dtAvi = DataFactory.Database().FindTableBySql(strSqlAvi.ToString(), false);
                if (dtAvi.Rows.Count>0)
                {
                    AviCd = dtAvi.Rows[0]["AviCd"].ToString();
                    AviNm = dtAvi.Rows[0]["AviNm"].ToString();
                    AviType = dtAvi.Rows[0]["AviType"].ToString();
                }
                #endregion




                string result = getRightVIN(VIN, AviId);
                if (result== "已采集")
                {
                    return Content(new JsonMessage { Success = false, Code = "2", Message = result }.ToString());
                }
                else if (result != "未采集")
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = result }.ToString());
                }
                int PlanCheckResult = 0;
                int BodyCheckResult = 0;
                string FaultReason = "";
                int isok = ChangeCarPast(VIN, AviId, AviCd, AviNm, AviType, ref FaultReason);
                if (isok > 0)
                {
                    int isok1 = OverRecordHandle(VIN, AviId);
                    if (isok1 > 0)
                    {
                        WorkBack(VIN, AviId);
                        return Content(new JsonMessage { Success = false, Code = "1", Message = "报工反馈成功！" }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = false, Code = "2", Message = "车身过点失败" }.ToString());
                    }
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "2", Message = "车身过点失败！" + FaultReason }.ToString());
                }

            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "2", Message = ex.Message }.ToString());
            }
        }
        #endregion

        #region 车身过点过程信息,检测质控点，插入车身过点过程表
        public int ChangeCarPast(string VIN, string AviId, string AviCd, string AviNm, string AviType, ref string FaultReason)
        {

            P_CarPastInfo_Pro carpast = new P_CarPastInfo_Pro();
            if (VIN != "9999")
            {

                //获取小时生产过程计划表
                string sqlProducePlaninfo = "select * from P_ProducePlan_Pro where VIN='" + VIN + "' and IsFile='0' and Enabled = '1'";
                DataTable ProducePlaninfo = DbHelperSQL.OpenTable(sqlProducePlaninfo);
                if (ProducePlaninfo.Rows.Count > 0)
                {
                    carpast.BodyNo = ProducePlaninfo.Rows[0]["BodyNo"].ToString();
                    carpast.CarType = ProducePlaninfo.Rows[0]["CarType"].ToString();
                    carpast.MatCd = ProducePlaninfo.Rows[0]["MatCd"].ToString();
                    carpast.CarColor1 = ProducePlaninfo.Rows[0]["CarColor1"].ToString();

                    carpast.SequenceNo = ProducePlaninfo.Rows[0]["SequenceNo"].ToString();
                    carpast.ProducePlanCd = ProducePlaninfo.Rows[0]["ProducePlanCd"].ToString();
                }

                //获取车身的顺序号
                string sql_record = "select PastNo,SequenceNo from P_LineProductionQueue_Pro  WITH(NOLOCK) where VIN = '" + VIN + "' ";
                DataTable carrecord = DbHelperSQL.OpenTable(sql_record);
                if (carrecord.Rows.Count > 0)
                {
                    carpast.PastNo = carrecord.Rows[0]["PastNo"].ToString();//顺序号
                    carpast.SequenceNo = carrecord.Rows[0]["SequenceNo"].ToString();//流水号
                }
            }
            carpast.CarRoute = 1;//过点
            string str_1 = "select * from BBdbR_AVIWhereaboutsConfig where AviId='" + AviId + "' and Enabled = '1'";
            DataTable AviConfig = DbHelperSQL.OpenTable(str_1);
            if (AviConfig.Rows.Count > 0)
            {
                carpast.PlineId = AviConfig.Rows[0]["PlineId"].ToString();
                carpast.ToAVIId = AviConfig.Rows[0]["ToAVIId"].ToString();
                string toavi = "select AviId,AviCd,AviNm,AviType from BBdbR_AVIBase where AviId = '" + carpast.ToAVIId + "' and Enabled = '1'";
                DataTable toavibase = DbHelperSQL.OpenTable(toavi);
                if (toavibase.Rows.Count > 0)
                {

                    carpast.ToAVICd = toavibase.Rows[0]["AviCd"].ToString();
                    carpast.ToAVINm = toavibase.Rows[0]["AviNm"].ToString();
                    //获取产线基础信息表
                    string sqlPlineinfo = "select * from BBdbR_PlineBase where PlineId ='" + carpast.PlineId + "' and Enabled = '1'";
                    DataTable Plineinfo = DbHelperSQL.OpenTable(sqlPlineinfo);
                    if (Plineinfo.Rows.Count > 0)
                    {
                        carpast.PlineCd = Plineinfo.Rows[0]["PlineCd"].ToString();
                        carpast.PlineNm = Plineinfo.Rows[0]["PlineNm"].ToString();
                    }
                }
            }
            string str_2 = "select * from BBdbR_AVIBase where AviId='" + AviId + "' and Enabled = '1'";
            DataTable AviInfo = DbHelperSQL.OpenTable(str_2);
            carpast.CarPastProId = Guid.NewGuid().ToString();
            carpast.AviId = AviId;
            carpast.AviCd = AviInfo.Rows[0]["AviCd"].ToString();
            carpast.AviNm = AviInfo.Rows[0]["AviNm"].ToString();
            carpast.AviType = AviInfo.Rows[0]["AviType"].ToString();
            carpast.VIN = VIN;
            carpast.ArrivalTm = DateTime.Now;

            carpast.CarPatType = 1;//自动
            carpast.PastDate = DateTime.Now.ToString("yyyy-MM-dd");
            carpast.PastTime = DateTime.Now;
            //carpast.IsWrite = 0; //0-未写入
            carpast.IsFile = 0; //0-未转档

            #region 硬防错
            bool isallow = false;//表示该车是否能记录过点
            if (AviCd != "FIT-2" && !String.IsNullOrEmpty(carpast.CarType)) //carpast.CarType != "L5" && carpast.CarType != "L6"
            {
                #region 判断是否所有质控点都录入且合格
                DataTable quatable = new DataTable();
                string QuaCd = "";

                switch (AviCd)
                {
                    case "OK-1":
                        QuaCd = "('Quality-AZ')";
                        break;
                    case "RAIN-1":
                        QuaCd = "('Quality-LY')";
                        break;
                    case "SUBMIT-1":
                        QuaCd = "('Quality-BZ','Quality-DP')";
                        break;
                    case "SUBMIT-2":
                        QuaCd = "('Quality-CZ')";
                        break;

                }

                quatable = getQCPByQCPGroup(QuaCd);
                if (quatable.Rows.Count > 0)
                {
                    int count = 0;
                    string uncrossquenm = "";//未经过的质控点名称
                    string uncheckquenm = "";//未复检的质控点名称
                    for (int i = 0; i < quatable.Rows.Count; i++)
                    {
                        //查询当前车辆是否在该质控点有数据
                        DataTable cardata = IsCarCheakedQCP(VIN, quatable.Rows[i]["QualityCheckPointId"].ToString());
                        if (cardata.Rows.Count != 0)
                        {
                            //如果质控点全部经过,查询是否有未复检不合格或维修项
                            DataTable badtable = IsCarOutPutQCP(VIN, quatable.Rows[i]["QualityCheckPointId"].ToString());
                            if (badtable.Rows.Count == 0)
                            {
                                continue;
                            }
                            else
                            {
                                uncheckquenm += badtable.Rows[0]["QualityCheckPointNm"].ToString() + " ";
                                //FaultReason = "未复检或存在不合格项！";
                            }

                        }
                        else
                        {
                            uncrossquenm += quatable.Rows[i]["QualityCheckPointNm"].ToString() + " ";
                            //FaultReason = "存在未检查的质控点！";


                        }
                        count++;
                    }
                    if (uncrossquenm != "")
                    {
                        FaultReason += "未经过质控点:" + uncrossquenm + " ";
                    }
                    if (uncheckquenm != "")
                    {
                        FaultReason += "存在未复检项质控点:" + uncheckquenm + " ";
                    }
                    if (count == 0)
                    {
                        isallow = true;
                    }
                }
                else
                {
                    isallow = true;
                    //FaultReason = "未找到质控点！";
                }
                #endregion
            }
            else//合装下线不需要判断
            {
                isallow = true;
            }
            if (!isallow)
            {
                return 0;
            }
            #endregion

            string sqlupdatepast = "insert into P_CarPastInfo_Pro(CarPastProId,AviId,AviCd,AviNm,AviType,BodyNo,CarType,MatCd,CarColor1,VIN,SequenceNo,ProducePlanCd,ArrivalTm,CarPatType,CarRoute,PlineId,PlineCd,PlineNm,ToAVIId,ToAVICd,ToAVINm,PastDate,PastTime,PastNo,IsFile,FaultReason) values";
            sqlupdatepast += " ('" + carpast.CarPastProId + "','" + carpast.AviId + "','" + carpast.AviCd + "','" + carpast.AviNm + "','" + carpast.AviType + "','" + carpast.BodyNo + "','" + carpast.CarType + "','" + carpast.MatCd + "','" + carpast.CarColor1 + "','" + carpast.VIN + "','" + carpast.SequenceNo + "','" + carpast.ProducePlanCd + "','" + carpast.ArrivalTm + "','" + carpast.CarPatType + "','" + carpast.CarRoute + "','" + carpast.PlineId + "','" + carpast.PlineCd + "','" + carpast.PlineNm + "','" + carpast.ToAVIId + "','" + carpast.ToAVICd + "','" + carpast.ToAVINm + "','" + carpast.PastDate + "','" + carpast.PastTime + "','" + carpast.PastNo + "','" + carpast.IsFile + "','')";
            int extrow = DbHelperSQL.ExecuteSql(sqlupdatepast);
            if (extrow == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        #endregion

        #region 车身过点记录及更新车身队列

        int OverRecordHandle(string VIN, string AviId)
        {

            //记录过点时间
            DateTime PastStrtTm = System.DateTime.Now;
            string PastDate = PastStrtTm.ToString("yyyy-MM-dd");
            string PlineId = "";
            string PlineCd = "";
            string PlineNm = "";
            string PlineTyp = "";
            string ToAVIId = "";
            string ToAVICd = "";
            string ToAVINm = "";
            string OrderCd = "";
            string PlanTime = "";
            try
            {
                P_CarPastRecordInfo newCarPastRecordInfo = new P_CarPastRecordInfo();

                string sqlcarproinfo = "select * from P_CarPastInfo_Pro where AviId='" + AviId + "' and VIN ='" + VIN + "' order by PastTime desc";//该站点最新的过点记录
                DataTable dtcarproinfo = DbHelperSQL.OpenTable(sqlcarproinfo);
                if (dtcarproinfo.Rows.Count > 0)
                {
                    newCarPastRecordInfo.AviCatg = dtcarproinfo.Rows[0]["AviCatg"].ToString();
                    newCarPastRecordInfo.AviType = dtcarproinfo.Rows[0]["AviType"].ToString();
                    newCarPastRecordInfo.BodyNo = dtcarproinfo.Rows[0]["BodyNo"].ToString();
                    newCarPastRecordInfo.CarType = dtcarproinfo.Rows[0]["CarType"].ToString();
                    newCarPastRecordInfo.MatCd = dtcarproinfo.Rows[0]["MatCd"].ToString();
                    newCarPastRecordInfo.CarColor1 = dtcarproinfo.Rows[0]["CarColor1"].ToString();
                    //newCarPastRecordInfo.CarColor2 = dtcarproinfo.Rows[0]["CarColor2"].ToString();
                    newCarPastRecordInfo.SequenceNo = dtcarproinfo.Rows[0]["SequenceNo"].ToString();
                    newCarPastRecordInfo.ProducePlanCd = dtcarproinfo.Rows[0]["ProducePlanCd"].ToString();
                    newCarPastRecordInfo.CarRoute = Convert.ToInt32(dtcarproinfo.Rows[0]["CarRoute"].ToString());

                    PlineId = dtcarproinfo.Rows[0]["PlineId"].ToString();
                    PlineCd = dtcarproinfo.Rows[0]["PlineCd"].ToString();
                    PlineNm = dtcarproinfo.Rows[0]["PlineNm"].ToString();
                    ToAVIId = dtcarproinfo.Rows[0]["ToAVIId"].ToString();
                    ToAVICd = dtcarproinfo.Rows[0]["ToAVICd"].ToString();
                    ToAVINm = dtcarproinfo.Rows[0]["ToAVINm"].ToString();
                    newCarPastRecordInfo.PlineId = PlineId;
                    newCarPastRecordInfo.PlineCd = PlineCd;
                    newCarPastRecordInfo.PlineNm = PlineNm;
                    newCarPastRecordInfo.PastDate = dtcarproinfo.Rows[0]["PastDate"].ToString();
                    newCarPastRecordInfo.PastTime = Convert.ToDateTime(dtcarproinfo.Rows[0]["PastTime"].ToString());
                    newCarPastRecordInfo.PastNo = dtcarproinfo.Rows[0]["PastNo"].ToString();
                    newCarPastRecordInfo.Rem = dtcarproinfo.Rows[0]["Rem"].ToString();

                }
                //查找计划信息
                string str_3 = "select * from P_ProducePlan_Pro where VIN='" + VIN + "' and IsFile='0' and Enabled = '1'";
                DataTable planbase = DbHelperSQL.OpenTable(str_3);
                if (planbase.Rows.Count > 0)
                {
                    OrderCd = planbase.Rows[0]["OrderCd"].ToString();
                    PlanTime = planbase.Rows[0]["PlanTime"].ToString();
                }

                string str_2 = "select * from BBdbR_AVIBase where AviId='" + AviId + "' and Enabled = '1'";
                DataTable AviInfo = DbHelperSQL.OpenTable(str_2);
                newCarPastRecordInfo.CarPastRecordId = Guid.NewGuid().ToString();
                newCarPastRecordInfo.AviId = AviId;
                newCarPastRecordInfo.AviCd = AviInfo.Rows[0]["AviCd"].ToString();
                newCarPastRecordInfo.AviNm = AviInfo.Rows[0]["AviNm"].ToString();


                newCarPastRecordInfo.VIN = VIN;

                newCarPastRecordInfo.PastType = 1;//自动

                newCarPastRecordInfo.RecordType = 0;//自动录入
                newCarPastRecordInfo.SpecialTag = 0;//未特殊标识
                newCarPastRecordInfo.CreTm = PastStrtTm;
                newCarPastRecordInfo.CreStaff = ManageProvider.Provider.Current().Account;
                newCarPastRecordInfo.IsBack = 0; //0-未反馈

                //过点信息写入数据库，更新车身队列，过点信息反馈至工厂FAS通过数据反馈处理服务完成
                string sqlinsertpastinfo = "insert into P_CarPastRecordInfo(CarPastRecordId,AviId,AviCd,AviNm,AviCatg,AviType,BodyNo,CarType,MatCd,CarColor1,VIN,SequenceNo,ProducePlanCd,CarRoute,PlineId,PlineCd,PlineNm,PastType,PastDate,PastTime,PastNo,RecordType,SpecialTag,CreTm,CreStaff,IsBack) values";
                sqlinsertpastinfo += " ('" + newCarPastRecordInfo.CarPastRecordId + "','" + newCarPastRecordInfo.AviId + "','" + newCarPastRecordInfo.AviCd + "','" + newCarPastRecordInfo.AviNm + "','" + newCarPastRecordInfo.AviCatg + "','" + newCarPastRecordInfo.AviType + "','" + newCarPastRecordInfo.BodyNo + "','" + newCarPastRecordInfo.CarType + "','" + newCarPastRecordInfo.MatCd + "','" + newCarPastRecordInfo.CarColor1 + "','" + newCarPastRecordInfo.VIN + "','" + newCarPastRecordInfo.SequenceNo + "','" + newCarPastRecordInfo.ProducePlanCd + "','" + newCarPastRecordInfo.CarRoute + "','" + newCarPastRecordInfo.PlineId + "','" + newCarPastRecordInfo.PlineCd + "','" + newCarPastRecordInfo.PlineNm + "','" + newCarPastRecordInfo.PastType + "','" + newCarPastRecordInfo.PastDate + "','" + newCarPastRecordInfo.PastTime + "','" + newCarPastRecordInfo.PastNo + "','" + newCarPastRecordInfo.RecordType + "','" + newCarPastRecordInfo.SpecialTag + "','" + newCarPastRecordInfo.CreTm + "','" + newCarPastRecordInfo.CreStaff + "','" + newCarPastRecordInfo.IsBack + "')";
                int extrow = DbHelperSQL.ExecuteSql(sqlinsertpastinfo);
                if (extrow == 1)
                {
                    //查找产线信息
                    string str_22 = "select * from BBdbR_PlineBase where PlineId='" + PlineId + "' and Enabled = '1'";
                    DataTable plinebase = DbHelperSQL.OpenTable(str_22);
                    if (plinebase.Rows.Count > 0)
                    {
                        PlineTyp = plinebase.Rows[0]["PlineTyp"].ToString();
                    }

                    //车身过点记录成功
                    //新增车身队列（拉入拉出车新增）

                    int carcount = 1;

                    string sql_que = "select isnull(max(CarQuene),0) from P_LineProductionQueue_Pro WITH(NOLOCK) where AviId = '" + AviId + "' ";
                    DataTable maxtable = DbHelperSQL.OpenTable(sql_que);//找到当日过点记录的个数，加1则为本次过点的顺序
                    carcount = Convert.ToInt32(maxtable.Rows[0][0].ToString()) + 1;


                    //string sqlupdateoldqueue = "update P_LineProductionQueue_Pro set CarQuene=CarQuene+1 where AVIId='" + AviId + "'";

                    ArrayList sqllist = new ArrayList();

                    string sqlinsertqueue = "";

                    string sql_line = "select PlineQueID from P_LineProductionQueue_Pro WITH(NOLOCK) where VIN ='" + VIN + "' ";
                    DataTable linetable = DbHelperSQL.OpenTable(sql_line);
                    if (linetable.Rows.Count > 0)//如果队列表中有则更新
                    {
                        sqlinsertqueue = "update P_LineProductionQueue_Pro set PlineId='" + newCarPastRecordInfo.PlineId + "', PlineCd='" + newCarPastRecordInfo.PlineCd + "',PlineNm='" + newCarPastRecordInfo.PlineNm + "',PlineTyp='" + PlineTyp + "', AVIId='" + AviId + "', AVICd='" + AviInfo.Rows[0]["AviCd"].ToString() + "',AVINm='" + AviInfo.Rows[0]["AVINm"].ToString() + "', ToAVIId='" + ToAVIId + "', ToAVICd='" + ToAVICd + "',ToAVINm='" + ToAVINm + "', CarQuene='" + carcount + "', PastNo='" + newCarPastRecordInfo.PastNo + "',UpdateTime='" + DateTime.Now + "' where PlineQueID ='" + linetable.Rows[0][0].ToString() + "'";

                        return DbHelperSQL.ExecuteSql(sqlinsertqueue); //同时执行，实现事务回滚
                    }
                    else
                    {
                        return 1;
                    }

                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion



        #endregion

        #region 查找某质控点组中所有质控点
        [HttpPost]
        [ValidateInput(false)]
        public DataTable getQCPByQCPGroup(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();
            strSql.Append(@"select a.WcId as QualityCheckPointId,a.WcNm as QualityCheckPointNm from BBdbR_WcBase a left join BBdbR_PlineBase b on a.PlineId = b.PlineId  where b.PlineCd in " + KeyValue + " and a.Enabled = '1'and b.Enabled = '1';");
            dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
            try
            {
                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }

        }
        #endregion

        #region 查询当前车辆是否在该质控点有数据
        [HttpPost]
        [ValidateInput(false)]
        public DataTable IsCarCheakedQCP(string VIN, string QualityCheckPointId)
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();
            strSql.Append(@"select QualityCheckPointNm,VIN,QualityResult from Q_CarQualityResult_Pro where VIN = '" + VIN + "' and QualityCheckPointId = '" + QualityCheckPointId + "' and isFile = 0 and Enabled = '1';");
            dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
            try
            {

                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
        }
        #endregion

        #region 查询当前车辆是否有不合格项
        [HttpPost]
        [ValidateInput(false)]
        public DataTable IsCarOutPutQCP(string VIN, string QualityCheckPointId)
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();
            strSql.Append(@"select QualityCheckPointNm,VIN,OutputResult,ReinspectionNumber from Q_CarQualityOutput_Pro where VIN = '" + VIN + "' and QualityCheckPointId = '" + QualityCheckPointId + "' and (OutputResult = '不合格 'or OutputResult = '维修完成 ' )  and isFile = 0 and Enabled = '1';");
            dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
            try
            {

                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }

        }
        #endregion

    }
}
