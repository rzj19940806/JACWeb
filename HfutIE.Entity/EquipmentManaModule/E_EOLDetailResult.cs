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
    /// E_EOLDetailResult
    /// <author>
    ///		<name>she</name>
    ///		<date>2022.01.05 21:21</date>
    /// </author>
    /// </summary>
    [Description("E_EOLDetailResult")]
    [PrimaryKey("EDRID")]
    public class E_EOLDetailResult : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// EDRID
        /// </summary>
        /// <returns></returns>
        [DisplayName("EDRID")]
        public string EDRID { get; set; }
        /// <summary>
        /// VIN
        /// </summary>
        /// <returns></returns>
        [DisplayName("VIN")]
        public string VIN { get; set; }
        /// <summary>
        /// TestNum
        /// </summary>
        /// <returns></returns>
        [DisplayName("TestNum")]
        public int? TestNum { get; set; }
        /// <summary>
        /// Station
        /// </summary>
        /// <returns></returns>
        [DisplayName("Station")]
        public string Station { get; set; }
        /// <summary>
        /// ModuleNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("ModuleNm")]
        public string ModuleNm { get; set; }
        /// <summary>
        /// CheckNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("CheckNm")]
        public string CheckNm { get; set; }
        /// <summary>
        /// CheckContent
        /// </summary>
        /// <returns></returns>
        [DisplayName("CheckContent")]
        public string CheckContent { get; set; }
        /// <summary>
        /// CheckResult
        /// </summary>
        /// <returns></returns>
        [DisplayName("CheckResult")]
        public string CheckResult { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.EDRID = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.EDRID = KeyValue;
                                            }
        #endregion
    }
}