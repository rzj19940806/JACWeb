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
    /// 滞留区域管理基本信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.27 15:54</date>
    /// </author>
    /// </summary>
    [Description("滞留区域管理基本信息表")]
    [PrimaryKey("RuleId")]
    public class BBdbR_CarStrandedBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string RuleId { get; set; }
        /// <summary>
        /// 区域编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("区域编号")]
        public string AreaCd { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("区域名称")]
        public string AreaNm { get; set; }
        /// <summary>
        /// 区域类别
        /// </summary>
        /// <returns></returns>
        [DisplayName("区域类别")]
        public int? AreaTyp { get; set; }
        /// <summary>
        /// 起始AVI站点主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("起始AVI站点主键")]
        public string StaAVIId { get; set; }
        /// <summary>
        /// 结束AVI站点主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("结束AVI站点主键")]
        public string EndAVIId { get; set; }
        /// <summary>
        /// 滞留等级
        /// </summary>
        /// <returns></returns>
        [DisplayName("滞留等级")]
        public string StrandedGrand { get; set; }
        /// <summary>
        /// 滞留时间临界值
        /// </summary>
        /// <returns></returns>
        [DisplayName("滞留时间临界值")]
        public string StrandedRuleTm { get; set; }
        /// <summary>
        /// 滞留报警规则
        /// </summary>
        /// <returns></returns>
        [DisplayName("滞留报警规则")]
        public string StrandedRule { get; set; }
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
            this.RuleId = CommonHelper.GetGuid;
            this.VersionNumber = "V1.0";
            this.CreTm = DateTime.Now;
            this.Enabled = "1";
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.RuleId = KeyValue;
            this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now;
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}