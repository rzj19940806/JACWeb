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
    /// ϵͳ�쳣��־��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.04.03 09:50</date>
    /// </author>
    /// </summary>
    public class S_LogErrBll : RepositoryFactory<S_LogErr>
    {
        #region
        #endregion

        #region ������־�б�
        /// <summary>
        /// �����б�
        /// </summary>
        /// <param name="StartTime">�Ƶ���ʼʱ��</param>
        /// <param name="EndTime">�Ƶ�����ʱ��</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetLogList(string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM S_LogErr WHERE 1=1 ");
            //�Ƶ���ʼ
            if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
            {

                parameter.Add(DbFactory.CreateDbParameter("@StartTime", CommonHelper.GetDateTime(StartTime + " 00:00")));
                parameter.Add(DbFactory.CreateDbParameter("@EndTime", CommonHelper.GetDateTime(EndTime + " 23:59")));
                strSql.Append(" AND RecTm Between '" + StartTime + "' AND '" + EndTime + "' ");
                //strSql.Append("AND ORDER BY SequenceNum ASC ");
            }
            return Repository().FindTableBySql(strSql.ToString());
        }       

        #endregion

        #region ���ر��
        public DataTable GetPageList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from S_LogErr where 1 = 1 Order By RecTm ASC");
            return Repository().FindTableBySql(strSql.ToString());

        }
        #endregion

        #region �����־
        /// <summary>
        /// ��ղ�����־
        /// </summary>
        /// <param name="CreateDate"></param>
        /// <returns></returns>
        public int RemoveLog(string KeepTime)
        {
            StringBuilder strSql = new StringBuilder();
            DateTime CreateDate = DateTime.Now;
            if (KeepTime == "7")//������һ��
            {
                CreateDate = DateTime.Now.AddDays(-7);
            }
            else if (KeepTime == "1")//������һ����
            {
                CreateDate = DateTime.Now.AddMonths(-1);
            }
            else if (KeepTime == "3")//������������
            {
                CreateDate = DateTime.Now.AddMonths(-3);
            }
            if (KeepTime == "0")//��������ȫ��ɾ��
            {
                strSql.Append("DELETE FROM S_LogErr");
                return DataFactory.Database().ExecuteBySql(strSql);
            }
            else
            {
                strSql.Append("DELETE FROM S_LogErr WHERE 1=1 ");
                strSql.Append("AND RecTm <= '" + CreateDate + "'");
                return DataFactory.Database().ExecuteBySql(strSql);
            }
        }
        #endregion

    }
}