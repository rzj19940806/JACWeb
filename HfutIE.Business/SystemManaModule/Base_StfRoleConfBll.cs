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
    /// 角色人员表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.04.03 09:48</date>
    /// </author>
    /// </summary>
    public class Base_StfRoleConfBll : RepositoryFactory<Base_StfRoleConf>
    {
        #region
        #endregion

        #region 成批提交角色用户
        public int BatchAddMember(string[] arrayUserId, string RoleId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"DELETE FROM Base_StfRoleConf WHERE RoleId = '" + RoleId + "' ");
                Repository().ExecuteBySql(strSql);
                int index = 1;
                foreach (string item in arrayUserId)
                {
                    if (item.Length > 0)
                    {                       
                        Base_StfRoleConf entity = new Base_StfRoleConf();
                        entity.RecId = Guid.NewGuid().ToString();
                        entity.RoleId = RoleId;
                        entity.StfId = item;
                        entity.SortCode = index;
                        entity.Enabled = "1";
                        entity.CreTm = DateTime.Now;
                        entity.Create();
                        index++;
                        database.Insert(entity, isOpenTrans);
                    }
                }
                database.Commit();
                return 1;
            }
            catch(Exception ex)
            {
                database.Rollback();
                return -1;
            }
        }
        #endregion

        #region 成批提交用户角色
        public int BatchAddRole(string[] arrayRoleId, string StfId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"DELETE FROM Base_StfRoleConf WHERE StfId = '" + StfId + "' ");
                Repository().ExecuteBySql(strSql);
                int index = 1;
                foreach (string item in arrayRoleId)
                {
                    if (item.Length > 0)
                    {
                        Base_StfRoleConf entity = new Base_StfRoleConf();
                        entity.RecId = Guid.NewGuid().ToString();
                        entity.RoleId = item;
                        entity.StfId = StfId;
                        entity.SortCode = index;
                        entity.Enabled = "1";
                        entity.CreTm = DateTime.Now;
                        entity.Create();
                        index++;
                        database.Insert(entity, isOpenTrans);
                    }
                }
                database.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                database.Rollback();
                return -1;
            }
        }
        #endregion

    }
}