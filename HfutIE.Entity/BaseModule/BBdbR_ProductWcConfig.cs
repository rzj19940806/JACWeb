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
    /// 产品工位配置表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.13 10:18</date>
    /// </author>
    /// </summary>
    [Description("产品工位配置表")]
    [PrimaryKey("ProductClassId")]
    public class BBdbR_ProductWcConfig : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 产品工位主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("产品工位主键")]
        public string ProductClassId { get; set; }
        /// <summary>
        /// 产品主键（物料主键）
        /// </summary>
        /// <returns></returns>
        [DisplayName("产品主键（物料主键）")]
        public string MatId { get; set; }
        /// <summary>
        /// 产品编号《物料编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("产品编号《物料编号")]
        public string MatCd { get; set; }
        /// <summary>
        /// 产品名称《物料名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("产品名称《物料名称")]
        public string MatNm { get; set; }
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
        /// 图片名称《文档名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("图片名称《文档名称")]
        public string FileNm { get; set; }
        /// <summary>
        /// 文档主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("文档主键")]
        public string ConfigId { get; set; }
        /// <summary>
        /// 配置图片（物料文档的图片编号）
        /// </summary>
        /// <returns></returns>
        [DisplayName("配置图片（物料文档的图片编号）")]
        public string FileCd { get; set; }
        /// <summary>
        /// 锁工位数
        /// </summary>
        /// <returns></returns>
        [DisplayName("锁工位数")]
        public int? LockStationNum { get; set; }
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
            this.ProductClassId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.ProductClassId = KeyValue;
                                            }
        #endregion
    }
}