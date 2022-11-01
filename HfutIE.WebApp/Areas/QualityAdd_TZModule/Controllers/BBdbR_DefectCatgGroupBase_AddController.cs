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

namespace HfutIE.WebApp.Areas.QualityAdd_TZModule.Controllers
{
    /// <summary>
    /// 缺陷类别分组基本信息表控制器
    /// </summary>
    public class BBdbR_DefectCatgGroupBase_AddController : PublicController<BBdbR_DefectCatgGroupBase_Add>
    {
        #region 创建数据库操作对象区域
        BBdbR_DefectCatgGroupBase_AddBll MyBll = new BBdbR_DefectCatgGroupBase_AddBll(); //===复制时需要修改===
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
        public ActionResult GridListJson(string sort, string areaId, string DefectCatgGroupCd, string DefectCatgGroupNm, JqGridParam jqgridparam)
        {
            try
            {
                #region 原版本
                //Stopwatch watch = CommonHelper.TimerStart();
                //DataTable ListData = MyBll.GetList(areaId, text, value, sort, ref jqgridparam);
                //var JsonData = new
                //{
                //    total = jqgridparam.total,
                //    page = jqgridparam.page,
                //    records = jqgridparam.records,
                //    costtime = CommonHelper.TimerEnd(watch),
                //    rows = ListData,
                //};
                //return Content(JsonData.ToJson());
                #endregion

                #region 查询修改
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select a.*,b.DefectCatgCd,b.DefectCatgNm from BBdbR_DefectCatgGroupBase_Add a left join BBdbR_DefectCatgBase_Add b on a.DefectCatgId = b.DefectCatgId where a.Enabled =1 and b.Enabled =1 and b.type='TZ' ");
                if (areaId != "all" && areaId != "undefined"&& areaId!="")
                {
                    strSql.Append(" and a.DefectCatgId = '" + areaId + "' ");
                }
                //else { }
                //if (DefectCatgGroupCd != "" && DefectCatgGroupCd != null)
                //{
                //    strSql.Append(" and DefectCatgGroupCd like '%" + DefectCatgGroupCd + "%' ");
                //}
                //else { }
                //if (DefectCatgGroupNm != "" && DefectCatgGroupNm != null)
                //{
                //    strSql.Append(" and DefectCatgGroupNm like '%" + DefectCatgGroupNm + "%' ");
                //}
                //else { }

                List<DbParameter> parameter = new List<DbParameter>();

                if (!String.IsNullOrEmpty(DefectCatgGroupCd))
                {
                    strSql.Append(" and DefectCatgGroupCd like @DefectCatgGroupCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@DefectCatgGroupCd", "%" + DefectCatgGroupCd + "%"));
                }
                if (!String.IsNullOrEmpty(DefectCatgGroupNm))
                {
                    strSql.Append(" and DefectCatgGroupNm like @DefectCatgGroupNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@DefectCatgGroupNm", "%" + DefectCatgGroupNm + "%"));
                }


                strSql.Append(" order by a.DefectCatgGroupNm ");

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

        #region 3.提交新增表单
        public ActionResult Submit(string KeyValue, string Catgid, BBdbR_DefectCatgGroupBase_Add entity)
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "DefectCatgGroupCd";        //页面中的编号字段名 
                string Value = entity.DefectCatgGroupCd;  //页面中的编号字段值           
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";//keyValue=空吗？是，返回新增；否，返回编辑。

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    //BBdbR_DefectCatgGroupBase_Add Oldentity1 = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    BBdbR_DefectCatgGroupBase_Add Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);
                    entity.DefectCatgGroupId = KeyValue;
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
                    entity.DefectCatgGroupId = System.Guid.NewGuid().ToString();
                    entity.DefectCatgId = Catgid;
                    entity.Enabled = "1";
                    entity.VersionNumber = "V1.0";
                    entity.CreTm = DateTime.Now;
                    entity.CreCd = ManageProvider.Provider.Current().UserId;
                    entity.CreNm = ManageProvider.Provider.Current().UserName;
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

        #region 4.查询
        public ActionResult GridPageByCondition(string sort, string areaId, string DefectCatgGroupCd, string DefectCatgGroupNm, JqGridParam jqgridparam)
        {
            try
            {
                #region 原方法
                //Stopwatch watch = CommonHelper.TimerStart();
                //List<BBdbR_DefectCatgGroupBase_Add> ListData;
                //ListData = MyBll.GetGridList(Condition, keywords, Nodeareaid, jqgridparam);
                //var JsonData = new
                //{
                //    total = jqgridparam.total,
                //    page = jqgridparam.page,
                //    records = jqgridparam.records,
                //    costtime = CommonHelper.TimerEnd(watch),
                //    rows = ListData,
                //};
                //return Content(JsonData.ToJson());
                #endregion

                #region 查询修改
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select a.*,b.DefectCatgCd,b.DefectCatgNm from BBdbR_DefectCatgGroupBase_Add a left join BBdbR_DefectCatgBase_Add b on a.DefectCatgId = b.DefectCatgId where a.Enabled =1 and b.Enabled =1  and b.type='TZ'");
                if (areaId != "all" && areaId != "undefined" && areaId != "")
                {
                    strSql.Append(" and a.DefectCatgId = '" + areaId + "' ");
                }
                //else { }
                //if (DefectCatgGroupCd != "" && DefectCatgGroupCd != null)
                //{
                //    strSql.Append(" and DefectCatgGroupCd like '%" + DefectCatgGroupCd + "%' ");
                //}
                //else { }
                //if (DefectCatgGroupNm != "" && DefectCatgGroupNm != null)
                //{
                //    strSql.Append(" and DefectCatgGroupNm like '%" + DefectCatgGroupNm + "%' ");
                //}
                //else { }

                List<DbParameter> parameter = new List<DbParameter>();

                if (!String.IsNullOrEmpty(DefectCatgGroupCd))
                {
                    strSql.Append(" and DefectCatgGroupCd like @DefectCatgGroupCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@DefectCatgGroupCd", "%" + DefectCatgGroupCd + "%"));
                }
                if (!String.IsNullOrEmpty(DefectCatgGroupNm))
                {
                    strSql.Append(" and DefectCatgGroupNm like @DefectCatgGroupNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@DefectCatgGroupNm", "%" + DefectCatgGroupNm + "%"));
                }

                strSql.Append(" order by a.DefectCatgGroupNm ");

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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "缺陷分组信息查询成功");
                return Content(JsonData.ToJson());
                #endregion

            }
            catch (Exception ex)
            {
                this.WriteLog(-1, null, null, null, "操作失败" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "缺陷分组信息查询操作失败：" + ex.Message }.ToString());
            }

        }
        #endregion
        
        #region 5.删除方法
        /// <summary>
        /// 首先判断需要删除的信息是否绑定了其他信息
        ///  
        ///  在确定没绑定数据的情况下，删除该数据
        ///     删除表示将该数据的Enabled属性设为false，并不真的删除该记录
        /// </summary>
        /// <param name="KeyValue">页面中提供的需要删除的数据的主键,可能是多条数据的主键，即多个主键</param>
        /// <param name="ParentId">不需要</param>
        /// <param name="DeleteMark">不需要</param>
        /// <returns></returns>
        public ActionResult DeleteCatgGroup(string KeyValue)
        {
            BBdbR_DefectCatgDeTail_AddBll DefectCatgGroupBaseBll = new BBdbR_DefectCatgDeTail_AddBll();
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                string Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功                   
                if (KeyValue != "")//
                {
                    int a = DefectCatgGroupBaseBll.GetDetailCountbygroup(KeyValue);//查找该缺陷类别分组下是否有具体的缺陷明细，如果有就不可以删除
                    if (a > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "该分组下有具体的缺陷明细，不可删除" }.ToString());
                    }
                    else
                    {
                        IsOk = MyBll.Delete(array);//执行删除操作
                        if (IsOk > 0)//表示删除成
                        {
                            Message = "删除成功。";//修改返回信息
                        }
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

        #region 6.重构导出
        public ActionResult GetExcel_Data(string sort, string areaId, string DefectCatgGroupCd, string DefectCatgGroupNm, JqGridParam jqgridparam)
        {
            try
            {

                #region 根据当前搜索条件查出数据并导出
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select a.DefectCatgGroupCd,a.DefectCatgGroupNm,b.DefectCatgNm,a.Dsc,a.CreTm,a.CreNm,a.MdfTm,a.MdfNm,a.Rem from BBdbR_DefectCatgGroupBase_Add a left join BBdbR_DefectCatgBase_Add b on a.DefectCatgId = b.DefectCatgId where a.Enabled =1 and b.Enabled =1  and b.type='TZ'");
                if (areaId != "all" && areaId != "undefined"&& areaId!="")
                {
                    strSql.Append(" and a.DefectCatgId = '" + areaId + "' ");
                }
                //else { }
                //if (DefectCatgGroupCd != "" && DefectCatgGroupCd != null)
                //{
                //    strSql.Append(" and DefectCatgGroupCd like '%" + DefectCatgGroupCd + "%' ");
                //}
                //else { }
                //if (DefectCatgGroupNm != "" && DefectCatgGroupNm != null)
                //{
                //    strSql.Append(" and DefectCatgGroupNm like '%" + DefectCatgGroupNm + "%' ");
                //}
                //else { }
                List<DbParameter> parameter = new List<DbParameter>();

                if (!String.IsNullOrEmpty(DefectCatgGroupCd))
                {
                    strSql.Append(" and DefectCatgGroupCd like @DefectCatgGroupCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@DefectCatgGroupCd", "%" + DefectCatgGroupCd + "%"));
                }
                if (!String.IsNullOrEmpty(DefectCatgGroupNm))
                {
                    strSql.Append(" and DefectCatgGroupNm like @DefectCatgGroupNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@DefectCatgGroupNm", "%" + DefectCatgGroupNm + "%"));
                }
                strSql.Append(" order by a.DefectCatgGroupNm ");

                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                #endregion

                string fileName = "缺陷分组";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_DefectCatgGroupBase(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "缺陷分组信息导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "缺陷分组信息导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        #region 7.获取缺陷类型下拉框
        public ActionResult GetDefectCatgList()
        {
            try
            {
                #region 
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select * from BBdbR_DefectCatgBase_Add where Enabled =1  and type='TZ' order by DefectCatgNm");
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