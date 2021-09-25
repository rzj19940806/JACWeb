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
    /// 设备基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.29 15:54</date>
    /// </author>
    /// </summary>
    [Description("设备基础信息表")]
    [PrimaryKey("DvcId")]
    public class BBdbR_DvcBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 设备主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("设备主键")]
        public string DvcId { get; set; }
        /// <summary>
        /// 机构级别
        /// </summary>
        /// <returns></returns>
        [DisplayName("机构级别")]
        public string Class { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("机构ID")]
        public string ClassId { get; set; }
        /// <summary>
        /// 设备位置
        /// </summary>
        /// <returns></returns>
        [DisplayName("设备位置")]
        public string DvcLocatn { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("设备编号")]
        public string DvcCd { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("设备名称")]
        public string DvcNm { get; set; }
        /// <summary>
        /// 设备类别
        /// </summary>
        /// <returns></returns>
        [DisplayName("设备类别")]
        public string DvcCatg { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("设备类型")]
        public string DvcTyp { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        /// <returns></returns>
        [DisplayName("IP地址")]
        public string IPAddr { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        /// <returns></returns>
        [DisplayName("端口")]
        public string Port { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        /// <returns></returns>
        [DisplayName("设备型号")]
        public string DvcMdl { get; set; }
        /// <summary>
        /// 设备产商
        /// </summary>
        /// <returns></returns>
        [DisplayName("设备产商")]
        public string DvcMaker { get; set; }
        /// <summary>
        /// 设备寿命
        /// </summary>
        /// <returns></returns>
        [DisplayName("设备寿命")]
        public string DvcLife { get; set; }
        /// <summary>
        /// 设备制造日期
        /// </summary>
        /// <returns></returns>
        [DisplayName("设备制造日期")]
        public string DvcMDt { get; set; }
        /// <summary>
        /// 维保周期(天)
        /// </summary>
        /// <returns></returns>
        [DisplayName("维保周期(天)")]
        public int? MaintCycle { get; set; }
        /// <summary>
        /// 提前期（天）
        /// </summary>
        /// <returns></returns>
        [DisplayName("提前期（天）")]
        public int? LeadTm { get; set; }
        /// <summary>
        /// 设备描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("设备描述")]
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
            this.DvcId = CommonHelper.GetGuid;
            this.VersionNumber = "V1.0";
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
            this.DvcId = KeyValue;
            this.MdfTm = DateTime.Now;
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}