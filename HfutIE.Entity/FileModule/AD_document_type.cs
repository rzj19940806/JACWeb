//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2018
// Software Developers @ HfutIE 2018
//=====================================================================================

using HfutIE.DataAccess;
using HfutIE.DataAccess.Attributes;
using HfutIE.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;

namespace HfutIE.Entity
{
    /// <summary>
    /// AD_document_type
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.01.27 09:33</date>
    /// </author>
    /// </summary>
    [Description("AD_document_type")]
    [PrimaryKey("document_type_key")]
    public class AD_document_type : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// document_type_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("document_type_key")]
        public string document_type_key { get; set; }
        /// <summary>
        /// type_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("type_code")]
        public string type_code { get; set; }
        /// <summary>
        /// type_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("type_name")]
        public string type_name { get; set; }
        /// <summary>
        /// parent_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("parent_key")]
        public string parent_key { get; set; }
        /// <summary>
        /// description
        /// </summary>
        /// <returns></returns>
        [DisplayName("description")]
        public string description { get; set; }
        /// <summary>
        /// sorder
        /// </summary>
        /// <returns></returns>
        [DisplayName("sorder")]
        public string sorder { get; set; }
        /// <summary>
        /// create_time
        /// </summary>
        /// <returns></returns>
        [DisplayName("create_time")]
        public DateTime? create_time { get; set; }
        /// <summary>
        /// creator_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("creator_code")]
        public string creator_code { get; set; }
        /// <summary>
        /// creator_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("creator_name")]
        public string creator_name { get; set; }
        /// <summary>
        /// modifier_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("modifier_code")]
        public string modifier_code { get; set; }
        /// <summary>
        /// modifier_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("modifier_name")]
        public string modifier_name { get; set; }
        /// <summary>
        /// modify_time
        /// </summary>
        /// <returns></returns>
        [DisplayName("modify_time")]
        public DateTime? modify_time { get; set; }
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
            //this.document_type_key = CommonHelper.GetGuid;//GetInt
            //this.document_type_key = DbHelper.GetDataSet(CommandType.Text, "select max(document_type_key)+1 from AD_document_type").Tables[0].Rows[0][0].ToString();
            //this.document_type_key = CommonHelper.GetInt;//GetInt
            this.create_time = DateTime.Now;
            this.creator_name = ManageProvider.Provider.Current().UserName;
            //this.creator_code = ManageProvider.Provider.Current().UserId;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.document_type_key = KeyValue;

            //this.atr_key = CommonHelper.GetGuid;
            //this.CreateDate = DateTime.Now;
            //this.CreateUserId = ManageProvider.Provider.Current().UserId;
            //this.CreateUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}