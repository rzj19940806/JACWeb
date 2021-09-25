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
using System.Drawing;
using System.Web.UI.WebControls;

namespace HfutIE.Entity
{
    /// <summary>
    /// 物料基础信息
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.04.02 16:27</date>
    /// </author>
    /// </summary>
    [Description("物料基础信息")]
    [PrimaryKey("MatId")]
    public class BBdbR_MatBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 物料ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("物料ID")]
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
        /// 物料类别
        /// </summary>
        /// <returns></returns>
        [DisplayName("物料类别")]
        public string MatCatg { get; set; }
        /// <summary>
        /// 物料类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("物料类型")]
        public string MatTyp { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        /// <returns></returns>
        [DisplayName("规格型号")]
        public string MatSpec { get; set; }
        /// <summary>
        /// 默认图片
        /// </summary>
        /// <returns></returns>
        [DisplayName("默认图片")]
        public byte[] MatImg { get; set; }
        /// <summary>
        /// 提前期
        /// </summary>
        /// <returns></returns>
        [DisplayName("提前期")]
        public string LeadTm { get; set; }
        /// <summary>
        /// 良品率
        /// </summary>
        /// <returns></returns>
        [DisplayName("良品率")]
        public Decimal? YieldRate { get; set; }
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
        public string CreTm { get; set; }
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
        public string MdfTm { get; set; }
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
            this.MatId = CommonHelper.GetGuid;
            this.VersionNumber = "V1.0";
            this.CreTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
            //this.ConfigId = KeyValue;
            this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}