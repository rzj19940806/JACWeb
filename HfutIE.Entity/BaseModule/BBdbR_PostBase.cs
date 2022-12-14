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
    /// 岗位基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.23 20:57</date>
    /// </author>
    /// </summary>
    [Description("岗位基础信息表")]
    [PrimaryKey("PostId")]
    public class BBdbR_PostBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 岗位主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("岗位主键")]
        public string PostId { get; set; }
        /// <summary>
        /// 工位主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位主键")]
        public string WcId { get; set; }
        /// <summary>
        /// 岗位编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("岗位编号")]
        public string PostCd { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("岗位名称")]
        public string PostNm { get; set; }
        /// <summary>
        /// 岗位类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("岗位类型")]
        public string PostType { get; set; }
        /// <summary>
        /// 岗位位置
        /// </summary>
        /// <returns></returns>
        [DisplayName("岗位位置")]
        public string PostPosition { get; set; }
        /// <summary>
        /// 开始
        /// </summary>
        /// <returns></returns>
        [DisplayName("开始")]
        public int? StartPoint { get; set; }
        /// <summary>
        /// 预警
        /// </summary>
        /// <returns></returns>
        [DisplayName("预警")]
        public int? PreAlarmPoint { get; set; }
        /// <summary>
        /// 结束
        /// </summary>
        /// <returns></returns>
        [DisplayName("结束")]
        public int? EndPoint { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        /// <returns></returns>
        [DisplayName("版本号")]
        public string VersionNumber { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效性")]
        public string Enabled { get; set; }
        /// <summary>
        /// 创建时间/*int?*/
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public string CreTm { get; set; }
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
        public string MdfTm { get; set; }
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
        /// <summary>
        /// 预留字段1
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段1")]
        public string RsvFld1 { get; set; }
        /// <summary>
        /// 预留字段2
        /// </summary>
        /// <returns></returns>
        [DisplayName("预留字段2")]
        public string RsvFld2 { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.PostId = CommonHelper.GetGuid;
            this.VersionNumber = "V1.0";
            this.CreTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.Enabled = "1";
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            //this.ConfigId = KeyValue;
            this.VersionNumber = "V1.0";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}