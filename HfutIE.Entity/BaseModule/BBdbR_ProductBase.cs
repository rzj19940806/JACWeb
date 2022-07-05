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
    [Description("产品基础信息")]
    [PrimaryKey("MatId")]
    public class BBdbR_ProductBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 车身物料主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身物料主键")]
        public string MatId { get; set; }
        /// <summary>
        /// 产品编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("产品编号")]
        public string MatCd { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("产品名称")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MatNm { get; set; }
        /// <summary>
        /// 车型
        /// </summary>
        /// <returns></returns>
        [DisplayName("车型")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CarType { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        /// <returns></returns>
        [DisplayName("颜色")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CarColor1 { get; set; }
        /// <summary>
        /// 整车型号(公告号)
        /// </summary>
        /// <returns></returns>
        [DisplayName("整车型号(公告号)")]
        public string Notification { get; set; }
        /// <summary>
        /// 发动机排量
        /// </summary>
        /// <returns></returns>
        [DisplayName("发动机排量")]
        public string EngineOut { get; set; }
        /// <summary>
        /// 发动机型号
        /// </summary>
        /// <returns></returns>
        [DisplayName("发动机型号")]
        public string EngineModel { get; set; }
        /// <summary>
        /// 发动机最大净功率/转速
        /// </summary>
        /// <returns></returns>
        [DisplayName("发动机最大净功率/转速")]
        public string EngineMaxPower { get; set; }
        /// <summary>
        /// 最大允许总质量
        /// </summary>
        /// <returns></returns>
        [DisplayName("最大允许总质量")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string TOTAL_WEIGHT { get; set; }
        /// <summary>
        /// 载客人数
        /// </summary>
        /// <returns></returns>
        [DisplayName("载客人数")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CAPACITY { get; set; }
        /// <summary>
        /// 整车整备质量
        /// </summary>
        /// <returns></returns>
        [DisplayName("整车整备质量")]
        public string BodyWeight { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        /// <returns></returns>
        [DisplayName("用途")]
        public string Purpose { get; set; }
        /// <summary>
        /// 特殊车辆名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("特殊车辆名称")]
        public string SpecialCarNm { get; set; }
        /// <summary>
        /// 发动机额定功率
        /// </summary>
        /// <returns></returns>
        [DisplayName("发动机额定功率")]
        public string MAX_POWER_SPEED { get; set; }
        /// <summary>
        /// 燃油标识主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("燃油标识主键")]
        public string Oil { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("描述")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DIS { get; set; }
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
            this.MatId = KeyValue;
            this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}