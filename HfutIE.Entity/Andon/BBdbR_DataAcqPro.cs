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
    /// Andon数据采集过程表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.09.11 15:17</date>
    /// </author>
    /// </summary>
    [Description("Andon数据采集过程表")]
    [PrimaryKey("RecId")]
    public class BBdbR_DataAcqPro : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 记录主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("记录主键")]
        public string RecId { get; set; }
        /// <summary>
        /// 停线主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("停线主键")]
        public string BPIERecId { get; set; }
        /// <summary>
        /// 产线主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("产线主键")]
        public string PlineId { get; set; }
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
        /// 工位主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位主键")]
        public string WcId { get; set; }
        /// <summary>
        /// 工位编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位主键")]
        public string WcCd { get; set; }
        /// <summary>
        /// 工位名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("工位名称")]
        public string WcNm { get; set; }
        /// <summary>
        /// 岗位主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("岗位主键")]
        public string PostId { get; set; }
        /// <summary>
        /// KepSId
        /// </summary>
        /// <returns></returns>
        [DisplayName("KepSId")]
        public string KepServerId { get; set; }
        /// <summary>
        /// KepSName
        /// </summary>
        /// <returns></returns>
        [DisplayName("KepSName")]
        public string KepServerNm { get; set; }
        /// <summary>
        /// KepSNode(IP)
        /// </summary>
        /// <returns></returns>
        [DisplayName("KepSNode(IP)")]
        public string KepServerNd { get; set; }
        /// <summary>
        /// KepServer来源
        /// </summary>
        /// <returns></returns>
        [DisplayName("KepServer来源")]
        public string KepServerSource { get; set; }
        /// <summary>
        /// 是否监控
        /// </summary>
        /// <returns></returns>
        [DisplayName("是否监控")]
        public string IsMonitoring { get; set; }
        /// <summary>
        /// 监控频率
        /// </summary>
        /// <returns></returns>
        [DisplayName("监控频率")]
        public string MonitoringRate { get; set; }
        /// <summary>
        /// 控制地址描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("控制地址描述")]
        public string CntlAddrDsc { get; set; }
        
        /// <summary>
        /// 信号类别
        /// </summary>
        /// <returns></returns>
        [DisplayName("信号类别")]
        public string SignalTyp { get; set; }
        /// <summary>
        /// 信号来源
        /// </summary>
        /// <returns></returns>
        [DisplayName("信号来源")]
        public string SignalSource { get; set; }
        /// <summary>
        /// 信号详情
        /// </summary>
        /// <returns></returns>
        [DisplayName("信号详情")]
        public string SignalDetail { get; set; }
        /// <summary>
        /// 当前值
        /// </summary>
        /// <returns></returns>
        [DisplayName("当前值")]
        public string CurValue { get; set; }
        /// <summary>
        /// 采集时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("采集时间")]
        public string AcqTm { get; set; }
        /// <summary>
        /// 处理状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("处理状态")]
        public string HandleSts { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("结束时间")]
        public string EndTm { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("处理时间")]
        public string HandleTm { get; set; }
        /// <summary>
        /// 处理人编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("处理人编号")]
        public string HandlerCode { get; set; }
        /// <summary>
        /// 处理人姓名
        /// </summary>
        /// <returns></returns>
        [DisplayName("处理人姓名")]
        public string HandlerName { get; set; }
        /// <summary>
        /// 异常类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("异常类型")]
        public string ExceptionType { get; set; }
        /// <summary>
        /// 异常描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("异常描述")]
        public string ExceptionDes { get; set; }
        /// <summary>
        /// 处理结果
        /// </summary>
        /// <returns></returns>
        [DisplayName("处理结果")]
        public string HandleResult { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效性")]
        public string Enabled { get; set; }
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
            this.RecId = CommonHelper.GetGuid;
            this.AcqTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.Enabled = "1";
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.RecId = KeyValue;
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}