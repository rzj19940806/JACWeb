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
    /// E_EOLCheckResult
    /// <author>
    ///		<name>she</name>
    ///		<date>2022.01.05 21:18</date>
    /// </author>
    /// </summary>
    [Description("E_EOLCheckResult")]
    [PrimaryKey("ECRID")]
    public class E_EOLCheckResult : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// ECRID
        /// </summary>
        /// <returns></returns>
        [DisplayName("ECRID")]
        public string ECRID { get; set; }
        /// <summary>
        /// VIN
        /// </summary>
        /// <returns></returns>
        [DisplayName("VIN")]
        public string VIN { get; set; }
        /// <summary>
        /// Car
        /// </summary>
        /// <returns></returns>
        [DisplayName("Car")]
        public string Car { get; set; }
        /// <summary>
        /// CarType
        /// </summary>
        /// <returns></returns>
        [DisplayName("CarType")]
        public string CarType { get; set; }
        /// <summary>
        /// CarName
        /// </summary>
        /// <returns></returns>
        [DisplayName("CarName")]
        public string CarName { get; set; }
        /// <summary>
        /// Station
        /// </summary>
        /// <returns></returns>
        [DisplayName("Station")]
        public string Station { get; set; }
        /// <summary>
        /// Time
        /// </summary>
        /// <returns></returns>
        [DisplayName("Time")]
        public DateTime? Time { get; set; }
        /// <summary>
        /// TestNum
        /// </summary>
        /// <returns></returns>
        [DisplayName("TestNum")]
        public int? TestNum { get; set; }
        /// <summary>
        /// Result
        /// </summary>
        /// <returns></returns>
        [DisplayName("Result")]
        public string Result { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ECRID = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.ECRID = KeyValue;
                                            }
        #endregion
    }
}