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
    /// 燃油标识信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.14 17:00</date>
    /// </author>
    /// </summary>
    [Description("燃油标识信息表")]
    [PrimaryKey("GID")]
    public class AUEX_NEW_FUELOIL_ID : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string GID { get; set; }
        /// <summary>
        /// 标识编码
        /// </summary>
        /// <returns></returns>
        [DisplayName("标识编码")]
        public string IDCODE { get; set; }
        /// <summary>
        /// 生产企业
        /// </summary>
        /// <returns></returns>
        [DisplayName("生产企业")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MANUFACTURER { get; set; }
        /// <summary>
        /// 变速器类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("变速器类型")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string TRANSMISSION_TYPE { get; set; }
        /// <summary>
        /// 能源种类
        /// </summary>
        /// <returns></returns>
        [DisplayName("能源种类")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FUEL_TYPE { get; set; }
        /// <summary>
        /// 整车整备质量
        /// </summary>
        /// <returns></returns>
        [DisplayName("整车整备质量")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ENTIRE_QUALITY { get; set; }
        /// <summary>
        /// 排量
        /// </summary>
        /// <returns></returns>
        [DisplayName("排量")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DISPLACEMENT { get; set; }
        /// <summary>
        /// 最大设计总质量
        /// </summary>
        /// <returns></returns>
        [DisplayName("最大设计总质量")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DESIGN_QUALITY { get; set; }
        /// <summary>
        /// 最大净功率
        /// </summary>
        /// <returns></returns>
        [DisplayName("最大净功率")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MAX_POWERS { get; set; }
        /// <summary>
        /// 驱动型式
        /// </summary>
        /// <returns></returns>
        [DisplayName("驱动型式")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DRIVE_TYPE { get; set; }
        /// <summary>
        /// 其他信息
        /// </summary>
        /// <returns></returns>
        [DisplayName("其他信息")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ADDTIONAL_INFORMATION { get; set; }
        /// <summary>
        /// 驱动电机峰值功率
        /// </summary>
        /// <returns></returns>
        [DisplayName("驱动电机峰值功率")]
        public string POWER_DRIVE_MOTOR { get; set; }
        /// <summary>
        /// 市区工况
        /// </summary>
        /// <returns></returns>
        [DisplayName("市区工况")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string URBAN_OPERATING_MODE { get; set; }
        /// <summary>
        /// 综合工况
        /// </summary>
        /// <returns></returns>
        [DisplayName("综合工况")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string INTEGRATED_OPERATING_MODE { get; set; }
        /// <summary>
        /// 综合工况电能消耗量
        /// </summary>
        /// <returns></returns>
        [DisplayName("综合工况电能消耗量")]
        public string INTEGRATED_CONSUMPTION { get; set; }
        /// <summary>
        /// 电能当量燃料消耗量
        /// </summary>
        /// <returns></returns>
        [DisplayName("电能当量燃料消耗量")]
        public string ELECTRI_CONSUMPTION { get; set; }
        /// <summary>
        /// 市郊工况
        /// </summary>
        /// <returns></returns>
        [DisplayName("市郊工况")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SUBURBAN_OPERATING_MODE { get; set; }
        /// <summary>
        /// 领跑值
        /// </summary>
        /// <returns></returns>
        [DisplayName("领跑值")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LEADVALUE { get; set; }
        /// <summary>
        /// 限值
        /// </summary>
        /// <returns></returns>
        [DisplayName("限值")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LIMIT { get; set; }
        /// <summary>
        /// 备案号
        /// </summary>
        /// <returns></returns>
        [DisplayName("备案号")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RECORD_NUMBER { get; set; }
        /// <summary>
        /// 启用日期
        /// </summary>
        /// <returns></returns>
        [DisplayName("启用日期")]
        public string OPENING_DATE { get; set; }
        /// <summary>
        /// 国标编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("国标编号")]
        public string GB_NO { get; set; }
        /// <summary>
        /// 车辆型号
        /// </summary>
        /// <returns></returns>
        [DisplayName("车辆型号")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PRODUCT_VEHICLE_CODE { get; set; }
        /// <summary>
        /// 发动机型号
        /// </summary>
        /// <returns></returns>
        [DisplayName("发动机型号")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ENGINE_MODEL { get; set; }
        /// <summary>
        /// 续航里程
        /// </summary>
        /// <returns></returns>
        [DisplayName("续航里程")]
        public string RANGE { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效性")]
        public string Enabled { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建人")]
        public string CREATE_ID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public DateTime? CREATE_DATE { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人")]
        public string MODIFY_ID { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改时间")]
        public DateTime? MODIFY_DATE { get; set; }
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
            this.CREATE_DATE = DateTime.Now;
            this.CREATE_ID = ManageProvider.Provider.Current().UserId;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.MODIFY_DATE = DateTime.Now;
            this.MODIFY_ID = ManageProvider.Provider.Current().UserId;

        }
        #endregion
    }
}