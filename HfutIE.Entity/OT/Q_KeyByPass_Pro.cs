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
    /// 关重件绑定过程表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.26 00:49</date>
    /// </author>
    /// </summary>
    [Description("关重件ByPass过程表")]
    [PrimaryKey("KeyByPassProId")]
    public class Q_KeyByPass_Pro : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 关重件绑定过程主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("关重件ByPass过程主键")]
        public string KeyByPassProId { get; set; }
        /// <summary>
        /// VIN码
        /// </summary>
        /// <returns></returns>
        [DisplayName("VIN码")]
        public string VIN { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单号")]
        public string OrderCd { get; set; }
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
        /// 整车物料代码
        /// </summary>
        /// <returns></returns>
        [DisplayName("整车物料代码")]
        public string ProductMatCd { get; set; }
        /// <summary>
        /// 车型
        /// </summary>
        /// <returns></returns>
        [DisplayName("车型")]
        public string CarType { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        /// <returns></returns>
        [DisplayName("颜色")]
        public string CarColor1 { get; set; }
        /// <summary>
        /// 关重件条码
        /// </summary>
        /// <returns></returns>
        [DisplayName("关重件条码")]
        public string BarCode { get; set; }
        /// <summary>
        /// 关重件物料主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("关重件物料主键")]
        public string MatId { get; set; }
        /// <summary>
        /// 关重件物料编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("关重件物料编号")]
        public string MatCd { get; set; }
        /// <summary>
        /// 关重件物料名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("关重件物料名称")]
        public string MatNm { get; set; }
        /// <summary>
        /// 关重件供应商主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("关重件供应商主键")]
        public string SupplierId { get; set; }
        /// <summary>
        /// 关重件供应商编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("关重件供应商编号")]
        public string SupplierCd { get; set; }
        /// <summary>
        /// 关重件供应商名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("关重件供应商名称")]
        public string SupplierNm { get; set; }
        /// <summary>
        /// 公司主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("公司主键")]
        public string CompanyId { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("公司编号")]
        public string CompanyCd { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("公司名称")]
        public string CompanyNm { get; set; }
        /// <summary>
        /// 工厂主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("工厂主键")]
        public string FacId { get; set; }
        /// <summary>
        /// 工厂编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("工厂编号")]
        public string FacCd { get; set; }
        /// <summary>
        /// 工厂名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("工厂名称")]
        public string FacNm { get; set; }
        /// <summary>
        /// 车间主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("车间主键")]
        public string WorkshopId { get; set; }
        /// <summary>
        /// 车间编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("车间编号")]
        public string WorkshopCd { get; set; }
        /// <summary>
        /// 车间名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("车间名称")]
        public string WorkshopNm { get; set; }
        /// <summary>
        /// 工段主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("工段主键")]
        public string WorkSectionId { get; set; }
        /// <summary>
        /// 工段编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("工段编号")]
        public string WorkSectionCd { get; set; }
        /// <summary>
        /// 工段名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("工段名称")]
        public string WorkSectionNm { get; set; }
        /// <summary>
        /// 产线主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线主键")]
        public string PlineId { get; set; }
        /// <summary>
        /// 产线编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线编号")]
        public string PlineCd { get; set; }
        /// <summary>
        /// 产线名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线名称")]
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
        /// 岗位主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("岗位主键")]
        public string PostId { get; set; }
        /// <summary>
        /// 岗位编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("岗位编号")]
        public string PostCd { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("岗位名称")]
        public string PostNm { get; set; }
        /// <summary>
        /// 班组主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("班组主键")]
        public string TeamId { get; set; }
        /// <summary>
        /// 班组编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("班组编号")]
        public string TeamCd { get; set; }
        /// <summary>
        /// 班组名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("班组名称")]
        public string TeamNm { get; set; }
        /// <summary>
        /// 班组负责人主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("班组负责人主键")]
        public string MgrId { get; set; }
        /// <summary>
        /// 班组负责人编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("班组负责人编号")]
        public string MgrCd { get; set; }
        /// <summary>
        /// 班组负责人名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("班组负责人名称")]
        public string MgrNm { get; set; }
        /// <summary>
        /// 人员主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("人员主键")]
        public string StfId { get; set; }
        /// <summary>
        /// 人员编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("人员编号")]
        public string StfCd { get; set; }
        /// <summary>
        /// 人员名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("人员名称")]
        public string StfNm { get; set; }
        /// <summary>
        /// 采集时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("采集时间")]
        public DateTime? Datetime { get; set; }
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
            KeyByPassProId = CommonHelper.GetGuid;
            Datetime = DateTime.Now;
            isFile = 0;
            Enabled = "1";
        }
        #endregion
    }
}