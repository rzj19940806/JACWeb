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
    /// 群聊基础表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.25 13:34</date>
    /// </author>
    /// </summary>
    [Description("群聊基础表")]
    [PrimaryKey("GroupchatId")]
    public class BBdbR_GroupChat : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 群ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("群ID")]
        public string GroupchatId { get; set; }
        /// <summary>
        /// 群名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("群名称")]
        public string GroupchatNm { get; set; }
        /// <summary>
        /// 群号
        /// </summary>
        /// <returns></returns>
        [DisplayName("群号")]
        public string Qrcode { get; set; }
        /// <summary>
        /// 群地址
        /// </summary>
        /// <returns></returns>
        [DisplayName("群地址")]
        public string GroupchatAddr { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效性")]
        public string Enabled { get; set; }
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
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Rem { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.GroupchatId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.GroupchatId = KeyValue;
                                            }
        #endregion
    }
}