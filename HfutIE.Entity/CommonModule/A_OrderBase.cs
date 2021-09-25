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
    /// 公司管理
    /// <author>
    ///		<name>she</name>
    ///		<date>2014.08.06 10:59</date>
    /// </author>
    /// </summary>
    [Description("订单主表")]
    [PrimaryKey("ID")]
    public class A_OrderBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public int? ID { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        [DisplayName("订单编号")]
        public string OrderCode { get; set; }
        /// <summary>
        /// 订单名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单名称")]
        public string OrderName { get; set; }
        /// <summary>
        /// 订单描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单描述")]
        public string Description { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("客户ID")]
        public int? CustomerID { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("客户编号")]
        public string CustomerCode { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("客户名称")]
        public string CustomerName { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单状态")]
        public int? State { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("订单类型")]
        public int? OrderType { get; set; }
        /// <summary>
        /// 优先级
        /// </summary>
        /// <returns></returns>
        [DisplayName("优先级")]
        public int? Priority { get; set; }
        /// <summary>
        /// 技术负责人
        /// </summary>
        /// <returns></returns>
        [DisplayName("技术负责人")]
        public string TechnicalDirector { get; set; }
        /// <summary>
        /// 生产负责人
        /// </summary>
        /// <returns></returns>
        [DisplayName("生产负责人")]
        public string ManufacturingDirector { get; set; }
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