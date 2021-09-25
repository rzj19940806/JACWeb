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
    /// 班制配置试图
    /// <author>
    ///		<name>she</name>
    ///		<date>2021.08.29 16:19</date>
    /// </author>
    /// </summary>
    [Description("班制配置视图")] 
    public class ClassConfigView : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 班制主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("班制主键")]
        public string ClassId { get; set; }
        /// <summary>
        /// 班制编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("班制编号")]
        public string ClassCd { get; set; }
        /// <summary>
        /// 班制名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("班制名称")]
        public string ClassNm { get; set; }
        /// <summary>
        /// 班制类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("班制类型")]
        public string ClassTyp { get; set; }
        /// <summary>
        /// 班制开始时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("班制开始时间")]
        public string ClassShifConfigId { get; set; }
        /// <summary>
        /// 班次编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("班次编号")]
        public string ShiftCd { get; set; }
        /// <summary>
        /// 班次名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("班次名称")]
        public string ShiftNm { get; set; }
        /// <summary>
        /// 班次主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("班次主键")]
        public string ShiftId { get; set; }
        /// <summary>
        /// 时间类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("时间类型")]
        public string RestTm { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("开始时间")]
        public string StrtRestTm { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("结束时间")]
        public string EndRestTm { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string RecId { get; set; }
        /// <summary>
        /// 班组id
        /// </summary>
        /// <returns></returns>
        [DisplayName("班组id")]
        public string TeamId { get; set; }
        /// <summary>
        /// 班组编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("班组编号")]
        public string TeamCd { get; set; }
        /// <summary>
        /// 班组名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("班组名称")]
        public string TeamNm { get; set; }
        /// <summary>
        /// 班组类型
        /// </summary>
        /// <returns></returns>
        [DisplayName("班组类型")]
        public string TeamTyp { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        /// <returns></returns>
        [DisplayName("描述")]
        public string Dsc { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>Enabled
        /// <returns></returns>
        [DisplayName("版本号")]
        public string VersionNumber { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>Enabled
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
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ClassId = CommonHelper.GetGuid;
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