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

namespace HfutIE.WebApp.Areas.QualityModule.Controllers
{
    /// <summary>
    /// 缺陷明细信息表控制器
    /// </summary>
    public class BBdbR_DefectCatgDeTailController : PublicController<BBdbR_DefectCatgDeTail>
    {
        #region 创建数据库操作对象区域
        BBdbR_DefectCatgDeTailBll MyBll = new BBdbR_DefectCatgDeTailBll(); //===复制时需要修改===
        #endregion

        #region 方法区
        #region 1.获取树
        public ActionResult TreeJson()
        {
            try
            {
                DataTable dt = MyBll.GetTree();//获取树所需数据
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
        #endregion.

        #region 2.点击树展示表格
        /// <summary>
        /// 根据点击树的节点在数据库中查询相应的信息
        /// </summary>
        /// <param name="areaId">点击的节点的主键</param>
        /// <param name="parentId">节点的父级主键</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GridListJson(string areaId, string sort, string DefectCd, string DefectNm, JqGridParam jqgridparam)
        {
            try
            {
                #region 原版本
                //Stopwatch watch = CommonHelper.TimerStart();
                ////===复制时需要修改===
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

                #region 修改方法
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(sort))//展示全部表格
                {
                    strSql.Append(@"select b.DefectCatgNm,b.DefectCatgCd,c.DefectCatgGroupCd,c.DefectCatgGroupNm,a.* from BBdbR_DefectCatgDeTail a left join BBdbR_DefectCatgBase b on a.DefectCatgId = b.DefectCatgId left join  BBdbR_DefectCatgGroupBase c on a.DefectCatgGroupId = c.DefectCatgGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 ");
                }
                else
                {
                    if (areaId == "all")
                    {
                        strSql.Append(@"select b.DefectCatgNm,b.DefectCatgCd,c.DefectCatgGroupCd,c.DefectCatgGroupNm,a.* from BBdbR_DefectCatgDeTail a left join BBdbR_DefectCatgBase b on a.DefectCatgId = b.DefectCatgId left join  BBdbR_DefectCatgGroupBase c on a.DefectCatgGroupId = c.DefectCatgGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 ");
                    }
                    else if (sort == "0")//缺陷类型节点
                    {
                        strSql.Append(@"select b.DefectCatgNm,b.DefectCatgCd,c.DefectCatgGroupCd,c.DefectCatgGroupNm,a.* from BBdbR_DefectCatgDeTail a left join BBdbR_DefectCatgBase b on a.DefectCatgId = b.DefectCatgId left join  BBdbR_DefectCatgGroupBase c on a.DefectCatgGroupId = c.DefectCatgGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and a.DefectCatgId = '" + areaId + "' ");
                    }
                    else//缺陷分组节点
                    {
                        strSql.Append(@"select b.DefectCatgNm,b.DefectCatgCd,c.DefectCatgGroupCd,c.DefectCatgGroupNm,a.* from BBdbR_DefectCatgDeTail a left join BBdbR_DefectCatgBase b on a.DefectCatgId = b.DefectCatgId left join  BBdbR_DefectCatgGroupBase c on a.DefectCatgGroupId = c.DefectCatgGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and a.DefectCatgGroupId = '" + areaId + "' ");
                    }
                }



                //if (DefectCd != "" && DefectCd != null)
                //{
                //    strSql.Append(" and DefectCd like '%" + DefectCd + "%'");
                //}
                //else { }
                //if (DefectNm != "" && DefectNm != null)
                //{
                //    strSql.Append(" and DefectNm like '%" + DefectNm + "%'");
                //}
                //else { }

                List<DbParameter> parameter = new List<DbParameter>();

                if (!String.IsNullOrEmpty(DefectCd))
                {
                    strSql.Append(" and DefectCd like @DefectCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@DefectCd", "%" + DefectCd + "%"));
                }
                if (!String.IsNullOrEmpty(DefectNm))
                {
                    strSql.Append(" and DefectNm like @DefectNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@DefectNm", "%" + DefectNm + "%"));
                }


                strSql.Append(" order by  DefectNm ");//按照缺陷名称排序
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

        #region 3.查询
        public ActionResult GridPageByCondition(string areaId, string sort, string DefectCd, string DefectNm, JqGridParam jqgridparam)
        {
            try
            {
                #region 原方法
                //Stopwatch watch = CommonHelper.TimerStart();
                //List<BBdbR_DefectCatgDeTail> ListData;
                //ListData = MyBll.GetGridList(Condition, keywords, Nodeareaid,treesort, jqgridparam);
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

                #region 修改方法
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(sort))//展示全部表格
                {
                    strSql.Append(@"select b.DefectCatgNm,b.DefectCatgCd,c.DefectCatgGroupCd,c.DefectCatgGroupNm,a.* from BBdbR_DefectCatgDeTail a left join BBdbR_DefectCatgBase b on a.DefectCatgId = b.DefectCatgId left join  BBdbR_DefectCatgGroupBase c on a.DefectCatgGroupId = c.DefectCatgGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 ");
                }
                else
                {
                    if (areaId == "all")
                    {
                        strSql.Append(@"select b.DefectCatgNm,b.DefectCatgCd,c.DefectCatgGroupCd,c.DefectCatgGroupNm,a.* from BBdbR_DefectCatgDeTail a left join BBdbR_DefectCatgBase b on a.DefectCatgId = b.DefectCatgId left join  BBdbR_DefectCatgGroupBase c on a.DefectCatgGroupId = c.DefectCatgGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 ");
                    }
                    else if (sort == "0")//缺陷类型节点
                    {
                        strSql.Append(@"select b.DefectCatgNm,b.DefectCatgCd,c.DefectCatgGroupCd,c.DefectCatgGroupNm,a.* from BBdbR_DefectCatgDeTail a left join BBdbR_DefectCatgBase b on a.DefectCatgId = b.DefectCatgId left join  BBdbR_DefectCatgGroupBase c on a.DefectCatgGroupId = c.DefectCatgGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and a.DefectCatgId = '" + areaId + "' ");
                    }
                    else//缺陷分组节点
                    {
                        strSql.Append(@"select b.DefectCatgNm,b.DefectCatgCd,c.DefectCatgGroupCd,c.DefectCatgGroupNm,a.* from BBdbR_DefectCatgDeTail a left join BBdbR_DefectCatgBase b on a.DefectCatgId = b.DefectCatgId left join  BBdbR_DefectCatgGroupBase c on a.DefectCatgGroupId = c.DefectCatgGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and a.DefectCatgGroupId = '" + areaId + "' ");
                    }
                }



                //if (DefectCd != "" && DefectCd != null)
                //{
                //    strSql.Append(" and DefectCd like '%" + DefectCd + "%'");
                //}
                //else { }
                //if (DefectNm != "" && DefectNm != null)
                //{
                //    strSql.Append(" and DefectNm like '%" + DefectNm + "%'");
                //}
                //else { }
                List<DbParameter> parameter = new List<DbParameter>();

                if (!String.IsNullOrEmpty(DefectCd))
                {
                    strSql.Append(" and DefectCd like @DefectCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@DefectCd", "%" + DefectCd + "%"));
                }
                if (!String.IsNullOrEmpty(DefectNm))
                {
                    strSql.Append(" and DefectNm like @DefectNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@DefectNm", "%" + DefectNm + "%"));
                }


                strSql.Append(" order by  DefectNm ");//按照缺陷名称排序
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "缺陷详情信息查询成功");
                return Content(JsonData.ToJson());
                #endregion

            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "缺陷详情信息查询发生异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }

        }
        #endregion

        #region 4.提交新增编辑表单
        public ActionResult Submit(string KeyValue, string catggroupid, BBdbR_DefectCatgDeTail entity)
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "DefectCd";        //页面中的编号字段名 
                string Value = entity.DefectCd;  //页面中的编号字段值           
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";//keyValue=空吗？是，返回新增；否，返回编辑。

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_DefectCatgDeTail Oldentity1 = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    BBdbR_DefectCatgDeTail Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);
                    Oldentity.Rem = entity.Rem;
                    Oldentity.Dsc = entity.Dsc;
                    Oldentity.MdfTm = DateTime.Now;
                    Oldentity.MdfCd = ManageProvider.Provider.Current().UserId;
                    Oldentity.MdfNm = ManageProvider.Provider.Current().UserName;
                    IsOk = MyBll.Update(Oldentity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, Oldentity, Oldentity1, KeyValue, Message);//记录日志                 
                }
                else//新增操作
                {
                    IsOk = MyBll.CheckCount(Name, Value);//判断页面中填写的数据的编号字段的值是否存在
                    if (IsOk > 0)//存在
                    {
                        Message = "该编号已经存在！";
                        return Content(new JsonMessage { Success = false, Code = IsOk.ToString(), Message = Message }.ToString());
                    }
                    var defectCatgId = MyBll.GetCatgId(catggroupid);
                    entity.DefectId = System.Guid.NewGuid().ToString();
                    entity.DefectCatgId = defectCatgId;
                    entity.DefectCatgGroupId = catggroupid;
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

        #region 5.删除方法
     
        public ActionResult DeleteCatg(string KeyValue)
        {
            //BBdbR_DefectCatgDeTailBll DefectCatgGroupBaseBll = new BBdbR_DefectCatgDeTailBll();
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                string Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功                   
                IsOk = MyBll.Delete(array);//执行删除操作
                 if (IsOk > 0)//表示删除成功
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

        #region 6.重构导出
        public ActionResult GetExcel_Data(string areaId, string sort, string DefectCd, string DefectNm, JqGridParam jqgridparam)
        {
            try
            {
                #region 根据当前搜索条件查出数据并导出
                StringBuilder strSql = new StringBuilder();
                if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(sort))//展示全部表格
                {
                    strSql.Append(@"select a.DefectCd,a.DefectNm,b.DefectCatgNm,c.DefectCatgGroupNm,a.Dsc,a.CreTm,a.CreNm,a.MdfTm,a.MdfNm,a.Rem from BBdbR_DefectCatgDeTail a left join BBdbR_DefectCatgBase b on a.DefectCatgId = b.DefectCatgId left join  BBdbR_DefectCatgGroupBase c on a.DefectCatgGroupId = c.DefectCatgGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 ");
                }
                else
                {
                    if (areaId == "all")
                    {
                        strSql.Append(@"select a.DefectCd,a.DefectNm,b.DefectCatgNm,c.DefectCatgGroupNm,a.Dsc,a.CreTm,a.CreNm,a.MdfTm,a.MdfNm,a.Rem  from BBdbR_DefectCatgDeTail a left join BBdbR_DefectCatgBase b on a.DefectCatgId = b.DefectCatgId left join  BBdbR_DefectCatgGroupBase c on a.DefectCatgGroupId = c.DefectCatgGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 ");
                    }
                    else if (sort == "0")//缺陷类型节点
                    {
                        strSql.Append(@"select a.DefectCd,a.DefectNm,b.DefectCatgNm,c.DefectCatgGroupNm,a.Dsc,a.CreTm,a.CreNm,a.MdfTm,a.MdfNm,a.Rem  from BBdbR_DefectCatgDeTail a left join BBdbR_DefectCatgBase b on a.DefectCatgId = b.DefectCatgId left join  BBdbR_DefectCatgGroupBase c on a.DefectCatgGroupId = c.DefectCatgGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and a.DefectCatgId = '" + areaId + "' ");
                    }
                    else//缺陷分组节点
                    {
                        strSql.Append(@"select a.DefectCd,a.DefectNm,b.DefectCatgNm,c.DefectCatgGroupNm,a.Dsc,a.CreTm,a.CreNm,a.MdfTm,a.MdfNm,a.Rem  from BBdbR_DefectCatgDeTail a left join BBdbR_DefectCatgBase b on a.DefectCatgId = b.DefectCatgId left join  BBdbR_DefectCatgGroupBase c on a.DefectCatgGroupId = c.DefectCatgGroupId where a.Enabled=1 and b.Enabled =1 and c.Enabled =1 and a.DefectCatgGroupId = '" + areaId + "' ");
                    }
                }



                //if (DefectCd != "" && DefectCd != null)
                //{
                //    strSql.Append(" and DefectCd like '%" + DefectCd + "%'");
                //}
                //else { }
                //if (DefectNm != "" && DefectNm != null)
                //{
                //    strSql.Append(" and DefectNm like '%" + DefectNm + "%'");
                //}
                //else { }
                List<DbParameter> parameter = new List<DbParameter>();

                if (!String.IsNullOrEmpty(DefectCd))
                {
                    strSql.Append(" and DefectCd like @DefectCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@DefectCd", "%" + DefectCd + "%"));
                }
                if (!String.IsNullOrEmpty(DefectNm))
                {
                    strSql.Append(" and DefectNm like @DefectNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@DefectNm", "%" + DefectNm + "%"));
                }


                strSql.Append(" order by  DefectNm ");//按照缺陷名称排序
                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                #endregion



                string fileName = "缺陷详细";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_DefectCatgDeTail(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other,"1", "缺陷详情信息导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "缺陷详情信息导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        #endregion

    }
}