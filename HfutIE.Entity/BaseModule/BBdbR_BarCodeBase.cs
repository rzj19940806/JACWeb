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
    /// BBdbR_BarCodeBase
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.13 14:41</date>
    /// </author>
    /// </summary>
    [Description("BBdbR_BarCodeBase")]
    [PrimaryKey("BarId")]
    public class BBdbR_BarCodeBase : BaseEntity
    {
        #region 获取/设置 字段值
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
        /// BarNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("BarNm")]
        public string BarNm { get; set; }
        /// <summary>
        /// BarLength
        /// </summary>
        /// <returns></returns>
        [DisplayName("BarLength")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? BarLength { get; set; }
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
        /// Rem
        /// </summary>
        /// <returns></returns>
        [DisplayName("Rem")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Rem { get; set; }
        /// <summary>
        /// RsvFld1
        /// </summary>
        /// <returns></returns>
        [DisplayName("RsvFld1")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
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
            this.BarId = CommonHelper.GetGuid;
            this.CreCd = ManageProvider.Provider.Current().Code;
            this.CreNm = ManageProvider.Provider.Current().UserName;
            this.CreTm = DateTime.Now;
            this.Enabled = "1";
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.BarId = KeyValue;
            this.MdfCd = ManageProvider.Provider.Current().Code;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
            this.MdfTm = DateTime.Now;
        }
        #endregion
    }
}