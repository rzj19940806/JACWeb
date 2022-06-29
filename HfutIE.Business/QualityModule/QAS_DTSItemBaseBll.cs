//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
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

namespace HfutIE.Business
{
    /// <summary>
    /// DTS����������Ϣ��
    /// <author>
    ///		<name>CHFAS</name>
    ///		<date>2021.07.06 12:00</date>
    /// </author>
    /// </summary>
    public class QAS_DTSItemBaseBll : RepositoryFactory<QAS_DTSItemBase>
    {
        #region
        #endregion
        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "QAS_DTSItemBase";
        #endregion

        #region ������
        /// <summary>
        /// ��������ѯ
        /// </summary>
        /// <param name="PropertyName"></param>
        /// <param name="PropertyValue"></param>
        /// <returns></returns>
        public List<QAS_DTSItemBase> GetPageList(string PropertyName, string PropertyValue) 
        {
            List<QAS_DTSItemBase> dt;
            string sql = "";
            if (PropertyName == "")
            {
                sql = "select * from " + tableName + " where 1=1 order by DTSItemID";
                dt = Repository().FindListBySql(sql);
            }
            else
            {
                //����������ѯ
                sql = "select * from " + tableName + " where " + PropertyName + " like  '%" + PropertyValue + "%' order by DTSItemID";
                dt = Repository().FindListBySql(sql);
            }
            return dt;
        }

        #endregion
    }
}