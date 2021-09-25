//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2021
// Software Developers @ HfutIE 2021
//=====================================================================================

using HfutIE.DataAccess.Attributes;
using HfutIE.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HfutIE.Entity
{
    /// <summary>
    /// 操作日志明细表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.04.03 09:50</date>
    /// </author>
    /// </summary>
    [Description("操作日志明细表")]
    [PrimaryKey("SysLogDetailId")]
    public class S_SysLogDetail : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 日志明细主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("日志明细主键")]
        public string SysLogDetailId { get; set; }
        /// <summary>
        /// 日志主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("日志主键")]
        public string SysLogId { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("属性名称")]
        public string PropertyName { get; set; }
        /// <summary>
        /// 属性字段
        /// </summary>
        /// <returns></returns>
        [DisplayName("属性字段")]
        public string PropertyField { get; set; }
        /// <summary>
        /// 属性新值
        /// </summary>
        /// <returns></returns>
        [DisplayName("属性新值")]
        public string NewValue { get; set; }
        /// <summary>
        /// 属性原值
        /// </summary>
        /// <returns></returns>
        [DisplayName("属性原值")]
        public string OldValue { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public DateTime? CreTm { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.SysLogDetailId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.SysLogDetailId = KeyValue;
                                            }
        #endregion
    }
}