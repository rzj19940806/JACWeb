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
    /// 打印基础信息表
    /// <author>
    ///		<name>she</name>
    ///		<date>2022.04.22 10:42</date>
    /// </author>
    /// </summary>
    [Description("打印基础信息表")]
    [PrimaryKey("Id")]
    public class BBdbR_Print : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string Id { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("编号")]
        public string PrintCd { get; set; }
        /// <summary>
        /// 类型—1：配置单；2：扫码单
        /// </summary>
        /// <returns></returns>
        [DisplayName("类型—1：配置单；2：扫码单")]
        public int? PrintType { get; set; }
        /// <summary>
        /// 左边距
        /// </summary>
        /// <returns></returns>
        [DisplayName("左边距")]
        public int? LeftMargin { get; set; }
        /// <summary>
        /// 右边距
        /// </summary>
        /// <returns></returns>
        [DisplayName("右边距")]
        public int? RightMargin { get; set; }
        /// <summary>
        /// 上边距
        /// </summary>
        /// <returns></returns>
        [DisplayName("上边距")]
        public int? UpMargin { get; set; }
        /// <summary>
        /// 下边距
        /// </summary>
        /// <returns></returns>
        [DisplayName("下边距")]
        public int? DownMargin { get; set; }
        /// <summary>
        /// 边框宽度
        /// </summary>
        /// <returns></returns>
        [DisplayName("边框宽度")]
        public int? Width { get; set; }
        /// <summary>
        /// 边框高度
        /// </summary>
        /// <returns></returns>
        [DisplayName("边框高度")]
        public int? Height { get; set; }
        /// <summary>
        /// 边框粗细
        /// </summary>
        /// <returns></returns>
        [DisplayName("边框粗细")]
        public int? Board { get; set; }
        /// <summary>
        /// 打印方向—1：横向，2：纵向
        /// </summary>
        /// <returns></returns>
        [DisplayName("打印方向—1：横向，2：纵向")]
        public int? Orientation { get; set; }
        /// <summary>
        /// 有效性—0：删除；1：启用；2：停用
        /// </summary>
        /// <returns></returns>
        [DisplayName("有效性—0：删除；1：启用；2：停用")]
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
        /// <summary>
        /// Rem
        /// </summary>
        /// <returns></returns>
        [DisplayName("Rem")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Rem { get; set; }
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