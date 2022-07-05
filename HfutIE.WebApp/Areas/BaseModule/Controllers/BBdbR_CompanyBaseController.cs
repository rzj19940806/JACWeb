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

namespace HfutIE.WebApp.Areas.baseModule.Controllers
{
    /// <summary>
    /// 公司基础信息表控制器
    /// </summary>
    public class BBdbR_CompanyBaseController : PublicController<BBdbR_CompanyBase>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_CompanyBase";//===复制时需要修改===
        public readonly RepositoryFactory<BBdbR_CompanyBase> repository_BBdbR_CompanyBase = new RepositoryFactory<BBdbR_CompanyBase>();
        #endregion

        #region 创建数据库操作对象区域
        BBdbR_CompanyBaseBll MyBll = new BBdbR_CompanyBaseBll(); //===复制时需要修改===
        #endregion

        #region 打开视图区域
        public ActionResult Select()//打开选择公司子界面页面
        {
            return View();
        }
        #endregion

        #region 方法区   

        #region 1.新增编辑方法
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
        public override ActionResult SubmitForm(BBdbR_CompanyBase entity, string KeyValue)//===复制时需要修改===
        {
            try
            {
                int IsOk = 0;//用于判断
                string Name = "CompanyCd";        //页面中的编号字段名，如：公司编号   //===复制时需要修改===
                string Value = entity.CompanyCd;  //页面中的编号字段值                 //===复制时需要修改===
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";

                if (!string.IsNullOrEmpty(KeyValue))//编辑操作
                {
                    //===复制时需要修改===
                    BBdbR_CompanyBase BBdbR_CompanyBaseentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.Modify(KeyValue);
                    IsOk = MyBll.Update(entity);//将修改后的实体更新到数据库，插入成功返回1，失败返回0；
                    this.WriteLog(IsOk, entity, BBdbR_CompanyBaseentity, KeyValue, Message);//记录日志
                }
                else//新增操作
                {
                    IsOk = MyBll.CheckCount(Name,Value);//判断页面中填写的数据的编号字段的值是否存在
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

        #region 2.删除方法
        /// <summary>
        /// <param name="KeyValue">页面中提供的需要删除的数据的主键,可能是多条数据的主键，即多个主键</param>
        /// <param name="ParentId">不需要</param>
        /// <param name="DeleteMark">不需要</param>
        /// <returns></returns>
        public ActionResult DeleteRule(string KeyValue, string ParentId, string DeleteMark)
        {
            //不管是多个主键还是单个主键，将主键拆分出来，放在数组中
            string[] array = KeyValue.Split(',');
            BBdbR_FacBaseBll FacBll = new BBdbR_FacBaseBll();
            try
            {
                var Message = "删除失败。";//定义返回信息，该信息将返回到界面上，给用户观看
                int IsOk = 0;//判断删除方法是否成，0表示不成功，大于0表示成功                
                int a = FacBll.GetFacCount(KeyValue);//查找工厂基础信息中是否有在本公司名下，如果有就不可以删除
                if (a>0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "公司下设置工厂，不可删除" }.ToString());
                }
                else
                {
                    //假删除
                    IsOk = MyBll.Delete(array);//执行删除操作
                    if (IsOk > 0)//表示删除成
                    {
                        Message = "删除成功。";//修改返回信息
                    }
                    WriteLog(IsOk, array, Message);
                    return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
                }            
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 3.查询方法
        //查询方法，本方法为单条件查询，即根据一个条件进行查询
        //查询条件为Condition，也是数据库表_CompanyBaseInformation中的一个字段名
        //查询值为keywords，也是数据库表_CompanyBaseInformation中的字段名的字段值
        //本查询采用近似查询（like）

        public ActionResult GridPageByCondition(string CompanyCd, string CompanyNm, JqGridParam jqgridparam)
        {
            try
            {
                CompanyCd = CompanyCd.Trim();
                CompanyNm = CompanyNm.Trim();
                Stopwatch watch = CommonHelper.TimerStart();

                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                //未加搜索条件
                strSql.Append(@"select * from BBdbR_CompanyBase where Enabled = '1' ");

                //公司编号模糊查询
                if (CompanyCd != "" && CompanyCd!=null)
                {
                    strSql.Append(" and CompanyCd like @CompanyCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@CompanyCd", "%" + CompanyCd + "%"));
                }
                else { }

                //公司名称模糊查询
                if (CompanyNm != "" && CompanyNm != null)
                {
                    strSql.Append(" and CompanyNm like @CompanyNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@CompanyNm", "%" + CompanyNm + "%"));
                }
                else { }

                //排序
                strSql.Append(" order by sort ");

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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "1", "公司基础信息查询成功");
                return Content(JsonData.ToJson());
                
            }
            catch (Exception ex)
            {
                //CCSLog.CCSLogHelper.WriteExLog(ex, CCSLog.LogType.WebSite);
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "公司基础信息查询发生异常错误：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 4.检查字段是否唯一
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
                    IsOk = MyBll.CheckCount(tableName, KeyName);
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

        #region 5.重构导出
        public ActionResult GetExcel_Data(string CompanyCd, string CompanyNm)
        {
            try
            {

                #region 根据当前搜索条件查出数据并导出
                CompanyCd = CompanyCd.Trim();
                CompanyNm = CompanyNm.Trim();
                StringBuilder strSql = new StringBuilder();
                List<DbParameter> parameter = new List<DbParameter>();
                //未加搜索条件
                strSql.Append(@"select CompanyCd,CompanyNm,ArtificialPerson,CompanyTelephone,CompanyFax,CompanyEmail,CompanyAddress,CompanyDescription,sort,CreCd,CreNm,CreTm,MdfCd,MdfNm,MdfTm,Rem from BBdbR_CompanyBase where Enabled = '1' ");
                //公司编号模糊查询
                if (CompanyCd != "" && CompanyCd != null)
                {
                    strSql.Append(" and CompanyCd like @CompanyCd ");
                    parameter.Add(DbFactory.CreateDbParameter("@CompanyCd", "%" + CompanyCd + "%"));
                }
                else { }
                //公司名称模糊查询
                if (CompanyNm != "" && CompanyNm != null)
                {
                    strSql.Append(" and CompanyNm like @CompanyNm ");
                    parameter.Add(DbFactory.CreateDbParameter("@CompanyNm", "%" + CompanyNm + "%"));
                }
                else { }
                //排序
                strSql.Append(" order by sort ");

                //DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), false);
                DataTable dt = DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
                #endregion



                string fileName = "公司管理";
                string excelType = "xls";
                MemoryStream ms = DeriveExcel.ExportExcel_CompanyBase(dt, excelType);
                if (!fileName.EndsWith(".xls"))
                {
                    fileName = fileName + ".xls";
                }
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "1", "公司基础信息导出成功");
                return File(ms, "application/vnd.ms-excel", Url.Encode(fileName));
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId")), OperationType.Other, "-1", "公司基础信息导出操作失败：" + ex.Message);
                return null;
            }

        }
        #endregion

        #endregion
    }
}