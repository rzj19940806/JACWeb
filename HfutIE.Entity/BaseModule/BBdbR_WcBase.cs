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
    /// 工位基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.27 16:34</date>
    /// </author>
    /// </summary>
    [Description("工位基础信息表")]
    [PrimaryKey("WcId")]
    public class BBdbR_WcBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 工位主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位主键")]
        public string WcId { get; set; }
        /// <summary>
        /// 产线主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线主键")]
        public string PlineId { get; set; }
        /// <summary>
        /// 工位编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位编号")]
        public string WcCd { get; set; }
        /// <summary>
        /// 工位名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位名称")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string WcNm { get; set; }
        /// <summary>
        /// 工位类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位类型")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string WcTyp { get; set; }
        /// <summary>
        /// 工位顺序
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位顺序")]
        public int? WcQueue { get; set; }
        /// <summary>
        /// 工位长度
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位长度")]
        public string WcLength { get; set; }
        /// <summary>
        /// 工位开始
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位开始")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? StartPoint { get; set; }
        /// <summary>
        /// 预警
        /// </summary>
        /// <returns></returns>
        [DisplayName("预警")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? PreAlarmPoint { get; set; }
        /// <summary>
        /// 结束
        /// </summary>
        /// <returns></returns>
        [DisplayName("结束")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? EndPoint { get; set; }
        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        [DisplayName("停止")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? StopPoint { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        /// <returns></returns>
        [DisplayName("Pass次数")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? Seq { get; set; }
        /// <summary>
        /// 工位描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位描述")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Dsc { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        /// <returns></returns>
        [DisplayName("版本号")]
        public string VersionNumber { get; set; }
        /// <summary>
        /// 顺序号
        /// </summary>
        /// <returns></returns>
        [DisplayName("顺序号")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string sort { get; set; }
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
        [DisplayFormat(ConvertEmptyStringToNull = false)]
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
            this.WcId = CommonHelper.GetGuid;
            this.VersionNumber = "V1.0";
            this.Enabled = "1";
            this.CreTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.WcId = KeyValue;
            this.VersionNumber = "V1.0";
            this.Enabled = "1";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}