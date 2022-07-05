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
    /// 工位物料配置
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.06 20:03</date>
    /// </author>
    /// </summary>
    public class BBdbR_ProductWcConfigBll : RepositoryFactory<BBdbR_ProductWcConfig>
    {
        #region 1.获取树，需要修改sql语句
        public DataTable GetTree(string productMatId)
        {
            StringBuilder strSql = new StringBuilder();
            //===复制时需要修改===
            strSql.Append($@"select PlineId as keys,PlineCd as code,PlineNm as name,'0'as parentId,'0'as sort from BBdbR_PlineBase A with(nolock) where Enabled=1 and exists(select 1 from BBdbR_WcBase with(nolock) where PlineId=A.PlineId) and PlineCd like 'Line-%' union select WcId as keys, WcCd as code,WcNm as name,B.PlineId as parentId,'1' as sort from BBdbR_WcBase A with(nolock),BBdbR_PlineBase B with(nolock) where B.PlineId=A.PlineId and A.Enabled=1 and A.WcId in (select ISNULL(C.WcId,D.WcId) from BBdbR_ProductWcMatConfig C with(nolock) join BBdbR_MatBase D with(nolock) on C.MatId=D.MatId and C.ProductId='{productMatId}') order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        public DataTable GridPageJson(JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
                strSql.Append($"select MatId,MatCd,MatNm,CarType,CarColor1 from BBdbR_ProductBase with(nolock) where Enabled=1 order by {jqgridparam.sidx} {jqgridparam.sord}");
            return Repository().FindTableBySql(strSql.ToString(),false);
        }
        public DataTable GridPageJson1(string VIN, string ProductCd, string ProductNm,JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append($"select MatId,MatCd,MatNm,CarType,CarColor1 from BBdbR_ProductBase with(nolock) where Enabled = 1 and(MatCd in (select distinct MatCd from P_ProducePlan_Pro where @VIN1 <> '' and Enabled = 1 and VIN like @VIN2) or @VIN1 = '') and MatCd like @ProductCd and MatNm like @ProductNm order by {jqgridparam.sidx} {jqgridparam.sord}");

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@VIN1", VIN ));
            parameter.Add(DbFactory.CreateDbParameter("@VIN2", "%" + VIN + "%"));
            parameter.Add(DbFactory.CreateDbParameter("@ProductCd", "%" + ProductCd + "%"));
            parameter.Add(DbFactory.CreateDbParameter("@ProductNm", "%" + ProductNm + "%"));

            return DataFactory.Database().FindTableBySql(strSql.ToString(), parameter.ToArray(), false);
        }
        #region 2.点击树展示表格，需要修改sql语句
        /// <summary>
        ///     根据传入的参数不同写不同的sql语句进行查询
        ///             
        /// </summary>
        /// <param name="areaId">点击的节点的主键</param>
        /// <param name="sort">点击的节点的层级编号</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public DataTable GetList(string areaId, string text, string value, string sort, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            if (sort != "0")
            {
                strSql.Append(@"select ProductClassId,MatId,MatCd,MatNm,FileNm,FileCd,LockStationNum,Rem from BBdbR_ProductWcConfig with(nolock) where  WcId='" + areaId + "' and Enabled=1");
            }
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 3.加载未配置产品
        public DataTable GetNotConfigProduct(ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select *from BBdbR_MatBase where MatId not in(select distinct MatId  from BBdbR_ProductWcConfig where Enabled=1 ) and MatCatg='产品' and Enabled=1");
            return Repository().FindTableBySql(strSql.ToString(), false);
        }
        #endregion

        #region 3.搜索展示未配置产品
        public DataTable GetNotConfigProductbycondition(string keywords, string Condition, ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            if (Condition == "all")
            {
                strSql.Append(@"select *from BBdbR_MatBase where MatId not in(select distinct MatId  from BBdbR_ProductWcConfig where Enabled=1 ) and MatCatg='产品' and Enabled=1");
            }
            else
            {
                strSql.Append(@"select *from BBdbR_MatBase where MatId not in(select distinct MatId  from BBdbR_ProductWcConfig where Enabled=1 ) and MatCatg='产品' and " + Condition + " like '%" + keywords + "%' and Enabled=1");
            }
            return Repository().FindTableBySql(strSql.ToString(), false);
        }
        #endregion

        #region 4.加载已配置产品的表（没用到）
        public DataTable GetProductList(string array)
        {
            string[] str1 = array.Split(',');
            string str2 = "";
            for (int i = 0; i < str1.Length - 1; i++)
            {
                if (i == str1.Length - 2)
                {
                    str2 += "'" + str1[i] + "'";
                }
                else
                {
                    str2 += "'" + str1[i] + "'" + ",";
                }
            }
            str2 = "(" + str2 + ")";
            string str = $"select  *from BBdbR_MatBase where MatId in{str2}";
            return Repository().FindTableBySql(str, false);
        }
        #endregion

        #region 5.提交新增产品表单
        #region 5.1获取选中产线下的所有工位
        public DataTable GetWcdt(string PlineId)
        {
            string sql = $"select WcId,WcCd,WcNm from BBdbR_WcBase where PlineId='{PlineId}' and Enabled=1";
            return Repository().FindTableBySql(sql, false);
        }
        #endregion

        #region 5.2插入到产品工位配置表中
        public int Insert(BBdbR_ProductWcConfig entity)
        {
            Repository().Insert(entity);
            return 1;
        }
        #endregion
        #region

        #endregion
        #endregion

        #region 6.编辑图片表单
        #region 6.1图片编号下拉框
        public DataTable GetFaileCd(string matid)
        {
            string sql = "select * from BBdbR_MatFileConfig where FileTy='0' and Enabled='1' and MatId='" + matid + "'";
            return Repository().FindTableBySql(sql, false);
        }
        #endregion
        #region 6.2根据编号加载
        public DataTable GetFaileData(string filecd, string matid)
        {
            string sql = "select * from BBdbR_MatFileConfig where FileTy='0' and Enabled='1' and FileCd='" + filecd + "' and MatId='" + matid + "'";
            return Repository().FindTableBySql(sql, false);
        }
        #endregion
        #region 6.3将新图片信息插入数据库
        public int SubmitPicture(string newfilecd, string newfilenm, string newconfigid, string productclassid)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append(@"update BBdbR_ProductWcConfig set FileNm='" + newfilenm + "',FileCd='" + newfilecd + "',ConfigId='" + newconfigid + "' where ProductClassId='" + productclassid + "'");
            //string sql = $"updata BBdbR_ProductWcConfig set FileNm='{newfilenm}',FileCd='{newfilecd}',ConfigId='{newconfigid}' where ProductClassId='{productclassid}'";
            int isok = Repository().ExecuteBySql(strsql);
            return isok;
        }
        #endregion
        #endregion

        //7.物料配置见BBdbR_ProductWcMatConfigBll(MyBll2)

        #region 8.删除配置
        public int ConfigMatDelete(string MatKeyValue)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"update BBdbR_ProductWcMatConfig set Enabled=0 where ProductClassMatId='" + MatKeyValue + "'");
            int isok = Repository().ExecuteBySql(sql);
            return isok;
        }

        #endregion

        #region 9.打印
        public DataTable Getprintdata1(string productMatId, string PlineCd)//得到选中的产品所在的产线
        {
            string sql;
            if (PlineCd == "Line-12" || PlineCd == "Line-23")//底盘A、发动机
            {
                string PlineCd2;
                if (PlineCd == "Line-12") PlineCd2 = "Line-13";
                else PlineCd2 = "Line-21";

                sql = $"select b.MatCd,b.MatNm,b.MatNum,b.RsvFld1 WcCd from produceBom b join BBdbR_WcBase w on b.RsvFld1=w.WcCd join BBdbR_PlineBase p on w.PlineId=p.PlineId where ProductId = '{productMatId}' and(p.PlineCd = '{PlineCd}' or p.PlineCd = '{PlineCd2}') and IsPrint = 1 order by b.RsvFld1";
            }
            else if (PlineCd == "Line-22")//后桥
            {
                sql = $"select b.MatCd,b.MatNm,b.MatNum,b.RsvFld1 WcCd from produceBom b join BBdbR_WcBase w on b.RsvFld1 = w.WcCd join BBdbR_PlineBase p on w.PlineId = p.PlineId where ProductId = '{productMatId}' and(p.PlineCd = '{PlineCd}'  or b.RsvFld1 = 'C-14') and IsPrint = 1 order by b.RsvFld1";
            }
            else if(PlineCd == "Line-10" || PlineCd == "Line-11" || PlineCd == "Line-14" || PlineCd == "Line-20")//内饰AB、合装、车门
            {
                sql = $"select b.MatCd,b.MatNm,b.MatNum,b.RsvFld1 WcCd from produceBom b join BBdbR_WcBase w on b.RsvFld1=w.WcCd join BBdbR_PlineBase p on w.PlineId=p.PlineId where ProductId='{productMatId}' and  p.PlineCd = '{PlineCd}' and IsPrint=1 order by b.RsvFld1";
            }
            else return null;

            //sql = $"select PlineTyp,PlineNm,MatCd,MatNm from produceBom A with(nolock) join BBdbR_WcBase B with(nolock) on A.RsvFld1=B.WcCd and IsPrint=1 and ProductId='{productMatId}' join BBdbR_PlineBase C with(nolock) on B.PlineId = C.PlineId and C.PlineCd = '{PlineCd}' order by B.WcCd";
            return Repository().FindTableBySql(sql, false);
        }

        public DataTable Getprintdata2(string productMatId, string PlineCd)//得到选中的产品所在的产线
        {

            string NoNumSql = "";
            if (PlineCd == "Line-23")//发动机
            {
                NoNumSql = $"select b.MatCd,b.MatNm,b.MatNum,b.WcCd from produceBom b join BBdbR_WcBase w on b.WcId=w.WcId join BBdbR_PlineBase p on w.PlineId=p.PlineId where ProductId='{productMatId}' and p.PlineCd in ('{PlineCd}','Line-21') and IsScan=1 and MatNum>0 order by b.WcCd";
            }
            else if (PlineCd == "Line-22")//后桥
            {
                NoNumSql = $"select b.MatCd,b.MatNm,b.MatNum,b.WcCd from produceBom b join BBdbR_WcBase w on b.WcId=w.WcId join BBdbR_PlineBase p on w.PlineId=p.PlineId where ProductId='{productMatId}' and  (p.PlineCd = '{PlineCd}' or b.WcId='53') and IsScan=1 and MatNum>0 order by b.WcCd";
            }
            else if (PlineCd == "Line-20")//车门
            {
                NoNumSql = $"select b.MatCd,b.MatNm,b.MatNum,b.WcCd from produceBom b join BBdbR_WcBase w on b.WcId=w.WcId join BBdbR_PlineBase p on w.PlineId=p.PlineId where ProductId='{productMatId}' and p.PlineCd = '{PlineCd}' and IsScan=1 and MatNum>0 order by b.WcCd";
            }
            else return null;
            DataTable NoNumscanData = Repository().FindTableBySql(NoNumSql, false);//初始扫码单
            DataTable scanData = NoNumscanData.Clone();//数量校准扫码单
            for (int i = 0; i < NoNumscanData.Rows.Count; i++)
            {
                if (int.Parse((NoNumscanData.Rows[i]["MatNum"].ToString())) > 0)
                {
                    int num = int.Parse((NoNumscanData.Rows[i]["MatNum"].ToString()));
                    NoNumscanData.Rows[i]["MatNum"] = 0;
                    for (int j = 0; j < num; j++)
                    {
                        scanData.ImportRow(NoNumscanData.Rows[i]);
                    }
                }
            }
            return scanData;
        }
        public DataTable GetPlineData(string plinecd)
        {
            return Repository().FindTableBySql($"select top 1 * from BBdbR_PlineBase where Enabled='1' and PlineCd='{plinecd}'",false);
        }

        #endregion
    }
}