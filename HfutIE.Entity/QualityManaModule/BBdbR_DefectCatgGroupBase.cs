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
    /// 缺陷类别分组基本信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.10.09 20:06</date>
    /// </author>
    /// </summary>
    [Description("缺陷类别分组基本信息表")]
    [PrimaryKey("DefectCatgGroupId")]
    public class BBdbR_DefectCatgGroupBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 缺陷类型分组主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("缺陷类型分组主键")]
        public string DefectCatgGroupId { get; set; }
        /// <summary>
        /// 缺陷类型主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("缺陷类型主键")]
        public string DefectCatgId { get; set; }
        /// <summary>
        /// 缺陷类型分组编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("缺陷类型分组编号")]
        public string DefectCatgGroupCd { get; set; }
        /// <summary>
        /// 缺陷类型分组名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("缺陷类型分组名称")]
        public string DefectCatgGroupNm { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("描述")]
        public string Dsc { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效性")]
        public string Enabled { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        /// <returns></returns>
        [DisplayName("版本号")]
        public string VersionNumber { get; set; }
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
        /// RsvFld1
        /// </summary>
        /// <returns></returns>
        [DisplayName("RsvFld1")]
        public string RsvFld1 { get; set; }
        /// <summary>
        /// RsvFld2
        /// </summary>
        /// <returns></returns>
        [DisplayName("RsvFld2")]
        public string RsvFld2 { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.DefectCatgGroupId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.DefectCatgGroupId = KeyValue;
            this.MdfTm = DateTime.Now;
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}