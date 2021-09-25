//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// 人员基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.26 19:27</date>
    /// </author>
    /// </summary>
    public class BBdbR_StfBaseBll : RepositoryFactory<BBdbR_StfBase>
    {
        #region 全局变量定义区

        //定义本页面主要操作的表的表名，称为主表
        string tableName = "BBdbR_StfBase";//===复制时需要修改===
        public readonly RepositoryFactory<Base_User> repository_User = new RepositoryFactory<Base_User>();
        #endregion

        #region 1.获取树，需要修改sql语句
        public DataTable GetTree()
        {
            StringBuilder strSql = new StringBuilder();
            //===复制时需要修改===
            strSql.Append(@"select  
                        DepartmentID AS keys,     
                        DepartmentCode AS code,
                        DepartmentName AS name,
                        Enabled As IsAvailable,
                        '0' as parentId,  
                        0 as sort    
                        from BBdbR_DepartmentBase where Enabled = '1' and DepartmentType='父部门'");
            strSql.Append(@" union select    
                             DepartmentID AS keys,
                             DepartmentCode AS code,
                             DepartmentName AS name,
                             Enabled As IsAvailable,
                             ParentDepartmentID AS parentId,
                             1 as sort 
                          from  BBdbR_DepartmentBase where Enabled = '1' and DepartmentType='子部门' order by code asc");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion

        #region 2.编辑方法
        //将修改后的实体跟新到数据库中
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Update(BBdbR_StfBase entity) //===复制时需要修改===
        {
            return Repository().Update(entity); //将修改后的实体跟新到数据库中
        }
        #endregion

        #region 3.检查某个字段的字段值是否存在
        /// <summary>
        ///   Enabled = 1的数据中某个字段（KeyName）的字段值（KeyValue）是否存在
        /// </summary>
        /// <param name="KeyName">字段名</param>
        /// <param name="KeyValue">字段值</param>
        /// <returns>返回的是搜索的表中包含该字段值的记录条数</returns>
        public int CheckCount(string KeyName, string KeyValue)
        {
            string sql = @"select * from " + tableName + " where Enabled = '1' and " + KeyName + " = '" + KeyValue + "'";
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
        public int Insert(BBdbR_StfBase entity) //===复制时需要修改===
        {
            entity.Enabled = "1";

            return Repository().Insert(entity);
        }
        #endregion

        #region 5.删除方法
        //array 需要删除的信息的主键的数组
        //删除表中某一条数据表示将表中该条数据的Enabled设置为0，并不是真的删除该数据
        //返回值为1，或者0
        //1表示操作成功，0表示操作失败
        public int Delete(string[] array)
        {
            //创建一个表格的list，用于存储通过主键查询到的信息
            List<BBdbR_StfBase> listEntity = new List<BBdbR_StfBase>(); //===复制时需要修改===
            foreach (string keyValue in array)
            {
                //===复制时需要修改===
                BBdbR_StfBase entity = Repository().FindEntity(keyValue);//根据主键（keyValue）在数据库中查找实体 //===复制时需要修改===
                entity.Enabled = "0";//将该实体的IsAvailable属性改为false
                listEntity.Add(entity);
            }
            return Repository().Update(listEntity);//修改数据库
        }
        #endregion

        #region 6.页面表格
        /// <summary>
        /// 联合查询，展示页面表格
        /// </summary>
        /// <param name="CheckId"></param>
        /// <returns></returns>

        public List<BBdbR_StfBase> GetPlineList(string WorkSectionNm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_WorkSectionBase where Enable=1 and WorkSectionNm='" + WorkSectionNm + "'");
            List<BBdbR_StfBase> dt = Repository().FindListBySql(strSql.ToString());
            return dt;
        }
        public List<BBdbR_StfBase> GetPlineList2(string WorkSectionCd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  * FROM  BBdbR_WorkSectionBase where Enable=1 and WorkSectionCd='" + WorkSectionCd + "'");
            List<BBdbR_StfBase> dt = Repository().FindListBySql(strSql.ToString());
            return dt;
        }

        #endregion

        #region 7.班组产线配置
        /// <summary>
        /// 查询班次基本信息表里存在，但未存在配置的班次列表
        /// 查询的时候传递了keywords
        /// </summary>
        /// <param name="keywords">查询值</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public List<BBdbR_StfBase> GetReStaffList(string keywords, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select * from " + tableName + " where Enabled=1 and StfId not in (select Distinct(StfId) from BFacR_TeamStfConfig where Enabled=1 and TeamId='" + keywords + "')";
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
        public List<BBdbR_StfBase> GetStaffList(string keywords, JqGridParam jqgridparam) //===复制时需要修改===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select a.* from " + tableName + " a join BFacR_TeamStfConfig b on a.StfId=b.StfId where a.Enabled=1 and b.Enabled=1 and b.TeamId='" + keywords + "'";
                return Repository().FindListPageBySql(sql.ToString(), ref jqgridparam);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 搜索表格中挂在部门名下的人员数量
        /// </summary>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns>返回搜索到的数据</returns>
        public int GetStfCount(string KeyValue) //===复制时需要修改===
        {
            string sql = "";
            if (KeyValue != "")
            {
                sql = @"select * from " + tableName + " where Enabled='1' and DepartmentID='" + KeyValue + "'";
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

        #region 系统管理

        #region  点击推送信息获取用户信息
        public DataTable GetUserInforList(string DeptCd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from BBdbR_StfBase where DeptCd='" + DeptCd + "'");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion
        #region  查询用户信息
        public DataTable GetAllUserList(string condition, string keywords, string Tycd, string ParameterJson, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT StfId,StfCd,StfNm,Email FROM BBdbR_StfBase where Enabled=1 ");
            switch (condition)
            {
                case "stfcd"://用户编号
                    strSql.Append(" and stfcd LIKE  '%" + keywords + "%'");
                    break;
                case "stfnm"://用户名称
                    strSql.Append(" and stfnm LIKE  '%" + keywords + "%'");
                    break;
                default:
                    break;
            }
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string code = dt.Rows[i]["StfId"].ToString();
                StringBuilder str = new StringBuilder();
                str.Append(@"select * from BBdbR_PushObjectConf where ObjectId='" + code + "' and InforTypCd='" + Tycd + "'and Enabled='" + 1 + "'");
                DataTable dt1 = Repository().FindTableBySql(str.ToString());
                if (dt1.Rows.Count != 0)
                {
                    dt.Rows.Remove(dr);
                }
            }
            return dt;
            //return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion
        #region 获取未配置该角色的用户信息
        public DataTable GetNoRoleUserList(string ObjectId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from BBdbR_StfBase where Enabled=1");
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string code = dt.Rows[i]["StfId"].ToString();
                StringBuilder str = new StringBuilder();
                str.Append(@"select * from Base_StfRoleConf where StfId='" + code + "' and RoleId='" + ObjectId + "' ");
                DataTable dt1 = Repository().FindTableBySql(str.ToString());
                if (dt1.Rows.Count != 0)
                {
                    dt.Rows.Remove(dr);
                }
            }
            return dt;
        }
        #endregion
        #region 获取角色人员
        /// <param name="RoleId">对象主键</param>
        public DataTable GetRoleUserList(string RoleId)
        {
            StringBuilder strSql = new StringBuilder();
            //List<DbParameter> parameter = new List<DbParameter>();
            //parameter.Add(DbFactory.CreateDbParameter("@RoleId", RoleId));
            strSql.Append(@"SELECT  r.StfId ,				
                                    r.DeptCd ,				
                                    r.StfNm ,			
                                    r.StfCd ,	
                                    r.StfGndr ,
                                    r.Account ,	
                                    ou.RoleId			
                            FROM    BBdbR_StfBase r
                                    LEFT JOIN Base_StfRoleConf ou ON ou.StfId = r.StfId
                                                                            AND ou.RoleId = '" + RoleId + "'");
            strSql.Append(" WHERE 1 = 1");

            //strSql.Append(@"SELECT  *
            //                FROM    ( SELECT    u.StfId ,				
            //                                    u.DeptCd ,				
            //                                    u.StfNm ,			
            //                                    u.StfCd ,				
            //                                    u.Account ,				
            //                                    u.StfGndr ,			
            //                                    u.SortCode ,					
            //                                    ou.RoleId				
            //                          FROM      BBdbR_StfBase u
            //                                    LEFT JOIN Base_StfRoleConf ou ON ou.StfId = u.StfId
            //AND ou.RoleId = @RoleId                                                                                        
            //                        ) T WHERE 1=1");

            return Repository().FindTableBySql(strSql.ToString());
        }

        #endregion
        #region 加载全部用户
        public DataTable GetUserList(JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM Base_User WHERE 1=1");
            return Repository().FindTableBySql(strSql.ToString());
        }
        #endregion
        #region 获取未配置用户信息
        public DataTable GetNoConList(string Tycd, string ParameterJson, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from BBdbR_StfBase where Enabled = 1");
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string code = dt.Rows[i]["StfId"].ToString();
                StringBuilder str = new StringBuilder();
                str.Append(@"select * from BBdbR_PushObjectConf where ObjectId='" + code + "' and InforTypCd='" + Tycd + "'and Enabled='" + 1 + "'");
                DataTable dt1 = Repository().FindTableBySql(str.ToString());
                if (dt1.Rows.Count != 0)
                {
                    dt.Rows.Remove(dr);
                }
            }
            return dt;
        }
        #endregion
        #region 获取已配置用户
        public DataTable GetConUserList(string Tycd, string Rank, string ParameterJson, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from PushObject_StfBase_V where InforTypCd='" + Tycd + "' and PushRank='" + Rank + "' and Enabled='" + 1 + "'");
            DataTable dt = Repository().FindTableBySql(strSql.ToString());
            return dt;
        }
        #endregion
        #region 删除之前用户的配置
        public int DeleteConUser(string Tycd, string Type, string Rank)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"Delete from BBdbR_PushObjectConf  where InforTypCd = '" + Tycd + "' and PushRank = '" + Rank + "' and ObjectTyp = '" + 1 + "'");
            return Repository().ExecuteBySql(strSql);
        }
        #endregion

        #endregion
    }
}
