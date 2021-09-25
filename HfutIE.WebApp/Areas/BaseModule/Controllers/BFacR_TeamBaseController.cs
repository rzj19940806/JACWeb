using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
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
using HfutIE.Repository;
using static HfutIE.WebApp.App_Start.PublicClass;
using NPOI.SS.Formula.Functions;

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// _FactoryBaseInformation控制器
    /// </summary>
    public class BFacR_TeamBaseController : PublicController<BFacR_TeamBase>
    {
        #region 创建数据库操作对象区域
        //创建数据库访问对象，用以访问其中操作数据库的方法
        BFacR_TeamBaseBll MyBll = new BFacR_TeamBaseBll(); //===复制时需要修改===MyBll.GetPlanList("");
        public readonly RepositoryFactory<BFacR_TeamBase> repository_teambase = new RepositoryFactory<BFacR_TeamBase>();
        #endregion

        #region 展开视图
        /// <summary>
        /// 班组人员配置
        /// </summary>
        /// <returns></returns>
        public ActionResult StaffConfigForm()
        {
            return View();
        }
        /// <summary>
        /// 班组产线配置
        /// </summary>
        /// <returns></returns>
        public ActionResult PlineConfigForm()
        {
            return View();
        }
        #endregion

        #region 方法区 

        //#region 1.加载表格数据
        ///// <summary>
        ///// 加载列表
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult GridPageJson1()
        //{
        //    try
        //    {
        //        List<BFacR_TeamBase> ListData = MyBll.GetPlanList("");
        //        var JsonData = new
        //        {
        //            rows = ListData,
        //        };
        //        string a = JsonData.ToJson();
        //        return Content(JsonData.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
        //    }
        //}

        //#endregion

        #region 1.展示页面表格
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
                List<BFacR_TeamBase> ListData = MyBll.GetPageList(jqgridparam);    //===复制时需要修改===
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

        #region 2.新增、编辑
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
        public override ActionResult SubmitForm(BFacR_TeamBase entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "TeamCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.TeamCd;  //页面中的编号字段值                 //===复制时需要修改===\
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===                 
                    BFacR_TeamBase Oldentity = repository_teambase.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.Modify(KeyValue);
                    entity.TeamId = KeyValue;
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

        #region 3.删除
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
                //IsOk = MyBll.Delete(array);//执行删除操作
                //直接删除
                if (array != null && array.Length > 0)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        IsOk = MyBll.Delete(array);//软删除-Enabled=0
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

        public ActionResult GridPageByCondition(string keywords, string Condition, JqGridParam jqgridparam)
        {
            try
            {
                string keyword = keywords.Trim();
                Stopwatch watch = CommonHelper.TimerStart();
                List<BFacR_TeamBase> ListData = MyBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
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

        #region 5.检查字段是否唯一
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

        #region 6.班组产线配置
        /// <summary>
        /// 查找班组配置的产线信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        public ActionResult GridListJsonPline(string value, JqGridParam jqgridparam)
        {
            Stopwatch watch = CommonHelper.TimerStart();
            TeamPlineConfigViewBll TeamBll = new TeamPlineConfigViewBll();
            string keyvalue = value.Trim();//删除字符串头和尾的空白字符
            List<TeamPlineConfigView> ListData = TeamBll.GetTeamOrgList(keyvalue, jqgridparam);
            try
            {
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
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        #region 7.班组人员配置
        /// <summary>
        /// 查找班组配置的人员信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ActionResult GridListJsonUser(string value, JqGridParam jqgridparam)
        {
            Stopwatch watch = CommonHelper.TimerStart();
            TeamStaffConfigViewBll TeamBll = new TeamStaffConfigViewBll();
            string keyvalue = value.Trim();//删除字符串头和尾的空白字符
            List<TeamStaffConfigView> ListData = TeamBll.GetTeamStaffList(keyvalue, jqgridparam);
            try
            {
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
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 获取全部人员清单-但未配置在班组的人员清单
        /// </summary>
        /// <param name="patientcodes"></param>
        /// <returns></returns>
        public ActionResult Right_GridReStaffListJson(string value,JqGridParam jqgridparam)
        {
            try
            {
                if (value != "")
                {
                    BBdbR_StfBaseBll StaffBll = new BBdbR_StfBaseBll();
                    string keyvalue = value.Trim();//删除字符串头和尾的空白字符
                    Stopwatch watch = CommonHelper.TimerStart();//测量时间
                    List<BBdbR_StfBase> ListData = StaffBll.GetReStaffList(keyvalue, jqgridparam);//===复制时需要修改===
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
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "请确保选中班组信息" }.ToString());
                }              
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 获取班组对应已配置人员信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ActionResult Right_GridStaffListJson(string value, JqGridParam jqgridparam)
        {
            try
            {
                if (value != "")
                {
                    BBdbR_StfBaseBll StaffBll = new BBdbR_StfBaseBll();
                    string keyvalue = value.Trim();//删除字符串头和尾的空白字符
                    Stopwatch watch = CommonHelper.TimerStart();//测量时间
                    List<BBdbR_StfBase> ListData = StaffBll.GetStaffList(keyvalue, jqgridparam);//===复制时需要修改===
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
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "请确保选中班组信息" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }          
        }
        #endregion

        #region 8.班组产线配置

        /// <summary>
        /// 获取全部产线信息
        /// </summary>
        /// <param name="patientcodes"></param>
        /// <returns></returns>
        public ActionResult GridListJson_Up_1(string value, JqGridParam jqgridparam)
        {
            try
            {
                string keyvalue = value.Trim();//删除字符串头和尾的空白字符
                Stopwatch watch = CommonHelper.TimerStart();//测量时间
                BBdbR_PlineBaseBll PlineBll = new BBdbR_PlineBaseBll();
                List<BBdbR_PlineBase> ListData = PlineBll.GetReStaffList(keyvalue, jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

       
        /// <summary>
        /// 获取班组对应已配置产线信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ActionResult Right_GridPlineListJson(string value, JqGridParam jqgridparam)
        {
            BBdbR_PlineBaseBll PlineBll = new BBdbR_PlineBaseBll();
            List<BBdbR_PlineBase> ListData = new List<BBdbR_PlineBase>();
            try
            {
                ListData = PlineBll.GetStaffList(value, jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }

        }
        #endregion

        #region 9.提交班组人员配置信息
        /// <summary>
        /// 提交班组人员配置信息
        /// </summary>
        /// <param name="patientcodes"></param>
        /// <returns></returns>
        public ActionResult SubmitStaffConfig(List<BBdbR_StfBase> listdto, string TeamId)
        {
            try
            {
                BFacR_TeamStfBll TeamBll = new BFacR_TeamStfBll();
                var Message = "配置失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//用于判断            
                string Result = "";
                if (TeamId != "")//先把对应TeamId中所有人员均删除，然后插入已配置人员
                {
                    List<BFacR_TeamStfConfig> ListData1 = TeamBll.GetClassList(TeamId, "");
                    if (ListData1.Count > 0)
                    {
                        IsOk = TeamBll.Delete(ListData1);//执行删除操作
                    }
                }
                if (listdto!=null)
                {
                    for (int i = 0; i < listdto.Count; i++)
                    {
                        List<BFacR_TeamStfConfig> ListData = TeamBll.GetClassList(TeamId, listdto[i].StfId.ToString());
                        if (ListData.Count == 0)
                        {
                            IsOk = TeamBll.Insert(listdto[i], TeamId);//将实体插入数据库，插入成功返回1，失败返回0；
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                Message = "配置成功";
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 提交产线配置信息
        /// </summary>
        /// <param name="patientcodes"></param>
        /// <returns></returns>
        public ActionResult SubmitStaffConfig1(List<BBdbR_PlineBase> listdto, string TeamId)
        {
            try
            {
                BFacR_TeamOrgBll TeamBll = new BFacR_TeamOrgBll();
                var Message = "配置失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功             
                string Result = "";
                BFacR_TeamOrgBll MyBll1 = new BFacR_TeamOrgBll();
                if (TeamId != "")//先把对应TeamId中所有产线均删除，然后插入已配置产线
                {
                    List<BFacR_TeamOrg> ListData1 = TeamBll.GetClassList(TeamId, "");
                    if (ListData1.Count > 0)
                    {
                        IsOk = TeamBll.Delete(ListData1);//执行删除操作
                    }
                }
                if (listdto != null)
                {
                    for (int i = 0; i < listdto.Count; i++)
                    {
                        List<BFacR_TeamOrg> ListData = TeamBll.GetClassList(TeamId, listdto[i].PlineId.ToString());
                        if (ListData.Count == 0)
                        {
                            IsOk = TeamBll.Insert(listdto[i], TeamId);//将实体插入数据库，插入成功返回1，失败返回0；
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                Message = "配置成功";
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #endregion
    }
}