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
    /// 车身过点记录信息表
    /// <author>
    ///		<name>ckl</name>
    ///		<date>2021.09.23 11:01</date>
    /// </author>
    /// </summary>
    [Description("报工反馈记录表")]
    [PrimaryKey("PlanFeedBackProId")]
    public class P_PlanFeedBack_Pro : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string PlanFeedBackProId { get; set; }
        /// <summary>
        /// 总装生产计划编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("总装生产计划编号")]
        public string ProducePlanCd { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        /// <returns></returns>
        [DisplayName("工序编码")]
        public string OP_CODE { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("工序名称")]
        public string OP_NAME { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改时间")]
        public string MODIFY_Time { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人")]
        public string MODIFY_ID { get; set; }
        /// <summary>
        /// 过点反馈时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("过点反馈时间")]
        public DateTime? FeedbackTime { get; set; }
        
        /// <summary>
        /// OrderCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("OrderCd")]
        public string OrderCd { get; set; }
        /// <summary>
        /// VIN码
        /// </summary>
        /// <returns></returns>
        [DisplayName("VIN码")]
        public string VIN { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        /// <returns></returns>
        [DisplayName("流水号")]
        public int? SequenceNo { get; set; }
        /// <summary>
        /// 车身号
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身号")]
        public int? BodyNo { get; set; }
        /// <summary>
        /// 车型
        /// </summary>
        /// <returns></returns>
        [DisplayName("车型")]
        public string CarType { get; set; }
        /// <summary>
        /// 车身物料号
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身物料号")]
        public string MatCd { get; set; }
        /// <summary>
        /// 车身面漆颜色
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身面漆颜色")]
        public string CarColor1 { get; set; }
        /// <summary>
        /// OrderStatus
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单状态")]
        public int? OrderStatus { get; set; }
        /// <summary>
        /// PlanStatus
        /// </summary>
        /// <returns></returns>
        [DisplayName("计划状态")]
        public string PlanStatus { get; set; }
        /// <summary>
        /// PlanDsc
        /// </summary>
        /// <returns></returns>
        [DisplayName("PlanDsc")]
        public string PlanDsc { get; set; }
        /// <summary>
        /// AVI站点主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI站点主键")]
        public string AviId { get; set; }
        ///// <summary>
        ///// AVI站点编号
        ///// </summary>
        ///// <returns></returns>
        //[DisplayName("AVI站点编号")]
        //public string AviCd { get; set; }
        ///// <summary>
        ///// AVI站点名称
        ///// </summary>
        ///// <returns></returns>
        //[DisplayName("AVI站点名称")]
        //public string AviNm { get; set; }
       
        /// <summary>
        /// 车身去向产线主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身去向产线主键")]
        public string PlineId { get; set; }
        /// <summary>
        /// 车身去向产线编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身去向产线编号")]
        public string PlineCd { get; set; }
        /// <summary>
        /// 车身去向产线名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身去向产线名称")]
        public string PlineNm { get; set; }
        /// <summary>
        /// 工位主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位主键")]
        public string WcId { get; set; }
        /// <summary>
        /// 工位编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位编号")]
        public string WcCd { get; set; }
        /// <summary>
        /// 工位名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位名称")]
        public string WcNm { get; set; }
        /// <summary>
        /// IsFile
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsFile")]
        public int? IsFile { get; set; }
       
        
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
            this.PlanFeedBackProId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.PlanFeedBackProId = KeyValue;
                                            }
        #endregion
    }
}