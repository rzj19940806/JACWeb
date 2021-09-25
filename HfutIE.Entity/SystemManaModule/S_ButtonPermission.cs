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
    /// 按钮权限表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.04.03 09:48</date>
    /// </author>
    /// </summary>
    [Description("按钮权限表")]
    [PrimaryKey("ModulePermissionId")]
    public class S_ButtonPermission : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string ModulePermissionId { get; set; }
        /// <summary>
        /// 对象类别
        /// </summary>
        /// <returns></returns>
        [DisplayName("对象类别")]
        public string ObjectCatg { get; set; }
        /// <summary>
        /// 对象类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("对象类型")]
        public string ObjectTyp { get; set; }
        /// <summary>
        /// 对象ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("对象ID")]
        public string ObjectId { get; set; }
        /// <summary>
        /// 模块ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("模块ID")]
        public string ModuleId { get; set; }
        /// <summary>
        /// 模块按钮ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("模块按钮ID")]
        public string ModuleButtonId { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        /// <returns></returns>
        [DisplayName("排序码")]
        public int? SortCode { get; set; }
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
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ModulePermissionId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.ModulePermissionId = KeyValue;
                                            }
        #endregion
    }
}