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
    /// S_PlanBoardInfo
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.10.23 23:06</date>
    /// </author>
    /// </summary>
    [Description("S_PlanBoardInfo")]
    [PrimaryKey("ID")]
    public class S_PlanBoardInfo : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// ID
        /// </summary>
        /// <returns></returns>
        [DisplayName("ID")]
        public string ID { get; set; }
        /// <summary>
        /// DayPlanNo
        /// </summary>
        /// <returns></returns>
        [DisplayName("DayPlanNo")]
        public string DayPlanNo { get; set; }
        /// <summary>
        /// DayPlanUph
        /// </summary>
        /// <returns></returns>
        [DisplayName("DayPlanUph")]
        public string DayPlanUph { get; set; }
        /// <summary>
        /// DayActualNo
        /// </summary>
        /// <returns></returns>
        [DisplayName("DayActualNo")]
        public string DayActualNo { get; set; }
        /// <summary>
        /// MouthPlanNo
        /// </summary>
        /// <returns></returns>
        [DisplayName("MouthPlanNo")]
        public string MouthPlanNo { get; set; }
        /// <summary>
        /// MouthActualNo
        /// </summary>
        /// <returns></returns>
        [DisplayName("MouthActualNo")]
        public string MouthActualNo { get; set; }

        /// <summary>
        /// DayOutNo
        /// </summary>
        /// <returns></returns>
        [DisplayName("DayOutNo")]
        public string DayOutNo { get; set; }
        /// <summary>
        /// DayWherehouseNo
        /// </summary>
        /// <returns></returns>
        [DisplayName("DayWherehouseNo")]
        public string DayWherehouseNo { get; set; }
        /// <summary>
        /// MouthOutNo
        /// </summary>
        /// <returns></returns>
        [DisplayName("MouthOutNo")]
        public string MouthOutNo { get; set; }
        /// <summary>
        /// MouthWherehouseNo
        /// </summary>
        /// <returns></returns>
        [DisplayName("MouthWherehouseNo")]
        public string MouthWherehouseNo { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        /// <returns></returns>
        [DisplayName("Type")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Type { get; set; }
        /// <summary>
        /// Label
        /// </summary>
        /// <returns></returns>
        [DisplayName("Label")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Label { get; set; }
        /// <summary>
        /// 欢迎标语
        /// </summary>
        /// <returns></returns>
        [DisplayName("WelcomeLabel")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string WelcomeLabel { get; set; }
        /// <summary>
        /// 背景颜色
        /// </summary>
        /// <returns></returns>
        [DisplayName("backgroundcolor")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string backgroundcolor { get; set; }
        /// <summary>
        /// 对齐方式
        /// </summary>
        /// <returns></returns>
        [DisplayName("alignType")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string alignType { get; set; }
        /// <summary>
        /// 字体颜色
        /// </summary>
        /// <returns></returns>
        [DisplayName("color")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string color { get; set; }
        /// <summary>
        /// 字体类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("fontfamily")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string fontfamily { get; set; }
        /// <summary>
        /// 字体是否有阴影
        /// </summary>
        /// <returns></returns>
        [DisplayName("ifShadow")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ifShadow { get; set; }
        /// <summary>
        /// 阴影颜色
        /// </summary>
        /// <returns></returns>
        [DisplayName("Shadowcolor")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Shadowcolor { get; set; }
        /// <summary>
        /// 生产看板持续时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("PlanBoardTm")]
        public int PlanBoardTm { get; set; }
        /// <summary>
        /// 欢迎界面持续时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("WelcomeTm")]
        public int WelcomeTm { get; set; }
        /// <summary>
        /// Enabled
        /// </summary>
        /// <returns></returns>
        [DisplayName("Enabled")]
        public string Enabled { get; set; }
        /// <summary>
        /// Date
        /// </summary>
        /// <returns></returns>
        [DisplayName("Date")]
        public string Date { get; set; }
        /// <summary>
        /// Rem
        /// </summary>
        /// <returns></returns>
        [DisplayName("Rem")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Rem { get; set; }
        /// <summary>
        /// CreTm
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreTm")]
        public string CreTm { get; set; }
        /// <summary>
        /// CreCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreCd")]
        public string CreCd { get; set; }
        /// <summary>
        /// CreNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreNm")]
        public string CreNm { get; set; }
        /// <summary>
        /// MdfTm
        /// </summary>
        /// <returns></returns>
        [DisplayName("MdfTm")]
        public string MdfTm { get; set; }
        /// <summary>
        /// MdfCd
        /// </summary>
        /// <returns></returns>
        [DisplayName("MdfCd")]
        public string MdfCd { get; set; }
        /// <summary>
        /// MdfNm
        /// </summary>
        /// <returns></returns>
        [DisplayName("MdfNm")]
        public string MdfNm { get; set; }
        /// <summary>
        /// RsvFld1
        /// </summary>
        /// <returns></returns>
        [DisplayName("RsvFld1")]
        public string RsvFld1 { get; set; }
        /// <summary>
        /// RsvFld2
        /// </summary>
        /// <returns></returns>
        [DisplayName("RsvFld2")]
        public string RsvFld2 { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = CommonHelper.GetGuid;
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
            this.ID = KeyValue;
            this.Enabled = "1";
            this.MdfTm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}