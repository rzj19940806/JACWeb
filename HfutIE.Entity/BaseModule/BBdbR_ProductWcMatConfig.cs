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
    /// 产品工位配置物料表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.13 10:18</date>
    /// </author>
    /// </summary>
    [Description("产品工位配置物料表")]
    [PrimaryKey("ProductClassMatId")]
    public class BBdbR_ProductWcMatConfig : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 产品工位物料主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("产品工位物料主键")]
        public string ProductClassMatId { get; set; }
        /// <summary>
        /// 产品工位主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("产品工位主键")]
        public string ProductClassId { get; set; }
        /// <summary>
        /// 产线主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线主键")]
        public string PlineId { get; set; }
        /// <summary>
        /// 工位主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位主键")]
        public string WcId { get; set; }
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
        /// 物料主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("物料主键")]
        public string MatId { get; set; }
        /// <summary>
        /// 物料编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("物料编号")]
        public string MatCd { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("物料名称")]
        public string MatNm { get; set; }
        /// <summary>
        /// 物料数量
        /// </summary>
        /// <returns></returns>
        [DisplayName("物料数量")]
        public int? MatNum { get; set; }
        /// <summary>
        /// 是否安全件
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否安全件")]
        public int? IsSafe { get; set; }
        /// <summary>
        /// 是否展示图片
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否展示图片")]
        public int? IsShow { get; set; }
        /// <summary>
        /// 是否关重件
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否关重件")]
        public int? IsScan { get; set; }
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
            this.ProductClassMatId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.ProductClassMatId = KeyValue;
                                            }
        #endregion
    }
}