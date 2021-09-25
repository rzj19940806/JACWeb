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
    /// 项目表
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.06 10:59</date>
    /// </author>
    /// </summary>
    [Description("项目表")]
    [PrimaryKey("ID")]
    public class A_ProjectInfomation : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public int? ID { get; set; }
        /// <summary>
        /// 所属订单ID
        /// </summary>
        [DisplayName("所属订单ID")]
        public int? OrderID { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        [DisplayName("产品ID")]
        public int? ProductID { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [DisplayName("产品名称")]
        public string ProductName { get; set; }
        /// <summary>
        /// 产品编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("产品编号")]
        public string ProductCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [DisplayName("项目名称")]
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("项目编号")]
        public string ProjectCode { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("项目类型")]
        public int? Type { get; set; }
        /// <summary>
        /// 产品数量
        /// </summary>
        /// <returns></returns>
        [DisplayName("产品数量")]
        public int? Num { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        /// <returns></returns>
        [DisplayName("单价")]
        public decimal Price { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        /// <returns></returns>
        [DisplayName("单位")]
        public string Unit { get; set; }
        /// <summary>
        /// 交货期
        /// </summary>
        /// <returns></returns>
        [DisplayName("交货期")]
        public DateTime? DeadLine { get; set; }
        /// <summary>
        /// 优先级
        /// </summary>
        /// <returns></returns>
        [DisplayName("优先级")]
        public int? SubPriority { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("状态")]
        public int? State { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效性")]
        public int? IsAvailable { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建人")]
        public string CreatorID { get; set; }
        /// <summary>
        /// 上次修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("上次修改时间")]
        public DateTime? LastModifiedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改人")]
        public string ModifierID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Remarks { get; set; }
        /// <summary>
        /// 预留字段
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段")]
        public string Reserve1 { get; set; }
        /// <summary>
        /// 预留字段
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段")]
        public string Reserve2 { get; set; }
        /// <summary>
        /// 预留字段
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段")]
        public string Reserve3 { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            //this.CompanyId = CommonHelper.GetGuid;
            this.CreateTime = DateTime.Now;
            this.CreatorID = ManageProvider.Provider.Current().UserId;
            //this.CreateUserName = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            //this.CompanyId = KeyValue;
            this.LastModifiedTime = DateTime.Now;
            this.ModifierID = ManageProvider.Provider.Current().UserId;
            //this.ModifyUserName = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}