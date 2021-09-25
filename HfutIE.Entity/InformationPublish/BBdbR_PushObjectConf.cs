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
    /// 信息推送对象配置表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.03.24 09:38</date>
    /// </author>
    /// </summary>
    [Description("信息推送对象配置表")]
    [PrimaryKey("RecId")]
    public class BBdbR_PushObjectConf : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string RecId { get; set; }
        /// <summary>
        /// 推送类型编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("推送类型编号")]
        public string InforTypCd { get; set; }
        /// <summary>
        /// 推送类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("推送类型")]
        public string InforTyp { get; set; }
        /// <summary>
        /// 推送对象类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("推送对象类型")]
        public string ObjectTyp { get; set; }
        /// <summary>
        /// 推送对象Id
        /// </summary>
        /// <returns></returns>
        [DisplayName("推送对象Id")]
        public string ObjectId { get; set; }
        /// <summary>
        /// 推送等级
        /// </summary>
        /// <returns></returns>
        [DisplayName("推送等级")]
        public string PushRank { get; set; }
        /// <summary>
        /// 间隔时长
        /// </summary>
        /// <returns></returns>
        [DisplayName("间隔时长")]
        public decimal? IntvlTm { get; set; }
        /// <summary>
        /// 时长单位
        /// </summary>
        /// <returns></returns>
        [DisplayName("时长单位")]
        public string TmUnit { get; set; }
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
            this.RecId = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.RecId = KeyValue;
                                            }
        #endregion
    }
}