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
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.SystemManaModule.Controllers
{
    /// <summary>
    /// 角色管理控制器
    /// </summary>
    public class RoleManaController : PublicController<Base_Roles>
    {
        

        #region 全局变量
        public readonly RepositoryFactory<Base_Roles> repositoryfactory_Base_Roles = new RepositoryFactory<Base_Roles>();
        #endregion

        #region 视图
        public ActionResult ROLEForm()
        {
            return View();
        }
        public ActionResult AllotRoleMember()
        {
            return View();
        }
        #endregion

        #region 数据库操作表格区域
        Base_Roles3Bll MyBll = new Base_Roles3Bll();
        //BBdbR_StfBaseBll StfBaseBll = new BBdbR_StfBaseBll();
        //Base_StfRoleConfBll StfRoleConfBll = new Base_StfRoleConfBll();
        Base_ObjectUserRelationBll base_objectuserrelationbll = new Base_ObjectUserRelationBll();
        Base_UserBll UserBll = new Base_UserBll();
        #endregion

        #region 方法区

        #region 左侧树
        /// <summary>
        /// 返回树JONS
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson()
        {
            DataTable dt = MyBll.GetRoles();
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            if (DataHelper.IsExistRows(dt))
            {
                foreach (DataRow row in dt.Rows)
                {
                    string ID = row["ID"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dt, "parentid='" + ID + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = ID;
                    tree.text = row["name"].ToString();
                    tree.parentId = row["parentid"].ToString();
                    tree.Attribute = "Type";
                    tree.AttributeValue = row["Sort"].ToString();
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    tree.img = "/Content/Images/Icon16/group.png";
                    TreeList.Add(tree);
                }
            }
            return Content(TreeList.TreeToJson());
        }
        #endregion

        #region 加载所有数据
        public ActionResult GridPageList( JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = MyBll.GetRoleList(ref jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 点击树加载表格
        /// <summary>
        /// 【角色管理】返回列表JONS
        /// </summary>
        /// <param name="RoleId">角色ID</param>
        /// <param name="jqgridparam">JqGrid表格参数</param>
        /// <returns></returns>
        public ActionResult GridPageListJson(string RoleId, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = MyBll.GetPageList(RoleId,jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 4.编辑角色信息
        public ActionResult SetRoleForm(string RoleId) //主键
        {
            try
            {
                Base_Roles dt = MyBll.SetPushinfor(RoleId);
                //DataTable ListData = MyBll.SetPushinfor(RoleId);
                
                return Content(dt.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 5.保存角色信息
        public ActionResult SubmitRoleInfor(string RoleId, Base_Roles entity)
        {
            try
            {
                string Message = RoleId == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(RoleId))
                {
                    Base_Roles Oldentity = repositoryfactory.Repository().FindEntity(RoleId);//获取没更新之前实体对象
                    entity.Modify(RoleId);
                    int IsOk = repositoryfactory_Base_Roles.Repository().Update(entity);
                    this.WriteLog(IsOk, entity, Oldentity, RoleId, Message);//记录日志
                }
                else
                {
                    //entity.RoleId = Guid.NewGuid().ToString();
                    entity.Create();
                    int  IsOk = repositoryfactory_Base_Roles.Repository().Insert(entity);
                    this.WriteLog(IsOk, entity, null, RoleId, Message);//记录日志
                }
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 删除角色信息
        /// <summary>
        ///移除推送信息
        /// </summary>
        /// <param name="KeyValueuser"></param>
        /// <returns></returns>
        public ActionResult DeleteRoleInfor(string KeyValue)
        {
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功      
                //直接删除
                if (array != null && array.Length > 0)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        repositoryfactory_Base_Roles.Repository().Delete(array[i]);
                    }
                    Message = "删除成功。";
                    IsOk = 1;
                }
                WriteLog(IsOk, array, Message);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 角色用户
        /// <summary>
        /// 加载角色
        /// <param name="RoleId">对象主键</param>
        /// <param name="Category">对象分类</param>
        /// </summary>
        /// <returns></returns>
        public ActionResult ScopeRoleList()
        {
            DataTable dt = MyBll.GetRoles();
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            if (DataHelper.IsExistRows(dt))
            {
                foreach (DataRow row in dt.Rows)
                {
                    string ID = row["ID"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dt, "parentid='" + ID + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = ID;
                    tree.text = row["name"].ToString();
                    tree.parentId = row["parentid"].ToString();
                    tree.Attribute = "Type";
                    tree.AttributeValue = row["Sort"].ToString();
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    tree.img = "/Content/Images/Icon16/group.png";
                    TreeList.Add(tree);
                }
            }
            return Content(TreeList.TreeToJson());
        }
        public ActionResult ScopeRoleList_new()
        {

            DataTable dt = UserBll.GetTree();//获取树所需数据
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
        #endregion

        #region 加载角色用户
        public ActionResult MemberList( string RoleId,string Category,string DepartmentId)
        {
            StringBuilder sb = new StringBuilder();
            //DataTable ListData = StfBaseBll.GetRoleUserList(RoleId);//加载用户         
            DataTable ListData = base_objectuserrelationbll.GetList_new(RoleId, Category, DepartmentId);
            if (ListData != null && ListData.Rows.Count != 0)
            {
                foreach (DataRow item in ListData.Rows)
                {
                    string Genderimg = "user_female.png";
                    if (item["Gender"].ToString() == "1")
                    {
                        Genderimg = "user_green.png";
                    }
                    string strchecked = "";
                    if (!string.IsNullOrEmpty(item["objectid"].ToString()))//判断是否选中
                    {
                        strchecked = "selected";
                    }
                    sb.Append("<li class=\"" + strchecked + "\">");
                    sb.Append("<a class=\"a_" + strchecked + "\" id=\"" + item["UserId"] + "\" title='人员编号：" + item["Code"] + "\r\n账户：" + item["Account"] + "'><img src=\"/Content/Images/Icon16/" + Genderimg + "\">" + item["RealName"] + "</a><i></i>");
                    sb.Append("</li>");
                }
            }
            return Content(sb.ToString());
        }
        #endregion

        #region 提交角色用户
        public ActionResult AuthorizedMember(string UserId, string RoleId, string Category)
        {
            try
            {
                string[] array = UserId.Split(',');
                int IsOk = base_objectuserrelationbll.BatchAddMember(array, RoleId, Category);
                if (IsOk>0)
                {
                    Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, IsOk.ToString(), "角色用户配置成功");
                    return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
                    
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作失败。" }.ToString());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "角色用户配置操作失败，错误：" + ex.Message }.ToString());
            }
        }
        #endregion

        #endregion



    }
}