//=====================================================================================
// All Rights Reserved , Copyright @ Learun 2017
// Software Developers @ Learun 2017
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
    /// AD_MAC_INFO基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.18 15:09</date>
    /// </author>
    /// </summary>
    [Description("AD_MAC_INFO")]
    [PrimaryKey("MAC_KEY")]
    public class AD_MAC_INFO : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("MAC_KEY主键")]
        public string mac_key { get; set; }
        /// <summary>
        /// PAD编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("PAD编号")]
        public string mac_code { get; set; }
        /// <summary>
        /// PAD名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("PAD名称")]
        public string mac_name { get; set; }
        /// <summary>
        /// 人员编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("人员code")]
        public string user_code { get; set; }
        /// <summary>
        /// 人员名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("人员name")]
        public string user_name { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效")]
        public int? enable { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public DateTime? creation_time { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string mark { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.mac_key = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.mac_key = KeyValue;
                                            }
        #endregion
    }
}