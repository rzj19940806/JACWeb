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
    [PrimaryKey("atr_key")]
    public class PROCESS_FILES : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// document_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("atr_key")]
        public string atr_key { get; set; }
        /// <summary>
        /// site_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("site_key")]
        public string site_key { get; set; }
       
        /// <summary>
        /// site_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("site_name")]
        public string site_name { get; set; }
        /// <summary>
        /// site_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("site_code")]
        public string site_code { get; set; }
        /// <summary>
        /// area_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("area_key")]
        public string area_key { get; set; }
       
        /// <summary>
        /// area_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("area_name")]
        public string area_name { get; set; }
        /// <summary>
        /// area_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("area_code")]
        public string area_code { get; set; }
        /// <summary>
        /// ws_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("ws_key")]
        public string ws_key { get; set; }
     
        /// <summary>
        /// ws_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("ws_name")]
        public string ws_name { get; set; }
        /// <summary>
        /// ws_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("ws_code")]
        public string ws_code { get; set; }
        /// <summary>
        /// p_line_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("p_line_key")]
        public string p_line_key { get; set; }
     
        /// <summary>
        /// p_line_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("p_line_name")]
        public string p_line_name { get; set; }

        /// <summary>
        /// p_line_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("p_line_code")]
        public string p_line_code { get; set; }
        /// <summary>
        /// wc_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("wc_key")]
        public string wc_key { get; set; }
      
        /// <summary>
        /// wc_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("wc_name")]
        public string wc_name { get; set; }
        /// <summary>
        /// wc_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("wc_code")]
        public string wc_code { get; set; }
        /// <summary>
        /// product_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("product_key")]
        public string product_key { get; set; }
    
        /// <summary>
        /// product_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("product_name")]
        public string product_name { get; set; }

        /// <summary>
        /// product_model
        /// </summary>
        /// <returns></returns>
        [DisplayName("product_model")]
        public string product_model { get; set; }
        /// <summary>
        /// euipment_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("euipment_key")]
        public string euipment_key { get; set; }
        /// <summary>
        /// euipment_code
        /// </summary>
        /// <returns></returns>
        [DisplayName("euipment_code")]
        public string euipment_code { get; set; }
        /// <summary>
        /// euipment_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("euipment_name")]
        public string euipment_name { get; set; }

        /// <summary>
        /// file_key
        /// </summary>
        /// <returns></returns>
        [DisplayName("file_key")]
        public string file_key { get; set; }

        /// <summary>
        /// file_name
        /// </summary>
        /// <returns></returns>
        [DisplayName("file_name")]
        public string file_name { get; set; }
        /// <summary>
        /// file_format
        /// </summary>
        /// <returns></returns>
        [DisplayName("file_format")]
        public string file_format { get; set; }

        /// <summary>
        /// remarks
        /// </summary>
        /// <returns></returns>
        [DisplayName("remarks")]
        public string remarks { get; set; }
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
        /// 修改用户Is_default
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改用户")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 当前版本
        /// </summary>
        /// <returns></returns>
        [DisplayName("Is_default")]
        public string Is_default { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            //this.atr_key = CommonHelper.GetGuid;设置成自增的
            this.CreateDate = DateTime.Now;
            //this.CreateUserId = ManageProvider.Provider.Current().UserId;
            this.CreateUserName = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.atr_key = KeyValue;
            this.ModifyDate = DateTime.Now;
            //this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}