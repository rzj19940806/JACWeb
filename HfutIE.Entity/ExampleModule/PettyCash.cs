//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2015
// Software Developers @ HfutIE 2015
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
    /// 备用金申请表
    /// <author>
    ///		<name>she</name>
    ///		<date>2015.05.15 17:37</date>
    /// </author>
    /// </summary>
    [Description("备用金申请表")]
    [PrimaryKey("PettyCashId")]
    public class PettyCash : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 备用金申请主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("备用金申请主键")]
        public string PettyCashId { get; set; }
        /// <summary>
        /// 申请部门主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("申请部门主键")]
        public string DepartmentId { get; set; }
        /// <summary>
        /// 申请金额
        /// </summary>
        /// <returns></returns>
        [DisplayName("申请金额")]
        public decimal? Amount { get; set; }
        /// <summary>
        /// 保管人
        /// </summary>
        /// <returns></returns>
        [DisplayName("保管人")]
        public string Keeper { get; set; }
        /// <summary>
        /// 保管方式
        /// </summary>
        /// <returns></returns>
        [DisplayName("保管方式")]
        public string KeepType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效")]
        public int? Enabled { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        /// <returns></returns>
        [DisplayName("排序码")]
        public int? SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        /// <returns></returns>
        [DisplayName("删除标记")]
        public int? DeleteMark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建用户主键")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建用户")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改时间")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改用户主键")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改用户")]
        public string ModifyUserName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.PettyCashId = CommonHelper.GetGuid;
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
            this.PettyCashId = KeyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = ManageProvider.Provider.Current().UserId;
            this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}