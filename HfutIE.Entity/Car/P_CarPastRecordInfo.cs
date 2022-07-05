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
    ///		<name>she</name>
    ///		<date>2021.09.23 11:01</date>
    /// </author>
    /// </summary>
    [Description("车身过点记录信息表")]
    [PrimaryKey("CarPastRecordId")]
    public class P_CarPastRecordInfo : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 过点信息记录主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("过点信息记录主键")]
        public string CarPastRecordId { get; set; }
        /// <summary>
        /// AVI站点主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI站点主键")]
        public string AviId { get; set; }
        /// <summary>
        /// AVI站点编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI站点编号")]
        public string AviCd { get; set; }
        /// <summary>
        /// AVI站点名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI站点名称")]
        public string AviNm { get; set; }
        /// <summary>
        /// AVI站点类别
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI站点类别")]
        public string AviCatg { get; set; }
        /// <summary>
        /// AVI站点类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI站点类型")]
        public string AviType { get; set; }
        /// <summary>
        /// 车身号
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身号")]
        public string BodyNo { get; set; }
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
        /// 车身罩光颜色
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身罩光颜色")]
        public string CarColor2 { get; set; }
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
        /// 总装生产计划编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("总装生产计划编号")]
        public string ProducePlanCd { get; set; }
        /// <summary>
        /// 车身去向
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身去向")]
        public int? CarRoute { get; set; }
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
        /// 过点类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("过点类型")]
        public int? PastType { get; set; }
        /// <summary>
        /// 过点日期
        /// </summary>
        /// <returns></returns>
        [DisplayName("过点日期")]
        public string PastDate { get; set; }
        /// <summary>
        /// 过点时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("过点时间")]
        public DateTime? PastTime { get; set; }
        /// <summary>
        /// 过点顺序
        /// </summary>
        /// <returns></returns>
        [DisplayName("过点顺序")]
        public string PastNo { get; set; }
        /// <summary>
        /// 录入类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("录入类型")]
        public int? RecordType { get; set; }
        /// <summary>
        /// 特殊标识
        /// </summary>
        /// <returns></returns>
        [DisplayName("特殊标识")]
        public int? SpecialTag { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("录入时间")]
        public DateTime? CreTm { get; set; }
        /// <summary>
        /// 录入人员
        /// </summary>
        /// <returns></returns>
        [DisplayName("录入人员")]
        public string CreStaff { get; set; }
        /// <summary>
        /// 是否反馈至工厂FAS系统
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否反馈至工厂FAS系统")]
        public int? IsBack { get; set; }
        
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

        //[DisplayName("去向AVIId")]
        //public string ToAVIId { get; set; }

        //[DisplayName("去向AVICd")]
        //public string ToAVICd { get; set; }

        //[DisplayName("去向AVINm")]
        //public string ToAVINm { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.CarPastRecordId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.CarPastRecordId = KeyValue;
                                            }
        #endregion
    }
}