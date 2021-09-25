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
using System.Web;
using System.Web.Mvc;

namespace HfutIE.WebApp.Areas.BaseModule.Controllers
{
    /// <summary>
    /// 检查模板基本信息表控制器
    /// </summary>
    public class BBdbR_QualityCheckModelBaseController : PublicController<BBdbR_QualityCheckModelBase>
    {
        #region 创建数据库操作对象区域
        //创建数据库访问对象，用以访问其中操作数据库的方法
        BBdbR_QualityCheckModelBaseBll MyBll = new BBdbR_QualityCheckModelBaseBll(); //===复制时需要修改===
        BBdbR_QuaCheckModelCarConfigBll MyBll1 = new BBdbR_QuaCheckModelCarConfigBll();
        BBdbR_MatBaseBll MyBll2 = new BBdbR_MatBaseBll();
        BBdbR_CarPartBaseBll MyBll3 = new BBdbR_CarPartBaseBll();
        BBdbR_QuaCheckModelCarPartConfigBll MyBll4 = new BBdbR_QuaCheckModelCarPartConfigBll();
        public readonly RepositoryFactory<BBdbR_QualityCheckModelBase> repository_avibase = new RepositoryFactory<BBdbR_QualityCheckModelBase>();
        #endregion

        #region 方法区   
        /// <summary>
        /// 产品配置
        /// </summary>
        /// <returns></returns>
        public ActionResult MatConfigForm()
        {
            return View();
        }
        #region 1.获取树
        public ActionResult TreeJson()
        {
            try
            {
                DataTable dt = MyBll.GetTree();//获取树所需数据
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
                        tree.isexpand = false;
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
                var list = TreeList.OrderBy(x => x.value).ToList();
                return Content(list.TreeToJson());
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
                List<BBdbR_QualityCheckModelBase> ListData = MyBll.GetList(areaId, parentId, ref jqgridparam);//===复制时需要修改===
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
                List<BBdbR_QualityCheckModelBase> ListData = MyBll.GetPageList(jqgridparam);    //===复制时需要修改===
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
        public override ActionResult SubmitForm(BBdbR_QualityCheckModelBase entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "QualityCheckModelCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.QualityCheckModelCd;  //页面中的编号字段值                 //===复制时需要修改===\
                string Message = "";
                if (KeyValue == "")
                {
                    Message = "新增成功。";
                }
                else
                {
                    Message = "编辑成功。";
                }

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_QualityCheckModelBase Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    
                    entity.Modify(KeyValue);
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
                int IsOk1 = 0;
                int IsOk2 = 0;

                for (int i = 0; i < array.Length; i++)
                {
                    //先把对应QualityCheckModelId中所有产品均删除

                    List<BBdbR_QuaCheckModelCarConfig> ListData1 = MyBll1.GetMatList(array[i], "");
                    if (ListData1.Count > 0)
                    {
                        IsOk1 = MyBll1.Delete(ListData1);//执行删除操作
                    }
                    List<BBdbR_QuaCheckModelCarPartConfig> ListData2 = MyBll4.GetCarPartList(array[i], "");
                    if (ListData2.Count > 0)
                    {
                        IsOk2 = MyBll4.Delete(ListData2);//执行删除操作
                    }
                }
                
                IsOk = MyBll.Delete(array);//执行删除操作
                if (IsOk > 0)
                {
                    Message = "删除成功";
                }
                WriteLog(IsOk, array, Message);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
                //return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 6.查询方法
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
                List<BBdbR_QualityCheckModelBase> ListData = MyBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
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

        #region 7.检查字段是否唯一
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

        
        #region 获取全部产品清单
        /// <summary>
        /// 获取全部产品清单
        /// </summary>
        /// <param name="patientcodes"></param>
        /// <returns></returns>
        public ActionResult GridListJson_Up(string QualityCheckModelId)
        {
            try
            {
                
                List<BBdbR_MatBase> ListData = MyBll2.GetMatList(QualityCheckModelId);
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
        

        /// <summary>
        /// 获取检查模板对应已配置产品信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ActionResult Right_GridMatListJson(string value)
        {
            
            List<BBdbR_MatBase> ListData = new List<BBdbR_MatBase>();
            try
            {
                List<BBdbR_QuaCheckModelCarConfig> ListData1 = MyBll1.GetPlanList(value);
                if (ListData1.Count > 0)
                {
                    for (int i = 0; i < ListData1.Count; i++)
                    {
                        BBdbR_MatBase Matentity = MyBll2.GetPlanList1(ListData1[i].MatId);
                        if(Matentity != null)
                        {
                            ListData.Add(Matentity);
                        }
                        
                    }
                }
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
        #region 获取全部车身部位清单
        /// <summary>
        /// 获取全部车身部位清单
        /// </summary>
        /// <param name="patientcodes"></param>
        /// <returns></returns>
        public ActionResult GridListJson_Up_1(string QualityCheckModelId)
        {
            try
            {

                List<BBdbR_CarPartBase> ListData = MyBll3.GetCarPartList(QualityCheckModelId);
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


        /// <summary>
        /// 获取检查模板对应已配置车身部位信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ActionResult Right_GridCarPartListJson(string value)
        {

            List<BBdbR_CarPartBase> ListData = new List<BBdbR_CarPartBase>();
            try
            {
                List<BBdbR_QuaCheckModelCarPartConfig> ListData1 = MyBll4.GetPlanList(value);
                if (ListData1.Count > 0)
                {
                    for (int i = 0; i < ListData1.Count; i++)
                    {
                        BBdbR_CarPartBase CarPartentity = MyBll3.GetPlanList1(ListData1[i].CarPartId);
                        if (CarPartentity != null)
                        {
                            ListData.Add(CarPartentity);
                        }

                    }
                }
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
        #region 提交检查模板产品配置信息
        /// <summary>
        /// 提交检查模板产品配置信息
        /// </summary>
        /// <param name="patientcodes"></param>
        /// <returns></returns>
        public ActionResult SubmitMatConfig(List<BBdbR_MatBase> listdto, string QualityCheckModelId)
        {
            try
            {
                var Message = "配置失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//用于判断
                int IsOk1 = 0;//判断删除方法是否成，0表示不成功，大于0表示成功             
                string Result = "";
                if (QualityCheckModelId != "")//先把对应QualityCheckModelId中所有产品均删除，然后插入已配置产品
                {
                    List<BBdbR_QuaCheckModelCarConfig> ListData1 = MyBll1.GetMatList(QualityCheckModelId, "");
                    if (ListData1.Count > 0)
                    {
                        IsOk1 = MyBll1.Delete(ListData1);//执行删除操作
                    }
                }
                if (listdto!=null && listdto.Count > 0)
                {
                    for (int i = 0; i < listdto.Count; i++)
                    {
                        List<BBdbR_QuaCheckModelCarConfig> ListData = MyBll1.GetMatList(QualityCheckModelId, listdto[i].MatId.ToString());
                        if (ListData.Count == 0)
                        {
                            IsOk = MyBll1.Insert(listdto[i], QualityCheckModelId);//将实体插入数据库，插入成功返回1，失败返回0；
                        }
                        else
                        {
                            continue;
                        }
                    }

                    Message = "配置成功";
                    //this.WriteLog(IsOk, entity, null, TeamId, Message);//记录日志
                }
                else
                {
                    IsOk = 1;
                    Message = "配置成功";
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion
        #region 提交检查模板车身部位配置信息
        /// <summary>
        /// 提交检查模板车身部位配置信息
        /// </summary>
        /// <param name="patientcodes"></param>
        /// <returns></returns>
        public ActionResult SubmitCarPartConfig(List<BBdbR_CarPartBase> listdto, string QualityCheckModelId)
        {
            try
            {
                var Message = "配置失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//用于判断
                int IsOk1 = 0;//判断删除方法是否成，0表示不成功，大于0表示成功             
                string Result = "";
                if (QualityCheckModelId != "")//先把对应QualityCheckModelId中所有车身部位均删除，然后插入已配置车身部位
                {
                    List<BBdbR_QuaCheckModelCarPartConfig> ListData1 = MyBll4.GetCarPartList(QualityCheckModelId, "");
                    if (ListData1.Count > 0)
                    {
                        IsOk1 = MyBll4.Delete(ListData1);//执行删除操作
                    }
                }
                if (listdto!=null && listdto.Count > 0)
                {
                    for (int i = 0; i < listdto.Count; i++)
                    {
                        List<BBdbR_QuaCheckModelCarPartConfig> ListData = MyBll4.GetCarPartList(QualityCheckModelId, listdto[i].CarPartId.ToString());
                        if (ListData.Count == 0)
                        {
                            IsOk = MyBll4.Insert(listdto[i], QualityCheckModelId);//将实体插入数据库，插入成功返回1，失败返回0；
                        }
                        else
                        {
                            continue;
                        }
                    }

                    Message = "配置成功";
                    //this.WriteLog(IsOk, entity, null, TeamId, Message);//记录日志
                }
                else
                {
                    IsOk = 1;
                    Message = "配置成功";
                }
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