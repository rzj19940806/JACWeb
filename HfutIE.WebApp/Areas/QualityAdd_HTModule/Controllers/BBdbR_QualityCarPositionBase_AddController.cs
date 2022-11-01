using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.QualityAdd_HTModule.Controllers
{
    /// <summary>
    /// 车身部位基础信息表控制器
    /// </summary>
    public class BBdbR_QualityCarPositionBase_AddController : PublicController<BBdbR_QualityCarPositionBase_Add>
    {
        #region 创建数据库操作对象区域
        //创建数据库访问对象，用以访问其中操作数据库的方法
        BBdbR_QualityCarPositionBaseBll_Add MyBll = new BBdbR_QualityCarPositionBaseBll_Add(); //===复制时需要修改===
        BBdbR_QualityCarPositionGroupBaseBll_Add MyBll1 = new BBdbR_QualityCarPositionGroupBaseBll_Add();//车身方位分组信息表
        public readonly RepositoryFactory<BBdbR_QualityCarPositionBase_Add> repository_QualityCarPositionBase = new RepositoryFactory<BBdbR_QualityCarPositionBase_Add>();
        #endregion

        #region 打开视图区域
        /// <summary>
        /// 产品配置
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckConfigForm()
        {
            return View();
        }
        public ActionResult CarTypeConfigForm()//打开车型配置
        {
            return View();
        }
        #endregion

        #region 方法区   

        #region 2.新增编辑方法
        //entity表示页面中传入的实体，KeyValue表示传入的主键
        //不管是新增还是编辑，页面中都会传入实体（entity）
        //新增时实体是一个全新的实体
        //编辑时实体是修改后的实体
        //只有在编辑时页面中才会传入主键entity（KeyValue），该主键是需要编辑那个实体的主键
        //
        //编辑方法首先根据KeyValue是否有值判断是需要编辑还是需要新增
        //如果是新增就将该实体新增到数据库中
        //如果是编辑就将该实体更新到数据中
        //
        //不管是新增还是编辑首先判断页面输入的编号是否已经存在
        //如果已经存在就直接返回“该编号已经存在！”的信息
        //不存在再进行下一步
        public override ActionResult SubmitForm(BBdbR_QualityCarPositionBase_Add entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "CarPositionCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.CarPositionCd;  //页面中的编号字段值                 //===复制时需要修改===\
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_QualityCarPositionBase_Add Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    
                    entity.Modify(KeyValue);
                    entity.Type = Oldentity.Type;
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
                    entity.Type = "CH";
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

        #region 3.删除方法
        /// <summary>
        /// 首先判断需要删除的信息是否绑定了其他信息
        ///     如：删除一条公司信息先要判断该条公司信息下面是否绑定了工厂信息
        ///         如果绑定了信息，则提示“当前所选有子节点数据，不能删除。”并结束
        ///  在确定没绑定数据的情况下，删除该数据
        ///     删除表示将该数据的IsAvailable属性设为false，并不真的删除该记录
        /// </summary>
        /// <param name="KeyValue">页面中提供的需要删除的数据的主键,可能是多条数据的主键，即多个主键</param>
        /// <param name="ParentId">不需要</param>
        /// <param name="DeleteMark">不需要</param>
        /// <returns></returns>
        public override ActionResult Delete(string KeyValue, string ParentId, string DeleteMark)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功  
                
                for (int i = 0; i < array.Length; i++) //先把对应CarPartId中所有检查项目均删除
                {
                    BBdbR_QualityCarPositionBase_Add Oldentity = repositoryfactory.Repository().FindEntity(array[i]);
                    int num = MyBll.hasChildNode(Oldentity.CarPositionId);
                    if (num > 0)
                    {
                        throw new Exception("当前所选有项有子节点数据，不能删除");
                    }
                    else
                    {
                        IsOk = MyBll.Delete(array[i]);
                        if (IsOk > 0) Message = "删除成功。";
                    }
                    
                }
                WriteLog(IsOk, array, Message);//记录日志
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);//记录日志
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 4.查询方法
        

        public ActionResult GridPageByCondition(string CarPositionCd, string CarPositionNm, JqGridParam jqgridparam)
        {
            try
            {
                #region 原方法
                //string keyword = keywords.Trim();
                //Stopwatch watch = CommonHelper.TimerStart();
                //List<BBdbR_QualityCarPositionBase_Add> ListData = MyBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
                //var JsonData = new
                //{
                //    total = jqgridparam.total,
                //    page = jqgridparam.page,
                //    records = jqgridparam.records,
                //    costtime = CommonHelper.TimerEnd(watch),
                //    rows = ListData,
                //};
                //return Content(ListData.ToJson());
                #endregion

                #region 查询修改
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select * from BBdbR_QualityCarPositionBase_Add where  Enabled = '1' and type='CH'");

                #region 判断输入框内容添加检索条件
                //是否加检验岗编号模糊搜索
                //if (CarPositionCd != "" && CarPositionCd != null)
                //{
                //    strSql.Append(" and CarPositionCd like '%" + CarPositionCd + "%'");
                //}
                //else { }

                ////是否加检验岗名称模糊搜索
                //if (CarPositionNm != "" && CarPositionNm != null)
                //{
                //    strSql.Append(" and CarPositionNm like '%" + CarPositionNm + "%'");
                //}
                //else { }


                List<DbParameter> parameter = new List<DbParameter>();
                
                if (!String.IsNullOrEmpty(CarPositionCd))
                {
                    strSql.Append(" and CarPositionCd like @CarPositionCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarPositionCd", "%" + CarPositionCd + "%"));
                }
                if (!String.IsNullOrEmpty(CarPositionNm))
                {
                    strSql.Append(" and CarPositionNm like @CarPositionNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarPositionNm", "%" + CarPositionNm + "%"));
                }



                #endregion

                //按照检验岗编号排序
                strSql.Append(" order by CarPositionCd desc");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "检验岗信息查询成功");
                return Content(JsonData.ToJson());
                #endregion
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "检验岗信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 5.检查字段是否唯一
        /// <summary>
        /// 根据传入的字段名和字段值判断该字段是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回该判断结果</returns>
        public ActionResult ChectOnlyOne(string KeyName, string KeyValue)
        {
            try
            {
                int IsOk = 0;
                string Message = "该字段已经存在！";

                if (!string.IsNullOrEmpty(KeyValue))
                {
                    IsOk = MyBll.CheckCount(KeyName, KeyValue);
                }
                if (IsOk > 0)
                {
                    return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        #region 6.导入
        ///// <summary>
        ///// 导入Excel弹出框页面
        ///// </summary>
        ///// <returns></returns>
        //[ManagerPermission(PermissionMode.Enforce)]
        //public ActionResult ExcelImportDialog()
        //{
        //    string moduleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
        //    //模板主表
        //    Base_ExcelImport base_excellimport = DataFactory.Database().FindEntity<Base_ExcelImport>("ModuleId", moduleId);
        //    if (base_excellimport.ModuleId != null)
        //    {
        //        ViewBag.ModuleId = moduleId;
        //        ViewBag.ImportFileName = base_excellimport.ImportFileName;
        //        ViewBag.ImportName = base_excellimport.ImportName;
        //        ViewBag.ImportId = base_excellimport.ImportId;
        //    }
        //    else
        //    {
        //        ViewBag.ModuleId = "0";
        //    }
        //    //ViewBag.ID = Request.Params["ID"];
        //    //ID1 = ViewBag.ID;
        //    //ViewBag.OrderID = Request.Params["OrderID"];
        //    //OrderID1 = ViewBag.OrderID;
        //    return View();
        //}
        //#region 导出模板
        ///// <summary>
        ///// 下载Excell模板
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult GetExcellTemperature(string ImportId)
        //{
        //    if (!string.IsNullOrEmpty(ImportId))
        //    {
        //        DataSet ds = new DataSet();
        //        DataTable data = new DataTable(); string DataColumn = ""; string fileName;
        //        MyBll.GetExcellTemperature(ImportId, out data, out DataColumn, out fileName);
        //        ds.Tables.Add(data);
        //        MemoryStream ms = DeriveExcel.ExportToExcel(ds, "xls", DataColumn.Split('|'));
        //        if (!fileName.EndsWith(".xls"))
        //        {
        //            fileName = fileName + ".xls";
        //        }
        //        return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //#endregion
        ///// <summary>
        ///// 清除Datatable空行
        ///// </summary>
        ///// <param name="dt"></param>
        //public void RemoveEmpty(DataTable dt)
        //{
        //    List<DataRow> removelist = new List<DataRow>();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        bool rowdataisnull = true;
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
        //            {

        //                rowdataisnull = false;
        //            }
        //        }
        //        if (rowdataisnull)
        //        {
        //            removelist.Add(dt.Rows[i]);
        //        }

        //    }
        //    for (int i = 0; i < removelist.Count; i++)
        //    {
        //        dt.Rows.Remove(removelist[i]);
        //    }
        //}

        ///// <summary>
        ///// 导入Excell数据
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult ImportExel()
        //{
        //    int IsOk = 0;//导入状态
        //    int IsCheck = 1;//用作检验重复地址的标识
        //    DataTable Result = new DataTable();//导入错误记录表
        //    IDatabase database = DataFactory.Database();
        //    List<BBdbR_CarPartBase> EntityList = new List<BBdbR_CarPartBase>();

        //    //构造导入返回结果表
        //    DataTable Newdt = new DataTable("Result");
        //    Newdt.Columns.Add("rowid", typeof(System.String));                 //行号
        //    Newdt.Columns.Add("locate", typeof(System.String));                 //位置
        //    Newdt.Columns.Add("reason", typeof(System.String));                 //原因
        //    int errorNum = 1;
        //    try
        //    {
        //        string moduleId = Request["moduleId"]; //表名
        //        //StringBuilder sb_table = new StringBuilder();
        //        HttpFileCollectionBase files = Request.Files;
        //        HttpPostedFileBase file = files["filePath"];//获取上传的文件
        //        string fullname = file.FileName;
        //        string IsXls = System.IO.Path.GetExtension(fullname).ToString().ToLower();//System.IO.Path.GetExtension获得文件的扩展名
        //        if (!IsXls.EndsWith(".xls") && !IsXls.EndsWith(".xlsx"))
        //        {
        //            IsOk = 0;
        //        }
        //        else
        //        {

        //            string filename = Guid.NewGuid().ToString() + ".xls";
        //            if (fullname.EndsWith(".xlsx"))
        //            {
        //                filename = Guid.NewGuid().ToString() + ".xlsx";
        //            }
        //            if (file != null && file.FileName != "")
        //            {
        //                string msg = UploadHelper.FileUpload(file, Server.MapPath("~/Resource/UploadFile/ImportExcel/"), filename);
        //            }

        //            DataTable dt = ImportExcel.ExcelToDataTable(Server.MapPath("~/Resource/UploadFile/ImportExcel/") + filename);

        //            RemoveEmpty(dt);//清除空行
        //            dt.Columns.Add("rowid", typeof(System.String));//用来标识excell行ID
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                dt.Rows[i]["rowid"] = i + 1;
        //            }
        //            #region 车身部位信息导入
        //            //校验
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                IsCheck = 1;//重置标识
        //                DataRow dr = Newdt.NewRow();


        //                if (dt.Rows[i]["车身部位编号"].ToString().Trim() != "")
        //                {
        //                    int Count = MyBll.CheckCount("CarPartCd", dt.Rows[i]["车身部位编号"].ToString().Trim());//是否有相同编号
        //                    if (Count > 0)
        //                    {
        //                        dr = Newdt.NewRow();
        //                        dr[0] = errorNum;
        //                        dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[车身部位编号]";
        //                        dr[2] = "在系统中已存在不能重复插入";
        //                        Newdt.Rows.Add(dr);
        //                        errorNum++;
        //                        IsCheck = 0;
        //                        continue;
        //                    }
        //                    else
        //                    {

        //                        BBdbR_CarPartBase entity = new BBdbR_CarPartBase();
        //                        entity.CarPartCd = dt.Rows[i]["车身部位编号"].ToString().Trim();
        //                        entity.CarPartNm = dt.Rows[i]["车身部位名称"].ToString().Trim();

        //                        entity.CarPartDsc = dt.Rows[i]["车身部位描述"].ToString().Trim();
        //                        entity.Rem = dt.Rows[i]["备注"].ToString().Trim();

        //                        entity.Create();
        //                        //Device.DvcMDt = DateTime.ParseExact(dt.Rows[i]["设备制造日期"].ToString().Trim(), "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);

        //                        EntityList.Add(entity);
        //                        int b = database.Insert(EntityList);
        //                        if (b > 0)
        //                        {
        //                            IsOk = IsOk + b;
        //                            EntityList.Clear();
        //                        }
        //                        else
        //                        {
        //                            dr = Newdt.NewRow();
        //                            dr[0] = errorNum;
        //                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
        //                            dr[2] = "车身部位信息插入失败";
        //                            Newdt.Rows.Add(dr);
        //                            IsCheck = 0;
        //                            continue;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    dr = Newdt.NewRow();
        //                    dr[0] = errorNum;
        //                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
        //                    dr[2] = "车身部位编号不能为空";
        //                    Newdt.Rows.Add(dr);
        //                    errorNum++;
        //                    IsCheck = 0;
        //                    continue;
        //                }
        //            }
        //            if (IsCheck == 0)
        //            {
        //                IsOk = 0;
        //            }
        //            #endregion

        //        }
        //        Result = Newdt;
        //    }
        //    catch (Exception ex)
        //    {
        //        Base_SysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "异常错误：" + ex.Message);
        //        IsOk = 0;
        //    }
        //    if (Result.Rows.Count > 0)
        //    {
        //        IsOk = 0;
        //    }
        //    var JsonData = new
        //    {
        //        Status = IsOk > 0 ? "true" : "false",
        //        ResultData = Result
        //    };
        //    return Content(JsonData.ToJson());
        //}
        #endregion

        #region 重构导出
        public ActionResult GetExcel_Data(string CarPositionCd, string CarPositionNm, JqGridParam jqgridparam)
        {
            try
            {


                #region 根据当前搜索条件查出数据并导出
                
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select CarPositionCd,CarPositionNm,Dsc,CreTm,CreNm,MdfTm,MdfNm,Rem from BBdbR_QualityCarPositionBase_Add where  Enabled = '1'and type='CH'");

                #region 判断输入框内容添加检索条件
                //是否加检验岗编号模糊搜索
                //if (CarPositionCd != "" && CarPositionCd != null)
                //{
                //    strSql.Append(" and CarPositionCd like '%" + CarPositionCd + "%'");
                //}
                //else { }

                ////是否加检验岗名称模糊搜索
                //if (CarPositionNm != "" && CarPositionNm != null)
                //{
                //    strSql.Append(" and CarPositionNm like '%" + CarPositionNm + "%'");
                //}
                //else { }
                List<DbParameter> parameter = new List<DbParameter>();

                if (!String.IsNullOrEmpty(CarPositionCd))
                {
                    strSql.Append(" and CarPositionCd like @CarPositionCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarPositionCd", "%" + CarPositionCd + "%"));
                }
                if (!String.IsNullOrEmpty(CarPositionNm))
                {
                    strSql.Append(" and CarPositionNm like @CarPositionNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarPositionNm", "%" + CarPositionNm + "%"));
                }



                #endregion

                //按照检验岗编号排序
                strSql.Append(" order by CarPositionCd desc");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                #endregion



                string fileName = "检验岗基础信息";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_QualityCarPosition(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "检验岗信息导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "检验岗信息导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion
        #endregion
    }
}