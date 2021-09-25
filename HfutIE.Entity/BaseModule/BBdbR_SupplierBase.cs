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
    /// 供应商基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.10 22:44</date>
    /// </author>
    /// </summary>
    [Description("供应商基础信息表")]
    [PrimaryKey("SupplierId")]
    public class BBdbR_SupplierBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 供应商主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("供应商主键")]
        public string SupplierId { get; set; }
        /// <summary>
        /// 供应商代码
        /// </summary>
        /// <returns></returns>
        [DisplayName("供应商代码")]
        public string SupplierCd { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("供应商名称")]
        public string SupplierNm { get; set; }
        /// <summary>
        /// 供应商联系电话
        /// </summary>
        /// <returns></returns>
        [DisplayName("供应商联系电话")]
        public string SupplierTeleph { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        /// <returns></returns>
        [DisplayName("负责人")]
        public string Mgr { get; set; }
        /// <summary>
        /// 供应商类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("供应商类型")]
        public string SupplierCatg { get; set; }
        /// <summary>
        /// 供应商等级
        /// </summary>
        /// <returns></returns>
        [DisplayName("供应商等级")]
        public string SupplierGrade { get; set; }
        /// <summary>
        /// 供应商地址
        /// </summary>
        /// <returns></returns>
        [DisplayName("供应商地址")]
        public string SupplierAddress { get; set; }
        /// <summary>
        /// 供应商邮箱
        /// </summary>
        /// <returns></returns>
        [DisplayName("供应商邮箱")]
        public string SupplierEmail { get; set; }
        /// <summary>
        /// 供应商网址
        /// </summary>
        /// <returns></returns>
        [DisplayName("供应商网址")]
        public string SupplierWebsite { get; set; }
        /// <summary>
        /// 供应商描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("供应商描述")]
        public string Description { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效性")]
        public string Enabled { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        /// <returns></returns>
        [DisplayName("版本号")]
        public string VersionNumber { get; set; }
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
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public string CreTm { get; set; }
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
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改时间")]
        public string MdfTm { get; set; }
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
        /// <summary>
        /// 预留字段3
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段3")]
        public string RsvFld3 { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.SupplierId = CommonHelper.GetGuid;
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
            this.SupplierId = KeyValue;
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}