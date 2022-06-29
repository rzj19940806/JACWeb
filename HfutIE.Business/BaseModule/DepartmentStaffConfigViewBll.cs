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
    /// ���ƻ�����Ϣ��
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.29 16:19</date>
    /// </author>
    /// </summary>
    public class DepartmentStaffConfigViewBll : RepositoryFactory<DepartmentStaffConfigView>
    {
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "DepartmentStaffConfigView";//===����ʱ��Ҫ�޸�===
        #endregion

        #region 1.����
        /// <summary>
        ///     ��ѯʱ�ṩ�������ؼ��֣�һ����Condition����һ����keywords
        ///     
        ///     Condition�ǹؼ��֣�����ѯ��������Ӧ���ݿ��е�һ���ֶ�
        ///     keywords�ǲ�ѯֵ������ѯ�����ľ���ֵ����Ӧ���ݿ��в�ѯ�����ֶε�ֵ
        ///     ��ѯ��ʱ�򴫵���Condition��keywords
        /// 
        /// </summary>
        /// <param name="keywords">��ѯֵ</param>
        /// <param name="Condition">�ؼ��֣���ѯ������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public List<DepartmentStaffConfigView> GetTeamStaffList(string keywords, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (keywords != "")
            {
                sql = @"select * from " + tableName + " where Enabled=1 and StfId='" + keywords+"'";
                return Repository().FindListBySql(sql);
            }
            else
            {
                sql = @"select * from " + tableName + " where Enabled=1";
                return Repository().FindListBySql(sql);
            }
        }
        #endregion

        #region 2.�����չʾ�����Ҫ�޸�sql���
        /// <summary>
        /// ������Ϣ������Ա��   --����-->   �����š�  --����-->  �������š�
        ///     ���ݴ���Ĳ�����ͬд��ͬ��sql�����в�ѯ           
        /// </summary>
        /// <param name="areaId">����Ľڵ������</param>
        /// <param name="parentId">�ڵ�ĸ�������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public List<DepartmentStaffConfigView> GetList(string areaId, string parentId, ref JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (string.IsNullOrEmpty(areaId) && string.IsNullOrEmpty(parentId))
            {
                sql = "select * from "+ tableName+" where  Enabled=1";     //===����ʱ��Ҫ�޸�===           
            }
            //δ�������Ĭ�ϼ��ر��
            else
            {
                //�������չʾ��Ӧ�ڵ���
                if (parentId != "0")
                {
                    //�ӱ����в�ѯ�ϼ��������봫��������ͬ��ȵ����ݣ��������б�
                    sql = "select * from " + tableName + " where DepartmentID ='" + areaId + "' and  Enabled=1";     //===����ʱ��Ҫ�޸�===               
                }
                else
                {
                    sql = "select * from  " + tableName + "  where DepartmentID='" + areaId + "'  and  Enabled=1  or ParentDepartmentID='" + areaId + "'";     //===����ʱ��Ҫ�޸�===             
                }
            }        
            return Repository().FindListBySql(sql);
        }
        #endregion

        #region 3.��ѯ��������Ҫ�޸�sql���
        /// <summary>
        ///     ��ѯʱ�ṩ�������ؼ��֣�һ����Condition����һ����keywords
        ///     
        ///     Condition�ǹؼ��֣�����ѯ��������Ӧ���ݿ��е�һ���ֶ�
        ///     keywords�ǲ�ѯֵ������ѯ�����ľ���ֵ����Ӧ���ݿ��в�ѯ�����ֶε�ֵ
        ///     ��ѯ��ʱ�򴫵���Condition��keywords
        /// 
        /// </summary>
        /// <param name="keywords">��ѯֵ</param>
        /// <param name="Condition">�ؼ��֣���ѯ������</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns>��ѯ�����ݣ��б�</returns>
        public List<DepartmentStaffConfigView> GetPageListByCondition(string keywords, string Condition, JqGridParam jqgridparam) //===����ʱ��Ҫ�޸�===
        {
            string sql = "";
            if (Condition == "all")
            {
                sql = "select * from " + tableName + " where  Enabled=1";     //===����ʱ��Ҫ�޸�===         
            }
            else
            {
                if (keywords == "all")
                {
                    sql = "select * from " + tableName + " where  Enabled=1";     //===����ʱ��Ҫ�޸�===       
                }
                else
                {
                    sql = "select * from " + tableName + " where  Enabled=1 and " + Condition + " like  '%" + keywords + "%'";   //===����ʱ��Ҫ�޸�===       
                }          
            }
            return Repository().FindListBySql(sql);
        }
        #endregion


    }
}