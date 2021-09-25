﻿//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HfutIE.Business
{
   
    /// <summary>
    /// 工厂基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.22 16:10</date>
    /// </author>
    /// </summary>
    public class BBdbR_FacBaseBll : RepositoryFactory<BBdbR_FacBase>
    {
        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_FacBase";//===复制时需要修改===
        #endregion


        #region 1.展示表格
        /// <summary>
        /// 搜索表格中所有Enabled = true的数据, 即为有效的工厂信息
        /// </summary>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns>返回搜索到的数据</returns>
        public List<BBdbR_FacBase> GetPageList(JqGridParam jqgridparam) //===复制时需要修改===
        {
            return Repository().FindList();
        }
        #endregion
        #region 2.编辑方法
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(BBdbR_FacBase entity) //===复制时需要修改===
        {
            return Repository().Update(entity); //将修改后的实体跟新到数据库中
        }
        #endregion
        #region 3.检查某个字段的字段值是否存在
        /// <summary>
        ///   检查表中IsAvailable = true的数据中某个字段（KeyName）的字段值（KeyValue）是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回的是搜索的表中包含该字段值的记录条数</returns>
        public int CheckCount(string tableName, string KeyName, string KeyValue)
        {
            //string sql = @"select * from " + tableName + " where Enabled = 1 and " + KeyName + " = '" + KeyValue + "'";
            string sql = @"select * from " + tableName + " where  " + KeyName + " = '" + KeyValue + "' and Enabled = '1'";
            DataTable  count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        public int CheckCount(string KeyName, string KeyValue)
        {
            //string sql = @"select * from " + tableName + " where Enabled = 1 and " + KeyName + " = '" + KeyValue + "'";
            string sql = @"select * from " + tableName + " where  " + KeyName + " = '" + KeyValue + "'";
            DataTable count = Repository().FindTableBySql(sql);
            int a = count.Rows.Count;
            return a;
        }
        #endregion
        #region 4.新增方法
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
        public int Insert(BBdbR_FacBase entity) //===复制时需要修改===
        {

            return Repository().Insert(entity);
        }
        #endregion
        #region 5.查询方法，需要修改sql语句
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
        /// <returns></returns>
        public List<BBdbR_FacBase> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (Condition == "all")
            {
                sql = @"select * from " + tableName + " where Enabled = '1'";
                return Repository().FindListBySql(sql);
            }
            else
            {
                if (keywords!="all")
                {
                    //根据条件查询
                    sql = @"select * from " + tableName + " where  " + Condition + " like  '%" + keywords + "%' and Enabled = '1'";
                    return Repository().FindListPageBySql(sql.ToString(), ref jqgridparam);
                }
                else
                {
                    sql = @"select * from " + tableName + " where Enabled = '1'";
                    return Repository().FindListPageBySql(sql.ToString(), ref jqgridparam);
                }
            }
        }
        #endregion
        #region 9.点击树展示表格，需要修改sql语句
        /// <summary>
        /// 基本信息：【车间】   --属于-->   【工厂】  --属于-->  【公司】
        ///     根据传入的参数不同写不同的sql语句进行查询           
        /// </summary>
        /// <param name="areaId">点击的节点的主键</param>
        /// <param name="parentId">节点的父级主键</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public List<BBdbR_FacBase> GetList(string areaId, string parentId, ref JqGridParam jqgridparam) //===复制时需要修改===
        {
            //  long longAreaId = Convert.ToInt32(areaId);//将节点的主键转化为long形式，因为数据库中的主键为bigint行
            List<BBdbR_FacBase> listEntity = new List<BBdbR_FacBase>();
            string sql = "";
            if (parentId != "0")
            {
                //从本表中查询上级表主键与传入主键相同相等的数据，并返回列表
                sql = $"select * from { tableName } where CompanyId ='{ areaId }'and Enabled = '1'";    //===复制时需要修改===
                listEntity = Repository().FindListBySql(sql); //执行sql语句
                for (int i = 0; i < listEntity.Count; i++)
                {
                    string sql1 = "select * from BBdbR_CompanyBase where CompanyId='" + listEntity[i].CompanyId + "' and Enabled = '1'";
                    DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        listEntity[i].CompanyId = dt1.Rows[0]["CompanyNm"].ToString();
                    }
                }
            }
            else
            {
                sql = "select * from BBdbR_FacBase where Enabled = '1'and CompanyId='" + areaId + "' ";     //===复制时需要修改===
                listEntity = Repository().FindListBySql(sql); //执行sql语句
                for (int i = 0; i < listEntity.Count; i++)
                {
                    string sql1 = "select * from BBdbR_FacBase where CompanyId='" + listEntity[i].CompanyId + "' and Enabled = '1'";
                    DataTable dt1 = Repository().FindTableBySql(sql1.ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        listEntity[i].RsvFld1 = dt1.Rows[0]["FacNm"].ToString();
                    }
                }
            }
            return listEntity;
        }
        #endregion
        #region 6.删除时使用，查找本表中某一条数据下面是否绑定了其他表格中的数据
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
            string sql = "select * from " + tableName + " where " + Condition + " = '" + keyValue + "' and Enabled = '1'";
            int count = Repository().FindCountBySql(sql);
            return count;
        }
        #endregion
        #region 7.删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string[] array)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<BBdbR_FacBase> listEntity = new List<BBdbR_FacBase>(); //===复制时需要修改===
            foreach (string keyValue in array)
            {
                //===复制时需要修改===
                BBdbR_FacBase entity = Repository().FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                entity.Enabled = "0";//将该实体的IsAvailable属性改为false
                entity.Modify(keyValue);
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion
        #region 8.加载全部工厂列表
        /// <summary>
        /// 联合查询
        /// </summary>
        /// <returns></returns>
        public List<BBdbR_FacBase> GetPlanList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_FacBase where Enabled = '1' order by FacCd");
            List<BBdbR_FacBase> dt = Repository().FindListBySql(strSql.ToString());          
            return dt;
        }
        public List<BBdbR_FacBase> GetFacList(string FacNm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_FacBase where  FacNm='" + FacNm + "' and Enabled = '1'");
            List<BBdbR_FacBase> dt = Repository().FindListBySql(strSql.ToString());
            return dt;
        }
        public List<BBdbR_FacBase> GetTypeList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from [Base_DataDictionaryDetail]where DataDictionaryId=(select [DataDictionaryId] from [Base_DataDictionary] where FullName='推送信息类型')");
            List<BBdbR_FacBase> dt = Repository().FindListBySql(strSql.ToString());
            return dt;
        }
        public List<BBdbR_FacBase> GetFacList1(string FacId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_FacBase where  FacId='" + FacId + "' and Enabled = '1'");
            List<BBdbR_FacBase> dt = Repository().FindListBySql(strSql.ToString());
            return dt;
        }

        #endregion
        #region 10.获取树，需要修改sql语句
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===复制时需要修改===
            strSql.Append(@"select CompanyId AS keys,
                        CompanyCd AS code,
                        CompanyNm AS name,
                        Enabled As IsAvailable,
                        '0' as parentId,  
                        '0' as sort    
                        from BBdbR_CompanyBase where Enabled = '1' ");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion
        //11.获取所有人员
        public DataTable GetPlineNm()
        {
            string sql = @"select StfId as id, stfnm from BBdbR_StfBase where Enabled='1' and StfPosn='工厂负责人'";
            return Repository().FindTableBySql(sql);
        }
        //获取人员信息
        public DataTable GetPlineNm2(string StfId)
        {
            string sql = @"select stfnm,phn,email from BBdbR_StfBase where Enabled='1' and StfId=" + "'" + StfId + "'";
            return Repository().FindTableBySql(sql);
        }
        /// <summary>
        /// 编辑弹框中数据源
        /// </summary>
        /// <returns></returns>
        public BBdbR_FacBase GetPlanList(string KeyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_FacBase where  FacId='" + KeyValue + "' and Enabled = '1'");
            List<BBdbR_FacBase> dt = Repository().FindListBySql(strSql.ToString());
            BBdbR_FacBase Dvcentity = new BBdbR_FacBase();
            //for (int i = 0; i < dt.Count; i++)
            //{
            //    string sql = "select * from BBdbR_FacBase where FacId='" + dt[i].FacId + "'";
            //    DataTable dt1 = Repository().FindTableBySql(sql.ToString());
            //    if (dt1.Rows.Count > 0)
            //    {
            //        dt[i].FacId = dt1.Rows[0]["FacNm"].ToString();
            //    }
            //}
            Dvcentity = dt[0];
            return Dvcentity;
        }
    }
}