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
    /// 操作日志表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.04.03 09:49</date>
    /// </author>
    /// </summary>
    [Description("操作日志表")]
    [PrimaryKey("SysLogId")]
    public class S_SysLog : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 日志主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("日志主键")]
        public string SysLogId { get; set; }
        /// <summary>
        /// 对象主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("对象主键")]
        public string ObjectId { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("日志类型")]
        public string LogType { get; set; }
        /// <summary>
        /// 模块主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("模块主键")]
        public string ModuleId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Remark { get; set; }
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
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改时间")]
        public DateTime? MdfTm { get; set; }
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
        /// 状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("状态")]
        public string Status { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        /// <returns></returns>
        [DisplayName("IP地址")]
        public string IPAddress { get; set; }
        /// <summary>
        /// IP地址名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("IP地址名称")]
        public string IPAddressName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.SysLogId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.SysLogId = KeyValue;
                                            }
        #endregion
    }
}