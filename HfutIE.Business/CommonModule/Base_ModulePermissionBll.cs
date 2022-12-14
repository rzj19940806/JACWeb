//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2014
// Software Developers @ HfutIE 2014
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;
using HfutIE.Cache;

namespace HfutIE.Business
{
    /// <summary>
    /// 模块权限表
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.18 16:01</date>
    /// </author>
    /// </summary>
    public class Base_ModulePermissionBll : RepositoryFactory<Base_ModulePermission>
    {
        private static Base_ModulePermissionBll item;
        public static Base_ModulePermissionBll Instance
        {
            get
            {
                if (item == null)
                {
                    item = new Base_ModulePermissionBll();
                }
                return item;
            }
        }
        /// <summary>
        /// 模块权限列表————分配权限时使用
        /// </summary>
        /// <param name="ObjectId">对象主键</param>
        /// <param name="Category">对象分类:1-部门2-角色3-岗位4-群组</param>
        /// <returns></returns>
        public DataTable GetList(string ObjectId, string Category)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"select * from (SELECT  m.ModuleId ,				
                                        m.ParentId ,				
                                        m.Code ,					
                                        m.FullName ,				
                                        m.Icon ,					
                                        m.SortCode ,				
                                        mp.ObjectId	,
                                        m.Enabled,
                                        ROW_NUMBER() OVER(PARTITION BY m.ModuleId ORDER BY m.SortCode ASC) as RepeatNum
                                FROM    Base_Module m
                                        LEFT JOIN Base_ModulePermission mp ON mp.ModuleId = m.ModuleId
                                                                          AND mp.ObjectId IN ('" + ObjectId.Replace(",", "','") + "') ");//@ObjectId

            //if (!ManageProvider.Provider.Current().IsSystem)
            //{
            //    strSql.Append(@"SELECT  m.ModuleId ,				
            //                            m.ParentId ,				
            //                            m.Code ,					
            //                            m.FullName ,				
            //                            m.Icon ,					
            //                            m.SortCode ,	
            //                            m.Enabled,
            //                            cp.ModuleId AS ObjectId     
            //                    FROM    Base_Module M INNER JOIN ( SELECT DISTINCT ModuleId  FROM   Base_ModulePermission");
            //    strSql.Append(" WHERE  ObjectId IN ('" + ObjectId.Replace(",", "','") + "')) MP ON M.ModuleId = mp.ModuleId");//ManageProvider.Provider.Current().
            //    strSql.Append(" LEFT JOIN ( SELECT DISTINCT ModuleId  FROM  Base_ModulePermission");
            //    //2021.11.29修改
            //    //strSql.Append(" WHERE  ObjectId = @ObjectId ) CP ON cp.ModuleId = M.ModuleId");
            //    strSql.Append(" WHERE  ObjectId IN ('" + ObjectId.Replace(",", "','") + "') ) CP ON cp.ModuleId = M.ModuleId");//ManageProvider.Provider.Current().

            //}
            //else
            //{


            //}
            //2021.11.28新增
            strSql.Append(@" where  m.Enabled = '1' ) a where RepeatNum = '1'");//2021.11.28新增

            //parameter.Add(DbFactory.CreateDbParameter("@ObjectId", ObjectId));
            strSql.Append(" ORDER BY  SortCode ASC ");
            return Repository().FindTableBySql(strSql.ToString(), parameter.ToArray());
        }
        /// <summary>
        /// 加载权限模块
        /// </summary>
        /// <param name="ObjectId">对象主键</param>
        /// <returns></returns>
        public List<Base_Module> GetModuleList(string ObjectId)
        {
            StringBuilder strSql = new StringBuilder();
            if (!ManageProvider.Provider.Current().IsSystem)
            {
                //strSql.Append($"SELECT DISTINCT  M.* FROM Base_Module M INNER JOIN Base_ModulePermission MP ON M.ModuleId = MP.ModuleId WHERE ObjectId IN (select RoleId from Base_StfRoleConf where StfId='{ManageProvider.Provider.Current().ObjectId}') or ObjectId='{ManageProvider.Provider.Current().ObjectId}' ORDER BY  M.SortCode ASC ");
                //strSql.Append($" INNER JOIN Base_ModulePermission MP ON M.ModuleId = MP.ModuleId WHERE ObjectId IN ('{ManageProvider.Provider.Current().ObjectId.Replace(",", "','")}')");
                strSql.Append(@"SELECT DISTINCT  M.* FROM Base_Module M");
                strSql.Append(" INNER JOIN Base_ModulePermission MP ON M.ModuleId = MP.ModuleId WHERE   ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "')");
            }
            else
            {
                strSql.Append(@"SELECT * FROM Base_Module M");
            }
            strSql.Append(" ORDER BY  M.SortCode ASC ");
            return DataFactory.Database().FindListBySql<Base_Module>(strSql.ToString());
        }
        /// <summary>
        /// 根据对象Id获取模块权限列表——用户登录个人中心时调用
        /// </summary>
        /// <param name="ObjectId">对象ID</param>
        /// <returns></returns>
        public DataTable GetModulePermission(string ObjectId)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append(@"(SELECT  m.ModuleId ,
            //                        m.ParentId ,
            //                        m.FullName ,
            //                        m.Icon
            //                FROM    Base_Module m
            //                        LEFT JOIN Base_ModulePermission mp ON mp.ModuleId = m.ModuleId");
            //strSql.Append(" WHERE   mp.ObjectId IN ('" + ObjectId.Replace(",", "','") + "')");
            //strSql.Append(" ORDER BY  m.SortCode ASC ");

            //2021.11.28 修改
            strSql.Append(@"select * from(SELECT  m.ModuleId ,
                                    m.ParentId ,
                                    m.FullName ,
                                    m.Icon,
                                    ROW_NUMBER() OVER(PARTITION BY m.ModuleId ORDER BY m.SortCode ASC) as RepeatNum
                            FROM    Base_Module m
                                    LEFT JOIN Base_ModulePermission mp ON mp.ModuleId = m.ModuleId");
            strSql.Append(" WHERE   mp.ObjectId IN ('" + ObjectId.Replace(",", "','") + "')) as a where a.RepeatNum = 1 ");
            return Repository().FindTableBySql(strSql.ToString());
        }

        /// <summary>
        /// Action执行权限认证
        /// </summary>
        /// <param name="Action">视图Action</param>
        /// <param name="ObjectId">对象主键</param>
        /// <param name="ModuleId">模块Id</param>
        /// <param name="UserId">用户Id</param>
        /// <returns></returns>
        public bool ActionAuthorize(string Action, string ObjectId, string ModuleId, string UserId)
        {
            List<Base_Module> ListData = new List<Base_Module>();
            object ActionAuthorize_List = DataCache.Get("ActionAuthorizeList_" + UserId);
            if (ActionAuthorize_List == null)
            {
                StringBuilder strSql = new StringBuilder();
             //   strSql.Append(@"SELECT  b.ModuleId,b.FullName ,b.ActionEvent AS Location FROM    Base_Button b INNER JOIN Base_ButtonPermission bp ON bp.ModuleButtonId = b.ButtonId AND bp.ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "')");
                strSql.Append(@"SELECT  b.ModuleId,b.FullName ,b.ActionEvent AS Location FROM    Base_Button b INNER JOIN Base_ButtonPermission bp ON bp.ModuleButtonId = b.ButtonId AND bp.ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "')");
                strSql.Append(" UNION ");
                strSql.Append(@"SELECT  m.ModuleId,m.FullName , m.Location FROM    Base_Module m INNER JOIN Base_ModulePermission mp ON mp.ModuleId = m.ModuleId  AND mp.ObjectId IN ('" + ManageProvider.Provider.Current().ObjectId.Replace(",", "','") + "')");
                ListData = DataFactory.Database().FindListBySql<Base_Module>(strSql.ToString());
                DataCache.Insert("ActionAuthorizeList_" + UserId, ListData);
            }
            else
            {
                ListData = (List<Base_Module>)ActionAuthorize_List;
            }
            foreach (Base_Module entity in ListData)
            {
                string[] url = entity.Location.ToLower().Split('?');
                if (url[0] == Action && entity.ModuleId == ModuleId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}