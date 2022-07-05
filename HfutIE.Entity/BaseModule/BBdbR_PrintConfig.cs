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
    /// 打印配置表
    /// <author>
    ///		<name>she</name>
    ///		<date>2022.04.22 10:43</date>
    /// </author>
    /// </summary>
    [Description("打印配置表")]
    [PrimaryKey("Id")]
    public class BBdbR_PrintConfig : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// Id
        /// </summary>
        /// <returns></returns>
        [DisplayName("Id")]
        public string Id { get; set; }
        /// <summary>
        /// PridId
        /// </summary>
        /// <returns></returns>
        [DisplayName("PridId")]
        public string PridId { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("编号")]
        public string PrintConfigCd { get; set; }
        /// <summary>
        /// 类型0：普通，1：表格，1：条码
        /// </summary>
        /// <returns></returns>
        [DisplayName("类型0：普通，1：表格，1：条码")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? Type { get; set; }
        /// <summary>
        /// 左侧距离(序号及物料相对于表格，其余相对于边框)
        /// </summary>
        /// <returns></returns>
        [DisplayName("左侧距离(序号及物料相对于表格，其余相对于边框)")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? X { get; set; }
        /// <summary>
        /// 左侧距离(序号及物料相对于表格，其余相对于边框)
        /// </summary>
        /// <returns></returns>
        [DisplayName("左侧距离(序号及物料相对于表格，其余相对于边框)")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MX { get; set; }
        /// <summary>
        /// 顶部距离(序号及物料相对于表格，其余相对于边框)
        /// </summary>
        /// <returns></returns>
        [DisplayName("顶部距离(序号及物料相对于表格，其余相对于边框)")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? Y { get; set; }
        /// <summary>
        /// 顶部距离(序号及物料相对于表格，其余相对于边框)
        /// </summary>
        /// <returns></returns>
        [DisplayName("顶部距离(序号及物料相对于表格，其余相对于边框)")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MY { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        /// <returns></returns>
        [DisplayName("宽度")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? Width { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        /// <returns></returns>
        [DisplayName("宽度")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MWidth { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        /// <returns></returns>
        [DisplayName("高度")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? Height { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        /// <returns></returns>
        [DisplayName("高度")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MHeight { get; set; }
        /// <summary>
        /// 边框粗细
        /// </summary>
        /// <returns></returns>
        [DisplayName("边框粗细")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? Board { get; set; }

        /// <summary>
        /// 自定义内容
        /// </summary>
        /// <returns></returns>
        [DisplayName("自定义内容")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Context { get; set; }

        /// <summary>
        /// 字号
        /// </summary>
        /// <returns></returns>
        [DisplayName("字号")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? FontSize { get; set; }
        /// <summary>
        /// 字体
        /// </summary>
        /// <returns></returns>
        [DisplayName("字体")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FontFamily { get; set; }
        /// <summary>
        /// 字形—0：普通；1：加粗；2：倾斜；4：下划线；8：删除线
        /// </summary>
        /// <returns></returns>
        [DisplayName("字形—0：普通；1：加粗；2：倾斜；4：下划线；8：删除线")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? FontStyle { get; set; }
        /// <summary>
        /// ARGB
        /// </summary>
        /// <returns></returns>
        [DisplayName("ARGB")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ARGB { get; set; }
        /// <summary>
        /// 水平对齐 0：居左，1：居中，2：居右，当类型为条码是为条码宽度
        /// </summary>
        /// <returns></returns>
        [DisplayName("水平对齐 0：居左，1：居中，2：居右，当类型为条码是为条码宽度")]
        public int? Alignment { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        /// <summary>
        /// 垂直对齐 0：居上，1：居中，2：居下,当类型为条码是为条码高度
        /// </summary>
        /// <returns></returns>
        [DisplayName("垂直对齐 0：居上，1：居中，2：居下,当类型为条码是为条码高度")]
        public int? LineAlignment { get; set; }
        /// <summary>
        /// 条码高度
        /// </summary>
        /// <returns></returns>
        [DisplayName("条码高度")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CodeHeight { get; set; }
        /// <summary>
        /// 条码宽度
        /// </summary>
        /// <returns></returns>
        [DisplayName("条码宽度")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CodeWidth { get; set; }
        /// <summary>
        /// 行数
        /// </summary>
        /// <returns></returns>
        [DisplayName("行数")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? RowNum { get; set; }
        /// <summary>
        /// 列数
        /// </summary>
        /// <returns></returns>
        [DisplayName("列数")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? ColNum { get; set; }
        /// <summary>
        /// 行间隔
        /// </summary>
        /// <returns></returns>
        [DisplayName("行间隔")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? RowHeight { get; set; }
        /// <summary>
        /// 列间隔
        /// </summary>
        /// <returns></returns>
        [DisplayName("列间隔")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? ColWidth { get; set; }
        /// <summary>
        /// Rem
        /// </summary>
        /// <returns></returns>
        [DisplayName("Rem")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Rem { get; set; }
        /// <summary>
        /// Enabled
        /// </summary>
        /// <returns></returns>
        [DisplayName("Enabled")]
        public string Enabled { get; set; }
        /// <summary>
        /// CreTm
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreTm")]
        public DateTime? CreTm { get; set; }
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
        public DateTime? MdfTm { get; set; }
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
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = CommonHelper.GetGuid;
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
            this.Id = KeyValue;
            this.MdfTm = DateTime.Now;
            this.MdfCd = ManageProvider.Provider.Current().UserId;
            this.MdfNm = ManageProvider.Provider.Current().UserName;
        }
        #endregion
    }
}