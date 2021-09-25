using HfutIE.Business;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.SystemManaModule.Controllers
{
    /// <summary>
    /// 焊装生产订单表控制器
    /// </summary>
    public class UserManaController : PublicController<BBdbR_StfBase>
    {
        #region
        #endregion

        #region 全局变量
        public readonly RepositoryFactory<BBdbR_StfBase> repositoryfactory_StfBase = new RepositoryFactory<BBdbR_StfBase>();
        public readonly RepositoryFactory<Base_User> repository_User = new RepositoryFactory<Base_User>();
        #endregion

        #region 视图
        public ActionResult ResetPassword()
        {
            ViewBag.Account = Request["Account"];
            return View();
        }
        public ActionResult AllotUserRole()
        {
            return View();
        }
        #endregion

        #region 操作数据库表格
        BBdbR_StfBaseBll MyBll = new BBdbR_StfBaseBll();
        Base_Roles3Bll Roles3Bll = new Base_Roles3Bll();
        Base_StfRoleConfBll StfRoleConfBll = new Base_StfRoleConfBll();
        Base_UserBll UserBll = new Base_UserBll();
        #endregion

        #region 方法区

        #region 1.用户树
        public ActionResult TreeJson()
        {
            try
            {
                DataTable dt = MyBll.GetTree();
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
                        tree.AttributeValue = row["sort"].ToString();
                        tree.isexpand = true;
                        tree.complete = true;
                        tree.hasChildren = hasChildren;
                        tree.img = "/Content/Images/Icon16/role.png";

                        TreeList.Add(tree);
                    }
                }
                return Content(TreeList.TreeToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 2.根据树节点加载用户信息表
        public ActionResult GridUserPageListJson(string DeptCd) //根据树节点加载表格
        {
            try
            {
                DataTable ListData = MyBll.GetUserInforList(DeptCd);
                var JsonData = new
                {
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

        #region 3.查询方法
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageByCondition(string keywords, string Condition, JqGridParam jqgridparam)
        {
            try
            {
                string keyword = keywords.Trim();
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = UserBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                return Content(ListData.ToJson());
            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 加载所有用户
        public ActionResult GridUserPageList(JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = MyBll.GetUserList(jqgridparam);
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

        #region 修改登录密码              
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="KeyValue">主键</param>
        /// <param name="Password">新密码</param>
        /// <returns></returns>
        public ActionResult ResetPasswordSubmit(string StfId, string Pssw)
        {
            try
            {
                int IsOk = 0;
                BBdbR_StfBase StfBase = new BBdbR_StfBase();
                StfBase.StfId = StfId;
                StfBase.MdfTm = DateTime.Now.ToString();
                StfBase.MdfCd = ManageProvider.Provider.Current().UserId;
                StfBase.MdfNm = ManageProvider.Provider.Current().UserName;
                StfBase.Sctkey = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
                StfBase.Pssw = Md5Helper.MD5(DESEncrypt.Encrypt(Pssw, StfBase.Sctkey).ToLower(), 32).ToLower();
                IsOk = repositoryfactory_StfBase.Repository().Update(StfBase);
                Base_SysLogBll.Instance.WriteLog(StfId, OperationType.Update, IsOk.ToString(), "修改密码");
                if (IsOk > 0)
                {
                    //更新用户表
                    Base_User Line = new Base_User();
                    Line.UserId = StfId;
                    Line.Password = StfBase.Pssw;
                    Line.Secretkey = StfBase.Sctkey;                   
                    repository_User.Repository().Update(Line);
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "密码修改成功。" }.ToString());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(StfId, OperationType.Update, "-1", "密码修改失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "密码修改失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 加载用户角色
        public ActionResult MemberList(string UserId)
        {
            StringBuilder sb = new StringBuilder();
            DataTable ListData = Roles3Bll.GetUserRoleList(UserId);//加载角色           
            if (ListData != null && ListData.Rows.Count != 0)
            {
                foreach (DataRow item in ListData.Rows)
                {
                    string Genderimg = "role.png";                    
                    string strchecked = "";
                    if (!string.IsNullOrEmpty(item["UserId"].ToString()))//判断是否选中
                    {
                        strchecked = "selected";
                    }
                    sb.Append("<li class=\"" + item["RoleTyp"] + " " + strchecked + "\">");
                    sb.Append("<a class=\"a_" + strchecked + "\" id=\"" + item["RoleId"] + "\" title='角色编号：" + item["RoleCd"] + "'><img src=\"/Content/Images/Icon16/" + Genderimg + "\">" + item["RoleNm"] + "</a><i></i>");
                    sb.Append("</li>");
                }
            }
            return Content(sb.ToString());
        }
        #endregion

        #region 提交用户角色
        public ActionResult AuthorizedMember(string RoleId, string StfId)
        {
            try
            {
                string[] array = RoleId.Split(',');//配置的角色数组
                int IsOk = StfRoleConfBll.BatchAddRole(array, StfId);
                if (IsOk > 0)
                {
                    return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作失败。" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message }.ToString());
            }
        }
        #endregion

        #endregion


    }
}