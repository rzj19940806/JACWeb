using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// _FactoryBaseInformation控制器
    /// </summary>
    public class BBdbR_StfBaseController : PublicController<BBdbR_StfBase>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_StfBase";
        public static DataTable StfList = new DataTable();
        #endregion

        #region 创建数据库操作对象区域
        //创建数据库访问对象，用以访问其中操作数据库的方法
        BBdbR_StfBaseBll MyBll = new BBdbR_StfBaseBll(); //===复制时需要修改===
        public readonly RepositoryFactory<BBdbR_StfBase> repository_StfBase = new RepositoryFactory<BBdbR_StfBase>();
        #endregion

        #region 方法区   

        #region 1.获取树
        public ActionResult TreeJson()
        {
            try
            {
                DataTable dt = MyBll.GetTree();//获取树所需数据
                List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
                if (DataHelper.IsExistRows(dt))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string area_key = row["keys"].ToString();
                        bool hasChildren = false;
                        DataTable childnode = DataHelper.GetNewDataTable(dt, "parentid='" + area_key + "'");
                        if (childnode.Rows.Count > 0)
                        {
                            hasChildren = true;
                        }
                        TreeJsonEntity tree = new TreeJsonEntity();
                        tree.id = area_key;
                        tree.text = row["name"].ToString();
                        tree.value = row["code"].ToString();
                        tree.parentId = row["parentId"].ToString();
                        tree.Attribute = "Type";
                        tree.AttributeValue = row["sort"].ToString();
                        tree.isexpand = true;
                        tree.complete = true;
                        tree.hasChildren = hasChildren;
                        if (row["parentid"].ToString() == "0")
                        {

                            tree.img = "/Content/Images/Icon16/house.png";
                        }
                        else if (row["parentid"].ToString() != "0")
                        {
                            tree.img = "/Content/Images/Icon16/factory.png";
                        }
                        TreeList.Add(tree);
                    }
                }
                return Content(TreeList.TreeToJson());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 2.点击树展示表格
        /// <summary>
        /// 根据点击树的节点在数据库中查询相应的信息
        /// </summary>
        /// <param name="areaId">点击的节点的主键</param>
        /// <param name="parentId">节点的父级主键</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GridListJson(string areaId, string parentId, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DepartmentStaffConfigViewBll ConfigBll = new DepartmentStaffConfigViewBll();
                //List<DepartmentStaffConfigView> ListData = ConfigBll.GetList(areaId, parentId, ref jqgridparam);//===复制时需要修改===
                StfList = ConfigBll.GetList(areaId, parentId, ref jqgridparam);//===复制时需要修改===
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = StfList,
                };
                return Json(JsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion      

        #region 3.新增编辑方法
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
        public override ActionResult SubmitForm(BBdbR_StfBase entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "StfCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.StfCd;  //页面中的编号字段值                 //===复制时需要修改===\
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_StfBase Oldentity = repository_StfBase.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.StfId = KeyValue;//编辑保持主键不变
                    entity.Modify(KeyValue);                  
                    IsOk = MyBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志
                    //if (IsOk > 0)
                    //{
                    //    //更新用户表
                    //    Base_User Line = new Base_User();
                    //    Line.UserId = entity.StfId;
                    //    Line.DepartmentId = entity.DepartmentID;
                    //    Line.OICQ = entity.Wechat;
                     
                    //    Line.Code = entity.StfCd;
                    //    Line.Account = entity.Account;
                    //    Line.Password = entity.Pssw;
                    //    Line.Secretkey = entity.Sctkey;
                    //    Line.RealName = entity.StfNm;
                    //    Line.Gender = entity.StfGndr;
                    //    Line.Mobile = entity.Phn;
                    //    Line.Email = entity.Email;
                    //    Line.Remark = entity.Rem;
                    //    Line.Enabled = Convert.ToInt32(entity.Enabled);                       
                    //    repository_User.Repository().Update(Line);
                    //}
                }
                else//新增操作
                {
                    IsOk = MyBll.CheckCount(Name, Value);//判断页面中填写的数据的编号字段的值是否存在
                    if (IsOk > 0)//存在
                    {
                        Message = "该编号已经存在！";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
                    entity.Create();
                    //entity.StfId = System.Guid.NewGuid().ToString();
                    //entity.Sctkey = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();                   
                    //entity.Pssw = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(entity.Pssw, 32).ToLower(), entity.Sctkey).ToLower(), 32).ToLower();//加密
                    IsOk = MyBll.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);//记录日志
                    //if (IsOk > 0)
                    //{
                    //    //新增用户表
                    //    Base_User Line = new Base_User();
                    //    Line.CompanyId = "b54a89ff-090f-4c9a-96d3-b7545f11265a";//登录验证公司ID，但新增未维护该字段
                    //    Line.UserId = entity.StfId;
                    //    Line.DepartmentId = entity.DepartmentID;
                    //    Line.OICQ = entity.Wechat;
                        
                    //    Line.Code = entity.StfCd;
                    //    Line.Account = entity.Account;
                    //    Line.Password = entity.Pssw;
                    //    Line.Secretkey = entity.Sctkey;
                    //    Line.RealName = entity.StfNm;
                    //    Line.Gender = entity.StfGndr;
                    //    Line.Mobile = entity.Phn;
                    //    Line.Email = entity.Email;
                    //    Line.Remark = entity.Rem;
                    //    Line.Enabled = Convert.ToInt32(entity.Enabled);
                    //    repository_User.Repository().Insert(Line);
                    //}
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

        #region 4.删除方法
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
                //直接删除
                if (array != null && array.Length > 0)
                {
                    IsOk = MyBll.Delete(array);//删// MyBll.Delete(array[i]);
                }
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

        #region 5.搜索
        /// <summary>
        /// 查询方法，本方法为单条件查询，即根据一个条件进行查询
        /// 查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        /// 查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        /// 本查询采用近似查询（like）
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="Condition"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>

        public ActionResult GridPageByCondition(string keywords, string Condition, JqGridParam jqgridparam)
        {
            try
            
            {
                DepartmentStaffConfigViewBll ConfigBll = new DepartmentStaffConfigViewBll();
                string keyword = "all";
                if (keywords!=null && keywords!="")
                {
                    keyword = keywords.Trim();
                }
                Stopwatch watch = CommonHelper.TimerStart();
                //DataTable ListData = MyBll.GetStaffList(keyword, Condition, jqgridparam);//===复制时需要修改===
                StfList = MyBll.GetStaffList(keyword, Condition, jqgridparam);//===复制时需要修改===
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = StfList,
                };
                return Content(StfList.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 6.检查字段是否唯一
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

        #region 9.导入
        /// <summary>
        /// 导入Excel弹出框页面
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ExcelImportDialog(string area_key)
        {
            string moduleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            //模板主表
            Base_ExcelImport base_excellimport = DataFactory.Database().FindEntity<Base_ExcelImport>("ModuleId", moduleId);
            if (base_excellimport.ModuleId != null)
            {
                ViewBag.ModuleId = moduleId;
                ViewBag.ImportFileName = base_excellimport.ImportFileName;
                ViewBag.ImportName = base_excellimport.ImportName;
                ViewBag.ImportId = base_excellimport.ImportId;
                ViewBag.area_key = area_key;
            }
            else
            {
                ViewBag.ModuleId = "0";
                ViewBag.area_key = area_key;
            }
            return View();
        }

        #region 导出模板
        /// <summary>
        /// 下载Excell模板
        /// </summary>
        /// <returns></returns>
        public ActionResult GetExcellTemperature(string ImportId)
        {
            if (!string.IsNullOrEmpty(ImportId))
            {
                DataSet ds = new DataSet();
                DataTable data = new DataTable(); string DataColumn = ""; string fileName;
                MyBll.GetExcellTemperature(ImportId, out data, out DataColumn, out fileName);
                ds.Tables.Add(data);
                MemoryStream ms = DeriveExcel.ExportToExcel(ds, "xls", DataColumn.Split('|'));
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            else
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 清除Datatable空行
        /// </summary>
        /// <param name="dt"></param>
        public void RemoveEmpty(DataTable dt)
        {
            List<DataRow> removelist = new List<DataRow>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool rowdataisnull = true;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
                    {

                        rowdataisnull = false;
                    }
                }
                if (rowdataisnull)
                {
                    removelist.Add(dt.Rows[i]);
                }

            }
            for (int i = 0; i < removelist.Count; i++)
            {
                dt.Rows.Remove(removelist[i]);
            }
        }

        /// <summary>
        /// 导入Excell数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportExel(string area_key)
        {
            int IsOk = 0;//导入状态
            int IsCheck = 1;//用作检验重复地址的标识
            DataTable Result = new DataTable();//导入错误记录表
            IDatabase database = DataFactory.Database();
            List<BBdbR_StfBase> CntlAddrList = new List<BBdbR_StfBase>();

            //构造导入返回结果表
            DataTable Newdt = new DataTable("Result");
            Newdt.Columns.Add("rowid", typeof(System.String));                 //行号
            Newdt.Columns.Add("locate", typeof(System.String));                 //位置
            Newdt.Columns.Add("reason", typeof(System.String));                 //原因
            int errorNum = 1;
            try
            {
                string moduleId = Request["moduleId"]; //表名
                StringBuilder sb_table = new StringBuilder();
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files["filePath"];//获取上传的文件
                string fullname = file.FileName;
                string IsXls = System.IO.Path.GetExtension(fullname).ToString().ToLower();//System.IO.Path.GetExtension获得文件的扩展名
                if (!IsXls.EndsWith(".xls") && !IsXls.EndsWith(".xlsx"))
                {
                    IsOk = 0;
                }
                else
                {

                    string filename = Guid.NewGuid().ToString() + ".xls";
                    if (fullname.EndsWith(".xlsx"))
                    {
                        filename = Guid.NewGuid().ToString() + ".xlsx";
                    }
                    if (file != null && file.FileName != "")
                    {
                        string msg = UploadHelper.FileUpload(file, Server.MapPath("~/Resource/UploadFile/ImportExcel/"), filename);
                    }

                    DataTable dt = ImportExcel.ExcelToDataTable(Server.MapPath("~/Resource/UploadFile/ImportExcel/") + filename);

                    RemoveEmpty(dt);//清除空行。???=>20210712注：方法是否真的有用？void返回对dt未生效
                    dt.Columns.Add("rowid", typeof(System.String));//用来标识excell行ID
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["rowid"] = i + 1;
                    }
                    #region 采集项信息导入
                    //校验
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        IsCheck = 1;//重置标识
                        DataRow dr = Newdt.NewRow();

                        string gender = "";
                        switch (dt.Rows[i]["性别"].ToString())
                        {
                            case "男":
                                gender = "1"; break;
                            case "女":
                                gender = "0"; break;
                            default:
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[性别]";
                                dr[2] = "数字格式不正确请重新输入";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                break;
                        }

                        if (dt.Rows[i]["人员编号"].ToString().Trim() != "" && dt.Rows[i]["人员姓名"].ToString().Trim() != "")
                        {
                            int DeviceCount = MyBll.CheckCount("StfCd", dt.Rows[i]["人员编号"].ToString());//是否有相同的人员编号
                            if (DeviceCount > 0)
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[人员编号]";
                                dr[2] = "在系统中已存在不能重复插入";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else
                            {
                                BBdbR_StfBase pushGetItem = new BBdbR_StfBase();
                                pushGetItem.StfId = System.Guid.NewGuid().ToString();
                                pushGetItem.DepartmentID = area_key;
                                pushGetItem.StfCd = dt.Rows[i]["人员编号"].ToString().Trim();
                                pushGetItem.StfNm = dt.Rows[i]["人员姓名"].ToString().Trim();
                                pushGetItem.StfGndr = gender;
                                pushGetItem.Phn = dt.Rows[i]["手机号"].ToString().Trim();
                                pushGetItem.Wechat = dt.Rows[i]["企业微信号"].ToString().Trim();
                                pushGetItem.Email = dt.Rows[i]["邮箱"].ToString().Trim();
                                pushGetItem.StfPosn = dt.Rows[i]["人员职位"].ToString().Trim();
                                pushGetItem.Rem = dt.Rows[i]["备注"].ToString().Trim();
                                pushGetItem.VersionNumber = "V1.0.0";
                                pushGetItem.Enabled = "1";
                                pushGetItem.CreTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                pushGetItem.CreCd = ManageProvider.Provider.Current().UserId;
                                pushGetItem.CreNm = ManageProvider.Provider.Current().UserName;


                                CntlAddrList.Add(pushGetItem);
                                int b = database.Insert(CntlAddrList);
                                if (b > 0)
                                {
                                    IsOk = IsOk + b;
                                    CntlAddrList.Clear();
                                }
                                else
                                {
                                    dr = Newdt.NewRow();
                                    dr[0] = errorNum;
                                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                                    dr[2] = "人员信息导入失败";
                                    Newdt.Rows.Add(dr);
                                    IsCheck = 0;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                            dr[2] = "人员信息不能为空";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                            continue;
                        }
                    }
                    if (IsCheck == 0)
                    {
                        IsOk = 0;
                    }
                    #endregion

                }
                Result = Newdt;
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "异常错误：" + ex.Message);
                IsOk = 0;
            }
            if (Result.Rows.Count > 0)
            {
                IsOk = 0;
            }
            var JsonData = new
            {
                Status = IsOk > 0 ? "true" : "false",
                ResultData = Result
            };
            return Content(JsonData.ToJson());
        }

        #endregion

        #region 10.重构导出方法
        /// <summary>
        /// 1.如果是按照条件查询后再进行
        /// </summary>
        /// <param name="type"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetExcel_Data(string type, JqGridParam jqgridparam)
        {
            Stopwatch watch = CommonHelper.TimerStart();
            DataTable ListData = new DataTable();

            ListData = StfList.DefaultView.ToTable("人员信息表", true, "StfCd", "StfNm", "DepartmentCode", "DepartmentName", "StfGndr", "Phn", "Wechat", "Email", "StfPosn",
                                  "CreTm", "CreCd", "CreNm");//获取AVI中特定列

            if (ListData.Rows.Count > 0)
            {
                string fileName = "人员基本信息";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_Stf(ListData, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            else
                return null;
        }
        #endregion
        #endregion
    }
}