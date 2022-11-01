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
    /// 拧紧使能配置表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.27 16:34</date>
    /// </author>
    /// </summary>
    [Description("拧紧使能配置表")]
    [PrimaryKey("ID")]
    public class Tg_JobEnableConfig : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("ID")]
        public string ID { get; set; }
        /// <summary>
        /// WJCId
        /// </summary>
        /// <returns></returns>
        [DisplayName("WJCId")]
        public string WJCId { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        /// <returns></returns>
        [DisplayName("Type")]
        public string Type { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        /// <returns></returns>
        [DisplayName("Code")]
        public string Code { get; set; }
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
        /// <summary>
        /// RsvFld1
        /// </summary>
        /// <returns></returns>
        [DisplayName("RsvFld1")]
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
            this.ID = CommonHelper.GetGuid;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.ID = KeyValue;
        }
        #endregion
    }
}