using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp
{
    /// <summary>
    /// 控制器公共基类
    /// 这样可以减少很多重复代码量
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class PublicController<TEntity> : Controller where TEntity : BaseEntity, new()
    {
        public readonly RepositoryFactory<TEntity> repositoryfactory = new RepositoryFactory<TEntity>();

        #region 列表
        /// <summary>
        /// 列表视图
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public virtual ActionResult Index()
        {
            return View();
        }


        public virtual ActionResult Form_CA()
        {
            return View();
        }

        public virtual ActionResult ClassDetlForm()
        {
            return View();
        }

        public virtual ActionResult Form_HCU()
        {
            return View();
        }

        public virtual ActionResult Form_HCU_Down()
        {
            return View();
        }

        public virtual ActionResult Form_HCU_UP()
        {
            return View();
        }


        public virtual ActionResult Form_MA()
        {
            return View();
        }

        public virtual ActionResult Form_SA()
        {
            return View();
        }


        public virtual ActionResult Form_SA1()
        {
            return View();
        }

        /// <summary>
        /// 绑定表格
        /// </summary>
        /// <param name="ParameterJson">查询条件</param>
        /// <param name="Gridpage">分页条件</param>
        /// <returns></returns>
        [LoginAuthorize]
        public virtual JsonResult GridPageJson(string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<TEntity> ListData = new List<TEntity>();
                if (!string.IsNullOrEmpty(ParameterJson))
                {
                    List<DbParameter> parameter = new List<DbParameter>();
                    IList conditions = ParameterJson.JonsToList<Condition>();
                    string WhereSql = ConditionBuilder.GetWhereSql(conditions, out parameter);
                    ListData = repositoryfactory.Repository().FindListPage(WhereSql, parameter.ToArray(), ref jqgridparam);
                }
                else
                {
                    ListData = repositoryfactory.Repository().FindListPage(ref jqgridparam);
                }
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message + "\r\n条件：" + ParameterJson);
                return null;
            }
        }
        /// <summary>
        /// 绑定表格
        /// </summary>
        /// <param name="ParameterJson">查询条件</param>
        /// <param name="Gridpage">排序条件</param>
        /// <returns></returns>
        [LoginAuthorize]
        public virtual JsonResult GridJson(string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                List<TEntity> ListData = new List<TEntity>();
                if (!string.IsNullOrEmpty(ParameterJson))
                {
                    List<DbParameter> parameter = new List<DbParameter>();
                    IList conditions = ParameterJson.JonsToList<Condition>();
                    string WhereSql = ConditionBuilder.GetWhereSql(conditions, out parameter, jqgridparam.sidx, jqgridparam.sord);
                    ListData = repositoryfactory.Repository().FindList(WhereSql, parameter.ToArray());
                }
                else
                {
                    ListData = repositoryfactory.Repository().FindList();
                }
                return Json(ListData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message + "\r\n条件：" + ParameterJson);
                return null;
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <param name="ParentId">判断是否有子节点</param>
        /// <returns></returns>
        [HttpPost]
        [ManagerPermission(PermissionMode.Enforce)]
        public virtual ActionResult Delete(string KeyValue, string ParentId, string DeleteMark)
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
                    IsOk = repositoryfactory.Repository().Delete(array);
                    if (IsOk > 0)
                    {
                        Message = "删除成功。";
                    }
                }
                else //物理删除
                {
                    string KeyField = DatabaseCommon.GetKeyField<TEntity>().ToString();
                    TEntity entity = new TEntity();
                    Type type = entity.GetType();
                    Hashtable ht_DeleteMark = new Hashtable();
                    ht_DeleteMark[KeyField] = KeyValue;
                    ht_DeleteMark["DeleteMark"] = 1;
                    IsOk = repositoryfactory.Repository().Update(type.Name, ht_DeleteMark, KeyField);
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

        #region 表单
        #region 表单验证字段唯一型
        public ActionResult OnlyOne(string Fieldname, string Fieldvalue, string KeyValue)
        {
            string hasdata = "no";
            if (KeyValue == "")//则为添加一个新的实体
            {
                if (repositoryfactory.Repository().FindCount(Fieldname, Fieldvalue) > 0)
                {
                    hasdata = "yes";
                }
            }
            else//编辑
            {
                TEntity a = repositoryfactory.Repository().FindEntity(KeyValue);
                Type type = a.GetType();
                PropertyInfo[] props = type.GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    if (prop.Name == Fieldname)
                    {
                        if (prop.GetValue(a).ToString() != Fieldvalue)//就说明编辑过此字段
                        {
                            if (repositoryfactory.Repository().FindCount(Fieldname, Fieldvalue) > 0)
                            {
                                hasdata = "yes";
                            }
                        }
                        break;
                    }
                }    
            }
            return Content(new { hasdata = hasdata }.ToJson());
        }
        #endregion
        /// <summary>
        /// 明细视图
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public virtual ActionResult Detail()
        {
            return View();
        }
        /// <summary>
        /// 表单视图
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 返回显示顺序号
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public virtual ActionResult SortCode()
        {
            string strCode = BaseFactory.BaseHelper().GetSortCode<TEntity>("SortCode").ToString();
            return Content(strCode);
        }
        /// <summary>
        /// 表单赋值
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [LoginAuthorize]
        public virtual ActionResult SetForm(string KeyValue)
        {
            TEntity entity = repositoryfactory.Repository().FindEntity(KeyValue);
            return Content(entity.ToJson());
        }
        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [LoginAuthorize]
        public virtual ActionResult SubmitForm(TEntity entity, string KeyValue)
        {
            try
            {
                int IsOk = 0;
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    TEntity Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.Modify(KeyValue);
                    IsOk = repositoryfactory.Repository().Update(entity);
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);
                }
                else
                {
                    entity.Create();
                    IsOk = repositoryfactory.Repository().Insert(entity);
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 写入作业日志
        /// <summary>
        /// 写入作业日志（新增、修改）
        /// </summary>
        /// <param name="IsOk">操作状态</param>
        /// <param name="entity">实体对象</param>
        /// <param name="Oldentity">之前实体对象</param>
        /// <param name="Message">备注信息</param>
        public void WriteLog(int IsOk, TEntity entity, TEntity Oldentity, string KeyValue, string Message = "")
        {
            if (!string.IsNullOrEmpty(KeyValue))
            {
                Base_SysLogBll.Instance.WriteLog(Oldentity, entity, OperationType.Update, IsOk.ToString(), Message);
            }
            else
            {
                Base_SysLogBll.Instance.WriteLog(entity, OperationType.Add, IsOk.ToString(), Message);
            }
        }
        /// <summary>
        /// 写入作业日志（删除操作）
        /// </summary>
        /// <param name="IsOk">操作状态</param>
        /// <param name="KeyValue">主键值</param>
        /// <param name="Message">备注信息</param>
        public void WriteLog(int IsOk, string[] KeyValue, string Message = "")
        {
            Base_SysLogBll.Instance.WriteLog<TEntity>(KeyValue, IsOk.ToString(), Message);
        }
        /// <summary>
        /// 写入作业日志（删除操作）
        /// </summary>
        /// <param name="IsOk">操作状态</param>
        /// <param name="KeyValue">主键值</param>
        /// <param name="Message">备注信息</param>
        public void WriteLog(int IsOk, string KeyValue, string Message = "")
        {
            string[] array = KeyValue.Split(',');
            Base_SysLogBll.Instance.WriteLog<TEntity>(array, IsOk.ToString(), Message);
        }
        #endregion

        #region ANCHI工厂
        //ps目前使用enabled进行软删除，个人感觉是不太好的，希望可以加入deletemark进行软删除
        /// <summary>
        /// 排除软删除的数据
        /// </summary>
        /// <param name="ParameterJson">筛选数据codition的List</param>
        /// <param name="jqgridparam">jqgrid的参数</param>
        /// <returns></returns>
        public virtual JsonResult GridPageJsonNew(string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Condition condition = new Condition();
                condition.ParamName = "Enabled";
                condition.Operation = ConditionOperate.Equal;
                condition.ParamValue = "1";
                condition.Logic = "AND";
                Stopwatch watch = CommonHelper.TimerStart();
                List<Condition> conditions = new List<Condition>();
                conditions.Add(condition);
                List<DbParameter> parameter = new List<DbParameter>();

                if (!string.IsNullOrEmpty(ParameterJson))
                {
                    conditions.AddRange(ParameterJson.JonsToList<Condition>());
                }
                string WhereSql = ConditionBuilder.GetWhereSql(conditions, out parameter);
                var dataList = repositoryfactory.Repository().FindListPage(WhereSql, parameter.ToArray(), ref jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dataList,
                };
                return Json(JsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message + "\r\n条件：" + ParameterJson);
                return null;
            }
        }


        /// <summary>
        /// 验证数据唯一性，排除软删除数据
        /// </summary>
        /// <param name="Fieldname">对象属性</param>
        /// <param name="Fieldvalue">对象值</param>
        /// <param name="KeyValue">对象主键值，非空为编辑验证，空为新增验证</param>
        /// <returns></returns>
        public ActionResult OnlyOneNew(string Fieldname, string Fieldvalue, string KeyValue)
        {
            string hasdata = "no";
            if (KeyValue == "")//则为添加一个新的实体
            {
                if (repositoryfactory.Repository().FindCount("AND Enabled='1' AND " + Fieldname + "='" + Fieldvalue + "'") > 0)
                {
                    hasdata = "yes";
                }
            }
            else//编辑
            {
                TEntity a = repositoryfactory.Repository().FindEntity(KeyValue);
                Type type = a.GetType();
                PropertyInfo[] props = type.GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    if (prop.Name == Fieldname)
                    {
                        if (prop.GetValue(a).ToString() != Fieldvalue)//就说明编辑过此字段
                        {
                            if (repositoryfactory.Repository().FindCount("AND Enabled='1' AND " + Fieldname + "='" + Fieldvalue + "'") > 0)
                            {
                                hasdata = "yes";
                            }
                        }
                        break;
                    }
                }
            }
            return Content(new { hasdata = hasdata }.ToJson());
        }


        /// <summary>
        /// 远有软删除是deltemark=1，修改为enabled=0
        /// </summary>
        /// <param name="KeyValue">对象主键</param>
        /// <param name="ParentId">父ID，判断该ID是否有子，有则无法删除</param>
        /// <param name="DeleteMark">非空表示软删除</param>
        /// <returns></returns>
        public ActionResult DeleteNew(string KeyValue, string ParentId, string DeleteMark)
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
                    IsOk = repositoryfactory.Repository().Delete(array);
                    if (IsOk > 0)
                    {
                        Message = "删除成功。";
                    }
                }
                else //软删除
                {
                    string KeyField = DatabaseCommon.GetKeyField<TEntity>().ToString();
                    TEntity entity = new TEntity();
                    Type type = entity.GetType();
                    Hashtable ht_DeleteMark = new Hashtable();
                    ht_DeleteMark[KeyField] = KeyValue;
                    ht_DeleteMark["Enabled"] = 0;
                    IsOk = repositoryfactory.Repository().Update(type.Name, ht_DeleteMark, KeyField);
                    Message = "软删除成功。";
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
    }
}
