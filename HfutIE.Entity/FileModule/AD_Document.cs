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
    /// AD_Document
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.01.27 09:32</date>
    /// </author>
    /// </summary>
    [Description("AD_Document")]
    [PrimaryKey("document_key")]
    public class AD_Document : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// document_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("document_key")]
        public string document_key { get; set; }
        /// <summary>
        /// document_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("document_code")]
        public string document_code { get; set; }
        /// <summary>
        /// document_name，document_format
        /// </summary>
        /// <returns></returns>
        [DisplayName("document_name")]
        public string document_name { get; set; }

        //document_format,文件的坠名称
        /// <summary>
        /// document_type
        /// </summary>
        /// <returns></returns>
        [DisplayName("document_format")]
        public string document_format { get; set; }

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
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            //this.document_key = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.document_key = KeyValue;
                                            }
        #endregion
    }
}