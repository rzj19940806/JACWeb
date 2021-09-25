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
    /// 部门基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.23 19:28</date>
    /// </author>
    /// </summary>
    [Description("部门基础信息表")]
    [PrimaryKey("DepartmentID")]
    public class BBdbR_DepartmentBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 部门主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("部门主键")]
        public string DepartmentID { get; set; }
        /// <summary>
        /// 工厂主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("工厂主键")]
        public string FacId { get; set; }
        /// <summary>
        /// 部门编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("部门编号")]
        public string DepartmentCode { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("部门名称")]
        public string DepartmentName { get; set; }
        /// <summary>
        /// 父部门主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("父部门主键")]
        public string ParentDepartmentID { get; set; }
        /// <summary>
        /// 部门类别
        /// </summary>
        /// <returns></returns>
        [DisplayName("部门类别")]
        public string DepartmentType { get; set; }
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
        /// 负责人手机号
        /// </summary>
        /// <returns></returns>
        [DisplayName("负责人手机号")]
        public string Phn { get; set; }
        /// <summary>
        /// 部门描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("部门描述")]
        public string DepartmentDescription { get; set; }
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
        public int? Enabled { get; set; }
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
            this.DepartmentID = CommonHelper.GetGuid;
            this.Enabled = 1;
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
            this.DepartmentID = KeyValue;
            this.Enabled = 1;
            this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion

    }
}