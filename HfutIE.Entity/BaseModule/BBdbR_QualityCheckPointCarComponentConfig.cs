﻿//=====================================================================================
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
    ///质控点组件配置
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.12.05 16:27</date>
    /// </author>
    /// </summary>
    [Description("质控点组件配置")]
    [PrimaryKey("ConfigId")]
    public class BBdbR_QualityCheckPointCarComponentConfig:BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string ConfigId { get; set; }
        /// <summary>
        /// 质控点主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("质控点主键")]
        public string QualityCheckPointId { get; set; }
        /// <summary>
        /// 车身方位主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身方位主键")]
        public string CarPositionId { get; set; }
        /// <summary>
        /// 车身方位分组主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身方位分组主键")]
        public string CarPositionGroupId { get; set; }
      
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
            this.ConfigId = CommonHelper.GetGuid;
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
            this.ConfigId = KeyValue;
            this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now;
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}
