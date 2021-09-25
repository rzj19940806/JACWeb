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
    /// 推送信息基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.25 21:11</date>
    /// </author>
    /// </summary>
    [Description("推送信息基础信息表")]
    [PrimaryKey("RecId")]
    public class BBdbR_PushInfor : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DisplayName("")]
        public string RecId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DisplayName("")]
        public string InforTypCd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DisplayName("")]
        public string InforTyp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DisplayName("")]
        public string PushRank { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DisplayName("")]
        public decimal? IntvlTm { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DisplayName("")]
        public string TmUnit { get; set; }
        /// <summary>
        /// 0-无效,1-有效
        /// </summary>
        /// <returns></returns>
        [DisplayName("0-无效,1-有效")]
        public string Enabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DisplayName("")]
        public DateTime? CreTm { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DisplayName("")]
        public string CreCd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DisplayName("")]
        public string CreNm { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DisplayName("")]
        public DateTime? MdfTm { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DisplayName("")]
        public string MdfCd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DisplayName("")]
        public string MdfNm { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DisplayName("")]
        public string Rem { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.RecId = CommonHelper.GetGuid;
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