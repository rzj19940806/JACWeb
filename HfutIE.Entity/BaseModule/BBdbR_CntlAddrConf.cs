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
    /// AVI站点基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.26 10:57</date>
    /// </author>
    /// </summary>
    [Description("数采地址信息表")]
    [PrimaryKey("RecId")]
    public class BBdbR_CntlAddrConf : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string RecId { get; set; }
       
        /// <summary>
        /// 工位Id
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位Id")]
        public string WcId { get; set; }
        /// <summary>
        /// 设备Id
        /// </summary>
        /// <returns></returns>
        [DisplayName("设备Id")]
        public string DvcId { get; set; }
       
        /// <summary>
        /// 数采类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("数采类型")]
        public string CntlType { get; set; }
        /// <summary>
        /// 地址名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("地址名称")]
        public string SingnalNm { get; set; }
        /// <summary>
        /// 地址值
        /// </summary>
        /// <returns></returns>
        [DisplayName("地址值")]
        public string CntlAddr { get; set; }
        /// <summary>
        /// 地址描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("地址描述")]
        public string CntlAddrDsc { get; set; }
        /// <summary>
        /// 地址数据类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("地址数据类型")]
        public string CntlDateType { get; set; }
        /// <summary>
        /// 地址来源
        /// </summary>
        /// <returns></returns>
        [DisplayName("地址来源")]
        public string SglSource { get; set; }
        /// <summary>
        /// 是否监控
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否监控")]
        public string IsMonitoring { get; set; }
        /// <summary>
        /// 监控频率
        /// </summary>
        /// <returns></returns>
        [DisplayName("监控频率")]
        public int? MonitorRate { get; set; }
        
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
            this.RecId = CommonHelper.GetGuid;
            //this.VersionNumber = "V1.0";
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
            this.RecId = KeyValue;
            //this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
       
    }
}