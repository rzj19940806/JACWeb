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
    /// 产线基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.27 09:17</date>
    /// </author>
    /// </summary>
    [Description("产线基础信息表")]
    [PrimaryKey("PlineId")]
    public class BBdbR_PlineBase : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 产线主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线主键")]
        public string PlineId { get; set; }
        /// <summary>
        /// 工段主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("工段主键")]
        public string WorkSectionId { get; set; }
        /// <summary>
        /// 产线编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线编号")]
        public string PlineCd { get; set; }
        /// <summary>
        /// 产线名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线名称")]
        public string PlineNm { get; set; }
        /// <summary>
        /// 产线类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线类型")]
        public string PlineTyp { get; set; }
        /// <summary>
        /// 工位数量
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位数量")]
        public int? WcQuantity { get; set; }
        /// <summary>
        /// 工位默认长度
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位默认长度")]
        public decimal? WcLength { get; set; }
        /// <summary>
        /// 工位默认截距
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位默认截距")]
        public decimal? WcIntercept { get; set; }
        /// <summary>
        /// JPH
        /// </summary>
        /// <returns></returns>
        [DisplayName("JPH")]
        public decimal? WspJPH { get; set; }
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
        /// 预警位
        /// </summary>
        /// <returns></returns>
        [DisplayName("预警位")]
        public int? PreAlarmPoint { get; set; }
        /// <summary>
        /// 停止位
        /// </summary>
        /// <returns></returns>
        [DisplayName("停止位")]
        public int? EndPoint { get; set; }
        /// <summary>
        /// 运行模式
        /// </summary>
        /// <returns></returns>
        [DisplayName("运行模式")]
        public string RunningMode { get; set; }
        /// <summary>
        /// 开始
        /// </summary>
        /// <returns></returns>
        [DisplayName("开始")]
        public decimal? StationBegion { get; set; }
        /// <summary>
        /// 结束
        /// </summary>
        /// <returns></returns>
        [DisplayName("结束")]
        public decimal? StationEnd { get; set; }
        /// <summary>
        /// 前缓存上限
        /// </summary>
        /// <returns></returns>
        [DisplayName("前缓存上限")]
        public int? HighestFrontCache { get; set; }
        /// <summary>
        /// 前缓存下限
        /// </summary>
        /// <returns></returns>
        [DisplayName("前缓存下限")]
        public int? LowestFrontCache { get; set; }
        /// <summary>
        /// 后缓存上限
        /// </summary>
        /// <returns></returns>
        [DisplayName("后缓存上限")]
        public int? HighestPostCache { get; set; }
        /// <summary>
        /// 后缓存下限
        /// </summary>
        /// <returns></returns>
        [DisplayName("后缓存下限")]
        public int? LowestPostCache { get; set; }
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
        /// 产线描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线描述")]
        public string Dsc { get; set; }
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
            this.PlineId = CommonHelper.GetGuid;
            this.Enabled = "1";
            this.VersionNumber = "V1.0";
            this.CreTm = DateTime.Now;
            this.CreCd = ManageProvider.Provider.Current().UserId;
            this.CreNm = ManageProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.PlineId = KeyValue;
            this.MdfTm = DateTime.Now;
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}