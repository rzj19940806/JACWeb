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
    /// BBdbR_DecodeRule
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.14 19:38</date>
    /// </author>
    /// </summary>
    [Description("BBdbR_DecodeRule")]
    [PrimaryKey("DecodeRulesId")]
    public class BBdbR_DecodeRule : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// DecodeRulesId
        /// </summary>
        /// <returns></returns>
        [DisplayName("DecodeRulesId")]
        public string DecodeRulesId { get; set; }
        /// <summary>
        /// BarId
        /// </summary>
        /// <returns></returns>
        [DisplayName("BarId")]
        public string BarId { get; set; }
        /// <summary>
        /// BarCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("BarCd")]
        public string BarCd { get; set; }
        /// <summary>
        /// CombineNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("CombineNm")]
        public string CombineNm { get; set; }
        /// <summary>
        /// CombineStart
        /// </summary>
        /// <returns></returns>
        [DisplayName("CombineStart")]
        public int? CombineStart { get; set; }
        /// <summary>
        /// CombineLength
        /// </summary>
        /// <returns></returns>
        [DisplayName("CombineLength")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? CombineLength { get; set; }
        /// <summary>
        /// VersionNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("VersionNumber")]
        public string VersionNumber { get; set; }
        /// <summary>
        /// Enabled
        /// </summary>
        /// <returns></returns>
        [DisplayName("Enabled")]
        public string Enabled { get; set; }
        /// <summary>
        /// CreTm
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public DateTime? CreTm { get; set; }
        /// <summary>
        /// CreCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建人编号")]
        public string CreCd { get; set; }
        /// <summary>
        /// CreNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建人名称")]
        public string CreNm { get; set; }
        /// <summary>
        /// MdfTm
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改时间")]
        public DateTime? MdfTm { get; set; }
        /// <summary>
        /// MdfCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人编号")]
        public string MdfCd { get; set; }
        /// <summary>
        /// MdfNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人名称")]
        public string MdfNm { get; set; }
        /// <summary>
        /// Rem
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Rem { get; set; }
        /// <summary>
        /// RsvFld1
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段1")]
        public string RsvFld1 { get; set; }
        /// <summary>
        /// RsvFld2
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
            this.DecodeRulesId = CommonHelper.GetGuid;
            this.Enabled = "1";
            this.CreCd = ManageProvider.Provider.Current().Code;
            this.CreNm = ManageProvider.Provider.Current().UserName;
            this.CreTm = DateTime.Now;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.DecodeRulesId = KeyValue;
            this.MdfTm = DateTime.Now;
            this.MdfCd = ManageProvider.Provider.Current().Code;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}
