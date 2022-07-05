//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace HfutIE.Business
{

    /// <summary>
    /// 部门基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.22 16:10</date>
    /// </author>
    /// </summary>
    public class DepartmentBll : RepositoryFactory<BBdbR_DepartmentBase>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_DepartmentBase";
        public readonly RepositoryFactory<BBdbR_DepartmentBase> repositoryFactory = new RepositoryFactory<BBdbR_DepartmentBase>();
        #endregion

        #region 1.获取树，需要修改sql语句
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===复制时需要修改=== CompanyId AS Attribute,
            strSql.Append(@"select    
                              CompanyId AS keys,
                              CompanyCd AS code,
                              CompanyNm AS name,
                              Enabled As IsAvailable,
                              '0' AS parentId,
                              CompanyId AS companyid,
                              '1' as sort 
                              from BBdbR_CompanyBase where Enabled = '1'
							  union
							  select  DepartmentID AS keys,     
                              DepartmentCode AS code,
                              DepartmentName AS name,
                              Enabled As IsAvailable,
                              ParentDepartmentID as parentId,  
                              CompanyId AS companyid,
                              '1' as sort 
                              from BBdbR_DepartmentBase where Enabled = '1'  order by code asc");
                
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.点击树展示表格，需要修改sql语句
        /// <summary>
        /// 基本信息：【部门】   --属于-->   【工厂】 
        ///     根据传入的参数不同写不同的sql语句进行查询           
        /// </summary>
        /// <param name="areaId">点击的节点的主键</param>
        /// <param name="parentId">节点的父级主键</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable GetList(string areaId, string parentId, string Condition, string keywords, ref JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            DataTable dt = new DataTable();
            List<DbParameter> parameter = new List<DbParameter>();
            if (parentId == "0"||String.IsNullOrEmpty(parentId))//点击公司节点
            {
                
                sql = "select a.*,b.CompanyCd as CompanyCd,b.CompanyNm as CompanyNm from " + tableName + " a join BBdbR_CompanyBase b on a.CompanyId=b.CompanyId  where a.Enabled=1 and b.Enabled=1  and ParentDepartmentID = b.CompanyId  ";     //===复制时需要修改===  and a.ParentDepartmentID = '" + areaId + "'
                
                if (Condition == "all"||String.IsNullOrEmpty(Condition))
                {

                }
                else
                {

                    sql += " and  a." + Condition + " like  @keywords "; 
                    parameter.Add(DbFactory.CreateDbParameter("@keywords", "%" + keywords + "%"));
                    //sql += " and  a." + Condition + " like  '%" + keywords + "%' ";
                }
                sql += "  order by DepartmentCode asc";
                dt = Repository().FindTableBySql(sql.ToString(), false);
            }
            else
            {
                //点击某部门
                sql = "select a.*,c.CompanyCd as CompanyCd,c.CompanyNm as CompanyNm from " + tableName + " a join BBdbR_CompanyBase c on a.CompanyId=c.CompanyId  where a.Enabled=1 and c.Enabled=1  and a.ParentDepartmentID='" + areaId + "'";     //===复制时需要修改===
                if (Condition == "all" || String.IsNullOrEmpty(Condition))
                {

                }
                else
                {
                    sql += " and  a." + Condition + " like  @keywords ";
                    parameter.Add(DbFactory.CreateDbParameter("@keywords", "%" + keywords + "%"));
                    //sql += " and  a." + Condition + " like  '%" + keywords + "%' ";
                }
                sql += "  order by DepartmentCode asc";
                //dt = Repository().FindTableBySql(sql.ToString(), false);
                dt = DataFactory.Database().FindTableBySql(sql, parameter.ToArray(), false);
            }
            return dt;           
        }

        #endregion

        #region 3.展示表格
        ///// <summary>
        ///// 搜索表格中所有Enabled = true的数据, 即为有效的工厂信息GetDepartmentBase
        ///// </summary>
        ///// <param name="jqgridparam">分页参数</param>
        ///// <returns>返回搜索到的数据</returns>
        //public List<BBdbR_DepartmentBase> GetPageList(JqGridParam jqgridparam) //===复制时需要修改===
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append(@"SELECT  * FROM  BBdbR_DepartmentBase where 1=1");
        //    List<BBdbR_DepartmentBase> dt = Repository().FindListBySql(strSql.ToString());
        //    for (int i = 0; i < dt.Count; i++)
        //    {
        //        string sql = "select * from BBdbR_FacBase where FacId='" + dt[i].FacId + "'";
        //        DataTable dt1 = Repository().FindTableBySql(sql.ToString());
        //        if (dt1.Rows.Count > 0)
        //        {
        //            dt[i].FacId = dt1.Rows[0]["FacNm"].ToString();
        //        }
        //    }
        //    return dt;
        //}

       /// <summary>
       /// 返回填充编辑界面数据源
       /// </summary>
       /// <param name="KeyValue"></param>
       /// <returns></returns>
        public DataTable GetDepartmentBase(string KeyValue) //===复制时需要修改===
        {
            string sql = "";
            DataTable dt = new DataTable();
            if (KeyValue != "")
            {
                sql = "select a.*,b.CompanyCd as CompanyCd,b.CompanyNm as CompanyNm from " + tableName + " a join BBdbR_CompanyBase b on a.CompanyId=b.CompanyId where a.Enabled=1 and b.Enabled=1 and a.DepartmentID='" + KeyValue+"'";     //===复制时需要修改===
                dt = Repository().FindTableBySql(sql.ToString(), false);
                return dt;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 4.新增编辑方法
        //entity实体中的数据是从页面中传来的，它是用户填写的数据
        //entity实体中只有部分字段有值，因为页面中只提供给部分字段赋值
        //将页面中填写的数据以实体（entity）的方式新增到数据库
        //其中，实体中的IsAvailable字段没有在页面中填写
        //IsAvailable字段的作用是做假删除，即数据库中的某一条数据的IsAvailable字段的字段值为true表示该数据存在
        //字段值为false表示该数据被删除
        //在删除数据库中的某一条数据时只要修改IsAvailable字段的字段值为false，并不删除该条数据
        //在新增时将实体的IsAvailable字段的值修改为true
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(BBdbR_DepartmentBase entity) //===复制时需要修改===
        {
            return Repository().Update(entity); //将修改后的实体跟新到数据库中
        }
        public int CheckCount(string tableName, string KeyName, string KeyValue)
        {
            //string sql = @"select * from " + tableName + " where Enabled = 1 and " + KeyName + " = '" + KeyValue + "'";
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(BBdbR_DepartmentBase entity) //===复制时需要修改===
        {      
            return Repository().Insert(entity);
        }
        #endregion

        #region 5.删除方法
        public int Delete(string[] array)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<BBdbR_DepartmentBase> listEntity = new List<BBdbR_DepartmentBase>(); //===复制时需要修改===
            foreach (string keyValue in array)
            { 
                //===复制时需要修改===
                BBdbR_DepartmentBase entity = Repository().FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                entity.Enabled = 0;          
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion

        #region 6.查询方法，需要修改sql语句
        /// <summary>
        ///     查询时提供了两个关键字，一个是Condition，另一个是keywords 
        ///     Condition是关键字，即查询条件，对应数据库中的一个字段
        ///     keywords是查询值，即查询条件的具体值，对应数据库中查询条件字段的值
        ///     查询的时候传递了Condition和keywords
        /// 
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="Condition">关键字（查询条件）</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable GetPageListByCondition(string areaId, string parentId, string Condition, string keywords, ref JqGridParam jqgridparam) //===复制时需要修改===
        {
            #region 原版本
            //string sql = "";
            //DataTable dt = new DataTable();
            //if (Condition == "all")
            //{
            //    sql = "select a.*,b.CompanyCd as CompanyCd,b.CompanyNm as CompanyNm,c.DepartmentName as ParentDepartmentName,c.DepartmentCode as ParentDepartmentCode from " + tableName + " a join BBdbR_CompanyBase b on a.CompanyId=b.CompanyId left join BBdbR_DepartmentBase c on  a.ParentDepartmentID=c.DepartmentID where a.Enabled=1 and b.Enabled=1";     //===复制时需要修改===
            //}
            //else
            //{
            //    if (keywords == "all")
            //    {
            //        sql = "select a.*,b.CompanyCd as CompanyCd,b.CompanyNm as CompanyNm,c.DepartmentName as ParentDepartmentName,c.DepartmentCode as ParentDepartmentCode from " + tableName + " a join BBdbR_CompanyBase b on a.CompanyId=b.CompanyId left join BBdbR_DepartmentBase c on  a.ParentDepartmentID=c.DepartmentID where a.Enabled=1 and b.Enabled=1";     //===复制时需要修改===
            //    }
            //    else
            //    {
            //        sql = "select a.*,b.CompanyCd as CompanyCd,b.CompanyNm as CompanyNm,c.DepartmentName as ParentDepartmentName,c.DepartmentCode as ParentDepartmentCode from " + tableName + " a join BBdbR_CompanyBase b on a.CompanyId=b.CompanyId left join BBdbR_DepartmentBase c on  a.ParentDepartmentID=c.DepartmentID where a.Enabled=1 and b.Enabled=1 and a." + Condition + " like  '%" + keywords + "%'";      //===复制时需要修改===
            //    }
            //}
            //return Repository().FindTableBySql(sql.ToString(), false);
            #endregion

            #region 修改版本
            string sql = "";
            DataTable dt = new DataTable();
            List<DbParameter> parameter = new List<DbParameter>();
            if (parentId == "0" || String.IsNullOrEmpty(parentId))//点击公司节点
            {

                sql = "select a.*,b.CompanyCd as CompanyCd,b.CompanyNm as CompanyNm from " + tableName + " a join BBdbR_CompanyBase b on a.CompanyId=b.CompanyId  where a.Enabled=1 and b.Enabled=1  and ParentDepartmentID = b.CompanyId  ";     //===复制时需要修改===  and a.ParentDepartmentID = '" + areaId + "'
                if (Condition == "all" || String.IsNullOrEmpty(Condition))
                {

                }
                else
                {
                    sql += " and  a." + Condition + " like  @keywords ";
                    parameter.Add(DbFactory.CreateDbParameter("@keywords", "%" + keywords + "%"));
                    //sql += " and  a." + Condition + " like  '%" + keywords + "%' ";
                }
                sql += "  order by DepartmentCode asc";
                

            }
            else
            {
                //点击某部门
                sql = "select a.*,c.CompanyCd as CompanyCd,c.CompanyNm as CompanyNm from " + tableName + " a join BBdbR_CompanyBase c on a.CompanyId=c.CompanyId  where a.Enabled=1 and c.Enabled=1  and a.ParentDepartmentID='" + areaId + "'";     //===复制时需要修改===
                if (Condition == "all" || String.IsNullOrEmpty(Condition))
                {

                }
                else
                {
                    sql += " and  a." + Condition + " like  @keywords ";
                    parameter.Add(DbFactory.CreateDbParameter("@keywords", "%" + keywords + "%"));
                    //sql += " and  a." + Condition + " like  '%" + keywords + "%' ";
                }
                sql += "  order by DepartmentCode asc";
                
            }
            //dt = Repository().FindTableBySql(sql.ToString(), false);
            dt = DataFactory.Database().FindTableBySql(sql, parameter.ToArray(), false);
            return dt;
            #endregion
        }

        #endregion

        #region 7.删除时使用，查找本表中某一条数据下面是否绑定了其他表格中的数据
        //判断要删除的数据下面是否绑定了其他表中的数据
        //如：公司信息表中一条公司信息下面一般会绑定多条工厂信息
        //    表示这些工厂是属于该公司的。
        //    那么在删除表中一条公司信息之前，就要判断该公司信息下面是否绑定了工厂信息
        //
        //本方法的作用就是判断该公司下面是否有工厂信息
        //Condition表示要查询的表中的字段名，本例中表示工厂基本信息表中的公司主键
        //keyValue表示要查询的表中字段的值。
        //返回值表示查询到了多少条数据
        public int FindChildCount(string tableName, string Condition, string keyValue)
        {
            string sql = "select * from " + tableName + " where " + Condition + " = '" + keyValue + "'";
            int count = Repository().FindCountBySql(sql);
            return count;
        }

        /// 编辑弹框中数据源
        /// </summary>
        /// <returns></returns>
        public BBdbR_DepartmentBase GetPlanList(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_DepartmentBase where DepartmentID='" + KeyValue + "'");
            List<BBdbR_DepartmentBase> dt = Repository().FindListBySql(strSql.ToString());
            BBdbR_DepartmentBase Dvcentity = new BBdbR_DepartmentBase();
            Dvcentity = dt[0];
            return Dvcentity;
        }
        #endregion

        #region 8.弹框负责人员
        /// <summary>
        /// 获取所有人员
        /// </summary>
        /// <returns></returns>
        public DataTable GetPlineNm(string StfId)
        {
            string sql = "";
            if (string.IsNullOrEmpty(StfId))
            {
                 sql = @"select UserId as id,Code,RealName,Telephone,Email from Base_User where Enabled='1' order by RealName desc";
            }
            else
            {
                sql = @"select UserId as id,Code,RealName,Telephone,Email from Base_User where Enabled='1' and UserId=" + "'" + StfId + "' order by RealName desc";
            }
            return Repository().FindTableBySql(sql, false);
        }
        

        /// <summary>
        /// 获取所有父部门
        /// </summary>
        /// <returns></returns>
        public DataTable GetPDepartMent()
        {
            string sql = @"select DepartmentID,DepartmentName from BBdbR_DepartmentBase where Enabled='1'
                          union 
                          select CompanyId as DepartmentID,CompanyNm as DepartmentName from  BBdbR_CompanyBase where Enabled = '1'
";
            return Repository().FindTableBySql(sql, false);
        }
        #endregion

        #region 9.查找父部门下是否有子部门
        /// <summary>
        /// 搜索表格中挂在父部门名下的子部门数量
        /// </summary>
        /// <param name="KeyValue">分页参数</param>
        /// <returns>返回搜索到的数据</returns>
        public int GetParentDepartCount(string KeyValue) //===复制时需要修改===
        {
            string sql = "";
            int count = 0;
            if (KeyValue != "")
            {
                sql = @"select * from " + tableName + " where Enabled='1' and ParentDepartmentID='" + KeyValue + "'";
                DataTable dt = Repository().FindTableBySql(sql);
                
                count = dt.Rows.Count;
            }
            return count;
        }
        #endregion

        #region 9.获取部门下拉框

        public DataTable GetDepartmentList()
        {
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();

            strSql.Append(@"SELECT DepartmentID, b.CompanyId, DepartmentCode,	DepartmentName  ,CompanyNm
                                                			
                                      FROM    BBdbR_DepartmentBase a left join BBdbR_CompanyBase b on a.CompanyId = b.CompanyId  where a.Enabled = '1' and b.Enabled = '1' ");
            return dt = Repository().FindTableBySql(strSql.ToString(), false);
        }

        #endregion

        #region 9.获得导出模板
        public void GetExcellTemperature(string ImportId, out DataTable data, out string DataColumn, out string fileName)
        {
            DataColumn = "";
            data = new DataTable();
            Base_ExcelImport base_excelimport = DataFactory.Database().FindEntity<Base_ExcelImport>(ImportId);
            fileName = base_excelimport.ImportFileName;
            List<Base_ExcelImportDetail> listBase_ExcelImportDetail = DataFactory.Database().FindList<Base_ExcelImportDetail>("ImportId", ImportId);
            object[] rows = new object[listBase_ExcelImportDetail.Count];
            int i = 0;
            foreach (Base_ExcelImportDetail excelImportDetail in listBase_ExcelImportDetail)
            {
                if (DataColumn == "")
                {
                    DataColumn = DataColumn + excelImportDetail.ColumnName;
                }
                else
                {
                    DataColumn = DataColumn + "|" + excelImportDetail.ColumnName;
                }
                switch (excelImportDetail.DataType)
                {
                    //字符串
                    case "0":
                        data.Columns.Add(excelImportDetail.ColumnName, typeof(string));
                        rows[i] = "";
                        break;
                    //数字
                    case "1":
                        data.Columns.Add(excelImportDetail.ColumnName, typeof(decimal));
                        rows[i] = "";
                        break;
                    //日期
                    case "2":
                        data.Columns.Add(excelImportDetail.ColumnName, typeof(DateTime));
                        rows[i] = DateTime.Now;
                        break;
                    //外键
                    case "3":
                        data.Columns.Add(excelImportDetail.ColumnName, typeof(string));
                        rows[i] = "";
                        break;
                    //唯一识别
                    case "4":
                        data.Columns.Add(excelImportDetail.ColumnName, typeof(string));
                        rows[i] = "";
                        break;
                    default:
                        break;
                }
                i++;
            }
            data.Rows.Add(rows);

        }
        #endregion

    }
}




