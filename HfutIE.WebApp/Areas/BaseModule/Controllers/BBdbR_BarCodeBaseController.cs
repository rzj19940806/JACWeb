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
    /// BBdbR_DecodeRule控制器
    /// </summary>
    public class BBdbR_BarCodeBaseController : PublicController<BBdbR_BarCodeBase>
    {
        BBdbR_DecodeRuleBll MyBll = new BBdbR_DecodeRuleBll();
        public readonly RepositoryFactory<BBdbR_DecodeRule> repository_facbase = new RepositoryFactory<BBdbR_DecodeRule>();
        #region 创建数据库操作对象区域
        //创建数据库访问对象，用以访问其中操作数据库的方法
        BBdbR_BarCodeBaseBll MyBll1 = new BBdbR_BarCodeBaseBll(); //===复制时需要修改===MyBll.GetPlanList("");

        // GET: BaseModule/CarStranded
        #endregion
        #region 方法区

        public virtual ActionResult FormMain()
        {
            return View();
        }
        public virtual ActionResult CodeWc()
        {
            return View();
        }
        #region 1.加载规则信息表的数据
        /// <summary>
        /// 搜索表格中所有的数据，并以json的形式返回
        /// </summary>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GridPageJson2(string KeyValue, JqGridParam jqgridparam)
        {
            try
            {
                if (KeyValue != "")
                {
                    string keyvalue = KeyValue.Trim();//删除字符串头和尾的空白字符
                    Stopwatch watch = CommonHelper.TimerStart();//测量时间
                    DataTable ListData = MyBll.GetConfigList(keyvalue, jqgridparam);//===复制时需要修改===
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
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "请确保选中条码信息" }.ToString());
                }
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 2.新增编辑方法
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
        public ActionResult SubmitForm1(BBdbR_DecodeRule entity, string DecodeRulesId)//===复制时需要修改===
        {
            try
            { 
                int IsOk = 0;//用于判断
                string Name = "BarCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.BarCd;  //页面中的编号字段值                 //===复制时需要修改
                //string Start = entity.CombineStart;
                //string Length = entity.CombineLength;
                string Message = DecodeRulesId == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(DecodeRulesId))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_DecodeRule Oldentity = repository_facbase.Repository().FindEntity(DecodeRulesId);//获取没更新之前实体对象
                    entity.Modify(DecodeRulesId);
                    IsOk = MyBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    if (entity.CombineLength == null)
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(@"update BBdbR_DecodeRule set CombineLength = NULL where Enabled = '1' and DecodeRulesId = '" + DecodeRulesId + "'");
                        int a = DataFactory.Database().ExecuteBySql(strSql);
                    }
                    else { }
                    this.WriteLog(IsOk, entity, Oldentity, DecodeRulesId, Message);//记录日志
                }
                else//新增操作
                {
                    IsOk = MyBll.CheckCount(entity.BarId, entity.CombineNm);//判断页面中填写的数据的编号字段的值是否存在
                    if (IsOk > 0)//存在
                    {
                        Message = "该编号已经存在！";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
                    entity.Create();
                    IsOk = MyBll.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
                    //this.WriteLog(IsOk, DecodeRulesId, Message);//记录日志
                    this.WriteLog(IsOk, entity, null, DecodeRulesId, Message);//记录日志
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                //this.WriteLog(-1, DecodeRulesId, "操作失败：" + ex.Message);//记录日志
                this.WriteLog(-1, entity, null, DecodeRulesId, "操作失败：" + ex.Message);//记录日志
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        public ActionResult  SubmitFormMain(BBdbR_BarCodeBase entity, string KeyValue)
        {
            try
            {
                int IsOk = 0;
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    BBdbR_BarCodeBase Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.Modify(KeyValue);
                    IsOk = repositoryfactory.Repository().Update(entity);
                    if (entity.BarLength == null)
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(@"update BBdbR_BarCodeBase set BarLength = NULL where Enabled = '1' and BarId = '" + KeyValue + "'");
                        int a = DataFactory.Database().ExecuteBySql(strSql);
                    }
                    else { }

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

        #region 写入作业日志BBdbR_DecodeRule
        /// <summary>
        /// 写入作业日志（新增、修改）
        /// </summary>
        /// <param name="IsOk">操作状态</param>
        /// <param name="entity">实体对象</param>
        /// <param name="Oldentity">之前实体对象</param>
        /// <param name="Message">备注信息</param>
        public void WriteLog(int IsOk, BBdbR_DecodeRule entity, BBdbR_DecodeRule Oldentity, string KeyValue, string Message = "")
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
        #endregion

        #region 3.删除方法
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
        public ActionResult Delete1(string DecodeRulesId, string RuleId, string DeleteMark)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = DecodeRulesId.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功
                             //IsOk = MyBll.Delete(array);//执行删除操作
                             //直接删除
                if (array != null && array.Length > 0)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        repository_facbase.Repository().Delete(array[i]);//删
                        IsOk = 1;
                    }
                }
                if (IsOk > 0)//表示删除成
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

        #region 4.查询方法
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageByCondition(string BarCd, string BarNm, JqGridParam jqgridparam)
        {
            try
            {
                #region 查询原方法
                //string keyword = keywords.Trim();
                //Stopwatch watch = CommonHelper.TimerStart();
                //List<BBdbR_DecodeRule> ListData = MyBll.GetPageListByCondition(keywords, Condition, jqgridparam);//===复制时需要修改===
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

                #region 查询修改
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select * from BBdbR_BarCodeBase where Enabled = '1'");
                List<DbParameter> parameter = new List<DbParameter>();
                //条码编号模糊搜索
                if (BarCd != "" && BarCd != null)
                {
                    //strSql.Append(" and BarCd like '%" + BarCd + "%' ");
                    strSql.Append(" and BarCd like @BarCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@BarCd", "%" + BarCd + "%"));
                }
                //条码名称模糊搜索
                if (BarNm != "" && BarNm != null)
                {
                    //strSql.Append(" and BarNm like '%" + BarNm + "%' ");
                    strSql.Append(" and BarNm like @BarNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@BarNm", "%" + BarNm + "%"));
                }
                //优先级排序
                strSql.Append(" order by RsvFld1 asc");
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
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 5.重构导出
        public ActionResult GetExcel_Data(string BarCd, string BarNm, JqGridParam jqgridparam)
        {
            try
            {
                #region 将搜索时保存的查询到的全列全局变量datatable转为只含导出列的datatable 数据量大时耗时较长
                //Stopwatch watch = CommonHelper.TimerStart();
                //DataTable ListData = new DataTable();
                //if (dtExport.Rows.Count > 0)
                //{
                //    ListData = dtExport.DefaultView.ToTable("总装生产日计划", true, "ProducePlanCd", "OrderCd", "VIN", "MatCd", "CarType", "CarColor1", "PlanTime", "PlanStatus", "RecieveTm");//获取表中特定列
                //}
                #endregion

                #region 根据当前搜索条件查出数据并导出
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select BarCd,BarNm,BarLength,RsvFld1,CreTm,CreNm,MdfTm,MdfNm,Rem from BBdbR_BarCodeBase where Enabled = '1'");

                List<DbParameter> parameter = new List<DbParameter>();
                //条码编号模糊搜索
                if (BarCd != "" && BarCd != null)
                {
                    //strSql.Append(" and BarCd like '%" + BarCd + "%' ");
                    strSql.Append(" and BarCd like @BarCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@BarCd", "%" + BarCd + "%"));
                }
                //条码名称模糊搜索
                if (BarNm != "" && BarNm != null)
                {
                    //strSql.Append(" and BarNm like '%" + BarNm + "%' ");
                    strSql.Append(" and BarNm like @BarNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@BarNm", "%" + BarNm + "%"));
                }
                //优先级排序
                strSql.Append(" order by RsvFld1 asc");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);

                #endregion



                string fileName = "条码解析";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_BarCodeBase(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "条码解析数据导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "条码解析数据导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        public virtual ActionResult SetForm1(string KeyValue)
        {
            BBdbR_DecodeRule entity = repository_facbase.Repository().FindEntity(KeyValue);
            return Content(entity.ToJson());
        }

        public ActionResult CodeWcList(string BarId)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = MyBll1.CodeWcList(BarId);
            foreach (DataRow dr in dt.Rows)
            {
                string strchecked = "";
                if (!string.IsNullOrEmpty(dr["CodeId"].ToString()))//判断是否选中a.WcId,a.WcCd,a.WcNm,C.CodeId
                {
                    strchecked = "selected";
                }
                sb.Append("<li title=\"" + dr["WcNm"] + "(" + dr["WcCd"] + ")" + "\" class=\"" + strchecked + "\">");
                sb.Append("<a id=\"" + dr["WcId"] + "\"><img src=\"../../Content/Images/Icon16/chess_queen_white.png \">" + dr["WcNm"] + "</a><i></i>");
                sb.Append("</li>");
            }
            return Content(sb.ToString());
        }
        public ActionResult SubmitCodeWcList(string CodeId, string WcId)
        {
            try
            {
                string[] array = WcId.Split(',');
                int IsOk = MyBll1.SubmitCodeWcList(CodeId, array);
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "工位条码配置成功");
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "工位条码配置操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message }.ToString());
            }
        }
    }
}
#endregion