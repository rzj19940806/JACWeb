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
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 工厂日历信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.29 20:18</date>
    /// </author>
    /// </summary>
    public class BPdb_DtBll : RepositoryFactory<BPdb_Dt>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BPdb_Dt";//===复制时需要修改===
        #endregion

        #region 1.获取树，需要修改sql语句
        public DataTable GetTree(string year)
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("keys", typeof(string));
            DataColumn dc2 = new DataColumn("code", typeof(string));
            DataColumn dc3 = new DataColumn("name", typeof(string));
            DataColumn dc4 = new DataColumn("IsAvailable", typeof(string));
            DataColumn dc5 = new DataColumn("parentId", typeof(string));
            DataColumn dc6 = new DataColumn("sort", typeof(string));
            dt.Columns.Add(dc1); dt.Columns.Add(dc2); dt.Columns.Add(dc3); dt.Columns.Add(dc4); dt.Columns.Add(dc5); dt.Columns.Add(dc6);
            string sql = "select * from " + tableName + " where Enabled = '1' order by Tm asc";     //===复制时需要修改===
            List<BPdb_Dt> listEntity = Repository().FindListBySql(sql); //执行sql语句
            List<BPdb_Dt> listEntity1 = new List<BPdb_Dt>(); //执行sql语句
            string a = "";
            for (int i = 0; i < listEntity.Count; i++)
            {
                a = listEntity[i].Tm.ToString().Substring(0, 4);
                if (listEntity1.Count == 0)
                {
                    listEntity1.Add(listEntity[i]);
                }
                else
                {
                    int c = 0;
                    for (int j = 0; j < listEntity1.Count; j++)
                    {
                        if (a == listEntity1[j].Tm.ToString().Substring(0, 4))
                        {
                            c = 1;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (c == 0)
                    {
                        listEntity1.Add(listEntity[i]);
                    }
                }
            }
            for (int j = 0; j < listEntity1.Count; j++)
            {
                DataRow dr1 = dt.NewRow();
                dr1["keys"] = listEntity1[j].Tm.ToString().Substring(0, 4) + "年";
                dr1["code"] = listEntity1[j].Tm.ToString().Substring(0, 4) + "年";
                dr1["name"] = listEntity1[j].Tm.ToString().Substring(0, 4) + "年";
                dr1["IsAvailable"] = "1";
                dr1["parentId"] = "0";
                dr1["sort"] = "1";
                dt.Rows.Add(dr1);
                for (int i = 1; i <= 12; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["keys"] = listEntity1[j].Tm.ToString().Substring(0, 4) + "年" + i + "月";//keys如果重复将会报错
                    dr["code"] = i;
                    dr["name"] = i + "月";
                    dr["IsAvailable"] = "1";
                    dr["parentId"] = listEntity1[j].Tm.ToString().Substring(0, 4) + "年";
                    dr["sort"] = "1";
                    dt.Rows.Add(dr);
                }
            }

            return dt;
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
        public List<BPdb_Dt> GetList(string areaId, string parentId, ref JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (parentId != "0")
            {
                string mounth = areaId.Substring(0, areaId.Length - 1);//取出具体的月份
                //从本表中查询上级表主键与传入主键相同相等的数据，并返回列表
                sql = "select * from " + tableName + " where Enabled = 1 and MONTH(Tm) = '" + areaId.Substring(0, areaId.Length - 1) + "' order by Tm asc";
            }
            else
            {
                sql = "select * from " + tableName + " where Enabled = 1 and YEAR(Tm) = '" + areaId.Substring(0, areaId.Length - 1) + "' order by Tm asc";
            }
            return Repository().FindListBySql(sql);
        }
        #endregion

        #region 3.Form页下拉框
        //班制
        public DataTable GetClassNm()
        {
            string sql = @"SELECT ClassId AS id, ClassNm
                           FROM BFacR_ClassBase
                           WHERE Enabled = '1' order by ClassCd";
            return Repository().FindTableBySql(sql);
        }
        //车间
        public DataTable GetWsbNm()
        {
            string sql = @"SELECT WorkshopId AS id, WorkshopNm
                           FROM BBdbR_WorkshopBase
                           WHERE 1 = 1";
            return Repository().FindTableBySql(sql);
        }

        //产线
        public DataTable GetPlineNm()
        {
            string sql = @"SELECT PlineId AS id, PlineNm
                           FROM BBdbR_PlineBase
                           WHERE PlineCd not like '%Quality%' and (PlineTyp != 'PBS线' or PlineCd = 'Line-01') and Enabled = '1'";
            return Repository().FindTableBySql(sql);
        }
        #endregion

        #region 3.页面表格
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>
        public List<BPdb_Dt> GetPlanList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BPdb_Dt where Enabled = '1' order by Tm desc");
            List<BPdb_Dt> dt = Repository().FindListBySql(strSql.ToString());
            //for (int i = 0; i < dt.Count; i++)
            //{
            //    if (dt[i].OrgRank!="")
            //    {
            //        if (dt[i].OrgRank == "1")
            //        {
            //            string sql1 = "select * from BBdbR_FacBase where FacId ='" + dt[i].OrgId + "' ";     //===复制时需要修改===
            //            DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
            //            if (dt1.Rows.Count > 0)
            //            {
            //                dt[i].RsvFld1 = dt1.Rows[0]["FacNm"].ToString();//组织机构名称
            //                dt[i].RsvFld2 = dt[i].Tm.ToString().Substring(0, 10);
            //            }
            //        }
            //        else if (dt[i].OrgRank == "2")
            //        {
            //            string sql1 = "select * from BBdbR_WorkshopBase where WorkshopId ='" + dt[i].OrgId + "' ";     //===复制时需要修改===
            //            DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
            //            if (dt1.Rows.Count > 0)
            //            {
            //                dt[i].RsvFld1 = dt1.Rows[0]["WorkshopNm"].ToString();//组织机构名称
            //                dt[i].RsvFld2 = dt[i].Tm.ToString().Substring(0, 10);
            //            }
            //        }
            //        else
            //        {
            //            string sql1 = "select * from BBdbR_PlineBase where PlineId ='" + dt[i].OrgId + "' ";     //===复制时需要修改===
            //            DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
            //            if (dt1.Rows.Count > 0)
            //            {
            //                dt[i].RsvFld1 = dt1.Rows[0]["PlineNm"].ToString();//组织机构名称
            //                dt[i].RsvFld2 = dt[i].Tm.ToString().Substring(0, 10);
            //            }
            //        }
            //        //string sql1 = "select * from BFacR_TeamOrg where OrgId ='" + dt[i].OrgId + "'and Enabled = '1' ";     //===复制时需要修改===
            //        //DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
            //        //if (dt1.Rows.Count > 0)
            //        //{
            //        //    dt[i].RsvFld1 = dt1.Rows[0]["OrgNm"].ToString();//组织机构名称
            //        //    dt[i].RsvFld2 = dt[i].Tm.ToString().Substring(0, 10);
            //        //}
            //        string sql2 = "select * from BFacR_ClassBase where ClassId ='" + dt[i].ClassId + "' ";     //===复制时需要修改===
            //        DataTable dt2 = Repository().FindTableBySql(sql2.ToString());
            //        if (dt2.Rows.Count > 0)
            //        {
            //            dt[i].RsvFld3 = dt2.Rows[0]["ClassNm"].ToString();//班制名称
            //            dt[i].RsvFld4 = dt2.Rows[0]["ClassTyp"].ToString();//班制类别
            //        }
            //    }
               
            //}
            return dt;
        }
        #endregion

        #region 3.展示表格
        /// <summary>
        /// 搜索表格中所有IsAvailable = true的数据
        /// </summary>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns>返回搜索到的数据</returns>
        public List<BPdb_Dt> GetPageList(JqGridParam jqgridparam) //===复制时需要修改===
        {
            return Repository().FindList();
        }
        #endregion

        #region 4.编辑方法
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(BPdb_Dt entity) //===复制时需要修改===
        {
            return Repository().Update(entity); //将修改后的实体跟新到数据库中
        }
        #endregion

        #region 5.检查某个字段的字段值是否存在
        /// <summary>
        ///   Enabled = 1的数据中某个字段（KeyName）的字段值（KeyValue）是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回的是搜索的表中包含该字段值的记录条数</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where '" + KeyName + "' = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion

        #region 6.新增方法
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
        public int Insert(BPdb_Dt entity) //===复制时需要修改===
        {      
            return Repository().Insert(entity);
        }
        #endregion

        #region 7.删除方法
        public int DeleteUseEnabled(string[] array)
        {            
            List<BPdb_Dt> listEntity = new List<BPdb_Dt>(); 
            foreach (string id in array)
            {
                BPdb_Dt entity = Repository().FindEntity(id);
                entity.Enabled = "0";
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);
        }
        #endregion

        #region 8.查询方法，需要修改sql语句
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
        public List<BPdb_Dt> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            List<BPdb_Dt> dt;
            if (Condition == "all")
            {
                sql = @"select * from " + tableName + " where Enabled = 1 order by Tm asc";
                dt = Repository().FindListBySql(sql.ToString());
            }
            else
            {
                if (keywords!="")
                {
                    //根据条件查询
                    sql = @"select * from " + tableName + " where  " + Condition + " like  '%" + keywords + "%' and Enabled = 1 order by Tm asc";
                    dt = Repository().FindListBySql(sql.ToString());
              
                }
                else
                {
                    sql = @"select * from " + tableName + " where Enabled = 1 order by Tm asc";
                    dt = Repository().FindListBySql(sql.ToString());
                }
            }
            return dt;
        }
        public List<BPdb_Dt> GetDtList(string keywords, string condition)
        {
            List<BPdb_Dt> dt = new List<BPdb_Dt>() ;
            if (keywords !="" && keywords != null)
            {
                string sql = "select * from " + tableName + " where " + keywords + " = '" + condition + "' and Enabled = 1";
                dt = Repository().FindListBySql(sql.ToString());
            }
            return dt;
        }
        #endregion

        #region 9.导出模板
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

        #region 11.导入时的机构ID获取
        public DataTable searchClassID(string ClassCD, string OrgRank)
        {
            try
            {
                string sql = "";
                DataTable Dt = new DataTable();
                if (OrgRank == "1") //1-车间
                {
                    sql = @"select WorkshopId as id from BBdbR_WorkshopBase where WorkshopCd = '" + ClassCD + "' and Enabled = 1";
                    Dt = Repository().FindTableBySql(sql.ToString(), false);
                }
                else if (OrgRank == "0")//2-产线
                {
                    sql = @"select PlineId as id from BBdbR_PlineBase where PlineCd = '" + ClassCD + "' and Enabled = 1";
                }

                return Dt != null ? Dt : null;
            }
            catch (Exception EX)
            {
                return null;
            }
        }
        #endregion

        #region 12.导入时的班制id
        public DataTable serarchId(string ClassCd) //班制编号
        {
            try
            {
                DataTable dataTable = new DataTable();
                string sql = @"select ClassId as id from BFacR_ClassBase where ClassCd = '" + ClassCd + "' and Enabled = '1'";
                dataTable = Repository().FindTableBySql(sql.ToString(), false);
                return dataTable != null ? dataTable : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        public DataTable getTime(string ClassId)
        {
            return Repository().FindTableBySql($"select min(StrtRestTm) StrtTm,max(EndRestTm) EndTm,max(TmSpan) from BFacR_ShiftBase A join BFacR_ClassConfig B on A.RestTm='总时间' and B.ClassId = '{ClassId}' and A.ShiftCd = B.ShiftCd join BFacR_ClassBase C on C.ClassId = B.ClassId ");
        }
    }
}