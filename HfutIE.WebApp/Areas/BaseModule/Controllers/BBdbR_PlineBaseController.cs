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
    /// _FactoryBaseInformation控制器
    /// </summary>
    public class BBdbR_PlineBaseController : PublicController<BBdbR_PlineBase>
    {
        #region 创建数据库操作对象区域
        //创建数据库访问对象，用以访问其中操作数据库的方法
        BBdbR_PlineBaseBll MyBll = new BBdbR_PlineBaseBll(); //===复制时需要修改===
        public readonly RepositoryFactory<BBdbR_PlineBase> repository_plinebase = new RepositoryFactory<BBdbR_PlineBase>();
        #endregion

        #region 方法区   

        #region 1.获取树
        /// <summary>
        /// 获取树结构
        /// </summary>
        /// <returns></returns>
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
        /// <param name="area_key">点击的节点的主键</param>
        /// <param name="parentId">节点的父级主键</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GridListJson(string area_key, string parentId,string sort, string PlineCd, string PlineNm, string PlineTyp, JqGridParam jqgridparam)
        {
            try
            {
                #region 查询原方法-不考虑搜索条件
                //Stopwatch watch = CommonHelper.TimerStart();
                ////获取点击节点对应的数据（列表）
                //DataTable ListData = MyBll.GetList(area_key, parentId, sort,ref jqgridparam);//===复制时需要修改===
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

                #region 查询修改-考虑搜索条件
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                strSql.Append(@"select a.*,b.WorkSectionCd as WorkSectionCd,b.WorkSectionNm as WorkSectionNm from BBdbR_PlineBase a join BBdbR_WorkSectionBase b on a.WorkSectionId=b.WorkSectionId join BBdbR_WorkshopBase c on b.WorkshopId=c.WorkshopId join BBdbR_FacBase d on c.FacId=d.FacId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 ");
                if (string.IsNullOrEmpty(area_key) && string.IsNullOrEmpty(parentId))
                {
                   
                }
                else
                {
                    if (parentId != "0")
                    {
                        if (sort == "1")
                        {
                            strSql.Append(@" and c.WorkshopId= '" + area_key + "' ");
                        }
                        else
                        {
                            strSql.Append(@" and a.WorkSectionId='" + area_key + "'  ");
                        }
                    }
                    else
                    {
                        strSql.Append(@" and d.FacId='" + area_key + "' ");
                    }
                }

                //产线编号模糊搜索
                if (PlineCd != "" && PlineCd != null)
                {
                    strSql.Append(" and PlineCd like @PlineCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@PlineCd", "%" + PlineCd + "%"));
                }
                else { }
                //产线名称模糊搜索
                if (PlineNm != "" && PlineNm != null)
                {
                    strSql.Append(" and PlineNm like @PlineNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@PlineNm", "%" + PlineNm + "%"));
                }
                else { }
                //产线类型模糊搜索
                if (PlineTyp != "" && PlineTyp != null)
                {
                    strSql.Append(" and PlineTyp like @PlineTyp ");
                    parameter.Add(DbFactory.CreateDbParameter("@PlineTyp", "%" + PlineTyp + "%"));
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
        public  ActionResult SubmitForm1(BBdbR_PlineBase entity, string KeyValue, string KeyValue2)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "PlineCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.PlineCd;  //页面中的编号字段值                 //===复制时需要修改===\
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";//keyValue=空吗？是，返回新增；否，返回编辑。

                
                if (entity.Dsc == null)
                {
                    entity.Dsc = "";
                }
                else { }
                if (entity.Rem == null)
                {
                    entity.Rem = "";
                }
                else { }

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_PlineBase Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.Modify(KeyValue);
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
                    entity.WorkSectionId = KeyValue2;
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
        public  ActionResult DeletePline(string KeyValue)
        {
            BBdbR_WcBaseBll WcBll = new BBdbR_WcBaseBll();
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功                   
                if (KeyValue != "")//先把对应CarPartId中所有检查项目均删除
                {
                    int a = WcBll.GetWcCount(KeyValue);//查找工厂基础信息中是否有在本公司名下，如果有就不可以删除
                    if (a > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "工厂下设置车间，不可删除" }.ToString());
                    }
                    else
                    {
                        IsOk = MyBll.Delete(array);//执行删除操作
                        if (IsOk > 0)//表示删除成
                        {
                            Message = "删除成功。";//修改返回信息
                        }
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

        public ActionResult GridPageByCondition(string area_key, string parentId, string sort, string PlineCd, string PlineNm, string PlineTyp, JqGridParam jqgridparam)
        {
            try
            {
                #region 查询原方法-不考虑树节点
                //string keyword = keywords.Trim();
                //Stopwatch watch = CommonHelper.TimerStart();
                //DataTable ListData = MyBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
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

                #region 查询修改-考虑树节点
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                strSql.Append(@"select a.*,b.WorkSectionCd as WorkSectionCd,b.WorkSectionNm as WorkSectionNm from BBdbR_PlineBase a join BBdbR_WorkSectionBase b on a.WorkSectionId=b.WorkSectionId join BBdbR_WorkshopBase c on b.WorkshopId=c.WorkshopId join BBdbR_FacBase d on c.FacId=d.FacId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 ");
                if (string.IsNullOrEmpty(area_key) && string.IsNullOrEmpty(parentId))
                {

                }
                else
                {
                    if (parentId != "0")
                    {
                        if (sort == "1")
                        {
                            strSql.Append(@" and c.WorkshopId= '" + area_key + "' ");
                        }
                        else
                        {
                            strSql.Append(@" and a.WorkSectionId='" + area_key + "'  ");
                        }
                    }
                    else
                    {
                        strSql.Append(@" and d.FacId='" + area_key + "' ");
                    }
                }

                //产线编号模糊搜索
                if (PlineCd != "" && PlineCd != null)
                {
                    strSql.Append(" and PlineCd like @PlineCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@PlineCd", "%" + PlineCd + "%"));
                }
                else { }
                //产线名称模糊搜索
                if (PlineNm != "" && PlineNm != null)
                {
                    strSql.Append(" and PlineNm like @PlineNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@PlineNm", "%" + PlineNm + "%"));
                }
                else { }
                //产线类型模糊搜索
                if (PlineTyp != "" && PlineTyp != null)
                {
                    strSql.Append(" and PlineTyp like @PlineTyp ");
                    parameter.Add(DbFactory.CreateDbParameter("@PlineTyp", "%" + PlineTyp + "%"));
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "产线基础信息查询成功");
                return Content(JsonData.ToJson());
                #endregion

            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "产线基础信息查询发生异常错误：" + ex.Message);
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
        /// <summary>
        /// 编辑填充界面
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public override ActionResult SetForm(string KeyValue)
        {
            try
            {
                BBdbR_PlineBase PlineEntity = MyBll.GetPlineList(KeyValue);

                return Content(PlineEntity.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 8.弹框负责人员信息
        /// <summary>
        /// 人员下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPlineNm(string StfId)
        {
            try
            {
                BBdbR_StfBaseBll StfBll = new BBdbR_StfBaseBll();
                DataTable dataTable = StfBll.GetPlineNm(StfId);
                var JsonData = new
                {
                    rows = dataTable,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 9.导入
        /// <summary>
        /// 导入Excel弹出框页面
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ExcelImportDialog(string areaId)
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

                ViewBag.WorkSectionId = areaId;
            }
            else
            {
                ViewBag.ModuleId = "0";
                ViewBag.WorkSectionId = areaId;
            }
            //ViewBag.ID = Request.Params["ID"];
            //ID1 = ViewBag.ID;
            //ViewBag.OrderID = Request.Params["OrderID"];
            //OrderID1 = ViewBag.OrderID;
            return View();
        }
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
        public ActionResult ImportExel(string WorkSectionId)
        {
            int IsOk = 0;//导入状态
            int IsCheck = 1;//用作检验重复地址的标识
            DataTable Result = new DataTable();//导入错误记录表
            IDatabase database = DataFactory.Database();
            List<BBdbR_PlineBase> EntityList = new List<BBdbR_PlineBase>();

            //构造导入返回结果表
            DataTable Newdt = new DataTable("Result");
            Newdt.Columns.Add("rowid", typeof(System.String));                 //行号
            Newdt.Columns.Add("locate", typeof(System.String));                 //位置
            Newdt.Columns.Add("reason", typeof(System.String));                 //原因
            int errorNum = 1;
            try
            {
                string moduleId = Request["moduleId"]; //表名
                //StringBuilder sb_table = new StringBuilder();
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

                    RemoveEmpty(dt);//清除空行
                    dt.Columns.Add("rowid", typeof(System.String));//用来标识excell行ID
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["rowid"] = i + 1;
                    }
                    #region 检查项目信息导入
                    //校验
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IsCheck = 1;//重置标识
                        DataRow dr = Newdt.NewRow();
                        string plinttyp = "";
                        int wcquantity = 0;
                        decimal wclength = 0;
                        decimal wcintercept = 0;
                        decimal wspjph = 0;
                        int cachequantity = 0;
                        int cachelimit = 0;
                        int highestquantity = 0;
                        int lowestquantity = 0;
                        int prealarmpoint = 0;
                        int endpoint = 0;
                        string runningmode = "";
                        decimal stationbegin = 0;
                        decimal stationend = 0;
                        int highfrontcache = 0;
                        int lowfrontcache = 0;
                        int highpostcache = 0;
                        int lowpostcache = 0;

                        #region 检查数据格式

                        //产线类型
                        Base_DataDictionary PlineType = database.FindEntity<Base_DataDictionary>("Code", "PlineType");
                        List<Base_DataDictionaryDetail> PlineType1 = database.FindList<Base_DataDictionaryDetail>("DataDictionaryId", PlineType.DataDictionaryId);
                        foreach (var item in PlineType1)
                        {
                            IsCheck = 0;
                            if (item.FullName == dt.Rows[i]["产线类型"].ToString().Trim())
                            {
                                IsCheck = 1;
                                plinttyp = item.Code;
                                break;
                            }
                        }
                        if (IsCheck == 0)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[产线类型]";
                            dr[2] = "在系统中不存在此产线类型或输入有误";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }
                        //运行模式
                        Base_DataDictionary Running = database.FindEntity<Base_DataDictionary>("Code", "RunningMode");
                        List<Base_DataDictionaryDetail> Running1 = database.FindList<Base_DataDictionaryDetail>("DataDictionaryId", Running.DataDictionaryId);
                        foreach (var item in Running1)
                        {
                            IsCheck = 0;
                            if (item.FullName == dt.Rows[i]["运行模式"].ToString().Trim())
                            {
                                IsCheck = 1;
                                runningmode = item.Code;
                                break;
                            }
                        }
                        if (IsCheck == 0)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[运行模式]";
                            dr[2] = "在系统中不存在此运行模式或输入有误";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }
                        if (int.TryParse(dt.Rows[i]["工位数量"].ToString(), out wcquantity) == false)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[工位数量]";
                            dr[2] = "数字格式不正确请重新输入";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }

                        if (decimal.TryParse(dt.Rows[i]["工位默认长度"].ToString(), out wclength) == false)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[工位默认长度]";
                            dr[2] = "数字格式不正确请重新输入";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }

                        if (decimal.TryParse(dt.Rows[i]["工位默认截距"].ToString(), out wcintercept) == false)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[工位默认截距]";
                            dr[2] = "数字格式不正确请重新输入";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }

                        if (decimal.TryParse(dt.Rows[i]["JPH"].ToString(), out wspjph) == false)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[JPH]";
                            dr[2] = "数字格式不正确请重新输入";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }
                        if (int.TryParse(dt.Rows[i]["缓存上限"].ToString(), out cachequantity) == false)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[缓存上限]";
                            dr[2] = "数字格式不正确请重新输入";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }
                        if (int.TryParse(dt.Rows[i]["缓存下限"].ToString(), out cachelimit) == false)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[缓存下限]";
                            dr[2] = "数字格式不正确请重新输入";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }
                        if (int.TryParse(dt.Rows[i]["最高在制"].ToString(), out highestquantity) == false)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[最高在制]";
                            dr[2] = "数字格式不正确请重新输入";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }
                        if (int.TryParse(dt.Rows[i]["最低在制"].ToString(), out lowestquantity) == false)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[最低在制]";
                            dr[2] = "数字格式不正确请重新输入";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }
                        if (int.TryParse(dt.Rows[i]["预警位"].ToString(), out prealarmpoint) == false)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[预警位]";
                            dr[2] = "数字格式不正确请重新输入";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }
                        if (int.TryParse(dt.Rows[i]["停止位"].ToString(), out endpoint) == false)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[停止位]";
                            dr[2] = "数字格式不正确请重新输入";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }
                        if (decimal.TryParse(dt.Rows[i]["开始"].ToString(), out stationbegin) == false)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[开始]";
                            dr[2] = "数字格式不正确请重新输入";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }
                        if (decimal.TryParse(dt.Rows[i]["结束"].ToString(), out stationend) == false)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[结束]";
                            dr[2] = "数字格式不正确请重新输入";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }
                        if (int.TryParse(dt.Rows[i]["前缓存上限"].ToString(), out highfrontcache) == false)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[前缓存上限]";
                            dr[2] = "数字格式不正确请重新输入";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }
                        if (int.TryParse(dt.Rows[i]["前缓存下限"].ToString(), out lowfrontcache) == false)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[前缓存下限]";
                            dr[2] = "数字格式不正确请重新输入";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }
                        if (int.TryParse(dt.Rows[i]["后缓存上限"].ToString(), out highpostcache) == false)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[后缓存上限]";
                            dr[2] = "数字格式不正确请重新输入";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }
                        if (int.TryParse(dt.Rows[i]["后缓存下限"].ToString(), out lowpostcache) == false)
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[后缓存下限]";
                            dr[2] = "数字格式不正确请重新输入";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                        }


                        #endregion

                        if (dt.Rows[i]["产线编号"].ToString().Trim() != "")
                        {
                            int Count = MyBll.CheckCount("PlineCd", dt.Rows[i]["产线编号"].ToString().Trim());//是否有相同编号
                            if (Count > 0)
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行[产线编号]";
                                dr[2] = "在系统中已存在不能重复插入";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                continue;
                            }
                            else
                            {
                                if (IsCheck == 0)//返回所有输入格式问题
                                {
                                    continue;
                                }
                                BBdbR_PlineBase entity = new BBdbR_PlineBase();
                                entity.WorkSectionId = WorkSectionId;   //工段主键
                                entity.PlineCd = dt.Rows[i]["产线编号"].ToString().Trim();
                                entity.PlineNm = dt.Rows[i]["产线名称"].ToString().Trim();
                                entity.PlineTyp = plinttyp;//产线类型
                                entity.WcQuantity = wcquantity;//工位数量
                                entity.WcLength = wclength;//工位默认长度
                                entity.WcIntercept = wcintercept;//工位默认截距
                                entity.WspJPH = wspjph;//JPH
                                entity.CacheQantity = cachequantity;//缓存上限
                                entity.CacheLimit = cachelimit;//缓存下限
                                entity.HighestQantity = highestquantity;//最高在制
                                entity.LowestQantity = lowestquantity;//最低在制
                                //entity.PreAlarmPoint = prealarmpoint;//预警位
                                //entity.EndPoint = endpoint;//停止位
                                entity.RunningMode = runningmode;//运行模式
                                entity.StationBegion = stationbegin;//开始
                                entity.StationEnd = stationend;//结束
                                entity.HighestFrontCache = highfrontcache;//前缓存上限
                                entity.LowestFrontCache = lowfrontcache;//前缓存下限
                                entity.HighestPostCache = highpostcache; //后缓存上限
                                entity.LowestPostCache = lowpostcache;//后缓存下限
                                BBdbR_StfBase staff = database.FindEntity<BBdbR_StfBase>("StfCd", dt.Rows[i]["负责人员编号"].ToString().Trim());
                                entity.StfId = staff.StfId;
                                entity.StfCd = dt.Rows[i]["负责人员编号"].ToString().Trim(); //负责人员编号
                                entity.StfNm = dt.Rows[i]["负责人员姓名"].ToString().Trim(); //负责人员姓名
                                entity.Phn = staff.Phn; //负责人员手机号
                                entity.Dsc = dt.Rows[i]["产线描述"].ToString().Trim(); //产线描述
                                entity.Rem = dt.Rows[i]["备注"].ToString().Trim(); //备注

                                entity.Create();
                                //Device.DvcMDt = DateTime.ParseExact(dt.Rows[i]["设备制造日期"].ToString().Trim(), "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);

                                EntityList.Add(entity);
                                int b = database.Insert(EntityList);
                                if (b > 0)
                                {
                                    IsOk = IsOk + b;
                                    EntityList.Clear();
                                }
                                else
                                {
                                    dr = Newdt.NewRow();
                                    dr[0] = errorNum;
                                    dr[1] = "第[" + dt.Rows[i]["rowid"].ToString() + "]行";
                                    dr[2] = "产线信息插入失败";
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
                            dr[2] = "产线编号不能为空";
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

        #region 10.导出模板
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

        #region 重构导出
        public ActionResult GetExcel_Data(string area_key, string parentId, string sort, string PlineCd, string PlineNm, string PlineTyp, JqGridParam jqgridparam)
        {
            try
            {
                #region 根据当前搜索条件查出数据并导出
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                strSql.Append(@"select a.PlineCd,a.PlineNm,b.WorkSectionCd as WorkSectionCd,b.WorkSectionNm as WorkSectionNm,a.Sort,a.PlineTyp,a.WcQuantity,a.WcLength,a.WcIntercept,a.Dsc,a.CreTm,a.CreNm,a.MdfTm,a.MdfNm,a.Rem from BBdbR_PlineBase a join BBdbR_WorkSectionBase b on a.WorkSectionId=b.WorkSectionId join BBdbR_WorkshopBase c on b.WorkshopId=c.WorkshopId join BBdbR_FacBase d on c.FacId=d.FacId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 ");
                if (string.IsNullOrEmpty(area_key) && string.IsNullOrEmpty(parentId))
                {

                }
                else
                {
                    if (parentId != "0")
                    {
                        if (sort == "1")
                        {
                            strSql.Append(@" and c.WorkshopId= '" + area_key + "' ");
                        }
                        else
                        {
                            strSql.Append(@" and a.WorkSectionId='" + area_key + "'  ");
                        }
                    }
                    else
                    {
                        strSql.Append(@" and d.FacId='" + area_key + "' ");
                    }
                }

                //产线编号模糊搜索
                if (PlineCd != "" && PlineCd != null)
                {
                    strSql.Append(" and PlineCd like @PlineCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@PlineCd", "%" + PlineCd + "%"));
                }
                else { }
                //产线名称模糊搜索
                if (PlineNm != "" && PlineNm != null)
                {
                    strSql.Append(" and PlineNm like @PlineNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@PlineNm", "%" + PlineNm + "%"));
                }
                else { }
                //产线类型模糊搜索
                if (PlineTyp != "" && PlineTyp != null)
                {
                    strSql.Append(" and PlineTyp like @PlineTyp ");
                    parameter.Add(DbFactory.CreateDbParameter("@PlineTyp", "%" + PlineTyp + "%"));
                }
                else { }
                //排序
                strSql.Append(" order by a.sort asc");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);

                #endregion



                string fileName = "产线基础信息";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_PlineBase(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "产线基础信息导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "产线基础信息导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        #endregion
    }
}