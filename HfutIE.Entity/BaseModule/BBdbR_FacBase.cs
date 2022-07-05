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
    /// 工厂基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.26 15:22</date>
    /// </author>
    /// </summary>
    [Description("工厂基础信息表")]
    [PrimaryKey("FacId")]
    public class BBdbR_FacBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 工厂主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("工厂主键")]
        public string FacId { get; set; }
        /// <summary>
        /// 工厂编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("工厂编号")]
        public string FacCd { get; set; }
        /// <summary>
        /// 工厂名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("工厂名称")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FacNm { get; set; }
        /// <summary>
        /// 公司主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("公司主键")]
        public string CompanyId { get; set; }
        /// <summary>
        /// 工厂类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("工厂类型")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FacTyp { get; set; }
        /// <summary>
        /// 工厂地址
        /// </summary>
        /// <returns></returns>
        [DisplayName("工厂地址")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Addr { get; set; }
        /// <summary>
        /// 工厂描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("工厂描述")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Dsc { get; set; }
        /// <summary>
        /// 负责人员主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("负责人员主键")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StfId { get; set; }
        /// <summary>
        /// 负责人员编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("负责人员编号")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StfCd { get; set; }
        /// <summary>
        /// 负责人员姓名
        /// </summary>
        /// <returns></returns>
        [DisplayName("负责人员姓名")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StfNm { get; set; }
        /// <summary>
        /// 负责手机号
        /// </summary>
        /// <returns></returns>
        [DisplayName("负责手机号")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Phn { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        /// <returns></returns>
        [DisplayName("联系电话")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FacTelephone { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        /// <returns></returns>
        [DisplayName("传真")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FacFax { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        /// <returns></returns>
        [DisplayName("邮箱")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FacEmail { get; set; }
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
            this.FacId = CommonHelper.GetGuid;
            this.Enabled = "1";
            this.VersionNumber = "V1.0";
            this.CreTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.FacId = KeyValue;
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}