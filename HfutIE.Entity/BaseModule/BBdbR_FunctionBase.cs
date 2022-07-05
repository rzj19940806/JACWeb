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
    /// 功能基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.10.03 20:27</date>
    /// </author>
    /// </summary>
    [Description("功能基础信息表")]
    [PrimaryKey("FunctionId")]
    public class BBdbR_FunctionBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string FunctionId { get; set; }
        /// <summary>
        /// 功能编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("功能编号")]
        public string FunctionCd { get; set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("功能名称")]
        public string FunctionNm { get; set; }
        /// <summary>
        /// 功能描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("功能描述")]
        public string FunctionDsc { get; set; }
        /// <summary>
        /// 功能类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("功能类型")]
        public int? FunctionType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Rem { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效性")]
        public string Enable { get; set; }
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
        public string CreatorCode { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建人名称")]
        public string CreatorName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public string CreateTime { get; set; }
        /// <summary>
        /// 修改人编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人编号")]
        public string ModifierCode { get; set; }
        /// <summary>
        /// 修改人名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人名称")]
        public string ModifierName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改时间")]
        public string ModifyTime { get; set; }
        /// <summary>
        /// 预留字段1
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段1")]
        public string ReserveFiled1 { get; set; }
        /// <summary>
        /// 预留字段2
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段2")]
        public string ReserveFiled2 { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.FunctionId = CommonHelper.GetGuid;
            this.VersionNumber = "V1.0";
            this.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.Enable = "1";
            this.CreatorCode = ManageProvider.Provider.Current().UserId;
            this.CreatorName = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.FunctionId = KeyValue;
            this.VersionNumber = "V1.0";
            this.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.Enable = "1";
            this.CreatorCode = ManageProvider.Provider.Current().UserId;
            this.CreatorName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}