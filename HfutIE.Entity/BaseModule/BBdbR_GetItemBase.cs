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
    /// 采集项基本信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.27 12:20</date>
    /// </author>
    /// </summary>
    [Description("采集项基本信息表")]
    [PrimaryKey("GetItemId")]
    public class BBdbR_GetItemBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 采集项主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("采集项主键")]
        public string GetItemId { get; set; }
        /// <summary>
        /// 采集项编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("采集项编号")]
        public string GetItemCd { get; set; }
        /// <summary>
        /// 采集项名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("采集项名称")]
        public string GetItemNm { get; set; }
        /// <summary>
        /// 工位编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位编号")]
        public string WcCd { get; set; }
        /// <summary>
        /// 工位名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位名称")]
        public string WcNm { get; set; }
        /// <summary>
        /// 采集项类别
        /// </summary>
        /// <returns></returns>
        [DisplayName("采集项类别")]
        public string GetItemType { get; set; }
        /// <summary>
        /// 采集项上限
        /// </summary>
        /// <returns></returns>
        [DisplayName("采集项上限")]
        public decimal? GetItemUpLimit { get; set; }
        /// <summary>
        /// 采集项下限
        /// </summary>
        /// <returns></returns>
        [DisplayName("采集项下限")]
        public decimal? GetItemLowLimit { get; set; }
        /// <summary>
        /// 采集项标准值
        /// </summary>
        /// <returns></returns>
        [DisplayName("采集项标准值")]
        public decimal? GetItemValue { get; set; }
        /// <summary>
        /// 采集项参数单位
        /// </summary>
        /// <returns></returns>
        [DisplayName("采集项参数单位")]
        public string GetItemUnit { get; set; }
        /// <summary>
        /// 采集项说明
        /// </summary>
        /// <returns></returns>
        [DisplayName("采集项说明")]
        public string GetItemExplain { get; set; }
        /// <summary>
        /// 采集方式
        /// </summary>
        /// <returns></returns>
        [DisplayName("采集方式")]
        public string GetItemTy { get; set; }
        /// <summary>
        /// 采集频次
        /// </summary>
        /// <returns></returns>
        [DisplayName("采集频次")]
        public string GetItemFq { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        /// <returns></returns>
        [DisplayName("版本号")]
        public string VersionNumber { get; set; }
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
            this.GetItemId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.GetItemId = KeyValue;
                                            }
        #endregion
    }
}