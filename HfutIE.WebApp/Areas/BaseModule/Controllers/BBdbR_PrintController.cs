using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// _FactoryBaseInformation控制器
    /// </summary>
    public class BBdbR_PrintController : PublicController<BBdbR_Print>
    {

        #region 创建数据库操作对象区域
        BBdbR_PrintBll MyBll = new BBdbR_PrintBll(); //===复制时需要修改===
        BBdbR_PrintConfigBll MyBll2 = new BBdbR_PrintConfigBll();
        public readonly RepositoryFactory<BBdbR_Print> repository_print = new RepositoryFactory<BBdbR_Print>();
        public readonly RepositoryFactory<BBdbR_PrintConfig> repository_printconfig = new RepositoryFactory<BBdbR_PrintConfig>();
        #endregion

        #region 打开视图区域
        public ActionResult Form2()//打印配置编辑界面
        {
            return View();
        }
        #endregion

        #region 方法区   

        #region 1.查询方法

        public ActionResult GridPageByCondition(string PrintCd, string PrintType, JqGridParam jqgridparam)
        {
            try
            {
                #region datatable查询并传回前端
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();

                strSql.Append(@"select * from BBdbR_Print where (Enabled = 1 or Enabled = 2)");

                List<DbParameter> parameter = new List<DbParameter>();
                if (PrintCd != "" && PrintCd != null)
                {
                    //strSql.Append(" and PrintCd like '%" + PrintCd + "%'");
                    strSql.Append(" and PrintCd like @PrintCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@PrintCd", "%" + PrintCd + "%"));
                }
                else { }
                if (PrintType != "" && PrintType != null)
                {
                    //strSql.Append(" and PrintType like '%" + PrintType + "%'");
                    strSql.Append(" and PrintType like @PrintType ");
                    parameter.Add(DbFactory.CreateDbParameter("@PrintType", "%" + PrintType + "%"));
                }
                else { }

                //排序
                strSql.Append(" order by PrintCd");

                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                return Content(JsonData.ToJson());
                #endregion

            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 2.点击打印上表，展示下表打印配置表格信息
        
        public ActionResult GridPageJson2(string KeyValue, JqGridParam jqgridparam)
        {
            try
            {
                #region datatable查询并传回前端
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select * from BBdbR_PrintConfig where Enabled = 1 and PridId = '" + KeyValue + "' order by Id");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                return Content(JsonData.ToJson());
                #endregion

            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion
        
        #region 3.新增编辑方法-打印
        public ActionResult SubmitForm1(BBdbR_Print entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "PrintCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.PrintCd;  //页面中的编号字段值                 //===复制时需要修改===
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_Print Oldentity = repository_print.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.Modify(KeyValue);
                    if (entity.Rem == null)
                    {
                        entity.Rem = "";
                    }
                    else { }
                    IsOk = MyBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志
                }
                else//新增操作
                {
                    IsOk = MyBll.CheckCount(Name, Value);//判断页面中填写的数据的编号字段的值是否存在
                    if (IsOk > 0)//存在
                    {
                        Message = "该编号已经存在！";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
                    entity.Id = System.Guid.NewGuid().ToString();
                    entity.Create();
                    IsOk = MyBll.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);//记录日志
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);//记录日志
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 3.新增编辑方法-打印配置
        public ActionResult SubmitForm2(BBdbR_PrintConfig entity, string KeyValue,string PridId)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "PrintConfigCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.PrintConfigCd;  //页面中的编号字段值                 //===复制时需要修改===
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                
                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_PrintConfig Oldentity = repository_printconfig.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.Modify(KeyValue);
                    #region 编辑时为空，传回为null，为null时无法更新
                    if (entity.Rem == null)
                    {
                        entity.Rem = "";
                    }
                    else { }
                    if (entity.MX == null)
                    {
                        entity.MX = "";
                    }
                    else { }
                    if (entity.MY == null)
                    {
                        entity.MY = "";
                    }
                    else { }
                    if (entity.MWidth == null)
                    {
                        entity.MWidth = "";
                    }
                    else { }
                    if (entity.MHeight == null)
                    {
                        entity.MHeight = "";
                    }
                    else { }
                    #endregion
                    IsOk = MyBll2.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志
                }
                else//新增操作
                {
                    IsOk = MyBll2.CheckCount(Name, Value);//判断页面中填写的数据的编号字段的值是否存在
                    if (IsOk > 0)//存在
                    {
                        Message = "该编号已经存在！";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
                    entity.Create();
                    IsOk = MyBll2.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);//记录日志
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);//记录日志
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 写入作业日志
        /// <summary>
        /// 写入作业日志（新增、修改）
        /// </summary>
        /// <param name="IsOk">操作状态</param>
        /// <param name="entity">实体对象</param>
        /// <param name="Oldentity">之前实体对象</param>
        /// <param name="Message">备注信息</param>
        public void WriteLog(int IsOk, BBdbR_PrintConfig entity, BBdbR_PrintConfig Oldentity, string KeyValue, string Message = "")
        {
            if (!string.IsNullOrEmpty(KeyValue))
            {
                Base_SysLogBll.Instance.WriteLog(Oldentity, entity, OperationType.Update, IsOk.ToString(), Message);
            }
            else
            {
                Base_SysLogBll.Instance.WriteLog(entity, OperationType.Add, IsOk.ToString(), Message);
            }
        }
        #endregion

        #region 删除方法

        public ActionResult DeletePrint(string KeyValue)//删除打印基础信息
        {
            #region 删除打印基础信息
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');

            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功

                IsOk = MyBll.Delete(array);//执行删除操作
                for (int i = 0; i < array.Length; i++)  //删除下级配置
                {

                    List<BBdbR_PrintConfig> ListData = MyBll2.GetList(array[i]);
                    string[] array2 = new string[ListData.Count];
                    if (ListData.Count > 0)
                    {
                        for (int j = 0; j < ListData.Count; j++)
                        {
                            array2[j] = ListData[j].Id;
                        }
                        MyBll2.Delete(array2);
                    }

                }
                if (IsOk > 0)//表示删除成功
                {
                    Message = "删除成功。";//修改返回信息
                }
                WriteLog(IsOk, array, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        #region 删除打印配置信息
        public ActionResult DeletePrintConfig(string KeyValue2)//删除物料文档配置
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue2.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功
                IsOk = MyBll2.Delete(array);//执行删除操作
                if (IsOk > 0)//表示删除成
                {
                    Message = "删除成功。";//修改返回信息
                }
                WriteLog(IsOk, array, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #endregion


        #region 7.检查字段是否唯一
        /// <summary>
        /// 根据传入的字段名和字段值判断该字段是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回该判断结果</returns>
        //public ActionResult ChectOnlyOne(string KeyName, string KeyValue)
        //{
        //    try
        //    {
        //        int IsOk = 0;
        //        string Message = "该字段已经存在！";

        //        if (!string.IsNullOrEmpty(KeyValue))
        //        {
        //            IsOk = MyBll.CheckCount(tableName,KeyName);
        //        }
        //        if (IsOk > 0)
        //        {
        //            return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
        //    }
        //}

        #endregion

        #endregion

        #region 编辑获取实体

        #region 打印Form获取编辑实体
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetPrintForm(string KeyValue)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select * from BBdbR_Print where Id = '" + KeyValue + "'");
                DataTable ListData = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
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

        #region 打印配置Form获取编辑实体
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetPrintConfigForm(string KeyValue)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select a.*,b.PrintCd,b.PrintType from BBdbR_PrintConfig a left join BBdbR_Print b on a.PridId = b.Id where a.Id = '" + KeyValue + "'");
                DataTable ListData = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
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


        #endregion

        #region 物料文档配置获取实体-用于form2获取物料编号
        ///// <summary>
        ///// 获取物料信息对象返回JSON
        ///// </summary>
        ///// <param name="KeyValue">主键值</param>
        ///// <returns></returns>
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult SetMatForm2(string KeyValue)
        //{
        //    try
        //    {
        //        DataTable ListData = MyBll.GetMatInfor(KeyValue);
        //        var JsonData = new
        //        {
        //            rows = ListData,
        //        };
        //        //var s = JsonData.ToJson();
        //        return Content(JsonData.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
        //    }


        //}

        #endregion

        #region 提交物料文档配置表单
        /// <summary>
        /// 提交物料文档配置表单
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        //public ActionResult SubmitPicture2(string KeyValue, BBdbR_MatFileConfig entity2, string file)//提交图片
        //{
        //    try
        //    {
        //        int IsOk = 0;//用于判断
        //        string Name = "FileCd";
        //        string Value = entity2.FileCd;  //页面中的编号字段值                 //===复制时需要修改===
        //        string Message = "" ;
        //            IsOk = MyBll2.CheckCount(Name, Value);//判断页面中填写的数据的编号字段的值是否存在
        //            if (IsOk > 0)//存在
        //            {
        //                Message = "该编号已经存在！";
        //                return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
        //            }
        //        //新增操作
        //            entity2.Create();
        //            var files = System.Web.HttpContext.Current.Request.Files;
        //            var file1 = files[0].InputStream;
        //            byte[] buff = new byte[file1.Length];
        //            entity2.MatId = KeyValue;
        //            int a = MyBll2.Insert(entity2);//将实体插入数据库，插入成功返回1，失败返回0；
        //            if (a > 0)
        //            {
        //                byte[] photograph = null;
        //                string photograph_type = "";
        //                photograph = GetPhotographFromRequest(out photograph_type);//获取图片的二进制和图片后缀名，方法在下面
        //                NameValueCollection forms = Request.Form;
        //                IsOk = MyBll2.InsertPicture(entity2.FileCd, photograph, photograph_type);//将实体插入数据库，插入成功返回1，失败返回0；
        //                if (IsOk > 0)
        //                {
        //                    Message = "新增配置信息成功！";
        //                }
        //                else
        //                {
        //                    Message = "新增配置信息失败！";
        //                }
        //            }

        //        return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());

        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
        //    }
        //}


        //public ActionResult SubmitDocument(string KeyValue, string MatId, string FileCd, string FileTy, string Enabled, string Rem, string file,string path2)//提交文件
        //{
        //    try
        //    {
        //        int IsOk = 0;//用于判断
        //        string Name = "FileCd";
        //        string Value = FileCd;  //页面中的编号字段值                 //===复制时需要修改===
        //        string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
        //        //path2 = @"C:\Users\Administrator\Desktop";

        //        IsOk = MyBll2.CheckCount(Name, Value);//判断页面中填写的数据的编号字段的值是否存在
        //        if (IsOk > 0)//存在
        //        {
        //            Message = "该编号已经存在！";
        //            return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
        //        }
        //        //新增操作
        //        //先将实体插入数据库
        //        BBdbR_MatFileConfig entity = new BBdbR_MatFileConfig();
        //        entity.ConfigId = System.Guid.NewGuid().ToString();
        //        entity.MatId = MatId;
        //        entity.FileCd = FileCd;
        //        entity.FileTy = FileTy;
        //        entity.Enabled = Enabled;
        //        entity.Rem = Rem;
        //        int a = MyBll2.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
        //        if (a > 0)
        //        {
        //           // byte[] document = null;
        //            byte[] document2 = null;
        //            //string document_type = "";
        //            document2 = DocumentToByte(path2);
        //            //document = GetDocumentFromRequest(out document_type);//获取文件的二进制和文件后缀名，方法在下面
        //            NameValueCollection forms = Request.Form;
        //            IsOk = MyBll2.InsertDocument(FileCd, document2);//将实体插入数据库，插入成功返回1，失败返回0；
        //            if (IsOk > 0)
        //            {
        //                Message = "新增配置信息成功！";
        //            }
        //            else
        //            {
        //                Message = "新增配置信息失败！";
        //            }
        //        }

        //        return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());

        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
        //    }
        //}
        #endregion

        #region 判断是否为16进制
        //如果存在返回关重件工位名称
        public ActionResult If16ToInt(string ARGB)
        {
            try
            {
                #region 将16进制转换为int
                int result2 = Convert.ToInt32(ARGB, 16);
                #endregion
                return Content("\"" + result2.ToJson() + "\"");

            }
            catch (Exception ex)
            {
                return Content("\"" + "unfind" + "\"");
            }
        }
        #endregion

        #region int类型转换为16进制字符串
        //如果存在返回关重件工位名称
        public ActionResult IntTo16(int ARGB)
        {
            try
            {
                #region 将16进制转换为int
                string result =  ARGB.ToString("X");
                #endregion
                return Content("\"" + result + "\"");

            }
            catch (Exception ex)
            {
                return Content("\"" + "unfind" + "\"");
            }
        }
        #endregion


    }
}