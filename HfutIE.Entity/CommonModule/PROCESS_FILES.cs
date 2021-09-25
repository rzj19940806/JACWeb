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
    /// PROCESS_FILES
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.08.04 19:57</date>
    /// </author>
    /// </summary>
    [Description("PROCESS_FILES")]
    [PrimaryKey("document_key")]
    public class PROCESS_FILES : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// document_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("document_key")]
        public string document_key { get; set; }
        /// <summary>
        /// site_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("site_key")]
        public string site_key { get; set; }
        /// <summary>
        /// site_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("site_code")]
        public string site_code { get; set; }
        /// <summary>
        /// site_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("site_name")]
        public string site_name { get; set; }
        /// <summary>
        /// area_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("area_key")]
        public string area_key { get; set; }
        /// <summary>
        /// area_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("area_code")]
        public string area_code { get; set; }
        /// <summary>
        /// area_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("area_name")]
        public string area_name { get; set; }
        /// <summary>
        /// ws_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("ws_key")]
        public string ws_key { get; set; }
        /// <summary>
        /// ws_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("ws_code")]
        public string ws_code { get; set; }
        /// <summary>
        /// ws_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("ws_name")]
        public string ws_name { get; set; }
        /// <summary>
        /// p_line_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("p_line_key")]
        public string p_line_key { get; set; }
        /// <summary>
        /// p_line_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("p_line_code")]
        public string p_line_code { get; set; }
        /// <summary>
        /// p_line_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("p_line_name")]
        public string p_line_name { get; set; }
        /// <summary>
        /// wc_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("wc_key")]
        public string wc_key { get; set; }
        /// <summary>
        /// wc_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("wc_code")]
        public string wc_code { get; set; }
        /// <summary>
        /// wc_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("wc_name")]
        public string wc_name { get; set; }
        /// <summary>
        /// product_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("product_key")]
        public string product_key { get; set; }
        /// <summary>
        /// product_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("product_code")]
        public string product_code { get; set; }
        /// <summary>
        /// product_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("product_name")]
        public string product_name { get; set; }
        /// <summary>
        /// document_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("document_code")]
        public string document_code { get; set; }
        /// <summary>
        /// document_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("document_name")]
        public string document_name { get; set; }
        /// <summary>
        /// document_type
        /// </summary>
        /// <returns></returns>
        [DisplayName("document_type")]
        public string document_type { get; set; }
        /// <summary>
        /// document_edition
        /// </summary>
        /// <returns></returns>
        [DisplayName("document_edition")]
        public string document_edition { get; set; }
        /// <summary>
        /// document_size
        /// </summary>
        /// <returns></returns>
        [DisplayName("document_size")]
        public string document_size { get; set; }
        /// <summary>
        /// document_file
        /// </summary>
        /// <returns></returns>
        [DisplayName("document_file")]
        public byte[] document_file { get; set; }
        /// <summary>
        /// upload_PC_Mac
        /// </summary>
        /// <returns></returns>
        [DisplayName("upload_PC_Mac")]
        public string upload_PC_Mac { get; set; }
        /// <summary>
        /// document_storage_path
        /// </summary>
        /// <returns></returns>
        [DisplayName("document_storage_path")]
        public string document_storage_path { get; set; }
        /// <summary>
        /// upload_PC_IP
        /// </summary>
        /// <returns></returns>
        [DisplayName("upload_PC_IP")]
        public string upload_PC_IP { get; set; }
        /// <summary>
        /// upload_PC_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("upload_PC_name")]
        public string upload_PC_name { get; set; }
        /// <summary>
        /// remarks
        /// </summary>
        /// <returns></returns>
        [DisplayName("remarks")]
        public string remarks { get; set; }
        /// <summary>
        /// reserve01
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve01")]
        public string reserve01 { get; set; }
        /// <summary>
        /// reserve02
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve02")]
        public string reserve02 { get; set; }
        /// <summary>
        /// reserve03
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve03")]
        public string reserve03 { get; set; }
        /// <summary>
        /// reserve04
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve04")]
        public string reserve04 { get; set; }
        /// <summary>
        /// reserve05
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve05")]
        public string reserve05 { get; set; }
        /// <summary>
        /// reserve06
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve06")]
        public string reserve06 { get; set; }
        /// <summary>
        /// reserve07
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve07")]
        public string reserve07 { get; set; }
        /// <summary>
        /// reserve08
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve08")]
        public string reserve08 { get; set; }
        /// <summary>
        /// reserve09
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve09")]
        public string reserve09 { get; set; }
        /// <summary>
        /// reserve10
        /// </summary>
        /// <returns></returns>
        [DisplayName("reserve10")]
        public string reserve10 { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建用户主键")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建用户")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改时间")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改用户主键")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改用户")]
        public string ModifyUserName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.document_key = CommonHelper.GetGuid;
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
            this.document_key = KeyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}