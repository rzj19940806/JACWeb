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
    /// 连接点质量结果数据中间表
    /// <author>
    ///	<name>CHFAS</name>
    /// <date>2021.07.20 12:00</date>
    /// </author>
    /// </summary>
    public class QAS_JunctionDataMidBll : RepositoryFactory<QAS_JunctionDataMid>
    {
        #region
        #endregion

        #region 全局变量定义区
        //定义本页面主要操作的表的表名，称为主表
        string tableName = "QAS_JunctionDataMid";

        #endregion

        #region 方法区
        /// <summary>
        /// 多条件查询
        /// </summary>
        /// <param name="PropertyName1">condition值</param>
        /// <param name="PropertyValue1">模糊对应</param>
        /// <param name="PropertyName2">字段名2</param>
        /// <param name="PropertyValue2">全对应</param>
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