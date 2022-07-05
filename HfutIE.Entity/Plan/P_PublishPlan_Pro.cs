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
    /// P_PublishPlan_Pro
    /// <author>
    ///		<name>she</name>
    ///		<date>2022.01.20 10:22</date>
    /// </author>
    /// </summary>
    [Description("P_PublishPlan_Pro")]
    [PrimaryKey("PublishPlanProId")]
    public class P_PublishPlan_Pro : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// PublishPlanProId
        /// </summary>
        /// <returns></returns>
        [DisplayName("PublishPlanProId")]
        public string PublishPlanProId { get; set; }
        /// <summary>
        /// ProducePlanCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("ProducePlanCd")]
        public string ProducePlanCd { get; set; }
        /// <summary>
        /// OrderCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("OrderCd")]
        public string OrderCd { get; set; }
        /// <summary>
        /// VIN
        /// </summary>
        /// <returns></returns>
        [DisplayName("VIN")]
        public string VIN { get; set; }
        /// <summary>
        /// IsCheck
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsCheck")]
        public int? IsCheck { get; set; }
        /// <summary>
        /// Quene
        /// </summary>
        /// <returns></returns>
        [DisplayName("Quene")]
        public int? Quene { get; set; }
        /// <summary>
        /// PrintNo
        /// </summary>
        /// <returns></returns>
        [DisplayName("PrintNo")]
        public int? PrintNo { get; set; }
        /// <summary>
        /// 铭牌是否打印
        /// </summary>
        /// <returns></returns>
        [DisplayName("铭牌是否打印")]
        public int? Nameplate { get; set; }
        /// <summary>
        /// SequenceNo
        /// </summary>
        /// <returns></returns>
        [DisplayName("SequenceNo")]
        public string SequenceNo { get; set; }
        /// <summary>
        /// BodyNo
        /// </summary>
        /// <returns></returns>
        [DisplayName("BodyNo")]
        public string BodyNo { get; set; }
        /// <summary>
        /// MatCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("MatCd")]
        public string MatCd { get; set; }
        /// <summary>
        /// CarType
        /// </summary>
        /// <returns></returns>
        [DisplayName("CarType")]
        public string CarType { get; set; }
        /// <summary>
        /// CarColor1
        /// </summary>
        /// <returns></returns>
        [DisplayName("CarColor1")]
        public string CarColor1 { get; set; }
        /// <summary>
        /// PlanTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("PlanTime")]
        public DateTime? PlanTime { get; set; }
        /// <summary>
        /// OrderStatus
        /// </summary>
        /// <returns></returns>
        [DisplayName("OrderStatus")]
        public string OrderStatus { get; set; }
        /// <summary>
        /// PlanStatus
        /// </summary>
        /// <returns></returns>
        [DisplayName("PlanStatus")]
        public string PlanStatus { get; set; }
        /// <summary>
        /// PlanDsc
        /// </summary>
        /// <returns></returns>
        [DisplayName("PlanDsc")]
        public string PlanDsc { get; set; }
        /// <summary>
        /// PlanPublishTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("PlanPublishTime")]
        public DateTime? PlanPublishTime { get; set; }
        /// <summary>
        /// PlanPublishDate
        /// </summary>
        /// <returns></returns>
        [DisplayName("PlanPublishDate")]
        public DateTime? PlanPublishDate { get; set; }
        /// <summary>
        /// IsFile
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsFile")]
        public int? IsFile { get; set; }
        /// <summary>
        /// Enabled
        /// </summary>
        /// <returns></returns>
        [DisplayName("Enabled")]
        public string Enabled { get; set; }
        /// <summary>
        /// Rem
        /// </summary>
        /// <returns></returns>
        [DisplayName("Rem")]
        public string Rem { get; set; }
        /// <summary>
        /// RsvFld1
        /// </summary>
        /// <returns></returns>
        [DisplayName("RsvFld1")]
        public string RsvFld1 { get; set; }
        /// <summary>
        /// RsvFld2
        /// </summary>
        /// <returns></returns>
        [DisplayName("RsvFld2")]
        public string RsvFld2 { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.PublishPlanProId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.PublishPlanProId = KeyValue;
                                            }
        #endregion
    }
}