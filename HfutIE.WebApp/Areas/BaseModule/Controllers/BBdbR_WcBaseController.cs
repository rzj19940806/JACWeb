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
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// 工位基础信息表控制器
    /// </summary>
    public class BBdbR_WcBaseController : PublicController<BBdbR_WcBase>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_WcBase";
        public static DataTable WcList = new DataTable();
        //public static DataTable QPointList = new DataTable();
        #endregion

        #region 创建数据库操作对象区域
        //创建数据库访问对象，用以访问其中操作数据库的方法
        BBdbR_WcBaseBll MyBll = new BBdbR_WcBaseBll(); //===复制时需要修改===
        BBdbR_QualityCheckPointCarComponentConfigBll MyBll1 = new BBdbR_QualityCheckPointCarComponentConfigBll(); //===复制时需要修改===
        public readonly RepositoryFactory<BBdbR_WcBase> repository_plinebase = new RepositoryFactory<BBdbR_WcBase>();
        #endregion

        #region 打开视图区域

        public  ActionResult QualityIndex()
        {
            return View();
        }
        public ActionResult QForm()
        {
            return View();
        }
        public ActionResult ConfigComponent()
        {
            return View();
        }

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
                        if (row["sort"].ToString() == "0")
                        {

                            tree.img = "/Content/Images/Icon16/house.png";
                        }
                        else if (row["sort"].ToString() == "1")
                        {
                            tree.img = "/Content/Images/Icon16/factory.png";
                        }
                        else if (row["sort"].ToString() == "2")
                        {
                            tree.img = "/Content/Images/Icon16/accordion.png";
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
        /// <param name="area_key">点击的节点的主键</param>
        /// <param name="parentId">节点的父级主键</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GridListJson(string area_key, string parentId,string sort, string WcCd, string WcNm, string WcTyp, JqGridParam jqgridparam)
        {
            try
            {
                #region 查询原方法-不考虑搜索条件
                //Stopwatch watch = CommonHelper.TimerStart();
                ////获取点击节点对应的数据（列表）
                ////DataTable ListData = MyBll.GetList(area_key,parentId,sort,ref jqgridparam);
                //WcList = MyBll.GetList(area_key, parentId, sort, ref jqgridparam);
                //var JsonData = new
                //{
                //    total = jqgridparam.total,
                //    page = jqgridparam.page,
                //    records = jqgridparam.records,
                //    costtime = CommonHelper.TimerEnd(watch),
                //    rows = WcList,
                //};
                //return Content(WcList.ToJson());
                #endregion

                #region 查询修改-考虑搜索条件
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on c.WorkshopId=d.WorkshopId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 ");

                if (string.IsNullOrEmpty(area_key) && string.IsNullOrEmpty(parentId))
                {
                    
                }
                else
                {
                    if (parentId != "0")
                    {
                        if (sort == "0")
                        {
                            strSql.Append(@" and  d.WorkshopId='" + area_key + "' ");
                        }
                        else if (sort == "1")
                        {
                            strSql.Append(@" and c.WorkSectionId='" + area_key + "' ");
                        }
                        else
                        {
                            strSql.Append(@" and b.PlineId='" + area_key + "' ");
                        }
                    }
                    else
                    {
                        strSql.Append(@" and  d.WorkshopId='" + area_key + "' ");
                    }
                }

                //工位编号模糊搜索
                if (WcCd != "" && WcCd != null)
                {
                    strSql.Append(" and a.WcCd like @WcCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@WcCd", "%" + WcCd + "%"));
                }
                else { }

                //工位名称模糊搜索
                if (WcNm != "" && WcNm != null)
                {
                    strSql.Append(" and a.WcNm like @WcNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@WcNm", "%" + WcNm + "%"));
                }
                else { }

                //工位类型模糊搜索
                if (WcTyp != "" && WcTyp != null)
                {
                    strSql.Append(" and a.WcTyp like @WcTyp ");
                    parameter.Add(DbFactory.CreateDbParameter("@WcTyp", "%" + WcTyp + "%"));
                }
                else { }
                //排序
                strSql.Append(" order by a.sort asc");

                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                return Content(JsonData.ToJson());
                #endregion
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
        public override ActionResult SubmitForm(BBdbR_WcBase entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "WcCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.WcCd;  //页面中的编号字段值                 //===复制时需要修改===\
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                
                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_WcBase Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    IsOk = MyBll.CheckCount(Name, Value);//判断页面中填写的数据的编号字段的值是否存在
                    if (IsOk > 1)//存在
                    {
                        Message = "该编号已经存在！";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
                    entity.Modify(KeyValue);
                    IsOk = MyBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    if (entity.Seq == null)
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(@"update BBdbR_WcBase set Seq = null where WcId = '" + KeyValue + "' ");
                        int a = DataFactory.Database().ExecuteBySql(strSql);
                    }
                    else { }
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
        public  ActionResult DeleteWc(string KeyValue)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功
                for(int i = 0; i < array.Length; i++)
                {
                    BBdbR_WcBase Oldentity = repositoryfactory.Repository().FindEntity(array[i]);
                    int nums = MyBll.hasChildNode(Oldentity.WcId);
                    if(nums > 0)
                    {
                        throw new Exception("当前所选有子节点数据，不能删除。");
                    } 
                    else
                    {
                        IsOk = MyBll.DeleteUseEnabled(array[i]);
                        if (IsOk > 0) Message = "删除成功。";
                    }

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

        #region 5.查询方法
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageByCondition(string area_key, string parentId, string sort, string WcCd, string WcNm, string WcTyp, JqGridParam jqgridparam)
        {
            try
            {
                #region 原查询-不考虑树节点
                //string keyword = keywords.Trim();
                //Stopwatch watch = CommonHelper.TimerStart();
                ////List<BBdbR_WcBase> ListData = MyBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
                //WcList = MyBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
                //var JsonData = new
                //{
                //    total = jqgridparam.total,
                //    page = jqgridparam.page,
                //    records = jqgridparam.records,
                //    costtime = CommonHelper.TimerEnd(watch),
                //    rows = WcList,
                //};
                //return Content(WcList.ToJson());
                #endregion

                #region 查询修改-考虑树节点
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on c.WorkshopId=d.WorkshopId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 ");

                if (string.IsNullOrEmpty(area_key) && string.IsNullOrEmpty(parentId))
                {

                }
                else
                {
                    if (parentId != "0")
                    {
                        if (sort == "0")
                        {
                            strSql.Append(@" and  d.WorkshopId='" + area_key + "' ");
                        }
                        else if (sort == "1")
                        {
                            strSql.Append(@" and c.WorkSectionId='" + area_key + "' ");
                        }
                        else
                        {
                            strSql.Append(@" and b.PlineId='" + area_key + "' ");
                        }
                    }
                    else
                    {
                        strSql.Append(@" and  d.WorkshopId='" + area_key + "' ");
                    }
                }

                //工位编号模糊搜索
                if (WcCd != "" && WcCd != null)
                {
                    strSql.Append(" and a.WcCd like @WcCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@WcCd", "%" + WcCd + "%"));
                }
                else { }

                //工位名称模糊搜索
                if (WcNm != "" && WcNm != null)
                {
                    strSql.Append(" and a.WcNm like @WcNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@WcNm", "%" + WcNm + "%"));
                }
                else { }

                //工位类型模糊搜索
                if (WcTyp != "" && WcTyp != null)
                {
                    strSql.Append(" and a.WcTyp like @WcTyp ");
                    parameter.Add(DbFactory.CreateDbParameter("@WcTyp", "%" + WcTyp + "%"));
                }
                else { }
                //排序
                strSql.Append(" order by a.sort asc");
                
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "工位基础信息查询成功");
                return Content(JsonData.ToJson());
                #endregion

            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "工位基础信息查询发生异常错误：" + ex.Message);
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

        #region 7.编辑填充界面数据
        public override ActionResult SetForm(string KeyValue)
        {
            try
            {
                BBdbR_WcBase Plineentity = MyBll.GetPlanList1(KeyValue);

                return Content(Plineentity.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 8.导入
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
            List<BBdbR_WcBase> CntlAddrList = new List<BBdbR_WcBase>();

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
                    #region 工位信息导入
                    //校验
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        IsCheck = 1;//重置标识
                        DataRow dr = Newdt.NewRow();
                        if(dt.Rows[i]["工位类型"].ToString().Trim() != "质量检测采集工位" && dt.Rows[i]["工位类型"].ToString().Trim() != "一般工位")
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[工位类型]";
                            dr[2] = "工位类型错误";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                            continue;
                        }

                        if (dt.Rows[i]["工位编号"].ToString().Trim() != "")
                        {
                            int DeviceCount = MyBll.CheckCount("WcCd", dt.Rows[i]["工位编号"].ToString());//是否有相同的区域编号
                            if (DeviceCount > 0)
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[工位编号]";
                                dr[2] = "在系统中已存在不能重复插入";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else
                            {
                                BBdbR_WcBase pushGetItem = new BBdbR_WcBase();
                                pushGetItem.WcId = System.Guid.NewGuid().ToString();
                                pushGetItem.PlineId = area_key;
                                pushGetItem.WcCd = dt.Rows[i]["工位编号"].ToString().Trim();
                                pushGetItem.WcNm = dt.Rows[i]["工位名称"].ToString().Trim();
                                pushGetItem.WcTyp = dt.Rows[i]["工位类型"].ToString().Trim();
                                pushGetItem.WcLength = dt.Rows[i]["工位长度"].ToString().Trim();
                                pushGetItem.WcQueue = int.Parse(dt.Rows[i]["工位顺序"].ToString().Trim());
                                pushGetItem.Dsc = dt.Rows[i]["工位描述"].ToString().Trim();
                                pushGetItem.VersionNumber = "V1.0.0";
                                pushGetItem.Enabled = "1";
                                pushGetItem.Rem = dt.Rows[i]["备注"].ToString().Trim();

                                pushGetItem.CreTm = DateTime.Now.ToString();  //修改时间
                                pushGetItem.CreCd = ManageProvider.Provider.Current().UserId; //修改人编号
                                pushGetItem.CreNm = ManageProvider.Provider.Current().UserName;//修改人名称

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
                                    dr[2] = "工位信息导入失败";
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
                            dr[2] = "工位编号不能为空";
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

        #endregion

        #region 9.质控点方法区

        #region 1.获取树
        public ActionResult TreeJsonQuality()
        {
            try
            {
                DataTable dt = MyBll.GetTreeQuality();//获取树所需数据
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
                        if (row["sort"].ToString() == "0")
                        {

                            tree.img = "/Content/Images/Icon16/house.png";
                        }
                        else if (row["sort"].ToString() == "1")
                        {
                            tree.img = "/Content/Images/Icon16/factory.png";
                        }
                        else if (row["sort"].ToString() == "2")
                        {
                            tree.img = "/Content/Images/Icon16/accordion.png";
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
        /// <param name="area_key">点击的节点的主键</param>
        /// <param name="parentId">节点的父级主键</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GridListJsonQuality(string area_key, string parentId, string sort, string WcCd, string WcNm,JqGridParam jqgridparam)
        {
            try
            {
                #region 原版本—点击树展示质控点信息，不考虑搜索条件
                //Stopwatch watch = CommonHelper.TimerStart();
                ////获取点击节点对应的数据（列表）
                //QPointList = MyBll.GetListQuality(area_key, parentId, sort, ref jqgridparam);
                //var JsonData = new
                //{
                //    total = jqgridparam.total,
                //    page = jqgridparam.page,
                //    records = jqgridparam.records,
                //    costtime = CommonHelper.TimerEnd(watch),
                //    rows = QPointList,
                //};
                //return Content(QPointList.ToJson());
                #endregion

                #region 点击树修改—点击树展示质控点信息，考虑搜索条件
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                
                    if (area_key == "" && parentId == "")
                    {
                        strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 and d.WorkshopCd='ZLCJXN01' ");
                    }
                    else
                    {
                        if (parentId != "0")
                        {
                            if (sort == "0")
                            {
                                strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + area_key + "' ");     //===复制时需要修改===
                            }
                            else if (sort == "1")
                            {
                                strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + area_key + "' ");     //===复制时需要修改===
                            }
                            else
                            {
                                strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId where a.Enabled=1 and b.Enabled=1 and a.PlineId='" + area_key + "' ");     //===复制时需要修改===要修改===
                            }
                        }
                        else
                        {
                            strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on c.WorkshopId=d.WorkshopId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 and d.WorkshopId='" + area_key + "' ");     //===复制时需要修改===
                        }
                    }
                    //模糊搜索质控点编号
                    if (WcCd != null && WcCd != "")
                    {
                        strSql.Append(" and a.WcCd like '%" + WcCd + "%'");
                    }
                    else { }
                    //模糊搜索质控点名称
                    if (WcNm != null && WcNm != "")
                    {
                        strSql.Append(" and a.WcNm like '%" + WcNm + "%'");
                    }
                    else { }

                    //排序
                    strSql.Append(" order by a.sort ");
                
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                return Content(JsonData.ToJson());

                #endregion

            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion      

        #region 5.查询方法
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageByConditionQuality(string area_key, string parentId, string sort, string WcCd, string WcNm, JqGridParam jqgridparam)
        {
            try
            {
                #region 原查询方法-不考虑左边树
                //string keyword = keywords.Trim();
                //Stopwatch watch = CommonHelper.TimerStart();
                //QPointList = MyBll.GetPageListByConditionQuality(keyword, Condition, jqgridparam);//===复制时需要修改===
                //var JsonData = new
                //{
                //    total = jqgridparam.total,
                //    page = jqgridparam.page,
                //    records = jqgridparam.records,
                //    costtime = CommonHelper.TimerEnd(watch),
                //    rows = QPointList,
                //};
                //return Content(QPointList.ToJson());
                #endregion

                #region 查询修改-考虑左边树
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                if (area_key == "" && parentId == "")
                {
                    strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 and d.WorkshopCd='ZLCJXN01' ");
                }
                else
                {
                    if (parentId != "0")
                    {
                        if (sort == "0")
                        {
                            strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + area_key + "' ");     //===复制时需要修改===
                        }
                        else if (sort == "1")
                        {
                            strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + area_key + "' ");     //===复制时需要修改===
                        }
                        else
                        {
                            strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId where a.Enabled=1 and b.Enabled=1 and a.PlineId='" + area_key + "' ");     //===复制时需要修改===要修改===
                        }
                    }
                    else
                    {
                        strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on c.WorkshopId=d.WorkshopId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 and d.WorkshopId='" + area_key + "' ");     //===复制时需要修改===
                    }
                }
                //模糊搜索质控点编号
                if (WcCd != null && WcCd != "")
                {
                    strSql.Append(" and a.WcCd like '%" + WcCd + "%'");
                }
                else { }
                //模糊搜索质控点名称
                if (WcNm != null && WcNm != "")
                {
                    strSql.Append(" and a.WcNm like '%" + WcNm + "%'");
                }
                else { }

                //排序
                strSql.Append(" order by a.sort ");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "质控点信息查询成功");
                return Content(JsonData.ToJson());
                #endregion'

            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "质控点信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 9.1 加载未配置组件
        public ActionResult GridNotConfigJson(string WcId, string Condition, string keywords, string isCH, JqGridParam jqgridparam)//加载未配置组件
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable dt = MyBll1.GetNotConfig(WcId, Condition, keywords, isCH, ref jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 9.2组件配置提交
        public ActionResult CarPositionSubmit(string QualityCheckPointId, List<BBdbR_QualityCheckPointCarComponentConfig> tabledata)//组件配置提交
        {
            try
            {
                int IsOk = 0;//用于判断提交是否成功，成功为1，失败为0
                //DataTable dt = MyBll1.GetPlineId(WcId);
                //string PlineId = dt.Rows[0]["PlineId"].ToString();
                for (int i = 0; i < tabledata.Count; i++)
                {
                    tabledata[i].ConfigId = System.Guid.NewGuid().ToString();
                    tabledata[i].QualityCheckPointId = QualityCheckPointId;
                    tabledata[i].VersionNumber = "V1.0";
                    tabledata[i].Enabled = "1";
                    tabledata[i].CreTm = DateTime.Now;
                    tabledata[i].CreCd = ManageProvider.Provider.Current().UserId;
                    tabledata[i].CreNm = ManageProvider.Provider.Current().UserName;
                    IsOk = MyBll1.CarPositionInsert(tabledata[i]);
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other,"1", "配置检验岗操作成功");
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString() }.ToString());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "配置检验岗操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 9.3在首页加载已经配置过的组件信息
        public ActionResult GridMatListJson(string KeyValue, string isCH, JqGridParam jqgridparam)//在首页加载已经配置过的组件信息
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable dt = MyBll1.GetMatList(KeyValue,isCH);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 9.4.删除配置
        public ActionResult ConfigDelete(string KeyValue)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除检验岗配置失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功
                for (int i = 0; i < array.Length; i++)
                {
                   
                        IsOk = MyBll1.ConfigDelete(array[i]);
                        if (IsOk > 0) Message = "删除检验岗配置成功。";
                    
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

        #region 10.重构导出方法
        /// <summary>
        /// 1.如果是按照条件查询后再进行
        /// </summary>
        /// <param name="type"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetExcel_Data(string area_key, string parentId, string sort, string WcCd, string WcNm, string WcTyp, JqGridParam jqgridparam)
        {
            try
            {
                #region 查询
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                strSql.Append(@"select a.WcCd,a.WcNm,a.WcTyp,b.PlineCd as PlineCd,b.PlineNm as PlineNm,a.Sort,a.StartPoint,a.PreAlarmPoint,a.StopPoint,a.EndPoint,a.Dsc,a.Seq,a.RsvFld1,a.CreTm,a.CreNm,a.MdfTm,a.MdfNm,a.Rem from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on c.WorkshopId=d.WorkshopId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 ");

                if (string.IsNullOrEmpty(area_key) && string.IsNullOrEmpty(parentId))
                {

                }
                else
                {
                    if (parentId != "0")
                    {
                        if (sort == "0")
                        {
                            strSql.Append(@" and  d.WorkshopId='" + area_key + "' ");
                        }
                        else if (sort == "1")
                        {
                            strSql.Append(@" and c.WorkSectionId='" + area_key + "' ");
                        }
                        else
                        {
                            strSql.Append(@" and b.PlineId='" + area_key + "' ");
                        }
                    }
                    else
                    {
                        strSql.Append(@" and  d.WorkshopId='" + area_key + "' ");
                    }
                }

                //工位编号模糊搜索
                if (WcCd != "" && WcCd != null)
                {
                    strSql.Append(" and a.WcCd like @WcCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@WcCd", "%" + WcCd + "%"));
                }
                else { }

                //工位名称模糊搜索
                if (WcNm != "" && WcNm != null)
                {
                    strSql.Append(" and a.WcNm like @WcNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@WcNm", "%" + WcNm + "%"));
                }
                else { }

                //工位类型模糊搜索
                if (WcTyp != "" && WcTyp != null)
                {
                    strSql.Append(" and a.WcTyp like @WcTyp ");
                    parameter.Add(DbFactory.CreateDbParameter("@WcTyp", "%" + WcTyp + "%"));
                }
                else { }
                //排序
                strSql.Append(" order by a.sort asc");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);

                #endregion

                string fileName = "工位基本信息";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_Wc(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "工位基础信息导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "工位基础信息导出操作失败：" + ex.Message);
                return null;
            }
           
        }
        #endregion

        #region 11.重构导出方法-质控点
        /// <summary>
        /// 1.如果是按照条件查询后再进行
        /// </summary>
        /// <param name="type"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetExcel_DataQuality(string area_key, string parentId, string sort, string WcCd, string WcNm, JqGridParam jqgridparam)
        {
            #region 查询
            Stopwatch watch = CommonHelper.TimerStart();
            StringBuilder strSql = new StringBuilder();
            if (area_key == "" && parentId == "")
            {
                strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on d.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 and d.WorkshopCd='ZLCJXN01' ");
            }
            else
            {
                if (parentId != "0")
                {
                    if (sort == "0")
                    {
                        strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + area_key + "' ");     //===复制时需要修改===
                    }
                    else if (sort == "1")
                    {
                        strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkSectionId='" + area_key + "' ");     //===复制时需要修改===
                    }
                    else
                    {
                        strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId where a.Enabled=1 and b.Enabled=1 and a.PlineId='" + area_key + "' ");     //===复制时需要修改===要修改===
                    }
                }
                else
                {
                    strSql.Append(@"select a.*,b.PlineCd as PlineCd,b.PlineNm as PlineNm from BBdbR_WcBase a join BBdbR_PlineBase b on a.PlineId=b.PlineId join BBdbR_WorkSectionBase c on b.WorkSectionId=c.WorkSectionId join BBdbR_WorkshopBase d on c.WorkshopId=d.WorkshopId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 and d.WorkshopId='" + area_key + "' ");     //===复制时需要修改===
                }
            }
            //模糊搜索质控点编号
            if (WcCd != null && WcCd != "")
            {
                strSql.Append(" and a.WcCd like '%" + WcCd + "%'");
            }
            else { }
            //模糊搜索质控点名称
            if (WcNm != null && WcNm != "")
            {
                strSql.Append(" and a.WcNm like '%" + WcNm + "%'");
            }
            else { }

            //排序
            strSql.Append(" order by a.sort ");
            DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);

            #endregion

            try
            {
                DataTable ListData = new DataTable();
                ListData = dt.DefaultView.ToTable("质控点信息表", true, "WcCd", "WcNm", "PlineCd", "PlineNm", "Dsc", "CreTm", "CreCd", "CreNm", "MdfTm", "MdfCd", "MdfNm", "Rem");//获取工位信息表中特定列

                if (ListData.Rows.Count > 0)
                {
                    string fileName = "质控点信息表";
                    string excelType = "xls";
                    MemoryStream ms = DeriveExcel.ExportExcel_Wc2(ListData, excelType);
                    if (!fileName.EndsWith(".xls"))
                    {
                        fileName = fileName + ".xls";
                    }
                    Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "质控点信息导出成功");
                    return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "质控点信息导出操作失败：" + ex.Message);
                return null;
            }
            
        }
        #endregion
    }
}