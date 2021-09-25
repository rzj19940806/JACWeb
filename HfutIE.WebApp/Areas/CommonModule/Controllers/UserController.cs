using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using Newtonsoft.Json;
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
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.CommonModule.Controllers
{
    /// <summary>
    /// 用户管理控制器
    /// </summary>
    public class UserController : PublicController<Base_User>
    {
        Base_UserBll base_userbll = new Base_UserBll();
        Base_CompanyBll base_companybll = new Base_CompanyBll();
        Base_ObjectUserRelationBll base_objectuserrelationbll = new Base_ObjectUserRelationBll();
        public readonly RepositoryFactory<BBdbR_StfBase> repositoryfactory2 = new RepositoryFactory<BBdbR_StfBase>();

        #region 用户管理

        public ActionResult TreeJson()
        {
            DataTable DataList = base_userbll.TreeUserList();
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            if (DataHelper.IsExistRows(DataList))
            {
                foreach (DataRow row in DataList.Rows)
                {
                    TreeJsonEntity tree = new TreeJsonEntity();
                    string id = row["id"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(DataList, "parentid='" + id + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    tree.Attribute = "Type";
                    tree.AttributeValue = row["sort"].ToString();
                    tree.id = id;
                    tree.text = row["fullname"].ToString();
                    tree.value = id;
                    tree.parentId = row["parentid"].ToString();
                    tree.title = row["code"].ToString();
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    if (row["parentid"].ToString() == "0")
                    {
                        tree.img = "/Content/Images/Icon16/molecule.png";
                    }
                    else if (row["sort"].ToString() == "Company")
                    {
                        tree.img = "/Content/Images/Icon16/hostname.png";
                    }
                    else if (row["sort"].ToString() == "Department")
                    {
                        tree.img = "/Content/Images/Icon16/chart_organisation.png";
                    }
                    else
                    {
                        string Genderimg = "user_female.png";
                        if (row["gender"].ToString() == "男")
                        {
                            Genderimg = "user_green.png";
                        }
                        tree.img = "/Content/Images/Icon16/" + Genderimg;
                    }
                    TreeList.Add(tree);
                }
            }
            return Content(TreeList.TreeToJson());
        }

        //public ActionResult TreeJson()
        //{
        //    DataTable dt = base_userbll.GetTree();
        //    List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
        //    if (DataHelper.IsExistRows(dt))
        //    {
        //        foreach (DataRow row in dt.Rows)//dt是部门表的所有数据加上parentid（一列全部为零），key是表的主键
        //        {
        //            string Treekey = row["Treekey"].ToString();//主键下面是否有子节点
        //            bool hasChildren = false;//检测是否有树结构的子节点
        //            DataTable childnode = DataHelper.GetNewDataTable(dt, "parentid='" + Treekey + "'");//设置的0为parentid，只要的0为设置的0为parentid的都是子节点
        //            if (childnode.Rows.Count > 0)
        //            {
        //                hasChildren = true;
        //            }
        //            TreeJsonEntity tree = new TreeJsonEntity();
        //            tree.id = Treekey;
        //            tree.text = row["Name"].ToString();
        //            tree.value = row["Code"].ToString();
        //            tree.parentId = row["parentid"].ToString();
        //            tree.Attribute = "image";
        //            tree.AttributeValue = row["image"].ToString();
        //            tree.isexpand = true;
        //            tree.complete = true;
        //            tree.hasChildren = hasChildren;//如果为true则显示加号，有下个节点
        //            if (row["image"].ToString() == "1")//公司
        //            {
        //                tree.img = "/Content/Images/Icon16/house.png";
        //            }
        //            else if (row["image"].ToString() == "2")//工厂
        //            {
        //                tree.img = "/Content/Images/Icon16/factory.png";
        //            }
        //            else if (row["image"].ToString() == "3")//部门
        //            {
        //                tree.img = "/Content/Images/Icon16/outlook_new_meeting.png";//不是子节点
        //            }
        //            TreeList.Add(tree);
        //        }
        //    }
        //    return Content(TreeList.TreeToJson());
        //}

        /// <summary>
        /// 查询前面50条用户信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult Autocomplete(string keywords)
        {
            DataTable ListData = base_userbll.OptionUserList(keywords);
            return Content(ListData.ToJson());
        }
        /// <summary>
        /// 【用户管理】返回用户列表JSON
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <param name="CompanyId">公司ID</param>
        /// <param name="DepartmentId">部门ID</param>
        /// <param name="ParameterJson">高级查询条件JSON</param>
        /// <param name="jqgridparam">表格参数</param>
        /// <returns></returns>
        public ActionResult GridPageListJson(string keywords, string CompanyId, string DepartmentId, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = base_userbll.GetPageList(keywords, CompanyId, DepartmentId, ParameterJson, ref jqgridparam);
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
        /// <summary>
        /// 【用户管理】提交表单
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <param name="base_user">用户信息</param>
        /// <param name="base_employee">员工信息</param>
        /// <param name="BuildFormJson">自定义表单</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitUserForm(string KeyValue, Base_User base_user, Base_Employee base_employee, string BuildFormJson)
        {
            byte[] photograph = GetPhotographFromRequest();//获取图片的二进制,没值
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
                    base_employee.Modify(KeyValue);
                    database.Update(base_user, isOpenTrans);
                    database.Update(base_employee, isOpenTrans);
                    //20210623加_部分数据同步修改BBdbR_StfBase
                    BBdbR_StfBase stfbase = new BBdbR_StfBase();
                    stfbase.StfId = KeyValue;
                    stfbase.StfCd = base_user.Code;
                    stfbase.StfNm = base_user.RealName;
                    stfbase.Wechat = base_user.OICQ;
                    stfbase.Email= base_user.Email;
                    //stfbase.InnerUser = base_user.InnerUser;
                    stfbase.Account = base_user.Account;
                    stfbase.Pssw = base_user.Password;
                    stfbase.StfGndr = base_user.Gender;
                    stfbase.Phn = base_user.Mobile;
                    stfbase.Sctkey = base_user.Secretkey;
                    stfbase.DepartmentID = base_user.DepartmentId;
                    stfbase.Rem = base_user.Remark;
                    stfbase.Enabled = Convert.ToString(base_user.Enabled);
                    database.Update(stfbase, isOpenTrans);
                }
                else
                {
                    base_user.Create();
                    base_user.SortCode = CommonHelper.GetInt(BaseFactory.BaseHelper().GetSortCode<Base_User>("SortCode"));
                    base_employee.Create();
                    base_employee.EmployeeId = base_user.UserId;
                    base_employee.UserId = base_user.UserId;//主键和外键是一个值，主键不再是生成乱码
                    database.Insert(base_user, isOpenTrans);
                    database.Insert(base_employee, isOpenTrans);
                    Base_DataScopePermissionBll.Instance.AddScopeDefault(ModuleId, ManageProvider.Provider.Current().UserId, base_user.UserId, "5", isOpenTrans);
                    //20210623加_部分数据插入BBdbR_StfBase
                    BBdbR_StfBase stfbase = new BBdbR_StfBase();
                    stfbase.StfId = base_user.UserId;                    
                    stfbase.StfCd = base_user.Code;
                    stfbase.StfNm = base_user.RealName;
                    stfbase.Wechat = base_user.OICQ;
                    stfbase.Email = base_user.Email;
                    //stfbase.InnerUser = base_user.InnerUser;
                    stfbase.Account = base_user.Account;
                    stfbase.Pssw = base_user.Password;
                    stfbase.StfGndr = base_user.Gender;
                    stfbase.Phn = base_user.Mobile;
                    stfbase.Sctkey = base_user.Secretkey;
                    stfbase.DepartmentID = base_user.DepartmentId;
                    stfbase.Rem = base_user.Remark;
                    stfbase.Enabled = Convert.ToString(base_user.Enabled);
                    database.Insert(stfbase, isOpenTrans);
                }
                #region 新增处理人员图片

                //USER_PHOTOGRAPH user_photograph = GetUserPhotograph(base_user);//通过USER获取USER_PHOTOGRAPH的实体
                //StringBuilder str = GetSQLStr(KeyValue, user_photograph);
                //DbParameter[] sqlparams = GetParameter(user_photograph);
                //database.ExecuteBySql(str, sqlparams, isOpenTrans);
                #endregion

                //Base_FormAttributeBll.Instance.SaveBuildForm(BuildFormJson, base_user.UserId, ModuleId, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }

            //string ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            //IDatabase database = DataFactory.Database();
            //DbTransaction isOpenTrans = database.BeginTrans();
            //try
            //{
            //    string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
            //    if (!string.IsNullOrEmpty(KeyValue))
            //    {
            //        if (KeyValue == ManageProvider.Provider.Current().UserId)
            //        {
            //            throw new Exception("无权限编辑本人信息");
            //        }
            //        base_user.Modify(KeyValue);
            //        base_employee.Modify(KeyValue);
            //        database.Update(base_user, isOpenTrans);
            //        database.Update(base_employee, isOpenTrans);
            //    }
            //    else
            //    {
            //        base_user.Create();
            //        base_user.SortCode = CommonHelper.GetInt(BaseFactory.BaseHelper().GetSortCode<Base_User>("SortCode"));
            //        base_employee.Create();
            //        base_employee.EmployeeId = base_user.UserId;
            //        base_employee.UserId = base_user.UserId;
            //        database.Insert(base_user, isOpenTrans);
            //        database.Insert(base_employee, isOpenTrans);
            //        Base_DataScopePermissionBll.Instance.AddScopeDefault(ModuleId, ManageProvider.Provider.Current().UserId, base_user.UserId, "5", isOpenTrans);
            //    }
            //    Base_FormAttributeBll.Instance.SaveBuildForm(BuildFormJson, base_user.UserId, ModuleId, isOpenTrans);
            //    database.Commit();
            //    return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            //}
            //catch (Exception ex)
            //{
            //    database.Rollback();
            //    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            //}
        }

        /// <summary>
        /// 获取用户职员信息对象返回JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetUserForm(string KeyValue)
        {
            Base_User base_user = DataFactory.Database().FindEntity<Base_User>(KeyValue);
            if (base_user == null)
            {
                return Content("");
            }          
            Base_Employee base_employee = DataFactory.Database().FindEntity<Base_Employee>(KeyValue);
            //Base_Company base_company = DataFactory.Database().FindEntity<Base_Company>(base_user.CompanyId);
            string strJson = base_user.ToJson();
            //公司
            //strJson = strJson.Insert(1, "\"CompanyName\":\"" + base_company.FullName + "\",");
            //员工信息
            strJson = strJson.Insert(1, base_employee.ToJson().Replace("{", "").Replace("}", "") + ",");
            //自定义
            //strJson = strJson.Insert(1, Base_FormAttributeBll.Instance.GetBuildForm(KeyValue));
            return Content(strJson);
        }
        #endregion

        #region 修改登录密码
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPassword()
        {
            ViewBag.Account = Request["Account"];
            return View();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="KeyValue">主键</param>
        /// <param name="Password">新密码</param>
        /// <returns></returns>
        public ActionResult ResetPasswordSubmit(string KeyValue, string Password)
        {
            try
            {
                int IsOk = 0;
                Base_User base_user = new Base_User();
                base_user.UserId = KeyValue;
                base_user.ModifyDate = DateTime.Now;
                base_user.ModifyUserId = ManageProvider.Provider.Current().UserId;
                base_user.ModifyUserName = ManageProvider.Provider.Current().UserName;
                base_user.Secretkey = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
                base_user.Password = Md5Helper.MD5(DESEncrypt.Encrypt(Password, base_user.Secretkey).ToLower(), 32).ToLower();
                IsOk = repositoryfactory.Repository().Update(base_user);
                Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk.ToString(), "修改密码");
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "密码修改成功。" }.ToString());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, "-1", "密码修改失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "密码修改失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 用户角色
        /// <summary>
        /// 用户角色
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult UserRole()
        {
            return View();
        }
        /// <summary>
        /// 加载用户角色
        /// </summary>
        /// <param name="CompanyId">公司ID</param>
        /// <param name="UserId">用户Id</param>
        /// <returns></returns>
        public ActionResult UserRoleList(string CompanyId, string UserId)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = base_userbll.UserRoleList(CompanyId, UserId);
            foreach (DataRow dr in dt.Rows)
            {
                string strchecked = "";
                if (!string.IsNullOrEmpty(dr["objectid"].ToString()))//判断是否选中
                {
                    strchecked = "selected";
                }
                sb.Append("<li title=\"" + dr["fullname"] + "(" + dr["code"] + ")" + "\" class=\"" + strchecked + "\">");
                sb.Append("<a id=\"" + dr["RoleId"] + "\"><img src=\"../../Content/Images/Icon16/role.png \">" + dr["fullname"] + "</a><i></i>");
                sb.Append("</li>");
            }
            return Content(sb.ToString());
        }
        /// <summary>
        /// 用户角色 - 提交保存
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <param name="ObjectId">角色id:1,2,3,4,5,6</param>
        /// <returns></returns>
        public ActionResult UserRoleSubmit(string UserId, string ObjectId)
        {
            try
            {
                string[] array = ObjectId.Split(',');
                int IsOk = base_objectuserrelationbll.BatchAddObject(UserId, array, "2");
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message }.ToString());
            }
        }
        #endregion

        /// <summary>
        /// 删除数据(20201028WT改_公共方法改为特定_为实现BBdbR_StfBase的数据处理)
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <param name="ParentId">判断是否有子节点</param>
        /// <returns></returns>
        [HttpPost]
        [ManagerPermission(PermissionMode.Enforce)]
        public virtual ActionResult DeleteNew(string KeyValue, string ParentId, string DeleteMark)
        {
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";
                int IsOk = 0;
                if (!string.IsNullOrEmpty(ParentId))
                {
                    if (repositoryfactory.Repository().FindCount("ParentId", ParentId) > 0)
                    {
                        throw new Exception("当前所选有子节点数据，不能删除。");
                    }
                }
                //直接删除
                if (string.IsNullOrEmpty(DeleteMark))
                {
                    //20210623删除BBdbR_StfBase的相同主键的数据
                    repositoryfactory2.Repository().Delete(array);
                    IsOk = repositoryfactory.Repository().Delete(array);
                    if (IsOk > 0)
                    {
                        Message = "删除成功。";
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

        #region 个人中心
        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonCenter()
        {
            if (ManageProvider.Provider.Current().IsSystem)
            {
                throw new Exception("系统超级管理员拥有所有权限");
            }
            if (ManageProvider.Provider.Current().Gender == "男")
            {
                ViewBag.imgGender = "man.png";
            }
            else
            {
                ViewBag.imgGender = "woman.png";
            }
            ViewBag.strUserInfo = ManageProvider.Provider.Current().UserName + "（" + ManageProvider.Provider.Current().Account + "）";
            return View();
        }
        /// <summary>
        /// 验证旧密码
        /// </summary>
        /// <param name="OldPassword"></param>
        /// <returns></returns>
        public ActionResult ValidationOldPassword(string OldPassword)
        {
            if (ManageProvider.Provider.Current().Account == "System" || ManageProvider.Provider.Current().Account == "guest")
            {
                return Content(new JsonMessage { Success = true, Code = "0", Message = "当前账户不能修改密码" }.ToString());
            }
            OldPassword = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(OldPassword, 32).ToLower(), ManageProvider.Provider.Current().Secretkey).ToLower(), 32).ToLower();
            if (OldPassword != ManageProvider.Provider.Current().Password)
            {
                return Content(new JsonMessage { Success = true, Code = "0", Message = "原密码错误，请重新输入" }.ToString());
            }
            else
            {
                return Content(new JsonMessage { Success = true, Code = "1", Message = "通过信息验证" }.ToString());
            }
        }
        #endregion

        #region 修改后的部分（上传图片）
        /// <summary>
        /// 根据form提交数据，生成USER_PHOTOGRAPH实体
        /// </summary>
        /// <param name="base_user">用户实体信息</param>
        /// <returns></returns>
        public USER_PHOTOGRAPH GetUserPhotograph(Base_User base_user)
        {
            try
            {
                long length = GetPhotographContentLength();
                byte[] photograph = new byte[length];
                photograph = GetPhotographFromRequest();//获取图片的二进制
                USER_PHOTOGRAPH user_photograph = new USER_PHOTOGRAPH();
                user_photograph.Create();
                user_photograph.photograph = photograph;//将图片二进制赋值给实体
                Type type = typeof(USER_PHOTOGRAPH);
                PropertyInfo[] props = type.GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    if (prop.Name != null)//如果属性名不为空，验证USER对象是否含有该属性
                    {
                        PropertyInfo _findedPropertyInfo = base_user.GetType().GetProperty(prop.Name);//验证USER对象是否含有该属性
                        if (_findedPropertyInfo != null)//如果USER对象含有该属性
                        {
                            object value = _findedPropertyInfo.GetValue(base_user, null);//获取USER对象的属性值
                            if (value != DBNull.Value)//返回null是没有你查询的结果集（比如没有满足指定where条件的），而返回DBNull是查询到了结果，但是里面存储的值是空值
                            {
                                //DBNull.Value;
                                prop.SetValue(user_photograph, value, null);//将属性值赋值给user_photograph对象
                            }
                        }
                    }
                }
                return user_photograph;
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        public StringBuilder GetSQLStr(string KeyValue, USER_PHOTOGRAPH user_photograph)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (KeyValue == "")
                {
                    sb = InsertSql(user_photograph);
                }
                else
                {
                    sb = UpdateSql(user_photograph);
                }
                return sb;
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 插入SQL语句
        /// </summary>
        /// <param name="user_photograph">实体</param>
        /// <returns></returns>
        public static StringBuilder InsertSql(USER_PHOTOGRAPH user_photograph)
        {
            Type type = typeof(USER_PHOTOGRAPH);
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert Into ");
            sb.Append(type.Name);
            sb.Append("(");
            StringBuilder sp = new StringBuilder();
            StringBuilder sb_prame = new StringBuilder();
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(user_photograph, null) != null)
                {
                    sb_prame.Append("," + (prop.Name));
                    sp.Append("," + DbHelper.DbParmChar + "" + (prop.Name));
                }
            }
            sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
            sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
            return sb;
        }
        /// <summary>
        /// 更新SQL语句
        /// </summary>
        /// <param name="user_photograph">实体</param>
        /// <returns></returns>
        public static StringBuilder UpdateSql(USER_PHOTOGRAPH user_photograph)
        {
            string pkName = "user_photograph_key";//主键名
            Type type = user_photograph.GetType();
            PropertyInfo[] props = type.GetProperties();
            StringBuilder sb = new StringBuilder();
            sb.Append("Update ");
            sb.Append(type.Name);
            sb.Append(" Set ");
            bool isFirstValue = true;
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(user_photograph, null) != null && pkName != prop.Name)
                {
                    if (isFirstValue)
                    {
                        isFirstValue = false;
                        sb.Append(prop.Name);
                        sb.Append("=");
                        sb.Append(DbHelper.DbParmChar + prop.Name);
                    }
                    else
                    {
                        sb.Append("," + prop.Name);
                        sb.Append("=");
                        sb.Append(DbHelper.DbParmChar + prop.Name);
                    }
                }
            }
            sb.Append(" Where ").Append(pkName).Append("=").Append(DbHelper.DbParmChar + pkName);
            return sb;
        }
        /// <summary>
        /// 获取SQL参数
        /// </summary>
        /// <typeparam name="T">泛型参数</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public static DbParameter[] GetParameter<T>(T entity)
        {
            IList<DbParameter> parameter = new List<DbParameter>();
            DbType dbtype = new DbType();
            Type type = entity.GetType();
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo pi in props)
            {
                if (pi.GetValue(entity, null) != null)
                {
                    switch (pi.PropertyType.ToString())
                    {
                        case "System.Nullable`1[System.Int32]":
                            dbtype = DbType.Int32;
                            break;
                        case "System.Nullable`1[System.Decimal]":
                            dbtype = DbType.Decimal;
                            break;
                        case "System.Nullable`1[System.DateTime]":
                            dbtype = DbType.DateTime;
                            break;
                        case "System.Byte[]":
                            dbtype = DbType.Binary;
                            break;
                        default:
                            dbtype = DbType.String;
                            break;
                    }
                    parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + pi.Name, pi.GetValue(entity, null), dbtype));
                }
            }
            return parameter.ToArray();
        }
        public long GetPhotographContentLength()
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files["Photograph"];//获取上传的文件
                Stream st = file.InputStream;
                return st.Length;
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 从request中获取上传图片的二进制
        /// </summary>
        /// <returns></returns>
        public byte[] GetPhotographFromRequest()
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files["Photograph"];//获取上传的文件
                Stream st = file.InputStream;
                byte[] bytes = new byte[st.Length];
                BinaryReader br = new BinaryReader((Stream)st);
                bytes = br.ReadBytes((Int32)st.Length);
                return bytes;
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 校验选中的图片的像素大小是否属于合理范围
        /// </summary>
        /// <returns></returns>
        public bool CheckPhotograph()
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files["file"];//获取上传的文件
                Stream st = file.InputStream;
                byte[] bytes = new byte[st.Length];
                BinaryReader br = new BinaryReader((Stream)st);
                bytes = br.ReadBytes((Int32)st.Length);
                bool is_standard = CheckPictureSize(bytes);
                if (is_standard == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return false;
            }
        }

        #region 二进制流转换成图片  
        /// <summary>
        /// 将二进制流转化为图片，从而获取该图片的像素大小，并判断图片像素是否不大于标准值
        /// </summary>
        /// <param name="pageData">图片的二进制流</param>
        /// <returns></returns>
        public bool CheckPictureSize(byte[] pageData)
        {
            //将二进制流数据转换为图片  
            System.Drawing.Image image = Image.FromStream(new MemoryStream(pageData));
            int width = image.Width;
            int height = image.Height;
            NameValueCollection forms = Request.Form;
            int width_stadard = Convert.ToInt16(forms["width"]);
            int height_stadard = Convert.ToInt16(forms["height"]);
            if (width < width_stadard && height < height_stadard)//图片像素大小小于标准值，返回true
            {
                return true;
            }
            else//图片像素大小大于标准值，返回false
            {
                return false;
            }
        }
        #endregion

        #region 从request中获取Base_User对象和Base_Employee对象（未使用）

        public Base_User GetUser()
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files["DepartmentId"];//获取上传的文件
                NameValueCollection forms = Request.Form;
                Base_User user = new Base_User();

                PropertyInfo[] properties = user.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo _target in properties)
                {
                    if (_target.CanWrite)
                    {
                        object value = forms[_target.Name];
                        if (value != null && value.ToString() != "" /*&& value.ToString() != "&nbsp;"*/)
                        {
                            if (value != null && value.ToString() == "on" && (_target.PropertyType == typeof(int) || _target.PropertyType == typeof(int?)))//此语句是为了前台CheckBox设计的，当前台CheckBox选中时，后台值为1；CheckBox不选中时，无此属性值
                            {
                                value = 1;
                            }
                            _target.SetValue(user, ChangeType(value, _target.PropertyType), null);
                        }
                    }
                }
                user.Enabled = user.Enabled == null ? 0 : 1;
                user.InnerUser = user.InnerUser == null ? 0 : 1;
                return user;
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        public Base_Employee GetEmployee()
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files["DepartmentId"];//获取上传的文件
                NameValueCollection forms = Request.Form;
                Base_Employee employee = new Base_Employee();
                PropertyInfo[] properties = employee.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo _target in properties)
                {
                    if (_target.CanWrite)
                    {
                        object value = forms[_target.Name];
                        if (value != null && value.ToString() == "on" && (_target.PropertyType == typeof(int) || _target.PropertyType == typeof(int?))) //此语句是为了前台CheckBox设计的，当前台CheckBox选中时，后台值为1；CheckBox不选中时，无此属性值
                        {
                            value = 1;
                        }
                        _target.SetValue(employee, ChangeType(value, _target.PropertyType), null);
                    }
                }
                employee.IsDimission = employee.IsDimission == null ? 0 : 1;
                return employee;
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 对于可空类型（如int?）使用ChangeType方法进行类型转化

        public static object ChangeType(object value, Type conversionType)
        {
            try
            {
                if (conversionType == null)
                {
                    throw new ArgumentNullException("conversionType");
                }
                if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    if (value == null)
                    {
                        return null;
                    }
                    NullableConverter nullableConverter = new NullableConverter(conversionType);
                    conversionType = nullableConverter.UnderlyingType;
                }
                return Convert.ChangeType(value, conversionType);
            }
            catch
            {
                return null;//返回空值
            }
        }
        #endregion

        #endregion
    }
}