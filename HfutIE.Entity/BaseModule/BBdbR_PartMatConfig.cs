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
using System.Drawing;
using System.Web.UI.WebControls;

namespace HfutIE.Entity
{
    /// <summary>
    /// 物料基础信息
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.04.02 16:27</date>
    /// </author>
    /// </summary>
    [Description("主键")]
    [PrimaryKey("Id")]
    public class BBdbR_PartMatConfig : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string Id { get; set; }
        /// <summary>
        /// 组件ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("组件ID")]
        public string PartId { get; set; }
        /// <summary>
        /// 组件编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("组件编号")]
        public string PartCd { get; set; }
        /// <summary>
        /// 组件名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("组件名称")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PartNm { get; set; }
        /// <summary>
        /// 物料编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("物料编号")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MatCd { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("物料名称")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MatNm { get; set; }
       
        /// <summary>
        /// 物料类别
        /// </summary>
        /// <returns></returns>
        [DisplayName("物料类别")]
        public string MatCatg { get; set; }
        /// <summary>
        /// 物料类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("物料类型")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MatTyp { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        /// <returns></returns>
        [DisplayName("规格型号")]
        public string MatSpec { get; set; }
        /// <summary>
        /// IsScan
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsScan")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string IsScan { get; set; }
        /// <summary>
        /// WcId
        /// </summary>
        /// <returns></returns>
        [DisplayName("WcId")]
        public string WcId { get; set; }
        /// <summary>
        /// WcCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("WcCd")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string WcCd { get; set; }
        /// <summary>
        /// WcNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("WcNm")]
        public string WcNm { get; set; }
        /// <summary>
        /// MatNum
        /// </summary>
        /// <returns></returns>
        [DisplayName("MatNum")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MatNum { get; set; }
        /// <summary>
        /// IsPrint
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsPrint")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string IsPrint { get; set; }
        /// <summary>
        /// ShortCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("ShortCode")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ShortCode { get; set; }
        /// <summary>
        /// Unit
        /// </summary>
        /// <returns></returns>
        [DisplayName("Unit")]
        public string Unit { get; set; }
       
        /// <summary>
        /// 版本号
        /// </summary>
        /// <returns></returns>
        [DisplayName("版本号")]
        public string VersionNumber { get; set; }
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
        public DateTime? CreTm { get; set; }
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
        public DateTime? MdfTm { get; set; }
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
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Rem { get; set; }
        /// <summary>
        /// 预留字段1
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段1")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RsvFld1 { get; set; }
        /// <summary>
        /// 预留字段2
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段2")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RsvFld2 { get; set; }
       
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = CommonHelper.GetGuid;
            this.VersionNumber = "V1.0";
            this.CreTm = DateTime.Now;
            this.Enabled = "1";
            this.CreCd = ManageProvider.Provider.Current().Code;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public void Modify()
        {
            this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now;
            this.MdfCd = ManageProvider.Provider.Current().Code;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}