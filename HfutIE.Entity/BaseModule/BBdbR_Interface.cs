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
    /// 接口基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.12.14 21:17</date>
    /// </author>
    /// </summary>
    [Description("接口基础信息表")]
    [PrimaryKey("InterfaceId")]
    public class BBdbR_Interface : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string InterfaceId { get; set; }
        /// <summary>
        /// 接口编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("接口编号")]
        public string InterfaceCd { get; set; }
        /// <summary>
        /// 接口名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("接口名称")]
        public string InterfaceNm { get; set; }
        /// <summary>
        /// 接口类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("接口类型")]
        public string InterfaceType { get; set; }
        /// <summary>
        /// 发送方
        /// </summary>
        /// <returns></returns>
        [DisplayName("发送方")]
        public string Sender { get; set; }
        /// <summary>
        /// 接收方
        /// </summary>
        /// <returns></returns>
        [DisplayName("接收方")]
        public string Receiver { get; set; }
        /// <summary>
        /// 执行方式
        /// </summary>
        /// <returns></returns>
        [DisplayName("执行方式")]
        public string ExecutionMode { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("执行时间")]
        public DateTime? ExecutionTime { get; set; }
        /// <summary>
        /// 执行间隔(分钟)
        /// </summary>
        /// <returns></returns>
        [DisplayName("执行间隔(分钟)")]
        public int InterfaceTime { get; set; }
        /// <summary>
        /// 上次执行时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("上次执行时间")]
        public DateTime? LastTime { get; set; }
        /// <summary>
        /// 下次执行时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("下次执行时间")]
        public DateTime? NextTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("状态")]
        public string Status { get; set; }
        /// <summary>
        /// SQL语句
        /// </summary>
        /// <returns></returns>
        [DisplayName("SQL语句")]
        public string Sql { get; set; }
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
        public string Enabled { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.InterfaceId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.InterfaceId = KeyValue;
                                            }
        #endregion
    }
}