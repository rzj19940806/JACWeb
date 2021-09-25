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
    /// 系统异常日志表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.04.03 09:50</date>
    /// </author>
    /// </summary>
    [Description("系统异常日志表")]
    [PrimaryKey("LogErrId")]
    public class S_LogErr : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string LogErrId { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("记录时间")]
        public DateTime? RecTm { get; set; }
        /// <summary>
        /// 方法名=>异常堆栈
        /// </summary>
        /// <returns></returns>
        [DisplayName("方法名=>异常堆栈")]
        public string MethNm { get; set; }
        /// <summary>
        /// 方法参数
        /// </summary>
        /// <returns></returns>
        [DisplayName("错误来源")]
        public string ErrFrom { get; set; }
        /// <summary>
        /// 错误详情
        /// </summary>
        /// <returns></returns>
        [DisplayName("错误详情")]
        public string ErrDetl { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Rem { get; set; }        
        /// <summary>
        /// 预留字段
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段")]
        public string RsvFld1 { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.LogErrId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.LogErrId = KeyValue;
                                            }
        #endregion
    }
}