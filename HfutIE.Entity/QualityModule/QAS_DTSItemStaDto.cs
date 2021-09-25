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
    /// DTS质量基础信息表
    /// <author>
    ///		<name>CHFAS</name>
    ///		<date>2021.07.06 12:00</date>
    /// </author>
    /// </summary>
    [Description("DTS质量基础信息表")]
    [PrimaryKey("RecordId")]
    public class QAS_DTSItemStaDto : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string RecordId { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("父级ID")]
        public string ParId { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        /// <returns></returns>
        [DisplayName("序号")]
        public string SerialNumber { get; set; }
        /// <summary>
        /// 基准值
        /// </summary>
        /// <returns></returns>
        [DisplayName("基准值")]
        public string BaseValue { get; set; }
        /// <summary>
        /// 公差值
        /// </summary>
        /// <returns></returns>
        [DisplayName("公差值")]
        public string TolValue { get; set; }
        /// <summary>
        /// TOL+
        /// </summary>
        /// <returns></returns>
        [DisplayName("TOL+")]
        public string TOLUp { get; set; }
        /// <summary>
        /// TOL-
        /// </summary>
        /// <returns></returns>
        [DisplayName("TOL-")]
        public string TOLDown { get; set; }
        /// <summary>
        /// 方向
        /// </summary>
        /// <returns></returns>
        [DisplayName("方向")]
        public string Direction { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效性")]
        public string Enabled { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public DateTime? CreTm { get; set; }
        /// <summary>
        /// 创建人编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建人编号")]
        public string CreCd { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建人名称")]
        public string CreNm { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改时间")]
        public DateTime? MdfTm { get; set; }
        /// <summary>
        /// 修改人编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人编号")]
        public string MdfCd { get; set; }
        /// <summary>
        /// 修改人名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人名称")]
        public string MdfNm { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Rem { get; set; }
        /// <summary>
        /// 预留字段1
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段1")]
        public string RsvFld1 { get; set; }
        /// <summary>
        /// 预留字段2
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段2")]
        public string RsvFld2 { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.RecordId = CommonHelper.GetGuid;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.RecordId = KeyValue;
        }
        #endregion
    }
}