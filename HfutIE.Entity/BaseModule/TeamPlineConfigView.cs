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
    /// 班组基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.30 16:15</date>
    /// </author>
    /// </summary>
    [Description("班组产线配置信息表")]
    public class TeamPlineConfigView : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 班组主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("班组主键")]
        public string TeamId { get; set; }
        /// <summary>
        /// 班组编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("班组编号")]
        public string TeamCd { get; set; }
        /// <summary>
        /// 班组名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("班组名称")]
        public string TeamNm { get; set; }
        /// <summary>
        /// 班组类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("班组类型")]
        public string TeamTyp { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("描述")]
        public string Dsc { get; set; }
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
        /// 产线编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线编号")]
        public string PlineCd { get; set; }
        /// <summary>
        /// 产线名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线名称")]
        public string PlineNm { get; set; }
        /// <summary>
        /// 产线类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线类型")]
        public string PlineTyp { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.TeamId = CommonHelper.GetGuid;
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