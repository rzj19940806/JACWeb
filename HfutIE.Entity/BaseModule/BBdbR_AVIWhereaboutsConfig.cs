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
    /// AVI去向配置信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.31 12:49</date>
    /// </author>
    /// </summary>
    [Description("AVI去向配置信息表")]
    [PrimaryKey("AVIWhereId")]
    public class BBdbR_AVIWhereaboutsConfig : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string AVIWhereId { get; set; }
        /// <summary>
        /// AVI主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI主键")]
        public string AviId { get; set; }
        /// <summary>
        /// 去向产线主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI站点顺序")]
        public int AVISequence { get; set; }
        /// <summary>
        /// 去向产线标识
        /// </summary>
        /// <returns></returns>
        [DisplayName("去向AVI站点主键")]
        public string ToAVIId { get; set; }
        /// <summary>
        /// 去向产线标识
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI站点编号")]
        public string ToAVICd { get; set; }
        /// <summary>
        /// 去向产线标识
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI站点名称")]
        public string ToAVINm { get; set; }
        /// <summary>
        /// 去向产线标识
        /// </summary>
        /// <returns></returns>
        [DisplayName("AVI站点是否独立")]
        public int IsIndependence { get; set; }
        /// <summary>
        /// 去向产线标识
        /// </summary>
        /// <returns></returns>
        [DisplayName("去向AVI站点顺序")]
        public int ToAVISequence { get; set; }
        /// <summary>
        /// 去向产线标识
        /// </summary>
        /// <returns></returns>
        [DisplayName("去向产线编号")]
        public string ToPlineCd { get; set; }
        /// <summary>
        /// 去向产线标识
        /// </summary>
        /// <returns></returns>
        [DisplayName("去向产线名称")]
        public string ToPlineNm { get; set; }
        /// <summary>
        /// 去向产线标识
        /// </summary>
        /// <returns></returns>
        [DisplayName("去向产线主键")]
        public string PlineId { get; set; }
        /// <summary>
        /// 去向产线标识
        /// </summary>
        /// <returns></returns>
        [DisplayName("去向产线标识")]
        public string PlineMark { get; set; }
        /// <summary>
        /// 队列差
        /// </summary>
        /// <returns></returns>
        [DisplayName("队列差")]
        public string Queue { get; set; }
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
            this.AVIWhereId = CommonHelper.GetGuid;
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
            this.AVIWhereId = KeyValue;
            this.VersionNumber = "V1.0";
            this.Enabled = "1";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}