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
    /// 指导文件
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.09 11:15</date>
    /// </author>
    /// </summary>
    [Description("指导文件")]
    [PrimaryKey("GuidanceFileID")]
    public class BBdbR_GuidanceFile : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 指导文件主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("指导文件主键")]
        public string GuidanceFileID { get; set; }
        /// <summary>
        /// 产线ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线ID")]
        public string PlineId { get; set; }
        /// <summary>
        /// 工位ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位ID")]
        public string WcId { get; set; }
        /// <summary>
        /// 工位名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位名称")]
        public string WcNm { get; set; }
        /// <summary>
        /// 指导文件编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("指导文件编号")]
        public string GuidanceFileCode { get; set; }
        /// <summary>
        /// 指导文件名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("指导文件名称")]
        public string GuidanceFileName { get; set; }
        /// <summary>
        /// 指导文件类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("指导文件类型")]
        public string GuidanceFileType { get; set; }
        /// <summary>
        /// 指导文件路径
        /// </summary>
        /// <returns></returns>
        [DisplayName("指导文件路径")]
        public string GuidanceFilePath { get; set; }
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
            this.GuidanceFileID = CommonHelper.GetGuid;
            this.CreTm = DateTime.Now.ToLocalTime();
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
            Enabled = "1";
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.GuidanceFileID = KeyValue;
                                            }
        #endregion
    }
}