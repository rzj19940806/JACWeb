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
    /// 车间基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.26 16:22</date>
    /// </author>
    /// </summary>
    [Description("车间基础信息表")]
    [PrimaryKey("WorkshopId")]
    public class BBdbR_WorkshopBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 车间主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("车间主键")]
        public string WorkshopId { get; set; }
        /// <summary>
        /// 工厂主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("工厂主键")]
        public string FacId { get; set; }
        /// <summary>
        /// 车间编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("车间编号")]
        public string WorkshopCd { get; set; }
        /// <summary>
        /// 车间名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("车间名称")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string WorkshopNm { get; set; }
        /// <summary>
        /// 车间类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("车间类型")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string WorkshopTyp { get; set; }
        /// <summary>
        /// 标准在制
        /// </summary>
        /// <returns></returns>
        [DisplayName("标准在制")]
        public int? WspInPro { get; set; }
        /// <summary>
        /// JPH
        /// </summary>
        /// <returns></returns>
        [DisplayName("JPH")]
        public int? WspJPH { get; set; }
        /// <summary>
        /// 缓存上限
        /// </summary>
        /// <returns></returns>
        [DisplayName("缓存上限")]
        public int? CacheQantity { get; set; }
        /// <summary>
        /// 缓存下限
        /// </summary>
        /// <returns></returns>
        [DisplayName("缓存下限")]
        public int? CacheLimit { get; set; }
        /// <summary>
        /// 最高在制
        /// </summary>
        /// <returns></returns>
        [DisplayName("最高在制")]
        public int? HighestQantity { get; set; }
        /// <summary>
        /// 最低在制
        /// </summary>
        /// <returns></returns>
        [DisplayName("最低在制")]
        public int? LowestQantity { get; set; }
        /// <summary>
        /// 负责人员主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("负责人员主键")]
        public string StfId { get; set; }
        /// <summary>
        /// 负责人员编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("负责人员编号")]
        public string StfCd { get; set; }
        /// <summary>
        /// 负责人员姓名
        /// </summary>
        /// <returns></returns>
        [DisplayName("负责人员姓名")]
        public string StfNm { get; set; }
        /// <summary>
        /// 负责手机号
        /// </summary>
        /// <returns></returns>
        [DisplayName("负责手机号")]
        public string Phn { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        /// <returns></returns>
        [DisplayName("地址")]
        public string Addr { get; set; }
        /// <summary>
        /// 车间描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("车间描述")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Dsc { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        /// <returns></returns>
        [DisplayName("版本号")]
        public string VersionNumber { get; set; }
        /// <summary>
        /// 顺序号
        /// </summary>
        /// <returns></returns>
        [DisplayName("顺序号")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string sort { get; set; }
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
            this.WorkshopId = CommonHelper.GetGuid;
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
            this.WorkshopId = KeyValue;
            this.VersionNumber = "V1.0";
            this.Enabled = "1";
            this.CreTm = DateTime.Now;
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}