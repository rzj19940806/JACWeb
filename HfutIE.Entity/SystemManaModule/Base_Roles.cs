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
    /// Base_Roles
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.25 13:41</date>
    /// </author>
    /// </summary>
    [Description("Base_Roles3")]
    [PrimaryKey("RoleId")]
    public class Base_Roles : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// RoleId
        /// </summary>
        /// <returns></returns>
        [DisplayName("RoleId")]
        public string RoleId { get; set; }
        /// <summary>
        /// RoleCatg
        /// </summary>
        /// <returns></returns>
        [DisplayName("RoleCatg")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RoleCatg { get; set; }
        /// <summary>
        /// RoleTyp
        /// </summary>
        /// <returns></returns>
        [DisplayName("RoleTyp")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RoleTyp { get; set; }
        /// <summary>
        /// RoleCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("RoleCd")]
        public string RoleCd { get; set; }
        /// <summary>
        /// RoleNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("RoleNm")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RoleNm { get; set; }
        /// <summary>
        /// SortCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("SortCode")]
        public int? SortCode { get; set; }
        /// <summary>
        /// DeleteMark
        /// </summary>
        /// <returns></returns>
        [DisplayName("DeleteMark")]
        public int? DeleteMark { get; set; }
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
        public string Rem { get; set; }
        [DisplayName("预留字段1")]
        public string RsvFld1 { get; set; }
        [DisplayName("预留字段2")]
        public string RsvFld2 { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.RoleId = CommonHelper.GetGuid;
            this.CreTm = DateTime.Now;
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
            this.Enabled = "1";
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.RoleId = KeyValue;
            this.MdfTm = DateTime.Now;
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}