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
    /// E_PressureRecordInfo
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.10.21 11:00</date>
    /// </author>
    /// </summary>
    [Description("E_PressureRecordInfo")]
    [PrimaryKey("ID")]
    public class E_PressureRecordInfo : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("ID")]
        public string ID { get; set; }
        /// <summary>
        /// VIN
        /// </summary>
        /// <returns></returns>
        [DisplayName("VIN")]
        public string VIN { get; set; }
        /// <summary>
        /// CarType
        /// </summary>
        /// <returns></returns>
        [DisplayName("CarType")]
        public string CarType { get; set; }
        /// <summary>
        /// SensorNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("SensorNm")]
        public string SensorNm { get; set; }
        /// <summary>
        /// RFTireID
        /// </summary>
        /// <returns></returns>
        [DisplayName("RFTireID")]
        public string RFTireID { get; set; }
        /// <summary>
        /// RFTirePressureLimit
        /// </summary>
        /// <returns></returns>
        [DisplayName("RFTirePressureLimit")]
        public string RFTirePressureLimit { get; set; }
        /// <summary>
        /// RFTirePressureLowerLimit
        /// </summary>
        /// <returns></returns>
        [DisplayName("RFTirePressureLowerLimit")]
        public string RFTirePressureLowerLimit { get; set; }
        /// <summary>
        /// RFTirePressureValue
        /// </summary>
        /// <returns></returns>
        [DisplayName("RFTirePressureValue")]
        public string RFTirePressureValue { get; set; }
        /// <summary>
        /// RFTirePressureResult
        /// </summary>
        /// <returns></returns>
        [DisplayName("RFTirePressureResult")]
        public string RFTirePressureResult { get; set; }
        /// <summary>
        /// LFTireID
        /// </summary>
        /// <returns></returns>
        [DisplayName("LFTireID")]
        public string LFTireID { get; set; }
        /// <summary>
        /// LFTirePressureLimit
        /// </summary>
        /// <returns></returns>
        [DisplayName("LFTirePressureLimit")]
        public string LFTirePressureLimit { get; set; }
        /// <summary>
        /// FTirePressureLowerLimit
        /// </summary>
        /// <returns></returns>
        [DisplayName("FTirePressureLowerLimit")]
        public string FTirePressureLowerLimit { get; set; }
        /// <summary>
        /// LFTirePressureValue
        /// </summary>
        /// <returns></returns>
        [DisplayName("LFTirePressureValue")]
        public string LFTirePressureValue { get; set; }
        /// <summary>
        /// LFTirePressureResult
        /// </summary>
        /// <returns></returns>
        [DisplayName("LFTirePressureResult")]
        public string LFTirePressureResult { get; set; }
        /// <summary>
        /// RBTireID
        /// </summary>
        /// <returns></returns>
        [DisplayName("RBTireID")]
        public string RBTireID { get; set; }
        /// <summary>
        /// RBTirePressureLimit
        /// </summary>
        /// <returns></returns>
        [DisplayName("RBTirePressureLimit")]
        public string RBTirePressureLimit { get; set; }
        /// <summary>
        /// RBTirePressureLowerLimit
        /// </summary>
        /// <returns></returns>
        [DisplayName("RBTirePressureLowerLimit")]
        public string RBTirePressureLowerLimit { get; set; }
        /// <summary>
        /// RBTirePressureValue
        /// </summary>
        /// <returns></returns>
        [DisplayName("RBTirePressureValue")]
        public string RBTirePressureValue { get; set; }
        /// <summary>
        /// RBTirePressureResult
        /// </summary>
        /// <returns></returns>
        [DisplayName("RBTirePressureResult")]
        public string RBTirePressureResult { get; set; }
        /// <summary>
        /// LBTireID
        /// </summary>
        /// <returns></returns>
        [DisplayName("LBTireID")]
        public string LBTireID { get; set; }
        /// <summary>
        /// LBTirePressureLimit
        /// </summary>
        /// <returns></returns>
        [DisplayName("LBTirePressureLimit")]
        public string LBTirePressureLimit { get; set; }
        /// <summary>
        /// LBTirePressureLowerLimit
        /// </summary>
        /// <returns></returns>
        [DisplayName("LBTirePressureLowerLimit")]
        public string LBTirePressureLowerLimit { get; set; }
        /// <summary>
        /// LBTirePressureValue
        /// </summary>
        /// <returns></returns>
        [DisplayName("LBTirePressureValue")]
        public string LBTirePressureValue { get; set; }
        /// <summary>
        /// LBTirePressureResult
        /// </summary>
        /// <returns></returns>
        [DisplayName("LBTirePressureResult")]
        public string LBTirePressureResult { get; set; }
        /// <summary>
        /// TirePressureUnit
        /// </summary>
        /// <returns></returns>
        [DisplayName("TirePressureUnit")]
        public string TirePressureUnit { get; set; }
        /// <summary>
        /// DetectionSource
        /// </summary>
        /// <returns></returns>
        [DisplayName("DetectionSource")]
        public string DetectionSource { get; set; }
        /// <summary>
        /// DetectionTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("DetectionTime")]
        public string DetectionTime { get; set; }
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