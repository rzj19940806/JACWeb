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

namespace HfutIE.WebApp.Areas.SystemManaModule.Controllers
{
    /// <summary>
    /// 用户管理控制器
    /// </summary>
    public class UserManaController : PublicController<Base_User>
    {
        

        #region 全局变量
        //public readonly RepositoryFactory<BBdbR_StfBase> repositoryfactory_StfBase = new RepositoryFactory<BBdbR_StfBase>();
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
        //BBdbR_StfBaseBll MyBll = new BBdbR_StfBaseBll();
        Base_ObjectUserRelationBll relationbll = new Base_ObjectUserRelationBll();
        //Base_Roles3Bll Roles3Bll = new Base_Roles3Bll();
        //Base_StfRoleConfBll StfRoleConfBll = new Base_StfRoleConfBll();
        Base_UserBll UserBll = new Base_UserBll();
        DepartmentBll DepartmentBll = new DepartmentBll();
        BBdbR_CompanyBaseBll CompanyBll = new BBdbR_CompanyBaseBll();
        #endregion

        #region 方法区

        #region 1.获取树
        public ActionResult TreeJson()
        {
            try
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
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 2.加载表格
        /// <summary>
        /// 【用户管理】返回用户列表JSON
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <param name="CompanyId">公司ID</param>
        /// <param name="DepartmentId">部门ID</param>
        /// <param name="ParameterJson">高级查询条件JSON</param>
        /// <param name="jqgridparam">表格参数</param>
        /// <returns></returns>
        public ActionResult GridPageListJson1(string keywords, string CompanyId, string DepartmentId, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = UserBll.GetPageList(keywords, CompanyId, DepartmentId, ParameterJson, ref jqgridparam);
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "用户信息查询成功");
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "用户信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion
        
        #region 3.表单提交
        /// <summary>
        /// 【用户管理】提交表单
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <param name="base_user">用户信息</param>
        /// <param name="base_employee">员工信息</param>
        /// <param name="BuildFormJson">自定义表单</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitUserForm(string KeyValue, Base_User base_user)
        {
            
            string ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    if (KeyValue == ManageProvider.Provider.Current().UserId)
                    {
                        throw new Exception("无权限编辑本人信息");
                    }
                    base_user.Modify(KeyValue);
                    
                    int IsOk = database.Update(base_user, isOpenTrans);
                    this.WriteLog(IsOk, base_user, null, KeyValue, Message);//记录日志       
                }
                else
                {
                    base_user.CompanyId = "451766d5-1bf9-4ecf-b63b-7ff4b19bb60e";//默认只有一个公司
                    base_user.Create();
                    base_user.SortCode = CommonHelper.GetInt(BaseFactory.BaseHelper().GetSortCode<Base_User>("SortCode"));

                    int IsOk = database.Insert(base_user, isOpenTrans);
                    this.WriteLog(IsOk, base_user, null, KeyValue, Message);//记录日志
                    //Base_DataScopePermissionBll.Instance.AddScopeDefault(ModuleId, ManageProvider.Provider.Current().UserId, base_user.UserId, "5", isOpenTrans);

                }
                
                
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                this.WriteLog(-1, base_user, null, KeyValue, "操作失败：" + ex.Message);//记录日志
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
            
        }
        #endregion

        #region 4.公司下拉框
        /// <summary>
        /// 【用户管理】返回用户列表JSON
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <param name="CompanyId">公司ID</param>
        /// <param name="DepartmentId">部门ID</param>
        /// <param name="ParameterJson">高级查询条件JSON</param>
        /// <param name="jqgridparam">表格参数</param>
        /// <returns></returns>
        public ActionResult CompanyList()
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = CompanyBll.GetCompanyList();

                return Content(ListData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 4.部门下拉框
        /// <summary>
        /// 【用户管理】返回用户列表JSON
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <param name="CompanyId">公司ID</param>
        /// <param name="DepartmentId">部门ID</param>
        /// <param name="ParameterJson">高级查询条件JSON</param>
        /// <param name="jqgridparam">表格参数</param>
        /// <returns></returns>
        public ActionResult DepartmentList()
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = DepartmentBll.GetDepartmentList();
                
                return Content(ListData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
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
                    IsOk = UserBll.Delete(array);
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

        #region 1.用户树
        //public ActionResult TreeJson()
        //{
        //    try
        //    {
        //        DataTable dt = MyBll.GetTree();
        //        List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
        //        if (DataHelper.IsExistRows(dt))
        //        {
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                string ID = row["ID"].ToString();
        //                bool hasChildren = false;
        //                DataTable childnode = DataHelper.GetNewDataTable(dt, "parentid='" + ID + "'");
        //                if (childnode.Rows.Count > 0)
        //                {
        //                    hasChildren = true;
        //                }
        //                TreeJsonEntity tree = new TreeJsonEntity();
        //                tree.id = ID;
        //                tree.text = row["name"].ToString();
        //                tree.parentId = row["parentid"].ToString();
        //                tree.Attribute = "Type";
        //                tree.AttributeValue = row["sort"].ToString();
        //                tree.isexpand = true;
        //                tree.complete = true;
        //                tree.hasChildren = hasChildren;
        //                tree.img = "/Content/Images/Icon16/role.png";

        //                TreeList.Add(tree);
        //            }
        //        }
        //        return Content(TreeList.TreeToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
        //        return null;
        //    }
        //}
        #endregion

        #region 2.根据树节点加载用户信息表
        //public ActionResult GridUserPageListJson(string DeptCd) //根据树节点加载表格
        //{
        //    try
        //    {
        //        DataTable ListData = MyBll.GetUserInforList(DeptCd);
        //        var JsonData = new
        //        {
        //            rows = ListData,
        //        };
        //        return Content(JsonData.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
        //        return null;
        //    }
        //}
        #endregion

        #region 3.查询方法
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        //public ActionResult GridPageByCondition(string keywords, string Condition, JqGridParam jqgridparam)
        //{
        //    try
        //    {
        //        string keyword = keywords.Trim();
        //        Stopwatch watch = CommonHelper.TimerStart();
        //        DataTable ListData = UserBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
        //        var JsonData = new
        //        {
        //            total = jqgridparam.total,
        //            page = jqgridparam.page,
        //            records = jqgridparam.records,
        //            costtime = CommonHelper.TimerEnd(watch),
        //            rows = ListData,
        //        };
        //        return Content(ListData.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
        //        Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
        //        return null;
        //    }
        //}
        #endregion

        #region 加载所有用户
        //public ActionResult GridUserPageList(JqGridParam jqgridparam)
        //{
        //    try
        //    {
        //        Stopwatch watch = CommonHelper.TimerStart();
        //        DataTable ListData = UserBll.GetUserList(jqgridparam);
        //        var JsonData = new
        //        {
        //            total = jqgridparam.total,
        //            page = jqgridparam.page,
        //            records = jqgridparam.records,
        //            costtime = CommonHelper.TimerEnd(watch),
        //            rows = ListData,
        //        };
        //        return Content(JsonData.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
        //        return null;
        //    }
        //}
        #endregion

        #region 修改登录密码              
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="KeyValue">主键</param>
        /// <param name="Password">新密码</param>
        /// <returns></returns>
        //public ActionResult ResetPasswordSubmit(string StfId, string Pssw)
        //{
        //    try
        //    {
        //        int IsOk = 0;
        //        BBdbR_StfBase StfBase = new BBdbR_StfBase();
        //        StfBase.StfId = StfId;
        //        StfBase.MdfTm = DateTime.Now.ToString();
        //        StfBase.MdfCd = ManageProvider.Provider.Current().UserId;
        //        StfBase.MdfNm = ManageProvider.Provider.Current().UserName;
        //        StfBase.Sctkey = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
        //        StfBase.Pssw = Md5Helper.MD5(DESEncrypt.Encrypt(Pssw, StfBase.Sctkey).ToLower(), 32).ToLower();
        //        IsOk = repositoryfactory_StfBase.Repository().Update(StfBase);
        //        Base_SysLogBll.Instance.WriteLog(StfId, OperationType.Update, IsOk.ToString(), "修改密码");
        //        if (IsOk > 0)
        //        {
        //            //更新用户表
        //            Base_User Line = new Base_User();
        //            Line.UserId = StfId;
        //            Line.Password = StfBase.Pssw;
        //            Line.Secretkey = StfBase.Sctkey;                   
        //            repository_User.Repository().Update(Line);
        //        }
        //        return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "密码修改成功。" }.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        Base_SysLogBll.Instance.WriteLog(StfId, OperationType.Update, "-1", "密码修改失败：" + ex.Message);
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "密码修改失败：" + ex.Message }.ToString());
        //    }
        //}
        #endregion

        #region 加载用户角色
        //public ActionResult MemberList(string UserId)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    DataTable ListData = Roles3Bll.GetUserRoleList(UserId);//加载角色           
        //    if (ListData != null && ListData.Rows.Count != 0)
        //    {
        //        foreach (DataRow item in ListData.Rows)
        //        {
        //            string Genderimg = "role.png";                    
        //            string strchecked = "";
        //            if (!string.IsNullOrEmpty(item["UserId"].ToString()))//判断是否选中
        //            {
        //                strchecked = "selected";
        //            }
        //            sb.Append("<li class=\"" + item["RoleTyp"] + " " + strchecked + "\">");
        //            sb.Append("<a class=\"a_" + strchecked + "\" id=\"" + item["RoleId"] + "\" title='角色编号：" + item["RoleCd"] + "'><img src=\"/Content/Images/Icon16/" + Genderimg + "\">" + item["RoleNm"] + "</a><i></i>");
        //            sb.Append("</li>");
        //        }
        //    }
        //    return Content(sb.ToString());
        //}
        #endregion

        #region 提交用户角色
        //public ActionResult AuthorizedMember(string RoleId, string StfId)
        //{
        //    try
        //    {
        //        string[] array = RoleId.Split(',');//配置的角色数组
        //        int IsOk = relationbll.BatchAddObject(StfId, array, "2");
        //        if (IsOk > 0)
        //        {
        //            return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
        //        }
        //        return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作失败。" }.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message }.ToString());
        //    }
        //}
        #endregion

        #region 2.加载冻结数据
        /// <summary>
        /// 【用户管理】返回用户列表JSON
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <param name="CompanyId">公司ID</param>
        /// <param name="DepartmentId">部门ID</param>
        /// <param name="ParameterJson">高级查询条件JSON</param>
        /// <param name="jqgridparam">表格参数</param>
        /// <returns></returns>
        //public ActionResult GridPageFreezeList(string keywords, string CompanyId, string DepartmentId, string ParameterJson, JqGridParam jqgridparam)
        //{
        //    try
        //    {
        //        Stopwatch watch = CommonHelper.TimerStart();
        //        DataTable ListData = UserBll.GetPageListFreeze(keywords, CompanyId, DepartmentId, ParameterJson, ref jqgridparam);
        //        var JsonData = new
        //        {
        //            total = jqgridparam.total,
        //            page = jqgridparam.page,
        //            records = jqgridparam.records,
        //            costtime = CommonHelper.TimerEnd(watch),
        //            rows = ListData,
        //        };
        //        return Content(JsonData.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
        //        return null;
        //    }
        //}
        #endregion

        #region 3.冻结账号
        /// <summary>
        /// 【用户管理】提交表单
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <param name="base_user">用户信息</param>
        /// <param name="base_employee">员工信息</param>
        /// <param name="BuildFormJson">自定义表单</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFreeze(string KeyValue)
        {

            string ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            string[] array = KeyValue.Split(',');
            try
            {
                if (array != null && array.Length > 0)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (KeyValue == ManageProvider.Provider.Current().UserId)
                        {
                            throw new Exception("无权限编辑本人信息");
                        }
                        
                        Base_User Oldentity = database.FindEntityBySql<Base_User>($"SELECT * FROM Base_User WHERE UserId = '{array[i]}'");//获取没更新之前实体对象
                        Oldentity.DepartmentId = "54d98ff2-5399-4043-8983-57a967fdd74f";
                        Oldentity.Enabled = "2";
                        database.Update(Oldentity, isOpenTrans);
                    }
                    
                }
                string Message = "冻结成功";
                
                database.Commit();
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "冻结成功");
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "冻结操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }

        }
        #endregion

        #region 7.编辑填充界面数据
        /// <summary>
        /// 填充界面数据
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public override ActionResult SetForm(string KeyValue)
        {
            try
            {
                Base_User dt = UserBll.GetUserList(KeyValue);

                return Content(dt.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 重构导出
        public ActionResult GetExcel_Data(string keywords, string CompanyId, string DepartmentId)
        {
            try
            {

                DataTable ListData = UserBll.GetExcel_Data(keywords, CompanyId, DepartmentId);
                ListData.Columns.Remove("CompanyId");
                ListData.Columns.Remove("DepartmentId");
                string fileName = "账号管理";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_UserMana(ListData, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "账号管理导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "账号管理导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        #endregion


    }
}