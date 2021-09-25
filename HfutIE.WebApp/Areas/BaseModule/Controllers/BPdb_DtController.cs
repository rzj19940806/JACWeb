using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
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

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// _FactoryBaseInformation控制器
    /// </summary>
    public class BPdb_DtController : PublicController<BPdb_Dt>
    {
        #region 创建数据库操作对象区域
        //创建数据库访问对象，用以访问其中操作数据库的方法
        BPdb_DtBll MyBll = new BPdb_DtBll(); //===复制时需要修改===
        public readonly RepositoryFactory<BPdb_Dt> repository_dt = new RepositoryFactory<BPdb_Dt>();
        #endregion

        #region 方法区   

        #region 1.获取树
        public ActionResult TreeJson()
        {
            try
            {
                string year = DateTime.Now.Year.ToString();
                DataTable dt = MyBll.GetTree(year);//获取树所需数据
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
                //获取点击节点对应的数据（列表）            
                List<BPdb_Dt> ListData = MyBll.GetList(areaId, parentId, ref jqgridparam);//===复制时需要修改===
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
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


        #region Form页 下拉框

        //班制
        public ActionResult GetClassNm()
        {
            try
            {
                DataTable dataTable = MyBll.GetClassNm();
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

        //车间
        public ActionResult GetWsbNm()
        {
            try
            {
                DataTable dataTable = MyBll.GetWsbNm();
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

        //产线
        public ActionResult GetPlineNm()
        {
            try
            {
                DataTable dataTable = MyBll.GetPlineNm();
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

        
        #region 3.展示页面表格
        /// <summary>
        /// 搜索表格中所有的数据，并以json的形式返回
        /// </summary>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GridPage(JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<BPdb_Dt> ListData = MyBll.GetPageList(jqgridparam);    //===复制时需要修改===
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 4.新增编辑方法
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
        public ActionResult SubmitFormCal(BPdb_Dt entity, string KeyValue, string idvalue, string OrgRank,string idtext,string classid,string classname)//===复制时需要修改===
        {
            try
            {
                string Message = KeyValue == "" ? "新增成功" : "编辑成功";                               
                //int count = 0;                
                if (!string.IsNullOrEmpty(KeyValue)) //编辑操作
                {
                    entity.OrgId = idvalue;
                    entity.RsvFld1 = idtext;
                    entity.ClassId = classid;
                    entity.RsvFld2 = classname;
                    entity.Modify(KeyValue);
                    MyBll.Update(entity);
                }
                else // 新增操作
                {
                   if(OrgRank == "1") //车间
                    {
                        entity.OrgId = idvalue;
                        entity.RsvFld1 = idtext;
                    }else //产线
                    {
                        entity.OrgId = idvalue;
                        entity.RsvFld1 = idtext;
                    }
                    entity.ClassId = classid;
                    entity.RsvFld2 = classname;
                    entity.Create();
                    MyBll.Insert(entity);
                }
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
                }
            catch (Exception ex)
            {
                this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);//记录日志
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 5.删除方法
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
                if (array != null && array.Length > 0) IsOk = MyBll.DeleteUseEnabled(array);
                if (IsOk > 0) Message = "删除成功。";
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

        #region 7.加载界面表格
        /// <summary>
        /// 加载列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageJson1()
        {
            try
            {
                List<BPdb_Dt> ListData = MyBll.GetPlanList();            
                var JsonData = new
                {
                    rows = ListData,
                };
                string a = JsonData.ToJson();
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 8.查询方法
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
                List<BPdb_Dt> ListData = MyBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
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