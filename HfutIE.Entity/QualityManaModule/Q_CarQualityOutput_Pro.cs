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
    /// 车身质量检查销项过程表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.12.25 10:08</date>
    /// </author>
    /// </summary>
    [Description("车身质量检查销项过程表")]
    [PrimaryKey("CarInspectionOutputId")]
    public class Q_CarQualityOutput_Pro : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 车身质检销项主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身质检销项主键")]
        public string CarInspectionOutputId { get; set; }
        /// <summary>
        /// 车身部位质检主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身部位质检主键")]
        public string CarQualityInspectionId { get; set; }
        /// <summary>
        /// 质控点组主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("质控点组主键")]
        public string QualityCheckPointGroupId { get; set; }
        /// <summary>
        /// 质控点组编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("质控点组编号")]
        public string QualityCheckPointGroupCd { get; set; }
        /// <summary>
        /// 质控点组名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("质控点组名称")]
        public string QualityCheckPointGroupNm { get; set; }
        /// <summary>
        /// 质控点主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("质控点主键")]
        public string QualityCheckPointId { get; set; }
        /// <summary>
        /// 质控点编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("质控点编号")]
        public string QualityCheckPointCd { get; set; }
        /// <summary>
        /// 质控点名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("质控点名称")]
        public string QualityCheckPointNm { get; set; }
        /// <summary>
        /// 车身号
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身号")]
        public string VIN { get; set; }
        /// <summary>
        /// 车身组件主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身组件主键")]
        public string CarComponentId { get; set; }
        /// <summary>
        /// 车身方位主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身方位主键")]
        public string CarPositionId { get; set; }
        /// <summary>
        /// 车身方位分组主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身方位分组主键")]
        public string CarPositionGroupId { get; set; }
        /// <summary>
        /// 车身组件最终编码
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身组件最终编码")]
        public string CarComponentCd { get; set; }
        /// <summary>
        /// 车身组件名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身组件名称")]
        public string CarComponentNm { get; set; }
        /// <summary>
        /// 车身组件简码
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身组件简码")]
        public string CarComponentSimpleCd { get; set; }
        /// <summary>
        /// 缺陷主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("缺陷主键")]
        public string DefectId { get; set; }
        /// <summary>
        /// 缺陷类型主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("缺陷类型主键")]
        public string DefectCatgId { get; set; }
        /// <summary>
        /// 缺陷类型分组主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("缺陷类型分组主键")]
        public string DefectCatgGroupId { get; set; }
        /// <summary>
        /// 缺陷编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("缺陷编号")]
        public string DefectCd { get; set; }
        /// <summary>
        /// 缺陷名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("缺陷名称")]
        public string DefectNm { get; set; }
        /// <summary>
        /// 缺陷分析
        /// </summary>
        /// <returns></returns>
        [DisplayName("缺陷分析")]
        public string DefectAnalysis { get; set; }
        /// <summary>
        /// 检查人员编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("检查人员编号")]
        public string StfCd { get; set; }
        /// <summary>
        /// 检查人员姓名
        /// </summary>
        /// <returns></returns>
        [DisplayName("检查人员姓名")]
        public string StfNm { get; set; }
        /// <summary>
        /// 质检录入时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("质检录入时间")]
        public string QualityInspectTm { get; set; }
        /// <summary>
        /// 维修人员编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("维修人员编号")]
        public string WStfCd { get; set; }
        /// <summary>
        /// 维修人员姓名
        /// </summary>
        /// <returns></returns>
        [DisplayName("维修人员姓名")]
        public string WStfNm { get; set; }
        /// <summary>
        /// 维修时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("维修时间")]
        public string WxTm { get; set; }
        /// <summary>
        /// 销项检查项数据
        /// </summary>
        /// <returns></returns>
        [DisplayName("销项检查项数据")]
        public decimal? OutputValue { get; set; }
        /// <summary>
        /// 销项检查项结果
        /// </summary>
        /// <returns></returns>
        [DisplayName("销项检查项结果")]
        public string OutputResult { get; set; }
        /// <summary>
        /// 销项人员编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("销项人员编号")]
        public string XStfCd { get; set; }
        /// <summary>
        /// 销项人员姓名
        /// </summary>
        /// <returns></returns>
        [DisplayName("销项人员姓名")]
        public string XStfNm { get; set; }
        /// <summary>
        /// 销项时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("销项时间")]
        public string OutputTime { get; set; }
        /// <summary>
        /// 复检次数
        /// </summary>
        /// <returns></returns>
        [DisplayName("复检次数")]
        public int? ReinspectionNumber { get; set; }
        /// <summary>
        /// 历史主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("历史主键")]
        public string HistoryId { get; set; }
        /// <summary>
        /// 是否转档
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否转档")]
        public int? isFile { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效性")]
        public string Enabled { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public string CreTm { get; set; }
        /// <summary>
        /// 创建人编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建人编号")]
        public string CreCd { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建人名称")]
        public string CreNm { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改时间")]
        public string MdfTm { get; set; }
        /// <summary>
        /// 修改人编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人编号")]
        public string MdfCd { get; set; }
        /// <summary>
        /// 修改人名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人名称")]
        public string MdfNm { get; set; }
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
            this.CarInspectionOutputId = CommonHelper.GetGuid;
            this.Enabled = "1";
            this.CreTm = DateTime.Now.ToString();
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.CarInspectionOutputId = KeyValue;
            this.MdfTm = DateTime.Now.ToString();
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}