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
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.CommonModule.Controllers
{
    /// <summary>
    /// 齐套校核控制器
    /// </summary>
    public class DemandCheckController : PublicController<A_MaterialProgramDemand>
    {
        A_ProjectPlanInfomationBll projectplaninfomationbll = new A_ProjectPlanInfomationBll();
        A_MaterialProgramDemandBll materialprogramdemandbll = new A_MaterialProgramDemandBll();
        //Base_DepartmentBll base_departmentbll = new Base_DepartmentBll();
        A_CutterDemandBll cutterdemandbll = new A_CutterDemandBll();
        /// <summary>
        /// 返回 订单、项目、计划 树JONS
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson()
        {
            var dt = projectplaninfomationbll.GetOrderTree();
            List<TreeJsonEntity> treeList = new List<TreeJsonEntity>();
            if (DataHelper.IsExistRows(dt))
            {
                dt.Columns.Add("ID");
                dt.Columns.Add("ParentID");
                foreach(DataRow row1 in dt.Rows)
                {
                    switch (row1["Sort"].ToString())
                    {
                        case "AllOrder":
                            row1["ID"] = "1";
                            row1["ParentID"] = "0";
                            break;
                        case "Order":
                            row1["ID"] = "O" + row1["EntityId"];
                            row1["ParentID"] = "1" ;
                            break;
                        case "Project":
                            row1["ID"] = "PROJ" + row1["EntityId"];
                            row1["ParentID"] = "O" + row1["EntityParentId"];
                            break;
                        //case "Plan":
                        //    row1["ID"] = "PLAN" + row1["EntityId"];
                        //    row1["ParentID"] = "PROJ" + row1["EntityParentId"];
                        //    break;
                    }
                }
                foreach (DataRow row in dt.Rows)
                {
                    var ID = row["ID"].ToString();

                    var hasChildren = false;
                    var childnode = DataHelper.GetNewDataTable(dt, "ParentId = '" + ID + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = ID.Trim();
                    tree.text = row["EntityName"].ToString().Trim();
                    tree.value = row["EntityName"].ToString().Trim();
                    tree.parentId = row["ParentId"].ToString().Trim();
                    tree.Attribute = "Type";
                    tree.AttributeValue = row["Sort"].ToString();
                    tree.AttributeA = "ID";
                    tree.AttributeValueA = row["ID"].ToString();
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    if (row["ID"].ToString().Trim() == "1")
                    {
                        tree.img = "/Content/Images/Icon32/folders_explorer.png";
                    }
                    else if (row["ParentId"].ToString().Trim() == "1")
                    {
                        tree.img = "/Content/Images/Icon32/folder_page.png";
                    }
                    else /*(row["Sort"].ToString().Trim() == "Project")*/
                    {
                        tree.img = "/Content/Images/Icon16/calculate_sheet.png";
                        //tree.img = "/Content/Images/Icon32/module.png";
                    }
                    //else
                    //{
                    //    tree.img = "/Content/Images/Icon16/calculate_sheet.png";
                    //}

                    treeList.Add(tree);
                }
            }
            return Content(treeList.TreeToJson());
        }
        /// <summary>
        /// 登录用户身份确认
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRoleType()
        {
            string userID = ManageProvider.Provider.Current().UserId;
            userID = "2020170280";  //临时用户（后续需修改）82：刀具；81：物料；80：程序
            Base_User users = DataFactory.Database().FindEntity<Base_User>("Code", userID);
            Base_ObjectUserRelation entity = DataFactory.Database().FindEntity<Base_ObjectUserRelation>("UserID", users.UserId);
            Base_Roles roleentity = DataFactory.Database().FindEntity<Base_Roles>("ID", entity.ObjectId);
            var role = roleentity.RoleNm;//1.物料管理员2.程序管理员3.刀具管理员
            return Content("\"" + role + "\"");
        }
        /// <summary>
        /// 加载计划表
        /// </summary>
        /// <param name="CheckId"></param>
        /// <param name="Role"></param>
        /// <returns></returns>
        public ActionResult GridPlanList(string CheckId, string Role)
        {
             
            List<A_ProjectPlanInfomation> PlanList = projectplaninfomationbll.GetPlanList(CheckId, Role);
            var JsonData = new
            {
                rows = PlanList,
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 【物料齐套刀具任务表】返回表格JONS
        /// </summary>
        /// <param name="CheckId">公司ID</param>
        /// <returns></returns>
        public ActionResult GridMateralList(string CheckId,string Role)
        {
            List<A_MaterialProgramDemand> MatList = materialprogramdemandbll.GetList(CheckId, Role);
            var JsonData = new
            {
                rows = MatList,
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 【刀具齐套任务表】返回表格JONS
        /// </summary>
        /// <param name="CheckId">主键</param>
        /// <returns></returns>
        public ActionResult GridCutterList(string CheckId)
        {
            List<A_CutterDemand> CutList = cutterdemandbll.GetList(CheckId);
            var JsonData = new
            {
                rows = CutList,
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 根据查询条件搜物料程序信息
        /// </summary>
        /// <param name="Condition"></param>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        public ActionResult GetMateralList(string Condition ,string Keyword,string Role)
        {
            List<A_ProjectPlanInfomation> CutList = projectplaninfomationbll.GetList(Condition,  Keyword,  Role);
            var JsonData = new
            {
                rows = CutList,
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 根据查询条件搜刀具信息
        /// </summary>
        /// <param name="Condition"></param>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        public ActionResult GetCutterList(string Condition, string Keyword)
        {
            List<A_ProjectPlanInfomation> CutList = projectplaninfomationbll.GetList(Condition,  Keyword);
            var JsonData = new
            {
                rows = CutList,
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 齐套确认
        /// </summary>
        /// <param name="Role"></param>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult ConfirmCheck(string Role,string KeyValue)
        {
            //var sum = 0;
            string[] arr = KeyValue.Split(',');
            if(Role == "物料管理员"|| Role == "程序管理员")
            {
                for(var i = 0;i < arr.Length;i++)
                {
                     var count = materialprogramdemandbll.ConfirmState(arr[i].ToString(), Role);
                    if (count == 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "齐套确认失败" }.ToString());
                    }
                }
            }
            else
            {
                for (var i = 0; i < arr.Length; i++)
                {
                    var count = cutterdemandbll.ConfirmState(arr[i].ToString());
                    if(count == 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "齐套确认失败" }.ToString());
                    }
                }
            }
            return Content(new JsonMessage { Success = true, Code = "2", Message = "齐套确认成功" }.ToString());
            
        }
        /// <summary>
        /// 【部门管理】根据公司id获取部门列表返回树JONS
        /// </summary>
        /// <param name="CompanyId">公司Id</param>
        /// <returns></returns>
        //public ActionResult DepartmentTreeJson(string CompanyId)
        //{
        //    DataTable ListData = base_departmentbll.GetList(CompanyId);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("[");
        //    foreach (DataRow item in ListData.Rows)
        //    {
        //        sb.Append("{");
        //        sb.Append("\"id\":\"" + item["departmentid"] + "\",");
        //        sb.Append("\"text\":\"" + item["fullname"] + "\",");
        //        sb.Append("\"value\":\"" + item["departmentid"] + "\",");
        //        sb.Append("\"img\":\"/Content/Images/Icon16/chart_organisation.png\",");
        //        sb.Append("\"isexpand\":true,");
        //        sb.Append("\"hasChildren\":false");
        //        sb.Append("},");
        //    }
        //    sb = sb.Remove(sb.Length - 1, 1);
        //    sb.Append("]");
        //    return Content(sb.ToString());
        //}
        /// <summary>
        /// 异常信息表单赋值
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SetAbnormalForm(string KeyValue,string Role)
        {
            IDatabase database = DataFactory.Database();
            if(Role == "刀具管理员")
            {
                A_CutterDemand CutList = cutterdemandbll.SetList(KeyValue);
                return Content(CutList.ToJson());
            }
            else
            {
                A_MaterialProgramDemand SetValue = materialprogramdemandbll.SetList( KeyValue,  Role);
                return Content(SetValue.ToJson());
            }
        }
        /// <summary>
        /// 物料程序异常备注提交表单
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="KeyValue">主键值</param>
        /// <param name="BuildFormJson">自定义表单</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SubmitMateralForm(A_MaterialProgramDemand entity, string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            //DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                int IsOk = 0;
                var Message = "异常信息录入失败。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    IsOk = materialprogramdemandbll.UpdateRemarks(KeyValue, entity.Remarks);
                    if (IsOk > 0)
                    {
                        Message = "异常信息录入成功";
                    }
                }
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 刀具异常备注提交表单
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="KeyValue">主键值</param>
        /// <param name="BuildFormJson">自定义表单</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SubmitCutterForm(A_CutterDemand entity, string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            //DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                int IsOk = 0;
                var Message = "异常信息录入失败。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    IsOk = cutterdemandbll.UpdateRemarks(KeyValue, entity.Remarks);
                    if (IsOk > 0)
                    {
                        Message = "异常信息录入成功";
                    }
                }
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
    }
}