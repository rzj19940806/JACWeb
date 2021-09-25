using HfutIE.Business;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    public class BBdbR_AVIBaseController : PublicController<BBdbR_AVIBase>
    {
        #region 创建数据库操作对象区域
        //创建数据库访问对象，用以访问其中操作数据库的方法
        BBdbR_AVIBaseBll MyBll = new BBdbR_AVIBaseBll(); //===复制时需要修改===
        BBdbR_AVIWhereaboutsConfigBll AVIWhereaboutsConfigBll = new BBdbR_AVIWhereaboutsConfigBll();
        BBdbR_AVIActionConfigBll AVIActionConfigBll = new BBdbR_AVIActionConfigBll();
        public readonly RepositoryFactory<BBdbR_AVIBase> repository_avibase = new RepositoryFactory<BBdbR_AVIBase>();

        #endregion

        #region 打开视图区域

        public ActionResult AVIConfigForm()//打开AVI配置子界面页面
        {
            return View();
        }

        public ActionResult AVIMarkForm()//打开AVI配置去向标识子页面
        {
            return View();
        }

        public ActionResult RuleConfigForm()//打开AVI配置车身调度规则子界面
        {
            return View();
        }
        public ActionResult AVIActionForm()//打开AVI配置行为子界面
        {
            return View();
        }
        #endregion

        #region 方法区  

        #region 1.加载表格数据
        /// <summary>
        /// 加载列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageJson1()
        {
            try
            {
                DataTable ListData = MyBll.GetPlanList();
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
        public override ActionResult SubmitForm(BBdbR_AVIBase entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "AviCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.AviCd;  //页面中的编号字段值                 //===复制时需要修改===\
                string Name2 = "AviNm";
                string Value2 = entity.AviNm;
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_AVIBase Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.AviId = KeyValue;//编辑保持主键不变
                    entity.Modify(KeyValue);
                    IsOk = MyBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    if (IsOk > 0)
                    {
                        Message = "编辑AVI信息成功！";
                    }
                    else
                    {
                        Message = "编辑AVI信息失败！";
                    }
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);//记录日志
                }
                else//新增操作
                {
                    IsOk = MyBll.CheckCount2(Name, Value, Name2, Value2);//判断页面中填写的数据的编号字段的值是否存在
                    if (IsOk == 1)//编号存在
                    {
                        Message = "该编号已经存在！";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
                    else if (IsOk == 2)
                    {
                        Message = "该名称已经存在！";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
                    entity.Create();
                    IsOk = MyBll.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
                    if (IsOk > 0)
                    {
                        Message = "新增AVI信息成功！";
                    }
                    else
                    {
                        Message = "新增AVI信息失败！";
                    }
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
                    IsOk=MyBll.Delete(array);
                   
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
                List<BBdbR_AVIBase> ListData = MyBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
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

        #region 6.AVI与产线配置
        /// <summary>
        /// 去向产线配置填充界面
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult SetConfigForm(string KeyValue)
        {
            try
            {
                BBdbR_AVIWhereaboutsConfig Deviceentity = AVIWhereaboutsConfigBll.SetConfigInfor(KeyValue);
                return Content(Deviceentity.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 提交去向配置
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult SubmitConfigForm(BBdbR_AVIWhereaboutsConfig entity, string KeyValue)
        {
            try
            {
                int IsOk = 0;
                string Message = "";
                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===                  
                    entity.Modify(KeyValue);
                    entity.RsvFld1 = "";
                    entity.RsvFld2 = "";
                    IsOk = AVIWhereaboutsConfigBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    if (IsOk > 0)
                    {
                        Message = "编辑AVI去向产线标识成功！";
                    }
                    else
                    {
                        Message = "编辑AVI去向产线标识失败！";
                    }
                 
                }          
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        #region 7.加载AVI配置弹框表格
        /// <summary>
        /// 加载配置页面下半个表格
        /// 加载的数据是已有班组信息也有配置
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GridPageListJson_Down(string KeyValue, JqGridParam jqgridparam)
        {
            try
            {
                if (KeyValue != "")
                {
                    string keyvalue = KeyValue.Trim();//删除字符串头和尾的空白字符
                    Stopwatch watch = CommonHelper.TimerStart();//测量时间
                    DataTable ListData = AVIWhereaboutsConfigBll.GetPageConfigList(keyvalue, jqgridparam);//===复制时需要修改===
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
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "请确保选中班制信息" }.ToString());
                }
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 加载配置页面下半个表格
        /// 加载的数据是已有班组信息也有配置
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GridPageListJson_Down2(string KeyValue, JqGridParam jqgridparam)
        {
            try
            {
                BBdbR_SchedulRuleBaseBll schedulRuleBaseBll = new BBdbR_SchedulRuleBaseBll();
                if (KeyValue != "")
                {
                    string keyvalue = KeyValue.Trim();//删除字符串头和尾的空白字符
                    Stopwatch watch = CommonHelper.TimerStart();//测量时间
                    DataTable ListData = schedulRuleBaseBll.GetConfigList(keyvalue, jqgridparam);//===复制时需要修改===
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
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "请确保选中班制信息" }.ToString());
                }
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 加载配置弹框中上半个表格
        /// 加载的数据是已有班组信息也有配置
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GridListJson_Up(string KeyValue, JqGridParam jqgridparam)
        {
            try
            {
                if (KeyValue != "")
                {
                    string keyvalue = KeyValue.Trim();//删除字符串头和尾的空白字符
                    Stopwatch watch = CommonHelper.TimerStart();//测量时间
                    DataTable ListData = AVIWhereaboutsConfigBll.ReGetConfigList(keyvalue, jqgridparam);//===复制时需要修改===
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
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "请确保选中班制信息" }.ToString());
                }
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 加载配置弹框中下半个表格
        /// 加载的数据是已有班组信息也有配置
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GridListJson_Down(string KeyValue, JqGridParam jqgridparam)
        {
            try
            {
                if (KeyValue != "")
                {
                    string keyvalue = KeyValue.Trim();//删除字符串头和尾的空白字符
                    Stopwatch watch = CommonHelper.TimerStart();//测量时间
                    DataTable ListData = AVIWhereaboutsConfigBll.GetConfigList(keyvalue, jqgridparam);//===复制时需要修改===
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
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "请确保选中班制信息" }.ToString());
                }
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 8.AVI去向产线配置
        /// <summary>
        /// 提交AVI站点及产线去向配置信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SubmitAviConfig(List<BBdbR_PlineBase> listdto,string AviId)
        {
            try
            {
                var Message = "配置失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//用于判断          
                if (AviId != "")//先把对应ClassId中所有班组均删除，然后插入已配置班组
                {
                    List<BBdbR_AVIWhereaboutsConfig> ListData1 = AVIWhereaboutsConfigBll.GetClassList(AviId, "");
                    if (ListData1.Count > 0)
                    {
                        AVIWhereaboutsConfigBll.Delete(ListData1);//执行删除操作
                    }
                }
                if (listdto != null)
                {
                    for (int i = 0; i < listdto.Count; i++)
                    {
                        List<BBdbR_AVIWhereaboutsConfig> ListData = AVIWhereaboutsConfigBll.GetClassList(AviId, listdto[i].PlineId.ToString());
                        if (ListData.Count == 0)
                        {
                            IsOk = AVIWhereaboutsConfigBll.Insert(AviId, listdto[i].PlineId.ToString());//将实体插入数据库，插入成功返回1，失败返回0；
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                IsOk = 1;
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "配置成功!" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 9.AVI站点车身调度规则配置
        /// <summary>
        /// 加载配置弹框中上半个表格
        /// 加载的数据是已有车身调度信息无配置
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult SchedulRuleJson_Up(string KeyValue, JqGridParam jqgridparam)
        {
            try
            {
                BBdbR_SchedulRuleBaseBll schedulRuleBaseBll = new BBdbR_SchedulRuleBaseBll();
                if (KeyValue != "")
                {
                    string keyvalue = KeyValue.Trim();//删除字符串头和尾的空白字符
                    Stopwatch watch = CommonHelper.TimerStart();//测量时间
                    DataTable ListData = schedulRuleBaseBll.ReGetConfigList(keyvalue, jqgridparam);//===复制时需要修改===
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
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "请确保选中班制信息" }.ToString());
                }
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 加载配置弹框中下半个表格
        /// 加载的数据是已有班组信息也有配置
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult SchedulRuleJson_Down(string KeyValue, JqGridParam jqgridparam)
        {
            try
            {
                BBdbR_SchedulRuleBaseBll schedulRuleBaseBll = new BBdbR_SchedulRuleBaseBll();
                if (KeyValue != "")
                {
                    string keyvalue = KeyValue.Trim();//删除字符串头和尾的空白字符
                    Stopwatch watch = CommonHelper.TimerStart();//测量时间
                    DataTable ListData = schedulRuleBaseBll.GetConfigList(keyvalue, jqgridparam);//===复制时需要修改===
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
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "请确保选中班制信息" }.ToString());
                }
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 提交AVI站点车身调度规则配置信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SubmitScheduleRuleConfig(List<BBdbR_SchedulRuleBase> listdto, string AviId)
        {
            try
            {
                BBdbR_CarSchedulRuleConfigBll CarSchedulRuleConfigBll = new BBdbR_CarSchedulRuleConfigBll();
                var Message = "配置失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//用于判断          
                if (AviId != "")//先把对应
                {
                    List<BBdbR_CarSchedulRuleConfig> ListData1 = CarSchedulRuleConfigBll.GetClassList(AviId, "");
                    if (ListData1.Count > 0)
                    {
                        CarSchedulRuleConfigBll.Delete(ListData1);//执行删除操作
                    }
                }
                if (listdto != null)
                {
                    for (int i = 0; i < listdto.Count; i++)
                    {
                        List<BBdbR_CarSchedulRuleConfig> ListData = CarSchedulRuleConfigBll.GetClassList(AviId, listdto[i].SchedulRuleId.ToString());
                        if (ListData.Count == 0)
                        {
                            IsOk = CarSchedulRuleConfigBll.Insert(AviId, listdto[i].SchedulRuleId.ToString());//将实体插入数据库，插入成功返回1，失败返回0；
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                IsOk = 1;
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "配置成功!" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 10.下拉框产线信息
        /// <summary>
        /// 人员下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult GetplineNm()
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

        #region 11.AVI站点行为配置
        public ActionResult SubmitActionConfigForm(BBdbR_AVIActionConfig entity, string AviActionConfigId)
        {
            try
            {
                int IsOk = 0;
                string Message = "";
                if (!string.IsNullOrEmpty(AviActionConfigId))//编辑AVI行为配置
                {
                    //===复制时需要修改===                  
                    entity.Modify(AviActionConfigId);
                    entity.RsvFld1 = "";
                    entity.RsvFld2 = "";
                    IsOk = AVIActionConfigBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    if (IsOk > 0)
                    {
                        Message = "编辑AVI站点行为成功！";
                    }
                    else
                    {
                        Message = "编辑AVI站点行为失败！";
                    }

                }
                else//新增AVI行为配置
                {
                    entity.Create();
                    IsOk = AVIActionConfigBll.Insert(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    if (IsOk > 0)
                    {
                        Message = "配置AVI站点行为成功！";
                    }
                    else
                    {
                        Message = "配置AVI站点行为失败！";
                    }

                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {

                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #region 10.2 AVI站点行为配置信息表
        //AVI站点行为配置信息表
        public ActionResult GridPageListJson_AviAction(string AviCd, JqGridParam jqgridparam)
        {
            try
            {
                if (AviCd != "")
                {
                    string keyvalue = AviCd.Trim();//删除字符串头和尾的空白字符
                    Stopwatch watch = CommonHelper.TimerStart();//测量时间
                    DataTable ListData = AVIActionConfigBll.GetPageConfigList(keyvalue, jqgridparam);//===复制时需要修改===
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
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "请确保选中AVI信息" }.ToString());
                }
            }
            catch (Exception ex)
            {
                WriteLog(-1, AviCd, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion
        #region 10.3 AVI站点行为编辑界面填充
        public ActionResult SetActionForm(string AviActionConfigId)
        {
            try
            {

                BBdbR_AVIActionConfig Deviceentity = AVIActionConfigBll.SetConfigInfor(AviActionConfigId);
                return Content(Deviceentity.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #endregion

        #region 12.下拉框去向Avi站点信息
        /// <summary>
        /// 人员下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAviNm()
        {
            try
            {
                DataTable dataTable = AVIWhereaboutsConfigBll.GetAviNm();
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
        /// <summary>
        /// 人员信息
        /// </summary>
        /// <param name="StfId"></param>
        /// <returns></returns>
        public ActionResult GetAviNm2(string ToAVIId)
        {
            try
            {
                DataTable dataTable = AVIWhereaboutsConfigBll.GetAviNm2(ToAVIId);
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

        #endregion

    }

}
