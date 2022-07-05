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
    /// _FactoryBaseInformation控制器
    /// </summary>
    public class BBdbR_WorkshopBaseController : PublicController<BBdbR_WorkshopBase>
    {

        #region 创建数据库操作对象区域
        //创建数据库访问对象，用以访问其中操作数据库的方法
        BBdbR_WorkshopBaseBll MyBll = new BBdbR_WorkshopBaseBll(); //===复制时需要修改===
        public readonly RepositoryFactory<BBdbR_WorkshopBase> repository_workshopbase = new RepositoryFactory<BBdbR_WorkshopBase>();
        #endregion

        #region 打开视图区域
        public ActionResult Select()//打开选择公司子界面页面
        {
            return View();
        }
        #endregion

        #region 方法区   

        #region 1.获取树
        /// <summary>
        /// 获取树
        /// </summary>
        /// <returns></returns>
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
                        tree.isexpand = true;
                        tree.complete = true;
                        tree.hasChildren = hasChildren;
                        if (row["sort"].ToString() == "0")
                        {

                            tree.img = "/Content/Images/Icon16/house.png";
                        }
                        else if (row["sort"].ToString() != "0")
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
        public ActionResult GridListJson(string areaId, string Type, string WorkshopCd, string WorkshopNm, string WorkshopTyp, JqGridParam jqgridparam)
        {
            try
            {
                #region 查询原方法-不考虑搜索条件
                //Stopwatch watch = CommonHelper.TimerStart();
                ////获取点击节点对应的数据（列表）
                //DataTable ListData = MyBll.GetWorkShopList(areaId, parentId, ref jqgridparam);//===复制时需要修改===
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

                #region 查询修改-考虑搜索条件
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(Type))
                {
                    strSql.Append(@"select a.*,b.FacCd as FacCd,b.FacNm as FacNm from BBdbR_WorkshopBase a join BBdbR_FacBase b on a.FacId=b.FacId join BBdbR_CompanyBase c on b.CompanyId=c.CompanyId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 ");
                }
                else
                {
                    //点击公司
                    if (Type == "0")
                    {
                        strSql.Append(@"select a.*,b.FacCd as FacCd,b.FacNm as FacNm from BBdbR_WorkshopBase a join BBdbR_FacBase b on a.FacId=b.FacId join BBdbR_CompanyBase c on b.CompanyId=c.CompanyId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.CompanyId='" + areaId + "' ");     
                    }
                    //点击工厂
                    else
                    {
                        strSql.Append(@"select a.*,b.FacCd as FacCd,b.FacNm as FacNm from BBdbR_WorkshopBase a join BBdbR_FacBase b on a.FacId=b.FacId join BBdbR_CompanyBase c on b.CompanyId=c.CompanyId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1  and a.FacId='" + areaId + "' ");
                    }
                }

                //是否加车间编号模糊搜索
                if (WorkshopCd != "" && WorkshopCd != null)
                {
                    strSql.Append(" and a.WorkshopCd like @WorkshopCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@WorkshopCd", "%" + WorkshopCd + "%"));
                }
                else { }
                //是否加车间名称模糊搜索
                if (WorkshopNm != "" && WorkshopNm != null)
                {
                    strSql.Append(" and a.WorkshopNm like @WorkshopNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@WorkshopNm", "%" + WorkshopNm + "%"));
                }
                else { }
                //是否加车间类型搜索
                if (WorkshopTyp != "" && WorkshopTyp != null)
                {
                    strSql.Append(" and a.WorkshopTyp = @WorkshopTyp");
                    parameter.Add(DbFactory.CreateDbParameter("@WorkshopTyp", WorkshopTyp));
                }
                else { }

                //按照顺序号排序
                strSql.Append(" order by a.sort asc");

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
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion          

        #region 3.新增编辑方法
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
        public  ActionResult SubmitFormWC(BBdbR_WorkshopBase entity, string KeyValue,string stfid,string stfcd, string stfnm, string phn, string FacId)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "WorkshopCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.WorkshopCd;  //页面中的编号字段值                 //===复制时需要修改===
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (entity.Dsc == null)
                {
                    entity.Dsc = "";
                }
                else { }
                if (entity.Rem == null)
                {
                    entity.Rem = "";
                }
                else { }

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_WorkshopBase Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
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
                    entity.FacId = FacId;
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
            BBdbR_WorkSectionBaseBll WorkSectionBll = new BBdbR_WorkSectionBaseBll();
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                ArrayList newarray = new ArrayList();
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功                
                int a = WorkSectionBll.GetWorkSectionCount(KeyValue);//查找工厂基础信息中是否有在本公司名下，如果有就不可以删除
                if (a > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "车间下设置工艺段，不可删除" }.ToString());
                }
                else
                {
                    IsOk = MyBll.Delete(array);//执行删除操作
                    if (IsOk > 0)//表示删除成
                    {
                        Message = "删除成功。";//修改返回信息
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
        #endregion

        #region 5.查询方法
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageByCondition(string areaId, string Type, string WorkshopCd, string WorkshopNm, string WorkshopTyp, JqGridParam jqgridparam)
        {
            try
            {
                #region 查询原方法-不考虑树节点
                //string keyword = keywords.Trim();
                //Stopwatch watch = CommonHelper.TimerStart();
                //DataTable  ListData = MyBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
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


                #region 查询修改-考虑树节点
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(Type))
                {
                    strSql.Append(@"select a.*,b.FacCd as FacCd,b.FacNm as FacNm from BBdbR_WorkshopBase a join BBdbR_FacBase b on a.FacId=b.FacId join BBdbR_CompanyBase c on b.CompanyId=c.CompanyId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 ");
                }
                else
                {
                    //点击公司
                    if (Type == "0")
                    {
                        strSql.Append(@"select a.*,b.FacCd as FacCd,b.FacNm as FacNm from BBdbR_WorkshopBase a join BBdbR_FacBase b on a.FacId=b.FacId join BBdbR_CompanyBase c on b.CompanyId=c.CompanyId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.CompanyId='" + areaId + "' ");
                    }
                    //点击工厂
                    else
                    {
                        strSql.Append(@"select a.*,b.FacCd as FacCd,b.FacNm as FacNm from BBdbR_WorkshopBase a join BBdbR_FacBase b on a.FacId=b.FacId join BBdbR_CompanyBase c on b.CompanyId=c.CompanyId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1  and a.FacId='" + areaId + "' ");
                    }
                }

                //是否加车间编号模糊搜索
                if (WorkshopCd != "" && WorkshopCd != null)
                {
                    strSql.Append(" and a.WorkshopCd like @WorkshopCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@WorkshopCd", "%" + WorkshopCd + "%"));
                }
                else { }
                //是否加车间名称模糊搜索
                if (WorkshopNm != "" && WorkshopNm != null)
                {
                    strSql.Append(" and a.WorkshopNm like @WorkshopNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@WorkshopNm", "%" + WorkshopNm + "%"));
                }
                else { }
                //是否加车间类型搜索
                if (WorkshopTyp != "" && WorkshopTyp != null)
                {
                    strSql.Append(" and a.WorkshopTyp = @WorkshopTyp");
                    parameter.Add(DbFactory.CreateDbParameter("@WorkshopTyp", WorkshopTyp));
                }
                else { }

                //按照顺序号排序
                strSql.Append(" order by a.sort asc");

                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    jqgridparam.total,
                    jqgridparam.page,
                    jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "车间基础信息查询成功");
                return Content(JsonData.ToJson());

                #endregion

                
            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "车间基础信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion         

        #region 6.重构导出
        public ActionResult GetExcel_Data(string areaId, string Type, string WorkshopCd, string WorkshopNm, string WorkshopTyp, JqGridParam jqgridparam)
        {
            try
            {


                #region 根据当前搜索条件查出数据并导出
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(Type))
                {
                    strSql.Append(@"select a.WorkshopCd,a.WorkshopNm,a.WorkshopTyp,b.FacCd as FacCd,b.FacNm as FacNm,a.Dsc,a.sort,a.CreTm,a.CreNm,a.MdfTm,a.MdfNm,a.Rem from BBdbR_WorkshopBase a join BBdbR_FacBase b on a.FacId=b.FacId join BBdbR_CompanyBase c on b.CompanyId=c.CompanyId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 ");
                }
                else
                {
                    //点击公司
                    if (Type == "0")
                    {
                        strSql.Append(@"select a.WorkshopCd,a.WorkshopNm,a.WorkshopTyp,b.FacCd as FacCd,b.FacNm as FacNm,a.Dsc,a.sort,a.CreTm,a.CreNm,a.MdfTm,a.MdfNm,a.Rem from BBdbR_WorkshopBase a join BBdbR_FacBase b on a.FacId=b.FacId join BBdbR_CompanyBase c on b.CompanyId=c.CompanyId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.CompanyId='" + areaId + "' ");
                    }
                    //点击工厂
                    else
                    {
                        strSql.Append(@"select a.WorkshopCd,a.WorkshopNm,a.WorkshopTyp,b.FacCd as FacCd,b.FacNm as FacNm,a.Dsc,a.sort,a.CreTm,a.CreNm,a.MdfTm,a.MdfNm,a.Rem from BBdbR_WorkshopBase a join BBdbR_FacBase b on a.FacId=b.FacId join BBdbR_CompanyBase c on b.CompanyId=c.CompanyId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1  and a.FacId='" + areaId + "' ");
                    }
                }

                //是否加车间编号模糊搜索
                if (WorkshopCd != "" && WorkshopCd != null)
                {
                    strSql.Append(" and a.WorkshopCd like @WorkshopCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@WorkshopCd", "%" + WorkshopCd + "%"));
                }
                else { }
                //是否加车间名称模糊搜索
                if (WorkshopNm != "" && WorkshopNm != null)
                {
                    strSql.Append(" and a.WorkshopNm like @WorkshopNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@WorkshopNm", "%" + WorkshopNm + "%"));
                }
                else { }
                //是否加车间类型搜索
                if (WorkshopTyp != "" && WorkshopTyp != null)
                {
                    strSql.Append(" and a.WorkshopTyp = @WorkshopTyp");
                    parameter.Add(DbFactory.CreateDbParameter("@WorkshopTyp", WorkshopTyp));
                }
                else { }

                //按照顺序号排序
                strSql.Append(" order by a.sort asc");

                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                #endregion



                string fileName = "车间管理基础信息";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_WorkshopBase(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "车间基础信息导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "车间基础信息导出操作失败：" + ex.Message);
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

        #region 8.加载全部车间列表
        /// <summary>
        /// 加载列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GridWorkShopJson()
        {
            try
            {
                List<BBdbR_WorkshopBase> ListData = MyBll.GetWorkShopList();
                //var JsonData = new
                //{
                //    rows = ListData,
                //};
                //string a = JsonData.ToJson();
                return Content(ListData.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 9.编辑填充界面数据
        public override ActionResult SetForm(string KeyValue)
        {
            try
            {
                BBdbR_WorkshopBase Deviceentity = MyBll.GetPlanList(KeyValue);
                return Content(Deviceentity.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        //8.人员下拉框
        public ActionResult GetPlineNm(string StfId)
        {
            try
            {
                BBdbR_StfBaseBll StfBll = new BBdbR_StfBaseBll();
                DataTable dataTable = StfBll.GetPlineNm(StfId);
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