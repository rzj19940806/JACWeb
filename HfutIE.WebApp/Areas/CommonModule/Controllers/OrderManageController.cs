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
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace HfutIE.WebApp.Areas.CommonModule.Controllers
{
    /// <summary>
    /// 公司管理控制器
    /// </summary>
    public class OrderManageController : PublicController<A_OrderBase>//  A_ProjectInfomation
    {
        A_OrderBaseBll a_orderbasebll = new A_OrderBaseBll();
        A_ProjectInfomationBll projectinfomationbll = new A_ProjectInfomationBll();
        public virtual ActionResult OrderForm()
        {
            return View();
        }
        public virtual ActionResult ProjectForm()
        {
            return View();
        }
        /// <summary>
        /// 【公司管理】返回树JONS
        /// </summary>
        /// <returns></returns>
        //public ActionResult TreeJson()
        //{
        //    List<Base_Company> list = base_companybll.GetList();
        //    List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
        //    foreach (Base_Company item in list)
        //    {
        //        bool hasChildren = false;
        //        IList childnode = list.FindAll(t => t.ParentId == item.CompanyId);
        //        if (childnode.Count > 0)
        //        {
        //            hasChildren = true;
        //        }
        //        TreeJsonEntity tree = new TreeJsonEntity();
        //        tree.id = item.CompanyId;
        //        tree.text = item.FullName;
        //        tree.value = item.CompanyId;
        //        tree.Attribute = "Category";
        //        tree.AttributeValue = item.Category;
        //        tree.isexpand = true;
        //        tree.complete = true;
        //        tree.hasChildren = hasChildren;
        //        tree.parentId = item.ParentId;
        //        if (item.ParentId == "0")
        //        {
        //            tree.img = "/Content/Images/Icon16/molecule.png";
        //        }
        //        else
        //        {
        //            tree.img = "/Content/Images/Icon16/hostname.png";
        //        }
        //        TreeList.Add(tree);
        //    }
        //    return Content(TreeList.TreeToJson());
        //}
        /// <summary>
        /// 【订单管理】返回订单列表JONS
        /// </summary>
        /// <returns></returns>
        //public ActionResult TreeGridListJson()
        //{
        //    List<A_OrderBase> ListData = a_orderbasebll.GetList();
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("{ \"rows\": [");
        //    sb.Append(TreeGridJson(ListData, -1));
        //    sb.Append("]}");
        //    return Content(sb.ToString());
        //}
        int lft = 1, rgt = 1000000;
        //public string TreeGridJson(List<Base_Company> ListData, int index, string ParentId = "0")
        //{
        //    StringBuilder sb = new StringBuilder();
        //    List<Base_Company> ChildNodeList = ListData.FindAll(t => t.ParentId == ParentId);
        //    if (ChildNodeList.Count > 0) { index++; }
        //    foreach (Base_Company entity in ChildNodeList)
        //    {
        //        string strJson = entity.ToJson();
        //        strJson = strJson.Insert(1, "\"level\":" + index + ",");
        //        strJson = strJson.Insert(1, "\"isLeaf\":" + (ListData.Count<Base_Company>(t => t.ParentId == entity.CompanyId) == 0 ? true : false).ToString().ToLower() + ",");
        //        strJson = strJson.Insert(1, "\"expanded\":true,");
        //        strJson = strJson.Insert(1, "\"lft\":" + lft++ + ",");
        //        strJson = strJson.Insert(1, "\"rgt\":" + rgt-- + ",");
        //        sb.Append(strJson);
        //        sb.Append(TreeGridJson(ListData, index, entity.CompanyId));
        //    }
        //    return sb.ToString().Replace("}{", "},{");
        //}
        /// <summary>
        /// 【公司管理】返回列表JONS
        /// </summary>
        /// <returns></returns>
        //public ActionResult ListJson(string ParentId)
        //{
        //    List<Base_Company> list = base_companybll.GetList();
        //    if (!string.IsNullOrEmpty(ParentId))
        //    {
        //        list = list.FindAll(t => t.ParentId == ParentId);
        //    }
        //    return Content(list.ToJson());
        //}

        /// <summary>
        /// 订单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GridList()
        {
            List<A_OrderBase> ListData = a_orderbasebll.GetList();
            var JsonData = new
            {
                rows = ListData,
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 项目列表
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult GetDetailList(string ID)
        {
            List<A_ProjectInfomation> ListData = projectinfomationbll.GetProjectList(ID);
            var JsonData = new
            {
                rows = ListData,
            };
            return Content(JsonData.ToJson());
        }


        /// <summary>
        /// 【订单管理】删除数据
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult DeleteOrder(string KeyValue)
        {
            try
            {
                IDatabase database = DataFactory.Database();
                bool success = true;
                string Message = "删除失败。";
                int IsOk = 0;
                int ProjectCount = Convert.ToInt32(DataFactory.Database().FindCount<A_ProjectInfomation>("OrderID", KeyValue));
                if (ProjectCount == 0)
                {
                    //IsOk = database.Update<A_OrderBase>("IsAvailable",  "0");
                    IsOk = a_orderbasebll.DeleteOrder(Convert.ToInt32(KeyValue));
                    //IsOk = repositoryfactory.Repository().Delete(KeyValue);
                    if (IsOk > 0)
                    {
                        Message = "删除成功。";
                    }
                }
                else
                {
                    Message = "订单内有项目，不能删除。";
                    success = false;
                }
                //WriteLog(IsOk, KeyValue, Message);
                return Content(new JsonMessage { Success = success, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                //WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 【项目管理】删除数据
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult DeleteProject(string KeyValue)
        {
            try
            {
                IDatabase database = DataFactory.Database();
                DbTransaction isOpenTrans = database.BeginTrans();
                bool success = true;
                string Message = "删除失败。";
                int IsOk = 0;
                int ProjectCount = Convert.ToInt32(DataFactory.Database().FindCount<A_ProjectPlanInfomation>("ProductID", KeyValue));
                if (ProjectCount == 0)
                {
                    //IsOk = database.Delete("A_ProjectInfomation", "ID",  KeyValue);
                    IsOk = projectinfomationbll.DeleteProject(Convert.ToInt32(KeyValue));
                    //IsOk = repositoryfactory.Repository().Delete(KeyValue);
                    if (IsOk > 0)
                    {
                        Message = "删除成功。";
                    }
                }
                else
                {
                    Message = "项目内有计划，不能删除。";
                    success = false;
                }
                //WriteLog(IsOk, KeyValue, Message);
                return Content(new JsonMessage { Success = success, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                //WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 加载订单状态下拉框
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult SetOrderState()
        {
            IDatabase database = DataFactory.Database();
            Base_DataDictionary typeId = database.FindEntity<Base_DataDictionary>("Code", "LotStatus");
            List<Base_DataDictionaryDetail> orderstate = database.FindList<Base_DataDictionaryDetail>("DataDictionaryId", typeId.DataDictionaryId);
             orderstate = orderstate.OrderBy(it => it.SortCode).ToList(); //从小到大
            string strJson = orderstate.ToJson();
            //自定义
            //strJson = strJson.Insert(1, Base_FormAttributeBll.Instance.GetBuildForm(KeyValue));
            return Content(strJson);
        }
        /// <summary>
        /// 加载订单优先级下拉框
        /// </summary>
        /// <returns></returns>
        //public virtual ActionResult SetOrderPriority()
        //{
        //    IDatabase database = DataFactory.Database();
        //    Base_DataDictionary typeId = database.FindEntity<Base_DataDictionary>("Code", "LotStatus");
        //    List<Base_DataDictionaryDetail> orderstate = database.FindList<Base_DataDictionaryDetail>("DataDictionaryId", typeId.DataDictionaryId);

        //    string strJson = orderstate.ToJson();
        //    //自定义
        //    //strJson = strJson.Insert(1, Base_FormAttributeBll.Instance.GetBuildForm(KeyValue));
        //    return Content(strJson);
        //}
        /// <summary>
        /// 获取订单状态
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public virtual ActionResult GetProjectState(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            
            List<A_OrderBase> ListData = a_orderbasebll.SetProjectState(Convert.ToInt32(KeyValue));
            string strJson = ListData.ToJson();
            return Content(strJson);
        }
        /// <summary>
        /// 根据条件查询订单信息
        /// </summary>
        /// <param name="Condition"></param>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        public virtual ActionResult SearchList(string Condition ,string Keyword)
        {
            IDatabase database = DataFactory.Database();
            List<A_OrderBase> ListData = a_orderbasebll.GetList(Condition,Keyword);
            var JsonData = new
            {
                rows = ListData,
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 【订单管理】表单赋值
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SetOrderForm(string KeyValue)
        {
            A_OrderBase entity = repositoryfactory.Repository().FindEntity(Convert.ToInt32(KeyValue));
            string strJson = entity.ToJson();
            //自定义
            //strJson = strJson.Insert(1, Base_FormAttributeBll.Instance.GetBuildForm(KeyValue));
            return Content(strJson);
        }
        /// <summary>
        /// 【项目管理】表单赋值
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SetProjectForm(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            A_ProjectInfomation entity = database.FindEntity<A_ProjectInfomation>(KeyValue);
            string strJson = entity.ToJson();
            //自定义
            //strJson = strJson.Insert(1, Base_FormAttributeBll.Instance.GetBuildForm(KeyValue));
            return Content(strJson);
        }
        /// <summary>
        /// 【订单管理】提交表单
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="KeyValue">主键值</param>
        /// <param name="BuildFormJson">自定义表单</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SubmitOrderForm(A_OrderBase entity, string KeyValue)
        {
            string ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                int IsOk = 0;
                //var Message = "新增成功。";
                var Message = (KeyValue == "" )? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    A_OrderBase Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.Modify(KeyValue);
                    IsOk = database.Update(entity, isOpenTrans);
                    //this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);
                }
                else
                {
                    int OrderCount = Convert.ToInt32(DataFactory.Database().FindCount<A_OrderBase>("OrderCode", entity.OrderCode));//是否有相同订单号
                    if (OrderCount == 0)
                    {
                        entity.Create();
                        IsOk = database.Insert(entity, isOpenTrans);
                        Base_DataScopePermissionBll.Instance.AddScopeDefault(ModuleId, ManageProvider.Provider.Current().UserId, "0", entity.ID.ToString(), isOpenTrans);
                        //this.WriteLog(IsOk, entity, null, KeyValue, Message);
                    }
                    else
                    {
                        Message = "";
                    }
                    
                }
                //Base_FormAttributeBll.Instance.SaveBuildForm(BuildFormJson, entity.ID.ToString(), ModuleId, isOpenTrans);
                database.Commit();
                
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                //this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 【项目管理】提交表单
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="KeyValue">主键值</param>
        /// <param name="BuildFormJson">自定义表单</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SubmitProjectForm(A_ProjectInfomation entity, string order_id, string project_id)
        {
            string ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                int IsOk = 0;
                var Message = string.IsNullOrEmpty(project_id) ? "新增成功。" : "编辑成功。";
                //var Message = (project_id == "") ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(project_id))
                {
                    A_ProjectInfomation Oldentity = database.FindEntity<A_ProjectInfomation>(project_id);
                    //A_ProjectInfomation Oldentity = repositoryfactory.Repository().FindEntity(order_id);//获取没更新之前实体对象
                    entity.Modify(project_id);
                    IsOk = database.Update(entity, isOpenTrans);
                    //this.WriteLog(IsOk, entity, Oldentity, order_id, Message);
                }
                else
                {
                    int ProjectCount = Convert.ToInt32(DataFactory.Database().FindCount<A_ProjectInfomation>("ProjectCode", entity.ProjectCode));//是否有相同项目号
                    if(ProjectCount == 0)
                    {
                        entity.OrderID = Convert.ToInt32(order_id);
                        entity.Create();
                        IsOk = database.Insert(entity, isOpenTrans);
                        Base_DataScopePermissionBll.Instance.AddScopeDefault(ModuleId, ManageProvider.Provider.Current().UserId, "0", entity.ID.ToString(), isOpenTrans);
                        //this.WriteLog(IsOk, entity, null, order_id, Message);
                    }
                    else
                    {
                        Message = "";
                    }
                }
                //Base_FormAttributeBll.Instance.SaveBuildForm(BuildFormJson, entity.ID.ToString(), ModuleId, isOpenTrans);
                database.Commit();

                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                //this.WriteLog(-1, entity, null, order_id, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 导入Excel弹出框页面
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ExcelImportDialog()
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
            }
            else
            {
                ViewBag.ModuleId = "0";
            }
            
            

            //ViewBag.ID = Request.Params["ID"];
            //ID1 = ViewBag.ID;
            //ViewBag.OrderID = Request.Params["OrderID"];
            //OrderID1 = ViewBag.OrderID;
            return View();
        }
        //public static string OrderID1;

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
                a_orderbasebll.GetExcellTemperature(ImportId, out data, out DataColumn, out fileName);
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

        #region 导入
        /// <summary>
        /// 导入Excell数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportExel()
        {
            int IsOk = 1;//导入状态
            int IsCheck = 1;//用作检验重复地址的标识
            DataTable Result = new DataTable();//导入错误记录表
            IDatabase database = DataFactory.Database();
            List<A_OrderBase> OrderbaseList = new List<A_OrderBase>();
            List<A_ProjectInfomation> projectinfomationList = new List<A_ProjectInfomation>();

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
                    //IsOk = a_orderbasebll.ImportExcel(moduleId, dt, out Result);
                    #region 订单信息导入
                    int ordertype = 0;//赋初值
                    int projecttype = 0;//赋初值
                    //校验
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = Newdt.NewRow();
                        StringBuilder rowSb = new StringBuilder();//累加每一个单元格的值，一行全空就停止插入
                        switch (dt.Rows[i]["订单类型"].ToString().Trim())
                        {
                            case "标准生产订单":
                                ordertype = 1; break;
                            case "返工订单":
                                ordertype = 2; break;
                            case "试制订单":
                                ordertype = 3; break;
                            case "1":
                                ordertype = 1; break;
                            case "2":
                                ordertype = 2; break;
                            case "3":
                                ordertype = 3; break;
                            default:
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + (i + 1).ToString() + "]行[订单类型]";
                                dr[2] = "在系统中不存在此订单类型或输入有误";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                break;
                        }
                        switch (dt.Rows[i]["项目类型"].ToString().Trim())
                        {
                            case "项目类型1":
                                projecttype = 1; break;
                            case "项目类型2":
                                projecttype = 2; break;
                            case "项目类型3":
                                projecttype = 3; break;
                            case "1":
                                projecttype = 1; break;
                            case "2":
                                projecttype = 2; break;
                            case "3":
                                projecttype = 3; break;
                            default:
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + (i + 1).ToString() + "]行[项目类型]";
                                dr[2] = "在系统中不存在此项目类型或输入有误";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                break;
                        }
                        if(dt.Rows[i]["订单编号"].ToString().Trim() != "" && dt.Rows[i]["项目编号"].ToString().Trim() != "")
                        {
                            int OrderCount = Convert.ToInt32(DataFactory.Database().FindCount<A_OrderBase>("OrderCode", dt.Rows[i]["订单编号"].ToString().Trim()));//是否有相同订单号
                            int ProjectCount = Convert.ToInt32(DataFactory.Database().FindCount<A_ProjectInfomation>("ProjectCode", dt.Rows[i]["项目编号"].ToString().Trim()));//是否有相同项目号
                            if (OrderCount > 0)
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + (i + 1).ToString() + "]行[订单编号]";
                                dr[2] = "在系统中已存在不能重复插入";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                break;
                            }
                            else if (ProjectCount > 0)
                            {
                                dr = Newdt.NewRow();
                                dr[0] = errorNum;
                                dr[1] = "第[" + (i + 1).ToString() + "]行[项目编号]";
                                dr[2] = "在系统中已存在不能重复插入";
                                Newdt.Rows.Add(dr);
                                errorNum++;
                                IsCheck = 0;
                                break;
                            }
                        }
                        else
                        {
                            dr = Newdt.NewRow();
                            dr[0] = errorNum;
                            dr[1] = "第[" + (i + 1).ToString() + "]行";
                            dr[2] = "订单编号或者项目编号不能为空";
                            Newdt.Rows.Add(dr);
                            errorNum++;
                            IsCheck = 0;
                            break;
                        }
                        
                        //如果遇到空行，说明从Excel导入后续的行都是空的，不再导入，清除掉最后一个错误
                        if (string.IsNullOrEmpty(rowSb.ToString()))
                        {
                            Newdt.Rows.RemoveAt(Newdt.Rows.Count - 1);
                            break;
                        }
                    }
                    if (IsCheck != 0)
                    {
                        //插入
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = Newdt.NewRow();
                            
                            A_OrderBase orderbase = new A_OrderBase();
                            A_ProjectInfomation projectinfomation = new A_ProjectInfomation();

                            if (i == 0 || dt.Rows[i]["订单编号"].ToString().Trim() != dt.Rows[i - 1]["订单编号"].ToString().Trim())//同一订单号无需重复插入
                            {
                                orderbase.OrderCode = dt.Rows[i]["订单编号"].ToString().Trim();
                                orderbase.OrderName = dt.Rows[i]["订单名称"].ToString().Trim();
                                orderbase.Description = dt.Rows[i]["订单描述"].ToString().Trim();
                                orderbase.CustomerID = 0;//客户ID默认值（后续从其他表取）
                                orderbase.CustomerCode = dt.Rows[i]["客户编号"].ToString().Trim();
                                orderbase.CustomerName = dt.Rows[i]["客户名称"].ToString().Trim();
                                orderbase.State = 0;//默认为待评估
                                orderbase.OrderType = ordertype;
                                orderbase.Priority = Convert.ToInt32(dt.Rows[i]["订单优先级"]);
                                orderbase.TechnicalDirector = dt.Rows[i]["技术负责人"].ToString().Trim();
                                orderbase.ManufacturingDirector = dt.Rows[i]["生产负责人"].ToString().Trim();
                                orderbase.IsAvailable = 1;//默认为有效
                                orderbase.Remarks = dt.Rows[i]["订单备注"].ToString().Trim();
                                orderbase.Create();

                                OrderbaseList.Add(orderbase);
                            }

                            projectinfomation.ProjectCode = dt.Rows[i]["项目编号"].ToString().Trim();
                            projectinfomation.ProjectName = dt.Rows[i]["项目名称"].ToString().Trim();
                            projectinfomation.ProductID = 0;//产品ID默认（后续从其他表取）
                            projectinfomation.ProductCode = dt.Rows[i]["产品编号"].ToString().Trim();
                            projectinfomation.ProductName = dt.Rows[i]["产品名称"].ToString().Trim();

                            projectinfomation.Type = projecttype;
                            projectinfomation.Num = Convert.ToInt32(dt.Rows[i]["产品数量"]);
                            projectinfomation.Price = Convert.ToDecimal(dt.Rows[i]["单价"]);
                            projectinfomation.Unit = dt.Rows[i]["单位"].ToString().Trim();
                            projectinfomation.DeadLine = Convert.ToDateTime(dt.Rows[i]["交货期"]);
                            projectinfomation.SubPriority = Convert.ToInt32(dt.Rows[i]["项目优先级"]);
                            projectinfomation.State = 0;//默认为待评估
                            projectinfomation.IsAvailable = 1;//默认为有效
                            projectinfomation.Remarks = dt.Rows[i]["项目备注"].ToString().Trim();
                            projectinfomation.Create();

                            projectinfomationList.Add(projectinfomation);
                            if (i == (dt.Rows.Count - 1) || dt.Rows[i]["订单编号"].ToString().Trim() != dt.Rows[i + 1]["订单编号"].ToString().Trim())
                            {
                                if (IsCheck == 1)
                                {
                                    //DbTransaction isOpenTrans = database.BeginTrans();
                                    int a = repositoryfactory.Repository().Insert(OrderbaseList);
                                    if (a > 0)
                                    {
                                        int b = database.Insert(projectinfomationList);
                                        if (b > 0)
                                        {
                                            IsOk = IsOk + b;
                                            OrderbaseList.Clear();
                                        }
                                        else
                                        {
                                            A_OrderBase entity = repositoryfactory.Repository().FindEntity("OrderCode", OrderbaseList[0].OrderCode);
                                            var c = repositoryfactory.Repository().Delete(entity);//删除刚刚插入的订单
                                            OrderbaseList.Clear();
                                            dr = Newdt.NewRow();
                                            dr[0] = errorNum;
                                            dr[1] = "第[" + (i + 1).ToString() + "]行之前";
                                            dr[2] = "项目插入失败";
                                            Newdt.Rows.Add(dr);
                                            IsCheck = 0;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        dr = Newdt.NewRow();
                                        dr[0] = errorNum;
                                        dr[1] = "第[" + (i + 1).ToString() + "]行";
                                        dr[2] = "订单插入失败";
                                        Newdt.Rows.Add(dr);
                                        IsCheck = 0;
                                        break;
                                    }

                                }
                                else
                                {
                                    IsOk = 0;
                                }
                            }

                        }
                    }
                    else
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
    }
}