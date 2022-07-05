//=====================================================================================
// All Rights Reserved , Copyright @ HfutIE 2022
// Software Developers @ HfutIE 2022
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
    /// 冲焊涂装质检结果记录过程表
    /// <author>
    ///		<name>she</name>
    ///		<date>2022.04.25 09:54</date>
    /// </author>
    /// </summary>
    [Description("冲焊涂装质检结果记录过程表")]
    [PrimaryKey("CarQualityResultId")]
    public class Q_HTCarQualityResult_Pro : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 车身部位质检结果主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("车身部位质检结果主键")]
        public string CarQualityResultId { get; set; }
        /// <summary>
        /// VIN
        /// </summary>
        /// <returns></returns>
        [DisplayName("VIN")]
        public string VIN { get; set; }
        /// <summary>
        /// 车型
        /// </summary>
        /// <returns></returns>
        [DisplayName("车型")]
        public string CarType { get; set; }
        /// <summary>
        /// 转序日期
        /// </summary>
        /// <returns></returns>
        [DisplayName("转序日期")]
        public DateTime? TransitionTm { get; set; }
        /// <summary>
        /// 质控点组主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("质控点组主键")]
        public string QualityCheckPointGroupId { get; set; }
        /// <summary>
        /// 质控点组编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("质控点组编号")]
        public string QualityCheckPointGroupCd { get; set; }
        /// <summary>
        /// 质控点组名称（工段）
        /// </summary>
        /// <returns></returns>
        [DisplayName("质控点组名称（工段）")]
        public string QualityCheckPointGroupNm { get; set; }
        /// <summary>
        /// 质控点主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("质控点主键")]
        public string QualityCheckPointId { get; set; }
        /// <summary>
        /// 质控点编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("质控点编号")]
        public string QualityCheckPointCd { get; set; }
        /// <summary>
        /// 质控点名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("质控点名称")]
        public string QualityCheckPointNm { get; set; }
        /// <summary>
        /// 质检录入时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("质检录入时间")]
        public DateTime? QualityInspectTm { get; set; }
        /// <summary>
        /// 质检结果
        /// </summary>
        /// <returns></returns>
        [DisplayName("质检结果")]
        public string QualityResult { get; set; }
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
            this.CarQualityResultId = CommonHelper.GetGuid;
            this.Enabled = "1";
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
            this.CarQualityResultId = KeyValue;
            this.MdfTm = DateTime.Now;
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}