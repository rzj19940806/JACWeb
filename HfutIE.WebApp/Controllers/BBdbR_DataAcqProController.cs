using HfutIE.Business;
using HfutIE.Entity;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.AndonModule.Controllers
{
    /// <summary>
    /// Andon数据采集过程表控制器
    /// </summary>
    public class BBdbR_DataAcqProController :Controller
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_DataAcqPro";//===复制时需要修改===

        #endregion

        #region 创建数据库操作对象区域
        BBdbR_DataAcqProBll MyBll = new BBdbR_DataAcqProBll(); //===复制时需要修改===
        //BPIE_SLDocBll MyBll2 = new BPIE_SLDocBll();
        #endregion

        #region 打开视图区域

        [LoginAuthorize]
        public ActionResult AndonIndex()//打开子界面
        {
            return View();
        }
        public ActionResult StampIndex()//打开子界面
        {
            return View();
        }

        public ActionResult Form()//打开子界面
        {
            return View();
        }
        public ActionResult myindex()
        {
            return View();
        }

        #endregion

        #region  获取Andon补录信息
        /// <summary>
        ///获取Andon补录信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageJson1(string PADPlineId)
        {
            try
            {
                DataTable ListData = MyBll.GetAndonList("", PADPlineId);
                var JsonData = new
                {
                    rows = ListData,
                };
                string a = JsonData.ToJson();
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        # region 通过关键词获取Andon记录
        /// <summary>
        /// 获取物料信息对象返回JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GridPageJson2(string KeyValue,string SearchTime)
        {
            
                try
                {
                    DataTable ListData = MyBll.GetAndonListByCondition(KeyValue, SearchTime);
                    var JsonData = new
                    {
                        rows = ListData,
                    };
                    return Content(JsonData.ToJson());
                }
                catch (Exception ex)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
                }
            
        }
        #endregion
        
        #region 通过主键获取Andon记录
        /// <summary>
        /// 获取物料信息对象返回JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetForm(string KeyValue)
        {

            try
            {
                DataTable ListData = MyBll.GetAndonList(KeyValue,"");
                var JsonData = new
                {
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }

        }
        #endregion

        #region 编辑方法
        
        public  ActionResult SubmitForm(BBdbR_DataAcqPro entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                BBdbR_DataAcqPro entity2 = new BBdbR_DataAcqPro();
                int IsOk = 0;//用于判断
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                //===复制时需要修改===
                entity2.RecId = KeyValue;
                entity2.Rem = entity.Rem == "&nbsp;"  ?  "" : entity.Rem;
                entity2.PostId = entity.PostId == "&nbsp;" ? "" : entity.PostId;
                entity2.HandlerCode = entity.HandlerCode == "&nbsp;" ? "" : entity.HandlerCode;
                entity2.HandlerName = entity.HandlerName == "&nbsp;" ? "" : entity.HandlerName;
                entity2.ExceptionDes = entity.ExceptionDes == "&nbsp;" ? "" : entity.ExceptionDes;
                entity2.HandleResult = entity.HandleResult == "&nbsp;" ? "" : entity.HandleResult;
                entity2.ExceptionType = entity.ExceptionType == "&nbsp;" ? "" : entity.ExceptionType;
                entity2.Modify(KeyValue);
                    entity2.CurValue="已补录";
                    IsOk = MyBll.Update(entity2);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
               
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 得到前端设备IP
        public ActionResult GetIpAddress()
        {
            try
            {
                string IPAddress = NetHelper.GetIPAddress();
                return Content("\"" + IPAddress + "\"");
            }
            catch (Exception)
            {
                return Content("error");
            }
        }
        #endregion

        #region 通过IP获取产线Id和产线名称 
        /// <summary>
        /// 获取物料信息对象返回JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetPlineIdByIp(string IP)
        {

            try
            {
                DataTable ListData = MyBll.GetPlineIdByIp(IP);
                var JsonData = new
                {
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }

        }
        #endregion 

        #region Andon大屏
        #region  获取Andon计划信息
        /// <summary>
        ///获取计划信息
        /// </summary>
        /// <returns></returns>
        public ActionResult FindPlan()
        {
            try
            {
                DataTable ListData = MyBll.GetTodayPlan();
                var JsonData = new
                {
                    rows = ListData,
                };
                string a = JsonData.ToJson();
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion
        
        #region  获取Andon班组信息
        /// <summary>
        ///获取计划信息
        /// </summary>
        /// <returns></returns>
        public ActionResult FindClass()
        {
            try
            {
                DataTable ListData = MyBll.GetTodayClass();
                var JsonData = new
                {
                    rows = ListData,
                };
                string a = JsonData.ToJson();
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion
        
        #region  获取停线次数信息
        /// <summary>
        ///获取计划信息
        /// </summary>
        /// <returns></returns>
        public ActionResult FindNumber()
        {
            try
            {
                DataTable ListData = MyBll.GetTodayNumber();
                var JsonData = new
                {
                    rows = ListData,
                };
                string a = JsonData.ToJson();
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion
        
        #region 通过Id获取Name
        /// <summary>
        /// 获取物料信息对象返回JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetNameById(string PlineId, string WcId, string PostId)
        {

            try
            {
                DataTable ListData = MyBll.GetAndonNameById(PlineId,WcId,PostId);
                var JsonData = new
                {
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }

        }
        #endregion

        #region 通过工位Id获取岗位名称
        /// <summary>
        /// 获取物料信息对象返回JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FindPostByWc(string WcId)
        {

            try
            {
                DataTable ListData = MyBll.GetAndonPostNameByWcId(WcId);
                var JsonData = new
                {
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }

        }
        #endregion

        #region 通过产线Id获取产线名称
        /// <summary>
        /// 获取物料信息对象返回JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetPlineName(string PlineId)
        {

            try
            {
                DataTable ListData = MyBll.GetAndonPlineNameById(PlineId);
                var JsonData = new
                {
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }

        }
        #endregion
        
        //#region 新增一条停线记录
        ///// <summary>
        ///// 新增一条停线记录
        ///// </summary>
        ///// <param name="PlineId">产线Id</param>
        ///// <returns></returns>

        //public void InsertStopInformation(string PlineId, string PlineNm, string Source,string SignalTm)
        //{

        //    try
        //    {
        //        BPIE_SLDoc entity = new BPIE_SLDoc();
        //        entity.RecId = CommonHelper.GetGuid;
        //        entity.PlineId = PlineId;
        //        entity.PlineNm = PlineNm;
        //        entity.State = "1";
        //        entity.SLSource = Source;
        //        entity.SLStrtTm = SignalTm;
        //        entity.Enabled = "1";
        //        MyBll2.Insert(entity);

        //    }
        //    catch (Exception ex)
        //    {
               
        //    }

        //}
        //#endregion

        //#region 停线恢复记录
        ///// <summary>
        ///// 停线恢复记录
        ///// </summary>
        ///// <param name="PlineId">产线Id</param>
        ///// <returns></returns>

        //public void CompleteStopInformation(string PlineId, string PlineNm, string Source, string SignalTm)
        //{
        //    //获取主键
        //    string RecId = MyBll2.GetRecId(PlineId, Source);
        //    //BPIE_SLDoc BPIE_SLDocentity = repositoryfactory.Repository().FindEntity(RecId);//获取没更新之前实体对象
            
        //    try
        //    {
        //        BPIE_SLDoc entity = new BPIE_SLDoc();
        //        entity.RecId = RecId;
        //        entity.PlineId = PlineId;
        //        entity.PlineNm = PlineNm;
        //        entity.State = "0";
        //        entity.SLCmplTm = SignalTm;
        //        MyBll2.Update(entity);

        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}
        //#endregion
        
        #region 得到服务器IP
        public ActionResult GetIP()
        {
            //得到本机名 
            string hostname = Dns.GetHostName();
            //解析主机名称或IP地址的system.net.iphostentry实例。
            IPHostEntry localhost = Dns.GetHostEntry(hostname);
            if (localhost != null)
            {
                foreach (IPAddress item in localhost.AddressList)
                {
                    //判断是否是内网IPv4地址
                    if (item.AddressFamily == AddressFamily.InterNetwork)
                    {
                        
                        string ip= item.MapToIPv4().ToString();
                        return Content("\""+ip+"\"");
                    }
                }
            }
            return Content("error");
        }
        #endregion

       
        #endregion






    }
}