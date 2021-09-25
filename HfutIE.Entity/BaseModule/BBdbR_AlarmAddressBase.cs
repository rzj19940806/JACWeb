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
    /// 设备报警地址管理表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.17 19:38</date>
    /// </author>
    /// </summary>
    [Description("设备报警地址管理表")]
    [PrimaryKey("RuleId")]
    public class BBdbR_AlarmAddressBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string RuleId { get; set; }
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
        /// 设备主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("设备主键")]
        public string DvcId { get; set; }
        /// <summary>
        /// 报警地址
        /// </summary>
        /// <returns></returns>
        [DisplayName("报警地址")]
        public string AlarmAddress { get; set; }
        /// <summary>
        /// 报警位数
        /// </summary>
        /// <returns></returns>
        [DisplayName("报警位数")]
        public string AlarmBit { get; set; }
        /// <summary>
        /// 报警路径
        /// </summary>
        /// <returns></returns>
        [DisplayName("报警路径")]
        public string AlarmRoute { get; set; }
        /// <summary>
        /// 报警等级
        /// </summary>
        /// <returns></returns>
        [DisplayName("报警等级")]
        public string AlarmClass { get; set; }
        /// <summary>
        /// 报警描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("报警描述")]
        public string  AlarmDsc { get; set; }
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
            this.RuleId = CommonHelper.GetGuid;
            this.AlarmRoute = "TL1++Zone01+190GPB01-SB01";
            this.VersionNumber = "V1.0";
            this.CreTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //ToString("yyyy-MM-dd HH:mm:ss");
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
            this.AlarmRoute = "TL1++Zone01+190GPB01-SB01";
            this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}