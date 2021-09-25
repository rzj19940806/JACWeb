//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2017
// Software Developers @ HfutIE 2017
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
    /// INTERFACE_FIELD
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.06.29 19:29</date>
    /// </author>
    /// </summary>
    [Description("INTERFACE_FIELD")]
    [PrimaryKey("interface_field_key")]
    public class INTERFACE_FIELD : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// interface_field_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("interface_field_key")]
        public string interface_field_key { get; set; }
        /// <summary>
        /// interface_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("interface_key")]
        public string interface_key { get; set; }
        /// <summary>
        /// interface_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("interface_code")]
        public string interface_code { get; set; }
        /// <summary>
        /// interface_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("interface_name")]
        public string interface_name { get; set; }
        /// <summary>
        /// from_table_field
        /// </summary>
        /// <returns></returns>
        [DisplayName("from_table_field")]
        public string from_table_field { get; set; }
        /// <summary>
        /// to_table_field
        /// </summary>
        /// <returns></returns>
        [DisplayName("to_table_field")]
        public string to_table_field { get; set; }
        /// <summary>
        /// is_identify_column
        /// </summary>
        /// <returns></returns>
        [DisplayName("is_identify_column")]
        public string is_identify_column { get; set; }
        /// <summary>
        /// reserve4
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve4")]
        public string reserve4 { get; set; }
        /// <summary>
        /// reserve5
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve5")]
        public string reserve5 { get; set; }
        /// <summary>
        /// reserve6
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve6")]
        public string reserve6 { get; set; }
        /// <summary>
        /// reserve7
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve7")]
        public string reserve7 { get; set; }
        /// <summary>
        /// reserve8
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve8")]
        public string reserve8 { get; set; }
        /// <summary>
        /// reserve9
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve9")]
        public string reserve9 { get; set; }
        /// <summary>
        /// reserve10
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve10")]
        public string reserve10 { get; set; }
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
            this.interface_field_key = CommonHelper.GetGuid;
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
            this.interface_field_key = KeyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}