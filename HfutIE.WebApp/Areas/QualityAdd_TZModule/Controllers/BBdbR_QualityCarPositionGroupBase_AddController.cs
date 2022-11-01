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

namespace HfutIE.WebApp.Areas.QualityAdd_TZModule.Controllers
{
    public class BBdbR_QualityCarPositionGroupBase_AddController : PublicController<BBdbR_QualityCarPositionGroupBase_Add>
    {
        /// <summary>
        /// 车身部位分组基础信息表控制器
        /// </summary>
        // GET: QualityAdd_TZModule/BBdbR_QualityCarPositionGroupBase_Add

        #region 创建数据库操作对象区域
        BBdbR_QualityCarPositionGroupBaseBll_Add MyBll = new BBdbR_QualityCarPositionGroupBaseBll_Add(); //===复制时需要修改===
        public readonly RepositoryFactory<BBdbR_QualityCarPositionGroupBase_Add> repository_postbase = new RepositoryFactory<BBdbR_QualityCarPositionGroupBase_Add>();
        #endregion

        #region 打开视图区域
        //public ActionResult Select()//打开选择公司子界面页面
        //{
        //    return View();
        //}
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
                        tree.id = area_key;                                 //检验岗Id
                        tree.text = row["name"].ToString();                 //检验岗名称
                        tree.value = row["code"].ToString();                //检验岗Cd
                        tree.parentId = row["parentId"].ToString();         //0
                        tree.Attribute = "Type";                            
                        tree.AttributeValue = row["sort"].ToString();       //层级编号 0
                        tree.isexpand = true;
                        tree.complete = true;
                        tree.hasChildren = hasChildren;
                        if (row["sort"].ToString() == "0")
                        {
                            tree.img = "/Content/Images/Icon16/house.png";
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
        public ActionResult GridListJson(string areaId, string parentId, string CarPositionGroupCd, string CarPositionGroupNm, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();//?
                //获取点击节点对应的数据（列表）
                DataTable ListData = MyBll.GetList("TZ",areaId, parentId, CarPositionGroupCd, CarPositionGroupNm, ref jqgridparam);//===复制时需要修改===
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
        public override ActionResult SubmitForm(BBdbR_QualityCarPositionGroupBase_Add entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "CarPositionGroupCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.CarPositionGroupCd;  //页面中的编号字段值                 //===复制时需要修改===
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";//keyValue=空吗？是，返回新增；否，返回编辑。

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_QualityCarPositionGroupBase_Add Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.CarPositionGroupId = KeyValue;
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
        public ActionResult GridPageByCondition(string CarPositionGroupCd, string CarPositionGroupNm,string areaId, JqGridParam jqgridparam)
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

                #region 查询修改
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select a.*,b.CarPositionCd AS CarPositionCd,b.CarPositionNm AS CarPositionNm from BBdbR_QualityCarPositionGroupBase_Add a join BBdbR_QualityCarPositionBase_Add b on a.CarPositionId=b.CarPositionId where a.Enabled = '1' and b.Enabled = '1' and b.type='TZ'");
                if (areaId != "all"&&areaId!="undefined")
                {
                    strSql.Append(" and a.CarPositionId = '" + areaId + "' ");
                }
                //else { }
                //if (CarPositionGroupCd != "" && CarPositionGroupCd != null)
                //{
                //    strSql.Append(" and CarPositionGroupCd like '%" + CarPositionGroupCd + "%' ");
                //}
                //else { }
                //if (CarPositionGroupNm != "" && CarPositionGroupNm != null)
                //{
                //    strSql.Append(" and CarPositionGroupNm like '%" + CarPositionGroupNm + "%' ");
                //}
                //else { }

                List<DbParameter> parameter = new List<DbParameter>();

                if (!String.IsNullOrEmpty(CarPositionGroupCd))
                {
                    strSql.Append(" and CarPositionGroupCd like @CarPositionGroupCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarPositionGroupCd", "%" + CarPositionGroupCd + "%"));
                }
                if (!String.IsNullOrEmpty(CarPositionGroupNm))
                {
                    strSql.Append(" and CarPositionGroupNm like @CarPositionGroupNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarPositionGroupNm", "%" + CarPositionGroupNm + "%"));
                }



                strSql.Append(" order by CarPositionGroupNm ");

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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "检验项目信息查询成功");
                return Content(JsonData.ToJson());
                #endregion

            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "检验项目信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 重构导出
        public ActionResult GetExcel_Data(string CarPositionGroupCd, string CarPositionGroupNm, string areaId, JqGridParam jqgridparam)
        {
            try
            {

                #region 查询
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select a.CarPositionGroupCd,a.CarPositionGroupNm,b.CarPositionNm AS CarPositionNm,a.Dsc,a.CreTm,a.CreNm,a.MdfTm,a.MdfNm,a.Rem from BBdbR_QualityCarPositionGroupBase_Add a join BBdbR_QualityCarPositionBase_Add b on a.CarPositionId=b.CarPositionId where a.Enabled = '1' and b.type='TZ'");
                if (areaId != "all" && areaId != "undefined")
                {
                    strSql.Append(" and a.CarPositionId = '" + areaId + "' ");
                }
                //else { }
                //if (CarPositionGroupCd != "" && CarPositionGroupCd != null)
                //{
                //    strSql.Append(" and CarPositionGroupCd like '%" + CarPositionGroupCd + "%' ");
                //}
                //else { }
                //if (CarPositionGroupNm != "" && CarPositionGroupNm != null)
                //{
                //    strSql.Append(" and CarPositionGroupNm like '%" + CarPositionGroupNm + "%' ");
                //}
                //else { }

                List<DbParameter> parameter = new List<DbParameter>();

                if (!String.IsNullOrEmpty(CarPositionGroupCd))
                {
                    strSql.Append(" and CarPositionGroupCd like @CarPositionGroupCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarPositionGroupCd", "%" + CarPositionGroupCd + "%"));
                }
                if (!String.IsNullOrEmpty(CarPositionGroupNm))
                {
                    strSql.Append(" and CarPositionGroupNm like @CarPositionGroupNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@CarPositionGroupNm", "%" + CarPositionGroupNm + "%"));
                }


                strSql.Append(" order by CarPositionGroupCd ");

                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);

                #endregion




                string fileName = "检验项目信息";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_QualityCarPositionGroup(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "检验项目信息导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "检验项目信息导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        #region 获取检验岗下拉框
        public ActionResult GetCarPositionList(string CarPositionGroupCd, string CarPositionGroupNm, string areaId, JqGridParam jqgridparam)
        {
            try
            {
                #region 
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select * from BBdbR_QualityCarPositionBase_Add where Enabled =1 and Type='TZ'order by CarPositionNm");
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                var JsonData = new
                {
                    rows = dt,
                };
                return Content(JsonData.ToJson());
                #endregion

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #endregion
    }
}