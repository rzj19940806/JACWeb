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
    [Description("AVI站点基础信息表")]
    [PrimaryKey("AviId")]
    public class BBdbR_AVIBase : BaseEntity
    {
        public string OP_CODE { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("工序名称")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string OP_NAME { get; set; }
        #region 获取/设置 字段值
        /// <summary>
        /// AVI站点主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI站点主键")]
        public string AviId { get; set; }
        /// <summary>
        /// AVI站点编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI站点编号")]
        public string AviCd { get; set; }
        /// <summary>
        /// AVI站点名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI站点名称")]
        public string AviNm { get; set; }
       
        /// <summary>
        /// AVI站点类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI站点类型")]
        public string AviType { get; set; }
        /// <summary>
        /// AVI描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI描述")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Dsc { get; set; }
        /// <summary>
        /// IsMonitor
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否需要关键视频监控")]
        public string IsMonitor { get; set; }
        /// <summary>
        /// 是否允许重复过点
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否允许重复过点")]
        public string IsRePeated { get; set; }
        /// <summary>
        /// AVI站点是否独立
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI站点是否独立")]
        public string IsIndependence { get; set; }
        /// <summary>
        /// 产线Id
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线Id")]
        public string PlineId { get; set; }
        /// <summary>
        /// 是否报工
        /// </summary>
        /// <returns></returns>
         [DisplayName("AVI站点顺序")]
        public int AVISequence { get; set; }
        /// <summary>
        /// 是否报工
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否报工")]
        public string IsReport { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否滞留管理")]
        public int IsStranded { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        /// <returns></returns>
        [DisplayName("滞留管理类别")]
        public string StrandedCategory { get; set; }
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
        [DisplayFormat(ConvertEmptyStringToNull = false)]
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
            this.AviId = CommonHelper.GetGuid;
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
            this.AviId = KeyValue;
            this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
       
    }
}