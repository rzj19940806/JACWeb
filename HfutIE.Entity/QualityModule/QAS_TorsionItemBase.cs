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
    /// 扭力检测项基础信息表
    /// <author>
    ///		<name>CHFAS</name>
    ///		<date>2021.07.06 12:00</date>
    /// </author>
    /// </summary>
    [Description("扭力检测项基础信息表")]
    [PrimaryKey("RecordId")]
    public class QAS_TorsionItemBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string RecordId { get; set; }
        /// <summary>
        /// 车型
        /// </summary>
        /// <returns></returns>
        [DisplayName("车型")]
        public string CarType { get; set; }
        /// <summary>
        /// 扭力检测项ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("扭力检测项ID")]
        public string TORItemID { get; set; }
        /// <summary>
        /// 扭力检测项
        /// </summary>
        /// <returns></returns>
        [DisplayName("扭力检测项")]
        public string TORItem { get; set; }
        /// <summary>
        /// <summary>
        /// 静态扭力最小值
        /// </summary>
        /// <returns></returns>
        [DisplayName("静态扭力最小值")]
        public float TORMin { get; set; }
        /// <summary>
        /// 静态扭力最大值
        /// </summary>
        /// <returns></returns>
        [DisplayName("静态扭力最大值")]
        public float TORMax { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        /// <returns></returns>
        [DisplayName("单位")]
        public string Unit { get; set; }
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