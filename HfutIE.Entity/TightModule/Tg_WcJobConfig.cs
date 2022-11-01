//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2022
// Software Developers @ HfutIE 2022
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
    /// Tg_WcJobConfig
    /// <author>
    ///		<name>she</name>
    ///		<date>2022.09.01 14:34</date>
    /// </author>
    /// </summary>
    [Description("Tg_WcJobConfig")]
    [PrimaryKey("ID")]
    public class Tg_WcJobConfig : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("ID")]
        public string ID { get; set; }
        /// <summary>
        /// WcId
        /// </summary>
        /// <returns></returns>
        [DisplayName("WcJobCd")]
        public string WcJobCd { get; set; }
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
        public string WcCd { get; set; }
        /// <summary>
        /// WcNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("WcNm")]
        public string WcNm { get; set; }
        /// <summary>
        /// Position
        /// </summary>
        /// <returns></returns>
        [DisplayName("Position")]
        public string Position { get; set; }
        /// <summary>
        /// JobCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("JobCd")]
        public string JobCd { get; set; }
        /// <summary>
        /// ControllerID
        /// </summary>
        /// <returns></returns>
        [DisplayName("ControllerID")]
        public string ControllerID { get; set; }
        /// <summary>
        /// ControllerPort
        /// </summary>
        /// <returns></returns>
        [DisplayName("ControllerPort")]
        public string ControllerPort { get; set; }
       
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