//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2022
// Software Developers @ HfutIE 2022
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
    /// P_ProductionLog
    /// <author>
    ///		<name>she</name>
    ///		<date>2022.03.01 13:03</date>
    /// </author>
    /// </summary>
    [Description("P_ProductionLog")]
    [PrimaryKey("OPId")]
    public class P_ProductionLog : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// OPId
        /// </summary>
        /// <returns></returns>
        [DisplayName("OPId")]
        public string OPId { get; set; }
        /// <summary>
        /// OPModule
        /// </summary>
        /// <returns></returns>
        [DisplayName("OPModule")]
        public string OPModule { get; set; }
        /// <summary>
        /// OPAction
        /// </summary>
        /// <returns></returns>
        [DisplayName("OPAction")]
        public string OPAction { get; set; }
        /// <summary>
        /// OPObject
        /// </summary>
        /// <returns></returns>
        [DisplayName("OPObject")]
        public string OPObject { get; set; }
        /// <summary>
        /// OPAssistObject
        /// </summary>
        /// <returns></returns>
        [DisplayName("OPAssistObject")]
        public string OPAssistObject { get; set; }
        /// <summary>
        /// OPType
        /// </summary>
        /// <returns></returns>
        [DisplayName("OPType")]
        public string OPType { get; set; }
        /// <summary>
        /// OPResult
        /// </summary>
        /// <returns></returns>
        [DisplayName("OPResult")]
        public string OPResult { get; set; }
        /// <summary>
        /// OPRem
        /// </summary>
        /// <returns></returns>
        [DisplayName("OPRem")]
        public string OPRem { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserId")]
        public string UserId { get; set; }
        /// <summary>
        /// UserCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserCd")]
        public string UserCd { get; set; }
        /// <summary>
        /// UserNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserNm")]
        public string UserNm { get; set; }
        /// <summary>
        /// PointId
        /// </summary>
        /// <returns></returns>
        [DisplayName("PointId")]
        public string PointId { get; set; }
        /// <summary>
        /// PointCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("PointCd")]
        public string PointCd { get; set; }
        /// <summary>
        /// PointNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("PointNm")]
        public string PointNm { get; set; }
        /// <summary>
        /// OPTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("OPTime")]
        public DateTime? OPTime { get; set; }
        /// <summary>
        /// Enabled
        /// </summary>
        /// <returns></returns>
        [DisplayName("Enabled")]
        public string Enabled { get; set; }
        /// <summary>
        /// Filed
        /// </summary>
        /// <returns></returns>
        [DisplayName("Filed")]
        public string Filed { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.OPId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.OPId = KeyValue;
                                            }
        #endregion
    }
}