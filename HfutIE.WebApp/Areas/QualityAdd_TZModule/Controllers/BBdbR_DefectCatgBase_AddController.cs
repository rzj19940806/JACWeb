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
    /// 缺陷类别基本信息表控制器
    /// </summary>
    public class BBdbR_DefectCatgBase_AddController : PublicController<BBdbR_DefectCatgBase_Add>
    {
        #region 创建数据库操作对象区域
        //创建数据库访问对象，用以访问其中操作数据库的方法
        BBdbR_DefectCatgBase_AddBll MyBll = new BBdbR_DefectCatgBase_AddBll(); //===复制时需要修改==       
        #endregion

        #region 方法区
        #region 1.新增编辑
        public ActionResult SubmitForm1(string KeyValue, BBdbR_DefectCatgBase_Add entity)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "DefectCatgCd";        //页面中的编号字段名，如：公司编号  
                string Value = entity.DefectCatgCd;  //页面中的编号字段值           
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";//keyValue=空吗？是，返回新增；否，返回编辑。

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_DefectCatgBase_Add Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.DefectCatgId = KeyValue;
                    entity.Type=Oldentity.Type;
                    entity.MdfTm = DateTime.Now;
                    entity.MdfCd = ManageProvider.Provider.Current().UserId;
                    entity.MdfNm = ManageProvider.Provider.Current().UserName;
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
                    entity.Type = "TZ";
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

        #region 2.删除方法
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
        public ActionResult DeleteCatg(string KeyValue)
        {
            BBdbR_DefectCatgDeTail_AddBll DefectCatgGroupBaseBll = new BBdbR_DefectCatgDeTail_AddBll();
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功                   
                if (KeyValue != "")//先把对应CarPartId中所有检查项目均删除
                {
                    int a = DefectCatgGroupBaseBll.GetDetailCount(KeyValue);//查找改类型缺陷下是否有具体的缺陷明细，如果有就不可以删除
                    if (a > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "该类别下有具体的缺陷明细，不可删除" }.ToString());
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

        #region 3.查询方法
        public ActionResult GetGridPageJson(string DefectCatgCd, string DefectCatgNm, JqGridParam jqgridparam)
        {
            try
            {
                #region datatable查询
                Stopwatch watch = CommonHelper.TimerStart();
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select * from BBdbR_DefectCatgBase_Add where Enabled = 1 and Type='TZ'");
                #region 判断输入框内容添加检索条件
                //是否加缺陷类型编号模糊搜索
                //if (DefectCatgCd != "" && DefectCatgCd != null)
                //{
                //    strSql.Append(" and DefectCatgCd like '%" + DefectCatgCd + "%' ");
                //}
                //else { }
                ////是否加缺陷类型名称模糊搜索
                //if (DefectCatgNm != "" && DefectCatgNm != null)
                //{
                //    strSql.Append(" and DefectCatgNm like '%" + DefectCatgNm + "%' ");
                //}
                //else { }

                List<DbParameter> parameter = new List<DbParameter>();
                //是否加缺陷类型编号模糊搜索
                if (!String.IsNullOrEmpty(DefectCatgCd))
                {
                    strSql.Append(" and DefectCatgCd like @DefectCatgCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@DefectCatgCd", "%" + DefectCatgCd + "%"));
                }
                //是否加缺陷类型名称模糊搜索
                if (!String.IsNullOrEmpty(DefectCatgNm))
                {
                    strSql.Append(" and DefectCatgNm like @DefectCatgNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@DefectCatgNm", "%" + DefectCatgNm + "%"));
                }


                #endregion

                //按照缺陷类型名称排序
                strSql.Append(" order by DefectCatgNm ");

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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "缺陷类型信息查询成功");
                return Content(JsonData.ToJson());
                #endregion
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "缺陷类型信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 4.重构导出
        public ActionResult GetExcel_Data(string DefectCatgCd, string DefectCatgNm, JqGridParam jqgridparam)
        {
            try
            {
                #region 根据当前搜索条件查出数据并导出
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select DefectCatgCd,DefectCatgNm,Dsc,CreTm,CreNm,MdfTm,MdfNm,Rem from BBdbR_DefectCatgBase_Add where Enabled = 1 and Type='TZ'");
                #region 判断输入框内容添加检索条件
                ////是否加缺陷类型编号模糊搜索
                //if (DefectCatgCd != "" && DefectCatgCd != null)
                //{
                //    strSql.Append(" and DefectCatgCd like '%" + DefectCatgCd + "%' ");
                //}
                //else { }
                ////是否加缺陷类型名称模糊搜索
                //if (DefectCatgNm != "" && DefectCatgNm != null)
                //{
                //    strSql.Append(" and DefectCatgNm like '%" + DefectCatgNm + "%' ");
                //}
                //else { }
                List<DbParameter> parameter = new List<DbParameter>();
                //是否加缺陷类型编号模糊搜索
                if (!String.IsNullOrEmpty(DefectCatgCd))
                {
                    strSql.Append(" and DefectCatgCd like @DefectCatgCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@DefectCatgCd", "%" + DefectCatgCd + "%"));
                }
                //是否加缺陷类型名称模糊搜索
                if (!String.IsNullOrEmpty(DefectCatgNm))
                {
                    strSql.Append(" and DefectCatgNm like @DefectCatgNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@DefectCatgNm", "%" + DefectCatgNm + "%"));
                }
                #endregion

                //按照缺陷类型名称排序
                strSql.Append(" order by DefectCatgNm ");

                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                #endregion

                string fileName = "缺陷类型";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_DefectCatgBase(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "缺陷类型信息导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "缺陷类型信息导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        #endregion
    
}
}