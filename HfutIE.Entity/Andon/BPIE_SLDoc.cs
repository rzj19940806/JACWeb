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
    /// 停线日志表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.12.06 15:17 ckl</date>
    /// </author>
    /// </summary>
    [Description("停线日志表")]
    [PrimaryKey("RecId")]
    public class BPIE_SLDoc : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 记录主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("记录主键")]
        public string RecId { get; set; }
        /// <summary>
        /// 产线主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线主键")]
        public string PlineId { get; set; }
        /// <summary>
        /// 产线名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线名称")]
        public string PlineNm { get; set; }
        /// <summary>
        /// 停线状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("停线状态")]
        public string State { get; set; }
        /// <summary>
        /// 停线来源
        /// </summary>
        /// <returns></returns>
        [DisplayName("停线来源")]
        public string SLSource { get; set; }
        /// <summary>
        /// 停线开始时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("停线开始时间")]
        public DateTime? SLStrtTm { get; set; }
        /// <summary>
        /// 停线结束时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("停线结束时间")]
        public DateTime? SLCmplTm { get; set; }
        /// <summary>
        /// 持续时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("持续时间")]
        public Decimal? SLContTm { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效性")]
        public string Enabled { get; set; }
     
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Rem { get; set; }
        /// <summary>
        /// 预留字段1
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段1")]
        public string RsvFld1 { get; set; }
        /// <summary>
        /// 预留字段2
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段2")]
        public string RsvFld2 { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.RecId = CommonHelper.GetGuid;
            this.Enabled = "1";
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.RecId = KeyValue;
        }
        #endregion
    }
}