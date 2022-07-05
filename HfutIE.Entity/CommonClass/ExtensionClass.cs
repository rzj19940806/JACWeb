//------------------------------------------------------------------------------
//辅助类
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web;

namespace HfutIE.Entity
{

    public class ConditionAndKey
    {

        #region 01 简单属性
        /// <summary>
        /// 检索条件
        /// </summary>
        public string Condition { get; set; }
        /// <summary>
        /// 条件与关键词之间关系
        /// </summary>
        public string ExpressExtension { get; set; }

        /// <summary>
        /// 主关键词
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 条件之间的关系
        /// </summary>
        public string AndOr { get; set; }




        #endregion
    }
}
