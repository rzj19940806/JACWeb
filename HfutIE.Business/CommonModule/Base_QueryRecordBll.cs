//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2014
// Software Developers @ HfutIE 2014
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.Entity;
using HfutIE.Repository;
using HfutIE.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace HfutIE.Business
{
    /// <summary>
    /// ��ѯ������¼
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.01 15:27</date>
    /// </author>
    /// </summary>
    public class Base_QueryRecordBll : RepositoryFactory<Base_QueryRecord>
    {
        /// <summary>
        /// ����������ȡ�����б�
        /// </summary>
        /// <param name="ModuleId">ģ��ID</param>
        /// <param name="CreateUserId">�û�ID</param>
        /// <returns></returns>
        public List<Base_QueryRecord> GetList(string ModuleId, string CreateUserId)
        {
            StringBuilder WhereSql = new StringBuilder();
            WhereSql.Append(" AND (CreateUserId = @CreateUserId ");
            WhereSql.Append(" OR ResourceShare=1)");
            WhereSql.Append(" AND ModuleId = @ModuleId Order By CreateDate Desc");
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@CreateUserId", CreateUserId));
            parameter.Add(DbFactory.CreateDbParameter("@ModuleId", ModuleId));
            return DataFactory.Database().FindList<Base_QueryRecord>(WhereSql.ToString(), parameter.ToArray());
        }
        /// <summary>
        /// ���ó�ʼ��Ĭ�Ϸ���
        /// </summary>
        /// <param name="ModuleId">ģ��ID</param>
        /// <param name="QueryRecordId">����</param>
        /// <returns></returns>
        public int DefaultProject(string ModuleId, string QueryRecordId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(string.Format("UPDATE Base_QueryRecord SET NextDefault = 0 WHERE ModuleId = '{0}'", ModuleId));
                database.ExecuteBySql(strSql, isOpenTrans);
                Base_QueryRecord entity = new Base_QueryRecord();
                if (!string.IsNullOrEmpty(QueryRecordId))
                {
                    entity.QueryRecordId = QueryRecordId;
                    entity.NextDefault = 1;
                    database.Update(entity, isOpenTrans);
                }
                database.Commit();
                return 1;
            }
            catch
            {
                database.Rollback();
                return -1;
            }
        }
    }
}