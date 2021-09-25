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
    /// 人员基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.26 15:42</date>
    /// </author>
    /// </summary>
    [Description("人员基础信息表")]
    [PrimaryKey("StfId")]
    public class BBdbR_StfBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 人员主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("人员主键")]
        public string StfId { get; set; }
        /// <summary>
        /// 部门主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("部门主键")]
        public string DepartmentID { get; set; }
        /// <summary>
        /// 人员编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("人员编号")]
        public string StfCd { get; set; }
        /// <summary>
        /// 人员姓名
        /// </summary>
        /// <returns></returns>
        [DisplayName("人员姓名")]
        public string StfNm { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        /// <returns></returns>
        [DisplayName("性别")]
        public string StfGndr { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        /// <returns></returns>
        [DisplayName("手机号")]
        public string Phn { get; set; }
        /// <summary>
        /// 企业微信号
        /// </summary>
        /// <returns></returns>
        [DisplayName("企业微信号")]
        public string Wechat { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        /// <returns></returns>
        [DisplayName("邮箱")]
        public string Email { get; set; }
        /// <summary>
        /// 人员类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("人员类型")]
        public string StfTyp { get; set; }
        /// <summary>
        /// 人员职位
        /// </summary>
        /// <returns></returns>
        [DisplayName("人员职位")]
        public string StfPosn { get; set; }
        /// <summary>
        /// 人员职称
        /// </summary>
        /// <returns></returns>
        [DisplayName("人员职称")]
        public string StfTitl { get; set; }
        /// <summary>
        /// 人员图片
        /// </summary>
        /// <returns></returns>
        [DisplayName("人员图片")]
        public string StfImg { get; set; }
        /// <summary>
        /// 图片类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("图片类型")]
        public string ImgTyp { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        /// <returns></returns>
        [DisplayName("账号")]
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        /// <returns></returns>
        [DisplayName("密码")]
        public string Pssw { get; set; }
        /// <summary>
        /// 秘钥
        /// </summary>
        /// <returns></returns>
        [DisplayName("秘钥")]
        public string Sctkey { get; set; }
        /// <summary>
        /// 修改密码时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改密码时间")]
        public DateTime? ChangePasswordDate { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        /// <returns></returns>
        [DisplayName("登录次数")]
        public int? LogOnCount { get; set; }
        /// <summary>
        /// 最后之前登录时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("最后之前登录时间")]
        public DateTime? PreviousVisit { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("最后登录时间")]
        public DateTime? LastVisit { get; set; }
        /// <summary>
        /// 最后登录ip
        /// </summary>
        /// <returns></returns>
        [DisplayName("最后登录ip")]
        public string LastLoginIp { get; set; }
        /// <summary>
        /// 在线情况
        /// </summary>
        /// <returns></returns>
        [DisplayName("在线情况")]
        public int? Online { get; set; }
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
            this.StfId = CommonHelper.GetGuid;
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
            //this.StfId = KeyValue;
            this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}