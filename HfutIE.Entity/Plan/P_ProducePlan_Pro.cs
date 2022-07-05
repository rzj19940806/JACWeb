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
    /// 完整计划接收过程表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.29 10:48</date>
    /// </author>
    /// </summary>
    [Description("完整计划接收过程表")]
    [PrimaryKey("PlanProId")]
    public class P_ProducePlan_Pro : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 计划过程主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("计划过程主键")]
        public string PlanProId { get; set; }
        /// <summary>
        /// 计划编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("计划编号")]
        public string ProducePlanCd { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单号")]
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
        public string SequenceNo { get; set; }
        /// <summary>
        /// 车身号
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身号")]
        public string BodyNo { get; set; }
        /// <summary>
        /// 车身物料号
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身物料号")]
        public string MatCd { get; set; }
        /// <summary>
        /// 车型
        /// </summary>
        /// <returns></returns>
        [DisplayName("车型")]
        public string CarType { get; set; }
        /// <summary>
        /// 车身面漆颜色
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身面漆颜色")]
        public string CarColor1 { get; set; }
        /// <summary>
        /// 车身罩光颜色
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身罩光颜色")]
        public string CarColor2 { get; set; }
        /// <summary>
        /// 预计上线日期
        /// </summary>
        /// <returns></returns>
        [DisplayName("预计上线日期")]
        public DateTime? PlanTime { get; set; }
        /// <summary>
        /// 获取方式
        /// </summary>
        /// <returns></returns>
        [DisplayName("获取方式")]
        public int? PlanType { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单状态")]
        public int? OrderStatus { get; set; }
        /// <summary>
        /// 计划状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("计划状态")]
        public int? PlanStatus { get; set; }
        /// <summary>
        /// 计划描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("计划描述")]
        public string PlanDsc { get; set; }
        /// <summary>
        /// 接收时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("接收时间")]
        public DateTime? RecieveTm { get; set; }
        /// <summary>
        /// 是否转档
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否转档")]
        public int? IsFile { get; set; }
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
        public string Remarks { get; set; }
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
            this.PlanProId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.PlanProId = KeyValue;
                                            }
        #endregion
    }
}