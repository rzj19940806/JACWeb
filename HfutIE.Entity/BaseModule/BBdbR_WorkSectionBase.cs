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
    /// 工艺段基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.24 17:14</date>
    /// </author>
    /// </summary>
    [Description("工艺段基础信息表")]
    [PrimaryKey("WorkSectionId")]
    public class BBdbR_WorkSectionBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 工段主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("工段主键")]
        public string WorkSectionId { get; set; }
        /// <summary>
        /// 车间主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("车间主键")]
        public string WorkshopId { get; set; }
        /// <summary>
        /// 工段编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("工段编号")]
        public string WorkSectionCd { get; set; }
        /// <summary>
        /// 工段名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("工段名称")]
        public string WorkSectionNm { get; set; }
        /// <summary>
        /// 工段类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("工段类型")]
        public string WorkSectionTy { get; set; }
        /// <summary>
        /// JPH
        /// </summary>
        /// <returns></returns>
        [DisplayName("JPH")]
        public int? WspJPH { get; set; }
        /// <summary>
        /// 缓存上限
        /// </summary>
        /// <returns></returns>
        [DisplayName("缓存上限")]
        public int? CacheQantity { get; set; }
        /// <summary>
        /// 缓存下限
        /// </summary>
        /// <returns></returns>
        [DisplayName("缓存下限")]
        public int? CacheLimit { get; set; }
        /// <summary>
        /// 最高在制
        /// </summary>
        /// <returns></returns>
        [DisplayName("最高在制")]
        public int? HighestQantity { get; set; }
        /// <summary>
        /// 最低在制
        /// </summary>
        /// <returns></returns>
        [DisplayName("最低在制")]
        public int? LowestQantity { get; set; }
        /// <summary>
        /// 负责人员主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("负责人员主键")]
        public string StfId { get; set; }
        /// <summary>
        /// 负责人员编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("负责人员编号")]
        public string StfCd { get; set; }
        /// <summary>
        /// 负责人员姓名
        /// </summary>
        /// <returns></returns>
        [DisplayName("负责人员姓名")]
        public string StfNm { get; set; }
        /// <summary>
        /// 负责人手机号
        /// </summary>
        /// <returns></returns>
        [DisplayName("负责人手机号")]
        public string Phn { get; set; }
        /// <summary>
        /// 工段描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("工段描述")]
        public string WorkSecDsc { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效性")]
        public string Enabled { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        /// <returns></returns>
        [DisplayName("版本号")]
        public string VersionNumber { get; set; }
        /// <summary>
        /// 创建时间
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
            this.WorkSectionId = CommonHelper.GetGuid;
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