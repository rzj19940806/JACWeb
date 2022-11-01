using HfutIE.Business;
using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HfutIE.WebApp.Areas.QualityAdd_TZModule.Controllers
{
    public class BBdbR_QualityCarComponentBase_AddController : PublicController<BBdbR_QualityCarComponentBase_Add>
    {
        // 车身组件管理控制器
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_QualityCarComponentBase_Add";//===复制时需要修改===
        public readonly RepositoryFactory<BBdbR_QualityCarComponentBase_Add> repository_postbase = new RepositoryFactory<BBdbR_QualityCarComponentBase_Add>();//?
        //public readonly RepositoryFactory<BBdbR_CarTypeAndComponentConfig> repository_CarTypeAndComponentConfig = new RepositoryFactory<BBdbR_CarTypeAndComponentConfig>();//?
        #endregion

        #region 创建数据库操作对象区域
        BBdbR_QualityCarComponentBaseBll_Add MyBll = new BBdbR_QualityCarComponentBaseBll_Add(); //===复制时需要修改===
        //BBdbR_CarTypeAndComponentConfigBll MyBll1 = new BBdbR_CarTypeAndComponentConfigBll();//===复制时需要修改===
        #endregion

        #region 打开视图区域
        public ActionResult ConfigForm()//打开车型车身组件配置界面页面
        {
            return View();
        }
        #endregion

        #region 方法区

        #region 1.获取树
        public ActionResult TreeJson()
        {
            try
            {
                DataTable dt = MyBll.GetTree("TZ");//获取树所需数据
                List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();

                #region 加一个全部的分支
                TreeJsonEntity treeALL = new TreeJsonEntity();
                treeALL.id = "all";
                treeALL.text = "全部";
                treeALL.value = "all";
                treeALL.parentId = "0";
                treeALL.Attribute = "Type";
                treeALL.AttributeValue = "0";
                treeALL.isexpand = true;
                treeALL.complete = true;
                treeALL.hasChildren = false;
                treeALL.img = "/Content/Images/Icon16/house.png";
                TreeList.Add(treeALL);
                #endregion

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
                        else if (row["sort"].ToString() == "1")
                        {
                            tree.img = "/Content/Images/Icon16/outlook_new_meeting.png";
                        }
                        else if (row["sort"].ToString() == "2")
                        {
                            tree.img = "/Content/Images/Icon16/role.png";
                        }
                        //tree.img = Business.FactoryModuleHelper<WORKSHOP>.GetAreaImg(row["table_name"].ToString());
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
        public ActionResult GridListJson(string areaId, string parentId, string Type, string CarComponentCd, string CarComponentNm, JqGridParam jqgridparam)
        {
            try
            {
                #region 原版本
                //Stopwatch watch = CommonHelper.TimerStart();//?
                ////获取点击节点对应的数据（列表）
                //DataTable ListData = MyBll.GetList(areaId, parentId, ref jqgridparam);//===复制时需要修改===
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

                #region 点击树展示表格-修改
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId))//展示全部表格
                {
                    strSql.Append(@"select b.CarPositionNm,b.CarPositionCd,c.CarPositionGroupCd,c.CarPositionGroupNm,a.* from BBdbR_QualityCarComponentBase_Add a left join BBdbR_QualityCarPositionBase_Add b on a.CarPositionId = b.CarPositionId left join  BBdbR_QualityCarPositionGroupBase_Add c on a.CarPositionGroupId = c.CarPositionGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and b.type='TZ'")  ;     
                }
                else
                {
                    if (areaId=="all")
                    {
                        strSql.Append(@"select b.CarPositionNm,b.CarPositionCd,c.CarPositionGroupCd,c.CarPositionGroupNm,a.* from BBdbR_QualityCarComponentBase_Add a left join BBdbR_QualityCarPositionBase_Add b on a.CarPositionId = b.CarPositionId left join  BBdbR_QualityCarPositionGroupBase_Add c on a.CarPositionGroupId = c.CarPositionGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and b.type='TZ'");
                    }
                    else if (Type == "0")//检验岗节点
                    {
                        strSql.Append(@"select b.CarPositionNm,b.CarPositionCd,c.CarPositionGroupCd,c.CarPositionGroupNm,a.* from BBdbR_QualityCarComponentBase_Add a left join BBdbR_QualityCarPositionBase_Add b on a.CarPositionId = b.CarPositionId left join  BBdbR_QualityCarPositionGroupBase_Add c on a.CarPositionGroupId = c.CarPositionGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and a.CarPositionId = '" + areaId + "' and b.type='TZ'"); 
                    }
                    else//检验项目节点
                    {
                        strSql.Append(@"select b.CarPositionNm,b.CarPositionCd,c.CarPositionGroupCd,c.CarPositionGroupNm,a.* from BBdbR_QualityCarComponentBase_Add a left join BBdbR_QualityCarPositionBase_Add b on a.CarPositionId = b.CarPositionId left join  BBdbR_QualityCarPositionGroupBase_Add c on a.CarPositionGroupId = c.CarPositionGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and a.CarPositionGroupId = '" + areaId + "' and b.type='TZ'");
                    }
                }
                //if (CarComponentCd != "" && CarComponentCd != null)
                //{
                //    strSql.Append(" and CarComponentCd like '%" + CarComponentCd + "%'");
                //}
                //else { }
                //if (CarComponentNm != "" && CarComponentNm != null)
                //{
                //    strSql.Append(" and CarComponentNm like '%" + CarComponentNm + "%'");
                //}
                //else { }

                List<DbParameter> parameter = new List<DbParameter>();

                if (!String.IsNullOrEmpty(CarComponentCd))
                {
                    strSql.Append(" and CarComponentCd like @CarComponentCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarComponentCd", "%" + CarComponentCd + "%"));
                }
                if (!String.IsNullOrEmpty(CarComponentNm))
                {
                    strSql.Append(" and CarComponentNm like @CarComponentNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarComponentNm", "%" + CarComponentNm + "%"));
                }


                strSql.Append(" order by  CarComponentNm ");//按照检验部件名称排序
                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);

                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
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
        public override ActionResult SubmitForm(BBdbR_QualityCarComponentBase_Add entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "CarComponentCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.CarComponentCd;  //页面中的编号字段值                 //===复制时需要修改===
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";//keyValue=空吗？是，返回新增；否，返回编辑。

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_QualityCarComponentBase_Add Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.CarComponentId = KeyValue;
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
                //IsOk = MyBll.Delete(array);//执行删除操作
                //直接删除
                if (array != null && array.Length > 0)
                {
                    IsOk = MyBll.Delete(array);//删// MyBll.Delete(array[i]);
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

        #region 5.查询方法
        /// <summary>
        ///  查询方法，本方法为单条件查询，即根据一个条件进行查询
        /// 查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        /// 查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        /// 本查询采用近似查询（like）
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="Condition"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GridPageByCondition(string areaId, string parentId, string Type, string CarComponentCd, string CarComponentNm, JqGridParam jqgridparam)
        {
            
            try
            {
                #region 查询原版本
                //string keyword = keywords.Trim();
                //Stopwatch watch = CommonHelper.TimerStart();
                //DataTable ListData = MyBll.GetPageListByCondition(keyword, Condition, jqgridparam);//===复制时需要修改===
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

                #region 查询-修改
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId))//展示全部表格
                {
                    strSql.Append(@"select b.CarPositionNm,b.CarPositionCd,c.CarPositionGroupCd,c.CarPositionGroupNm,a.* from BBdbR_QualityCarComponentBase_Add a left join BBdbR_QualityCarPositionBase_Add b on a.CarPositionId = b.CarPositionId left join  BBdbR_QualityCarPositionGroupBase_Add c on a.CarPositionGroupId = c.CarPositionGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and b.type='TZ'");
                }
                else
                {
                    if (areaId == "all")
                    {
                        strSql.Append(@"select b.CarPositionNm,b.CarPositionCd,c.CarPositionGroupCd,c.CarPositionGroupNm,a.* from BBdbR_QualityCarComponentBase_Add a left join BBdbR_QualityCarPositionBase_Add b on a.CarPositionId = b.CarPositionId left join  BBdbR_QualityCarPositionGroupBase_Add c on a.CarPositionGroupId = c.CarPositionGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and b.type='TZ'");
                    }
                    else if (Type == "0")//检验岗节点
                    {
                        strSql.Append(@"select b.CarPositionNm,b.CarPositionCd,c.CarPositionGroupCd,c.CarPositionGroupNm,a.* from BBdbR_QualityCarComponentBase_Add a left join BBdbR_QualityCarPositionBase_Add b on a.CarPositionId = b.CarPositionId left join  BBdbR_QualityCarPositionGroupBase_Add c on a.CarPositionGroupId = c.CarPositionGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and a.CarPositionId = '" + areaId + "'and b.type='TZ' ");
                    }
                    else//检验项目节点
                    {
                        strSql.Append(@"select b.CarPositionNm,b.CarPositionCd,c.CarPositionGroupCd,c.CarPositionGroupNm,a.* from BBdbR_QualityCarComponentBase_Add a left join BBdbR_QualityCarPositionBase_Add b on a.CarPositionId = b.CarPositionId left join  BBdbR_QualityCarPositionGroupBase_Add c on a.CarPositionGroupId = c.CarPositionGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and a.CarPositionGroupId = '" + areaId + "'and b.type='TZ' ");
                    }
                }



                //if (CarComponentCd != "" && CarComponentCd != null)
                //{
                //    strSql.Append(" and CarComponentCd like '%" + CarComponentCd + "%'");
                //}
                //else { }
                //if (CarComponentNm != "" && CarComponentNm != null)
                //{
                //    strSql.Append(" and CarComponentNm like '%" + CarComponentNm + "%'");
                //}
                //else { }
                List<DbParameter> parameter = new List<DbParameter>();

                if (!String.IsNullOrEmpty(CarComponentCd))
                {
                    strSql.Append(" and CarComponentCd like @CarComponentCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarComponentCd", "%" + CarComponentCd + "%"));
                }
                if (!String.IsNullOrEmpty(CarComponentNm))
                {
                    strSql.Append(" and CarComponentNm like @CarComponentNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarComponentNm", "%" + CarComponentNm + "%"));
                }


                strSql.Append(" order by  CarComponentNm ");//按照检验部件名称排序
                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = dt,
                };
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "检验部件信息查询成功");
                return Content(JsonData.ToJson());
                #endregion

            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "检验部件信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 重构导出
        public ActionResult GetExcel_Data(string areaId, string parentId, string Type, string CarComponentCd, string CarComponentNm, JqGridParam jqgridparam)
        {
            try
            {
                #region 根据当前搜索条件查出数据并导出
                StringBuilder strSql = new StringBuilder();
                if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId))//展示全部表格
                {
                    strSql.Append(@"select b.CarPositionNm,b.CarPositionCd,c.CarPositionGroupCd,c.CarPositionGroupNm,a.* from BBdbR_QualityCarComponentBase_Add a left join BBdbR_QualityCarPositionBase_Add b on a.CarPositionId = b.CarPositionId left join  BBdbR_QualityCarPositionGroupBase_Add c on a.CarPositionGroupId = c.CarPositionGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and b.type='TZ'");
                }
                else
                {
                    if (areaId == "all")
                    {
                        strSql.Append(@"select b.CarPositionNm,b.CarPositionCd,c.CarPositionGroupCd,c.CarPositionGroupNm,a.* from BBdbR_QualityCarComponentBase_Add a left join BBdbR_QualityCarPositionBase_Add b on a.CarPositionId = b.CarPositionId left join  BBdbR_QualityCarPositionGroupBase_Add c on a.CarPositionGroupId = c.CarPositionGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and b.type='TZ'");
                    }
                    else if (Type == "0")//检验岗节点
                    {
                        strSql.Append(@"select b.CarPositionNm,b.CarPositionCd,c.CarPositionGroupCd,c.CarPositionGroupNm,a.* from BBdbR_QualityCarComponentBase_Add a left join BBdbR_QualityCarPositionBase_Add b on a.CarPositionId = b.CarPositionId left join  BBdbR_QualityCarPositionGroupBase_Add c on a.CarPositionGroupId = c.CarPositionGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and a.CarPositionId = '" + areaId + "' and b.type='TZ'");
                    }
                    else//检验项目节点
                    {
                        strSql.Append(@"select b.CarPositionNm,b.CarPositionCd,c.CarPositionGroupCd,c.CarPositionGroupNm,a.* from BBdbR_QualityCarComponentBase_Add a left join BBdbR_QualityCarPositionBase_Add b on a.CarPositionId = b.CarPositionId left join  BBdbR_QualityCarPositionGroupBase_Add c on a.CarPositionGroupId = c.CarPositionGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and a.CarPositionGroupId = '" + areaId + "' and b.type='TZ'");
                    }
                }



                //if (CarComponentCd != "" && CarComponentCd != null)
                //{
                //    strSql.Append(" and CarComponentCd like '%" + CarComponentCd + "%'");
                //}
                //else { }
                //if (CarComponentNm != "" && CarComponentNm != null)
                //{
                //    strSql.Append(" and CarComponentNm like '%" + CarComponentNm + "%'");
                //}
                //else { }
                List<DbParameter> parameter = new List<DbParameter>();

                if (!String.IsNullOrEmpty(CarComponentCd))
                {
                    strSql.Append(" and CarComponentCd like @CarComponentCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarComponentCd", "%" + CarComponentCd + "%"));
                }
                if (!String.IsNullOrEmpty(CarComponentNm))
                {
                    strSql.Append(" and CarComponentNm like @CarComponentNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarComponentNm", "%" + CarComponentNm + "%"));
                }


                strSql.Append(" order by  CarComponentNm ");//按照检验部件名称排序
                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                #endregion

                DataTable ListData = new DataTable();
                if (dt.Rows.Count > 0)
                {
                    ListData = dt.DefaultView.ToTable("检验部件信息", true, "CarComponentCd", "CarComponentNm", "CarPositionNm", "CarPositionGroupNm", "Dsc", "CreTm", "CreNm", "MdfTm", "MdfNm", "Rem");//获取表中特定列
                }

                string fileName = "检验部件信息";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_QualityCarComponent(ListData, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "检验部件信息导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "检验部件信息导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion



        //#region 6.配置车型
        ////entity表示页面中传入的实体，KeyValue表示传入的主键
        ////不管是新增还是编辑，页面中都会传入实体（entity）
        ////新增时实体是一个全新的实体
        ////编辑时实体是修改后的实体
        ////只有在编辑时页面中才会传入主键entity（KeyValue），该主键是需要编辑那个实体的主键
        ////
        ////编辑方法首先根据KeyValue是否有值判断是需要编辑还是需要新增
        ////如果是新增就将该实体新增到数据库中
        ////如果是编辑就将该实体更新到数据中
        ////
        ////不管是新增还是编辑首先判断页面输入的编号是否已经存在
        ////如果已经存在就直接返回“该编号已经存在！”的信息
        ////不存在再进行下一步
        //public ActionResult SubmitCarTypeForm(string postData, BBdbR_CarTypeAndComponentConfig entity, string KeyValue)//===复制时需要修改===
        //{
        //    try
        //    {
        //        #region
        //        byte[] photograph = null;
        //        string photograph_type = "";
        //        photograph = GetPhotographFromRequest(out photograph_type);//获取图片的二进制和图片后缀名，方法在下面
        //        NameValueCollection forms = Request.Form;
        //        //Image images = Image.FromStream(new MemoryStream(photograph));
        //        #endregion

        //        int IsOk = 0;//用于判断
        //        string Message = "配置成功。";
        //        //把json转为对象
        //        BBdbR_CarTypeAndComponentConfig p = new BBdbR_CarTypeAndComponentConfig();
        //        JavaScriptSerializer sr = new JavaScriptSerializer();
        //        BBdbR_CarTypeAndComponentConfig p1 = sr.Deserialize(postData, p.GetType()) as BBdbR_CarTypeAndComponentConfig;

        //        p1.CarComponentId = KeyValue;

        //        p1.Create();
        //        IsOk = MyBll1.Insert(p1);//将实体插入数据库，插入成功返回1，失败返回0；
        //        int IsOk1 = MyBll.InsertPicture(KeyValue, photograph, photograph_type);//将实体插入数据库，插入成功返回1，失败返回0；
        //        if (IsOk > 0 && IsOk1 > 0)
        //        {
        //            Message = "配置成功。";
        //        }
        //        //this.WriteLog(IsOk, entity, null, KeyValue, Message);//记录日志
        //        //}
        //        return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        //this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);//记录日志
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
        //    }
        //}
        //#endregion

        //#region 7.配置车型界面填充
        //public ActionResult SetCarTypeForm(string KeyValue)
        //{
        //    try
        //    {
        //        DataTable ListData = MyBll1.SetCarTypeForm(KeyValue);
        //        var JsonData = new
        //        {
        //            rows = ListData,
        //        };
        //        return Content(JsonData.ToJson());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
        //    }
        //}
        //#endregion

        #endregion

        #region 7.插入图片

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
                bool is_standard = CheckPictureSize(bytes);//方法在下面， 将二进制流转化为图片，从而获取该图片的像素大小，并判断图片像素是否小于标准值
                if (is_standard == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 将二进制流转化为图片，从而获取该图片的像素大小，并判断图片像素是否小于标准值
        /// </summary>
        /// <param name="pageData">图片的二进制流</param>
        /// <returns></returns>
        public bool CheckPictureSize(byte[] pageData)
        {
            //将二进制流数据转换为图片  
            Image image = Image.FromStream(new MemoryStream(pageData));
            int width = image.Width;
            int height = image.Height;
            NameValueCollection forms = Request.Form;
            int width_stadard = Convert.ToInt16(forms["width"]);
            int height_stadard = Convert.ToInt16(forms["height"]);
            if (width <= width_stadard && height <= height_stadard)//图片像素大小小于标准值，返回true
            {
                return true;
            }
            else//图片像素大小大于标准值，返回false
            {
                return false;
            }
        }
        #endregion

        #region 9.提交图片
        /// <summary>
        /// 提交物料基础信息编辑表单
        /// </summary>
        /// <param name="ProductKey">产品编码</param>
        /// <param name="productdto">产品实体</param>
        /// <returns></returns>
        public ActionResult SubmitPicture(string KeyValue)
        {
            try
            {
                string Message = "";
                byte[] photograph = null;
                string photograph_type = "";
                photograph = GetPhotographFromRequest(out photograph_type);//获取图片的二进制和图片后缀名，方法在下面
                NameValueCollection forms = Request.Form;
                int IsOk = MyBll.InsertPicture(KeyValue, photograph, photograph_type);//将实体插入数据库，插入成功返回1，失败返回0；
                if (IsOk > 0)
                {
                    Message = "插入图片成功！";
                }
                else
                {
                    Message = "插入图片失败！";
                }

                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        ///// <summary>
        ///// 提交物料基础信息编辑表单
        ///// </summary>
        ///// <param name=""></param>
        ///// <param name=""></param>
        ///// <returns></returns>
        //public ActionResult SubmitPicture1(string KeyValue, BBdbR_CarTypeAndComponentConfig entity, string file)
        //{
        //    try
        //    {
        //        int IsOk = 0;//用于判断
        //        string Name = "MatCd";
        //        //string Value = entity.MatCd;  //页面中的编号字段值                 //===复制时需要修改===
        //        string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
        //        if (!string.IsNullOrEmpty(KeyValue))//编辑操作
        //        {
        //            //===复制时需要修改===
        //            //entity.MatId = KeyValue;
        //            //entity.Modify(KeyValue);//调用扩展编辑方法
        //            //int a = MyBll.Update(entity);//将实体插入数据库，插入成功返回1，失败返回0；

        //            //if (a > 0 && file != "undefined")
        //            //{
        //            //    byte[] photograph = null;
        //            //    string photograph_type = "";
        //            //    photograph = GetPhotographFromRequest(out photograph_type);//获取图片的二进制和图片后缀名，方法在下面
        //            //    NameValueCollection forms = Request.Form;
        //            //    IsOk = MyBll.InsertPicture(entity.MatCd, photograph, photograph_type);//将实体插入数据库，插入成功返回1，失败返回0；
        //            //    if (IsOk > 0)
        //            //    {
        //            //        Message = "编辑物料信息成功！";
        //            //    }
        //            //    else
        //            //    {
        //            //        Message = "编辑物料信息失败！";
        //            //    }
        //            //}
        //            //if (a > 0)//编辑但是没有更新图片
        //            //{
        //            //    IsOk = 1;
        //            //    Message = "编辑物料信息成功！";
        //            //}
        //        }
        //        else//新增操作
        //        {
        //            //IsOk = MyBll.CheckCount(Name, Value);//判断页面中填写的数据的编号字段的值是否存在
        //            //if (IsOk > 0)//存在
        //            //{
        //            //    Message = "该编号已经存在！";
        //            //    return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
        //            //}

        //            entity.Create();
        //            var files = System.Web.HttpContext.Current.Request.Files;
        //            var file1 = files[0].InputStream;
        //            byte[] buff = new byte[file1.Length];
        //            int a = MyBll1.Insert(entity);//将实体插入数据库，插入成功返回1，失败返回0；
        //            if (a > 0)
        //            {
        //                byte[] photograph = null;
        //                string photograph_type = "";
        //                photograph = GetPhotographFromRequest(out photograph_type);//获取图片的二进制和图片后缀名，方法在下面
        //                NameValueCollection forms = Request.Form;
        //                IsOk = MyBll.InsertPicture(entity.CarComponentId, photograph, photograph_type);//将实体插入数据库，插入成功返回1，失败返回0；
        //                if (IsOk > 0)
        //                {
        //                    Message = "新增信息成功！";
        //                }
        //                else
        //                {
        //                    Message = "新增信息失败！";
        //                }
        //            }
        //        }
        //        return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());

        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
        //    }
        //}

        /// <summary>
        /// 从request中获取上传图片的二进制
        /// </summary>
        /// <param name="photograph_type">通过out修饰符返回图片后缀名</param>
        /// <returns></returns>
        public byte[] GetPhotographFromRequest(out string photograph_type)
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files["file"];//获取上传的文件
                photograph_type = file.FileName.Substring(file.FileName.LastIndexOf('.'));//获取上传的文件后缀名
                if (file != null && file.InputStream != null)
                {
                    Stream st = file.InputStream;//生成文件流对象
                    byte[] bytes = new byte[st.Length];
                    BinaryReader br = new BinaryReader((Stream)st);
                    bytes = br.ReadBytes((Int32)st.Length);
                    return bytes;
                }
                return null;
            }
            catch (Exception)
            {
                photograph_type = "";
                return null;
            }
        }
        #endregion

        #region 8.加载图片

        //
        public ActionResult LoadImage(string KeyValue)
        {
            DataTable ListData = MyBll.GetImage(KeyValue);
            var JsonData = new
            {

                rows = ListData,
            };
            return Content(JsonData.ToJson());
        }

        #endregion

        
    }
}
