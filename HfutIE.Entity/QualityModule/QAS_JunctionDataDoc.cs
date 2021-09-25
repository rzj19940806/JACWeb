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
    /// 连接点质量结果数据档案表
    /// <author>
    ///		<name>CHFAS</name>
    ///		<date>2021.07.20 12:00</date>
    /// </author>
    /// </summary>
    [Description("连接点质量结果数据档案表")]
    [PrimaryKey("RecordId")]
    public class QAS_JunctionDataDoc : BaseEntity
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
        /// 编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("编号")]
        public string Code { get; set; }
        /// <summary>
        /// ItemNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("检测项")]
        public string ItemNm { get; set; }
        /// <summary>
        /// 工位名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位名称")]
        public string WcNm { get; set; }
        /// <summary>
        /// 零件名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("零件名称")]
        public string PartNm { get; set; }
        /// <summary>
        /// CC/SC
        /// </summary>
        /// <returns></returns>
        [DisplayName("CC/SC")]
        public string CCSC { get; set; }
        /// <summary>
        /// <summary>
        /// 头高标准
        /// </summary>
        /// <returns></returns>
        [DisplayName("头高标准")]
        public string HeadHeghitSta { get; set; }
        /// <summary>
        /// 头部间隙
        /// </summary>
        /// <returns></returns>
        [DisplayName("头部间隙")]
        public string HeadGapSta { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        /// <returns></returns>
        [DisplayName("单位")]
        public string Unit { get; set; }
        /// <summary>
        /// 长度标准
        /// </summary>
        /// <returns></returns>
        [DisplayName("长度标准")]
        public string LengthSta { get; set; }/// <summary>
        /// 拧紧扭矩
        /// </summary>
        /// <returns></returns>
        [DisplayName("拧紧扭矩")]
        public string TightenTOR { get; set; }
        /// 焊点位置
        /// </summary>
        /// <returns></returns>
        [DisplayName("焊点位置")]
        public string SpotLocation { get; set; }
        /// <summary>
        /// 焊核标准值
        /// </summary>
        /// <returns></returns>
        [DisplayName("焊核标准值")]
        public string SpotStaValue { get; set; }
        /// <summary>
        /// 检测项类别
        /// </summary>
        /// <returns></returns>
        [DisplayName("检测项类别")]
        public string Category { get; set; }
        /// <summary>
        /// 车身号
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身号")]
        public string BodyNo { get; set; }
        /// <summary>
        /// 检测值
        /// </summary>
        /// <returns></returns>
        [DisplayName("检测值")]
        public string CheckValue { get; set; }
        /// <summary>
        /// 检测日期
        /// </summary>
        /// <returns></returns>
        [DisplayName("检测日期")]
        public DateTime? CheckTm { get; set; }
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
        /// 更新时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("更新时间")]
        public DateTime? UpdateTm { get; set; }
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