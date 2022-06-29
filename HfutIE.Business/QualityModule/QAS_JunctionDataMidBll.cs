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
    /// ���ӵ�������������м��
    /// <author>
    ///	<name>CHFAS</name>
    /// <date>2021.07.20 12:00</date>
    /// </author>
    /// </summary>
    public class QAS_JunctionDataMidBll : RepositoryFactory<QAS_JunctionDataMid>
    {
        #region
        #endregion

        #region ȫ�ֱ���������
        //���屾ҳ����Ҫ�����ı�ı�������Ϊ����
        string tableName = "QAS_JunctionDataMid";

        #endregion

        #region ������
        /// <summary>
        /// ��������ѯ
        /// </summary>
        /// <param name="PropertyName1">conditionֵ</param>
        /// <param name="PropertyValue1">ģ����Ӧ</param>
        /// <param name="PropertyName2">�ֶ���2</param>
        /// <param name="PropertyValue2">ȫ��Ӧ</param>
        /// <returns></returns>
        public List<QAS_JunctionDataMid> GetPageList(string PropNm1, string PropValue1, string PropNm2, string PropValue2, string PropNm3, string PropValue3)
        {
            List<QAS_JunctionDataMid> dt = new List<QAS_JunctionDataMid>();
            string sql = "";
            if (PropNm1 != "" && PropNm2 != "" && PropNm3 != "" && PropNm3 != null)
            {
                sql = "select * from " + tableName + " where " + PropNm1 + " = '" + PropValue1 + "' and " + PropNm2 + " = '" + PropValue2 + "'and " + PropNm3 + " = '" + PropValue3 + "' order by Code";
                dt = Repository().FindListBySql(sql);
            }
            else if (PropNm1 != "" && PropNm2 != "" && (PropNm3 == null || PropNm3 == ""))
            {
                sql = "select * from " + tableName + " where " + PropNm1 + " = '" + PropValue1 + "' and " + PropNm2 + " = '" + PropValue2 + "' order by Code";
                dt = Repository().FindListBySql(sql);
            }
            else if ((PropNm2 == "" || PropNm2 == null) && (PropNm3 == "" || PropNm3 == null))
            {
                sql = "select * from " + tableName + " where " + PropNm1 + " = '" + PropValue1 + "' order by Code";
                dt = Repository().FindListBySql(sql);
            }    
            return dt;
        }
        #endregion

    }
}