//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2014
// Software Developers @ HfutIE 2014
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
    /// 角色管理表
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.07 15:34</date>
    /// </author>
    /// </summary>
    [Description("角色管理表")]
    [PrimaryKey("RoleId")]
    public class Base_Roles : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string RoleId { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("公司ID")]
        public string CompanyId { get; set; }
        /// <summary>
        /// 角色类别
        /// </summary>
        /// <returns></returns>
        [DisplayName("角色类别")]
        public string Category { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("角色编号")]
        public string Code { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("角色名称")]
        public string FullName { get; set; }
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
        public int? Enabled { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        /// <returns></returns>
        [DisplayName("排序码")]
        public int? SortCode { get; set; }
        /// <summary>
        /// DeleteMark
        /// </summary>
        /// <returns></returns>
        [DisplayName("DeleteMark")]
        public int? DeleteMark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建人ID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建人名称")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 上次修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("上次修改时间")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改人ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人ID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改人名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人名称")]
        public string ModifyUserName { get; set; }

        ///// <summary>
        ///// 预留字段
        ///// </summary>
        ///// <returns></returns>
        //[DisplayName("预留字段")]
        //public string Reserve1 { get; set; }
        ///// <summary>
        ///// 预留字段
        ///// </summary>
        ///// <returns></returns>
        //[DisplayName("预留字段")]
        //public string Reserve2 { get; set; }
        ///// <summary>
        ///// 预留字段
        ///// </summary>
        ///// <returns></returns>
        //[DisplayName("预留字段")]
        //public string Reserve3 { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.RoleId = CommonHelper.GetGuid;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = ManageProvider.Provider.Current().UserId;
            this.CreateUserName = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.RoleId = KeyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}