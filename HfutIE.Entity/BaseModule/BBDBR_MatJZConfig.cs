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
    /// 物料加注配置表
    /// <author>
    ///		<name>she</name>
    ///		<date>2022.05.26 15:13</date>
    /// </author>
    /// </summary>
    [Description("物料加注配置表")]
    [PrimaryKey("Id")]
    public class BBDBR_MatJZConfig : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 物料加注配置主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("物料加注配置主键")]
        public string Id { get; set; }
        /// <summary>
        /// 物料编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("物料编号")]
        public string MatCd { get; set; }
        /// <summary>
        /// 加注类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("加注类型")]
        public string JZType { get; set; }
        /// <summary>
        /// 加注量
        /// </summary>
        /// <returns></returns>
        [DisplayName("加注量")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public decimal? JZNumber { get; set; }
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
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Rem { get; set; }
        /// <summary>
        /// 备用字段1
        /// </summary>
        /// <returns></returns>
        [DisplayName("备用字段1")]
        public string RsvFld1 { get; set; }
        /// <summary>
        /// 备用字段2
        /// </summary>
        /// <returns></returns>
        [DisplayName("备用字段2")]
        public string RsvFld2 { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = CommonHelper.GetGuid;
            this.Enabled = "1";
            this.CreTm = DateTime.Now;
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.Id = KeyValue;
            this.MdfTm = DateTime.Now;
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}