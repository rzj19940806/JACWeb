//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2018
// Software Developers @ HfutIE 2018
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
    /// AD_User
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.02.02 09:29</date>
    /// </author>
    /// </summary>
    [Description("AD_User")]
    [PrimaryKey("UserId")]
    public class AD_User : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// UserId
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserId")]
        public string UserId { get; set; }
        /// <summary>
        /// CompanyId
        /// </summary>
        /// <returns></returns>
        [DisplayName("CompanyId")]
        public string CompanyId { get; set; }
        /// <summary>
        /// DepartmentId
        /// </summary>
        /// <returns></returns>
        [DisplayName("DepartmentId")]
        public string DepartmentId { get; set; }
        /// <summary>
        /// InnerUser
        /// </summary>
        /// <returns></returns>
        [DisplayName("InnerUser")]
        public int? InnerUser { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        /// <returns></returns>
        [DisplayName("Code")]
        public string Code { get; set; }
        /// <summary>
        /// Account
        /// </summary>
        /// <returns></returns>
        [DisplayName("Account")]
        public string Account { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        /// <returns></returns>
        [DisplayName("Password")]
        public string Password { get; set; }
        /// <summary>
        /// Secretkey
        /// </summary>
        /// <returns></returns>
        [DisplayName("Secretkey")]
        public string Secretkey { get; set; }
        /// <summary>
        /// RealName
        /// </summary>
        /// <returns></returns>
        [DisplayName("RealName")]
        public string RealName { get; set; }
        /// <summary>
        /// Spell
        /// </summary>
        /// <returns></returns>
        [DisplayName("Spell")]
        public string Spell { get; set; }
        /// <summary>
        /// Gender
        /// </summary>
        /// <returns></returns>
        [DisplayName("Gender")]
        public string Gender { get; set; }
        /// <summary>
        /// Birthday
        /// </summary>
        /// <returns></returns>
        [DisplayName("Birthday")]
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// Mobile
        /// </summary>
        /// <returns></returns>
        [DisplayName("Mobile")]
        public string Mobile { get; set; }
        /// <summary>
        /// Telephone
        /// </summary>
        /// <returns></returns>
        [DisplayName("Telephone")]
        public string Telephone { get; set; }
        /// <summary>
        /// OICQ
        /// </summary>
        /// <returns></returns>
        [DisplayName("OICQ")]
        public string OICQ { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        /// <returns></returns>
        [DisplayName("Email")]
        public string Email { get; set; }
        /// <summary>
        /// ChangePasswordDate
        /// </summary>
        /// <returns></returns>
        [DisplayName("ChangePasswordDate")]
        public DateTime? ChangePasswordDate { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        /// <returns></returns>
        [DisplayName("OpenId")]
        public int? OpenId { get; set; }
        /// <summary>
        /// LogOnCount
        /// </summary>
        /// <returns></returns>
        [DisplayName("LogOnCount")]
        public int? LogOnCount { get; set; }
        /// <summary>
        /// FirstVisit
        /// </summary>
        /// <returns></returns>
        [DisplayName("FirstVisit")]
        public DateTime? FirstVisit { get; set; }
        /// <summary>
        /// PreviousVisit
        /// </summary>
        /// <returns></returns>
        [DisplayName("PreviousVisit")]
        public DateTime? PreviousVisit { get; set; }
        /// <summary>
        /// LastVisit
        /// </summary>
        /// <returns></returns>
        [DisplayName("LastVisit")]
        public DateTime? LastVisit { get; set; }
        /// <summary>
        /// AuditStatus
        /// </summary>
        /// <returns></returns>
        [DisplayName("AuditStatus")]
        public string AuditStatus { get; set; }
        /// <summary>
        /// AuditUserId
        /// </summary>
        /// <returns></returns>
        [DisplayName("AuditUserId")]
        public string AuditUserId { get; set; }
        /// <summary>
        /// AuditUserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("AuditUserName")]
        public string AuditUserName { get; set; }
        /// <summary>
        /// AuditDateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("AuditDateTime")]
        public DateTime? AuditDateTime { get; set; }
        /// <summary>
        /// Online
        /// </summary>
        /// <returns></returns>
        [DisplayName("Online")]
        public int? Online { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        /// <returns></returns>
        [DisplayName("Remark")]
        public string Remark { get; set; }
        /// <summary>
        /// Enabled
        /// </summary>
        /// <returns></returns>
        [DisplayName("Enabled")]
        public int? Enabled { get; set; }
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
        /// CreateDate
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateDate")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// CreateUserId
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateUserId")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// CreateUserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateUserName")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// ModifyDate
        /// </summary>
        /// <returns></returns>
        [DisplayName("ModifyDate")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// ModifyUserId
        /// </summary>
        /// <returns></returns>
        [DisplayName("ModifyUserId")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// ModifyUserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("ModifyUserName")]
        public string ModifyUserName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.UserId = CommonHelper.GetGuid;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = ManageProvider.Provider.Current().UserId;
            this.CreateUserName = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.UserId = KeyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}