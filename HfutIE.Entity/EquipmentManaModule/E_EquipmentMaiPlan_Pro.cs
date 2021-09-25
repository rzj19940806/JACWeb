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
    /// E_EquipmentMaiPlan_Pro
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.16 11:27陈克利</date>
    /// </author>
    /// </summary>
    [Description("E_EquipmentMaiPlan_Pro")]
    [PrimaryKey("MaiPlanId")]
    public class E_EquipmentMaiPlan_Pro : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// MaiPlanId
        /// </summary>
        /// <returns></returns>
        [DisplayName("MaiPlanId")]
        public string MaiPlanId { get; set; }
        /// <summary>
        /// MaiPlanCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("MaiPlanCode")]
        public string MaiPlanCode { get; set; }
        /// <summary>
        /// MaiPlanName
        /// </summary>
        /// <returns></returns>
        [DisplayName("MaiPlanName")]
        public string MaiPlanName { get; set; }
        /// <summary>
        /// DvcCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("DvcCd")]
        public string DvcCd { get; set; }
        /// <summary>
        /// DvcNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("DvcNm")]
        public string DvcNm { get; set; }
        /// <summary>
        /// CarPartId
        /// </summary>
        /// <returns></returns>
        [DisplayName("CarPartId")]
        public string CarPartId { get; set; }
        /// <summary>
        /// MaiPlanCatg
        /// </summary>
        /// <returns></returns>
        [DisplayName("MaiPlanCatg")]
        public string MaiPlanCatg { get; set; }
        /// <summary>
        /// MaiPlanType
        /// </summary>
        /// <returns></returns>
        [DisplayName("MaiPlanType")]
        public string MaiPlanType { get; set; }
        /// <summary>
        /// MaiPlanCon
        /// </summary>
        /// <returns></returns>
        [DisplayName("MaiPlanCon")]
        public string MaiPlanCon { get; set; }
        /// <summary>
        /// MaiPlanStandard
        /// </summary>
        /// <returns></returns>
        [DisplayName("MaiPlanStandard")]
        public string MaiPlanStandard { get; set; }
        /// <summary>
        /// MaiPlanCycle
        /// </summary>
        /// <returns></returns>
        [DisplayName("MaiPlanCycle")]
        public string MaiPlanCycle { get; set; }
        /// <summary>
        /// MaiPlanSta
        /// </summary>
        /// <returns></returns>
        [DisplayName("MaiPlanSta")]
        public string MaiPlanSta { get; set; }
        /// <summary>
        /// PlineId
        /// </summary>
        /// <returns></returns>
        [DisplayName("PlineId")]
        public string PlineId { get; set; }
        /// <summary>
        /// StfId
        /// </summary>
        /// <returns></returns>
        [DisplayName("StfId")]
        public string StfId { get; set; }
        /// <summary>
        /// Frequency
        /// </summary>
        /// <returns></returns>
        [DisplayName("Frequency")]
        public string Frequency { get; set; }
        /// <summary>
        /// MaiPlanPer
        /// </summary>
        /// <returns></returns>
        [DisplayName("MaiPlanPer")]
        public string MaiPlanPer { get; set; }
        /// <summary>
        /// MaiPlanTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("MaiPlanTime")]
        public string MaiPlanTime { get; set; }
        /// <summary>
        /// MaiPlanTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreTm")]
        public DateTime? CreTm { get; set; }
        /// <summary>
        /// CreCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreCd")]
        public string CreCd { get; set; }
        /// <summary>
        /// CreNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreNm")]
        public string CreNm { get; set; }
        /// <summary>
        /// MdfTm
        /// </summary>
        /// <returns></returns>
        [DisplayName("MdfTm")]
        public DateTime? MdfTm { get; set; }
        /// <summary>
        /// MdfCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("MdfCd")]
        public string MdfCd { get; set; }
        /// <summary>
        /// MdfNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("MdfNm")]
        public string MdfNm { get; set; }
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
            this.MaiPlanId = CommonHelper.GetGuid;
            this.CreTm = DateTime.Now;
            this.Enabled = "1";
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.MaiPlanId = KeyValue;
            this.MdfTm = DateTime.Now;
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}