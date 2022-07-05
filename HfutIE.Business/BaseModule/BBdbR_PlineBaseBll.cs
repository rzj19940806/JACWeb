//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

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
    /// 产线基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.23 15:33</date>
    /// </author>
    /// </summary>
    public class BBdbR_PlineBaseBll : RepositoryFactory<BBdbR_PlineBase>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_PlineBase";//===复制时需要修改===
        #endregion

        #region 1.获取树，需要修改sql语句
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===复制时需要修改===
            strSql.Append(@"select  FacId AS keys,     
                        FacCd AS code,
                        FacNm AS name,
                        Enabled As IsAvailable,
                        '0' as parentId,  
                        '0' as sort    
                        from BBdbR_FacBase where Enabled = '1' ");
            strSql.Append(@" union select    
                                    a.WorkshopId AS keys,
                                    a.WorkshopCd AS code,
                                    a.WorkshopNm AS name,
                                    a.Enabled As IsAvailable,
                                    a.FacId AS parentId,
                                    '1' as sort 
                             from  BBdbR_WorkshopBase a,BBdbR_FacBase b 
                             where a.FacId=b.FacId and a.Enabled = '1' and b.Enabled = '1' ");
            strSql.Append(@" union select    
                                   c.WorkSectionId AS keys,
                                   c.WorkSectionCd AS code,
                                   c.WorkSectionNm AS name,
                                   c.Enabled As IsAvailable,
                                   c.WorkshopId AS parentId,
                                   '2' as sort 
                             from  BBdbR_WorkSectionBase c,BBdbR_WorkshopBase d 
                             where c.WorkshopId=d.WorkshopId and c.Enabled = '1' and d.Enabled = '1' order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.点击树展示表格，需要修改sql语句
        /// <summary>
        /// 基本信息：【车间】   --属于-->   【工厂】  --属于-->  【公司】
        ///     根据传入的参数不同写不同的sql语句进行查询           
        /// </summary>
        /// <param name="areaId">点击的节点的主键</param>
        /// <param name="parentId">节点的父级主键</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable GetList(string areaId, string parentId,string sort, ref JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            DataTable dt = new DataTable();
            //if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId)&& string.IsNullOrEmpty(sort))
            //{
            //    sql = "select PlineId,PlineNm from "+tableName+" where Enabled=1";
            //}
            //else
            if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId))
            {
                sql = "select a.*,b.WorkSectionCd as WorkSectionCd,b.WorkSectionNm as WorkSectionNm from " + tableName + " a join BBdbR_WorkSectionBase b on a.WorkSectionId=b.WorkSectionId where a.Enabled=1 and b.Enabled=1 order by a.sort asc";     //===复制时需要修改===
            }
            else
            {
                if (parentId != "0")
                {
                    if (sort=="1")
                    {
                        sql = "select a.*,b.WorkSectionCd as WorkSectionCd,b.WorkSectionNm as WorkSectionNm from " + tableName + " a join BBdbR_WorkSectionBase b on a.WorkSectionId=b.WorkSectionId join BBdbR_WorkshopBase c on b.WorkshopId=c.WorkshopId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and c.WorkshopId='"+ areaId+ "' order by a.sort asc";     //===复制时需要修改===
                    }
                    else
                    {
                        sql = "select a.*,b.WorkSectionCd as WorkSectionCd,b.WorkSectionNm as WorkSectionNm from " + tableName + " a join BBdbR_WorkSectionBase b on a.WorkSectionId=b.WorkSectionId where a.Enabled=1 and b.Enabled=1 and a.WorkSectionId='"+ areaId+ "' order by a.sort asc";     //===复制时需要修改===
                    }
                }
                else
                {
                    sql = "select a.*,b.WorkSectionCd as WorkSectionCd,b.WorkSectionNm as WorkSectionNm from " + tableName + " a join BBdbR_WorkSectionBase b on a.WorkSectionId=b.WorkSectionId join BBdbR_WorkshopBase c on b.WorkshopId=c.WorkshopId join BBdbR_FacBase d on c.FacId=d.FacId where a.Enabled=1 and b.Enabled=1 and c.Enabled=1 and d.Enabled=1 and d.FacId='" + areaId + "' order by a.sort asc";     //===复制时需要修改===
                }
            }
            return Repository().FindTableBySql(sql.ToString(), false);           
        }
        #endregion

        #region 3.编辑方法
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(BBdbR_PlineBase entity) //===复制时需要修改===
        {
            return Repository().Update(entity); //将修改后的实体跟新到数据库中
        }
        #endregion

        #region 4.检查某个字段的字段值是否存在
        /// <summary>
        ///   Enabled = 1的数据中某个字段（KeyName）的字段值（KeyValue）是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回的是搜索的表中包含该字段值的记录条数</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {           
            string sql = @"select * from " + tableName + " where Enabled = '1' and  "+ KeyName + "  = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion

        #region 5.新增方法
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
        public int Insert(BBdbR_PlineBase entity) //===复制时需要修改===
        {
            return Repository().Insert(entity);
        }
        #endregion

        #region 6.删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string[] array)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<BBdbR_PlineBase> listEntity = new List<BBdbR_PlineBase>(); //===复制时需要修改===
            foreach (string keyValue in array)
            {
                //===复制时需要修改===
                BBdbR_PlineBase entity = Repository().FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                entity.Enabled = "0";//将该实体的IsAvailable属性改为false
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion

        #region 7.查询方法，需要修改sql语句
        /// <summary>
        ///     查询时提供了两个关键字，一个是Condition，另一个是keywords
        ///     
        ///     Condition是关键字，即查询条件，对应数据库中的一个字段
        ///     keywords是查询值，即查询条件的具体值，对应数据库中查询条件字段的值
        ///     查询的时候传递了Condition和keywords
        /// 
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="Condition">关键字（查询条件）</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns>查询的数据（列表）</returns>
        public DataTable GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            DataTable dt = new DataTable();
            if (Condition == "all")
            {
                sql = "select a.*,b.WorkSectionCd as WorkSectionCd,b.WorkSectionNm as WorkSectionNm from " + tableName + " a join BBdbR_WorkSectionBase b on a.WorkSectionId=b.WorkSectionId where a.Enabled=1 and b.Enabled=1 order by a.sort asc";     //===复制时需要修改===
            }
            else
            {
                if (keywords != "all")
                {
                    sql = "select a.*,b.WorkSectionCd as WorkSectionCd,b.WorkSectionNm as WorkSectionNm from " + tableName + " a join BBdbR_WorkSectionBase b on a.WorkSectionId=b.WorkSectionId where a.Enabled=1 and b.Enabled=1 and " + Condition + " like  '%" + keywords + "%' order by a.sort asc";     //===复制时需要修改===
                }
                else
                {
                    sql = "select a.*,b.WorkSectionCd as WorkSectionCd,b.WorkSectionNm as WorkSectionNm from " + tableName + " a join BBdbR_WorkSectionBase b on a.WorkSectionId=b.WorkSectionId where a.Enabled=1 and b.Enabled=1 order by a.sort asc";     //===复制时需要修改===
                }
            }
            return Repository().FindTableBySql(sql.ToString(), false);
        }
        #endregion

        #region 8.编辑页面
        /// <summary>
        /// 编辑弹框中数据源
        /// </summary>
        /// <returns></returns>
        public BBdbR_PlineBase GetPlineList(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_PlineBase where  PlineId='" + KeyValue + "' order by sort asc");
            List<BBdbR_PlineBase> dt = Repository().FindListBySql(strSql.ToString());
            BBdbR_PlineBase Dvcentity = new BBdbR_PlineBase();
            Dvcentity = dt[0];
            return Dvcentity;
        }
        #endregion

        #region 9.查找工艺段下产线数
        /// <summary>
        /// 查找车间下工艺段数量
        /// </summary>
        /// <returns></returns>
        public int GetPlineCount(string KeyValue) //===复制时需要修改===
        {
            string sql = "";
            if (KeyValue != "")
            {
                sql = @"select * from BBdbR_PlineBase where Enabled='1' and WorkSectionId='" + KeyValue + "' order by sort asc";
                DataTable dt = Repository().FindTableBySql(sql);
                if (dt.Rows.Count > 0)
                {
                    return Repository().FindTableBySql(sql).Rows.Count;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 1;
            }
        }
        #endregion

        #region 10.班组产线配置
        /// <summary>
        /// 查询班次基本信息表里存在，但未存在配置的班次列表
        /// 查询的时候传递了keywords
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public List<BBdbR_PlineBase> GetReStaffList(string keywords, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select * from " + tableName + " where Enabled=1 and PlineId not in (select Distinct(PlineId) from BFacR_TeamOrg where Enabled=1 and TeamId='" + keywords + "') order by sort asc";
                return Repository().FindListPageBySql(sql.ToString(), ref jqgridparam);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询班次基本信息表里存在，也存在配置的班次列表
        /// 查询的时候传递了keywords
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public List<BBdbR_PlineBase> GetStaffList(string keywords, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select a.* from " + tableName + " a join BFacR_TeamOrg b on a.PlineId=b.PlineId where a.Enabled=1 and b.Enabled=1 and b.TeamId='" + keywords + "' order by a.sort asc";
                return Repository().FindListPageBySql(sql.ToString(), ref jqgridparam);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 11.弹框负责人员
        //11.获取所有人员
        public DataTable GetPlineNm()
        {
            string sql = @"select StfId as id, stfnm from BBdbR_StfBase where Enabled='1' and StfPosn like '%产线%'";
            return Repository().FindTableBySql(sql);
        }
        //获取人员信息
        public DataTable GetPlineNm2(string StfId)
        {
            string sql = @"select stfnm,phn,email from BBdbR_StfBase where Enabled='1' and StfId=" + "'" + StfId + "'";
            return Repository().FindTableBySql(sql);
        }
        #endregion

        #region 12.导入模板
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